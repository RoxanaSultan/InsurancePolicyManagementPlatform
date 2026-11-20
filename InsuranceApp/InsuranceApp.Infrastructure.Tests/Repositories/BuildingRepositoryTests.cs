using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Domain.Entities.Building;
using InsuranceApp.Domain.Entities.Client;
using InsuranceApp.Domain.Entities.Geography;
using InsuranceApp.Domain.ValueObjects;
using InsuranceApp.Infrastructure.Persistence.Repositories.Building;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public class BuildingRepositoryTests
{
    [Fact]
    public async Task GetByIdAsync_ShouldIncludeGeography()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new BuildingRepository(db);

        var country = new Country { Name = "Romania" };
        var county = new County { Name = "Bucharest", Code = "B", Country = country };
        var city = new City { Name = "Bucharest", County = county };
        var client = new Client
        {
            Name = "Ion",
            IdentificationNumber = "1234567890123",
            ContactInfo = new ContactInfo("ion@test.com", "0712345678", new Address("Str", "1"))
        };

        var building = new Building
        {
            Address = new Address("Str Building", "10"),
            City = city,
            ConstructionYear = 2010,
            BuildingType = Domain.Enums.BuildingType.Residential,
            SurfaceArea = 100,
            InsuredValue = 100000,
            NumberOfFloors = 2,
            Client = client
        };

        db.Buildings.Add(building);
        await db.SaveChangesAsync();

        var result = await repo.GetByIdAsync(building.Id);

        Assert.NotNull(result);
        Assert.NotNull(result!.City);
        Assert.NotNull(result.City.County);
        Assert.NotNull(result.City.County.Country);
    }

    [Fact]
    public async Task GetByClientIdAsync_ShouldReturnPagedItems()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new BuildingRepository(db);

        var client = new Client
        {
            Name = "Ion",
            IdentificationNumber = "1234567890123",
            ContactInfo = new ContactInfo("ion@test.com", "0712345678", new Address("Str", "1"))
        };
        db.Clients.Add(client);
        await db.SaveChangesAsync();

        db.Buildings.AddRange(
            new Building
            {
                Address = new Address("Str A", "1"),
                CityId = 1,
                ConstructionYear = 2010,
                BuildingType = Domain.Enums.BuildingType.Residential,
                SurfaceArea = 100,
                InsuredValue = 100000,
                NumberOfFloors = 2,
                ClientId = client.Id
            },
            new Building
            {
                Address = new Address("Str B", "2"),
                CityId = 1,
                ConstructionYear = 2011,
                BuildingType = Domain.Enums.BuildingType.Residential,
                SurfaceArea = 120,
                InsuredValue = 120000,
                NumberOfFloors = 3,
                ClientId = client.Id
            });
        await db.SaveChangesAsync();

        var page = await repo.GetByClientIdAsync(client.Id, new PageRequest { Page = 1, PageSize = 1 });

        Assert.Single(page.Items);
        Assert.Equal(1, page.Page);
        Assert.Equal(1, page.PageSize);
        Assert.Equal(2, page.TotalItems);
    }
}
