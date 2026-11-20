namespace InsuranceApp.IntegrationTests.TestData;

public static class BrokerTestData
{
    public static object ValidCreate(string code) => new
    {
        code,
        name = "John Broker",
        email = $"{code.ToLowerInvariant()}@test.com",
        phone = "0712345678",
        status = "Active",
        commissionPercentage = 10
    };

    public static object ValidCreateInactive(string code) => new
    {
        code,
        name = "Inactive Broker",
        email = $"{code.ToLowerInvariant()}@test.com",
        phone = "0712345678",
        status = "Inactive",
        commissionPercentage = 10
    };

    public static object ValidUpdate(string code) => new
    {
        code,
        name = "Updated Broker",
        email = $"{code.ToLowerInvariant()}@test.com",
        phone = "0712345678",
        commissionPercentage = 12
    };

    public static object InvalidCreate() => new
    {
        code = "",
        name = "",
        email = "not-an-email",
        phone = "123",
        status = "Active",
        commissionPercentage = -1
    };

    public const int MissingId = 9999;
}