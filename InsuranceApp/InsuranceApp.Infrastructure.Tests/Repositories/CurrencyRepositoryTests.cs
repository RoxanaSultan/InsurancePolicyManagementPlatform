using InsuranceApp.Domain.Entities.Metadata;
using InsuranceApp.Infrastructure.Persistence.Repositories.Metadata;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public class CurrencyRepositoryTests
{
    [Fact]
    public async Task IsCodeUnique_ShouldReturnTrueWhenCodeExists()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new CurrencyRepository(db);

        db.Currencies.Add(new Currency { Code = "EUR", Name = "Euro", ExchangeRateToBase = 4.9m, IsActive = true });
        await db.SaveChangesAsync();

        var exists = await repo.IsCodeUnique("EUR");

        Assert.True(exists);
    }

    [Fact]
    public async Task IsCodeUnique_ShouldReturnFalseWhenCodeMissing()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new CurrencyRepository(db);

        var exists = await repo.IsCodeUnique("USD");

        Assert.False(exists);
    }
}
