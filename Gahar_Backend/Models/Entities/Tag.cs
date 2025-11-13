using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

/// <summary>
/// وسم (Tag) لتصنيف وتنظيم المحتوى
/// </summary>
public class Tag : TranslatableEntity
{
    /// <summary>
    /// اسم الوسم
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

  /// <summary>
    /// الكود المختصر للوسم
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
  /// وصف الوسم
  /// </summary>
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// لون الوسم (Hex)
    /// </summary>
    [StringLength(7)]
    public string? Color { get; set; }

    // Navigation Properties
    public ICollection<ContentTag> ContentTags { get; set; } = new List<ContentTag>();
}

/// <summary>
/// ربط بين المحتوى والوسم
/// </summary>
public class ContentTag : BaseEntity
{
    public int ContentId { get; set; }
    public Content Content { get; set; } = null!;

    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
