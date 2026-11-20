using InsuranceApp.Application.DTOs.Clients;
using InsuranceApp.Application.DTOs.Paging;

namespace InsuranceApp.Application.Interfaces.Services.Client;

public interface IClientService
{
    Task<IEnumerable<ClientResponse>> SearchAsync(
        ClientSearchRequest request, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken);
    Task<ClientDetailsResponse?> GetByIdAsync(
        int clientId, 
        CancellationToken cancellationToken);
    Task<ClientResponse> CreateAsync(
        ClientCreateRequest request, 
        CancellationToken cancellationToken);
    Task<ClientResponse> UpdateAsync(
        string userId, 
        int clientId, 
        ClientUpdateRequest request, 
        CancellationToken cancellationToken);
    Task<IEnumerable<ClientIdentificationNumberAuditResponse>> GetIdentificationNumberAuditAsync(
        int clientId, 
        CancellationToken cancellationToken);
}