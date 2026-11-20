namespace InsuranceApp.Application.Common.ValidatorMessages;

public static class BrokerValidatorMessages
{
    public const string NameRequired =
        "Name is required";

    public const string NameMaxLength =
        "Name cannot exceed 100 characters.";

    public const string NameInvalidCharacters =
        "Name must contain only letters and spaces.";

    public const string EmailRequired =
        "Email is required.";

    public const string EmailMinLength =
        "Email must be at least 5 characters.";

    public const string EmailMaxLength =
        "Email cannot exceed 254 characters.";

    public const string EmailInvalid =
        "Email must be a valid email address.";

    public const string PhoneRequired =
        "Phone number is required.";

    public const string PhoneInvalid =
        "Phone number must be Romanian: exactly 10 digits and start with 0.";

    public const string CommissionPercentageMin =
        "Commission percentage greater than or equal to 0.";

    public const string StatusInvalid =
        "Broker status must be between Active and Inactive.";
}