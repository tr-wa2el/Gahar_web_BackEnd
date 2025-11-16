using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class CertificateHolderRepository : GenericRepository<CertificateHolder>, ICertificateHolderRepository
{
    public CertificateHolderRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<CertificateHolder>> GetByCertificateIdAsync(int certificateId)
    {
 return await _context.CertificateHolders
  .Where(ch => ch.CertificateId == certificateId)
 .OrderByDescending(ch => ch.CreatedAt)
      .ToListAsync();
    }

    public async Task<CertificateHolder?> GetByRegistrationNumberAsync(string registrationNumber)
    {
  return await _context.CertificateHolders
     .FirstOrDefaultAsync(ch => ch.RegistrationNumber == registrationNumber);
    }

    public async Task<IEnumerable<CertificateHolder>> SearchAsync(string searchTerm)
  {
  return await _context.CertificateHolders
     .Where(ch => ch.HolderName!.Contains(searchTerm) || ch.RegistrationNumber!.Contains(searchTerm) || ch.HolderEmail!.Contains(searchTerm))
     .ToListAsync();
    }

 public IQueryable<CertificateHolder> GetQueryable() => _context.CertificateHolders.AsQueryable();
   public async Task AddAsync(CertificateHolder entity) => await _context.CertificateHolders.AddAsync(entity);
    public void Update(CertificateHolder entity) => _context.CertificateHolders.Update(entity);
    public void Delete(CertificateHolder entity) => entity.IsDeleted = true;
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
