using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class FacilityImage : BaseEntity
{
    public int FacilityId { get; set; }
    public Facility Facility { get; set; } = null!;

    [Required]
    [StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Caption { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsMainImage { get; set; } = false;
}
