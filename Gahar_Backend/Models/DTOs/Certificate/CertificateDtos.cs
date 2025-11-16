using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Certificate;

public class CertificateListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
  public int DisplayOrder { get; set; }
    public string? IssuingBody { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsExpired { get; set; }
    public bool IsRenewable { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
  public DateTime? PublishedAt { get; set; }
    public string? AuthorName { get; set; }
    public int CategoryCount { get; set; }
    public int RequirementCount { get; set; }
    public int HolderCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CertificateDetailDto : CertificateListDto
{
    public string? DocumentUrl { get; set; }
    public List<CertificateCategoryDto> Categories { get; set; } = new();
    public List<CertificateRequirementDto> Requirements { get; set; } = new();
    public List<CertificateHolderDto> Holders { get; set; } = new();
}

public class CertificateCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class CertificateRequirementDto
{
    public int Id { get; set; }
    public string Requirement { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsOptional { get; set; }
}

public class CertificateHolderDto
{
    public int Id { get; set; }
    public string? HolderName { get; set; }
public string? HolderEmail { get; set; }
    public string? RegistrationNumber { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsExpired { get; set; }
    public bool IsVerified { get; set; }
    public string? CertificateUrl { get; set; }
    public string? VerificationNotes { get; set; }
}

public class CreateCertificateDto
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
    public string? ImageUrl { get; set; }

    [StringLength(500)]
    public string? DocumentUrl { get; set; }

    [StringLength(100)]
    public string? IssuingBody { get; set; }

    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
  public bool IsRenewable { get; set; } = false;
}

public class UpdateCertificateDto : CreateCertificateDto
{
    public int DisplayOrder { get; set; }
  public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
}

public class CreateCertificateCategoryDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }
}

public class UpdateCertificateCategoryDto : CreateCertificateCategoryDto
{
    public int DisplayOrder { get; set; }
}

public class CreateCertificateRequirementDto
{
    [Required]
  [StringLength(500)]
    public string Requirement { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public bool IsOptional { get; set; } = false;
}

public class UpdateCertificateRequirementDto : CreateCertificateRequirementDto
{
    public int DisplayOrder { get; set; }
}

public class CreateCertificateHolderDto
{
    [StringLength(200)]
  public string? HolderName { get; set; }

    [StringLength(200)]
    [EmailAddress]
    public string? HolderEmail { get; set; }

    [StringLength(100)]
    public string? RegistrationNumber { get; set; }

    [Required]
    public DateTime IssueDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    [StringLength(500)]
    public string? CertificateUrl { get; set; }
}

public class UpdateCertificateHolderDto : CreateCertificateHolderDto
{
    public bool IsVerified { get; set; }

    [StringLength(500)]
    public string? VerificationNotes { get; set; }
}

public class CertificateFilterDto
{
    public string? SearchTerm { get; set; }
    public bool? IsPublished { get; set; }
    public string? IssuingBody { get; set; }
    public bool? IsRenewable { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "DisplayOrder";
    public string? SortOrder { get; set; } = "asc";
}
