using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class ContentTypeFieldConfiguration : IEntityTypeConfiguration<ContentTypeField>
    {
   public void Configure(EntityTypeBuilder<ContentTypeField> builder)
      {
     builder.ToTable("ContentTypeFields");

builder.HasKey(f => f.Id);

// Composite unique index
    builder.HasIndex(f => new { f.ContentTypeId, f.FieldKey })
         .IsUnique();

       // Properties
          builder.Property(f => f.Name)
       .IsRequired()
       .HasMaxLength(100);

       builder.Property(f => f.FieldKey)
           .IsRequired()
       .HasMaxLength(100);

    builder.Property(f => f.FieldType)
   .IsRequired()
         .HasMaxLength(50);

 builder.Property(f => f.Description)
        .HasMaxLength(500);

       builder.Property(f => f.IsRequired)
     .HasDefaultValue(false);

       builder.Property(f => f.IsTranslatable)
       .HasDefaultValue(true);

     builder.Property(f => f.ShowInList)
        .HasDefaultValue(true);

     builder.Property(f => f.Placeholder)
        .HasMaxLength(500);

     builder.Property(f => f.CreatedAt)
      .HasDefaultValueSql("GETUTCDATE()");

            // Relationship configured in ContentTypeConfiguration
        }
    }
}
