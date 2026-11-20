using FluentAssertions;
using InsuranceApp.IntegrationTests.Infrastructure;
using InsuranceApp.IntegrationTests.TestData;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace InsuranceApp.IntegrationTests.Tests;

public class CurrenciesControllerTests
{
    [Fact]
    public async Task GetAll_ShouldReturnList()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync("/api/admin/currencies?page=1&pageSize=20");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Object);
        doc.RootElement.GetProperty("items").ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Fact]
    public async Task Create_InvalidInput_ShouldReturn400()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PostAsJsonAsync("/api/admin/currencies", CurrenciesTestData.InvalidCreate());

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_WhenMissing_ShouldReturn404()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PutAsJsonAsync(
            $"/api/admin/currencies/{CurrenciesTestData.MissingId}",
            CurrenciesTestData.ValidUpdate());

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ThenUpdate_ShouldSucceed()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var createResp = await http.PostAsJsonAsync("/api/admin/currencies", CurrenciesTestData.ValidCreate());

        // Assert
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var createJson = await createResp.Content.ReadAsStringAsync();
        using var createDoc = JsonDocument.Parse(createJson);
        var currencyId = createDoc.RootElement.GetProperty("id").GetInt32();

        var updateResp = await http.PutAsJsonAsync(
            $"/api/admin/currencies/{currencyId}",
            CurrenciesTestData.ValidUpdate());
        updateResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
