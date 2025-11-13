using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface ICertificateHolderRepository : IGenericRepository<CertificateHolder>
{
    Task<IEnumerable<CertificateHolder>> GetByCertificateIdAsync(int certificateId);
    Task<CertificateHolder?> GetByRegistrationNumberAsync(string registrationNumber);
    Task<IEnumerable<CertificateHolder>> SearchAsync(string searchTerm);
   IQueryable<CertificateHolder> GetQueryable();
    Task AddAsync(CertificateHolder entity);
    void Update(CertificateHolder entity);
    void Delete(CertificateHolder entity);
    Task SaveChangesAsync();
}
