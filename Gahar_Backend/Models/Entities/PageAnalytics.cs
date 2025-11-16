using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class PageAnalytics : BaseEntity
{
    [StringLength(500)]
    public string? PageUrl { get; set; }

    [StringLength(100)]
    public string? PageName { get; set; }

    public long PageViews { get; set; } = 0;

    public long UniqueVisitors { get; set; } = 0;

    public double AverageTimeOnPage { get; set; } = 0; // in seconds

    public double BounceRate { get; set; } = 0; // percentage

    public long ClickCount { get; set; } = 0;

    [StringLength(200)]
    public string? TopReferrer { get; set; }

    [StringLength(100)]
    public string? TopDevice { get; set; } // Desktop, Mobile, Tablet

    public DateTime? LastAnalyzedAt { get; set; }

    public int? ConversionCount { get; set; } = 0;

    public double? ConversionRate { get; set; } = 0;

    [StringLength(100)]
    public string? EntityType { get; set; }

    public int? EntityId { get; set; }
}
