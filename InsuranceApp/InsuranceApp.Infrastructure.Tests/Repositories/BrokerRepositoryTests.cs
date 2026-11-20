using InsuranceApp.Domain.Entities.User;
using InsuranceApp.Infrastructure.Persistence.Repositories.Broker;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public class BrokerRepositoryTests
{
    [Fact]
    public async Task GetByEmailAsync_ShouldReturnBroker()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new BrokerRepository(db);

        var broker = new Broker
        {
            Code = "BRK-1",
            Name = "Broker One",
            Email = "a@test.com",
            Phone = "0712345678",
            Status = Domain.Enums.BrokerStatus.Active
        };
        db.Brokers.Add(broker);
        await db.SaveChangesAsync();

        var result = await repo.GetByEmailAsync("a@test.com");

        Assert.NotNull(result);
        Assert.Equal(broker.Id, result!.Id);
    }

    [Fact]
    public async Task IsEmailUniqueAsync_ShouldRespectExcludeId()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new BrokerRepository(db);

        var broker = new Broker
        {
            Code = "BRK-1",
            Name = "Broker One",
            Email = "dup@test.com",
            Phone = "0712345678",
            Status = Domain.Enums.BrokerStatus.Active
        };
        db.Brokers.Add(broker);
        await db.SaveChangesAsync();

        var notUnique = await repo.IsEmailUniqueAsync("dup@test.com");
        var uniqueWhenExcluded = await repo.IsEmailUniqueAsync("dup@test.com", broker.Id);

        Assert.False(notUnique);
        Assert.True(uniqueWhenExcluded);
    }
}
