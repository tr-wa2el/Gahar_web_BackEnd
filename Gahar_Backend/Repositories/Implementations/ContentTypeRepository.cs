using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations
{
    public class ContentTypeRepository : GenericRepository<ContentType>, IContentTypeRepository
{
        public ContentTypeRepository(ApplicationDbContext context) : base(context)
  {
        }

   public async Task<IEnumerable<ContentType>> GetAllWithContentCountAsync()
   {
return await _dbSet
      .Include(ct => ct.Contents)
   .OrderBy(ct => ct.DisplayOrder)
    .ThenBy(ct => ct.Name)
            .ToListAsync();
        }

        public async Task<ContentType?> GetByIdWithFieldsAsync(int id)
 {
  return await _dbSet
.Include(ct => ct.Fields.OrderBy(f => f.DisplayOrder))
     .FirstOrDefaultAsync(ct => ct.Id == id);
   }

public async Task<ContentType?> GetBySlugAsync(string slug)
  {
return await _dbSet
.Include(ct => ct.Fields.OrderBy(f => f.DisplayOrder))
.FirstOrDefaultAsync(ct => ct.Slug == slug);
     }

  public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
   {
   var query = _dbSet.Where(ct => ct.Slug == slug);
   
            if (excludeId.HasValue)
      {
            query = query.Where(ct => ct.Id != excludeId.Value);
            }

       return await query.AnyAsync();
   }

        public async Task<ContentTypeField?> GetFieldByIdAsync(int fieldId)
 {
      return await _context.Set<ContentTypeField>()
         .FirstOrDefaultAsync(f => f.Id == fieldId);
        }

public async Task<bool> FieldKeyExistsAsync(int contentTypeId, string fieldKey, int? excludeFieldId = null)
  {
    var query = _context.Set<ContentTypeField>()
     .Where(f => f.ContentTypeId == contentTypeId && f.FieldKey == fieldKey);
 
      if (excludeFieldId.HasValue)
            {
           query = query.Where(f => f.Id != excludeFieldId.Value);
     }

return await query.AnyAsync();
        }
  }
}
