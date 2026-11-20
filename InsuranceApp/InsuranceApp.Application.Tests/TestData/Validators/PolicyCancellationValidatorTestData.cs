using InsuranceApp.Application.DTOs.PolicyCancellation;

namespace InsuranceApp.Application.Tests.TestData;

public static class PolicyCancellationValidatorTestData
{
    public static PolicyCancellationCreateRequest ValidRequest(
        DateTime? cancellationDate = null,
        string cancellationReason = "Customer request")
        => new()
        {
            CancellationDate = cancellationDate ?? new DateTime(2024, 6, 1),
            CancellationReason = cancellationReason
        };
}
