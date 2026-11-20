namespace InsuranceApp.Application.Common.ValidatorMessages;

public static class PolicyCancellationValidatorMessages
{
    public const string CancellationDateRequired =
        "Cancellation date is required.";

    public const string CancellationDateInvalid =
        "Cancellation date is invalid.";

    public const string CancellationReasonRequired =
        "Cancellation reason is required.";

    public const string CancellationReasonMaxLength =
        "Cancellation reason cannot exceed 500 characters.";
}