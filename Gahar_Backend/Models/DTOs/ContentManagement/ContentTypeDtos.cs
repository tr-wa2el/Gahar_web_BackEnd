using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.ContentType;

/// <summary>
/// DTO لعرض نوع محتوى بسيط
/// </summary>
public class ContentTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Icon { get; set; } = string.Empty;
    public bool IsSinglePage { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
  public int ContentCount { get; set; }
    public int FieldCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO لعرض نوع محتوى مع جميع التفاصيل
/// </summary>
public class ContentTypeDetailDto : ContentTypeDto
{
    public List<ContentTypeFieldDto> Fields { get; set; } = new();
    public string? MetaTitleTemplate { get; set; }
    public string? MetaDescriptionTemplate { get; set; }
}

/// <summary>
/// DTO لعرض حقل نوع محتوى
/// </summary>
public class ContentTypeFieldDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FieldKey { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsRequired { get; set; }
    public bool IsTranslatable { get; set; }
    public bool ShowInList { get; set; }
    public int DisplayOrder { get; set; }
    public string? ValidationRules { get; set; }
    public string? DefaultValue { get; set; }
    public string? Placeholder { get; set; }
    public List<string>? Options { get; set; }
}

/// <summary>
/// DTO لإنشاء نوع محتوى جديد
/// </summary>
public class CreateContentTypeDto
{
    [Required(ErrorMessage = "الاسم مطلوب")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

  [Required(ErrorMessage = "الكود المختصر مطلوب")]
    [StringLength(100)]
    [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "الكود المختصر يجب أن يحتوي على أحرف صغيرة وأرقام والشرطة فقط")]
    public string Slug { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string Icon { get; set; } = "FileText";

    public bool IsSinglePage { get; set; } = false;

    [StringLength(200)]
    public string? MetaTitleTemplate { get; set; }

    [StringLength(500)]
    public string? MetaDescriptionTemplate { get; set; }
}

/// <summary>
/// DTO لتحديث نوع محتوى
/// </summary>
public class UpdateContentTypeDto : CreateContentTypeDto
{
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}

/// <summary>
/// DTO لإنشاء حقل نوع محتوى جديد
/// </summary>
public class CreateContentTypeFieldDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
  [RegularExpression(@"^[a-z0-9_]+$")]
    public string FieldKey { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string FieldType { get; set; } = string.Empty;

 [StringLength(500)]
    public string? Description { get; set; }

    public bool IsRequired { get; set; } = false;
    public bool IsTranslatable { get; set; } = true;
    public bool ShowInList { get; set; } = true;
    public string? ValidationRules { get; set; }
    public string? DefaultValue { get; set; }

    [StringLength(500)]
  public string? Placeholder { get; set; }

    public List<string>? Options { get; set; }
}

/// <summary>
/// DTO لتحديث حقل نوع محتوى
/// </summary>
public class UpdateContentTypeFieldDto : CreateContentTypeFieldDto
{
    public int DisplayOrder { get; set; }
}

/// <summary>
/// DTO لإعادة ترتيب الحقول
/// </summary>
public class ReorderFieldsDto
{
    [Required]
    public List<int> FieldIds { get; set; } = new();
}

/// <summary>
/// DTO لوسم
/// </summary>
public class TagDto
{
    public int Id { get; set; }
 public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Color { get; set; }
    public int ContentCount { get; set; }
}

/// <summary>
/// DTO لإنشاء وسم جديد
/// </summary>
public class CreateTagDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(7)]
    [RegularExpression(@"^#[0-9A-Fa-f]{6}$")]
    public string? Color { get; set; }
}

/// <summary>
/// DTO للتخطيط
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
}

/// <summary>
/// DTO لإنشاء تخطيط جديد
/// </summary>
public class CreateLayoutDto
{
    [Required]
    [StringLength(100)]
  public string Name { get; set; } = string.Empty;

    [StringLength(500)]
 public string? Description { get; set; }

    [Required]
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
