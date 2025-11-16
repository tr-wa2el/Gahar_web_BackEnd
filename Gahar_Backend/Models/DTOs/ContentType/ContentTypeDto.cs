using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.ContentType
{
    /// <summary>
    /// Basic DTO for Content Type list
    /// </summary>
    public class ContentTypeDto
    {
        public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
   public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
   public string Icon { get; set; } = "FileText";
   public bool IsSinglePage { get; set; }
      public bool IsActive { get; set; }
   public int DisplayOrder { get; set; }
   public int ContentCount { get; set; }
  public DateTime CreatedAt { get; set; }
    }

  /// <summary>
    /// Detailed DTO for Content Type with fields
    /// </summary>
    public class ContentTypeDetailDto : ContentTypeDto
    {
 public List<ContentTypeFieldDto> Fields { get; set; } = new();
        public string? MetaTitleTemplate { get; set; }
        public string? MetaDescriptionTemplate { get; set; }
    }

    /// <summary>
    /// DTO for Content Type Field
    /// </summary>
    public class ContentTypeFieldDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
   public string FieldKey { get; set; } = string.Empty;
        public string FieldType { get; set; } = string.Empty;
     public string? Description { get; set; }
        public bool IsRequired { get; set; }
   public bool IsTranslatable { get; set; }
 public bool ShowInList { get; set; }
    public int DisplayOrder { get; set; }
 public string? ValidationRules { get; set; }
  public string? DefaultValue { get; set; }
        public string? Placeholder { get; set; }
        public List<string>? Options { get; set; }
    }

    /// <summary>
    /// DTO for creating a new Content Type
    /// </summary>
    public class CreateContentTypeDto
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

[StringLength(50, ErrorMessage = "Icon must not exceed 50 characters")]
        public string Icon { get; set; } = "FileText";

     public bool IsSinglePage { get; set; } = false;

[StringLength(200, ErrorMessage = "Meta title template must not exceed 200 characters")]
  public string? MetaTitleTemplate { get; set; }

[StringLength(500, ErrorMessage = "Meta description template must not exceed 500 characters")]
        public string? MetaDescriptionTemplate { get; set; }
    }

    /// <summary>
    /// DTO for updating a Content Type
    /// </summary>
    public class UpdateContentTypeDto : CreateContentTypeDto
    {
public bool IsActive { get; set; } = true;
 public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// DTO for creating a new Content Type Field
 /// </summary>
  public class CreateContentTypeFieldDto
    {
 [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must not exceed 100 characters")]
  public string Name { get; set; } = string.Empty;

   [Required(ErrorMessage = "Field key is required")]
 [StringLength(100, ErrorMessage = "Field key must not exceed 100 characters")]
        [RegularExpression(@"^[a-z0-9_]+$", 
         ErrorMessage = "Field key must be lowercase letters, numbers, and underscores only")]
      public string FieldKey { get; set; } = string.Empty;

        [Required(ErrorMessage = "Field type is required")]
   [StringLength(50, ErrorMessage = "Field type must not exceed 50 characters")]
      public string FieldType { get; set; } = string.Empty;

[StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
        public string? Description { get; set; }

        public bool IsRequired { get; set; } = false;
 public bool IsTranslatable { get; set; } = true;
        public bool ShowInList { get; set; } = true;

     public string? ValidationRules { get; set; }
        public string? DefaultValue { get; set; }

[StringLength(500, ErrorMessage = "Placeholder must not exceed 500 characters")]
      public string? Placeholder { get; set; }

public List<string>? Options { get; set; }
}

    /// <summary>
    /// DTO for updating a Content Type Field
    /// </summary>
    public class UpdateContentTypeFieldDto : CreateContentTypeFieldDto
    {
     public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// DTO for reordering fields
    /// </summary>
    public class ReorderFieldsDto
    {
        [Required(ErrorMessage = "Field IDs are required")]
     [MinLength(1, ErrorMessage = "At least one field ID is required")]
        public List<int> FieldIds { get; set; } = new();
    }
}
