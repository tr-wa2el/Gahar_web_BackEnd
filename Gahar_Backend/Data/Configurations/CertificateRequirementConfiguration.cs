using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class CertificateRequirementConfiguration : IEntityTypeConfiguration<CertificateRequirement>
{
    public void Configure(EntityTypeBuilder<CertificateRequirement> builder)
  {
    builder.ToTable("CertificateRequirements");

   builder.HasIndex(cr => new { cr.CertificateId, cr.DisplayOrder });
   }
}
