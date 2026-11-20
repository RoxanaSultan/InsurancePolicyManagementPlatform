namespace InsuranceApp.Application.Common.ValidatorMessages;

public static class FeeConfigurationValidatorMessages
{
    public const string NameRequired =
        "Name is required.";

    public const string NameMaxLength =
        "Name cannot exceed 150 characters.";

    public const string TypeRequired =
        "Type is required.";

    public const string TypeInvalid =
        "Type must be BrokerCommission, RiskAdjustment or AdminFee.";

    public const string PercentageRequired =
        "Percentage is required.";

    public const string PercentageRange =
        "Percentage must be between 0.01 and 100.00.";

    public const string EffectiveFromRequired =
        "EffectiveFrom is required.";

    public const string EffectiveToAfterFrom =
        "EffectiveTo must be greater than EffectiveFrom.";

    public const string IsActiveRequired =
        "IsActive is required.";
}