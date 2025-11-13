using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class FacilityService : BaseEntity
{
    public int FacilityId { get; set; }
    public Facility Facility { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? Icon { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsAvailable { get; set; } = true;
}
