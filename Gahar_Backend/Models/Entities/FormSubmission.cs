using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class FormSubmission : BaseEntity
{
    public int FormId { get; set; }
    public Form Form { get; set; } = null!;

    public string SubmissionData { get; set; } = "{}"; // JSON with field values

    [StringLength(500)]
    public string? SubmitterEmail { get; set; }

    [StringLength(100)]
    public string? SubmitterIpAddress { get; set; }

    public bool IsRead { get; set; } = false;

    public DateTime? ReadAt { get; set; }

    public bool IsArchived { get; set; } = false;

    public DateTime? ArchivedAt { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}
