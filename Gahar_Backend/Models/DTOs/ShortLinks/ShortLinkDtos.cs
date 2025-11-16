using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.ShortLinks;

/// <summary>
/// DTO لإنشاء رابط مختصر جديد
/// </summary>
public class CreateShortLinkDto
{
 /// <summary>
    /// الرابط الأصلي الكامل
    /// </summary>
    [Required(ErrorMessage = "الرابط الأصلي مطلوب")]
    [Url(ErrorMessage = "يجب أن يكون رابط صحيح")]
    [StringLength(2048, MinimumLength = 10, ErrorMessage = "الرابط يجب أن يكون بين 10 و 2048 حرف")]
    public string OriginalUrl { get; set; } = string.Empty;

    /// <summary>
    /// عنوان الرابط
    /// </summary>
    [StringLength(200, ErrorMessage = "العنوان لا يجب أن يتجاوز 200 حرف")]
    public string? Title { get; set; }

    /// <summary>
 /// وصف الرابط
    /// </summary>
    [StringLength(500, ErrorMessage = "الوصف لا يجب أن يتجاوز 500 حرف")]
    public string? Description { get; set; }

    /// <summary>
    /// تاريخ انتهاء الرابط
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
  /// هل يتم عرض QR Code؟
    /// </summary>
    public bool ShowQrCode { get; set; } = true;

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

    /// <summary>
    /// الملاحظات
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }
}

/// <summary>
/// DTO لتحديث رابط مختصر
/// </summary>
public class UpdateShortLinkDto
{
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
    /// تاريخ انتهاء الرابط
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// هل الرابط نشط؟
    /// </summary>
    public bool IsActive { get; set; } = true;

  /// <summary>
    /// هل يتم عرض QR Code؟
    /// </summary>
    public bool ShowQrCode { get; set; } = true;

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

    /// <summary>
    /// الملاحظات
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }
}

/// <summary>
/// DTO لعرض رابط مختصر
/// </summary>
public class ShortLinkDto
{
 public int Id { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int ClickCount { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; }
    public bool ShowQrCode { get; set; }
    public string? QrCodeData { get; set; }
 public DateTime? LastAccessedAt { get; set; }
    public string? LastAccessCountry { get; set; }
    public string? LastAccessCity { get; set; }
    public string? LastAccessDevice { get; set; }
public string? Category { get; set; }
    public string? Tags { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO لإحصائيات الروابط المختصرة
/// </summary>
public class ShortLinkAnalyticsDto
{
    public int Id { get; set; }
    public int ShortLinkId { get; set; }
    public DateTime ClickTime { get; set; }
    public string? IpAddress { get; set; }
    public string? Country { get; set; }
 public string? City { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? DeviceType { get; set; }
    public string? OperatingSystem { get; set; }
    public string? BrowserName { get; set; }
  public string? BrowserVersion { get; set; }
    public string? Language { get; set; }
}

/// <summary>
/// DTO لإحصائيات مجمعة للرابط
/// </summary>
public class ShortLinkStatisticsDto
{
    public int ShortLinkId { get; set; }
    public string ShortCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int TotalClicks { get; set; }
    public int UniqueClicks { get; set; }
    public DateTime? FirstClickedAt { get; set; }
    public DateTime? LastClickedAt { get; set; }
    public Dictionary<string, int> ClicksByCountry { get; set; } = new();
    public Dictionary<string, int> ClicksByDevice { get; set; } = new();
    public Dictionary<string, int> ClicksByBrowser { get; set; } = new();
}

/// <summary>
/// DTO لتوليد QR Code
/// </summary>
public class GenerateQrCodeDto
{
    public string ShortUrl { get; set; } = string.Empty;
    public int Size { get; set; } = 300; // Default size in pixels
    public string LogoUrl { get; set; } = string.Empty; // Logo image URL
    public int LogoSize { get; set; } = 80; // Logo size in pixels
    public string? BackgroundColor { get; set; } = "#FFFFFF";
    public string? ForegroundColor { get; set; } = "#000000";
}

/// <summary>
/// DTO لنتيجة توليد QR Code
/// </summary>
public class QrCodeResultDto
{
    public string QrCodeData { get; set; } = string.Empty; // Base64 image
    public string ShortUrl { get; set; } = string.Empty;
    public string MimeType { get; set; } = "image/png";
}
