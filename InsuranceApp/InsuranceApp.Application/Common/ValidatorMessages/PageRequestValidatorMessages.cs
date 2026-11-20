namespace InsuranceApp.Application.Common.ValidatorMessages;

public static class PageRequestValidatorMessages
{
    public const string PageMin =
        "Page number must be at least 1.";

    public const string PageSizeRange =
        "Page size must be between 1 and 100.";
}