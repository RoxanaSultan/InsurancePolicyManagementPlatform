using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Domain.Entities.Geography;
using InsuranceApp.Infrastructure.Persistence.Repositories;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public class GenericRepositoryTests
{
    [Fact]
    public async Task AddAndGetById_ShouldReturnEntity()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new GenericRepository<Country>(db);

        var country = new Country { Name = "Romania" };

        await repo.AddAsync(country);
        await db.SaveChangesAsync();

        var result = await repo.GetByIdAsync(country.Id);

        Assert.NotNull(result);
        Assert.Equal("Romania", result!.Name);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedItems()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new GenericRepository<Country>(db);

        db.Countries.AddRange(
            new Country { Name = "A" },
            new Country { Name = "B" });
        await db.SaveChangesAsync();

        var result = await repo.GetAllAsync(new PageRequest { Page = 1, PageSize = 1 });

        Assert.Single(result.Items);
        Assert.Equal(1, result.Page);
        Assert.Equal(1, result.PageSize);
        Assert.Equal(1, result.TotalItems);
        Assert.Equal(1, result.TotalPages);
    }
}
