using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IFacilityServiceRepository : IGenericRepository<FacilityService>
{
    Task<IEnumerable<FacilityService>> GetByFacilityIdAsync(int facilityId);
  IQueryable<FacilityService> GetQueryable();
    Task AddAsync(FacilityService entity);
 void Update(FacilityService entity);
    void Delete(FacilityService entity);
    Task SaveChangesAsync();
}
