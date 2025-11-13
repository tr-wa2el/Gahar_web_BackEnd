using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Layout;

/// <summary>
/// DTO لعرض التخطيط
/// </summary>
public class LayoutDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Configuration { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public string? PreviewImage { get; set; }
    public int ContentCount { get; set; }
 public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO لعرض التخطيط بالتفاصيل الكاملة
/// </summary>
public class LayoutDetailDto : LayoutDto
{
    public string? Configuration { get; set; }
}

/// <summary>
/// DTO لإنشاء تخطيط جديد
/// </summary>
public class CreateLayoutDto
{
    [Required(ErrorMessage = "الاسم مطلوب")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

  [Required(ErrorMessage = "الإعدادات مطلوبة")]
    public string Configuration { get; set; } = "{}";

    [StringLength(500)]
    public string? PreviewImage { get; set; }
}

/// <summary>
/// DTO لتحديث تخطيط
/// </summary>
public class UpdateLayoutDto : CreateLayoutDto
{
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// DTO لتعيين تخطيط افتراضي
/// </summary>
public class SetDefaultLayoutDto
{
    [Required]
    public int LayoutId { get; set; }
}

/// <summary>
/// DTO لإعادة تخطيط المحتوى
/// </summary>
public class BulkUpdateLayoutDto
{
    [Required]
    public int LayoutId { get; set; }

    [Required]
    public List<int> ContentIds { get; set; } = new();
}
