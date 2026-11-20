using InsuranceApp.Domain.Entities.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceApp.Infrastructure.Persistence.Configurations.Geography;

public class CountyConfiguration : IEntityTypeConfiguration<County>
{
    public void Configure(EntityTypeBuilder<County> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.HasMany(c => c.Cities)
            .WithOne(ci => ci.County)
            .HasForeignKey(ci => ci.CountyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
           // Romania (CountryId = 1)
           new County { Id = 1, Name = "Cluj", Code = "CJ", CountryId = 1 },
           new County { Id = 2, Name = "Timis", Code = "TM", CountryId = 1 },

           // United States (CountryId = 2)
           new County { Id = 3, Name = "Los Angeles County", Code = "LA", CountryId = 2 },
           new County { Id = 4, Name = "Cook County", Code = "CK", CountryId = 2 },

           // Germany (CountryId = 3)
           new County { Id = 5, Name = "Bavaria", Code = "BY", CountryId = 3 }
       );
    }
}
