using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

/// <summary>
/// Repository interface for Page entity
/// </summary>
public interface IPageRepository : IGenericRepository<Page>
{
    Task<Page?> GetBySlugAsync(string slug, string? lang = "ar");
    Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
    Task<IEnumerable<Page>> GetPublishedPagesAsync();
    Task<IEnumerable<Page>> GetByAuthorAsync(int authorId);
    IQueryable<Page> GetQueryable();
    Task AddAsync(Page entity);
    void Update(Page entity);
    void Delete(Page entity);
    Task SaveChangesAsync();
}
