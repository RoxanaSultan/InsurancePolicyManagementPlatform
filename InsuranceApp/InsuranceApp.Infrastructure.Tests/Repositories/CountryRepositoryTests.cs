using InsuranceApp.Domain.Entities.Geography;
using InsuranceApp.Infrastructure.Persistence.Repositories.Geography;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public class CountryRepositoryTests
{
    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueWhenFound()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new CountryRepository(db);

        var country = new Country { Name = "Romania" };
        db.Countries.Add(country);
        await db.SaveChangesAsync();

        var exists = await repo.ExistsAsync(country.Id);

        Assert.True(exists);
    }
}
