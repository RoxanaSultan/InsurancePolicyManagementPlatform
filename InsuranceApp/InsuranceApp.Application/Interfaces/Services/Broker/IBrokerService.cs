using InsuranceApp.Application.DTOs.Broker;
using InsuranceApp.Application.DTOs.Paging;

namespace InsuranceApp.Application.Interfaces.Services.Broker;

public interface IBrokerService
{
    Task<PageResponse<BrokerResponse>> GetAllAsync(
        PageRequest pageRequest, 
        CancellationToken cancellationToken);
    Task<BrokerDetailsResponse> GetByIdAsync(
        int brokerId, 
        CancellationToken cancellationToken);
    Task<BrokerResponse> CreateAsync(
        BrokerCreateRequest request, 
        CancellationToken cancellationToken);
    Task<BrokerResponse> UpdateAsync(
        int brokerId, 
        BrokerUpdateRequest request, 
        CancellationToken cancellationToken);
    Task<BrokerResponse> ActivateAsync(
        int brokerId, 
        CancellationToken cancellationToken);
    Task<BrokerResponse> DeactivateAsync(
        int brokerId, 
        CancellationToken cancellationToken);
}