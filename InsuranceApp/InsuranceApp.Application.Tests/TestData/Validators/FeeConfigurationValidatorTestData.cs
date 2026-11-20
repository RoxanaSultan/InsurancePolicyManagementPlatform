using InsuranceApp.Application.DTOs.FeeConfiguration;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Tests.TestData;

public static class FeeConfigurationValidatorTestData
{
    public static FeeConfigurationBase ValidFeeBase(
        string name = "Admin Fee",
        FeeType type = FeeType.AdminFee,
        decimal percentage = 5m,
        DateTime? effectiveFrom = null,
        DateTime? effectiveTo = null)
        => new()
        {
            Name = name,
            Type = type,
            Percentage = percentage,
            EffectiveFrom = effectiveFrom ?? new DateTime(2024, 1, 1),
            EffectiveTo = effectiveTo ?? new DateTime(2025, 1, 1)
        };

    public static FeeConfigurationCreateRequest ValidCreateRequest(
        string name = "Admin Fee",
        FeeType type = FeeType.AdminFee,
        decimal percentage = 5m,
        DateTime? effectiveFrom = null,
        DateTime? effectiveTo = null)
        => new()
        {
            Name = name,
            Type = type,
            Percentage = percentage,
            EffectiveFrom = effectiveFrom ?? new DateTime(2024, 1, 1),
            EffectiveTo = effectiveTo ?? new DateTime(2025, 1, 1)
        };

    public static FeeConfigurationUpdateRequest ValidUpdateRequest(
        string name = "Admin Fee",
        FeeType type = FeeType.AdminFee,
        decimal percentage = 5m,
        DateTime? effectiveFrom = null,
        DateTime? effectiveTo = null,
        bool isActive = false)
        => new()
        {
            Name = name,
            Type = type,
            Percentage = percentage,
            EffectiveFrom = effectiveFrom ?? new DateTime(2024, 1, 1),
            EffectiveTo = effectiveTo ?? new DateTime(2025, 1, 1),
            IsActive = isActive
        };
}
