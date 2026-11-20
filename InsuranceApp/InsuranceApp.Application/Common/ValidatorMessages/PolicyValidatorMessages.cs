namespace InsuranceApp.Application.Common.ValidatorMessages;

public static class PolicyValidatorMessages
{
    public const string StartDateRequired =
        "StartDate is required.";

    public const string StartDateInvalid =
        "StartDate is invalid.";

    public const string EndDateRequired =
        "EndDate is required.";

    public const string EndDateInvalid =
        "EndDate is invalid.";

    public const string EndDateAfterStart =
        "EndDate must be after StartDate.";

    public const string BasePremiumMin =
        "BasePremiumAmount must be greater than 0.";

    public const string BasePremiumMaxDecimals =
        "BasePremiumAmount must have at most 2 decimals.";

    public const string BasePremiumTooLarge =
        "BasePremiumAmount is too large.";

    public const string BuildingIdInvalid =
        "BuildingId must be a valid id.";

    public const string CurrencyIdInvalid =
        "CurrencyId must be a valid id.";
}