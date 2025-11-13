using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

/// <summary>
/// نموذج المحتوى - محتوى فعلي لنوع محتوى معين
/// </summary>
public class Content : TranslatableEntity
{
    /// <summary>
    /// معرف نوع المحتوى
    /// </summary>
    public int ContentTypeId { get; set; }
    public ContentType ContentType { get; set; } = null!;

    /// <summary>
    /// عنوان المحتوى
    /// </summary>
    [Required]
    [StringLength(200)]
 public string Title { get; set; } = string.Empty;

    /// <summary>
    /// الكود المختصر (Slug)
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
 /// ملخص المحتوى
/// </summary>
    [StringLength(1000)]
    public string? Summary { get; set; }

    /// <summary>
    /// محتوى النص الغني
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    /// صورة العرض الرئيسية
    /// </summary>
  [StringLength(500)]
    public string? FeaturedImage { get; set; }

    /// <summary>
    /// معرف التخطيط المستخدم
    /// </summary>
    public int? LayoutId { get; set; }
 public Layout? Layout { get; set; }

    // SEO Fields
    /// <summary>
    /// عنوان SEO
    /// </summary>
    [StringLength(200)]
  public string? MetaTitle { get; set; }

 /// <summary>
    /// وصف SEO
    /// </summary>
    [StringLength(500)]
    public string? MetaDescription { get; set; }

    /// <summary>
    /// كلمات مفتاحية للـ SEO
    /// </summary>
    [StringLength(500)]
    public string? MetaKeywords { get; set; }

    // Open Graph Tags
    [StringLength(200)]
    public string? OgTitle { get; set; }

    [StringLength(500)]
    public string? OgDescription { get; set; }

    [StringLength(500)]
    public string? OgImage { get; set; }

    // Publishing
    /// <summary>
    /// حالة المحتوى (Draft, Published, Scheduled, Archived)
    /// </summary>
    [StringLength(20)]
    public string Status { get; set; } = "Draft";

    /// <summary>
    /// تاريخ النشر
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// تاريخ الجدولة المخطط له
    /// </summary>
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// معرف مؤلف المحتوى
    /// </summary>
 public int? AuthorId { get; set; }
    public User? Author { get; set; }

    // Stats & Features
    /// <summary>
    /// عدد المشاهدات
    /// </summary>
    public int ViewCount { get; set; } = 0;

    /// <summary>
    /// هل المحتوى مميز/مبرز
    /// </summary>
    public bool IsFeatured { get; set; } = false;

    /// <summary>
    /// هل يسمح بالتعليقات
    /// </summary>
  public bool AllowComments { get; set; } = true;

  // Navigation Properties
    public ICollection<ContentFieldValue> FieldValues { get; set; } = new List<ContentFieldValue>();
    public ICollection<ContentTag> Tags { get; set; } = new List<ContentTag>();
}
