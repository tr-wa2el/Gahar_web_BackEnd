using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Repositories.Interfaces
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
 new Task<IEnumerable<Tag>> GetAllAsync();
        new Task<Tag?> GetByIdAsync(int id);
        Task<Tag?> GetBySlugAsync(string slug);
        Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
        Task<IEnumerable<Tag>> GetPopularTagsAsync(int count);
        Task IncrementUsageCountAsync(int tagId);
 Task DecrementUsageCountAsync(int tagId);
        Task<IEnumerable<Tag>> SearchAsync(string searchTerm);
  }
}
