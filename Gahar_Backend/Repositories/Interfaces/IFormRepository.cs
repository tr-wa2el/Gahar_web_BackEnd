using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IFormRepository : IGenericRepository<Form>
{
    Task<Form?> GetBySlugAsync(string slug);
    Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
    Task<IEnumerable<Form>> GetPublishedFormsAsync();
    Task<IEnumerable<Form>> GetByAuthorAsync(int authorId);
    IQueryable<Form> GetQueryable();
    Task AddAsync(Form entity);
    void Update(Form entity);
    void Delete(Form entity);
    Task SaveChangesAsync();
}
