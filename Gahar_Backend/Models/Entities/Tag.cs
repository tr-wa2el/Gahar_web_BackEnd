using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    /// <summary>
    /// Represents a tag for categorizing content
    /// </summary>
    public class Tag : TranslatableEntity
    {
        /// <summary>
        /// Tag name
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// URL-friendly slug
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Tag description
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Tag color for UI display
        /// </summary>
        [StringLength(20)]
        public string? Color { get; set; }

        /// <summary>
        /// Number of times this tag has been used
        /// </summary>
        public int UsageCount { get; set; } = 0;

        /// <summary>
        /// Content items with this tag
        /// </summary>
        public ICollection<ContentTag> ContentTags { get; set; } = new List<ContentTag>();
    }
}
