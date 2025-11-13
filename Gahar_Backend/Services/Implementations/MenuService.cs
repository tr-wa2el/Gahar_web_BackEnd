using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Menu;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Implementations;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMenuItemRepository _itemRepository;

    public MenuService(IMenuRepository menuRepository, IMenuItemRepository itemRepository)
    {
    _menuRepository = menuRepository;
   _itemRepository = itemRepository;
    }

    #region Menu Management

    public async Task<PagedResult<MenuListDto>> GetAllAsync(PageFilterDto filter)
    {
        var query = _menuRepository.GetQueryable()
         .Include(m => m.Author)
  .Include(m => m.Items)
     .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
     {
    query = query.Where(m => m.Name.Contains(filter.SearchTerm));
        }

        var totalCount = await query.CountAsync();

        query = filter.SortBy?.ToLower() switch
       {
      "name" => filter.SortOrder?.ToLower() == "asc"
      ? query.OrderBy(m => m.Name)
   : query.OrderByDescending(m => m.Name),
           _ => filter.SortOrder?.ToLower() == "asc"
       ? query.OrderBy(m => m.DisplayOrder)
         : query.OrderByDescending(m => m.DisplayOrder)
      };

  var items = await query
      .Skip((filter.PageNumber - 1) * filter.PageSize)
          .Take(filter.PageSize)
           .Select(m => new MenuListDto
      {
        Id = m.Id,
     Name = m.Name,
       Slug = m.Slug,
         Description = m.Description,
        DisplayOrder = m.DisplayOrder,
      IsActive = m.IsActive,
      IsPublished = m.IsPublished,
    PublishedAt = m.PublishedAt,
     AuthorName = m.Author != null ? $"{m.Author.FirstName} {m.Author.LastName}".Trim() : null,
 ItemCount = m.Items.Count,
     CreatedAt = m.CreatedAt,
         UpdatedAt = m.UpdatedAt
        })
            .ToListAsync();

        return new PagedResult<MenuListDto>
  {
      Items = items,
        TotalCount = totalCount,
            PageNumber = filter.PageNumber,
    PageSize = filter.PageSize
        };
    }

    public async Task<MenuDetailDto> GetByIdAsync(int id)
    {
  var menu = await _menuRepository.GetQueryable()
      .Include(m => m.Author)
       .Include(m => m.Items)
 .FirstOrDefaultAsync(m => m.Id == id);

        if (menu == null)
    throw new NotFoundException($"Menu with ID {id} not found");

  return MapToDetailDto(menu);
    }

    public async Task<MenuDetailDto> GetBySlugAsync(string slug)
    {
 var menu = await _menuRepository.GetBySlugAsync(slug);

   if (menu == null)
      throw new NotFoundException($"Menu with slug '{slug}' not found");

  if (!menu.IsPublished || !menu.IsActive)
           throw new BadRequestException("Menu is not available");

     return MapToDetailDto(menu);
    }

    public async Task<MenuDetailDto> CreateAsync(CreateMenuDto dto, int authorId)
    {
        if (await _menuRepository.SlugExistsAsync(dto.Slug))
      throw new BadRequestException($"Slug '{dto.Slug}' already exists");

  var menu = new Menu
     {
            Name = dto.Name,
Slug = dto.Slug,
  Description = dto.Description,
           DisplayOrder = 1,
  AuthorId = authorId,
     IsActive = true,
     IsPublished = false
        };

        await _menuRepository.AddAsync(menu);
        await _menuRepository.SaveChangesAsync();

        return await GetByIdAsync(menu.Id);
   }

 public async Task<MenuDetailDto> UpdateAsync(int id, UpdateMenuDto dto)
    {
        var menu = await _menuRepository.GetByIdAsync(id);
      if (menu == null)
       throw new NotFoundException($"Menu with ID {id} not found");

  if (dto.Slug != menu.Slug && await _menuRepository.SlugExistsAsync(dto.Slug, id))
       throw new BadRequestException($"Slug '{dto.Slug}' already exists");

        menu.Name = dto.Name;
      menu.Slug = dto.Slug;
   menu.Description = dto.Description;
        menu.DisplayOrder = dto.DisplayOrder;
 menu.IsActive = dto.IsActive;
      menu.IsPublished = dto.IsPublished;

 _menuRepository.Update(menu);
   await _menuRepository.SaveChangesAsync();

       return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
 {
 var menu = await _menuRepository.GetByIdAsync(id);
        if (menu == null)
 throw new NotFoundException($"Menu with ID {id} not found");

      _menuRepository.Delete(menu);
   await _menuRepository.SaveChangesAsync();
    }

    public async Task PublishAsync(int id)
    {
   var menu = await _menuRepository.GetByIdAsync(id);
        if (menu == null)
  throw new NotFoundException($"Menu with ID {id} not found");

      menu.IsPublished = true;
        menu.PublishedAt = DateTime.UtcNow;

     _menuRepository.Update(menu);
  await _menuRepository.SaveChangesAsync();
    }

  public async Task UnpublishAsync(int id)
    {
   var menu = await _menuRepository.GetByIdAsync(id);
   if (menu == null)
       throw new NotFoundException($"Menu with ID {id} not found");

    menu.IsPublished = false;

        _menuRepository.Update(menu);
        await _menuRepository.SaveChangesAsync();
    }

    #endregion

    #region Menu Item Management

    public async Task<MenuItemDto> AddItemAsync(int menuId, CreateMenuItemDto dto)
    {
     var menu = await _menuRepository.GetByIdAsync(menuId);
  if (menu == null)
      throw new NotFoundException($"Menu with ID {menuId} not found");

      var maxOrder = await _itemRepository.GetQueryable()
 .Where(mi => mi.MenuId == menuId && mi.ParentItemId == dto.ParentItemId)
        .MaxAsync(mi => (int?)mi.DisplayOrder) ?? 0;

        var item = new MenuItem
        {
MenuId = menuId,
          Label = dto.Label,
   Url = dto.Url,
  Icon = dto.Icon,
       CssClass = dto.CssClass,
       DisplayOrder = maxOrder + 1,
     IsVisible = dto.IsVisible,
         OpenInNewTab = dto.OpenInNewTab,
    Title = dto.Title,
          RelatedPageId = dto.RelatedPageId,
 ParentItemId = dto.ParentItemId
        };

     await _itemRepository.AddAsync(item);
   await _itemRepository.SaveChangesAsync();

       return MapItemToDto(item);
    }

 public async Task<MenuItemDto> UpdateItemAsync(int menuId, int itemId, UpdateMenuItemDto dto)
    {
     var item = await _itemRepository.GetQueryable()
      .FirstOrDefaultAsync(mi => mi.Id == itemId && mi.MenuId == menuId);

  if (item == null)
   throw new NotFoundException($"Item with ID {itemId} not found in menu {menuId}");

       item.Label = dto.Label;
  item.Url = dto.Url;
      item.Icon = dto.Icon;
 item.CssClass = dto.CssClass;
       item.DisplayOrder = dto.DisplayOrder;
       item.IsVisible = dto.IsVisible;
    item.OpenInNewTab = dto.OpenInNewTab;
       item.Title = dto.Title;
 item.RelatedPageId = dto.RelatedPageId;
  item.ParentItemId = dto.ParentItemId;

        _itemRepository.Update(item);
     await _itemRepository.SaveChangesAsync();

 return MapItemToDto(item);
 }

  public async Task DeleteItemAsync(int menuId, int itemId)
    {
      var item = await _itemRepository.GetQueryable()
        .FirstOrDefaultAsync(mi => mi.Id == itemId && mi.MenuId == menuId);

 if (item == null)
       throw new NotFoundException($"Item with ID {itemId} not found in menu {menuId}");

       _itemRepository.Delete(item);
    await _itemRepository.SaveChangesAsync();
    }

    public async Task ReorderItemsAsync(int menuId, ReorderMenuItemsDto dto)
    {
    var menu = await _menuRepository.GetByIdAsync(menuId);
   if (menu == null)
throw new NotFoundException($"Menu with ID {menuId} not found");

      await _itemRepository.ReorderItemsAsync(menuId, dto.ItemIds);
   }

 #endregion

    #region Public Access

    public async Task<MenuDetailDto> GetPublishedMenuAsync(string slug)
    {
 return await GetBySlugAsync(slug);
 }

    public async Task<IEnumerable<MenuDetailDto>> GetAllPublishedMenusAsync()
    {
   var menus = await _menuRepository.GetPublishedMenusAsync();
  return menus.Select(MapToDetailDto);
    }

    #endregion

    #region Helper Methods

    private MenuDetailDto MapToDetailDto(Menu menu)
  {
   return new MenuDetailDto
     {
     Id = menu.Id,
        Name = menu.Name,
        Slug = menu.Slug,
       Description = menu.Description,
        DisplayOrder = menu.DisplayOrder,
  IsActive = menu.IsActive,
      IsPublished = menu.IsPublished,
          PublishedAt = menu.PublishedAt,
        AuthorName = menu.Author != null ? $"{menu.Author.FirstName} {menu.Author.LastName}".Trim() : null,
          ItemCount = menu.Items.Count,
        CreatedAt = menu.CreatedAt,
      UpdatedAt = menu.UpdatedAt,
         Items = menu.Items
    .Where(i => i.ParentItemId == null)
  .OrderBy(i => i.DisplayOrder)
  .Select(MapItemToDto)
  .ToList()
     };
    }

 private MenuItemDto MapItemToDto(MenuItem item)
    {
        return new MenuItemDto
       {
      Id = item.Id,
   Label = item.Label,
     Url = item.Url,
   Icon = item.Icon,
      CssClass = item.CssClass,
    DisplayOrder = item.DisplayOrder,
     IsVisible = item.IsVisible,
          OpenInNewTab = item.OpenInNewTab,
  Title = item.Title,
    RelatedPageId = item.RelatedPageId,
      ParentItemId = item.ParentItemId,
    ChildItems = item.ChildItems
     .OrderBy(c => c.DisplayOrder)
    .Select(MapItemToDto)
         .ToList()
        };
    }

    #endregion
}
