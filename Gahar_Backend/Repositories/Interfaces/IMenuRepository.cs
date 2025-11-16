using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IMenuRepository : IGenericRepository<Menu>
{
    Task<Menu?> GetBySlugAsync(string slug);
    Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
    Task<IEnumerable<Menu>> GetPublishedMenusAsync();
    IQueryable<Menu> GetQueryable();
    Task AddAsync(Menu entity);
    void Update(Menu entity);
    void Delete(Menu entity);
    Task SaveChangesAsync();
}
