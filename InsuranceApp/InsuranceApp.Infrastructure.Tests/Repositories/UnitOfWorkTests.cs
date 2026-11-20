using InsuranceApp.Domain.Entities.Geography;
using InsuranceApp.Infrastructure.Persistence.Repositories;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public class UnitOfWorkTests
{
    [Fact]
    public void Properties_ShouldReturnRepositories()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var uow = new UnitOfWork(db);

        Assert.NotNull(uow.Brokers);
        Assert.NotNull(uow.Buildings);
        Assert.NotNull(uow.Clients);
        Assert.NotNull(uow.Countries);
        Assert.NotNull(uow.Counties);
        Assert.NotNull(uow.Cities);
        Assert.NotNull(uow.Currencies);
        Assert.NotNull(uow.FeeConfigurations);
        Assert.NotNull(uow.RiskFactorConfigurations);
        Assert.NotNull(uow.Policies);
        Assert.NotNull(uow.PolicyCancellations);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldPersist()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var uow = new UnitOfWork(db);

        db.Countries.Add(new Country { Name = "Romania" });

        var changes = await uow.SaveChangesAsync();

        Assert.Equal(1, changes);
        Assert.Single(db.Countries);
    }
}
