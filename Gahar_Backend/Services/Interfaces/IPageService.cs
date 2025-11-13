using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Page;

namespace Gahar_Backend.Services.Interfaces;

/// <summary>
/// Service for managing pages and page blocks
/// </summary>
public interface IPageService
{
Task<PagedResult<PageListDto>> GetAllAsync(PageFilterDto filter);
    Task<PageDetailDto> GetByIdAsync(int id, string? lang = "ar");
    Task<PageDetailDto> GetBySlugAsync(string slug, string? lang = "ar");
    Task<PageDetailDto> CreateAsync(CreatePageDto dto, int authorId);
    Task<PageDetailDto> UpdateAsync(int id, UpdatePageDto dto);
    Task DeleteAsync(int id);
 Task PublishAsync(int id);
    Task UnpublishAsync(int id);
  Task<PageBlockDto> AddBlockAsync(int pageId, CreatePageBlockDto dto);
    Task<PageBlockDto> UpdateBlockAsync(int pageId, int blockId, UpdatePageBlockDto dto);
    Task DeleteBlockAsync(int pageId, int blockId);
    Task ReorderBlocksAsync(int pageId, List<int> blockIds);
    Task<PageDetailDto> DuplicateAsync(int id, int authorId);
}
