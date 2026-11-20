using InsuranceApp.Domain.Entities.Metadata;

namespace InsuranceApp.Application.Interfaces.Repositories.Metadata;

public interface IFeeConfigurationRepository : IRepository<FeeConfiguration>
{
    Task<IReadOnlyCollection<FeeConfiguration>> GetActiveAsync(
        DateTime at, 
        CancellationToken cancellationToken);
}