using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context) : base(context)
        {
        }

        public new async Task<IEnumerable<Tag>> GetAllAsync()
        {
 return await _dbSet
           .Where(t => !t.IsDeleted)
           .OrderBy(t => t.Name)
   .ToListAsync();
        }

        public new async Task<Tag?> GetByIdAsync(int id)
        {
            return await _dbSet
    .Include(t => t.ContentTags)
              .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task<Tag?> GetBySlugAsync(string slug)
        {
    return await _dbSet
 .Include(t => t.ContentTags)
                .FirstOrDefaultAsync(t => t.Slug == slug && !t.IsDeleted);
        }

   public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
        {
    var query = _dbSet.Where(t => t.Slug == slug && !t.IsDeleted);
 
            if (excludeId.HasValue)
   {
           query = query.Where(t => t.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Tag>> GetPopularTagsAsync(int count)
        {
            return await _dbSet
  .Where(t => !t.IsDeleted)
    .OrderByDescending(t => t.UsageCount)
            .Take(count)
     .ToListAsync();
        }

    public async Task IncrementUsageCountAsync(int tagId)
        {
            var tag = await _dbSet.FindAsync(tagId);
            if (tag != null)
            {
     tag.UsageCount++;
       await _context.SaveChangesAsync();
            }
        }

        public async Task DecrementUsageCountAsync(int tagId)
      {
      var tag = await _dbSet.FindAsync(tagId);
 if (tag != null && tag.UsageCount > 0)
            {
 tag.UsageCount--;
 await _context.SaveChangesAsync();
            }
}

        public async Task<IEnumerable<Tag>> SearchAsync(string searchTerm)
        {
       var term = searchTerm.ToLower();
            return await _dbSet
                .Where(t => !t.IsDeleted && 
         (t.Name.ToLower().Contains(term) || 
 (t.Description != null && t.Description.ToLower().Contains(term))))
                .OrderBy(t => t.Name)
             .ToListAsync();
        }
    }
}
