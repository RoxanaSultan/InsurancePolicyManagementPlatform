namespace InsuranceApp.IntegrationTests.TestData;

public static class ClientsTestData
{
    public static object ValidCreate(string email = "ion@test.com") => new
    {
        name = "Ion Popescu",
        identificationNumber = "1234567890123",
        clientType = "Individual",
        contactInfo = new
        {
            email,
            phoneNumber = "0712345678",
            address = new { street = "Str Client", number = "1" }
        }
    };

    public static object ValidUpdate(string email = "updated@test.com") => new
    {
        name = "Ion Updated",
        identificationNumber = "1234567890123",
        contactInfo = new
        {
            email,
            phoneNumber = "0712345678",
            address = new { street = "Str Client", number = "1" }
        }
    };

    public static object InvalidCreate() => new
    {
        name = "",
        identificationNumber = "",
        clientType = "Individual",
        contactInfo = new
        {
            email = "not-an-email",
            phoneNumber = "",
            address = new { street = "", number = "" }
        }
    };

    public const int MissingId = 9999;

    public static object CreateIndividualClient(string email = "ion@test.com") =>
        ValidCreate(email);
}
