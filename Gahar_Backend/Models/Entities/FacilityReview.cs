using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class FacilityReview : BaseEntity
{
    public int FacilityId { get; set; }
    public Facility Facility { get; set; } = null!;

    [StringLength(100)]
    public string? ReviewerName { get; set; }

    [StringLength(500)]
    public string? Email { get; set; }

    [StringLength(1000)]
    public string ReviewText { get; set; } = string.Empty;

    public int Rating { get; set; } // 1-5

    public bool IsApproved { get; set; } = false;

    public int? ApprovedBy { get; set; }
}
