using InsuranceApp.Domain.Entities.Building;
using InsuranceApp.Domain.Entities.Client;
using InsuranceApp.Domain.Entities.Geography;
using InsuranceApp.Domain.Entities.Metadata;
using InsuranceApp.Domain.Entities.Policy;
using InsuranceApp.Domain.Entities.User;
using InsuranceApp.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Infrastructure.Persistence;

public class InsuranceDbContext(DbContextOptions<InsuranceDbContext> options) : IdentityDbContext<IdentityUser, IdentityRole, string>(options)
{
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<County> Counties => Set<County>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Building> Buildings => Set<Building>();
    public DbSet<ClientIdentificationNumberAudit> ClientIdentificationNumberAudits => Set<ClientIdentificationNumberAudit>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<FeeConfiguration> FeeConfigurations => Set<FeeConfiguration>();
    public DbSet<RiskFactorConfiguration> RiskFactorConfigurations => Set<RiskFactorConfiguration>();
    public DbSet<Broker> Brokers => Set<Broker>();
    public DbSet<Policy> Policies => Set<Policy>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(InsuranceDbContext).Assembly);

        builder.Owned<ContactInfo>();
        builder.Owned<Address>();

        builder.Entity<RiskFactorConfiguration>()
            .ToTable(tb => tb.UseSqlOutputClause(false));
    }
}