using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

/// <summary>
/// التخطيط - يحدد كيفية عرض المحتوى
/// </summary>
public class Layout : TranslatableEntity
{
    /// <summary>
    /// اسم التخطيط
 /// </summary>
    [Required]
  [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// وصف التخطيط
    /// </summary>
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// إعدادات التخطيط (JSON)
    /// </summary>
    public string Configuration { get; set; } = "{}";

    /// <summary>
    /// هل هذا التخطيط الافتراضي
    /// </summary>
  public bool IsDefault { get; set; } = false;

 /// <summary>
    /// هل التخطيط نشط
    /// </summary>
    public bool IsActive { get; set; } = true;

/// <summary>
    /// صورة معاينة التخطيط
    /// </summary>
    [StringLength(500)]
    public string? PreviewImage { get; set; }

    // Navigation Properties
    public ICollection<Content> Contents { get; set; } = new List<Content>();
}
