using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Page;

public class PageListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? AuthorName { get; set; }
    public int BlockCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PageDetailDto : PageListDto
{
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
    public string Template { get; set; } = "Default";
  public bool ShowTitle { get; set; }
    public bool ShowBreadcrumbs { get; set; }
    public List<PageBlockDto> Blocks { get; set; } = new();
}

public class PageBlockDto
{
    public int Id { get; set; }
    public string BlockType { get; set; } = string.Empty;
    public object Configuration { get; set; } = new { };
  public int DisplayOrder { get; set; }
    public bool IsVisible { get; set; }
    public string? CssClass { get; set; }
    public string? CustomId { get; set; }
}

public class CreatePageDto
{
  [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

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

    [StringLength(50)]
    public string Template { get; set; } = "Default";

    public bool ShowTitle { get; set; } = true;
    public bool ShowBreadcrumbs { get; set; } = true;
    
    public Dictionary<string, PageTranslationDto>? Translations { get; set; }
}

public class UpdatePageDto : CreatePageDto
{
    public bool IsPublished { get; set; }
}

public class CreatePageBlockDto
{
    [Required]
[StringLength(50)]
    public string BlockType { get; set; } = string.Empty;

    [Required]
    public object Configuration { get; set; } = new { };

    public bool IsVisible { get; set; } = true;
    public string? CssClass { get; set; }
    public string? CustomId { get; set; }
}

public class UpdatePageBlockDto : CreatePageBlockDto
{
    public int DisplayOrder { get; set; }
}

public class ReorderBlocksDto
{
    [Required]
    public List<int> BlockIds { get; set; } = new();
}

public class PageTranslationDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }
}
