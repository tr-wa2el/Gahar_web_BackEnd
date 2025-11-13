using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Menu;

namespace Gahar_Backend.Services.Interfaces;

public interface IMenuService
{
    // Menu Management
    Task<PagedResult<MenuListDto>> GetAllAsync(PageFilterDto filter);
    Task<MenuDetailDto> GetByIdAsync(int id);
    Task<MenuDetailDto> GetBySlugAsync(string slug);
    Task<MenuDetailDto> CreateAsync(CreateMenuDto dto, int authorId);
    Task<MenuDetailDto> UpdateAsync(int id, UpdateMenuDto dto);
    Task DeleteAsync(int id);
    Task PublishAsync(int id);
    Task UnpublishAsync(int id);

    // Menu Item Management
    Task<MenuItemDto> AddItemAsync(int menuId, CreateMenuItemDto dto);
    Task<MenuItemDto> UpdateItemAsync(int menuId, int itemId, UpdateMenuItemDto dto);
    Task DeleteItemAsync(int menuId, int itemId);
    Task ReorderItemsAsync(int menuId, ReorderMenuItemsDto dto);

    // Public Access
    Task<MenuDetailDto> GetPublishedMenuAsync(string slug);
    Task<IEnumerable<MenuDetailDto>> GetAllPublishedMenusAsync();
}
