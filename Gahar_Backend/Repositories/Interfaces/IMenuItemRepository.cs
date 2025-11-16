using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IMenuItemRepository : IGenericRepository<MenuItem>
{
    Task<IEnumerable<MenuItem>> GetByMenuIdAsync(int menuId);
    Task<IEnumerable<MenuItem>> GetRootItemsAsync(int menuId);
    Task ReorderItemsAsync(int menuId, List<int> itemIds);
    IQueryable<MenuItem> GetQueryable();
    Task AddAsync(MenuItem entity);
    void Update(MenuItem entity);
    void Delete(MenuItem entity);
    Task SaveChangesAsync();
}
