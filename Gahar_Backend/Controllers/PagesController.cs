using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Gahar_Backend.Constants;
using Gahar_Backend.Filters;
using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Page;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PagesController : ControllerBase
{
    private readonly IPageService _pageService;
    private readonly ILogger<PagesController> _logger;

    public PagesController(IPageService pageService, ILogger<PagesController> logger)
    {
      _pageService = pageService;
        _logger = logger;
  }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<PageListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<PageListDto>>> GetAll([FromQuery] PageFilterDto filter)
    {
        _logger.LogInformation("Getting all pages");
        var pages = await _pageService.GetAllAsync(filter);
        return Ok(pages);
 }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PageDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PageDetailDto>> GetById(int id)
    {
        _logger.LogInformation("Getting page {PageId}", id);
        var page = await _pageService.GetByIdAsync(id);
      return Ok(page);
    }

    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PageDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PageDetailDto>> GetBySlug(string slug)
    {
      _logger.LogInformation("Getting page by slug: {Slug}", slug);
      var page = await _pageService.GetBySlugAsync(slug);
        return Ok(page);
    }

    [HttpPost]
    [Authorize]
    [Permission(Permissions.PagesCreate)]
    [ProducesResponseType(typeof(PageDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<ActionResult<PageDetailDto>> Create([FromBody] CreatePageDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
 return Unauthorized("Invalid user ID");
        }

        _logger.LogInformation("Creating new page: {Title}", dto.Title);
        var page = await _pageService.CreateAsync(dto, userId);
   return CreatedAtAction(nameof(GetById), new { id = page.Id }, page);
    }

    [HttpPut("{id}")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    [ProducesResponseType(typeof(PageDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PageDetailDto>> Update(int id, [FromBody] UpdatePageDto dto)
    {
        _logger.LogInformation("Updating page {PageId}", id);
        var page = await _pageService.UpdateAsync(id, dto);
        return Ok(page);
    }

    [HttpDelete("{id}")]
    [Authorize]
    [Permission(Permissions.PagesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting page {PageId}", id);
    await _pageService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/publish")]
    [Authorize]
    [Permission(Permissions.PagesPublish)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Publish(int id)
    {
        _logger.LogInformation("Publishing page {PageId}", id);
        await _pageService.PublishAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/unpublish")]
    [Authorize]
    [Permission(Permissions.PagesPublish)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Unpublish(int id)
    {
        _logger.LogInformation("Unpublishing page {PageId}", id);
        await _pageService.UnpublishAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/blocks")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    [ProducesResponseType(typeof(PageBlockDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PageBlockDto>> AddBlock(int id, [FromBody] CreatePageBlockDto dto)
    {
        _logger.LogInformation("Adding block to page {PageId}", id);
        var block = await _pageService.AddBlockAsync(id, dto);
        return Ok(block);
    }

    [HttpPut("{id}/blocks/{blockId}")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    [ProducesResponseType(typeof(PageBlockDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PageBlockDto>> UpdateBlock(int id, int blockId, [FromBody] UpdatePageBlockDto dto)
    {
        _logger.LogInformation("Updating block {BlockId} in page {PageId}", blockId, id);
        var block = await _pageService.UpdateBlockAsync(id, blockId, dto);
        return Ok(block);
    }

    [HttpDelete("{id}/blocks/{blockId}")]
    [Authorize]
    [Permission(Permissions.PagesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteBlock(int id, int blockId)
    {
        _logger.LogInformation("Deleting block {BlockId} from page {PageId}", blockId, id);
        await _pageService.DeleteBlockAsync(id, blockId);
        return NoContent();
 }

    [HttpPost("{id}/reorder-blocks")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ReorderBlocks(int id, [FromBody] ReorderBlocksDto dto)
    {
        _logger.LogInformation("Reordering blocks in page {PageId}", id);
        await _pageService.ReorderBlocksAsync(id, dto.BlockIds);
        return NoContent();
    }

    [HttpPost("{id}/duplicate")]
    [Authorize]
    [Permission(Permissions.PagesCreate)]
    [ProducesResponseType(typeof(PageDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PageDetailDto>> Duplicate(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
    {
        return Unauthorized("Invalid user ID");
        }

     _logger.LogInformation("Duplicating page {PageId}", id);
        var page = await _pageService.DuplicateAsync(id, userId);
     return Ok(page);
  }
}
