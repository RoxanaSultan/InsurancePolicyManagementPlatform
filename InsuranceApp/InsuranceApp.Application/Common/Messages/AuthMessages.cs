namespace InsuranceApp.Application.Common.Messages;

public static class AuthMessages
{
    public static string BrokerNotFound(string email) =>
        $"Broker with email {email} doesn't exist. An administrator must create it first.";

    public const string InvalidCredentials =
        "Invalid credentials.";

    public const string BrokerProfileNotFound =
        "Broker profile not found.";

    public const string BrokerInactive =
        "Broker is inactive.";

    public static string IdentityErrors(IEnumerable<string> errors) =>
        string.Join("; ", errors);
}