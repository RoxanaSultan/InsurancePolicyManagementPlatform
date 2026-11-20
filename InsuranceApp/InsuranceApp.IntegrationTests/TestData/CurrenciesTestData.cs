namespace InsuranceApp.IntegrationTests.TestData;

public static class CurrenciesTestData
{
    public static object ValidCreate(string code = "EUR") => new
    {
        code,
        name = "Euro",
        exchangeRateToBase = 4.9
    };

    public static object ValidUpdate(string code = "EUR") => new
    {
        code,
        name = "Euro Updated",
        exchangeRateToBase = 5.1,
        isActive = true
    };

    public static object InvalidCreate() => new
    {
        code = "",
        name = "",
        exchangeRateToBase = 0
    };

    public const int MissingId = 9999;
}
