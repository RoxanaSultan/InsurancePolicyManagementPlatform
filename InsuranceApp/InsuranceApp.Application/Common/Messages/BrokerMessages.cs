namespace InsuranceApp.Application.Common.Messages;

public static class BrokerMessages
{
    public static string NotFound(int brokerId) =>
        $"Broker with ID {brokerId} not found.";

    public static string EmailAlreadyExists(string email) =>
        $"Broker with Email {email} already exists.";

    public static string PhoneNumberAlreadyExists(string phoneNumber) =>
        $"Broker with Phone Number {phoneNumber} already exists.";
}