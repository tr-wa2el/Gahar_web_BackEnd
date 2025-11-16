using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class FacilityDepartmentRepository : GenericRepository<FacilityDepartment>, IFacilityDepartmentRepository
{
    public FacilityDepartmentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<FacilityDepartment>> GetByFacilityIdAsync(int facilityId)
    {
     return await _context.FacilityDepartments
  .Where(fd => fd.FacilityId == facilityId)
       .OrderBy(fd => fd.DisplayOrder)
    .ToListAsync();
    }

    public IQueryable<FacilityDepartment> GetQueryable() => _context.FacilityDepartments.AsQueryable();

  public async Task AddAsync(FacilityDepartment entity) => await _context.FacilityDepartments.AddAsync(entity);

    public void Update(FacilityDepartment entity) => _context.FacilityDepartments.Update(entity);

   public void Delete(FacilityDepartment entity) => entity.IsDeleted = true;

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
