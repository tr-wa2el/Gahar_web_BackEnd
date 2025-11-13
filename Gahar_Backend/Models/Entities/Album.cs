using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

/// <summary>
/// نموذج الألبوم - مجموعة من الصور
/// </summary>
public class Album : TranslatableEntity
{
    /// <summary>
    /// عنوان الألبوم
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

  /// <summary>
    /// الكود المختصر للألبوم
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// وصف الألبوم
    /// </summary>
    [StringLength(1000)]
 public string? Description { get; set; }

    /// <summary>
    /// معرف صورة الغلاف
    /// </summary>
  public int? CoverImageId { get; set; }
   public Media? CoverImage { get; set; }

    /// <summary>
    /// هل الألبوم منشور
    /// </summary>
    public bool IsPublished { get; set; } = true;

 /// <summary>
    /// تاريخ النشر
  /// </summary>
  public DateTime? PublishedAt { get; set; }

    /// <summary>
   /// عدد المشاهدات
    /// </summary>
    public int ViewCount { get; set; } = 0;

    /// <summary>
    /// معرف منشئ الألبوم
    /// </summary>
    public int? CreatedBy { get; set; }
    public User? Creator { get; set; }

    // Navigation Properties
    public ICollection<AlbumMedia> AlbumMedias { get; set; } = new List<AlbumMedia>();
}

/// <summary>
/// نموذج ربط الألبوم بالصور
/// </summary>
public class AlbumMedia : BaseEntity
{
    /// <summary>
    /// معرف الألبوم
    /// </summary>
    public int AlbumId { get; set; }
    public Album Album { get; set; } = null!;

    /// <summary>
    /// معرف الملف (الصورة)
    /// </summary>
   public int MediaId { get; set; }
    public Media Media { get; set; } = null!;

  /// <summary>
    /// ترتيب الصورة في الألبوم
    /// </summary>
  public int DisplayOrder { get; set; }

    /// <summary>
    /// تعليق على الصورة
    /// </summary>
    [StringLength(200)]
    public string? Caption { get; set; }

    /// <summary>
    /// هل الصورة مميزة في الألبوم
    /// </summary>
    public bool IsFeatured { get; set; } = false;
}
