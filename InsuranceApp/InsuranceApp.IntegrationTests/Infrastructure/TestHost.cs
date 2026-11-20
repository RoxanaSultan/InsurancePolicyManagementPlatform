using System.Net.Http;

namespace InsuranceApp.IntegrationTests.Infrastructure;

public sealed class TestHost : IAsyncDisposable
{
    public ApiFactory Factory { get; }
    public HttpClient Http { get; }

    private TestHost(ApiFactory factory)
    {
        Factory = factory;
        Http = factory.CreateClient();
    }

    public static Task<TestHost> CreateAsync()
    {
        var factory = new ApiFactory($"db_{Guid.NewGuid()}");
        return Task.FromResult(new TestHost(factory));
    }

    public async ValueTask DisposeAsync()
    {
        Http.Dispose();
        await Factory.DisposeAsync();
    }
}