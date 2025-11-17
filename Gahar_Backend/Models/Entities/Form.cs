using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class Form : BaseEntity
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
 public string Slug { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    // Form Configuration
    public string FormConfiguration { get; set; } = "{}"; // JSON configuration

    // Submission Settings
  public bool AllowMultipleSubmissions { get; set; } = true;

    [StringLength(500)]
    public string? SuccessMessage { get; set; }

    [StringLength(500)]
    public string? ErrorMessage { get; set; }

    [StringLength(500)]
    public string? RedirectUrl { get; set; }

 // Email Notifications
    public bool SendNotificationEmail { get; set; } = false;

    [StringLength(500)]
 public string? NotificationEmail { get; set; }

    public bool SendSubmitterEmail { get; set; } = false;

    [StringLength(500)]
    public string? SubmitterEmailField { get; set; } = "Email";

    // Status
    public bool IsActive { get; set; } = true;

    public bool IsPublished { get; set; } = false;

    public DateTime? PublishedAt { get; set; }

    // Author
public int? AuthorId { get; set; }
    public User? Author { get; set; }

    /// <summary>
    /// القسم التابع له النموذج
    /// يحدد من يمكنه رؤية وتعديل النموذج
    /// </summary>
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    // Navigation Properties
    public ICollection<FormField> Fields { get; set; } = new List<FormField>();
    public ICollection<FormSubmission> Submissions { get; set; } = new List<FormSubmission>();
}
