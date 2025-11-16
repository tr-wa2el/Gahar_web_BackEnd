using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class CertificateHolderConfiguration : IEntityTypeConfiguration<CertificateHolder>
{
    public void Configure(EntityTypeBuilder<CertificateHolder> builder)
    {
  builder.ToTable("CertificateHolders");

        builder.HasIndex(ch => new { ch.CertificateId, ch.CreatedAt }).IsDescending(false, true);
        builder.HasIndex(ch => ch.HolderEmail);
        builder.HasIndex(ch => ch.RegistrationNumber);
   }
}
