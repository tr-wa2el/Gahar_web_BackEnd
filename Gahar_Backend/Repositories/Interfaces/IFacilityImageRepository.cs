using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IFacilityImageRepository : IGenericRepository<FacilityImage>
{
    Task<IEnumerable<FacilityImage>> GetByFacilityIdAsync(int facilityId);
    IQueryable<FacilityImage> GetQueryable();
    Task AddAsync(FacilityImage entity);
    void Update(FacilityImage entity);
  void Delete(FacilityImage entity);
   Task SaveChangesAsync();
}
