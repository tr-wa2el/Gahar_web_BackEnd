using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class FacilityReviewRepository : GenericRepository<FacilityReview>, IFacilityReviewRepository
{
    public FacilityReviewRepository(ApplicationDbContext context) : base(context) { }

   public async Task<IEnumerable<FacilityReview>> GetByFacilityIdAsync(int facilityId)
    {
        return await _context.FacilityReviews
     .Where(fr => fr.FacilityId == facilityId)
      .OrderByDescending(fr => fr.CreatedAt)
  .ToListAsync();
   }

   public async Task<IEnumerable<FacilityReview>> GetApprovedAsync(int facilityId)
    {
    return await _context.FacilityReviews
      .Where(fr => fr.FacilityId == facilityId && fr.IsApproved)
        .OrderByDescending(fr => fr.CreatedAt)
        .ToListAsync();
  }

 public IQueryable<FacilityReview> GetQueryable() => _context.FacilityReviews.AsQueryable();

   public async Task AddAsync(FacilityReview entity) => await _context.FacilityReviews.AddAsync(entity);

    public void Update(FacilityReview entity) => _context.FacilityReviews.Update(entity);

  public void Delete(FacilityReview entity) => entity.IsDeleted = true;

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
