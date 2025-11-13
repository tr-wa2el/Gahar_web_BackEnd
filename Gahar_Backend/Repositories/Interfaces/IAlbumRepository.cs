using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Repositories.Interfaces
{
    /// <summary>
 /// Repository interface for Album operations
 /// </summary>
    public interface IAlbumRepository
    {
    /// <summary>
   /// Get all albums with optional filtering
        /// </summary>
        Task<(IEnumerable<Album> Items, int TotalCount)> GetAllAsync(
  bool? isPublished = null,
  string? searchTerm = null,
        int page = 1,
    int pageSize = 20,
     string sortBy = "createdAt",
   string sortOrder = "desc");

      /// <summary>
        /// Get album by ID with media
        /// </summary>
      Task<Album?> GetByIdAsync(int id);

    /// <summary>
        /// Get album by slug
      /// </summary>
   Task<Album?> GetBySlugAsync(string slug);

        /// <summary>
  /// Create album
  /// </summary>
 Task<Album> CreateAsync(Album album);

  /// <summary>
        /// Update album
        /// </summary>
   Task<Album> UpdateAsync(Album album);

 /// <summary>
  /// Delete album
        /// </summary>
     Task<bool> DeleteAsync(int id);

      /// <summary>
      /// Check if slug already exists
     /// </summary>
      Task<bool> SlugExistsAsync(string slug, int? excludeId = null);

        /// <summary>
        /// Get album media by album ID
        /// </summary>
     Task<List<AlbumMedia>> GetAlbumMediaAsync(int albumId);

   /// <summary>
        /// Add media to album
    /// </summary>
   Task AddMediaToAlbumAsync(int albumId, int mediaId, string? caption = null);

 /// <summary>
        /// Remove media from album
    /// </summary>
 Task<bool> RemoveMediaFromAlbumAsync(int albumId, int mediaId);

     /// <summary>
        /// Reorder media in album
 /// </summary>
      Task ReorderAlbumMediaAsync(int albumId, List<int> mediaIds);

        /// <summary>
        /// Increment album view count
     /// </summary>
      Task IncrementViewCountAsync(int albumId);

        /// <summary>
        /// Get published albums
        /// </summary>
      Task<IEnumerable<Album>> GetPublishedAsync(int top = 10);

    /// <summary>
 /// Get albums by user
   /// </summary>
  Task<IEnumerable<Album>> GetByUserAsync(int userId);

     /// <summary>
        /// Get album media count
     /// </summary>
 Task<int> GetMediaCountAsync(int albumId);
    }
}
