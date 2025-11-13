using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    /// <summary>
    /// Junction entity representing a media file in an album
    /// </summary>
    public class AlbumMedia : BaseEntity
    {
        [Required]
        public int AlbumId { get; set; }
    public Album Album { get; set; } = null!;

    [Required]
        public int MediaId { get; set; }
        public Media Media { get; set; } = null!;

   /// <summary>
        /// Display order of media in album
      /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Optional caption for the media
 /// </summary>
        [StringLength(500)]
        public string? Caption { get; set; }

        /// <summary>
 /// Whether this media is featured/highlighted
        /// </summary>
        public bool IsFeatured { get; set; } = false;
    }
}
