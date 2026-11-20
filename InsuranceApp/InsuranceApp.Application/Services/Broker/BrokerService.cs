using AutoMapper;
using InsuranceApp.Application.Common.Messages;
using InsuranceApp.Application.DTOs.Broker;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Services.Broker;
using InsuranceApp.Domain.Enums;
using Microsoft.Extensions.Logging;

using BrokerEntity = InsuranceApp.Domain.Entities.User.Broker;

namespace InsuranceApp.Application.Services.Broker;

public class BrokerService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BrokerService> logger
    ) : IBrokerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<BrokerService> _logger = logger;

    public async Task<BrokerResponse> ActivateAsync(
        int brokerId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Activating broker {BrokerId}.", brokerId);

        var existingBroker = await _unitOfWork.Brokers.GetByIdAsync(brokerId, cancellationToken)
            ?? throw new NotFoundException(BrokerMessages.NotFound(brokerId));

        existingBroker.Status = BrokerStatus.Active;

        await _unitOfWork.Brokers.UpdateAsync(existingBroker, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BrokerResponse>(existingBroker);
    }

    public async Task<BrokerResponse> CreateAsync(
        BrokerCreateRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating broker.");

        await ValidateBrokerUniquenessAsync(
            request.Email,
            request.Phone,
            null,
            cancellationToken);

        var broker = _mapper.Map<BrokerEntity>(request);
        broker.Code = await GenerateUniqueBrokerCode(cancellationToken);

        await _unitOfWork.Brokers.AddAsync(broker, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Broker created with ID {BrokerId} and Code {BrokerCode}.", broker.Id, broker.Code);

        return _mapper.Map<BrokerResponse>(broker);
    }

    public async Task<BrokerResponse> DeactivateAsync(
        int brokerId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deactivating broker {BrokerId}.", brokerId);

        var existingBroker = await _unitOfWork.Brokers.GetByIdAsync(brokerId, cancellationToken)
            ?? throw new NotFoundException(BrokerMessages.NotFound(brokerId));

        existingBroker.Status = BrokerStatus.Inactive;

        await _unitOfWork.Brokers.UpdateAsync(existingBroker, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BrokerResponse>(existingBroker);
    }

    public async Task<PageResponse<BrokerResponse>> GetAllAsync(
        PageRequest pageRequest,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting brokers.");

        var brokers = await _unitOfWork.Brokers.GetAllAsync(pageRequest, cancellationToken);

        var mappedItems = _mapper.Map<IReadOnlyList<BrokerResponse>>(brokers.Items);

        return new PageResponse<BrokerResponse>
        {
            Items = mappedItems,
            Page = brokers.Page,
            PageSize = brokers.PageSize,
            TotalItems = brokers.TotalItems,
            TotalPages = brokers.TotalPages
        };
    }

    public async Task<BrokerDetailsResponse> GetByIdAsync(
        int brokerId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting broker {BrokerId}.", brokerId);

        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId, cancellationToken)
            ?? throw new NotFoundException(BrokerMessages.NotFound(brokerId));

        return _mapper.Map<BrokerDetailsResponse>(broker);
    }

    public async Task<BrokerResponse> UpdateAsync(
        int brokerId,
        BrokerUpdateRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating broker {BrokerId}.", brokerId);

        var existingBroker = await _unitOfWork.Brokers.GetByIdAsync(brokerId, cancellationToken)
            ?? throw new NotFoundException(BrokerMessages.NotFound(brokerId));

        await ValidateBrokerUniquenessAsync(
            request.Email,
            request.Phone,
            brokerId,
            cancellationToken);

        _mapper.Map(request, existingBroker);

        await _unitOfWork.Brokers.UpdateAsync(existingBroker, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BrokerResponse>(existingBroker);
    }

    private async Task ValidateBrokerUniquenessAsync(
        string email,
        string phoneNumber,
        int? brokerId,
        CancellationToken cancellationToken)
    {
        bool isEmailUnique = brokerId.HasValue
            ? await _unitOfWork.Brokers.IsEmailUniqueAsync(email, brokerId.Value, cancellationToken)
            : await _unitOfWork.Brokers.IsEmailUniqueAsync(email, cancellationToken: cancellationToken);

        if (!isEmailUnique)
        {
            throw new ConflictException(BrokerMessages.EmailAlreadyExists(email));
        }

        bool isPhoneNumberUnique = brokerId.HasValue
            ? await _unitOfWork.Brokers.IsPhoneNumberUniqueAsync(phoneNumber, brokerId.Value, cancellationToken)
            : await _unitOfWork.Brokers.IsPhoneNumberUniqueAsync(phoneNumber, cancellationToken: cancellationToken);

        if (!isPhoneNumberUnique)
        {
            throw new ConflictException(BrokerMessages.PhoneNumberAlreadyExists(phoneNumber));
        }
    }

    private async Task<string> GenerateUniqueBrokerCode(CancellationToken cancellationToken)
    {
        string code;
        do
        {
            code = $"BRK-{Guid.NewGuid().ToString("N")[..3].ToUpper()}";
        }
        while (!await _unitOfWork.Brokers.IsCodeUniqueAsync(code, cancellationToken: cancellationToken));

        return code;
    }
}
