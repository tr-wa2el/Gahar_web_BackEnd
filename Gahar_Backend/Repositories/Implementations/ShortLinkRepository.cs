using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

/// <summary>
/// Repository implementation for Short Links
/// </summary>
public class ShortLinkRepository : GenericRepository<ShortLink>, IShortLinkRepository
{
    public ShortLinkRepository(ApplicationDbContext context) : base(context) { }

    public async Task<ShortLink?> GetByShortCodeAsync(string shortCode)
    {
  return await _context.ShortLinks
   .Include(s => s.CreatedByUser)
       .FirstOrDefaultAsync(s => s.ShortCode == shortCode);
    }

    public async Task<bool> ShortCodeExistsAsync(string shortCode)
    {
   return await _context.ShortLinks
    .AnyAsync(s => s.ShortCode == shortCode);
 }

    public async Task<IEnumerable<ShortLink>> GetByUserAsync(int userId, int page = 1, int pageSize = 10)
    {
        return await _context.ShortLinks
 .Where(s => s.CreatedByUserId == userId && !s.IsDeleted)
            .OrderByDescending(s => s.CreatedAt)
     .Skip((page - 1) * pageSize)
    .Take(pageSize)
.ToListAsync();
    }

    public async Task<IEnumerable<ShortLink>> GetByCategoryAsync(string category, int page = 1, int pageSize = 10)
    {
        return await _context.ShortLinks
      .Where(s => s.Category == category && s.IsActive && !s.IsDeleted)
            .OrderByDescending(s => s.CreatedAt)
      .Skip((page - 1) * pageSize)
            .Take(pageSize)
    .ToListAsync();
  }

    public async Task<IEnumerable<ShortLink>> SearchAsync(string searchTerm, int page = 1, int pageSize = 10)
    {
      return await _context.ShortLinks
         .Where(s => (s.Title != null && s.Title.Contains(searchTerm)) ||
 (s.Description != null && s.Description.Contains(searchTerm)) ||
    s.ShortCode.Contains(searchTerm) ||
     s.OriginalUrl.Contains(searchTerm))
            .OrderByDescending(s => s.CreatedAt)
      .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

public async Task<IEnumerable<ShortLink>> GetActiveAsync(int page = 1, int pageSize = 10)
    {
        return await _context.ShortLinks
       .Where(s => s.IsActive && !s.IsDeleted && 
     (s.ExpiryDate == null || s.ExpiryDate > DateTime.UtcNow))
   .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
       .Take(pageSize)
  .ToListAsync();
    }

    public async Task<IEnumerable<ShortLink>> GetExpiredAsync()
    {
        return await _context.ShortLinks
   .Where(s => s.ExpiryDate != null && s.ExpiryDate <= DateTime.UtcNow)
       .ToListAsync();
    }

    public async Task IncrementClickCountAsync(int shortLinkId)
    {
var shortLink = await _context.ShortLinks.FindAsync(shortLinkId);
        if (shortLink != null)
        {
     shortLink.ClickCount++;
     _context.ShortLinks.Update(shortLink);
        await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateLastAccessedAsync(int shortLinkId, string? country, string? city, string? device)
    {
        var shortLink = await _context.ShortLinks.FindAsync(shortLinkId);
    if (shortLink != null)
    {
  shortLink.LastAccessedAt = DateTime.UtcNow;
        shortLink.LastAccessCountry = country;
 shortLink.LastAccessCity = city;
            shortLink.LastAccessDevice = device;
  _context.ShortLinks.Update(shortLink);
   await _context.SaveChangesAsync();
      }
    }

    public async Task<IEnumerable<ShortLink>> GetTopByClicksAsync(int count = 10)
    {
        return await _context.ShortLinks
            .Where(s => s.IsActive && !s.IsDeleted)
          .OrderByDescending(s => s.ClickCount)
     .Take(count)
            .ToListAsync();
    }
}

/// <summary>
/// Repository implementation for Short Link Analytics
/// </summary>
public class ShortLinkAnalyticsRepository : GenericRepository<ShortLinkAnalytics>, IShortLinkAnalyticsRepository
{
    public ShortLinkAnalyticsRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<ShortLinkAnalytics>> GetByShortLinkAsync(int shortLinkId, int page = 1, int pageSize = 50)
    {
        return await _context.ShortLinkAnalytics
            .Where(a => a.ShortLinkId == shortLinkId)
   .OrderByDescending(a => a.ClickTime)
     .Skip((page - 1) * pageSize)
  .Take(pageSize)
     .ToListAsync();
    }

    public async Task<Dictionary<string, int>> GetClicksByCountryAsync(int shortLinkId)
    {
        return await _context.ShortLinkAnalytics
            .Where(a => a.ShortLinkId == shortLinkId && a.Country != null)
            .GroupBy(a => a.Country)
         .Select(g => new { g.Key, Count = g.Count() })
  .ToDictionaryAsync(x => x.Key ?? "Unknown", x => x.Count);
    }

    public async Task<Dictionary<string, int>> GetClicksByDeviceAsync(int shortLinkId)
    {
     return await _context.ShortLinkAnalytics
        .Where(a => a.ShortLinkId == shortLinkId && a.DeviceType != null)
            .GroupBy(a => a.DeviceType)
    .Select(g => new { g.Key, Count = g.Count() })
   .ToDictionaryAsync(x => x.Key ?? "Unknown", x => x.Count);
    }

    public async Task<Dictionary<string, int>> GetClicksByBrowserAsync(int shortLinkId)
    {
     return await _context.ShortLinkAnalytics
       .Where(a => a.ShortLinkId == shortLinkId && a.BrowserName != null)
            .GroupBy(a => a.BrowserName)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Key ?? "Unknown", x => x.Count);
    }

    public async Task<int> GetTotalClicksAsync(int shortLinkId)
    {
        return await _context.ShortLinkAnalytics
         .Where(a => a.ShortLinkId == shortLinkId)
            .CountAsync();
    }

    public async Task<int> GetUniqueClicksAsync(int shortLinkId)
    {
        return await _context.ShortLinkAnalytics
      .Where(a => a.ShortLinkId == shortLinkId)
            .Select(a => a.IpAddress)
  .Distinct()
            .CountAsync();
    }

    public async Task<Dictionary<string, int>> GetClickTimelineAsync(int shortLinkId, int days = 30)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);

        return await _context.ShortLinkAnalytics
            .Where(a => a.ShortLinkId == shortLinkId && a.ClickTime >= startDate)
          .GroupBy(a => a.ClickTime.Date)
   .Select(g => new { Date = g.Key.ToString("yyyy-MM-dd"), Count = g.Count() })
            .OrderBy(x => x.Date)
  .ToDictionaryAsync(x => x.Date, x => x.Count);
    }

    public async Task DeleteOldAnalyticsAsync(int days = 90)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-days);
        var oldAnalytics = await _context.ShortLinkAnalytics
            .Where(a => a.ClickTime < cutoffDate)
    .ToListAsync();

        _context.ShortLinkAnalytics.RemoveRange(oldAnalytics);
 await _context.SaveChangesAsync();
    }
}
