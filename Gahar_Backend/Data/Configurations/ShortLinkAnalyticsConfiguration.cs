using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

/// <summary>
/// Configuration for ShortLinkAnalytics entity
/// </summary>
public class ShortLinkAnalyticsConfiguration : IEntityTypeConfiguration<ShortLinkAnalytics>
{
    public void Configure(EntityTypeBuilder<ShortLinkAnalytics> builder)
    {
        builder.ToTable("ShortLinkAnalytics");

        builder.HasKey(a => a.Id);

    // Index on ShortLinkId
        builder.HasIndex(a => a.ShortLinkId)
   .HasDatabaseName("IX_ShortLinkAnalytics_ShortLinkId");

  // Index on ClickTime for analytics queries
        builder.HasIndex(a => a.ClickTime)
       .IsDescending(true)
     .HasDatabaseName("IX_ShortLinkAnalytics_ClickTime");

        // Index on Country for geographic analytics
        builder.HasIndex(a => a.Country)
            .HasDatabaseName("IX_ShortLinkAnalytics_Country");

        // Index on DeviceType for device analytics
    builder.HasIndex(a => a.DeviceType)
            .HasDatabaseName("IX_ShortLinkAnalytics_DeviceType");

        // Composite index for common queries
        builder.HasIndex(a => new { a.ShortLinkId, a.ClickTime })
  .IsDescending(false, true)
    .HasDatabaseName("IX_ShortLinkAnalytics_ShortLinkId_ClickTime");

     // Foreign key relationship
builder.HasOne(a => a.ShortLink)
  .WithMany(s => s.Analytics)
.HasForeignKey(a => a.ShortLinkId)
   .OnDelete(DeleteBehavior.Cascade);
    }
}
