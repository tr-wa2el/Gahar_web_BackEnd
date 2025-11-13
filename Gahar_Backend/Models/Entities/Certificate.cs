using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class Certificate : BaseEntity
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

    // Certificate Details
    public int DisplayOrder { get; set; }

    [StringLength(100)]
    public string? IssuingBody { get; set; }

    public DateTime? IssueDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool IsExpired { get; set; } = false;

    public bool IsRenewable { get; set; } = false;

    // Status
    public bool IsActive { get; set; } = true;

  public bool IsPublished { get; set; } = false;

    public DateTime? PublishedAt { get; set; }

    // Author
    public int? AuthorId { get; set; }
    public User? Author { get; set; }

    // Navigation Properties
    public ICollection<CertificateCategory> Categories { get; set; } = new List<CertificateCategory>();
    public ICollection<CertificateRequirement> Requirements { get; set; } = new List<CertificateRequirement>();
    public ICollection<CertificateHolder> Holders { get; set; } = new List<CertificateHolder>();
}
