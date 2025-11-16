using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

/// <summary>
/// Configuration for ShortLink entity
/// </summary>
public class ShortLinkConfiguration : IEntityTypeConfiguration<ShortLink>
{
    public void Configure(EntityTypeBuilder<ShortLink> builder)
    {
        builder.ToTable("ShortLinks");

        builder.HasKey(s => s.Id);

        // Unique constraint on ShortCode
      builder.HasIndex(s => s.ShortCode)
     .IsUnique()
   .HasDatabaseName("IX_ShortLinks_ShortCode_Unique");

  // Index on OriginalUrl for search
        builder.HasIndex(s => s.OriginalUrl)
            .HasDatabaseName("IX_ShortLinks_OriginalUrl");

        // Index on CreatedByUserId for user's links
        builder.HasIndex(s => s.CreatedByUserId)
            .HasDatabaseName("IX_ShortLinks_CreatedByUserId");

        // Index on IsActive for active links query
    builder.HasIndex(s => s.IsActive)
            .HasDatabaseName("IX_ShortLinks_IsActive");

        // Index on Category for category filtering
        builder.HasIndex(s => s.Category)
            .HasDatabaseName("IX_ShortLinks_Category");

        // Index on ExpiryDate for checking expired links
      builder.HasIndex(s => s.ExpiryDate)
     .HasDatabaseName("IX_ShortLinks_ExpiryDate");

        // Index on CreatedAt for sorting
        builder.HasIndex(s => s.CreatedAt)
            .IsDescending(true)
    .HasDatabaseName("IX_ShortLinks_CreatedAt");

        // Index on LastAccessedAt for analytics
        builder.HasIndex(s => s.LastAccessedAt)
            .IsDescending(true)
 .HasDatabaseName("IX_ShortLinks_LastAccessedAt");

      // Foreign key
        builder.HasOne(s => s.CreatedByUser)
            .WithMany()
   .HasForeignKey(s => s.CreatedByUserId)
    .OnDelete(DeleteBehavior.SetNull);

 // One-to-Many relationship
        builder.HasMany(s => s.Analytics)
.WithOne(a => a.ShortLink)
         .HasForeignKey(a => a.ShortLinkId)
            .OnDelete(DeleteBehavior.Cascade);
  }
}
