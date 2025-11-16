using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Form;

public class FormListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
   public string? Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
public DateTime? PublishedAt { get; set; }
    public string? AuthorName { get; set; }
    public int FieldCount { get; set; }
    public int SubmissionCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class FormDetailDto : FormListDto
{
    public string FormConfiguration { get; set; } = "{}";
    public bool AllowMultipleSubmissions { get; set; }
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }
    public string? RedirectUrl { get; set; }
    public bool SendNotificationEmail { get; set; }
    public string? NotificationEmail { get; set; }
    public bool SendSubmitterEmail { get; set; }
public string? SubmitterEmailField { get; set; }
    public List<FormFieldDto> Fields { get; set; } = new();
}

public class FormFieldDto
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public string FieldConfiguration { get; set; } = "{}";
    public int DisplayOrder { get; set; }
    public bool IsRequired { get; set; }
    public bool IsVisible { get; set; }
   public string? PlaceHolder { get; set; }
public string? HelpText { get; set; }
    public string? CssClass { get; set; }
    public string? CustomId { get; set; }
}

public class CreateFormDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public string FormConfiguration { get; set; } = "{}";
    public bool AllowMultipleSubmissions { get; set; } = true;
    public string? SuccessMessage { get; set; }
 public string? ErrorMessage { get; set; }
    public string? RedirectUrl { get; set; }
    public bool SendNotificationEmail { get; set; } = false;
    public string? NotificationEmail { get; set; }
    public bool SendSubmitterEmail { get; set; } = false;
    public string? SubmitterEmailField { get; set; } = "Email";
}

public class UpdateFormDto : CreateFormDto
{
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
}

public class CreateFormFieldDto
{
    [Required]
    [StringLength(100)]
    public string Label { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FieldName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string FieldType { get; set; } = string.Empty;

  public string FieldConfiguration { get; set; } = "{}";
    public bool IsRequired { get; set; } = false;
    public bool IsVisible { get; set; } = true;
    public string? PlaceHolder { get; set; }
    public string? HelpText { get; set; }
    public string? CssClass { get; set; }
    public string? CustomId { get; set; }
}

public class UpdateFormFieldDto : CreateFormFieldDto
{
    public int DisplayOrder { get; set; }
}

public class ReorderFormFieldsDto
{
    [Required]
  public List<int> FieldIds { get; set; } = new();
}

public class FormSubmissionDto
{
    public int Id { get; set; }
    public string SubmissionData { get; set; } = "{}";
    public string? SubmitterEmail { get; set; }
    public string? SubmitterIpAddress { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public bool IsArchived { get; set; }
    public DateTime? ArchivedAt { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SubmitFormDto
{
    [Required]
    public Dictionary<string, object> Data { get; set; } = new();

    public string? Email { get; set; }
}

public class FormSubmissionFilterDto
{
 public int? FormId { get; set; }
    public string? SearchTerm { get; set; }
    public bool? IsRead { get; set; }
    public bool? IsArchived { get; set; }
    public DateTime? FromDate { get; set; }
  public DateTime? ToDate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortOrder { get; set; } = "desc";
}
