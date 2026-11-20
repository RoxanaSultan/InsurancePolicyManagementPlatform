using InsuranceApp.Application.DTOs.Paging;
using BuildingEntity = InsuranceApp.Domain.Entities.Building.Building;

namespace InsuranceApp.Application.Interfaces.Repositories.Building;

public interface IBuildingRepository : IRepository<BuildingEntity>
{
    Task<PageResponse<BuildingEntity>> GetByClientIdAsync(
        int clientId, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken = default);
}