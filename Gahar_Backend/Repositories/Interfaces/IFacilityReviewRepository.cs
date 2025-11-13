using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IFacilityReviewRepository : IGenericRepository<FacilityReview>
{
 Task<IEnumerable<FacilityReview>> GetByFacilityIdAsync(int facilityId);
    Task<IEnumerable<FacilityReview>> GetApprovedAsync(int facilityId);
    IQueryable<FacilityReview> GetQueryable();
   Task AddAsync(FacilityReview entity);
    void Update(FacilityReview entity);
    void Delete(FacilityReview entity);
    Task SaveChangesAsync();
}
