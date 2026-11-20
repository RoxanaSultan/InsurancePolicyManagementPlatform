using InsuranceApp.Application.DTOs.Clients;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Domain.Entities.Client;
using ClientEntity = InsuranceApp.Domain.Entities.Client.Client;

namespace InsuranceApp.Application.Interfaces.Repositories.Client;

public interface IClientRepository : IRepository<ClientEntity>
{
    Task<IReadOnlyList<ClientEntity>> SearchAsync(
        ClientSearchRequest request, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken = default);

    Task<bool> IsIdentificationNumberUniqueAsync(
        string identificationNumber, 
        int? excludeClientId = null, 
        CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(
        string email, 
        int? excludeClientId = null, 
        CancellationToken cancellationToken = default);
    Task<bool> IsPhoneNumberUniqueAsync(
        string phoneNumber, 
        int? excludeClientId = null, 
        CancellationToken cancellationToken = default);

    Task AddIdentificationNumberAuditAsync(
        ClientIdentificationNumberAudit audit, 
        CancellationToken cancellationToken = default);
    Task<IEnumerable<ClientIdentificationNumberAudit>> GetIdentificationNumberAuditAsync(
        int clientId, 
        CancellationToken cancellationToken = default);
}