using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class ContentFieldValueConfiguration : IEntityTypeConfiguration<ContentFieldValue>
 {
        public void Configure(EntityTypeBuilder<ContentFieldValue> builder)
        {
  builder.ToTable("ContentFieldValues");

    builder.HasKey(fv => fv.Id);

     // Composite unique index
            builder.HasIndex(fv => new { fv.ContentId, fv.ContentTypeFieldId, fv.LanguageId })
  .IsUnique();

     // Properties
      builder.Property(fv => fv.FieldKey)
         .IsRequired()
        .HasMaxLength(100);

     builder.Property(fv => fv.CreatedAt)
   .HasDefaultValueSql("GETUTCDATE()");

     // Relationships configured in ContentConfiguration and ContentTypeFieldConfiguration
      }
    }
}
