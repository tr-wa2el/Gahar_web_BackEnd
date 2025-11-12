using Gahar_Backend.Models.DTOs.Content;

namespace Gahar_Backend.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllAsync();
    Task<TagDto> GetByIdAsync(int id);
        Task<TagDto> GetBySlugAsync(string slug);
     Task<TagDto> CreateAsync(CreateTagDto dto);
      Task<TagDto> UpdateAsync(int id, UpdateTagDto dto);
        Task DeleteAsync(int id);
  Task<IEnumerable<TagDto>> GetPopularAsync(int count);
   Task<IEnumerable<TagDto>> SearchAsync(string searchTerm);
    }
}
