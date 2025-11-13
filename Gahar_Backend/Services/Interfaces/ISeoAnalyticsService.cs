using Gahar_Backend.Models.DTOs.Seo;

namespace Gahar_Backend.Services.Interfaces;

public interface ISeoAnalyticsService
{
    // SEO Metadata
    Task<SeoMetadataDto> GetSeoMetadataAsync(string entityType, int entityId);
    Task<SeoMetadataDto> UpdateSeoMetadataAsync(string entityType, int entityId, UpdateSeoMetadataDto dto);
    Task<SeoMetadataDto> CreateSeoMetadataAsync(string entityType, int entityId, UpdateSeoMetadataDto dto);

    // Page Analytics
    Task<PageAnalyticsDto> GetPageAnalyticsAsync(string pageUrl);
    Task<IEnumerable<PageAnalyticsDto>> GetTopPagesAsync(int count = 10);
    Task<PageAnalyticsDto> RecordPageViewAsync(string pageUrl, string? pageName = null);

    // Analytics Events
    Task TrackEventAsync(TrackEventDto dto, string? ipAddress = null, string? userAgent = null, int? userId = null, string? sessionId = null);
    Task<IEnumerable<AnalyticsEventDto>> GetEventsByTypeAsync(string eventType, int take = 100);
    Task<IEnumerable<AnalyticsEventDto>> GetSessionEventsAsync(string sessionId);

    // Keywords
    Task<KeywordDto> AddKeywordAsync(CreateKeywordDto dto);
    Task<KeywordDto> UpdateKeywordAsync(int keywordId, CreateKeywordDto dto);
    Task<IEnumerable<KeywordDto>> SearchKeywordsAsync(string searchTerm);
    Task<IEnumerable<KeywordDto>> GetTargetedKeywordsAsync();
}
