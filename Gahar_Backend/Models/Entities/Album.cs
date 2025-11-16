using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    /// <summary>
    /// Represents an album of media files (photos, images)
    /// </summary>
    public class Album : TranslatableEntity
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

    [Required]
      [StringLength(200)]
        public string Slug { get; set; } = string.Empty;

    [StringLength(1000)]
        public string? Description { get; set; }

        /// <summary>
      /// Cover image for the album
        /// </summary>
 public int? CoverImageId { get; set; }
     public Media? CoverImage { get; set; }

        /// <summary>
        /// Whether the album is published and visible to public
        /// </summary>
        public bool IsPublished { get; set; } = false;

      public DateTime? PublishedAt { get; set; }

 /// <summary>
        /// Number of views for this album
        /// </summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// User who created the album
        /// </summary>
        public int? CreatedBy { get; set; }
        public User? Creator { get; set; }

        // Navigation properties
        public ICollection<AlbumMedia> AlbumMedias { get; set; } = new List<AlbumMedia>();
    }
}
