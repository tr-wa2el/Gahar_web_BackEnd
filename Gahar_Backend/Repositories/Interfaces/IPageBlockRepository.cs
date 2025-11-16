using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

/// <summary>
/// Repository interface for PageBlock entity
/// </summary>
public interface IPageBlockRepository : IGenericRepository<PageBlock>
{
    Task<IEnumerable<PageBlock>> GetByPageIdAsync(int pageId);
    Task ReorderBlocksAsync(int pageId, List<int> blockIds);
    IQueryable<PageBlock> GetQueryable();
    Task AddAsync(PageBlock entity);
    void Update(PageBlock entity);
    void Delete(PageBlock entity);
    Task SaveChangesAsync();
}
