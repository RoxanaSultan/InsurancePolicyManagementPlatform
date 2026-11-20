using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Domain.Entities.Geography;

namespace InsuranceApp.Application.Interfaces.Repositories.Geography;

public interface ICityRepository : IRepository<City>
{
    Task<PageResponse<City>> GetByCountyIdAsync(
        int countyId, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(
        int cityId, 
        CancellationToken cancellationToken = default);
}