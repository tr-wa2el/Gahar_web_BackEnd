using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class PageRepository : GenericRepository<Page>, IPageRepository
{
    public PageRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Page?> GetBySlugAsync(string slug, string? lang = "ar")
    {
        return await _context.Pages
            .Include(p => p.Author)
 .Include(p => p.Blocks.OrderBy(b => b.DisplayOrder))
  .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
    {
        var query = _context.Pages.Where(p => p.Slug == slug);
 if (excludeId.HasValue)
   {
            query = query.Where(p => p.Id != excludeId.Value);
      }
        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Page>> GetPublishedPagesAsync()
    {
  return await _context.Pages
      .Include(p => p.Author)
            .Include(p => p.Blocks.OrderBy(b => b.DisplayOrder))
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.PublishedAt)
 .ToListAsync();
  }

    public async Task<IEnumerable<Page>> GetByAuthorAsync(int authorId)
    {
        return await _context.Pages
      .Include(p => p.Blocks)
            .Where(p => p.AuthorId == authorId)
         .OrderByDescending(p => p.CreatedAt)
  .ToListAsync();
    }
}
