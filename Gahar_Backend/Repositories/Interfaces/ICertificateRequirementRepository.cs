using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface ICertificateRequirementRepository : IGenericRepository<CertificateRequirement>
{
    Task<IEnumerable<CertificateRequirement>> GetByCertificateIdAsync(int certificateId);
    IQueryable<CertificateRequirement> GetQueryable();
    Task AddAsync(CertificateRequirement entity);
    void Update(CertificateRequirement entity);
    void Delete(CertificateRequirement entity);
    Task SaveChangesAsync();
}
