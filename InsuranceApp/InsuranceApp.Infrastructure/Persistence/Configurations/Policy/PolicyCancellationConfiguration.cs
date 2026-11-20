using InsuranceApp.Domain.Entities.Policy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceApp.Infrastructure.Persistence.Configurations.Policy;

public class PolicyCancellationConfiguration : IEntityTypeConfiguration<PolicyCancellation>
{
    public void Configure(EntityTypeBuilder<PolicyCancellation> builder)
    {
        builder.ToTable("PolicyCancellation");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CancellationDate)
            .IsRequired();

        builder.Property(x => x.CancellationReason)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.PolicyId)
            .IsRequired();

        builder.HasIndex(x => x.PolicyId)
            .IsUnique();
    }
}