using FluentAssertions;
using InsuranceApp.IntegrationTests.Infrastructure;
using InsuranceApp.IntegrationTests.TestData;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace InsuranceApp.IntegrationTests.Tests;

public class ReportsControllerTests
{
    private static async Task<JsonElement> ReadRootAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.Clone();
    }

    [Theory]
    [InlineData("/api/admin/reports/policies-by-country")]
    [InlineData("/api/admin/reports/policies-by-county")]
    [InlineData("/api/admin/reports/policies-by-city")]
    [InlineData("/api/admin/reports/policies-by-broker")]
    public async Task Reports_WhenValid_ShouldReturnReportWithPagedItems(string endpoint)
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync($"{endpoint}{ReportsTestData.BaseQuery}");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
        var root = await ReadRootAsync(resp);

        root.ValueKind.Should().Be(JsonValueKind.Object);
        var reports = root.GetProperty("reports");
        reports.ValueKind.Should().Be(JsonValueKind.Object);
        reports.GetProperty("items").ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Theory]
    [InlineData("/api/admin/reports/policies-by-country")]
    [InlineData("/api/admin/reports/policies-by-county")]
    [InlineData("/api/admin/reports/policies-by-city")]
    [InlineData("/api/admin/reports/policies-by-broker")]
    public async Task Reports_WhenMissingDates_ShouldReturn400(string endpoint)
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync($"{endpoint}{ReportsTestData.MissingDatesQuery}");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("/api/admin/reports/policies-by-country")]
    [InlineData("/api/admin/reports/policies-by-county")]
    [InlineData("/api/admin/reports/policies-by-city")]
    [InlineData("/api/admin/reports/policies-by-broker")]
    public async Task Reports_WhenStatusInvalid_ShouldReturn400(string endpoint)
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.GetAsync($"{endpoint}{ReportsTestData.InvalidStatusQuery}");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
