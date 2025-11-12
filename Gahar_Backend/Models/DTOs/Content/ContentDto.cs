using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Content
{
    /// <summary>
    /// DTO for content list item
    /// </summary>
    public class ContentListDto
   {
 public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
        public string? Summary { get; set; }
      public string? FeaturedImage { get; set; }
   public string Status { get; set; } = string.Empty;
  public DateTime? PublishedAt { get; set; }
   public int ViewCount { get; set; }
   public bool IsFeatured { get; set; }
   public string ContentTypeName { get; set; } = string.Empty;
        public int ContentTypeId { get; set; }
 public string? AuthorName { get; set; }
        public List<TagDto> Tags { get; set; } = new();
        public DateTime CreatedAt { get; set; }
   public DateTime? UpdatedAt { get; set; }
  }

    /// <summary>
    /// DTO for detailed content view
    /// </summary>
    public class ContentDetailDto : ContentListDto
    {
        public string? Body { get; set; }
  public string? MetaTitle { get; set; }
  public string? MetaDescription { get; set; }
   public string? MetaKeywords { get; set; }
        public string? OgTitle { get; set; }
        public string? OgDescription { get; set; }
        public string? OgImage { get; set; }
  public Dictionary<string, object> CustomFields { get; set; } = new();
        public DateTime? ScheduledAt { get; set; }
   public bool AllowComments { get; set; }
    }

    /// <summary>
 /// DTO for creating content
 /// </summary>
   public class CreateContentDto
  {
   [Required(ErrorMessage = "Content type ID is required")]
        public int ContentTypeId { get; set; }

   [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title must not exceed 200 characters")]
 public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug is required")]
  [StringLength(200, ErrorMessage = "Slug must not exceed 200 characters")]
  [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", 
     ErrorMessage = "Slug must be lowercase letters, numbers, and hyphens only")]
     public string Slug { get; set; } = string.Empty;

        public string? Summary { get; set; }
        public string? Body { get; set; }
   
      [StringLength(500, ErrorMessage = "Featured image URL must not exceed 500 characters")]
        public string? FeaturedImage { get; set; }

        // SEO
    [StringLength(200, ErrorMessage = "Meta title must not exceed 200 characters")]
   public string? MetaTitle { get; set; }
        
        [StringLength(500, ErrorMessage = "Meta description must not exceed 500 characters")]
        public string? MetaDescription { get; set; }
  
        public string? MetaKeywords { get; set; }
   
        [StringLength(200, ErrorMessage = "OG title must not exceed 200 characters")]
  public string? OgTitle { get; set; }
   
        [StringLength(500, ErrorMessage = "OG description must not exceed 500 characters")]
   public string? OgDescription { get; set; }
  
  [StringLength(500, ErrorMessage = "OG image URL must not exceed 500 characters")]
      public string? OgImage { get; set; }

        // Publishing
        [Required]
[RegularExpression("^(Draft|Published|Scheduled|Archived)$", 
        ErrorMessage = "Status must be Draft, Published, Scheduled, or Archived")]
  public string Status { get; set; } = "Draft";
  
  public DateTime? ScheduledAt { get; set; }
   public bool IsFeatured { get; set; } = false;
    public bool AllowComments { get; set; } = true;

        // Tags
        public List<int>? TagIds { get; set; }

      // Custom Fields
   public Dictionary<string, object>? CustomFields { get; set; }
 }

  /// <summary>
    /// DTO for updating content
    /// </summary>
 public class UpdateContentDto : CreateContentDto
    {
    }

    /// <summary>
    /// DTO for content filtering
    /// </summary>
    public class ContentFilterDto
    {
        public int? ContentTypeId { get; set; }
  public string? Status { get; set; }
     public bool? IsFeatured { get; set; }
  public int? AuthorId { get; set; }
   public List<int>? TagIds { get; set; }
   public string? SearchTerm { get; set; }
  public DateTime? FromDate { get; set; }
   public DateTime? ToDate { get; set; }
        public string? Language { get; set; } = "ar";
   
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
   public int Page { get; set; } = 1;
 
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;
   
   public string? SortBy { get; set; } = "CreatedAt";
        
        [RegularExpression("^(asc|desc)$", ErrorMessage = "Sort order must be 'asc' or 'desc'")]
   public string? SortOrder { get; set; } = "desc";
    }

    /// <summary>
    /// DTO for paginated result
    /// </summary>
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new();
     public int TotalCount { get; set; }
        public int Page { get; set; }
   public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
  public bool HasPrevious => Page > 1;
   public bool HasNext => Page < TotalPages;
  }

    /// <summary>
    /// DTO for tag
    /// </summary>
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
 public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Color { get; set; }
        public int UsageCount { get; set; }
    }

    /// <summary>
    /// DTO for creating tag
  /// </summary>
    public class CreateTagDto
    {
 [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must not exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug is required")]
        [StringLength(100, ErrorMessage = "Slug must not exceed 100 characters")]
      [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", 
     ErrorMessage = "Slug must be lowercase letters, numbers, and hyphens only")]
     public string Slug { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
   public string? Description { get; set; }

    [StringLength(20, ErrorMessage = "Color must not exceed 20 characters")]
  [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", 
     ErrorMessage = "Color must be a valid hex color code")]
   public string? Color { get; set; }
    }

    /// <summary>
    /// DTO for updating tag
    /// </summary>
    public class UpdateTagDto : CreateTagDto
    {
    }
}
