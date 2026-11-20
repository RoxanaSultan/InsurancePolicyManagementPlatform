using FluentAssertions;
using InsuranceApp.IntegrationTests.Infrastructure;
using InsuranceApp.IntegrationTests.TestData;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace InsuranceApp.IntegrationTests.Tests;

public class BrokersControllerTests
{
    [Fact]
    public async Task GetAll_ShouldReturnList()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync("/api/admin/brokers?page=1&pageSize=20");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Object);
        doc.RootElement.GetProperty("items").ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Fact]
    public async Task GetById_WhenMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync($"/api/admin/brokers/{BrokerTestData.MissingId}");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateBroker_InvalidInput_ShouldReturn400()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PostAsJsonAsync("/api/admin/brokers", BrokerTestData.InvalidCreate());

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ActivateBroker_WhenMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PostAsync($"/api/admin/brokers/{BrokerTestData.MissingId}/activate", content: null);

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeactivateBroker_WhenMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PostAsync($"/api/admin/brokers/{BrokerTestData.MissingId}/deactivate", content: null);

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateBroker_ThenGetById_ShouldReturnBroker()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        var createReq = BrokerTestData.ValidCreate("BRK-100");

        // Act
        var createResp = await http.PostAsJsonAsync("/api/admin/brokers", createReq);

        // Assert
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var createJson = await createResp.Content.ReadAsStringAsync();
        using var createDoc = JsonDocument.Parse(createJson);
        var brokerId = createDoc.RootElement.GetProperty("id").GetInt32();

        var getResp = await http.GetAsync($"/api/admin/brokers/{brokerId}");
        getResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateBroker_WhenMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        var updateReq = BrokerTestData.ValidUpdate("BRK-200");

        // Act
        var resp = await http.PutAsJsonAsync($"/api/admin/brokers/{BrokerTestData.MissingId}", updateReq);

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateBroker_ThenUpdate_ShouldReturnOk()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        var createResp = await http.PostAsJsonAsync("/api/admin/brokers", BrokerTestData.ValidCreate("BRK-101"));
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var createJson = await createResp.Content.ReadAsStringAsync();
        using var createDoc = JsonDocument.Parse(createJson);
        var brokerId = createDoc.RootElement.GetProperty("id").GetInt32();

        // Act
        var updateResp = await http.PutAsJsonAsync($"/api/admin/brokers/{brokerId}", BrokerTestData.ValidUpdate("BRK-101"));

        // Assert
        updateResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateBroker_ThenActivate_ShouldReturnOk()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        var createResp =
            await http.PostAsJsonAsync("/api/admin/brokers", BrokerTestData.ValidCreateInactive("BRK-102"));

        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var createJson = await createResp.Content.ReadAsStringAsync();
        using var createDoc = JsonDocument.Parse(createJson);
        var brokerId = createDoc.RootElement.GetProperty("id").GetInt32();

        // Act
        var activateResp = await http.PostAsync($"/api/admin/brokers/{brokerId}/activate", content: null);

        // Assert
        activateResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateBroker_ThenDeactivate_ShouldReturnOk()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        var createResp =
            await http.PostAsJsonAsync("/api/admin/brokers", BrokerTestData.ValidCreate("BRK-103"));

        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var createJson = await createResp.Content.ReadAsStringAsync();
        using var createDoc = JsonDocument.Parse(createJson);
        var brokerId = createDoc.RootElement.GetProperty("id").GetInt32();

        // Act
        var deactivateResp = await http.PostAsync($"/api/admin/brokers/{brokerId}/deactivate", content: null);

        // Assert
        deactivateResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}