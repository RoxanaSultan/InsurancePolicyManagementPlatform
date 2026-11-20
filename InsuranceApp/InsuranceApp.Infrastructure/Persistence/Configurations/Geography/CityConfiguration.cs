using InsuranceApp.Domain.Entities.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceApp.Infrastructure.Persistence.Configurations.Geography;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(120);

        builder.HasIndex(x => new { x.CountyId, x.Name })
            .IsUnique();

        builder.HasOne(x => x.County)
            .WithMany(x => x.Cities)
            .HasForeignKey(x => x.CountyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            // Counties in Romania
            new City { Id = 1, Name = "Cluj-Napoca", CountyId = 1 },
            new City { Id = 2, Name = "Turda", CountyId = 1 },
            new City { Id = 3, Name = "Timisoara", CountyId = 2 },

            // US counties
            new City { Id = 4, Name = "Los Angeles", CountyId = 3 },
            new City { Id = 5, Name = "Chicago", CountyId = 4 },

            // Germany
            new City { Id = 6, Name = "Munich", CountyId = 5 }
        );
    }
}