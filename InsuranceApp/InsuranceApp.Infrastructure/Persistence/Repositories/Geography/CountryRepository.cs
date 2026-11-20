using InsuranceApp.Application.Interfaces.Repositories.Geography;
using InsuranceApp.Domain.Entities.Geography;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Infrastructure.Persistence.Repositories.Geography;

public class CountryRepository(InsuranceDbContext context) : GenericRepository<Country>(context), ICountryRepository
{
    public async Task<bool> ExistsAsync(
        int countryId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Countries
            .AsNoTracking()
            .AnyAsync(c => c.Id == countryId, cancellationToken);
    }
}