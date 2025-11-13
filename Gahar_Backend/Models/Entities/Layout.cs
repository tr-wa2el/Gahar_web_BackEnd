using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    /// <summary>
    /// Represents a page layout configuration for content display
    /// </summary>
    public class Layout : TranslatableEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
    
        /// <summary>
        /// JSON configuration for the layout
        /// </summary>
        [Required]
        public string Configuration { get; set; } = "{}";
        
        /// <summary>
        /// Indicates if this is the default layout
        /// </summary>
        public bool IsDefault { get; set; } = false;
        
        /// <summary>
        /// Indicates if the layout is active
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// URL or path to preview image
        /// </summary>
        [StringLength(500)]
        public string? PreviewImage { get; set; }
        
        // Navigation properties
        public ICollection<Content> Contents { get; set; } = new List<Content>();
    }
}
