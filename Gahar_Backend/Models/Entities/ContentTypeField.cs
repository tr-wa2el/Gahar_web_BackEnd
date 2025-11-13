using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

/// <summary>
/// حقل من حقول نوع المحتوى (مثل: تاريخ الفعالية، المكان، إلخ)
/// </summary>
public class ContentTypeField : TranslatableEntity
{
    /// <summary>
    /// معرف نوع المحتوى
    /// </summary>
    public int ContentTypeId { get; set; }
    public ContentType ContentType { get; set; } = null!;

    /// <summary>
    /// اسم الحقل
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// مفتاح الحقل (بدون مسافات - للبرمجة)
    /// </summary>
    [Required]
    [StringLength(100)]
    public string FieldKey { get; set; } = string.Empty;

    /// <summary>
    /// نوع الحقل (Text, Textarea, RichText, Date, Number, Image, File, Select, Radio, Checkbox)
    /// </summary>
    [Required]
    [StringLength(50)]
    public string FieldType { get; set; } = string.Empty;

    /// <summary>
    /// وصف الحقل
    /// </summary>
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// هل الحقل مطلوب
    /// </summary>
    public bool IsRequired { get; set; } = false;

    /// <summary>
    /// هل يمكن ترجمة هذا الحقل
 /// </summary>
    public bool IsTranslatable { get; set; } = true;

    /// <summary>
 /// هل يظهر في قائمة المحتوى
    /// </summary>
    public bool ShowInList { get; set; } = true;

    /// <summary>
    /// ترتيب الحقل
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// قواعد التحقق من الصحة (JSON)
    /// </summary>
    public string? ValidationRules { get; set; }

    /// <summary>
    /// القيمة الافتراضية
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// النص الذي يظهر في الحقل الفارغ
    /// </summary>
    [StringLength(500)]
    public string? Placeholder { get; set; }

    /// <summary>
    /// خيارات الحقل للـ Select, Radio, Checkbox (JSON)
    /// </summary>
    public string? Options { get; set; }
}
