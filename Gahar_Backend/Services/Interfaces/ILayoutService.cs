using Gahar_Backend.Models.DTOs.Layout;

namespace Gahar_Backend.Services.Interfaces;

/// <summary>
/// خدمة إدارة التخطيطات
/// </summary>
public interface ILayoutService
{
    Task<IEnumerable<LayoutDto>> GetAllAsync();
    Task<LayoutDetailDto> GetByIdAsync(int id);
    Task<LayoutDto> GetDefaultAsync();
Task<LayoutDto> CreateAsync(CreateLayoutDto dto);
    Task<LayoutDto> UpdateAsync(int id, UpdateLayoutDto dto);
    Task DeleteAsync(int id);
    Task SetDefaultAsync(int id);
    Task BulkUpdateLayoutAsync(int layoutId, List<int> contentIds);
}
