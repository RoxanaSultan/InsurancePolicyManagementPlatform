namespace InsuranceApp.Application.Common.Messages;

public static class ClientMessages
{
    public static string NotFound(int clientId) =>
        $"Client with id {clientId} not found.";

    public static string IdentificationNumberAlreadyExists(string identificationNumber) =>
        $"Client with Identification Number {identificationNumber} already exists.";

    public static string EmailAlreadyExists(string email) =>
        $"Client with Email {email} already exists.";

    public static string PhoneNumberAlreadyExists(string phoneNumber) =>
        $"Client with Phone Number {phoneNumber} already exists.";
}
