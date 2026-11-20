namespace InsuranceApp.IntegrationTests.TestData;

public static class FeeConfigurationsTestData
{
    public static object ValidCreate() => new
    {
        name = "Admin Fee",
        type = "AdminFee",
        percentage = 2.5,
        effectiveFrom = new DateTime(2024, 1, 1),
        effectiveTo = new DateTime(2025, 1, 1)
    };

    public static object ValidUpdate() => new
    {
        name = "Admin Fee Updated",
        type = "AdminFee",
        percentage = 3.0,
        effectiveFrom = new DateTime(2024, 1, 1),
        effectiveTo = new DateTime(2026, 1, 1),
        isActive = false
    };

    public static object InvalidCreate() => new
    {
        name = "",
        type = "AdminFee",
        percentage = 0,
        effectiveFrom = DateTime.UtcNow,
        effectiveTo = DateTime.UtcNow.AddDays(-1)
    };

    public const int MissingId = 9999;
}
