using InsuranceApp.Application.DTOs.Policy;

namespace InsuranceApp.Application.Tests.TestData;

public static class PolicyValidatorTestData
{
    public static PolicyBase ValidPolicyBase(
        DateTime? startDate = null,
        DateTime? endDate = null,
        decimal basePremiumAmount = 100m)
        => new()
        {
            StartDate = startDate ?? new DateTime(2024, 1, 1),
            EndDate = endDate ?? new DateTime(2025, 1, 1),
            BasePremiumAmount = basePremiumAmount
        };

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
}
