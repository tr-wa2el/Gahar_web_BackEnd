using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

/// <summary>
/// نموذج تحليلات الروابط المختصرة
/// </summary>
public class ShortLinkAnalytics : BaseEntity
{
    /// <summary>
    /// معرف الرابط المختصر
    /// </summary>
    [Required]
    public int ShortLinkId { get; set; }

    /// <summary>
    /// الزمن الذي تم النقر عليه
    /// </summary>
    public DateTime ClickTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// عنوان IP
    /// </summary>
    [StringLength(45)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// User Agent
    /// </summary>
    [StringLength(500)]
 public string? UserAgent { get; set; }

  /// <summary>
    /// Referrer
    /// </summary>
    [StringLength(500)]
    public string? Referrer { get; set; }

    /// <summary>
    /// البلد
    /// </summary>
    [StringLength(100)]
    public string? Country { get; set; }

    /// <summary>
    /// المدينة
    /// </summary>
    [StringLength(100)]
    public string? City { get; set; }

  /// <summary>
 /// خطوط الطول والعرض (Latitude)
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// خطوط الطول والعرض (Longitude)
  /// </summary>
    public double? Longitude { get; set; }

    /// <summary>
    /// نوع الجهاز
    /// </summary>
    [StringLength(50)]
    public string? DeviceType { get; set; } // Desktop, Mobile, Tablet

    /// <summary>
    /// نظام التشغيل
    /// </summary>
    [StringLength(100)]
    public string? OperatingSystem { get; set; }

  /// <summary>
    /// اسم المتصفح
    /// </summary>
    [StringLength(100)]
  public string? BrowserName { get; set; }

    /// <summary>
    /// إصدار المتصفح
    /// </summary>
    [StringLength(50)]
    public string? BrowserVersion { get; set; }

    /// <summary>
    /// اللغة
    /// </summary>
    [StringLength(10)]
    public string? Language { get; set; }

    // Navigation Properties
    public virtual ShortLink ShortLink { get; set; } = null!;
}
