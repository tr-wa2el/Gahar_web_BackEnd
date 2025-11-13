using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Content;

/// <summary>
/// DTO لعرض المحتوى في القائمة
/// </summary>
public class ContentListDto
{
    public int Id { get; set; }
 public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }
  public string? FeaturedImage { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; }
    public bool IsFeatured { get; set; }
    public string ContentTypeName { get; set; } = string.Empty;
    public string? AuthorName { get; set; }
    public List<TagDto> Tags { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO لعرض المحتوى بالتفاصيل الكاملة
/// </summary>
public class ContentDetailDto : ContentListDto
{
    public string? Body { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// DTO بسيط للمحتوى
/// </summary>
public class ContentDto
{
    public int Id { get; set; }
    public int ContentTypeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Body { get; set; }
    public string? FeaturedImage { get; set; }
    public int? LayoutId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public DateTime? ScheduledAt { get; set; }
 public int? AuthorId { get; set; }
    public int ViewCount { get; set; }
    public bool IsFeatured { get; set; }
 public bool AllowComments { get; set; }
}

/// <summary>
/// DTO لإنشاء محتوى جديد
/// </summary>
public class CreateContentDto
{
    [Required(ErrorMessage = "نوع المحتوى مطلوب")]
    public int ContentTypeId { get; set; }

  [Required(ErrorMessage = "العنوان مطلوب")]
    [StringLength(200, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "الكود المختصر مطلوب")]
    [StringLength(200)]
    [RegularExpression(@"^[a-z0-9-]+$")]
    public string Slug { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Summary { get; set; }

    public string? Body { get; set; }

    [StringLength(500)]
    public string? FeaturedImage { get; set; }

    public int? LayoutId { get; set; }

    // SEO
    [StringLength(200)]
    public string? MetaTitle { get; set; }

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    [StringLength(500)]
    public string? MetaKeywords { get; set; }

    [StringLength(200)]
    public string? OgTitle { get; set; }

    [StringLength(500)]
    public string? OgDescription { get; set; }

    [StringLength(500)]
    public string? OgImage { get; set; }

    // Publishing
    [StringLength(20)]
    public string Status { get; set; } = "Draft";

    public DateTime? ScheduledAt { get; set; }
    public bool IsFeatured { get; set; } = false;
    public bool AllowComments { get; set; } = true;

    // Tags & Custom Fields
  public List<int>? TagIds { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
    public Dictionary<string, ContentTranslationDto>? Translations { get; set; }
}

/// <summary>
/// DTO لتحديث محتوى
/// </summary>
public class UpdateContentDto : CreateContentDto
{
}

/// <summary>
/// DTO للترجمة
/// </summary>
public class ContentTranslationDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

 [Required]
    public string Slug { get; set; } = string.Empty;

 public string? Summary { get; set; }
    public string? Body { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}

/// <summary>
/// DTO للتصفية والبحث
/// </summary>
public class ContentFilterDto
{
    public int? ContentTypeId { get; set; }
  public string? Status { get; set; }
    public bool? IsFeatured { get; set; }
    public int? AuthorId { get; set; }
    public List<int>? TagIds { get; set; }
    public string? SearchTerm { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Language { get; set; } = "ar";
 public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortOrder { get; set; } = "desc";
}

/// <summary>
/// DTO للوسم
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
/// DTO لإنشاء وسم
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
    [RegularExpression(@"^#[0-9A-Fa-f]{6}$|^$")]
    public string? Color { get; set; }
}

/// <summary>
/// نتيجة مرقمة
/// </summary>
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}
