using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IFacilityRepository : IGenericRepository<Facility>
{
    Task<Facility?> GetBySlugAsync(string slug);
  Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
    Task<IEnumerable<Facility>> GetPublishedAsync();
    Task<IEnumerable<Facility>> SearchAsync(string searchTerm);
    IQueryable<Facility> GetQueryable();
    Task AddAsync(Facility entity);
    void Update(Facility entity);
    void Delete(Facility entity);
    Task SaveChangesAsync();
}
