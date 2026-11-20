using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Domain.Entities.Geography;
using InsuranceApp.Infrastructure.Persistence.Repositories.Geography;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public class CityRepositoryTests
{
    [Fact]
    public async Task GetByCountyIdAsync_ShouldReturnPagedItems()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new CityRepository(db);

        var county = new County { Name = "Bucharest", Code = "B", CountryId = 1 };
        db.Counties.Add(county);
        await db.SaveChangesAsync();

        db.Cities.AddRange(
            new City { Name = "City1", CountyId = county.Id },
            new City { Name = "City2", CountyId = county.Id });
        await db.SaveChangesAsync();

        var page = await repo.GetByCountyIdAsync(county.Id, new PageRequest { Page = 1, PageSize = 10 });

        Assert.Equal(2, page.Items.Count);
        Assert.Equal(2, page.TotalItems);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueWhenFound()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new CityRepository(db);

        var city = new City { Name = "City1", CountyId = 1 };
        db.Cities.Add(city);
        await db.SaveChangesAsync();

        var exists = await repo.ExistsAsync(city.Id);

        Assert.True(exists);
    }
}
