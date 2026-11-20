namespace InsuranceApp.Application.Common.Messages;

public static class CurrencyMessages
{
    public static string CodeAlreadyExists(string code) =>
        $"Currency code '{code}' already exists.";

    public static string NotFound(int currencyId) =>
        $"Currency with id {currencyId} not found.";

    public const string CannotDeactivateBecauseActivePolicy =
        "Currency cannot be deactivated because it is used in an active policy.";
}