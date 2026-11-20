using ClientEntity = InsuranceApp.Domain.Entities.Client.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceApp.Infrastructure.Persistence.Configurations.Client;

public class ClientConfiguration : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.IdentificationNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.IdentificationNumber)
            .IsUnique();

        builder.Property(x => x.ClientType)
            .IsRequired();

        builder.OwnsOne(x => x.ContactInfo, ci =>
        {
            ci.Property(p => p.Email)
                .HasMaxLength(254)
                .HasColumnName("ContactInfo_Email");

            ci.Property(p => p.PhoneNumber)
                .HasMaxLength(30)
                .HasColumnName("ContactInfo_PhoneNumber");

            // Address = owned type în owned type
            ci.OwnsOne(p => p.Address, a =>
            {
                a.Property(x => x.Street)
                    .HasMaxLength(200)
                    .HasColumnName("ContactInfo_Address_Street");

                a.Property(x => x.Number)
                    .HasMaxLength(20)
                    .HasColumnName("ContactInfo_Address_Number");
            });
        });

        builder.Navigation(x => x.ContactInfo)
            .IsRequired();
    }
}