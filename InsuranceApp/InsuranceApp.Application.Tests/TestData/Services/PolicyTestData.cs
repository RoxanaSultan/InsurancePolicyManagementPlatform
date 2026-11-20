using InsuranceApp.Application.DTOs.Policy;
using InsuranceApp.Application.DTOs.PolicyCancellation;

namespace InsuranceApp.Application.Tests.TestData.Services;

public static class PolicyTestData
{
    public static PolicyCreateRequest ValidCreateRequest(
        int buildingId = 1,
        int currencyId = 2,
        DateTime? startDate = null,
        DateTime? endDate = null,
        decimal basePremiumAmount = 100m)
        => new()
        {
            BuildingId = buildingId,
            CurrencyId = currencyId,
            StartDate = startDate ?? new DateTime(2024, 1, 1),
            EndDate = endDate ?? new DateTime(2025, 1, 1),
            BasePremiumAmount = basePremiumAmount
        };

    public static PolicyCancellationCreateRequest ValidCancellationRequest(
        DateTime? cancellationDate = null,
        string cancellationReason = "No longer needed")
        => new()
        {
            CancellationDate = cancellationDate ?? new DateTime(2024, 6, 1),
            CancellationReason = cancellationReason
        };
}