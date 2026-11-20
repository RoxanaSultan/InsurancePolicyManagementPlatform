using InsuranceApp.Application.DTOs.Buildings;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.ValueObjects;

namespace InsuranceApp.Application.Tests.TestData;

public static class BuildingValidatorTestData
{
    public static Address ValidAddress(
        string street = "Main Street",
        string number = "10A")
        => new()
        {
            Street = street,
            Number = number
        };

    public static BuildingBase ValidBuildingBase(
        Address? address = null,
        int cityId = 1,
        int constructionYear = 2000,
        BuildingType buildingType = BuildingType.Residential,
        decimal surfaceArea = 100,
        decimal insuredValue = 500_000,
        int numberOfFloors = 2)
        => new()
        {
            Address = address ?? ValidAddress(),
            CityId = cityId,
            ConstructionYear = constructionYear,
            BuildingType = buildingType,
            SurfaceArea = surfaceArea,
            InsuredValue = insuredValue,
            NumberOfFloors = numberOfFloors
        };

    public static BuildingCreateRequest ValidCreateRequest(
        Address? address = null,
        int cityId = 1,
        int constructionYear = 2000,
        BuildingType buildingType = BuildingType.Residential,
        decimal surfaceArea = 100,
        decimal insuredValue = 500_000,
        int numberOfFloors = 2)
        => new()
        {
            Address = address ?? ValidAddress(),
            CityId = cityId,
            ConstructionYear = constructionYear,
            BuildingType = buildingType,
            SurfaceArea = surfaceArea,
            InsuredValue = insuredValue,
            NumberOfFloors = numberOfFloors
        };

    public static BuildingUpdateRequest ValidUpdateRequest(
        Address? address = null,
        int cityId = 1,
        int constructionYear = 2000,
        BuildingType buildingType = BuildingType.Residential,
        decimal surfaceArea = 100,
        decimal insuredValue = 500_000,
        int numberOfFloors = 2)
        => new()
        {
            Address = address ?? ValidAddress(),
            CityId = cityId,
            ConstructionYear = constructionYear,
            BuildingType = buildingType,
            SurfaceArea = surfaceArea,
            InsuredValue = insuredValue,
            NumberOfFloors = numberOfFloors
        };
}
