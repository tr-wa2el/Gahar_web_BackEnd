using System.ComponentModel.DataAnnotations;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Models.Entities;

/// <summary>
/// نموذج الروابط المختصرة
/// </summary>
public class ShortLink : BaseEntity
{
    /// <summary>
    /// الرابط الأصلي الكامل
    /// </summary>
    [Required]
    [StringLength(2048)]
    public string OriginalUrl { get; set; } = string.Empty;

    /// <summary>
    /// الكود المختصر الفريد
    /// </summary>
    [Required]
  [StringLength(20)]
    public string ShortCode { get; set; } = string.Empty;

    /// <summary>
    /// الرابط المختصر الكامل
    /// </summary>
    [StringLength(500)]
    public string ShortUrl { get; set; } = string.Empty;

    /// <summary>
    /// عنوان الرابط
    /// </summary>
    [StringLength(200)]
    public string? Title { get; set; }

    /// <summary>
    /// وصف الرابط
    /// </summary>
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// عدد مرات النقر على الرابط
    /// </summary>
    public int ClickCount { get; set; } = 0;

    /// <summary>
    /// تاريخ انتهاء الرابط (اختياري)
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// هل الرابط نشط؟
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// هل يتم عرض QR Code في الرابط؟
    /// </summary>
    public bool ShowQrCode { get; set; } = true;

    /// <summary>
  /// بيانات QR Code (صورة base64)
    /// </summary>
    [StringLength(int.MaxValue)]
    public string? QrCodeData { get; set; }

    /// <summary>
  /// معرف المستخدم الذي أنشأ الرابط
    /// </summary>
    public int? CreatedByUserId { get; set; }

    /// <summary>
    /// آخر زيارة للرابط
    /// </summary>
    public DateTime? LastAccessedAt { get; set; }

    /// <summary>
    /// البلد من آخر زيارة
    /// </summary>
    [StringLength(100)]
    public string? LastAccessCountry { get; set; }

    /// <summary>
    /// المدينة من آخر زيارة
  /// </summary>
    [StringLength(100)]
    public string? LastAccessCity { get; set; }

    /// <summary>
    /// جهاز آخر زيارة
    /// </summary>
    [StringLength(100)]
    public string? LastAccessDevice { get; set; }

    /// <summary>
    /// الملاحظات
    /// </summary>
    [StringLength(500)]
  public string? Notes { get; set; }

    /// <summary>
    /// الفئة/الكاتيجوري
    /// </summary>
    [StringLength(100)]
    public string? Category { get; set; }

    /// <summary>
    /// الوسوم
    /// </summary>
    [StringLength(500)]
    public string? Tags { get; set; }

    // Navigation Properties
 public virtual User? CreatedByUser { get; set; }
    public virtual ICollection<ShortLinkAnalytics> Analytics { get; set; } = new List<ShortLinkAnalytics>();
}
