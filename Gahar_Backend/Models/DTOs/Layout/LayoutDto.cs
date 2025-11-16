using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Layout
{
    /// <summary>
    /// DTO for Layout response
    /// </summary>
    public class LayoutDto
    {
    public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
 public string? Description { get; set; }
        public object Configuration { get; set; } = new { };
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string? PreviewImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO for creating a new layout
    /// </summary>
    public class CreateLayoutDto
    {
        [Required(ErrorMessage = "Layout name is required")]
      [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

     [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Configuration is required")]
     public object Configuration { get; set; } = new { };

   public bool IsActive { get; set; } = true;

        [StringLength(500, ErrorMessage = "Preview image path cannot exceed 500 characters")]
      public string? PreviewImage { get; set; }
    }

    /// <summary>
    /// DTO for updating an existing layout
    /// </summary>
    public class UpdateLayoutDto
    {
        [Required(ErrorMessage = "Layout name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
public string Name { get; set; } = string.Empty;

  [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Configuration is required")]
        public object Configuration { get; set; } = new { };

        public bool IsActive { get; set; }

 [StringLength(500, ErrorMessage = "Preview image path cannot exceed 500 characters")]
        public string? PreviewImage { get; set; }
  }

    /// <summary>
    /// DTO for layout with content count
 /// </summary>
    public class LayoutWithStatsDto : LayoutDto
    {
   public int ContentCount { get; set; }
    }
}
