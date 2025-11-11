using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
  {
   builder.ToTable("Languages");

    builder.HasKey(l => l.Id);

    builder.HasIndex(l => l.Code)
      .IsUnique();

    builder.Property(l => l.Code)
    .IsRequired()
       .HasMaxLength(5);

       builder.Property(l => l.Name)
         .IsRequired()
  .HasMaxLength(100);

       builder.Property(l => l.IsDefault)
  .HasDefaultValue(false);

  builder.Property(l => l.IsActive)
     .HasDefaultValue(true);

   builder.Property(l => l.Direction)
  .HasMaxLength(10)
 .HasDefaultValue("ltr");

            builder.Property(l => l.CreatedAt)
     .HasDefaultValueSql("GETUTCDATE()");

     // Relationships
       builder.HasMany(l => l.Translations)
    .WithOne(t => t.Language)
     .HasForeignKey(t => t.LanguageId)
       .OnDelete(DeleteBehavior.Cascade);
 }
    }
}
