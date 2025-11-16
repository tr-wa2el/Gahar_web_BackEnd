using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Repositories.Interfaces;

public interface ISeoMetadataRepository : IGenericRepository<SeoMetadata>
{
    Task<SeoMetadata?> GetByEntityAsync(string entityType, int entityId);
    IQueryable<SeoMetadata> GetQueryable();
    Task AddAsync(SeoMetadata entity);
    void Update(SeoMetadata entity);
    Task SaveChangesAsync();
}

public interface IPageAnalyticsRepository : IGenericRepository<PageAnalytics>
{
    Task<PageAnalytics?> GetByPageUrlAsync(string pageUrl);
    Task<IEnumerable<PageAnalytics>> GetTopPagesAsync(int count = 10);
    IQueryable<PageAnalytics> GetQueryable();
    Task AddAsync(PageAnalytics entity);
    void Update(PageAnalytics entity);
    Task SaveChangesAsync();
}

public interface IAnalyticsEventRepository : IGenericRepository<AnalyticsEvent>
{
    Task<IEnumerable<AnalyticsEvent>> GetEventsByTypeAsync(string eventType, int take = 100);
    Task<IEnumerable<AnalyticsEvent>> GetEventsBySessionAsync(string sessionId);
    IQueryable<AnalyticsEvent> GetQueryable();
    Task AddAsync(AnalyticsEvent entity);
    Task SaveChangesAsync();
}

public interface IKeywordRepository : IGenericRepository<Keyword>
{
    Task<Keyword?> GetByTermAsync(string term);
    Task<IEnumerable<Keyword>> SearchByTermAsync(string searchTerm);
    Task<IEnumerable<Keyword>> GetTargetedKeywordsAsync();
    IQueryable<Keyword> GetQueryable();
    Task AddAsync(Keyword entity);
    void Update(Keyword entity);
    Task SaveChangesAsync();
}
