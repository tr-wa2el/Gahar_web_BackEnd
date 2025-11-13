using Gahar_Backend.Models.DTOs.ContentType;

namespace Gahar_Backend.Services.Interfaces
{
    public interface IContentTypeService
    {
      Task<IEnumerable<ContentTypeDto>> GetAllAsync();
        Task<ContentTypeDetailDto> GetByIdAsync(int id);
   Task<ContentTypeDto> GetBySlugAsync(string slug);
      Task<ContentTypeDto> CreateAsync(CreateContentTypeDto dto);
        Task<ContentTypeDto> UpdateAsync(int id, UpdateContentTypeDto dto);
   Task DeleteAsync(int id);
  Task<ContentTypeFieldDto> AddFieldAsync(int contentTypeId, CreateContentTypeFieldDto dto);
 Task<ContentTypeFieldDto> UpdateFieldAsync(int contentTypeId, int fieldId, UpdateContentTypeFieldDto dto);
   Task DeleteFieldAsync(int contentTypeId, int fieldId);
  Task ReorderFieldsAsync(int contentTypeId, List<int> fieldIds);
    }
}
