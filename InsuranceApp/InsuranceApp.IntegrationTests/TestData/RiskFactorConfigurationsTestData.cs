namespace InsuranceApp.IntegrationTests.TestData;

public static class RiskFactorConfigurationsTestData
{
    public static object ValidCreate() => new
    {
        level = "Country",
        referenceId = 1,
        adjustmentPercentage = 5
    };

    public static object ValidUpdate() => new
    {
        level = "Country",
        referenceId = 1,
        adjustmentPercentage = 7,
        isActive = false
    };

    public static object InvalidCreate() => new
    {
        level = "City",
        referenceId = 0,
        adjustmentPercentage = 200
    };

    public const int MissingId = 9999;
}
