using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Gahar_Backend.Models.DTOs.Content;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Filters;
using Gahar_Backend.Constants;

namespace Gahar_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentsController : ControllerBase
    {
     private readonly IContentService _contentService;

public ContentsController(IContentService contentService)
        {
            _contentService = contentService;
  }

        /// <summary>
  /// Get all contents with filtering and pagination
        /// </summary>
     [HttpGet]
  [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResultDto<ContentListDto>), StatusCodes.Status200OK)]
   public async Task<ActionResult<PagedResultDto<ContentListDto>>> GetAll([FromQuery] ContentFilterDto filter)
        {
   var contents = await _contentService.GetAllAsync(filter);
            return Ok(contents);
        }

     /// <summary>
        /// Get content by ID
      /// </summary>
      [HttpGet("{id}")]
     [AllowAnonymous]
    [ProducesResponseType(typeof(ContentDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<ActionResult<ContentDetailDto>> GetById(int id)
        {
            var content = await _contentService.GetByIdAsync(id);
   
            // Increment view count for public access
     if (!User.Identity?.IsAuthenticated ?? true)
            {
    await _contentService.IncrementViewCountAsync(id);
         }
            
            return Ok(content);
        }

        /// <summary>
        /// Get content by slug and content type
        /// </summary>
        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ContentDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContentDetailDto>> GetBySlug(
            string slug,
            [FromQuery] int contentTypeId)
        {
        var content = await _contentService.GetBySlugAsync(slug, contentTypeId);
     
            // Increment view count for public access
 if (!User.Identity?.IsAuthenticated ?? true)
          {
 await _contentService.IncrementViewCountAsync(content.Id);
     }
            
       return Ok(content);
        }

     /// <summary>
        /// Create new content
        /// </summary>
        [HttpPost]
  [Authorize]
    [Permission(Permissions.ContentCreate)]
        [ProducesResponseType(typeof(ContentDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ContentDetailDto>> Create([FromBody] CreateContentDto dto)
        {
      var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var content = await _contentService.CreateAsync(dto, userId);
      return CreatedAtAction(nameof(GetById), new { id = content.Id }, content);
        }

    /// <summary>
        /// Update content
     /// </summary>
        [HttpPut("{id}")]
      [Authorize]
   [Permission(Permissions.ContentEdit)]
  [ProducesResponseType(typeof(ContentDetailDto), StatusCodes.Status200OK)]
     [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ContentDetailDto>> Update(int id, [FromBody] UpdateContentDto dto)
     {
var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
         var content = await _contentService.UpdateAsync(id, dto, userId);
    return Ok(content);
      }

        /// <summary>
        /// Delete content
        /// </summary>
     [HttpDelete("{id}")]
 [Authorize]
      [Permission(Permissions.ContentDelete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
 public async Task<ActionResult> Delete(int id)
        {
 await _contentService.DeleteAsync(id);
      return NoContent();
 }

   /// <summary>
    /// Duplicate content
        /// </summary>
    [HttpPost("{id}/duplicate")]
      [Authorize]
        [Permission(Permissions.ContentCreate)]
        [ProducesResponseType(typeof(ContentDetailDto), StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
 public async Task<ActionResult<ContentDetailDto>> Duplicate(int id)
     {
     var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
     var content = await _contentService.DuplicateAsync(id, userId);
            return Ok(content);
        }

        /// <summary>
  /// Publish content
        /// </summary>
    [HttpPost("{id}/publish")]
    [Authorize]
        [Permission(Permissions.ContentPublish)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Publish(int id)
     {
await _contentService.PublishAsync(id);
 return NoContent();
        }

  /// <summary>
 /// Unpublish content
   /// </summary>
        [HttpPost("{id}/unpublish")]
        [Authorize]
        [Permission(Permissions.ContentPublish)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
 public async Task<ActionResult> Unpublish(int id)
        {
     await _contentService.UnpublishAsync(id);
      return NoContent();
    }

  /// <summary>
 /// Archive content
        /// </summary>
        [HttpPost("{id}/archive")]
    [Authorize]
      [Permission(Permissions.ContentEdit)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Archive(int id)
        {
   await _contentService.ArchiveAsync(id);
return NoContent();
        }

      /// <summary>
        /// Get featured content by content type
   /// </summary>
        [HttpGet("featured")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ContentListDto>), StatusCodes.Status200OK)]
 public async Task<ActionResult<IEnumerable<ContentListDto>>> GetFeatured(
     [FromQuery] int contentTypeId,
      [FromQuery] int count = 5)
        {
       var contents = await _contentService.GetFeaturedAsync(contentTypeId, count);
            return Ok(contents);
   }

        /// <summary>
        /// Get recent content by content type
        /// </summary>
        [HttpGet("recent")]
        [AllowAnonymous]
      [ProducesResponseType(typeof(IEnumerable<ContentListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ContentListDto>>> GetRecent(
         [FromQuery] int contentTypeId,
            [FromQuery] int count = 10)
        {
     var contents = await _contentService.GetRecentAsync(contentTypeId, count);
       return Ok(contents);
        }

        /// <summary>
 /// Get content by tag
        /// </summary>
        [HttpGet("by-tag/{tagId}")]
        [AllowAnonymous]
      [ProducesResponseType(typeof(IEnumerable<ContentListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ContentListDto>>> GetByTag(
         int tagId,
 [FromQuery] int pageSize = 10,
            [FromQuery] int page = 1)
     {
            var contents = await _contentService.GetByTagAsync(tagId, pageSize, page);
      return Ok(contents);
        }
    }
}
