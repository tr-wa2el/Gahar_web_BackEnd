using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class AnalyticsEvent : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string EventType { get; set; } = string.Empty; // PageView, Click, FormSubmit, etc

    [StringLength(500)]
    public string? EventData { get; set; } // JSON

    [StringLength(500)]
    public string? PageUrl { get; set; }

    [StringLength(100)]
    public string? UserAgent { get; set; }

    [StringLength(45)]
    public string? IpAddress { get; set; }

 [StringLength(100)]
    public string? CountryCode { get; set; }

    [StringLength(100)]
    public string? CityName { get; set; }

    [StringLength(100)]
    public string? DeviceType { get; set; }

    [StringLength(100)]
    public string? BrowserName { get; set; }

    [StringLength(100)]
    public string? OsName { get; set; }

    [StringLength(100)]
    public string? SourceName { get; set; } // Organic, Direct, Social, etc

    public int? UserId { get; set; }

    [StringLength(100)]
    public string? SessionId { get; set; }
}
