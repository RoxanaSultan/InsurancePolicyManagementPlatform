using InsuranceApp.Domain.Entities.Metadata;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Interfaces.Repositories.Metadata;

public interface IRiskFactorConfigurationRepository : IRepository<RiskFactorConfiguration>
{
    Task<bool> ActiveUniqueLevelAndReferenceIdExistsAsync(
        RiskFactorLevel level, 
        int referenceId, 
        int? excludeId, 
        CancellationToken cancellationToken);
    Task<IReadOnlyCollection<RiskFactorConfiguration>> GetApplicableAsync(
        int countryId,
        int countyId,
        int cityId,
        int buildingTypeId,
        CancellationToken cancellationToken);
}