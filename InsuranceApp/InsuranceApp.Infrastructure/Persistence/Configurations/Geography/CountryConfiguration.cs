using InsuranceApp.Domain.Entities.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceApp.Infrastructure.Persistence.Configurations.Geography;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.HasMany(c => c.Counties)
            .WithOne(co => co.Country)
            .HasForeignKey(co => co.CountryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new Country { Id = 1, Name = "Romania" },
            new Country { Id = 2, Name = "United States" },
            new Country { Id = 3, Name = "Germany" }
        );
    }
}
