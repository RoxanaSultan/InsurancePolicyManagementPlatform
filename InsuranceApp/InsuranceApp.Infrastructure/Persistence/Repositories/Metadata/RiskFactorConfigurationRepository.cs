using InsuranceApp.Application.Interfaces.Repositories.Metadata;
using InsuranceApp.Domain.Entities.Metadata;
using InsuranceApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Infrastructure.Persistence.Repositories.Metadata;

public class RiskFactorConfigurationRepository(InsuranceDbContext context) : GenericRepository<RiskFactorConfiguration>(context), IRiskFactorConfigurationRepository
{
    public async Task<bool> ActiveUniqueLevelAndReferenceIdExistsAsync(
        RiskFactorLevel level, 
        int referenceId, 
        int? excludeId, 
        CancellationToken cancellationToken)
    {
        return await _context.RiskFactorConfigurations
            .AnyAsync(x =>
                x.IsActive &&
                x.Level == level &&
                x.ReferenceId == referenceId &&
                (!excludeId.HasValue || x.Id != excludeId.Value),
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<RiskFactorConfiguration>> GetApplicableAsync(
        int countryId,
        int countyId,
        int cityId,
        int buildingTypeId,
        CancellationToken cancellationToken)
    {
        return await _context.RiskFactorConfigurations
            .AsNoTracking()
            .Where(r => r.IsActive &&
                (
                    (r.Level == RiskFactorLevel.Country && r.ReferenceId == countryId) ||
                    (r.Level == RiskFactorLevel.County && r.ReferenceId == countyId) ||
                    (r.Level == RiskFactorLevel.City && r.ReferenceId == cityId) ||
                    (r.Level == RiskFactorLevel.BuildingType && r.ReferenceId == buildingTypeId)
                ))
            .ToListAsync(cancellationToken);
    }
}