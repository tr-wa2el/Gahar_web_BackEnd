using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IFormFieldRepository : IGenericRepository<FormField>
{
    Task<IEnumerable<FormField>> GetByFormIdAsync(int formId);
    Task ReorderFieldsAsync(int formId, List<int> fieldIds);
    IQueryable<FormField> GetQueryable();
    Task AddAsync(FormField entity);
    void Update(FormField entity);
void Delete(FormField entity);
  Task SaveChangesAsync();
}
