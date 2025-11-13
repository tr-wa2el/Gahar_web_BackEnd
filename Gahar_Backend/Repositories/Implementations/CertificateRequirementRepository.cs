using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class CertificateRequirementRepository : GenericRepository<CertificateRequirement>, ICertificateRequirementRepository
{
 public CertificateRequirementRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<CertificateRequirement>> GetByCertificateIdAsync(int certificateId)
    {
        return await _context.CertificateRequirements
      .Where(cr => cr.CertificateId == certificateId)
    .OrderBy(cr => cr.DisplayOrder)
     .ToListAsync();
  }

    public IQueryable<CertificateRequirement> GetQueryable() => _context.CertificateRequirements.AsQueryable();
    public async Task AddAsync(CertificateRequirement entity) => await _context.CertificateRequirements.AddAsync(entity);
    public void Update(CertificateRequirement entity) => _context.CertificateRequirements.Update(entity);
  public void Delete(CertificateRequirement entity) => entity.IsDeleted = true;
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
