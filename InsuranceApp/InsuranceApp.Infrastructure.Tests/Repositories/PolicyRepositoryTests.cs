using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.Policy;
using InsuranceApp.Application.DTOs.Report;
using InsuranceApp.Domain.Entities.Building;
using InsuranceApp.Domain.Entities.Geography;
using InsuranceApp.Domain.Entities.Metadata;
using InsuranceApp.Domain.Entities.Policy;
using InsuranceApp.Domain.Entities.User;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.ValueObjects;
using InsuranceApp.Infrastructure.Persistence;
using InsuranceApp.Infrastructure.Persistence.Repositories.Policy;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public class PolicyRepositoryTests
{
    [Fact]
    public async Task ExistsActivePolicyForCurrencyAsync_ShouldReturnTrueWhenActiveInRange()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new PolicyRepository(db);

        var (broker, currency, building) = SeedPolicyDependencies(db);
        db.Policies.Add(new Policy
        {
            BrokerId = broker.Id,
            BuildingId = building.Id,
            CurrencyId = currency.Id,
            Status = PolicyStatus.Active,
            StartDate = new DateTime(2024, 1, 1),
            EndDate = new DateTime(2025, 1, 1),
            BasePremiumAmount = 100m,
            CalculatedPremiumAmount = 110m,
            CreationDate = new DateTime(2024, 1, 1),
            LastUpdateDate = new DateTime(2024, 6, 1)
        });
        await db.SaveChangesAsync();

        var exists = await repo.ExistsActivePolicyForCurrencyAsync(
            currency.Id,
            new DateTime(2024, 6, 1),
            CancellationToken.None);

        Assert.True(exists);
    }

    [Fact]
    public async Task GetPoliciesAsync_ShouldFilterByStatus()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new PolicyRepository(db);

        var (broker, currency, building) = SeedPolicyDependencies(db);
        db.Policies.AddRange(
            CreatePolicy(broker.Id, currency.Id, building.Id, PolicyStatus.Active, new DateTime(2024, 1, 1)),
            CreatePolicy(broker.Id, currency.Id, building.Id, PolicyStatus.Draft, new DateTime(2024, 2, 1)));
        await db.SaveChangesAsync();

        var result = await repo.GetPoliciesAsync(
            new PolicyFilterRequest { Status = PolicyStatus.Active },
            new PageRequest { Page = 1, PageSize = 10 },
            CancellationToken.None);

        Assert.Single(result.Items);
        Assert.Equal(PolicyStatus.Active, result.Items[0].Status);
    }

    [Fact]
    public async Task GetPoliciesToExpireAsync_ShouldReturnExpiredCandidates()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new PolicyRepository(db);

        var (broker, currency, building) = SeedPolicyDependencies(db);
        db.Policies.AddRange(
            CreatePolicy(broker.Id, currency.Id, building.Id, PolicyStatus.Active, new DateTime(2024, 1, 1), new DateTime(2024, 6, 1)),
            CreatePolicy(broker.Id, currency.Id, building.Id, PolicyStatus.Active, new DateTime(2024, 7, 1), new DateTime(2025, 6, 1)));
        await db.SaveChangesAsync();

        var result = await repo.GetPoliciesToExpireAsync(new DateTime(2024, 6, 1), CancellationToken.None);

        Assert.Single(result);
    }

    [Fact]
    public async Task GetPoliciesByBrokerAsync_ShouldGroupByBrokerAndCurrency()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new PolicyRepository(db);

        var (broker, currency, building) = SeedPolicyDependencies(db);
        db.Policies.Add(CreatePolicy(broker.Id, currency.Id, building.Id, PolicyStatus.Active, new DateTime(2024, 1, 1)));
        await db.SaveChangesAsync();

        var report = await repo.GetPoliciesByBrokerAsync(
            new ReportFilterRequest { From = new DateTime(2024, 1, 1), To = new DateTime(2025, 1, 1) },
            new PageRequest { Page = 1, PageSize = 10 },
            CancellationToken.None);

        Assert.Single(report.Items);
        Assert.Equal(broker.Id, report.Items[0].Broker.Id);
        Assert.Equal(currency.Code, report.Items[0].Currency!.Code);
    }

    private static Policy CreatePolicy(
        int brokerId,
        int currencyId,
        int buildingId,
        PolicyStatus status,
        DateTime startDate,
        DateTime? endDate = null)
        => new()
        {
            BrokerId = brokerId,
            CurrencyId = currencyId,
            BuildingId = buildingId,
            Status = status,
            StartDate = startDate,
            EndDate = endDate ?? startDate.AddYears(1),
            BasePremiumAmount = 100m,
            CalculatedPremiumAmount = 110m,
            CreationDate = startDate,
            LastUpdateDate = startDate.AddMonths(1)
        };

    private static (Broker broker, Currency currency, Building building) SeedPolicyDependencies(InsuranceDbContext db)
    {
        var country = new Country { Name = "Romania" };
        var county = new County { Name = "Bucharest", Code = "B", Country = country };
        var city = new City { Name = "Bucharest", County = county };
        var broker = new Broker
        {
            Code = "BRK-1",
            Name = "Broker 1",
            Email = "broker@test.com",
            Phone = "0712345678"
        };
        var currency = new Currency { Code = "EUR", Name = "Euro", ExchangeRateToBase = 4.9m, IsActive = true };
        var client = new Domain.Entities.Client.Client
        {
            Name = "Client 1",
            IdentificationNumber = "1234567890123",
            ContactInfo = new ContactInfo("client@test.com", "0712345678", new Address("Str", "1"))
        };
        var building = new Building
        {
            Address = new Address("Str Building", "10"),
            City = city,
            ConstructionYear = 2010,
            BuildingType = BuildingType.Residential,
            SurfaceArea = 100,
            InsuredValue = 100000,
            NumberOfFloors = 2,
            Client = client
        };

        db.AddRange(country, county, city, broker, currency, client, building);
        db.SaveChanges();

        return (broker, currency, building);
    }
}
