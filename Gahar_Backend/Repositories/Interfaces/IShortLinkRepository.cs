using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Repositories.Interfaces;

/// <summary>
/// Repository interface for Short Links
/// </summary>
public interface IShortLinkRepository : IGenericRepository<ShortLink>
{
    /// <summary>
    /// Get short link by short code
    /// </summary>
    Task<ShortLink?> GetByShortCodeAsync(string shortCode);

    /// <summary>
    /// Check if short code exists
  /// </summary>
    Task<bool> ShortCodeExistsAsync(string shortCode);

    /// <summary>
    /// Get short links by user
    /// </summary>
Task<IEnumerable<ShortLink>> GetByUserAsync(int userId, int page = 1, int pageSize = 10);

    /// <summary>
    /// Get short links by category
    /// </summary>
    Task<IEnumerable<ShortLink>> GetByCategoryAsync(string category, int page = 1, int pageSize = 10);

    /// <summary>
    /// Search short links
    /// </summary>
    Task<IEnumerable<ShortLink>> SearchAsync(string searchTerm, int page = 1, int pageSize = 10);

    /// <summary>
    /// Get active short links only
    /// </summary>
    Task<IEnumerable<ShortLink>> GetActiveAsync(int page = 1, int pageSize = 10);

    /// <summary>
    /// Get expired short links
 /// </summary>
    Task<IEnumerable<ShortLink>> GetExpiredAsync();

    /// <summary>
    /// Increment click count
    /// </summary>
    Task IncrementClickCountAsync(int shortLinkId);

    /// <summary>
    /// Update last accessed info
    /// </summary>
    Task UpdateLastAccessedAsync(int shortLinkId, string? country, string? city, string? device);

    /// <summary>
    /// Get top short links by clicks
    /// </summary>
    Task<IEnumerable<ShortLink>> GetTopByClicksAsync(int count = 10);
}

/// <summary>
/// Repository interface for Short Link Analytics
/// </summary>
public interface IShortLinkAnalyticsRepository : IGenericRepository<ShortLinkAnalytics>
{
    /// <summary>
    /// Get analytics for short link
    /// </summary>
    Task<IEnumerable<ShortLinkAnalytics>> GetByShortLinkAsync(int shortLinkId, int page = 1, int pageSize = 50);

    /// <summary>
    /// Get analytics by country
    /// </summary>
    Task<Dictionary<string, int>> GetClicksByCountryAsync(int shortLinkId);

    /// <summary>
    /// Get analytics by device type
 /// </summary>
    Task<Dictionary<string, int>> GetClicksByDeviceAsync(int shortLinkId);

    /// <summary>
    /// Get analytics by browser
    /// </summary>
    Task<Dictionary<string, int>> GetClicksByBrowserAsync(int shortLinkId);

    /// <summary>
    /// Get total clicks for short link
    /// </summary>
    Task<int> GetTotalClicksAsync(int shortLinkId);

    /// <summary>
    /// Get unique clicks for short link
    /// </summary>
    Task<int> GetUniqueClicksAsync(int shortLinkId);

    /// <summary>
    /// Get click timeline for last N days
    /// </summary>
  Task<Dictionary<string, int>> GetClickTimelineAsync(int shortLinkId, int days = 30);

    /// <summary>
    /// Delete old analytics (older than N days)
    /// </summary>
  Task DeleteOldAnalyticsAsync(int days = 90);
}
