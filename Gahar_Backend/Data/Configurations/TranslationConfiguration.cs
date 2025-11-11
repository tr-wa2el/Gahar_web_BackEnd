using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class TranslationConfiguration : IEntityTypeConfiguration<Translation>
{
 public void Configure(EntityTypeBuilder<Translation> builder)
  {
  builder.ToTable("Translations");

    builder.HasKey(t => t.Id);

       builder.HasIndex(t => new { t.EntityType, t.EntityId, t.FieldName, t.LanguageId })
    .IsUnique();

  builder.Property(t => t.EntityType)
     .IsRequired()
 .HasMaxLength(100);

         builder.Property(t => t.EntityId)
         .IsRequired();

      builder.Property(t => t.FieldName)
    .IsRequired()
   .HasMaxLength(50);

builder.Property(t => t.Value)
    .IsRequired();

            builder.Property(t => t.CreatedAt)
      .HasDefaultValueSql("GETUTCDATE()");

// Relationships
   builder.HasOne(t => t.Language)
    .WithMany(l => l.Translations)
     .HasForeignKey(t => t.LanguageId)
      .OnDelete(DeleteBehavior.Cascade);
  }
    }
}
