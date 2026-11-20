namespace InsuranceApp.Application.Common.ValidatorMessages;

public static class RiskFactorConfigurationValidatorMessages
{
    public const string LevelInvalid =
        "Level must be between Country, County, City and BuildingType.";

    public const string ReferenceIdRequired =
        "ReferenceId is required.";

    public const string AdjustmentPercentageRange =
        "AdjustmentPercentage must be between -100 and 100.";

    public const string IsActiveRequired =
        "IsActive is required.";
}