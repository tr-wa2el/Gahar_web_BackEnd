using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class Facility : BaseEntity
{
    [Required]
  [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(500)]
  public string? Address { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(200)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(500)]
    public string? Website { get; set; }

    // Location
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // Details
    [StringLength(100)]
    public string? DirectorName { get; set; }

    public int? TotalBeds { get; set; }

    public int? AverageWaitTime { get; set; } // in minutes

    [StringLength(50)]
    public string? AccreditationStatus { get; set; }

    // Status
    public bool IsActive { get; set; } = true;
    public bool IsPublished { get; set; } = false;
  public DateTime? PublishedAt { get; set; }

    // Author
    public int? AuthorId { get; set; }
    public User? Author { get; set; }

    // Navigation Properties
    public ICollection<FacilityDepartment> Departments { get; set; } = new List<FacilityDepartment>();
    public ICollection<FacilityService> Services { get; set; } = new List<FacilityService>();
 public ICollection<FacilityImage> Images { get; set; } = new List<FacilityImage>();
    public ICollection<FacilityReview> Reviews { get; set; } = new List<FacilityReview>();
}
