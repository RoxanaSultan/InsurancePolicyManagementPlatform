using InsuranceApp.Domain.Entities.Policy;
using InsuranceApp.Infrastructure.Persistence.Repositories.Policy;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public class PolicyCancellationRepositoryTests
{
    [Fact]
    public async Task AddAndGetById_ShouldReturnEntity()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new PolicyCancellationRepository(db);

        var entity = new PolicyCancellation
        {
            PolicyId = 1,
            CancellationDate = new DateTime(2024, 1, 1),
            CancellationReason = "Test"
        };

        await repo.AddAsync(entity);
        await db.SaveChangesAsync();

        var result = await repo.GetByIdAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal("Test", result!.CancellationReason);
    }
}
