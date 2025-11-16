using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface ICertificateRepository : IGenericRepository<Certificate>
{
    Task<Certificate?> GetBySlugAsync(string slug);
    Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
    Task<IEnumerable<Certificate>> GetPublishedAsync();
    Task<IEnumerable<Certificate>> SearchAsync(string searchTerm);
    IQueryable<Certificate> GetQueryable();
    Task AddAsync(Certificate entity);
    void Update(Certificate entity);
    void Delete(Certificate entity);
    Task SaveChangesAsync();
}
