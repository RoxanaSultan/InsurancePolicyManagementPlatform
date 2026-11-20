namespace InsuranceApp.Application.Common.ValidatorMessages;

public static class ReportValidatorMessages
{
    public const string FromRequired =
        "'From' is required.";

    public const string ToRequired =
        "'To' is required.";

    public const string ToAfterFrom =
        "'To' must be greater than or equal to 'From'.";
}