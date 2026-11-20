using InsuranceApp.API;
using InsuranceApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceApp.IntegrationTests.Infrastructure;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName;

    public ApiFactory(string dbName)
    {
        _dbName = dbName;
        EnsureJwtTestSettings();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "TEST_TEST_TEST_TEST_TEST_TEST_TEST_TEST",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience"
            });
        });

        builder.UseContentRoot(FindProjectPath("InsuranceApp.API"));

        builder.ConfigureServices(services =>
        {
            var descriptorsToRemove = services
                .Where(d =>
                    d.ServiceType == typeof(InsuranceDbContext) ||
                    d.ServiceType == typeof(DbContextOptions<InsuranceDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    (d.ServiceType.IsGenericType &&
                     d.ServiceType.GetGenericArguments().Length == 1 &&
                     d.ServiceType.GetGenericArguments()[0] == typeof(InsuranceDbContext) &&
                     (d.ServiceType.Name.Contains("IDbContextOptionsConfiguration") ||
                      d.ServiceType.FullName!.Contains("IDbContextOptionsConfiguration"))))
                .ToList();

            foreach (var d in descriptorsToRemove)
                services.Remove(d);

            var sqlServerRelated = services
                .Where(d =>
                    d.ImplementationType?.Assembly.GetName().Name == "Microsoft.EntityFrameworkCore.SqlServer" ||
                    d.ServiceType.Assembly.GetName().Name == "Microsoft.EntityFrameworkCore.SqlServer")
                .ToList();

            foreach (var d in sqlServerRelated)
                services.Remove(d);

            services.AddDbContext<InsuranceDbContext>(options =>
                options.UseInMemoryDatabase(_dbName));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.SchemeName, _ => { });

            using var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            TestDbSeeder.Seed(db);
        });
    }

    private static void EnsureJwtTestSettings()
    {
        Environment.SetEnvironmentVariable("Jwt__Key", "TEST_TEST_TEST_TEST_TEST_TEST_TEST_TEST");
        Environment.SetEnvironmentVariable("Jwt__Issuer", "TestIssuer");
        Environment.SetEnvironmentVariable("Jwt__Audience", "TestAudience");
    }

    private static string FindProjectPath(string projectFolderName)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, projectFolderName);
            if (Directory.Exists(candidate))
                return candidate;

            var nestedCandidate = Directory.GetDirectories(dir.FullName, projectFolderName, SearchOption.AllDirectories)
                .FirstOrDefault();
            if (nestedCandidate is not null)
                return nestedCandidate;

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException(
            $"Could not locate '{projectFolderName}' starting from '{AppContext.BaseDirectory}'.");
    }
}
