using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

/// <summary>
/// نموذج الملف المرفوع (صورة، فيديو، مستند، إلخ)
/// </summary>
public class Media : BaseEntity
{
    /// <summary>
    /// اسم الملف الأصلي
    /// </summary>
    [Required]
    [StringLength(200)]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// مسار الملف الأصلي
    /// </summary>
    [Required]
    [StringLength(500)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// مسار الصورة المصغرة (للصور فقط)
    /// </summary>
    [StringLength(500)]
    public string? ThumbnailPath { get; set; }

    /// <summary>
    /// مسار نسخة WebP (للصور)
    /// </summary>
    [StringLength(500)]
    public string? WebPPath { get; set; }

    /// <summary>
    /// نوع MIME
    /// </summary>
    [Required]
    [StringLength(100)]
    public string MimeType { get; set; } = string.Empty;

    /// <summary>
    /// حجم الملف بالبايتات
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
  /// حجم ملف WebP
    /// </summary>
    public long? WebPFileSize { get; set; }

    /// <summary>
    /// عرض الصورة
/// </summary>
public int? Width { get; set; }

    /// <summary>
    /// ارتفاع الصورة
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// نص ALT للصورة
    /// </summary>
    [StringLength(200)]
    public string? Alt { get; set; }

 /// <summary>
    /// تعليق على الملف
    /// </summary>
    [StringLength(500)]
    public string? Caption { get; set; }

    /// <summary>
    /// نوع الملف (Image, Video, Document, Audio)
    /// </summary>
    [StringLength(50)]
    public string MediaType { get; set; } = "Document";

    /// <summary>
    /// معرف المستخدم الذي رفع الملف
    /// </summary>
  public int? UploadedBy { get; set; }
    public User? Uploader { get; set; }

    /// <summary>
    /// هل تم معالجة الملف (تحويل لـ WebP مثلاً)
    /// </summary>
    public bool IsProcessed { get; set; } = false;

    /// <summary>
    /// تاريخ انتهاء المعالجة
    /// </summary>
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
/// عدد مرات استخدام هذا الملف
    /// </summary>
    public int UsageCount { get; set; } = 0;
}
