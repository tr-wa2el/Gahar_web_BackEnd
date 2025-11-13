using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

/// <summary>
/// نموذج نوع المحتوى - يمثل قسم محتوى مخصص (أخبار، فعاليات، إلخ)
/// </summary>
public class ContentType : TranslatableEntity
{
    /// <summary>
    /// اسم نوع المحتوى
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// الكود المختصر (Slug) لنوع المحتوى
    /// </summary>
    [Required]
[StringLength(100)]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// وصف نوع المحتوى
    /// </summary>
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// اسم الأيقونة (Lucide icon)
    /// </summary>
    [StringLength(50)]
    public string Icon { get; set; } = "FileText";

    /// <summary>
    /// هل هذا نوع محتوى واحد فقط (مثل صفحة عن الموقع)
 /// </summary>
    public bool IsSinglePage { get; set; } = false;

    /// <summary>
    /// هل نوع المحتوى نشط
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// ترتيب العرض
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// قالب عنوان SEO (مثل: {Title} - Gahar)
    /// </summary>
[StringLength(200)]
  public string? MetaTitleTemplate { get; set; }

    /// <summary>
    /// قالب وصف SEO
    /// </summary>
    [StringLength(500)]
    public string? MetaDescriptionTemplate { get; set; }

    // Navigation Properties
    public ICollection<ContentTypeField> Fields { get; set; } = new List<ContentTypeField>();
    public ICollection<Content> Contents { get; set; } = new List<Content>();
}
