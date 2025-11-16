using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class MenuItem : BaseEntity
{
    public int MenuId { get; set; }
    public Menu Menu { get; set; } = null!;

    public int? ParentItemId { get; set; }
    public MenuItem? ParentItem { get; set; }

    [Required]
    [StringLength(100)]
 public string Label { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Url { get; set; }

    [StringLength(100)]
    public string? Icon { get; set; }

    [StringLength(100)]
    public string? CssClass { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsVisible { get; set; } = true;

    public bool OpenInNewTab { get; set; } = false;

    [StringLength(500)]
    public string? Title { get; set; }

    public int? RelatedPageId { get; set; }

 // Navigation Properties
    public ICollection<MenuItem> ChildItems { get; set; } = new List<MenuItem>();
}
