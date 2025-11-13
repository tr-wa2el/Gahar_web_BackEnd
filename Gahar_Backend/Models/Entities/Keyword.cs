using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class Keyword : BaseEntity
{
    [Required]
    [StringLength(200)]
    public string Term { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public int SearchVolume { get; set; } = 0;

    [Range(0, 100)]
    public int Difficulty { get; set; } = 0; // 0-100

    [Range(0, 100)]
 public int SearchIntent { get; set; } = 0; // 0-100

    public int RankingPages { get; set; } = 0;

    public bool IsTargeted { get; set; } = false;

    [StringLength(100)]
    public string? TargetEntity { get; set; }

    public int? TargetEntityId { get; set; }

    public int Impressions { get; set; } = 0;

    public int Clicks { get; set; } = 0;

    [Range(0, 100)]
    public double? ClickThroughRate { get; set; }

    public double? AveragePosition { get; set; }

    public DateTime? LastUpdatedAt { get; set; }
}
