using InsuranceApp.Domain.Entities.Client;
using ClientEntity = InsuranceApp.Domain.Entities.Client.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceApp.Infrastructure.Persistence.Configurations.Client;

public class ClientIdentificationAuditConfiguration : IEntityTypeConfiguration<ClientIdentificationNumberAudit>
{
    public void Configure(EntityTypeBuilder<ClientIdentificationNumberAudit> builder)
    {
        builder.ToTable("ClientIdentificationNumberAudit");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
               .IsRequired()
               .HasMaxLength(450);

        builder.Property(x => x.OldIdentificationNumber)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(x => x.NewIdentificationNumber)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(x => x.ChangedAt)
               .IsRequired();

        builder.HasIndex(x => x.ClientId);
        builder.HasOne<ClientEntity>()
               .WithMany()
               .HasForeignKey(x => x.ClientId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}