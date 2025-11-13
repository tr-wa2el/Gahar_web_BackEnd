using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

/// <summary>
/// قيمة حقل المحتوى - تخزين القيم المخصصة لحقول نوع المحتوى
/// </summary>
public class ContentFieldValue : BaseEntity
{
    /// <summary>
    /// معرف المحتوى
    /// </summary>
    public int ContentId { get; set; }
    public Content Content { get; set; } = null!;

    /// <summary>
    /// معرف حقل نوع المحتوى
    /// </summary>
    public int ContentTypeFieldId { get; set; }
    public ContentTypeField ContentTypeField { get; set; } = null!;

    /// <summary>
  /// مفتاح الحقل
    /// </summary>
    [Required]
    [StringLength(100)]
    public string FieldKey { get; set; } = string.Empty;

    /// <summary>
    /// قيمة الحقل (نص، رقم، تاريخ، JSON، إلخ)
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// معرف اللغة (للحقول القابلة للترجمة)
    /// </summary>
    public int? LanguageId { get; set; }
    public Language? Language { get; set; }
}
