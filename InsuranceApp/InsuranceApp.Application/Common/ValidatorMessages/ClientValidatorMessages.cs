namespace InsuranceApp.Application.Common.ValidatorMessages;

public static class ClientValidatorMessages
{
    public const string NameRequired =
        "Name is required.";

    public const string NameMinLength =
        "Name must be at least 2 characters.";

    public const string NameMaxLength =
        "Name cannot exceed 100 characters.";

    public const string ContactInfoRequired =
        "Contact information is required.";

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

    public const string StreetRequired =
        "Street is required.";

    public const string StreetMinLength =
        "Street must be at least 2 characters.";

    public const string StreetMaxLength =
        "Street must be at most 200 characters.";

    public const string NumberRequired =
        "Number is required.";

    public const string NumberMinLength =
        "Number must be at least 1 character.";

    public const string NumberMaxLength =
        "Number must be at most 20 characters.";

    public const string ClientTypeRequired =
        "Client type is required.";

    public const string ClientTypeInvalid =
        "Client type must be between Individual and Company.";

    public const string IdentificationNumberRequired =
        "Identification number is required.";

    public const string CnpInvalid =
        "CNP must contain exactly 13 digits.";

    public const string CompanyRegistrationDigitsOnly =
        "Company Registration Number must contain only digits.";

    public const string CompanyRegistrationMinLength =
        "Company Registration Number must be at least 2 digits.";

    public const string CompanyRegistrationMaxLength =
        "Company Registration Number cannot exceed 20 digits.";

    public const string SearchRequiresNameOrIdentificationNumber =
        "Provide at least Name or IdentificationNumber.";
}