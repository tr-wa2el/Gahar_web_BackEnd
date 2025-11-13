using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class FacilityRepository : GenericRepository<Facility>, IFacilityRepository
{
    public FacilityRepository(ApplicationDbContext context) : base(context) { }

  public async Task<Facility?> GetBySlugAsync(string slug)
 {
        return await _context.Facilities
     .Include(f => f.Author)
       .Include(f => f.Departments.OrderBy(d => d.DisplayOrder))
    .Include(f => f.Services.OrderBy(s => s.DisplayOrder))
  .Include(f => f.Images.OrderBy(i => i.DisplayOrder))
          .Include(f => f.Reviews.Where(r => r.IsApproved).OrderByDescending(r => r.CreatedAt))
        .FirstOrDefaultAsync(f => f.Slug == slug);
    }

    public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
    {
        var query = _context.Facilities.Where(f => f.Slug == slug);
        if (excludeId.HasValue)
            query = query.Where(f => f.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Facility>> GetPublishedAsync()
    {
        return await _context.Facilities
    .Include(f => f.Departments)
            .Include(f => f.Services)
     .Include(f => f.Images)
  .Where(f => f.IsPublished && f.IsActive)
     .OrderBy(f => f.Name)
          .ToListAsync();
 }

    public async Task<IEnumerable<Facility>> SearchAsync(string searchTerm)
 {
   return await _context.Facilities
          .Where(f => f.Name.Contains(searchTerm) || f.Description!.Contains(searchTerm) || f.Address!.Contains(searchTerm))
  .OrderBy(f => f.Name)
     .ToListAsync();
   }

    public IQueryable<Facility> GetQueryable() => _context.Facilities.AsQueryable();

    public async Task AddAsync(Facility entity) => await _context.Facilities.AddAsync(entity);

    public void Update(Facility entity) => _context.Facilities.Update(entity);

 public void Delete(Facility entity) => entity.IsDeleted = true;

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
