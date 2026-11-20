using InsuranceApp.Application.Interfaces.Repositories.Metadata;
using InsuranceApp.Domain.Entities.Metadata;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Infrastructure.Persistence.Repositories.Metadata;

public class FeeConfigurationRepository(InsuranceDbContext context) : GenericRepository<FeeConfiguration>(context), IFeeConfigurationRepository
{
    public async Task<IReadOnlyCollection<FeeConfiguration>> GetActiveAsync(
        DateTime at, 
        CancellationToken cancellationToken)
    {
        return await _context.FeeConfigurations
            .AsNoTracking()
            .Where(f => f.IsActive && f.EffectiveFrom <= at && at <= f.EffectiveTo)
            .ToListAsync(cancellationToken);
    }
}