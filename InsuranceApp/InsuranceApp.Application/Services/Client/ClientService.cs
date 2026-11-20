using AutoMapper;
using InsuranceApp.Application.Common.Messages;
using InsuranceApp.Application.DTOs.Clients;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Services.Client;
using InsuranceApp.Domain.Entities.Client;
using Microsoft.Extensions.Logging;
using ClientEntity = InsuranceApp.Domain.Entities.Client.Client;

namespace InsuranceApp.Application.Services.Client;

public class ClientService(
    IUnitOfWork unitOfWork, 
    IMapper mapper,
    ILogger<ClientService> logger
    ) : IClientService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<ClientService> _logger = logger;

    public async Task<IEnumerable<ClientResponse>> SearchAsync(
        ClientSearchRequest request, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching clients.");

        ArgumentNullException.ThrowIfNull(request);

        var clients = await _unitOfWork.Clients.SearchAsync(request, pageRequest, cancellationToken);

        return _mapper.Map<IEnumerable<ClientResponse>>(clients);
    }

    public async Task<ClientDetailsResponse?> GetByIdAsync(
        int clientId, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting client {ClientId}.", clientId);

        var client = await _unitOfWork.Clients.GetByIdAsync(clientId, cancellationToken)
            ?? throw new NotFoundException(ClientMessages.NotFound(clientId));

        return _mapper.Map<ClientDetailsResponse>(client);
    }

    public async Task<ClientResponse> CreateAsync(
        ClientCreateRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating client.");

        await ValidateClientUniquenessAsync(
            request.IdentificationNumber,
            request.ContactInfo.Email,
            request.ContactInfo.PhoneNumber,
            null,
            cancellationToken);

        var client = _mapper.Map<ClientEntity>(request);

        await _unitOfWork.Clients.AddAsync(client, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Client created with ID {ClientId}.", client.Id);

        return _mapper.Map<ClientResponse>(client);
    }

    public async Task<ClientResponse> UpdateAsync(
        string userId, 
        int clientId, 
        ClientUpdateRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating client {ClientId}.", clientId);

        var existingClient = await _unitOfWork.Clients.GetByIdAsync(clientId, cancellationToken)
            ?? throw new NotFoundException(ClientMessages.NotFound(clientId));

        await ValidateClientUniquenessAsync(
            request.IdentificationNumber,
            request.ContactInfo.Email,
            request.ContactInfo.PhoneNumber,
            clientId,
            cancellationToken);

        var oldIdentificationNumber = existingClient.IdentificationNumber;

        var clientType = existingClient.ClientType;
        _mapper.Map(request, existingClient);
        existingClient.ClientType = clientType;

        var newIdentificationNumber = existingClient.IdentificationNumber;
        if (!string.Equals(oldIdentificationNumber, newIdentificationNumber, StringComparison.Ordinal))
        {
            var audit = new ClientIdentificationNumberAudit
            {
                UserId = userId,
                ClientId = existingClient.Id,
                OldIdentificationNumber = oldIdentificationNumber,
                NewIdentificationNumber = newIdentificationNumber,
                ChangedAt = DateTime.UtcNow
            };

            await _unitOfWork.Clients.AddIdentificationNumberAuditAsync(audit, cancellationToken);
        }

        await _unitOfWork.Clients.UpdateAsync(existingClient, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ClientResponse>(existingClient);
    }

    public async Task<IEnumerable<ClientIdentificationNumberAuditResponse>> GetIdentificationNumberAuditAsync(
        int clientId, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting identification number audit for Client {ClientId}.", clientId);

        _ = await _unitOfWork.Clients.GetByIdAsync(clientId, cancellationToken)
            ?? throw new NotFoundException(ClientMessages.NotFound(clientId));

        var audits = await _unitOfWork.Clients.GetIdentificationNumberAuditAsync(clientId, cancellationToken);

        return _mapper.Map<IEnumerable<ClientIdentificationNumberAuditResponse>>(audits);
    }

    private async Task ValidateClientUniquenessAsync(
        string identificationNumber,
        string email,
        string phoneNumber,
        int? clientId,
        CancellationToken cancellationToken)
    {
        bool isIdentificationNumberUnique = clientId.HasValue
            ? await _unitOfWork.Clients.IsIdentificationNumberUniqueAsync(identificationNumber, clientId.Value, cancellationToken)
            : await _unitOfWork.Clients.IsIdentificationNumberUniqueAsync(identificationNumber, cancellationToken: cancellationToken);

        if (!isIdentificationNumberUnique)
        {
            throw new ConflictException(ClientMessages.IdentificationNumberAlreadyExists(identificationNumber));
        }

        bool isEmailUnique = clientId.HasValue
            ? await _unitOfWork.Clients.IsEmailUniqueAsync(email, clientId.Value, cancellationToken)
            : await _unitOfWork.Clients.IsEmailUniqueAsync(email, cancellationToken: cancellationToken);

        if (!isEmailUnique)
        {
            throw new ConflictException(ClientMessages.EmailAlreadyExists(email));
        }

        bool isPhoneNumberUnique = clientId.HasValue
            ? await _unitOfWork.Clients.IsPhoneNumberUniqueAsync(phoneNumber, clientId.Value, cancellationToken)
            : await _unitOfWork.Clients.IsPhoneNumberUniqueAsync(phoneNumber, cancellationToken: cancellationToken);

        if (!isPhoneNumberUnique)
        {
            throw new ConflictException(ClientMessages.PhoneNumberAlreadyExists(phoneNumber));
        }
    }
}
