using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class AnalyticsEventConfiguration : IEntityTypeConfiguration<AnalyticsEvent>
{
    public void Configure(EntityTypeBuilder<AnalyticsEvent> builder)
    {
        builder.ToTable("AnalyticsEvents");

builder.HasIndex(ae => ae.EventType);
        builder.HasIndex(ae => ae.PageUrl);
 builder.HasIndex(ae => ae.SessionId);
        builder.HasIndex(ae => ae.CreatedAt).IsDescending(true);
    builder.HasIndex(ae => new { ae.UserId, ae.CreatedAt }).IsDescending(false, true);
    }
}
