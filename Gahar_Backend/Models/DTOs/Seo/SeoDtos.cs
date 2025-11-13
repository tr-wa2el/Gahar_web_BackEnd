using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Seo;

public class SeoMetadataDto
{
    public int Id { get; set; }
    public string PageTitle { get; set; } = string.Empty;
    public string? MetaDescription { get; set; }
  public string? MetaKeywords { get; set; }
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
    public string? CanonicalUrl { get; set; }
    public bool IsIndexable { get; set; }
    public bool IsFollowable { get; set; }
    public string? EntityType { get; set; }
    public int? EntityId { get; set; }
    public DateTime? LastOptimizedAt { get; set; }
    public int? SeoScore { get; set; }
    public string? Recommendations { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UpdateSeoMetadataDto
{
    [Required]
    [StringLength(200)]
    public string PageTitle { get; set; } = string.Empty;

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    [StringLength(500)]
    public string? MetaKeywords { get; set; }

    [StringLength(200)]
    public string? OgTitle { get; set; }

    [StringLength(500)]
    public string? OgDescription { get; set; }

    [StringLength(500)]
  public string? OgImage { get; set; }

    [StringLength(200)]
    public string? CanonicalUrl { get; set; }

    public bool IsIndexable { get; set; } = true;
    public bool IsFollowable { get; set; } = true;
}

public class PageAnalyticsDto
{
    public int Id { get; set; }
    public string? PageUrl { get; set; }
    public string? PageName { get; set; }
    public long PageViews { get; set; }
    public long UniqueVisitors { get; set; }
    public double AverageTimeOnPage { get; set; }
    public double BounceRate { get; set; }
  public long ClickCount { get; set; }
    public string? TopReferrer { get; set; }
    public string? TopDevice { get; set; }
    public DateTime? LastAnalyzedAt { get; set; }
    public int? ConversionCount { get; set; }
    public double? ConversionRate { get; set; }
}

public class AnalyticsEventDto
{
    public int Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string? EventData { get; set; }
    public string? PageUrl { get; set; }
    public string? UserAgent { get; set; }
    public string? CountryCode { get; set; }
  public string? CityName { get; set; }
    public string? DeviceType { get; set; }
    public string? BrowserName { get; set; }
    public string? SourceName { get; set; }
    public string? SessionId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class KeywordDto
{
    public int Id { get; set; }
    public string Term { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SearchVolume { get; set; }
    public int Difficulty { get; set; }
    public int SearchIntent { get; set; }
    public int RankingPages { get; set; }
    public bool IsTargeted { get; set; }
    public string? TargetEntity { get; set; }
    public int? TargetEntityId { get; set; }
    public int Impressions { get; set; }
    public int Clicks { get; set; }
    public double? ClickThroughRate { get; set; }
    public double? AveragePosition { get; set; }
}

public class CreateKeywordDto
{
    [Required]
    [StringLength(200)]
    public string Term { get; set; } = string.Empty;

    [StringLength(500)]
  public string? Description { get; set; }

    public int SearchVolume { get; set; } = 0;

    [Range(0, 100)]
    public int Difficulty { get; set; } = 0;

    public bool IsTargeted { get; set; } = false;

    [StringLength(100)]
    public string? TargetEntity { get; set; }

    public int? TargetEntityId { get; set; }
}

public class TrackEventDto
{
    [Required]
    [StringLength(100)]
    public string EventType { get; set; } = string.Empty;

    [StringLength(500)]
    public string? PageUrl { get; set; }

    [StringLength(500)]
    public string? EventData { get; set; }

    [StringLength(100)]
    public string? SourceName { get; set; }
}
