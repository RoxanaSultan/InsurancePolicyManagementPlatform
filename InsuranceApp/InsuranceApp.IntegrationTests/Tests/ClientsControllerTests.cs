using FluentAssertions;
using InsuranceApp.IntegrationTests.Infrastructure;
using InsuranceApp.IntegrationTests.TestData;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace InsuranceApp.IntegrationTests.Tests;

public class ClientsControllerTests
{
    [Fact]
    public async Task GetClients_ShouldReturnList()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync("/api/brokers/clients?name=Ion&page=1&pageSize=20");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Fact]
    public async Task GetClientById_WhenMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync($"/api/brokers/clients/{ClientsTestData.MissingId}");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateClient_ShouldReturn201()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PostAsJsonAsync("/api/brokers/clients", ClientsTestData.ValidCreate());

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateClient_InvalidInput_ShouldReturn400()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PostAsJsonAsync("/api/brokers/clients", ClientsTestData.InvalidCreate());

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateClient_ThenGetById_ShouldReturnClient()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var createResp = await http.PostAsJsonAsync("/api/brokers/clients", ClientsTestData.ValidCreate("ion2@test.com"));

        // Assert
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var json = await createResp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var clientId = doc.RootElement.GetProperty("id").GetInt32();

        var getResp = await http.GetAsync($"/api/brokers/clients/{clientId}");
        getResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateClient_WhenMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PutAsJsonAsync($"/api/brokers/clients/{ClientsTestData.MissingId}", ClientsTestData.ValidUpdate());

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateClient_ThenUpdate_ShouldReturnOk()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        var createResp = await http.PostAsJsonAsync("/api/brokers/clients", ClientsTestData.ValidCreate("ion3@test.com"));
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var createJson = await createResp.Content.ReadAsStringAsync();
        using var createDoc = JsonDocument.Parse(createJson);
        var clientId = createDoc.RootElement.GetProperty("id").GetInt32();

        // Act
        var updateResp = await http.PutAsJsonAsync(
            $"/api/brokers/clients/{clientId}",
            ClientsTestData.ValidUpdate("ion.updated@test.com"));

        // Assert
        updateResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetClientAudit_WhenMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync($"/api/brokers/clients/{ClientsTestData.MissingId}/audit");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateClient_ThenGetAudit_ShouldReturnList()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        var createResp = await http.PostAsJsonAsync("/api/brokers/clients", ClientsTestData.ValidCreate("ion4@test.com"));
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var createJson = await createResp.Content.ReadAsStringAsync();
        using var createDoc = JsonDocument.Parse(createJson);
        var clientId = createDoc.RootElement.GetProperty("id").GetInt32();

        // Act
        var auditResp = await http.GetAsync($"/api/brokers/clients/{clientId}/audit");

        // Assert
        auditResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var auditJson = await auditResp.Content.ReadAsStringAsync();
        using var auditDoc = JsonDocument.Parse(auditJson);
        auditDoc.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
    }
}
