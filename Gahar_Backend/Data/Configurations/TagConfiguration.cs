using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
     public void Configure(EntityTypeBuilder<Tag> builder)
        {
   builder.ToTable("Tags");

 builder.HasKey(t => t.Id);

  // Unique constraint on slug
 builder.HasIndex(t => t.Slug)
      .IsUnique();

 // Index on UsageCount for sorting
       builder.HasIndex(t => t.UsageCount);

     // Properties
       builder.Property(t => t.Name)
       .IsRequired()
      .HasMaxLength(100);

        builder.Property(t => t.Slug)
      .IsRequired()
        .HasMaxLength(100);

          builder.Property(t => t.Description)
  .HasMaxLength(500);

     builder.Property(t => t.Color)
      .HasMaxLength(20);

 builder.Property(t => t.UsageCount)
    .HasDefaultValue(0);

  builder.Property(t => t.CreatedAt)
     .HasDefaultValueSql("GETUTCDATE()");

   // Relationships
  builder.HasMany(t => t.ContentTags)
  .WithOne(ct => ct.Tag)
     .HasForeignKey(ct => ct.TagId)
      .OnDelete(DeleteBehavior.Cascade);
  }
    }
}
