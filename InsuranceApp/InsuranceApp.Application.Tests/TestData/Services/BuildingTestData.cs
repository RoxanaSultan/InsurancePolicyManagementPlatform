using InsuranceApp.Application.DTOs.Buildings;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.ValueObjects;

namespace InsuranceApp.Application.Tests.TestData.Services;

public static class BuildingTestData
{
    public static BuildingCreateRequest ValidCreateRequest(int cityId = 10) =>
        new()
        {
            Address = new Address("Libertatii", "10"),
            CityId = cityId,
            ConstructionYear = 2005,
            BuildingType = BuildingType.Residential,
            SurfaceArea = 120.5m,
            InsuredValue = 250000m,
            NumberOfFloors = 2
        };

    public static BuildingUpdateRequest ValidUpdateRequest(int cityId = 10) =>
        new()
        {
            Address = new Address("Memorandului", "15"),
            CityId = cityId,
            ConstructionYear = 2010,
            BuildingType = BuildingType.Office,
            SurfaceArea = 300m,
            InsuredValue = 500000m,
            NumberOfFloors = 5
        };
}