using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class CertificateRepository : GenericRepository<Certificate>, ICertificateRepository
{
    public CertificateRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Certificate?> GetBySlugAsync(string slug)
    {
     return await _context.Certificates
   .Include(c => c.Author)
        .Include(c => c.Categories.OrderBy(c => c.DisplayOrder))
            .Include(c => c.Requirements.OrderBy(r => r.DisplayOrder))
            .Include(c => c.Holders)
   .FirstOrDefaultAsync(c => c.Slug == slug);
    }

    public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
    {
        var query = _context.Certificates.Where(c => c.Slug == slug);
     if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Certificate>> GetPublishedAsync()
    {
        return await _context.Certificates
          .Include(c => c.Categories)
            .Include(c => c.Requirements)
            .Where(c => c.IsPublished && c.IsActive)
            .OrderBy(c => c.DisplayOrder)
    .ToListAsync();
    }

    public async Task<IEnumerable<Certificate>> SearchAsync(string searchTerm)
    {
      return await _context.Certificates
            .Where(c => c.Name.Contains(searchTerm) || c.Description!.Contains(searchTerm))
            .OrderBy(c => c.DisplayOrder)
  .ToListAsync();
    }

    public IQueryable<Certificate> GetQueryable() => _context.Certificates.AsQueryable();
    public async Task AddAsync(Certificate entity) => await _context.Certificates.AddAsync(entity);
    public void Update(Certificate entity) => _context.Certificates.Update(entity);
    public void Delete(Certificate entity) => entity.IsDeleted = true;
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
