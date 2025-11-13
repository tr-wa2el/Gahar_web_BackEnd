using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class PageAnalyticsConfiguration : IEntityTypeConfiguration<PageAnalytics>
{
    public void Configure(EntityTypeBuilder<PageAnalytics> builder)
    {
        builder.ToTable("PageAnalytics");

        builder.HasIndex(pa => pa.PageUrl);
        builder.HasIndex(pa => new { pa.EntityType, pa.EntityId });
        builder.HasIndex(pa => pa.LastAnalyzedAt).IsDescending(true);
  }
}
