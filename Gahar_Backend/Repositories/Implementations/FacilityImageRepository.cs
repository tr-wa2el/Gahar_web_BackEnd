using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class FacilityImageRepository : GenericRepository<FacilityImage>, IFacilityImageRepository
{
    public FacilityImageRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<FacilityImage>> GetByFacilityIdAsync(int facilityId)
    {
        return await _context.FacilityImages
     .Where(fi => fi.FacilityId == facilityId)
        .OrderBy(fi => fi.DisplayOrder)
          .ToListAsync();
    }

    public IQueryable<FacilityImage> GetQueryable() => _context.FacilityImages.AsQueryable();

public async Task AddAsync(FacilityImage entity) => await _context.FacilityImages.AddAsync(entity);

    public void Update(FacilityImage entity) => _context.FacilityImages.Update(entity);

    public void Delete(FacilityImage entity) => entity.IsDeleted = true;

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
