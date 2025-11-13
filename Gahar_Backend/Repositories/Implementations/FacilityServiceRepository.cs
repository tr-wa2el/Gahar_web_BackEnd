using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class FacilityServiceRepository : GenericRepository<FacilityService>, IFacilityServiceRepository
{
  public FacilityServiceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<FacilityService>> GetByFacilityIdAsync(int facilityId)
    {
  return await _context.FacilityServices
  .Where(fs => fs.FacilityId == facilityId)
      .OrderBy(fs => fs.DisplayOrder)
  .ToListAsync();
    }

    public IQueryable<FacilityService> GetQueryable() => _context.FacilityServices.AsQueryable();

  public async Task AddAsync(FacilityService entity) => await _context.FacilityServices.AddAsync(entity);

  public void Update(FacilityService entity) => _context.FacilityServices.Update(entity);

    public void Delete(FacilityService entity) => entity.IsDeleted = true;

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
