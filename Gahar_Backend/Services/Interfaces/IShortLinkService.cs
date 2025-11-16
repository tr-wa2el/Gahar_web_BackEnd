using Gahar_Backend.Models.DTOs.ShortLinks;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Services.Interfaces;

/// <summary>
/// Service interface for Short Links management
/// </summary>
public interface IShortLinkService
{
    /// <summary>
    /// Create new short link
    /// </summary>
    Task<ShortLinkDto> CreateShortLinkAsync(CreateShortLinkDto dto, int userId);

    /// <summary>
    /// Get short link by ID
    /// </summary>
    Task<ShortLinkDto> GetShortLinkAsync(int id);

    /// <summary>
    /// Get short link by short code
    /// </summary>
    Task<ShortLinkDto> GetShortLinkByCodeAsync(string shortCode);

    /// <summary>
    /// Update short link
    /// </summary>
    Task<ShortLinkDto> UpdateShortLinkAsync(int id, UpdateShortLinkDto dto, int userId);

    /// <summary>
  /// Delete short link
    /// </summary>
    Task DeleteShortLinkAsync(int id, int userId);

    /// <summary>
    /// Get user's short links
    /// </summary>
    Task<IEnumerable<ShortLinkDto>> GetUserShortLinksAsync(int userId, int page = 1, int pageSize = 10);

    /// <summary>
    /// Search short links
    /// </summary>
    Task<IEnumerable<ShortLinkDto>> SearchShortLinksAsync(string searchTerm, int page = 1, int pageSize = 10);

    /// <summary>
    /// Get short links by category
    /// </summary>
    Task<IEnumerable<ShortLinkDto>> GetShortLinksByCategoryAsync(string category, int page = 1, int pageSize = 10);

    /// <summary>
    /// Get active short links
  /// </summary>
    Task<IEnumerable<ShortLinkDto>> GetActiveShortLinksAsync(int page = 1, int pageSize = 10);

    /// <summary>
    /// Record click on short link
    /// </summary>
    Task RecordClickAsync(int shortLinkId, ShortLinkAnalyticsDto analyticsDto);

    /// <summary>
    /// Get short link statistics
    /// </summary>
    Task<ShortLinkStatisticsDto> GetStatisticsAsync(int shortLinkId);

    /// <summary>
    /// Generate unique short code
    /// </summary>
    Task<string> GenerateUniqueShortCodeAsync(int length = 6);

    /// <summary>
    /// Get top short links
    /// </summary>
    Task<IEnumerable<ShortLinkDto>> GetTopShortLinksAsync(int count = 10);

    /// <summary>
    /// Regenerate QR code
    /// </summary>
    Task<QrCodeResultDto> RegenerateQrCodeAsync(int shortLinkId, string? logoUrl = null);
}
