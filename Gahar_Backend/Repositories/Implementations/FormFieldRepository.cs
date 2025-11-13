using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class FormFieldRepository : GenericRepository<FormField>, IFormFieldRepository
{
    public FormFieldRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<FormField>> GetByFormIdAsync(int formId)
    {
     return await _context.FormFields
  .Where(ff => ff.FormId == formId)
      .OrderBy(ff => ff.DisplayOrder)
   .ToListAsync();
 }

    public async Task ReorderFieldsAsync(int formId, List<int> fieldIds)
    {
        var fields = await _context.FormFields
       .Where(ff => ff.FormId == formId && fieldIds.Contains(ff.Id))
   .ToListAsync();

        for (int i = 0; i < fieldIds.Count; i++)
        {
   var field = fields.FirstOrDefault(f => f.Id == fieldIds[i]);
     if (field != null)
           {
field.DisplayOrder = i + 1;
   }
    }

 await _context.SaveChangesAsync();
    }

    public IQueryable<FormField> GetQueryable() => _context.FormFields.AsQueryable();

    public async Task AddAsync(FormField entity) => await _context.FormFields.AddAsync(entity);

    public void Update(FormField entity) => _context.FormFields.Update(entity);

public void Delete(FormField entity) => entity.IsDeleted = true;

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
