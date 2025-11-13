using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    /// <summary>
  /// Represents a media file (image, video, document, audio)
    /// </summary>
    public class Media : BaseEntity
    {
        [Required]
   [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Original file path relative to wwwroot
  /// </summary>
        [Required]
   [StringLength(500)]
    public string FilePath { get; set; } = string.Empty;

        /// <summary>
      /// Thumbnail path (for images only)
     /// </summary>
    [StringLength(500)]
        public string? ThumbnailPath { get; set; }

        /// <summary>
     /// WebP version path (for images only)
        /// </summary>
     [StringLength(500)]
 public string? WebPPath { get; set; }

        [Required]
        [StringLength(50)]
     public string MimeType { get; set; } = string.Empty;

        /// <summary>
  /// File size in bytes
        /// </summary>
  public long FileSize { get; set; }

        /// <summary>
        /// WebP file size in bytes (for images only)
  /// </summary>
    public long? WebPFileSize { get; set; }

        /// <summary>
/// Image width in pixels (for images only)
        /// </summary>
   public int? Width { get; set; }

        /// <summary>
        /// Image height in pixels (for images only)
   /// </summary>
    public int? Height { get; set; }

        /// <summary>
 /// Alt text for images
     /// </summary>
        [StringLength(500)]
      public string? Alt { get; set; }

        /// <summary>
        /// Media type: Image, Video, Document, Audio
        /// </summary>
        [Required]
        [StringLength(50)]
     public string MediaType { get; set; } = string.Empty;

        /// <summary>
        /// User ID who uploaded the file
      /// </summary>
        public int? UploadedBy { get; set; }

        /// <summary>
        /// Whether the file has been processed (thumbnail + WebP created)
        /// </summary>
   public bool IsProcessed { get; set; } = false;

        /// <summary>
        /// Original file extension
        /// </summary>
     [StringLength(10)]
   public string FileExtension { get; set; } = string.Empty;

        // Navigation properties
        public User? UploadedByUser { get; set; }
    }
}
