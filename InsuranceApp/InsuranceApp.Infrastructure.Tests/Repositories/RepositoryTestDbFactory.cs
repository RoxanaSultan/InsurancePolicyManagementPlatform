using InsuranceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public static class RepositoryTestDbFactory
{
    public static InsuranceDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<InsuranceDbContext>()
            .UseInMemoryDatabase($"test-db-{Guid.NewGuid()}")
            .Options;

        return new InsuranceDbContext(options);
    }
}
