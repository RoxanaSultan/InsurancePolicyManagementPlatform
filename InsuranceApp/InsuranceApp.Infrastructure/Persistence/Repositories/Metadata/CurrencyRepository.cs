using InsuranceApp.Application.Interfaces.Repositories.Metadata;
using InsuranceApp.Domain.Entities.Metadata;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Infrastructure.Persistence.Repositories.Metadata;

public class CurrencyRepository(InsuranceDbContext context) : GenericRepository<Currency>(context), ICurrencyRepository
{
    public async Task<bool> IsCodeUnique(
        string code, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Currencies
            .AsNoTracking()
            .AnyAsync(c => c.Code == code, cancellationToken);
    }
}