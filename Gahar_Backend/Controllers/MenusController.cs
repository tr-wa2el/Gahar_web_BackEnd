using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Gahar_Backend.Constants;
using Gahar_Backend.Filters;
using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Menu;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MenusController : ControllerBase
{
    private readonly IMenuService _menuService;
    private readonly ILogger<MenusController> _logger;

    public MenusController(IMenuService menuService, ILogger<MenusController> logger)
    {
   _menuService = menuService;
        _logger = logger;
    }

    #region Menu Management

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<MenuListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<MenuListDto>>> GetAll([FromQuery] PageFilterDto filter)
 {
        _logger.LogInformation("Getting all menus");
        var menus = await _menuService.GetAllAsync(filter);
     return Ok(menus);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(MenuDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<MenuDetailDto>> GetById(int id)
 {
        _logger.LogInformation("Getting menu {MenuId}", id);
        var menu = await _menuService.GetByIdAsync(id);
        return Ok(menu);
    }

    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(MenuDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<MenuDetailDto>> GetBySlug(string slug)
    {
        _logger.LogInformation("Getting menu by slug: {Slug}", slug);
        var menu = await _menuService.GetBySlugAsync(slug);
        return Ok(menu);
    }

    [HttpPost]
    [Authorize]
    [Permission(Permissions.MenusCreate)]
    [ProducesResponseType(typeof(MenuDetailDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<MenuDetailDto>> Create([FromBody] CreateMenuDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        _logger.LogInformation("Creating new menu: {Name}", dto.Name);
        var menu = await _menuService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = menu.Id }, menu);
    }

    [HttpPut("{id}")]
    [Authorize]
    [Permission(Permissions.MenusEdit)]
    [ProducesResponseType(typeof(MenuDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<MenuDetailDto>> Update(int id, [FromBody] UpdateMenuDto dto)
    {
        _logger.LogInformation("Updating menu {MenuId}", id);
  var menu = await _menuService.UpdateAsync(id, dto);
        return Ok(menu);
    }

    [HttpDelete("{id}")]
    [Authorize]
    [Permission(Permissions.MenusDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting menu {MenuId}", id);
        await _menuService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/publish")]
    [Authorize]
    [Permission(Permissions.MenusPublish)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Publish(int id)
    {
 _logger.LogInformation("Publishing menu {MenuId}", id);
        await _menuService.PublishAsync(id);
  return NoContent();
    }

    [HttpPost("{id}/unpublish")]
    [Authorize]
    [Permission(Permissions.MenusPublish)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Unpublish(int id)
    {
    _logger.LogInformation("Unpublishing menu {MenuId}", id);
        await _menuService.UnpublishAsync(id);
    return NoContent();
    }

    #endregion

    #region Menu Item Management

    [HttpPost("{id}/items")]
    [Authorize]
    [Permission(Permissions.MenusEdit)]
[ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<MenuItemDto>> AddItem(int id, [FromBody] CreateMenuItemDto dto)
  {
        _logger.LogInformation("Adding item to menu {MenuId}", id);
        var item = await _menuService.AddItemAsync(id, dto);
        return Ok(item);
    }

    [HttpPut("{id}/items/{itemId}")]
    [Authorize]
    [Permission(Permissions.MenusEdit)]
    [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<MenuItemDto>> UpdateItem(int id, int itemId, [FromBody] UpdateMenuItemDto dto)
    {
    _logger.LogInformation("Updating item {ItemId} in menu {MenuId}", itemId, id);
        var item = await _menuService.UpdateItemAsync(id, itemId, dto);
        return Ok(item);
    }

    [HttpDelete("{id}/items/{itemId}")]
    [Authorize]
    [Permission(Permissions.MenusDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteItem(int id, int itemId)
    {
      _logger.LogInformation("Deleting item {ItemId} from menu {MenuId}", itemId, id);
     await _menuService.DeleteItemAsync(id, itemId);
  return NoContent();
    }

    [HttpPost("{id}/reorder-items")]
    [Authorize]
    [Permission(Permissions.MenusEdit)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ReorderItems(int id, [FromBody] ReorderMenuItemsDto dto)
    {
        _logger.LogInformation("Reordering items in menu {MenuId}", id);
      await _menuService.ReorderItemsAsync(id, dto);
        return NoContent();
    }

  #endregion

 #region Public Access

    [HttpGet("published/all")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<MenuDetailDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MenuDetailDto>>> GetAllPublishedMenus()
    {
  _logger.LogInformation("Getting all published menus");
        var menus = await _menuService.GetAllPublishedMenusAsync();
        return Ok(menus);
    }

    #endregion
}
