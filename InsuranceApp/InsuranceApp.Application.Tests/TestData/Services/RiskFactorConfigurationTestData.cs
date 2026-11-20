using InsuranceApp.Application.DTOs.RiskFactorConfiguration;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Tests.TestData.Services;

public static class RiskFactorConfigurationTestData
{
    public static RiskFactorConfigurationCreateRequest ValidCreateRequest(
        RiskFactorLevel level,
        int referenceId,
        bool isActive,
        decimal adjustmentPercentage = 5m)
        => new()
        {
            Level = level,
            ReferenceId = referenceId,
            AdjustmentPercentage = adjustmentPercentage,
            IsActive = isActive
        };

    public static RiskFactorConfigurationUpdateRequest ValidUpdateRequest(
        RiskFactorLevel level,
        int referenceId,
        bool isActive,
        decimal adjustmentPercentage = 3m)
        => new()
        {
            Level = level,
            ReferenceId = referenceId,
            AdjustmentPercentage = adjustmentPercentage,
            IsActive = isActive
        };
}