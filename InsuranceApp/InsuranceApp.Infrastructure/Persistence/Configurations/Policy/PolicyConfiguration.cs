using InsuranceApp.Domain.Entities.Policy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PolicyEntity = InsuranceApp.Domain.Entities.Policy.Policy;

namespace InsuranceApp.Infrastructure.Persistence.Configurations.Policy;

public class PolicyConfiguration : IEntityTypeConfiguration<PolicyEntity>
{
    public void Configure(EntityTypeBuilder<PolicyEntity> builder)
    {
        builder.ToTable("Policy", t =>
        {
            t.HasCheckConstraint(
                "CK_Policy_EndDate_After_StartDate",
                "[EndDate] > [StartDate]"
            );

            t.HasCheckConstraint(
                "CK_Policy_LastUpdateDate_After_CreationDate",
                "[LastUpdateDate] > [CreationDate]"
            );
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.EndDate)
            .IsRequired();

        builder.Property(x => x.BasePremiumAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.CalculatedPremiumAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.CreationDate)
            .IsRequired();

        builder.Property(x => x.LastUpdateDate)
            .IsRequired();

        builder.HasOne(x => x.Building)
            .WithMany(b => b.Policies)
            .HasForeignKey(x => x.BuildingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Broker)
            .WithMany(b => b.Policies)
            .HasForeignKey(x => x.BrokerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Currency)
            .WithMany(c => c.Policies)
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.BuildingId, x.Status });
        builder.HasIndex(x => new { x.BrokerId, x.StartDate, x.EndDate });
    }
}