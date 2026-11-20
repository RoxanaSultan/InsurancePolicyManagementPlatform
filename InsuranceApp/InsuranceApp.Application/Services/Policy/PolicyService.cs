using AutoMapper;
using InsuranceApp.Application.Common.Messages;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.Policy;
using InsuranceApp.Application.DTOs.PolicyCancellation;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Services.Authentication;
using InsuranceApp.Application.Interfaces.Services.Policy;
using InsuranceApp.Domain.Entities.Geography;
using InsuranceApp.Domain.Enums;
using Microsoft.Extensions.Logging;
using BrokerEntity = InsuranceApp.Domain.Entities.User.Broker;
using BuildingEntity = InsuranceApp.Domain.Entities.Building.Building;
using PolicyEntity = InsuranceApp.Domain.Entities.Policy.Policy;

namespace InsuranceApp.Application.Services.Policy;

public class PolicyService(
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    ICurrentUserService currentUserService, 
    IPolicyCancellationService policyCancellationService,
    ILogger<PolicyService> logger
    ) : IPolicyService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IPolicyCancellationService _policyCancellationService = policyCancellationService;
    private readonly ILogger<PolicyService> _logger = logger;

    public async Task<PolicyResponse> ActivateAsync(
        int policyId, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Activating policy {PolicyId}.", policyId);

        var existingPolicy = await _unitOfWork.Policies.GetByIdAsync(policyId, cancellationToken)
            ?? throw new NotFoundException(PolicyMessages.NotFoundWithID(policyId));

        existingPolicy.Activate();

        await _unitOfWork.Policies.UpdateAsync(existingPolicy, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PolicyResponse>(existingPolicy);
    }

    public async Task<PolicyCancellationResponse> CancelAsync(
        int policyId, PolicyCancellationCreateRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling policy {PolicyId}.", policyId);

        var existingPolicy = await _unitOfWork.Policies.GetByIdAsync(policyId, cancellationToken)
            ?? throw new NotFoundException(PolicyMessages.NotFoundWithId(policyId));

        if (existingPolicy.Status != PolicyStatus.Active)
        {
            throw new InvalidOperationException(PolicyMessages.OnlyActivePoliciesCanBeCancelled(existingPolicy.Status.ToString()));
        }

        existingPolicy.Cancel();
        await _unitOfWork.Policies.UpdateAsync(existingPolicy, cancellationToken);

        var policyCancellation = await _policyCancellationService.CreateAsync(policyId, request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return policyCancellation;
    }

    public async Task<PolicyResponse> CreateAsync(
        PolicyCreateRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating policy.");

        var policy = _mapper.Map<PolicyEntity>(request);

        policy.CreationDate = DateTime.UtcNow;
        policy.Draft();

        var currentUserId = _currentUserService.GetUserId();

        var broker = await _unitOfWork.Brokers.GetByUserIdAsync(currentUserId!, cancellationToken)
            ?? throw new NotFoundException(PolicyMessages.BrokerForUserNotFound(currentUserId));

        policy.BrokerId = broker.Id;

        var building = await _unitOfWork.Buildings.GetByIdAsync(request.BuildingId, cancellationToken)
            ?? throw new NotFoundException(PolicyMessages.BuildingNotFound(request.BuildingId));

        var city = await _unitOfWork.Cities.GetByIdAsync(building.CityId, cancellationToken)
            ?? throw new NotFoundException(PolicyMessages.CityNotFound(building.CityId));

        var county = await _unitOfWork.Counties.GetByIdAsync(city.CountyId, cancellationToken)
            ?? throw new NotFoundException(PolicyMessages.CountyNotFound(city.CountyId));

        var country = await _unitOfWork.Countries.GetByIdAsync(county.CountryId, cancellationToken)
            ?? throw new NotFoundException(PolicyMessages.CountryNotFound(county.CountryId));

        var currency = await _unitOfWork.Currencies.GetByIdAsync(request.CurrencyId, cancellationToken)
            ?? throw new NotFoundException(CurrencyMessages.NotFound(request.CurrencyId));

        policy.CalculatedPremiumAmount =
            await CalculatePremiumAmount(policy, broker, building, city, county, country) * currency.ExchangeRateToBase;
                   
        await _unitOfWork.Policies.AddAsync(policy, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Policy created with ID {PolicyId}.", policy.Id);

        return _mapper.Map<PolicyResponse>(policy);
    }

    public async Task<PolicyDetailsResponse> GetByIdAsync(
        int policyId, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting policy {PolicyId}.", policyId);

        var policy = await _unitOfWork.Policies.GetByIdAsync(policyId, cancellationToken)
            ?? throw new NotFoundException(PolicyMessages.NotFoundWithID(policyId));
        
        return _mapper.Map<PolicyDetailsResponse>(policy);
    }

    public async Task<PageResponse<PolicyResponse>> GetPoliciesAsync(
        PolicyFilterRequest filterRequest, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting policies.");

        var policies = await _unitOfWork.Policies.GetPoliciesAsync(filterRequest, pageRequest, cancellationToken);

        var mappedItems = _mapper.Map<IReadOnlyList<PolicyResponse>>(policies.Items);

        return new PageResponse<PolicyResponse>
        {
            Items = mappedItems,
            Page = policies.Page,
            PageSize = policies.PageSize,
            TotalItems = policies.TotalItems,
            TotalPages = policies.TotalPages
        };
    }

    private async Task<decimal> CalculatePremiumAmount(
        PolicyEntity policy, 
        BrokerEntity broker, 
        BuildingEntity building, 
        City city, 
        County county, 
        Country country)
    {
        var now = DateTime.UtcNow;
        decimal percentages = 0m;

        if (broker.CommissionPercentage.HasValue)
        {
            percentages += broker.CommissionPercentage.Value;
        }

        var activeFees = await _unitOfWork.FeeConfigurations.GetActiveAsync(now, CancellationToken.None);
        
        percentages += activeFees.Sum(f => f.Percentage);
        
        var buildingTypeId = (int)building.BuildingType;

        var riskFactors = await _unitOfWork.RiskFactorConfigurations.GetApplicableAsync(
            countryId: country.Id,
            countyId: county.Id,
            cityId: city.Id,
            buildingTypeId: buildingTypeId,
            cancellationToken: CancellationToken.None);

        percentages += riskFactors.Sum(r => r.AdjustmentPercentage);
        
        return policy.BasePremiumAmount * (1m + percentages / 100);
    }
}
