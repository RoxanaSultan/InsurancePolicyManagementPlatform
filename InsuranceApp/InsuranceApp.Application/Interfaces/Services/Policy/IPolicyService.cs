using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.Policy;
using InsuranceApp.Application.DTOs.PolicyCancellation;

namespace InsuranceApp.Application.Interfaces.Services.Policy;

public interface IPolicyService
{
    Task<PageResponse<PolicyResponse>> GetPoliciesAsync(
        PolicyFilterRequest filterRequest, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken);
    Task<PolicyDetailsResponse> GetByIdAsync(
        int policyId, 
        CancellationToken cancellationToken);
    Task<PolicyResponse> CreateAsync(
        PolicyCreateRequest request, 
        CancellationToken cancellationToken);
    Task<PolicyResponse> ActivateAsync(
        int policyId, 
        CancellationToken cancellationToken);
    Task<PolicyCancellationResponse> CancelAsync(
        int policyId, 
        PolicyCancellationCreateRequest request, 
        CancellationToken cancellationToken);
}