using InsuranceApp.Application.DTOs.Buildings;
using InsuranceApp.Application.DTOs.Paging;

namespace InsuranceApp.Application.Interfaces.Services.Building;

public interface IBuildingService
{
    Task<PageResponse<BuildingResponse>> GetByClientIdAsync(
        int clientId, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken);
    Task<BuildingDetailsResponse> GetByIdAsync(
        int buildingId, 
        CancellationToken cancellationToken);
    Task<BuildingResponse> CreateAsync(
        int clientId, 
        BuildingCreateRequest request, 
        CancellationToken cancellationToken);
    Task<BuildingResponse> UpdateAsync(
        int buildingId, 
        BuildingUpdateRequest request, 
        CancellationToken cancellationToken);
}
