using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class FormRepository : GenericRepository<Form>, IFormRepository
{
    public FormRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Form?> GetBySlugAsync(string slug)
    {
        return await _context.Forms
            .Include(f => f.Author)
  .Include(f => f.Fields.OrderBy(ff => ff.DisplayOrder))
          .FirstOrDefaultAsync(f => f.Slug == slug);
    }

    public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
    {
        var query = _context.Forms.Where(f => f.Slug == slug);
        if (excludeId.HasValue)
            query = query.Where(f => f.Id != excludeId.Value);
  return await query.AnyAsync();
    }

    public async Task<IEnumerable<Form>> GetPublishedFormsAsync()
    {
 return await _context.Forms
 .Include(f => f.Author)
        .Include(f => f.Fields.OrderBy(ff => ff.DisplayOrder))
          .Where(f => f.IsPublished && f.IsActive)
        .OrderByDescending(f => f.PublishedAt)
      .ToListAsync();
    }

    public async Task<IEnumerable<Form>> GetByAuthorAsync(int authorId)
    {
        return await _context.Forms
   .Include(f => f.Fields)
     .Where(f => f.AuthorId == authorId)
      .OrderByDescending(f => f.CreatedAt)
     .ToListAsync();
    }

 public IQueryable<Form> GetQueryable() => _context.Forms.AsQueryable();

    public async Task AddAsync(Form entity) => await _context.Forms.AddAsync(entity);

    public void Update(Form entity) => _context.Forms.Update(entity);

    public void Delete(Form entity) => entity.IsDeleted = true;

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
