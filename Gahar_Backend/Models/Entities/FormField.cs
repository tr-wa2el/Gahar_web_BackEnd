using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class FormField : BaseEntity
{
    public int FormId { get; set; }
    public Form Form { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Label { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FieldName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
  public string FieldType { get; set; } = string.Empty; // Text, Email, Number, Select, Checkbox, etc.

    public string FieldConfiguration { get; set; } = "{}"; // JSON with options, validation rules, etc.

    public int DisplayOrder { get; set; }

    public bool IsRequired { get; set; } = false;

    public bool IsVisible { get; set; } = true;

    [StringLength(500)]
    public string? PlaceHolder { get; set; }

    [StringLength(500)]
  public string? HelpText { get; set; }

    [StringLength(100)]
    public string? CssClass { get; set; }

    [StringLength(100)]
    public string? CustomId { get; set; }
}
