using FluentAssertions;
using InsuranceApp.Infrastructure.Persistence;
using InsuranceApp.IntegrationTests.Infrastructure;
using InsuranceApp.IntegrationTests.TestData;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace InsuranceApp.IntegrationTests.Tests;

public class BuildingsControllerTests
{
    [Fact]
    public async Task GetBuildingsByClientId_WhenClientMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync(
            $"/api/brokers/clients/{BuildingsTestData.MissingId}/buildings?pageNumber=1&pageSize=20");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetBuildingById_WhenMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync($"/api/brokers/buildings/{BuildingsTestData.MissingId}");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateBuilding_WhenInsuredValueNegative_ShouldReturn400()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PostAsJsonAsync("/api/brokers/clients/1/buildings", BuildingsTestData.CreateBuilding_InvalidInsuredValue());

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateBuilding_ClientNotFound_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        var missingClientId = BuildingsTestData.MissingId;

        // Act
        var resp = await http.PostAsJsonAsync(
            $"/api/brokers/clients/{missingClientId}/buildings",
            BuildingsTestData.CreateBuilding_ValidForMissingClient());

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateClient_ThenCreateBuilding_ShouldPersistAndLink()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act - create client
        var clientResp = await http.PostAsJsonAsync("/api/brokers/clients", ClientsTestData.CreateIndividualClient());
        clientResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var clientJson = await clientResp.Content.ReadAsStringAsync();
        using var clientDoc = JsonDocument.Parse(clientJson);
        var clientId = clientDoc.RootElement.GetProperty("id").GetInt32();

        // Act - create building for client
        var buildingResp = await http.PostAsJsonAsync(
            $"/api/brokers/clients/{clientId}/buildings",
            BuildingsTestData.CreateBuilding_Valid());

        buildingResp.StatusCode.Should().Be(HttpStatusCode.Created);

        // Assert - verify persistence
        using var scope = sut.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();

        db.Clients.Should().ContainSingle(c => c.Id == clientId);
        db.Buildings.Should().ContainSingle(b => b.ClientId == clientId);
    }

    [Fact]
    public async Task GetBuildingsByClientId_AfterCreate_ShouldReturnList()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // create client
        var clientResp = await http.PostAsJsonAsync("/api/brokers/clients", ClientsTestData.CreateIndividualClient());
        clientResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var clientJson = await clientResp.Content.ReadAsStringAsync();
        using var clientDoc = JsonDocument.Parse(clientJson);
        var clientId = clientDoc.RootElement.GetProperty("id").GetInt32();

        // create building
        var createBuildingResp = await http.PostAsJsonAsync(
            $"/api/brokers/clients/{clientId}/buildings",
            BuildingsTestData.CreateBuilding_Valid());

        createBuildingResp.StatusCode.Should().Be(HttpStatusCode.Created);

        // Act
        var listResp = await http.GetAsync(
            $"/api/brokers/clients/{clientId}/buildings?pageNumber=1&pageSize=50");

        // Assert
        listResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var listJson = await listResp.Content.ReadAsStringAsync();
        using var listDoc = JsonDocument.Parse(listJson);

        listDoc.RootElement.ValueKind.Should().Be(JsonValueKind.Object);

        var items = listDoc.RootElement.GetProperty("items");
        items.ValueKind.Should().Be(JsonValueKind.Array);
        items.GetArrayLength().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateBuilding_ThenGetById_ShouldReturnOk()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // create client
        var clientResp = await http.PostAsJsonAsync("/api/brokers/clients", ClientsTestData.CreateIndividualClient());
        clientResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var clientJson = await clientResp.Content.ReadAsStringAsync();
        using var clientDoc = JsonDocument.Parse(clientJson);
        var clientId = clientDoc.RootElement.GetProperty("id").GetInt32();

        // create building
        var createResp = await http.PostAsJsonAsync(
            $"/api/brokers/clients/{clientId}/buildings",
            BuildingsTestData.CreateBuilding_Valid());

        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var buildingJson = await createResp.Content.ReadAsStringAsync();
        using var buildingDoc = JsonDocument.Parse(buildingJson);
        var buildingId = buildingDoc.RootElement.GetProperty("id").GetInt32();

        // Act
        var getResp = await http.GetAsync($"/api/brokers/buildings/{buildingId}");

        // Assert
        getResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateBuilding_ThenUpdate_ShouldReturnOk()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // create client
        var clientResp = await http.PostAsJsonAsync("/api/brokers/clients", ClientsTestData.CreateIndividualClient());
        clientResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var clientJson = await clientResp.Content.ReadAsStringAsync();
        using var clientDoc = JsonDocument.Parse(clientJson);
        var clientId = clientDoc.RootElement.GetProperty("id").GetInt32();

        // create building
        var createResp = await http.PostAsJsonAsync(
            $"/api/brokers/clients/{clientId}/buildings",
            BuildingsTestData.CreateBuilding_Valid());

        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var buildingJson = await createResp.Content.ReadAsStringAsync();
        using var buildingDoc = JsonDocument.Parse(buildingJson);
        var buildingId = buildingDoc.RootElement.GetProperty("id").GetInt32();

        // Act
        var updateResp = await http.PutAsJsonAsync(
            $"/api/brokers/buildings/{buildingId}",
            BuildingsTestData.UpdateBuilding_Valid());

        // Assert
        updateResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}