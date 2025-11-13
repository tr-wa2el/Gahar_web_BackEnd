using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Media operations
    /// </summary>
    public interface IMediaRepository
    {
        /// <summary>
        /// Get all media files with optional filtering
        /// </summary>
        Task<(IEnumerable<Media> Items, int TotalCount)> GetAllAsync(
   string? mediaType = null,
       string? searchTerm = null,
     int page = 1,
         int pageSize = 20,
       string sortBy = "createdAt",
         string sortOrder = "desc");

 /// <summary>
   /// Get media by ID
   /// </summary>
     Task<Media?> GetByIdAsync(int id);

        /// <summary>
        /// Create media record
/// </summary>
  Task<Media> CreateAsync(Media media);

        /// <summary>
        /// Update media record
 /// </summary>
      Task<Media> UpdateAsync(Media media);

        /// <summary>
        /// Delete media record
        /// </summary>
   Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Check if file name already exists
        /// </summary>
    Task<bool> FileNameExistsAsync(string fileName, int? excludeId = null);

   /// <summary>
        /// Get media by file path
      /// </summary>
        Task<Media?> GetByFilePathAsync(string filePath);

        /// <summary>
   /// Get media count by type
        /// </summary>
     Task<int> GetCountByTypeAsync(string mediaType);

        /// <summary>
        /// Search media by file name or alt text
        /// </summary>
        Task<IEnumerable<Media>> SearchAsync(string searchTerm);

        /// <summary>
    /// Get media by uploaded user
     /// </summary>
        Task<IEnumerable<Media>> GetByUserAsync(int userId);
    }
}
