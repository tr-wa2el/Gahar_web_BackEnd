using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class CertificateCategory : BaseEntity
{
    public int CertificateId { get; set; }
    public Certificate Certificate { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;
}
