namespace InsuranceApp.Application.Common.ValidatorMessages;

public static class RegisterValidatorMessages
{
    public const string EmailRequired =
        "Email is required.";

    public const string EmailMinLength =
        "Email must be at least 5 characters.";

    public const string EmailMaxLength =
        "Email cannot exceed 254 characters.";

    public const string EmailInvalid =
        "Email must be a valid email address.";

    public const string PasswordRequired =
        "Password is required.";

    public const string PasswordMinLength =
        "Password must be at least 8 characters long.";

    public const string PasswordLowercaseRequired =
        "Password must contain at least one lowercase letter.";

    public const string PasswordUppercaseRequired =
        "Password must contain at least one uppercase letter.";

    public const string PasswordDigitRequired =
        "Password must contain at least one digit.";

    public const string PasswordSpecialCharacterRequired =
        "Password must contain at least one special character.";
}