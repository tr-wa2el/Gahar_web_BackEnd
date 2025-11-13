using Gahar_Backend.Models.DTOs.Content;

namespace Gahar_Backend.Services.Interfaces
{
    public interface IContentService
    {
  Task<PagedResultDto<ContentListDto>> GetAllAsync(ContentFilterDto filter);
   Task<ContentDetailDto> GetByIdAsync(int id);
   Task<ContentDetailDto> GetBySlugAsync(string slug, int contentTypeId);
   Task<ContentDetailDto> CreateAsync(CreateContentDto dto, int? userId);
        Task<ContentDetailDto> UpdateAsync(int id, UpdateContentDto dto, int? userId);
   Task DeleteAsync(int id);
  Task<ContentDetailDto> DuplicateAsync(int id, int? userId);
  Task PublishAsync(int id);
        Task UnpublishAsync(int id);
   Task ArchiveAsync(int id);
     Task IncrementViewCountAsync(int id);
Task ProcessScheduledContentAsync();
    Task<IEnumerable<ContentListDto>> GetFeaturedAsync(int contentTypeId, int count);
        Task<IEnumerable<ContentListDto>> GetRecentAsync(int contentTypeId, int count);
   Task<IEnumerable<ContentListDto>> GetByTagAsync(int tagId, int pageSize, int page);
    }
}
