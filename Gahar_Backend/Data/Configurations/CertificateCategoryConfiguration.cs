using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class CertificateCategoryConfiguration : IEntityTypeConfiguration<CertificateCategory>
{
    public void Configure(EntityTypeBuilder<CertificateCategory> builder)
    {
     builder.ToTable("CertificateCategories");

        builder.HasIndex(cc => new { cc.CertificateId, cc.DisplayOrder });
    }
}
