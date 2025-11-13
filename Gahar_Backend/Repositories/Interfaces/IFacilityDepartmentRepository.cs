using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IFacilityDepartmentRepository : IGenericRepository<FacilityDepartment>
{
    Task<IEnumerable<FacilityDepartment>> GetByFacilityIdAsync(int facilityId);
    IQueryable<FacilityDepartment> GetQueryable();
    Task AddAsync(FacilityDepartment entity);
    void Update(FacilityDepartment entity);
    void Delete(FacilityDepartment entity);
    Task SaveChangesAsync();
}
