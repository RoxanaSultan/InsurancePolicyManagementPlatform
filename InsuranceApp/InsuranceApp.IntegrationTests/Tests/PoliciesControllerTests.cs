using FluentAssertions;
using InsuranceApp.Domain.Entities.Building;
using InsuranceApp.Domain.Entities.Client;
using InsuranceApp.Domain.Entities.Metadata;
using InsuranceApp.Domain.Entities.Policy;
using InsuranceApp.Domain.Entities.User;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.ValueObjects;
using InsuranceApp.IntegrationTests.Infrastructure;
using InsuranceApp.IntegrationTests.TestData;
using InsuranceApp.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Net;    
using System.Net.Http.Json;
using System.Text.Json;

namespace InsuranceApp.IntegrationTests.Tests;

public class PoliciesControllerTests
{
    [Fact]
    public async Task GetPolicies_ShouldReturnPagedList()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync("/api/brokers/policies?page=1&pageSize=20");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Object);
        doc.RootElement.GetProperty("items").ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Fact]
    public async Task GetPolicyById_WhenMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync($"/api/brokers/policies/{PoliciesTestData.MissingId}");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ActivatePolicy_WhenMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PostAsync($"/api/brokers/policies/{PoliciesTestData.MissingId}/activate", content: null);

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CancelPolicy_WhenMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PostAsJsonAsync(
            $"/api/brokers/policies/{PoliciesTestData.MissingId}/cancel",
            PoliciesTestData.CancelRequest("Test"));

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreatePolicy_InvalidInput_ShouldReturn400()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PostAsJsonAsync("/api/brokers/policies", PoliciesTestData.InvalidCreate());

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreatePolicy_WhenBuildingMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        using (var scope = sut.Factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
            SeedBrokerAndCurrency(db);
        }

        // Act
        var resp = await http.PostAsJsonAsync("/api/brokers/policies", new
        {
            buildingId = PoliciesTestData.MissingBuildingId,
            currencyId = 1,
            startDate = PoliciesTestData.PolicyStartDate,
            endDate = PoliciesTestData.PolicyEndDate,
            basePremiumAmount = PoliciesTestData.PolicyBasePremium
        });

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPolicyById_WhenExists_ShouldReturnOk()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        int policyId;
        using (var scope = sut.Factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
            policyId = SeedPolicy(db, PolicyStatus.Draft);
        }

        // Act
        var resp = await http.GetAsync($"/api/brokers/policies/{policyId}");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ActivatePolicy_WhenDraft_ShouldReturnOk()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        int policyId;
        using (var scope = sut.Factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
            policyId = SeedPolicy(db, PolicyStatus.Draft);
        }

        // Act
        var resp = await http.PostAsync($"/api/brokers/policies/{policyId}/activate", content: null);

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CancelPolicy_WhenActive_ShouldReturnOk()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        int policyId;
        using (var scope = sut.Factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
            policyId = SeedPolicy(db, PolicyStatus.Active);
        }

        // Act
        var resp = await http.PostAsJsonAsync(
            $"/api/brokers/policies/{policyId}/cancel",
            PoliciesTestData.CancelRequest());

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private static void SeedBrokerAndCurrency(InsuranceDbContext db)
    {
        if (!db.Brokers.Any())
        {
            db.Brokers.Add(new Broker
            {
                Code = PoliciesTestData.BrokerCode,
                Name = PoliciesTestData.BrokerName,
                Email = PoliciesTestData.BrokerEmail,
                Phone = PoliciesTestData.BrokerPhone,
                Status = BrokerStatus.Active,
                UserId = "test-user-1"
            });
        }

        if (!db.Currencies.Any())
        {
            db.Currencies.Add(new Currency
            {
                Code = PoliciesTestData.CurrencyCode,
                Name = PoliciesTestData.CurrencyName,
                ExchangeRateToBase = PoliciesTestData.CurrencyExchangeRate,
                IsActive = true
            });
        }

        db.SaveChanges();
    }

    private static (int buildingId, int currencyId) SeedBrokerCurrencyClientAndBuilding(InsuranceDbContext db)
    {
        SeedBrokerAndCurrency(db);

        var client = new Client
        {
            ClientType = ClientType.Individual,
            Name = PoliciesTestData.ClientName,
            IdentificationNumber = PoliciesTestData.ClientIdentificationNumber,
            ContactInfo = new ContactInfo(
                PoliciesTestData.ClientEmail,
                PoliciesTestData.ClientPhone,
                new Address(PoliciesTestData.ClientStreet, PoliciesTestData.ClientNumber))
        };

        db.Clients.Add(client);
        db.SaveChanges();

        var building = new Building
        {
            Address = new Address(PoliciesTestData.BuildingStreet, PoliciesTestData.BuildingNumber),
            CityId = PoliciesTestData.BuildingCityId,
            ConstructionYear = PoliciesTestData.BuildingConstructionYear,
            BuildingType = PoliciesTestData.BuildingTypeValue,
            SurfaceArea = PoliciesTestData.BuildingSurfaceArea,
            InsuredValue = PoliciesTestData.BuildingInsuredValue,
            NumberOfFloors = PoliciesTestData.BuildingFloors,
            ClientId = client.Id
        };

        db.Buildings.Add(building);
        db.SaveChanges();

        var currencyId = db.Currencies.Select(c => c.Id).First();
        return (building.Id, currencyId);
    }

    private static int SeedPolicy(InsuranceDbContext db, PolicyStatus status)
    {
        var (buildingId, currencyId) = SeedBrokerCurrencyClientAndBuilding(db);
        var brokerId = db.Brokers.Select(b => b.Id).First();

        var policy = new Policy
        {
            BrokerId = brokerId,
            BuildingId = buildingId,
            CurrencyId = currencyId,
            Status = status,
            StartDate = PoliciesTestData.PolicyStartDate,
            EndDate = PoliciesTestData.PolicyEndDate,
            BasePremiumAmount = PoliciesTestData.PolicyBasePremium,
            CalculatedPremiumAmount = PoliciesTestData.PolicyCalculatedPremium,
            CreationDate = PoliciesTestData.PolicyCreationDate,
            LastUpdateDate = PoliciesTestData.PolicyLastUpdateDate
        };

        db.Policies.Add(policy);
        db.SaveChanges();

        return policy.Id;
    }
}
