using InsuranceApp.Application.DTOs.RiskFactorConfiguration;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Tests.TestData;

public static class RiskFactorConfigurationValidatorTestData
{
    public static RiskFactorConfigurationBase ValidRiskFactorBase(
        RiskFactorLevel level = RiskFactorLevel.Country,
        int referenceId = 1,
        decimal adjustmentPercentage = 5m)
        => new()
        {
            Level = level,
            ReferenceId = referenceId,
            AdjustmentPercentage = adjustmentPercentage
        };

    public static RiskFactorConfigurationCreateRequest ValidCreateRequest(
        RiskFactorLevel level = RiskFactorLevel.City,
        int referenceId = 3,
        decimal adjustmentPercentage = 2m,
        bool isActive = true)
        => new()
        {
            Level = level,
            ReferenceId = referenceId,
            AdjustmentPercentage = adjustmentPercentage,
            IsActive = isActive
        };

    public static RiskFactorConfigurationUpdateRequest ValidUpdateRequest(
        RiskFactorLevel level = RiskFactorLevel.BuildingType,
        int referenceId = 1,
        decimal adjustmentPercentage = 1m,
        bool isActive = false)
        => new()
        {
            Level = level,
            ReferenceId = referenceId,
            AdjustmentPercentage = adjustmentPercentage,
            IsActive = isActive
        };
}
