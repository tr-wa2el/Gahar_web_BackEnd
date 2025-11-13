using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Menu;

public class MenuListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? AuthorName { get; set; }
 public int ItemCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class MenuDetailDto : MenuListDto
{
  public List<MenuItemDto> Items { get; set; } = new();
}

public class MenuItemDto
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public string? CssClass { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsVisible { get; set; }
    public bool OpenInNewTab { get; set; }
    public string? Title { get; set; }
    public int? RelatedPageId { get; set; }
public int? ParentItemId { get; set; }
    public List<MenuItemDto> ChildItems { get; set; } = new();
}

public class CreateMenuDto
{
[Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
 [StringLength(100)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }
}

public class UpdateMenuDto : CreateMenuDto
{
    public int DisplayOrder { get; set; }
 public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
}

public class CreateMenuItemDto
{
    [Required]
    [StringLength(100)]
    public string Label { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Url { get; set; }

    [StringLength(100)]
    public string? Icon { get; set; }

    [StringLength(100)]
    public string? CssClass { get; set; }

    public bool IsVisible { get; set; } = true;
    public bool OpenInNewTab { get; set; } = false;
    public string? Title { get; set; }
    public int? RelatedPageId { get; set; }
    public int? ParentItemId { get; set; }
}

public class UpdateMenuItemDto : CreateMenuItemDto
{
    public int DisplayOrder { get; set; }
}

public class ReorderMenuItemsDto
{
    [Required]
  public List<int> ItemIds { get; set; } = new();
}
