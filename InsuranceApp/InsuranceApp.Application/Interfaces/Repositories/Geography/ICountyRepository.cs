using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Domain.Entities.Geography;

namespace InsuranceApp.Application.Interfaces.Repositories.Geography;

public interface ICountyRepository : IRepository<County>
{
    Task<PageResponse<County>> GetByCountryIdAsync(
        int countryId, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(
        int countyId, 
        CancellationToken cancellationToken = default);
}