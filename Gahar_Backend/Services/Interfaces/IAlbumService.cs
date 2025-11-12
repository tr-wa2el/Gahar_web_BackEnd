using Gahar_Backend.Models.DTOs.Album;

namespace Gahar_Backend.Services.Interfaces
{
    /// <summary>
    /// Service interface for album management
    /// </summary>
    public interface IAlbumService
    {
 /// <summary>
    /// Get all albums with optional filtering
        /// </summary>
   Task<(IEnumerable<AlbumDto> Items, int TotalCount)> GetAllAsync(
   bool? isPublished = null,
      string? searchTerm = null,
    int page = 1,
  int pageSize = 20);

        /// <summary>
  /// Get album by ID with full details
        /// </summary>
        Task<AlbumDetailDto?> GetByIdAsync(int id);

    /// <summary>
        /// Get album by slug
  /// </summary>
 Task<AlbumDetailDto?> GetBySlugAsync(string slug);

     /// <summary>
  /// Create new album
 /// </summary>
        Task<AlbumDto> CreateAsync(CreateAlbumDto dto, int userId);

      /// <summary>
        /// Update album
    /// </summary>
  Task<AlbumDto> UpdateAsync(int id, UpdateAlbumDto dto);

  /// <summary>
     /// Delete album
        /// </summary>
   Task<bool> DeleteAsync(int id);

        /// <summary>
   /// Add media to album
  /// </summary>
  Task<AlbumMediaDto> AddMediaAsync(int albumId, AddMediaToAlbumDto dto);

  /// <summary>
        /// Add multiple media to album
   /// </summary>
    Task<List<AlbumMediaDto>> AddMultipleMediaAsync(int albumId, AddMultipleMediaToAlbumDto dto);

     /// <summary>
   /// Remove media from album
  /// </summary>
   Task<bool> RemoveMediaAsync(int albumId, int mediaId);

      /// <summary>
    /// Reorder media in album
      /// </summary>
    Task<bool> ReorderMediaAsync(int albumId, ReorderAlbumMediaDto dto);

   /// <summary>
      /// Update album media (caption, featured flag)
   /// </summary>
  Task<AlbumMediaDto> UpdateMediaAsync(int albumId, int mediaId, UpdateAlbumMediaDto dto);

        /// <summary>
     /// Set album cover image
        /// </summary>
 Task<AlbumDto> SetCoverImageAsync(int albumId, int mediaId);

   /// <summary>
   /// Increment view count for album
      /// </summary>
       Task IncrementViewCountAsync(int albumId);

     /// <summary>
        /// Get published albums
  /// </summary>
  Task<IEnumerable<AlbumDto>> GetPublishedAsync(int top = 10);

     /// <summary>
  /// Get albums by user
    /// </summary>
   Task<IEnumerable<AlbumDto>> GetByUserAsync(int userId);

/// <summary>
        /// Search albums
        /// </summary>
     Task<IEnumerable<AlbumDto>> SearchAsync(string searchTerm);
    }
}
