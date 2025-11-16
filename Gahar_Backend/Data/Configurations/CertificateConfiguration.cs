using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
  builder.ToTable("Certificates");

        builder.HasIndex(c => c.Slug).IsUnique();

        builder.HasOne(c => c.Author)
        .WithMany()
   .HasForeignKey(c => c.AuthorId)
     .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(c => c.Categories)
            .WithOne(cc => cc.Certificate)
       .HasForeignKey(cc => cc.CertificateId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasMany(c => c.Requirements)
 .WithOne(cr => cr.Certificate)
    .HasForeignKey(cr => cr.CertificateId)
            .OnDelete(DeleteBehavior.Cascade);

  builder.HasMany(c => c.Holders)
            .WithOne(ch => ch.Certificate)
     .HasForeignKey(ch => ch.CertificateId)
       .OnDelete(DeleteBehavior.Cascade);
    }
}
