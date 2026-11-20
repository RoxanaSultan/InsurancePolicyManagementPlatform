using InsuranceApp.Application.DTOs.PolicyCancellation;

namespace InsuranceApp.Application.Tests.TestData.Services;

public static class PolicyCancellationTestData
{
    public static PolicyCancellationCreateRequest ValidCreateRequest(
        DateTime? cancellationDate = null,
        string cancellationReason = "Client request")
        => new()
        {
            CancellationDate = cancellationDate ?? new DateTime(2024, 6, 1),
            CancellationReason = cancellationReason
        };
}