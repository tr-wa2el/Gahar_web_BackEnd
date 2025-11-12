using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    /// <summary>
    /// Represents a content type (e.g., News, Events, Services)
    /// </summary>
    public class ContentType : TranslatableEntity
{
      /// <summary>
        /// Name of the content type (e.g., "News", "Events")
        /// </summary>
 [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// URL-friendly slug (e.g., "news", "events")
/// </summary>
      [Required]
        [StringLength(100)]
     public string Slug { get; set; } = string.Empty;

  /// <summary>
        /// Description of the content type
 /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

   /// <summary>
        /// Lucide icon name for UI display
        /// </summary>
        [StringLength(50)]
      public string Icon { get; set; } = "FileText";

        /// <summary>
  /// Indicates if this content type should be treated as a single page
        /// (e.g., "About Us" page)
    /// </summary>
        public bool IsSinglePage { get; set; } = false;

        /// <summary>
     /// Indicates if this content type is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
      /// Display order in the UI
/// </summary>
        public int DisplayOrder { get; set; }

        // SEO Settings
        /// <summary>
  /// Template for meta title (e.g., "{Title} - Gahar")
        /// </summary>
        [StringLength(200)]
  public string? MetaTitleTemplate { get; set; }

        /// <summary>
        /// Template for meta description
        /// </summary>
   [StringLength(500)]
        public string? MetaDescriptionTemplate { get; set; }

     // Navigation Properties
        /// <summary>
     /// Fields associated with this content type
        /// </summary>
        public ICollection<ContentTypeField> Fields { get; set; } = new List<ContentTypeField>();

        /// <summary>
        /// Content items of this type
        /// </summary>
        public ICollection<Content> Contents { get; set; } = new List<Content>();
    }
}
