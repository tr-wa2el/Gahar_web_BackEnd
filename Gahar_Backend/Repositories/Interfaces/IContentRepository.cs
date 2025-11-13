using Gahar_Backend.Models.Entities;
using Gahar_Backend.Models.DTOs.Content;

namespace Gahar_Backend.Repositories.Interfaces
{
    public interface IContentRepository : IGenericRepository<Content>
    {
        Task<IEnumerable<Content>> GetAllAsync(ContentFilterDto filter);
      Task<int> GetTotalCountAsync(ContentFilterDto filter);
        Task<Content?> GetByIdWithDetailsAsync(int id);
        Task<Content?> GetBySlugAsync(string slug, int contentTypeId);
     Task<bool> SlugExistsAsync(string slug, int contentTypeId, int? excludeId = null);
    Task<IEnumerable<Content>> GetFeaturedAsync(int contentTypeId, int count);
     Task<IEnumerable<Content>> GetRecentAsync(int contentTypeId, int count);
        Task<IEnumerable<Content>> GetByTagAsync(int tagId, int pageSize, int page);
        Task IncrementViewCountAsync(int contentId);
        Task<IEnumerable<Content>> GetScheduledContentAsync();
    }
}
