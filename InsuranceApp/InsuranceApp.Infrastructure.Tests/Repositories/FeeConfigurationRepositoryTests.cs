using InsuranceApp.Domain.Entities.Metadata;
using InsuranceApp.Infrastructure.Persistence.Repositories.Metadata;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public class FeeConfigurationRepositoryTests
{
    [Fact]
    public async Task GetActiveAsync_ShouldReturnOnlyActiveInRange()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new FeeConfigurationRepository(db);

        db.FeeConfigurations.AddRange(
            new FeeConfiguration
            {
                Name = "Active",
                Type = Domain.Enums.FeeType.AdminFee,
                Percentage = 2m,
                EffectiveFrom = new DateTime(2024, 1, 1),
                EffectiveTo = new DateTime(2026, 1, 1),
                IsActive = true
            },
            new FeeConfiguration
            {
                Name = "Inactive",
                Type = Domain.Enums.FeeType.AdminFee,
                Percentage = 2m,
                EffectiveFrom = new DateTime(2024, 1, 1),
                EffectiveTo = new DateTime(2026, 1, 1),
                IsActive = false
            });
        await db.SaveChangesAsync();

        var result = await repo.GetActiveAsync(new DateTime(2025, 1, 1), CancellationToken.None);

        Assert.Single(result);
        Assert.Equal("Active", result.First().Name);
    }
}
