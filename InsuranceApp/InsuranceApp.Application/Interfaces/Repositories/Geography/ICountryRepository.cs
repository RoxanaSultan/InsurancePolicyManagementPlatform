using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Domain.Entities.Geography;

namespace InsuranceApp.Application.Interfaces.Repositories.Geography;

public interface ICountryRepository : IRepository<Country>
{
    Task<bool> ExistsAsync(
        int countryId, 
        CancellationToken cancellationToken = default);
}