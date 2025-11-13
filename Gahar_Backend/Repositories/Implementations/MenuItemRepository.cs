using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
{
    public MenuItemRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<MenuItem>> GetByMenuIdAsync(int menuId)
    {
    return await _context.MenuItems
 .Where(mi => mi.MenuId == menuId)
        .Include(mi => mi.ChildItems.OrderBy(c => c.DisplayOrder))
    .OrderBy(mi => mi.DisplayOrder)
  .ToListAsync();
    }

    public async Task<IEnumerable<MenuItem>> GetRootItemsAsync(int menuId)
    {
    return await _context.MenuItems
   .Where(mi => mi.MenuId == menuId && mi.ParentItemId == null)
     .Include(mi => mi.ChildItems.OrderBy(c => c.DisplayOrder))
 .OrderBy(mi => mi.DisplayOrder)
      .ToListAsync();
 }

    public async Task ReorderItemsAsync(int menuId, List<int> itemIds)
    {
        var items = await _context.MenuItems
 .Where(mi => mi.MenuId == menuId && itemIds.Contains(mi.Id))
    .ToListAsync();

        for (int i = 0; i < itemIds.Count; i++)
    {
    var item = items.FirstOrDefault(mi => mi.Id == itemIds[i]);
      if (item != null)
          {
       item.DisplayOrder = i + 1;
   }
        }

        await _context.SaveChangesAsync();
    }

    public IQueryable<MenuItem> GetQueryable() => _context.MenuItems.AsQueryable();

    public async Task AddAsync(MenuItem entity) => await _context.MenuItems.AddAsync(entity);

    public void Update(MenuItem entity) => _context.MenuItems.Update(entity);

   public void Delete(MenuItem entity) => entity.IsDeleted = true;

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
