using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class SeoMetadataRepository : GenericRepository<SeoMetadata>, ISeoMetadataRepository
{
    public SeoMetadataRepository(ApplicationDbContext context) : base(context) { }

    public async Task<SeoMetadata?> GetByEntityAsync(string entityType, int entityId)
    {
        return await _context.SeoMetadata
        .FirstOrDefaultAsync(s => s.EntityType == entityType && s.EntityId == entityId);
  }

    public IQueryable<SeoMetadata> GetQueryable() => _context.SeoMetadata.AsQueryable();
    public async Task AddAsync(SeoMetadata entity) => await _context.SeoMetadata.AddAsync(entity);
    public void Update(SeoMetadata entity) => _context.SeoMetadata.Update(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}

public class PageAnalyticsRepository : GenericRepository<PageAnalytics>, IPageAnalyticsRepository
{
  public PageAnalyticsRepository(ApplicationDbContext context) : base(context) { }

    public async Task<PageAnalytics?> GetByPageUrlAsync(string pageUrl)
    {
        return await _context.PageAnalytics.FirstOrDefaultAsync(p => p.PageUrl == pageUrl);
    }

    public async Task<IEnumerable<PageAnalytics>> GetTopPagesAsync(int count = 10)
    {
        return await _context.PageAnalytics
            .OrderByDescending(p => p.PageViews)
   .Take(count)
        .ToListAsync();
    }

    public IQueryable<PageAnalytics> GetQueryable() => _context.PageAnalytics.AsQueryable();
    public async Task AddAsync(PageAnalytics entity) => await _context.PageAnalytics.AddAsync(entity);
    public void Update(PageAnalytics entity) => _context.PageAnalytics.Update(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}

public class AnalyticsEventRepository : GenericRepository<AnalyticsEvent>, IAnalyticsEventRepository
{
    public AnalyticsEventRepository(ApplicationDbContext context) : base(context) { }

  public async Task<IEnumerable<AnalyticsEvent>> GetEventsByTypeAsync(string eventType, int take = 100)
    {
      return await _context.AnalyticsEvents
     .Where(e => e.EventType == eventType)
            .OrderByDescending(e => e.CreatedAt)
 .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<AnalyticsEvent>> GetEventsBySessionAsync(string sessionId)
    {
        return await _context.AnalyticsEvents
       .Where(e => e.SessionId == sessionId)
            .OrderByDescending(e => e.CreatedAt)
       .ToListAsync();
    }

    public IQueryable<AnalyticsEvent> GetQueryable() => _context.AnalyticsEvents.AsQueryable();
    public async Task AddAsync(AnalyticsEvent entity) => await _context.AnalyticsEvents.AddAsync(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}

public class KeywordRepository : GenericRepository<Keyword>, IKeywordRepository
{
    public KeywordRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Keyword?> GetByTermAsync(string term)
    {
     return await _context.Keywords.FirstOrDefaultAsync(k => k.Term == term);
 }

    public async Task<IEnumerable<Keyword>> SearchByTermAsync(string searchTerm)
{
        return await _context.Keywords
   .Where(k => k.Term.Contains(searchTerm))
            .ToListAsync();
    }

    public async Task<IEnumerable<Keyword>> GetTargetedKeywordsAsync()
    {
        return await _context.Keywords
     .Where(k => k.IsTargeted)
      .OrderByDescending(k => k.SearchVolume)
            .ToListAsync();
    }

    public IQueryable<Keyword> GetQueryable() => _context.Keywords.AsQueryable();
    public async Task AddAsync(Keyword entity) => await _context.Keywords.AddAsync(entity);
    public void Update(Keyword entity) => _context.Keywords.Update(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
