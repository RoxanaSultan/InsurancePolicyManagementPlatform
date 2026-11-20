using InsuranceApp.Domain.Entities.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceApp.Infrastructure.Persistence.Configurations.Metadata;

public class RiskFactorConfigurationConfiguration : IEntityTypeConfiguration<RiskFactorConfiguration>
{
    public void Configure(EntityTypeBuilder<RiskFactorConfiguration> builder)
    {
        builder.ToTable("RiskFactorConfiguration");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Level).IsRequired();
        builder.Property(x => x.ReferenceId).IsRequired();
        builder.Property(x => x.AdjustmentPercentage)
               .HasPrecision(6, 2)
               .IsRequired();
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasIndex(x => new { x.Level, x.ReferenceId })
               .IsUnique()
               .HasFilter("[IsActive] = 1");

        builder.HasIndex(x => x.Level);
        builder.HasIndex(x => x.IsActive);
    }
}