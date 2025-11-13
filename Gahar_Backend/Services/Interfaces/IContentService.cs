using Gahar_Backend.Models.DTOs.Content;

namespace Gahar_Backend.Services.Interfaces;

/// <summary>
/// خدمة إدارة المحتوى الديناميكي
/// </summary>
public interface IContentService
{
    // Content Management
    Task<PagedResult<ContentListDto>> GetAllAsync(ContentFilterDto filter);
    Task<ContentDetailDto> GetByIdAsync(int id, string? language = null);
    Task<ContentDetailDto> GetBySlugAsync(string slug, string contentType, string? language = null);
    Task<ContentDto> CreateAsync(CreateContentDto dto, int userId);
    Task<ContentDto> UpdateAsync(int id, UpdateContentDto dto);
    Task DeleteAsync(int id);

 // Publishing
    Task PublishAsync(int id);
    Task UnpublishAsync(int id);
   Task ScheduleAsync(int id, DateTime publishDate);
    Task<IEnumerable<ContentListDto>> GetScheduledContentAsync();

    // Features
    Task DuplicateAsync(int id, int userId);
   Task MoveToContentTypeAsync(int id, int targetContentTypeId);
    Task IncrementViewsAsync(int id);

 // Search & Filter
    Task<PagedResult<ContentListDto>> SearchAsync(string searchTerm, ContentFilterDto filter);
    Task<IEnumerable<ContentListDto>> GetFeaturedAsync(int count = 10);
    Task<IEnumerable<ContentListDto>> GetRecentAsync(int count = 10);
}
