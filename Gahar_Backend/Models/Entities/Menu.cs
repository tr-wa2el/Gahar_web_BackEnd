using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class Menu : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(500)]
  public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsPublished { get; set; } = false;

    public DateTime? PublishedAt { get; set; }

    public int? AuthorId { get; set; }
    public User? Author { get; set; }

    // Navigation Properties
    public ICollection<MenuItem> Items { get; set; } = new List<MenuItem>();
}
