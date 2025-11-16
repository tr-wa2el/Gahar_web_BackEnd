using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface ICertificateCategoryRepository : IGenericRepository<CertificateCategory>
{
    Task<IEnumerable<CertificateCategory>> GetByCertificateIdAsync(int certificateId);
    IQueryable<CertificateCategory> GetQueryable();
    Task AddAsync(CertificateCategory entity);
    void Update(CertificateCategory entity);
    void Delete(CertificateCategory entity);
    Task SaveChangesAsync();
}
