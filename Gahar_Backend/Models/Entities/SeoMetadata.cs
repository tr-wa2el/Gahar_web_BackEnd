using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class SeoMetadata : BaseEntity
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

 [StringLength(100)]
    public string? EntityType { get; set; } // Page, Certificate, Facility, etc

    public int? EntityId { get; set; }

    public DateTime? LastOptimizedAt { get; set; }

    [Range(0, 100)]
    public int? SeoScore { get; set; }

    [StringLength(500)]
    public string? Recommendations { get; set; }
}
