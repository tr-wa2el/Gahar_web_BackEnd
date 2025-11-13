using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class ContentTypeConfiguration : IEntityTypeConfiguration<ContentType>
{
    public void Configure(EntityTypeBuilder<ContentType> builder)
    {
        builder.ToTable("ContentTypes");

        builder.HasIndex(ct => ct.Slug).IsUnique();
        builder.HasIndex(ct => new { ct.IsActive, ct.DisplayOrder });

        builder.HasMany(ct => ct.Fields)
   .WithOne(f => f.ContentType)
  .HasForeignKey(f => f.ContentTypeId)
  .OnDelete(DeleteBehavior.Cascade);

      builder.HasMany(ct => ct.Contents)
 .WithOne(c => c.ContentType)
     .HasForeignKey(c => c.ContentTypeId)
 .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ContentTypeFieldConfiguration : IEntityTypeConfiguration<ContentTypeField>
{
    public void Configure(EntityTypeBuilder<ContentTypeField> builder)
    {
    builder.ToTable("ContentTypeFields");

        builder.HasIndex(ctf => new { ctf.ContentTypeId, ctf.FieldKey }).IsUnique();
        builder.HasIndex(ctf => new { ctf.ContentTypeId, ctf.DisplayOrder });
    }
}
