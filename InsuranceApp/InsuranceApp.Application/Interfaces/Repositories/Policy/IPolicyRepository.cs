using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.Policy;
using InsuranceApp.Application.DTOs.Report;
using PolicyEntity = InsuranceApp.Domain.Entities.Policy.Policy;

namespace InsuranceApp.Application.Interfaces.Repositories.Policy;

public interface IPolicyRepository : IRepository<PolicyEntity>
{
    Task<bool> ExistsActivePolicyForCurrencyAsync(
        int currencyId, 
        DateTime nowUtc, 
        CancellationToken cancellationToken = default);
    Task<PageResponse<PolicyEntity>> GetPoliciesAsync(
        PolicyFilterRequest filter, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken = default);

    Task<PageResponse<ReportByCountryResponse>> GetPoliciesByCountryAsync(
        ReportFilterRequest request, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken);
    Task<PageResponse<ReportByCountyResponse>> GetPoliciesByCountyAsync(
        ReportFilterRequest request, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken = default);
    Task<PageResponse<ReportByCityResponse>> GetPoliciesByCityAsync(
        ReportFilterRequest request, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken = default);
    Task<PageResponse<ReportByBrokerResponse>> GetPoliciesByBrokerAsync(
        ReportFilterRequest request, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PolicyEntity>> GetPoliciesToExpireAsync(
        DateTime utcNow, 
        CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(
        IEnumerable<PolicyEntity> policies, 
        CancellationToken cancellationToken = default);

}