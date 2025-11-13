using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class CertificateRequirement : BaseEntity
{
 public int CertificateId { get; set; }
    public Certificate Certificate { get; set; } = null!;

    [Required]
    [StringLength(500)]
 public string Requirement { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsOptional { get; set; } = false;
}
