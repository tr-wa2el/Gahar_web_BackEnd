using Gahar_Backend.Models.DTOs.Media;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Services.Interfaces
{
    /// <summary>
    /// Service interface for media management
    /// </summary>
  public interface IMediaService
    {
        /// <summary>
  /// Upload single file
    /// </summary>
        Task<MediaDto> UploadAsync(IFormFile file, string? alt = null, int? userId = null);

   /// <summary>
        /// Upload multiple files
    /// </summary>
        Task<IEnumerable<MediaDto>> UploadMultipleAsync(IFormFileCollection files, int? userId = null);

 /// <summary>
        /// Get all media with optional filtering
  /// </summary>
      Task<(IEnumerable<MediaDto> Items, int TotalCount)> GetAllAsync(
 string? mediaType = null,
     string? searchTerm = null,
            int page = 1,
     int pageSize = 20);

      /// <summary>
        /// Get media by ID
 /// </summary>
      Task<MediaDto?> GetByIdAsync(int id);

     /// <summary>
      /// Update media metadata
        /// </summary>
  Task<MediaDto> UpdateAsync(int id, UpdateMediaDto dto);

    /// <summary>
     /// Delete media file
      /// </summary>
       Task<bool> DeleteAsync(int id);

      /// <summary>
 /// Search media by term
        /// </summary>
      Task<IEnumerable<MediaDto>> SearchAsync(string searchTerm);

    /// <summary>
        /// Regenerate WebP for an image
     /// </summary>
    Task<MediaDto> RegenerateWebPAsync(int id, int quality = 85);

      /// <summary>
        /// Validate file before upload
    /// </summary>
        Task<(bool IsValid, string? ErrorMessage)> ValidateFileAsync(IFormFile file);

     /// <summary>
        /// Get media statistics
    /// </summary>
        Task<MediaStatsDto> GetStatsAsync();
    }

    /// <summary>
    /// DTO for media statistics
    /// </summary>
    public class MediaStatsDto
   {
        public int TotalFiles { get; set; }
    public int ImageCount { get; set; }
        public int VideoCount { get; set; }
   public int DocumentCount { get; set; }
    public int AudioCount { get; set; }
        public long TotalStorageSize { get; set; }
  public long WebPTotalSize { get; set; }
    }
}
