using AutoMapper;
using InsuranceApp.Application.Common.Messages;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.RiskFactorConfiguration;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Services.Metadata;
using InsuranceApp.Domain.Entities.Metadata;
using InsuranceApp.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace InsuranceApp.Application.Services.Metadata;

public class RiskFactorConfigurationService(
    IUnitOfWork unitOfWork, 
    IMapper mapper,
    ILogger<RiskFactorConfigurationService> logger
    ) : IRiskFactorConfigurationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<RiskFactorConfigurationService> _logger = logger;

    public async Task<PageResponse<RiskFactorConfigurationResponse>> GetAllAsync(
        PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting risk factor configurations.");

        var riskFactorConfigurations = await _unitOfWork.RiskFactorConfigurations.GetAllAsync(pageRequest, cancellationToken);

        var mappedItems = _mapper.Map<IReadOnlyList<RiskFactorConfigurationResponse>>(riskFactorConfigurations.Items);

        return new PageResponse<RiskFactorConfigurationResponse>
        {
            Items = mappedItems,
            Page = riskFactorConfigurations.Page,
            PageSize = riskFactorConfigurations.PageSize,
            TotalItems = riskFactorConfigurations.TotalItems,
            TotalPages = riskFactorConfigurations.TotalPages
        };
    }

    public async Task<RiskFactorConfigurationResponse> CreateAsync(
        RiskFactorConfigurationCreateRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating risk factor configuration.");

        await ValidateReferenceAsync(request.Level, request.ReferenceId, cancellationToken);

        if (request.IsActive)
        {
            var exists = await _unitOfWork.RiskFactorConfigurations.ActiveUniqueLevelAndReferenceIdExistsAsync(
                request.Level, request.ReferenceId, excludeId: null, cancellationToken);

            if (exists)
            {
                throw new ConflictException(RiskFactorConfigurationMessages.ActiveUniqueLevelAndReferenceExists(request.Level, request.ReferenceId));
            }
        }

        var riskFactorConfiguration = _mapper.Map<RiskFactorConfiguration>(request);

        await _unitOfWork.RiskFactorConfigurations.AddAsync(riskFactorConfiguration, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Risk factor configuration created with ID {RiskFactorConfigurationId}.", riskFactorConfiguration.Id);

        return _mapper.Map<RiskFactorConfigurationResponse>(riskFactorConfiguration);
    }

    public async Task<RiskFactorConfigurationResponse> UpdateAsync(
        int riskFactorConfigurationId, 
        RiskFactorConfigurationUpdateRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating risk factor configuration {RiskFactorConfigurationId}.", riskFactorConfigurationId);

        var existingRiskFactorConfiguration = await _unitOfWork.RiskFactorConfigurations.GetByIdAsync(riskFactorConfigurationId, cancellationToken)
            ?? throw new NotFoundException(RiskFactorConfigurationMessages.NotFound(riskFactorConfigurationId));

        await ValidateReferenceAsync(request.Level, request.ReferenceId, cancellationToken);

        if (request.IsActive)
        {
            var exists = await _unitOfWork.RiskFactorConfigurations.ActiveUniqueLevelAndReferenceIdExistsAsync(
                request.Level, request.ReferenceId, excludeId: existingRiskFactorConfiguration.Id, cancellationToken);

            if (exists)
            {
                throw new ConflictException(RiskFactorConfigurationMessages.ActiveUniqueLevelAndReferenceExists(request.Level, request.ReferenceId));
            }
        }

        _mapper.Map(request, existingRiskFactorConfiguration);

        await _unitOfWork.RiskFactorConfigurations.UpdateAsync(existingRiskFactorConfiguration, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RiskFactorConfigurationResponse>(existingRiskFactorConfiguration);
    }

    private async Task ValidateReferenceAsync(
        RiskFactorLevel level, 
        int? referenceId, 
        CancellationToken cancellationToken)
    {
        switch (level)
        {
            case RiskFactorLevel.Country:
                if (!await _unitOfWork.Countries.ExistsAsync(referenceId!.Value, cancellationToken))
                {
                    throw new NotFoundException(RiskFactorConfigurationMessages.CountryNotFound(referenceId.Value));
                }
                break;

            case RiskFactorLevel.County:
                if (!await _unitOfWork.Counties.ExistsAsync(referenceId!.Value, cancellationToken))
                {
                    throw new NotFoundException(RiskFactorConfigurationMessages.CountyNotFound(referenceId.Value));
                }
                break;

            case RiskFactorLevel.City:
                if (!await _unitOfWork.Cities.ExistsAsync(referenceId!.Value, cancellationToken))
                {
                    throw new NotFoundException(RiskFactorConfigurationMessages.CityNotFound(referenceId.Value));
                }
                break;

            case RiskFactorLevel.BuildingType:
                if (!Enum.IsDefined(typeof(BuildingType), referenceId!.Value))
                {
                    throw new NotFoundException(RiskFactorConfigurationMessages.BuildingTypeNotFound(referenceId.Value));
                }
                break;

            default:
                throw new BadRequestException(RiskFactorConfigurationMessages.UnsupportedRiskFactorLevel);
        }
    }
}
