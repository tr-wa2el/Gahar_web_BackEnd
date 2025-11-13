using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class CertificateCategoryRepository : GenericRepository<CertificateCategory>, ICertificateCategoryRepository
{
    public CertificateCategoryRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<CertificateCategory>> GetByCertificateIdAsync(int certificateId)
    {
        return await _context.CertificateCategories
  .Where(cc => cc.CertificateId == certificateId)
            .OrderBy(cc => cc.DisplayOrder)
   .ToListAsync();
    }

public IQueryable<CertificateCategory> GetQueryable() => _context.CertificateCategories.AsQueryable();
    public async Task AddAsync(CertificateCategory entity) => await _context.CertificateCategories.AddAsync(entity);
    public void Update(CertificateCategory entity) => _context.CertificateCategories.Update(entity);
  public void Delete(CertificateCategory entity) => entity.IsDeleted = true;
  public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
