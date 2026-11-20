namespace InsuranceApp.IntegrationTests.TestData;

public static class AuthTestData
{
    public static object RegisterInvalid() => new
    {
        email = "",
        password = "short"
    };

    public static object Register(string email = "broker@test.com", string password = "Valid123!") => new
    {
        email,
        password
    };

    public static object Login(string email = "missing@test.com", string password = "Wrong123!") => new
    {
        email,
        password
    };

    // Folosit in testul de login valid (brokerul trebuie sa existe inainte de register)
    public static object BrokerForAuth(string code = "BRK-AUTH-1") => new
    {
        code,
        name = "Auth Broker",
        email = "auth.broker@test.com",
        phone = "0712345678",
        status = "Active",
        commissionPercentage = 10
    };

    public const string AuthBrokerEmail = "auth.broker@test.com";
    public const string ValidPassword = "Valid123!";
    public const string MissingEmail = "missing@test.com";
    public const string WrongPassword = "Wrong123!";
    public const string BrokerMissingEmail = "broker@test.com";
}