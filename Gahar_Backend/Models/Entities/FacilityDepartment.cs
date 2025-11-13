using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class FacilityDepartment : BaseEntity
{
    public int FacilityId { get; set; }
    public Facility Facility { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

  [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(200)]
    public string? HeadName { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;
}
