using InsuranceApp.Application.DTOs.Clients;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Domain.Entities.Client;
using InsuranceApp.Domain.ValueObjects;
using InsuranceApp.Infrastructure.Persistence.Repositories.Client;

namespace InsuranceApp.Infrastructure.Tests.Repositories;

public class ClientRepositoryTests
{
    [Fact]
    public async Task SearchAsync_ByName_ShouldReturnMatch()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new ClientRepository(db);

        db.Clients.AddRange(
            new Client
            {
                Name = "Ion Popescu",
                IdentificationNumber = "1234567890123",
                ContactInfo = new ContactInfo("ion@test.com", "0712345678", new Address("Str", "1"))
            },
            new Client
            {
                Name = "Maria Ionescu",
                IdentificationNumber = "1234567890124",
                ContactInfo = new ContactInfo("maria@test.com", "0712345679", new Address("Str", "2"))
            });
        await db.SaveChangesAsync();

        var result = await repo.SearchAsync(
            new ClientSearchRequest { Name = "Ion Popescu" },
            new PageRequest { Page = 1, PageSize = 20 });

        Assert.Single(result);
        Assert.Equal("Ion Popescu", result[0].Name);
    }

    [Fact]
    public async Task IsEmailUniqueAsync_ShouldReturnFalseWhenExists()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new ClientRepository(db);

        var client = new Client
        {
            Name = "Ion",
            IdentificationNumber = "1234567890123",
            ContactInfo = new ContactInfo("dup@test.com", "0712345678", new Address("Str", "1"))
        };
        db.Clients.Add(client);
        await db.SaveChangesAsync();

        var isUnique = await repo.IsEmailUniqueAsync("dup@test.com");

        Assert.False(isUnique);
    }

    [Fact]
    public async Task GetIdentificationNumberAuditAsync_ShouldReturnOrdered()
    {
        using var db = RepositoryTestDbFactory.CreateContext();
        var repo = new ClientRepository(db);

        var client = new Client
        {
            Name = "Ion",
            IdentificationNumber = "1234567890123",
            ContactInfo = new ContactInfo("ion@test.com", "0712345678", new Address("Str", "1"))
        };
        db.Clients.Add(client);
        await db.SaveChangesAsync();

        await repo.AddIdentificationNumberAuditAsync(new ClientIdentificationNumberAudit
        {
            ClientId = client.Id,
            OldIdentificationNumber = "111",
            NewIdentificationNumber = "222",
            ChangedAt = new DateTime(2024, 1, 1)
        });
        await repo.AddIdentificationNumberAuditAsync(new ClientIdentificationNumberAudit
        {
            ClientId = client.Id,
            OldIdentificationNumber = "222",
            NewIdentificationNumber = "333",
            ChangedAt = new DateTime(2024, 2, 1)
        });
        await db.SaveChangesAsync();

        var audits = await repo.GetIdentificationNumberAuditAsync(client.Id, CancellationToken.None);

        Assert.Equal(2, audits.Count());
        Assert.Equal("333", audits.First().NewIdentificationNumber);
    }
}
