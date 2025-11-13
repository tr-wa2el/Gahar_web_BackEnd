using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class Page : TranslatableEntity
{
    [Required]
    [StringLength(200)]
 public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    // SEO
    [StringLength(200)]
    public string? MetaTitle { get; set; }

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    public string? MetaKeywords { get; set; }

    [StringLength(200)]
    public string? OgTitle { get; set; }

    [StringLength(500)]
    public string? OgDescription { get; set; }

    [StringLength(500)]
    public string? OgImage { get; set; }

    // Publishing
    public bool IsPublished { get; set; } = false;

    public DateTime? PublishedAt { get; set; }

    public int? AuthorId { get; set; }
    public User? Author { get; set; }

    // Template
    [StringLength(50)]
    public string Template { get; set; } = "Default"; // Default, FullWidth, Sidebar

    public bool ShowTitle { get; set; } = true;

    public bool ShowBreadcrumbs { get; set; } = true;

    // Navigation Properties
public ICollection<PageBlock> Blocks { get; set; } = new List<PageBlock>();
}
