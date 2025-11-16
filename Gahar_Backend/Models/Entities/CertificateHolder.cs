using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class CertificateHolder : BaseEntity
{
    public int CertificateId { get; set; }
    public Certificate Certificate { get; set; } = null!;

    [StringLength(200)]
    public string? HolderName { get; set; }

    [StringLength(200)]
    [EmailAddress]
 public string? HolderEmail { get; set; }

    [StringLength(100)]
    public string? RegistrationNumber { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool IsExpired { get; set; } = false;

    public bool IsVerified { get; set; } = false;

    [StringLength(500)]
    public string? CertificateUrl { get; set; }

    [StringLength(500)]
    public string? VerificationNotes { get; set; }
}
