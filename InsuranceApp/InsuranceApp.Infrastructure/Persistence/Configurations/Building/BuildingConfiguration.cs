using BuildingEntity = InsuranceApp.Domain.Entities.Building.Building;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceApp.Infrastructure.Persistence.Configurations.Building;

public class BuildingConfiguration : IEntityTypeConfiguration<BuildingEntity>
{
    public void Configure(EntityTypeBuilder<BuildingEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Address_Street");

            address.Property(a => a.Number)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("Address_Number");
        });

        builder.Navigation(x => x.Address)
            .IsRequired();

        builder.Property(x => x.SurfaceArea)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.InsuredValue)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.BuildingType)
            .IsRequired();

        builder.HasOne(x => x.Client)
            .WithMany(x => x.Buildings)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.City)
            .WithMany(x => x.Buildings)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Policies)
            .WithOne(p => p.Building)
            .HasForeignKey(p => p.BuildingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.CityId, x.ClientId });
    }
}