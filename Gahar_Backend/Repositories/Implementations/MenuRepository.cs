using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class MenuRepository : GenericRepository<Menu>, IMenuRepository
{
    public MenuRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Menu?> GetBySlugAsync(string slug)
    {
        return await _context.Menus
         .Include(m => m.Author)
    .Include(m => m.Items.Where(i => i.ParentItemId == null).OrderBy(i => i.DisplayOrder))
            .ThenInclude(mi => mi.ChildItems.OrderBy(c => c.DisplayOrder))
      .FirstOrDefaultAsync(m => m.Slug == slug);
    }

    public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
    {
    var query = _context.Menus.Where(m => m.Slug == slug);
   if (excludeId.HasValue)
      query = query.Where(m => m.Id != excludeId.Value);
return await query.AnyAsync();
    }

    public async Task<IEnumerable<Menu>> GetPublishedMenusAsync()
    {
  return await _context.Menus
    .Include(m => m.Items.OrderBy(i => i.DisplayOrder))
   .Where(m => m.IsPublished && m.IsActive)
          .OrderBy(m => m.DisplayOrder)
            .ToListAsync();
    }

    public IQueryable<Menu> GetQueryable() => _context.Menus.AsQueryable();

    public async Task AddAsync(Menu entity) => await _context.Menus.AddAsync(entity);

    public void Update(Menu entity) => _context.Menus.Update(entity);

    public void Delete(Menu entity) => entity.IsDeleted = true;

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
