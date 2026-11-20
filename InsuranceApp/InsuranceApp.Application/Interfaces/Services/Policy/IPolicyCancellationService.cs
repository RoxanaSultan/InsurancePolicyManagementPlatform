using InsuranceApp.Application.DTOs.PolicyCancellation;

namespace InsuranceApp.Application.Interfaces.Services.Policy;

public interface IPolicyCancellationService
{
    Task<PolicyCancellationResponse> CreateAsync(
        int policyId, 
        PolicyCancellationCreateRequest request, 
        CancellationToken cancellationToken);
}