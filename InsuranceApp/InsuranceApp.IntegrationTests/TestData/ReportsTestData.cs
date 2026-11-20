namespace InsuranceApp.IntegrationTests.TestData;

public static class ReportsTestData
{
    public const string BaseQuery = "?from=2025-01-01&to=2025-01-02&page=1&pageSize=20";
    public const string MissingDatesQuery = "?page=1&pageSize=20";
    public const string InvalidStatusQuery = "?from=2025-01-01&to=2025-01-02&page=1&pageSize=20&status=999";
}
