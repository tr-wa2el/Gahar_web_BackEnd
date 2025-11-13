using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class PageBlock : BaseEntity
{
    public int PageId { get; set; }
  public Page Page { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string BlockType { get; set; } = string.Empty;

    [Required]
    public string Configuration { get; set; } = "{}"; // JSON configuration

    public int DisplayOrder { get; set; }

    public bool IsVisible { get; set; } = true;

    [StringLength(100)]
    public string? CssClass { get; set; }

    [StringLength(100)]
    public string? CustomId { get; set; }
}
