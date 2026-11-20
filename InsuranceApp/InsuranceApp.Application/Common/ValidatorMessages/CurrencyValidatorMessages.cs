namespace InsuranceApp.Application.Common.ValidatorMessages;

public static class CurrencyValidatorMessages
{
    public const string CodeRequired =
        "Code is required";

    public const string CodeLength =
        "Code must have exactly 3 characters";

    public const string CodeLettersOnly =
        "Code must contain only letters";

    public const string NameRequired =
        "Name is required";

    public const string NameMaxLength =
        "Name cannot exceed 100 characters.";

    public const string NameInvalidCharacters =
        "Name must contain only letters and spaces.";

    public const string ExchangeRateRequired =
        "ExchangeRateToBase is required.";

    public const string ExchangeRateMin =
        "ExchangeRateToBase must be greater than 0.";

    public const string IsActiveRequired =
        "IsActive is required";
}