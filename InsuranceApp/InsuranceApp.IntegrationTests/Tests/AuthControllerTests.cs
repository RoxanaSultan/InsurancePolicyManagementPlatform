using FluentAssertions;
using InsuranceApp.IntegrationTests.Infrastructure;
using InsuranceApp.IntegrationTests.TestData;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace InsuranceApp.IntegrationTests.Tests;

public class AuthControllerTests
{
    [Fact]
    public async Task Register_InvalidInput_ShouldReturn400()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PostAsJsonAsync("/api/auth/register", AuthTestData.RegisterInvalid());

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_WhenBrokerMissing_ShouldReturn409()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PostAsJsonAsync(
            "/api/auth/register",
            AuthTestData.Register(email: AuthTestData.BrokerMissingEmail, password: AuthTestData.ValidPassword));

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ShouldReturn401()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // Act
        var resp = await http.PostAsJsonAsync(
            "/api/auth/login",
            AuthTestData.Login(email: AuthTestData.MissingEmail, password: AuthTestData.WrongPassword));

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_ValidCredentials_ShouldReturnAccessToken()
    {
        // Arrange
        await using var sut = await TestHost.CreateAsync();
        var http = sut.Http;

        // 1) Create broker (register expects broker to exist)
        var createBrokerResp = await http.PostAsJsonAsync("/api/admin/brokers", AuthTestData.BrokerForAuth());
        createBrokerResp.StatusCode.Should().Be(HttpStatusCode.Created);

        // 2) Register user
        var registerResp = await http.PostAsJsonAsync(
            "/api/auth/register",
            AuthTestData.Register(email: AuthTestData.AuthBrokerEmail, password: AuthTestData.ValidPassword));

        registerResp.StatusCode.Should().Be(HttpStatusCode.OK);

        // 3) Login
        var loginReq = AuthTestData.Login(email: AuthTestData.AuthBrokerEmail, password: AuthTestData.ValidPassword);

        // Act
        var loginResp = await http.PostAsJsonAsync("/api/auth/login", loginReq);

        // Assert
        loginResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await loginResp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        doc.RootElement.TryGetProperty("accessToken", out var tokenElement).Should().BeTrue();
        tokenElement.GetString().Should().NotBeNullOrWhiteSpace();
    }
}