using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Repositories.Interfaces
{
    public interface IContentTypeRepository : IGenericRepository<ContentType>
    {
   Task<IEnumerable<ContentType>> GetAllWithContentCountAsync();
        Task<ContentType?> GetByIdWithFieldsAsync(int id);
        Task<ContentType?> GetBySlugAsync(string slug);
Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
        Task<ContentTypeField?> GetFieldByIdAsync(int fieldId);
        Task<bool> FieldKeyExistsAsync(int contentTypeId, string fieldKey, int? excludeFieldId = null);
    }
}
