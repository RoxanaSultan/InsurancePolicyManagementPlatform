using InsuranceApp.Domain.Entities.Metadata;

namespace InsuranceApp.Application.Interfaces.Repositories.Metadata;

public interface ICurrencyRepository : IRepository<Currency>
{
    Task<bool> IsCodeUnique(
        string code, 
        CancellationToken cancellationToken = default);
}
