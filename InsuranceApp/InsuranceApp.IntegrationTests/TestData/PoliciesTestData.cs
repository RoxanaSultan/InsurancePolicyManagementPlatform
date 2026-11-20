using InsuranceApp.Domain.Enums;

namespace InsuranceApp.IntegrationTests.TestData;

public static class PoliciesTestData
{
    public static object InvalidCreate() => new
    {
        buildingId = 0,
        currencyId = 0,
        startDate = default(DateTime),
        endDate = default(DateTime),
        basePremiumAmount = 0
    };

    public static object CancelRequest(string reason = "Test cancellation") => new
    {
        cancellationDate = DateTime.UtcNow,
        cancellationReason = reason
    };

    public const int MissingId = 9999;
    public const int MissingBuildingId = 9999;

    public const string BrokerCode = "BRK-1";
    public const string BrokerName = "Test Broker";
    public const string BrokerEmail = "broker@test.com";
    public const string BrokerPhone = "0712345678";

    public const string CurrencyCode = "EUR";
    public const string CurrencyName = "Euro";
    public const decimal CurrencyExchangeRate = 4.9m;

    public const string ClientName = "Ion Popescu";
    public const string ClientIdentificationNumber = "1234567890123";
    public const string ClientEmail = "ion@test.com";
    public const string ClientPhone = "0712345678";
    public const string ClientStreet = "Str Test";
    public const string ClientNumber = "1";

    public const string BuildingStreet = "Str Building";
    public const string BuildingNumber = "10";
    public const int BuildingCityId = 1;
    public const int BuildingConstructionYear = 2010;
    public const BuildingType BuildingTypeValue = BuildingType.Residential;
    public const decimal BuildingSurfaceArea = 100;
    public const decimal BuildingInsuredValue = 100000;
    public const int BuildingFloors = 2;

    public static readonly DateTime PolicyStartDate = new(2024, 1, 1);
    public static readonly DateTime PolicyEndDate = new(2025, 1, 1);
    public const decimal PolicyBasePremium = 100m;
    public const decimal PolicyCalculatedPremium = 110m;
    public static readonly DateTime PolicyCreationDate = new(2024, 1, 1);
    public static readonly DateTime PolicyLastUpdateDate = new(2024, 1, 2);
}
