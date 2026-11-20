using InsuranceApp.Domain.Entities.Metadata;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Infrastructure.Persistence.Repositories.Metadata;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public class RiskFactorConfigurationRepositoryTests
{
    [Fact]
    public async Task ActiveUniqueLevelAndReferenceIdExistsAsync_ShouldRespectExcludeId()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new RiskFactorConfigurationRepository(db);

        var item = new RiskFactorConfiguration
        {
            Level = RiskFactorLevel.Country,
            ReferenceId = 1,
            AdjustmentPercentage = 5m,
            IsActive = true
        };
        db.RiskFactorConfigurations.Add(item);
        await db.SaveChangesAsync();

        var exists = await repo.ActiveUniqueLevelAndReferenceIdExistsAsync(
            RiskFactorLevel.Country, 1, null, CancellationToken.None);
        var existsExcluded = await repo.ActiveUniqueLevelAndReferenceIdExistsAsync(
            RiskFactorLevel.Country, 1, item.Id, CancellationToken.None);

        Assert.True(exists);
        Assert.False(existsExcluded);
    }

    [Fact]
    public async Task GetApplicableAsync_ShouldReturnMatchingLevels()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new RiskFactorConfigurationRepository(db);

        db.RiskFactorConfigurations.AddRange(
            new RiskFactorConfiguration
            {
                Level = RiskFactorLevel.Country,
                ReferenceId = 1,
                AdjustmentPercentage = 5m,
                IsActive = true
            },
            new RiskFactorConfiguration
            {
                Level = RiskFactorLevel.City,
                ReferenceId = 2,
                AdjustmentPercentage = 3m,
                IsActive = true
            },
            new RiskFactorConfiguration
            {
                Level = RiskFactorLevel.City,
                ReferenceId = 99,
                AdjustmentPercentage = 3m,
                IsActive = true
            });
        await db.SaveChangesAsync();

        var result = await repo.GetApplicableAsync(1, 0, 2, 0, CancellationToken.None);

        Assert.Equal(2, result.Count);
    }
}
