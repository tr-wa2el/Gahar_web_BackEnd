using Gahar_Backend.Models.DTOs.Seo;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Implementations;

public class SeoAnalyticsService : ISeoAnalyticsService
{
  private readonly ISeoMetadataRepository _seoRepo;
    private readonly IPageAnalyticsRepository _analyticsRepo;
    private readonly IAnalyticsEventRepository _eventRepo;
    private readonly IKeywordRepository _keywordRepo;

    public SeoAnalyticsService(
        ISeoMetadataRepository seoRepo,
        IPageAnalyticsRepository analyticsRepo,
        IAnalyticsEventRepository eventRepo,
        IKeywordRepository keywordRepo)
    {
  _seoRepo = seoRepo;
        _analyticsRepo = analyticsRepo;
        _eventRepo = eventRepo;
        _keywordRepo = keywordRepo;
    }

    #region SEO Metadata

    public async Task<SeoMetadataDto> GetSeoMetadataAsync(string entityType, int entityId)
    {
   var seo = await _seoRepo.GetByEntityAsync(entityType, entityId);
        if (seo == null)
  throw new NotFoundException("SEO metadata not found");

     return MapToDto(seo);
    }

    public async Task<SeoMetadataDto> UpdateSeoMetadataAsync(string entityType, int entityId, UpdateSeoMetadataDto dto)
    {
        var seo = await _seoRepo.GetByEntityAsync(entityType, entityId);
     if (seo == null)
   throw new NotFoundException("SEO metadata not found");

        seo.PageTitle = dto.PageTitle;
        seo.MetaDescription = dto.MetaDescription;
        seo.MetaKeywords = dto.MetaKeywords;
   seo.OgTitle = dto.OgTitle;
        seo.OgDescription = dto.OgDescription;
        seo.OgImage = dto.OgImage;
    seo.CanonicalUrl = dto.CanonicalUrl;
  seo.IsIndexable = dto.IsIndexable;
        seo.IsFollowable = dto.IsFollowable;
        seo.LastOptimizedAt = DateTime.UtcNow;

  _seoRepo.Update(seo);
    await _seoRepo.SaveChangesAsync();

        return MapToDto(seo);
    }

    public async Task<SeoMetadataDto> CreateSeoMetadataAsync(string entityType, int entityId, UpdateSeoMetadataDto dto)
 {
        var seo = new SeoMetadata
        {
   EntityType = entityType,
   EntityId = entityId,
     PageTitle = dto.PageTitle,
          MetaDescription = dto.MetaDescription,
 MetaKeywords = dto.MetaKeywords,
            OgTitle = dto.OgTitle,
      OgDescription = dto.OgDescription,
            OgImage = dto.OgImage,
  CanonicalUrl = dto.CanonicalUrl,
            IsIndexable = dto.IsIndexable,
            IsFollowable = dto.IsFollowable,
   LastOptimizedAt = DateTime.UtcNow
   };

   await _seoRepo.AddAsync(seo);
        await _seoRepo.SaveChangesAsync();

        return MapToDto(seo);
    }

    #endregion

    #region Page Analytics

    public async Task<PageAnalyticsDto> GetPageAnalyticsAsync(string pageUrl)
    {
  var analytics = await _analyticsRepo.GetByPageUrlAsync(pageUrl);
  if (analytics == null)
          throw new NotFoundException("Analytics not found");

        return MapAnalyticsToDto(analytics);
    }

    public async Task<IEnumerable<PageAnalyticsDto>> GetTopPagesAsync(int count = 10)
    {
        var pages = await _analyticsRepo.GetTopPagesAsync(count);
        return pages.Select(MapAnalyticsToDto);
    }

    public async Task<PageAnalyticsDto> RecordPageViewAsync(string pageUrl, string? pageName = null)
    {
        var analytics = await _analyticsRepo.GetByPageUrlAsync(pageUrl);
    
    if (analytics == null)
        {
       analytics = new PageAnalytics
            {
    PageUrl = pageUrl,
      PageName = pageName,
   PageViews = 1,
  UniqueVisitors = 1,
    LastAnalyzedAt = DateTime.UtcNow
            };
            await _analyticsRepo.AddAsync(analytics);
        }
        else
        {
 analytics.PageViews++;
       analytics.LastAnalyzedAt = DateTime.UtcNow;
     _analyticsRepo.Update(analytics);
        }

    await _analyticsRepo.SaveChangesAsync();
        return MapAnalyticsToDto(analytics);
    }

    #endregion

    #region Analytics Events

    public async Task TrackEventAsync(TrackEventDto dto, string? ipAddress = null, string? userAgent = null, int? userId = null, string? sessionId = null)
    {
    var analyticsEvent = new AnalyticsEvent
        {
    EventType = dto.EventType,
            PageUrl = dto.PageUrl,
          EventData = dto.EventData,
  SourceName = dto.SourceName,
   IpAddress = ipAddress,
         UserAgent = userAgent,
   UserId = userId,
            SessionId = sessionId
        };

      await _eventRepo.AddAsync(analyticsEvent);
 await _eventRepo.SaveChangesAsync();
    }

public async Task<IEnumerable<AnalyticsEventDto>> GetEventsByTypeAsync(string eventType, int take = 100)
    {
    var events = await _eventRepo.GetEventsByTypeAsync(eventType, take);
      return events.Select(MapEventToDto);
    }

    public async Task<IEnumerable<AnalyticsEventDto>> GetSessionEventsAsync(string sessionId)
    {
        var events = await _eventRepo.GetEventsBySessionAsync(sessionId);
 return events.Select(MapEventToDto);
    }

    #endregion

    #region Keywords

    public async Task<KeywordDto> AddKeywordAsync(CreateKeywordDto dto)
    {
        var existing = await _keywordRepo.GetByTermAsync(dto.Term);
        if (existing != null)
     throw new BadRequestException($"Keyword '{dto.Term}' already exists");

        var keyword = new Keyword
      {
     Term = dto.Term,
      Description = dto.Description,
            SearchVolume = dto.SearchVolume,
 Difficulty = dto.Difficulty,
  IsTargeted = dto.IsTargeted,
      TargetEntity = dto.TargetEntity,
            TargetEntityId = dto.TargetEntityId,
      LastUpdatedAt = DateTime.UtcNow
        };

        await _keywordRepo.AddAsync(keyword);
        await _keywordRepo.SaveChangesAsync();

     return MapKeywordToDto(keyword);
    }

   public async Task<KeywordDto> UpdateKeywordAsync(int keywordId, CreateKeywordDto dto)
    {
        var keyword = await _keywordRepo.GetByIdAsync(keywordId);
        if (keyword == null)
         throw new NotFoundException("Keyword not found");

        keyword.Term = dto.Term;
     keyword.Description = dto.Description;
    keyword.SearchVolume = dto.SearchVolume;
   keyword.Difficulty = dto.Difficulty;
      keyword.IsTargeted = dto.IsTargeted;
        keyword.TargetEntity = dto.TargetEntity;
        keyword.TargetEntityId = dto.TargetEntityId;
        keyword.LastUpdatedAt = DateTime.UtcNow;

     _keywordRepo.Update(keyword);
     await _keywordRepo.SaveChangesAsync();

        return MapKeywordToDto(keyword);
    }

    public async Task<IEnumerable<KeywordDto>> SearchKeywordsAsync(string searchTerm)
{
        var keywords = await _keywordRepo.SearchByTermAsync(searchTerm);
    return keywords.Select(MapKeywordToDto);
    }

    public async Task<IEnumerable<KeywordDto>> GetTargetedKeywordsAsync()
    {
        var keywords = await _keywordRepo.GetTargetedKeywordsAsync();
  return keywords.Select(MapKeywordToDto);
    }

    #endregion

    #region Helper Methods

    private SeoMetadataDto MapToDto(SeoMetadata seo)
    {
        return new SeoMetadataDto
        {
  Id = seo.Id,
  PageTitle = seo.PageTitle,
            MetaDescription = seo.MetaDescription,
            MetaKeywords = seo.MetaKeywords,
          OgTitle = seo.OgTitle,
        OgDescription = seo.OgDescription,
            OgImage = seo.OgImage,
     CanonicalUrl = seo.CanonicalUrl,
     IsIndexable = seo.IsIndexable,
            IsFollowable = seo.IsFollowable,
            EntityType = seo.EntityType,
         EntityId = seo.EntityId,
            LastOptimizedAt = seo.LastOptimizedAt,
   SeoScore = seo.SeoScore,
     Recommendations = seo.Recommendations,
      CreatedAt = seo.CreatedAt
        };
    }

    private PageAnalyticsDto MapAnalyticsToDto(PageAnalytics analytics)
    {
        return new PageAnalyticsDto
        {
  Id = analytics.Id,
    PageUrl = analytics.PageUrl,
      PageName = analytics.PageName,
      PageViews = analytics.PageViews,
      UniqueVisitors = analytics.UniqueVisitors,
   AverageTimeOnPage = analytics.AverageTimeOnPage,
 BounceRate = analytics.BounceRate,
    ClickCount = analytics.ClickCount,
            TopReferrer = analytics.TopReferrer,
       TopDevice = analytics.TopDevice,
   LastAnalyzedAt = analytics.LastAnalyzedAt,
   ConversionCount = analytics.ConversionCount,
  ConversionRate = analytics.ConversionRate
      };
    }

    private AnalyticsEventDto MapEventToDto(AnalyticsEvent analyticsEvent)
    {
        return new AnalyticsEventDto
      {
      Id = analyticsEvent.Id,
            EventType = analyticsEvent.EventType,
          EventData = analyticsEvent.EventData,
      PageUrl = analyticsEvent.PageUrl,
    UserAgent = analyticsEvent.UserAgent,
     CountryCode = analyticsEvent.CountryCode,
            CityName = analyticsEvent.CityName,
    DeviceType = analyticsEvent.DeviceType,
   BrowserName = analyticsEvent.BrowserName,
     SourceName = analyticsEvent.SourceName,
   SessionId = analyticsEvent.SessionId,
          CreatedAt = analyticsEvent.CreatedAt
        };
    }

    private KeywordDto MapKeywordToDto(Keyword keyword)
    {
   return new KeywordDto
        {
      Id = keyword.Id,
  Term = keyword.Term,
  Description = keyword.Description,
      SearchVolume = keyword.SearchVolume,
    Difficulty = keyword.Difficulty,
            SearchIntent = keyword.SearchIntent,
RankingPages = keyword.RankingPages,
       IsTargeted = keyword.IsTargeted,
            TargetEntity = keyword.TargetEntity,
         TargetEntityId = keyword.TargetEntityId,
      Impressions = keyword.Impressions,
            Clicks = keyword.Clicks,
     ClickThroughRate = keyword.ClickThroughRate,
            AveragePosition = keyword.AveragePosition
        };
    }

    #endregion
}
