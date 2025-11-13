using Gahar_Backend.Models.DTOs.ContentType;

namespace Gahar_Backend.Services.Interfaces;

/// <summary>
/// خدمة إدارة أنواع المحتوى
/// </summary>
public interface IContentTypeService
{
    // ContentType Management
    Task<IEnumerable<ContentTypeDto>> GetAllAsync();
    Task<ContentTypeDetailDto> GetByIdAsync(int id);
  Task<ContentTypeDto> GetBySlugAsync(string slug);
    Task<ContentTypeDto> CreateAsync(CreateContentTypeDto dto);
  Task<ContentTypeDto> UpdateAsync(int id, UpdateContentTypeDto dto);
    Task DeleteAsync(int id);

    // Field Management
    Task<ContentTypeFieldDto> AddFieldAsync(int contentTypeId, CreateContentTypeFieldDto dto);
    Task<ContentTypeFieldDto> UpdateFieldAsync(int contentTypeId, int fieldId, UpdateContentTypeFieldDto dto);
    Task DeleteFieldAsync(int contentTypeId, int fieldId);
    Task ReorderFieldsAsync(int contentTypeId, List<int> fieldIds);

    // Tag Management
    Task<TagDto> CreateTagAsync(CreateTagDto dto);
  Task<TagDto> UpdateTagAsync(int id, CreateTagDto dto);
    Task DeleteTagAsync(int id);
    Task<IEnumerable<TagDto>> GetAllTagsAsync();
    Task<IEnumerable<TagDto>> SearchTagsAsync(string searchTerm);

    // Layout Management
    Task<LayoutDto> CreateLayoutAsync(CreateLayoutDto dto);
    Task<LayoutDto> UpdateLayoutAsync(int id, UpdateLayoutDto dto);
    Task DeleteLayoutAsync(int id);
    Task<IEnumerable<LayoutDto>> GetAllLayoutsAsync();
 Task SetDefaultLayoutAsync(int layoutId);
}
