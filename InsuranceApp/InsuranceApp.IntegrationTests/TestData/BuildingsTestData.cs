namespace InsuranceApp.IntegrationTests.TestData;

public static class BuildingsTestData
{
    public const int MissingId = 9999;
    public const int DefaultCityId = 1;

    public static object CreateBuilding_InvalidInsuredValue() => new
    {
        address = new { street = "Str Test", number = "10" },
        cityId = DefaultCityId,
        constructionYear = 2000,
        buildingType = "Residential",
        surfaceArea = 120,
        insuredValue = -1,
        numberOfFloors = 2
    };

    public static object CreateBuilding_Valid(string street = "Str Building", string number = "10") => new
    {
        address = new { street, number },
        cityId = DefaultCityId,
        constructionYear = 2010,
        buildingType = "Residential",
        surfaceArea = 100,
        insuredValue = 100000,
        numberOfFloors = 2
    };
    public static object CreateBuilding_ValidForMissingClient() =>
        CreateBuilding_Valid(street: "Str Test", number: "10");

    public static object UpdateBuilding_Valid() => new
    {
        address = new { street = "Str Building Updated", number = "11" },
        cityId = DefaultCityId,
        constructionYear = 2012,
        buildingType = "Residential",
        surfaceArea = 120,
        insuredValue = 120000,
        numberOfFloors = 3
    };
}