using InsuranceApp.Application.DTOs.FeeConfiguration;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Tests.TestData.Services;

public static class FeeConfigurationTestData
{
    public static FeeConfigurationCreateRequest ValidCreateRequest(
        string name = "Admin fee",
        FeeType type = FeeType.AdminFee,
        decimal percentage = 2.5m,
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
        string name = "Broker commission",
        FeeType type = FeeType.BrokerCommission,
        decimal percentage = 5.0m,
        DateTime? effectiveFrom = null,
        DateTime? effectiveTo = null,
        bool isActive = false)
        => new()
        {
            Name = name,
            Type = type,
            Percentage = percentage,
            EffectiveFrom = effectiveFrom ?? new DateTime(2024, 6, 1),
            EffectiveTo = effectiveTo ?? new DateTime(2026, 6, 1),
            IsActive = isActive
        };
}