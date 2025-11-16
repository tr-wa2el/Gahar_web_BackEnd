using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Facility;

public class FacilityListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? DirectorName { get; set; }
    public int? TotalBeds { get; set; }
    public int? AverageWaitTime { get; set; }
    public string? AccreditationStatus { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? AuthorName { get; set; }
    public int DepartmentCount { get; set; }
    public int ServiceCount { get; set; }
    public int ImageCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class FacilityDetailDto : FacilityListDto
{
    public List<FacilityDepartmentDto> Departments { get; set; } = new();
    public List<FacilityServiceDto> Services { get; set; } = new();
    public List<FacilityImageDto> Images { get; set; } = new();
    public List<FacilityReviewDto> Reviews { get; set; } = new();
    public double AverageRating { get; set; }
    public int ApprovedReviewCount { get; set; }
}

public class FacilityDepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public string? HeadName { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class FacilityServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsAvailable { get; set; }
}

public class FacilityImageDto
{
  public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
  public int DisplayOrder { get; set; }
    public bool IsMainImage { get; set; }
}

public class FacilityReviewDto
{
    public int Id { get; set; }
    public string? ReviewerName { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public int Rating { get; set; }
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateFacilityDto
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

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [StringLength(100)]
    public string? DirectorName { get; set; }

    public int? TotalBeds { get; set; }
    public int? AverageWaitTime { get; set; }

    [StringLength(50)]
  public string? AccreditationStatus { get; set; }
}

public class UpdateFacilityDto : CreateFacilityDto
{
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
}

public class CreateFacilityDepartmentDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(200)]
    public string? HeadName { get; set; }
}

public class UpdateFacilityDepartmentDto : CreateFacilityDepartmentDto
{
    public int DisplayOrder { get; set; }
}

public class CreateFacilityServiceDto
{
    [Required]
  [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? Icon { get; set; }
}

public class UpdateFacilityServiceDto : CreateFacilityServiceDto
{
  public int DisplayOrder { get; set; }
}

public class CreateFacilityImageDto
{
    [Required]
    [StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Caption { get; set; }

  public bool IsMainImage { get; set; } = false;
}

public class UpdateFacilityImageDto : CreateFacilityImageDto
{
    public int DisplayOrder { get; set; }
}

public class CreateFacilityReviewDto
{
    [StringLength(100)]
    public string? ReviewerName { get; set; }

    [StringLength(500)]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [StringLength(1000)]
  public string ReviewText { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; }
}

public class ApproveFacilityReviewDto
{
    public bool IsApproved { get; set; }
}

public class FacilityFilterDto
{
    public string? SearchTerm { get; set; }
    public bool? IsPublished { get; set; }
    public string? AccreditationStatus { get; set; }
    public double? Latitude { get; set; }
  public double? Longitude { get; set; }
    public double? Radius { get; set; } // in km
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "Name";
    public string? SortOrder { get; set; } = "asc";
}
