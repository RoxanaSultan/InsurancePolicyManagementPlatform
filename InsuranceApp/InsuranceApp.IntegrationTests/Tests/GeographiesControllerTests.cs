using FluentAssertions;
using InsuranceApp.IntegrationTests.Infrastructure;
using InsuranceApp.IntegrationTests.TestData;
using System.Net;
using System.Text.Json;

namespace InsuranceApp.IntegrationTests.Tests;

public class GeographiesControllerTests
{
    [Fact]
    public async Task GetCountries_ShouldReturnList()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync("/api/brokers/countries?page=1&pageSize=20");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Object);
        var items = doc.RootElement.GetProperty("items");
        items.ValueKind.Should().Be(JsonValueKind.Array);
        items.GetArrayLength().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetCounties_ByCountryId_ShouldReturnList()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync(
            $"/api/brokers/countries/{GeographiesTestData.CountryId}/counties?page=1&pageSize=20");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Object);
        var items = doc.RootElement.GetProperty("items");
        items.ValueKind.Should().Be(JsonValueKind.Array);
        items.GetArrayLength().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetCities_ByCountyId_ShouldReturnList()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync(
            $"/api/brokers/counties/{GeographiesTestData.CountyId}/cities?page=1&pageSize=20");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Object);
        var items = doc.RootElement.GetProperty("items");
        items.ValueKind.Should().Be(JsonValueKind.Array);
        items.GetArrayLength().Should().BeGreaterThan(0);
    }
}
