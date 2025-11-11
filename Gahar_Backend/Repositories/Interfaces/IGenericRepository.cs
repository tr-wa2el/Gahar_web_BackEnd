using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
   Task<IEnumerable<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
     Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
