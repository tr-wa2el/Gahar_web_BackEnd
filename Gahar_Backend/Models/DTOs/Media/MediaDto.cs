using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Media
{
    /// <summary>
    /// DTO for Media response
    /// </summary>
    public class MediaDto
    {
     public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
 public string? ThumbnailPath { get; set; }
  public string? WebPPath { get; set; }
     public string MimeType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public long? WebPFileSize { get; set; }
    public int? Width { get; set; }
        public int? Height { get; set; }
        public string? Alt { get; set; }
        public string MediaType { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public bool IsProcessed { get; set; }
  public int? UploadedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

   /// <summary>
        /// Compression ratio percentage (WebP vs Original)
 /// </summary>
        public double? CompressionRatio
        {
      get
      {
                if (FileSize == 0 || WebPFileSize == null)
  return null;

      return Math.Round((1 - (double)WebPFileSize / FileSize) * 100, 2);
            }
        }
    }

    /// <summary>
    /// DTO for file upload
    /// </summary>
    public class MediaUploadDto
    {
        [Required(ErrorMessage = "File is required")]
 public IFormFile? File { get; set; }

      [StringLength(500, ErrorMessage = "Alt text cannot exceed 500 characters")]
        public string? Alt { get; set; }

        /// <summary>
 /// Optional: Specify media type (auto-detected if not provided)
     /// </summary>
        [StringLength(50)]
        public string? MediaType { get; set; }
    }

    /// <summary>
    /// DTO for bulk file upload
    /// </summary>
    public class BulkMediaUploadDto
    {
        [Required(ErrorMessage = "At least one file is required")]
    public IFormFileCollection? Files { get; set; }

        [StringLength(50)]
        public string? MediaType { get; set; }
    }

    /// <summary>
    /// DTO for updating media metadata
    /// </summary>
    public class UpdateMediaDto
    {
        [StringLength(255, ErrorMessage = "File name cannot exceed 255 characters")]
        public string? FileName { get; set; }

    [StringLength(500, ErrorMessage = "Alt text cannot exceed 500 characters")]
 public string? Alt { get; set; }
    }

    /// <summary>
 /// DTO for media search/filter
    /// </summary>
    public class MediaFilterDto
    {
      [StringLength(50)]
        public string? MediaType { get; set; }

        [StringLength(100)]
        public string? SearchTerm { get; set; }

        public int Page { get; set; } = 1;

      public int PageSize { get; set; } = 20;

        public string? SortBy { get; set; } = "createdAt";

        public string? SortOrder { get; set; } = "desc";
    }

    /// <summary>
    /// DTO for media summary (minimal info)
    /// </summary>
    public class MediaSummaryDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
      public string? ThumbnailPath { get; set; }
        public string? WebPPath { get; set; }
   public int? Width { get; set; }
        public int? Height { get; set; }
        public string MediaType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO for regenerate WebP
    /// </summary>
    public class RegenerateWebPDto
    {
  /// <summary>
        /// Quality for WebP (1-100, default 85)
        /// </summary>
        [Range(1, 100)]
        public int Quality { get; set; } = 85;
    }
}
