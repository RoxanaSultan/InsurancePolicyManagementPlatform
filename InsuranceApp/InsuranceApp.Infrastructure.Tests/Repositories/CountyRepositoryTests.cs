using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Domain.Entities.Geography;
using InsuranceApp.Infrastructure.Persistence.Repositories.Geography;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public class CountyRepositoryTests
{
    [Fact]
    public async Task GetByCountryIdAsync_ShouldReturnPagedItems()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new CountyRepository(db);

        var country = new Country { Name = "Romania" };
        db.Countries.Add(country);
        await db.SaveChangesAsync();

        db.Counties.AddRange(
            new County { Name = "County1", Code = "C1", CountryId = country.Id },
            new County { Name = "County2", Code = "C2", CountryId = country.Id });
        await db.SaveChangesAsync();

        var page = await repo.GetByCountryIdAsync(country.Id, new PageRequest { Page = 1, PageSize = 10 });

        Assert.Equal(2, page.Items.Count);
        Assert.Equal(2, page.TotalItems);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueWhenFound()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new CountyRepository(db);

        var county = new County { Name = "County1", Code = "C1", CountryId = 1 };
        db.Counties.Add(county);
        await db.SaveChangesAsync();

        var exists = await repo.ExistsAsync(county.Id);

        Assert.True(exists);
    }
}
