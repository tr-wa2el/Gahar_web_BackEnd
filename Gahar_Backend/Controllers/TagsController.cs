using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Gahar_Backend.Models.DTOs.Content;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Filters;
using Gahar_Backend.Constants;

namespace Gahar_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
      private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
      {
            _tagService = tagService;
        }

        /// <summary>
        /// Get all tags
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
   [ProducesResponseType(typeof(IEnumerable<TagDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetAll()
        {
            var tags = await _tagService.GetAllAsync();
   return Ok(tags);
}

        /// <summary>
     /// Get tag by ID
  /// </summary>
  [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TagDto), StatusCodes.Status200OK)]
     [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TagDto>> GetById(int id)
      {
    var tag = await _tagService.GetByIdAsync(id);
   return Ok(tag);
        }

        /// <summary>
      /// Get tag by slug
        /// </summary>
        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
[ProducesResponseType(typeof(TagDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TagDto>> GetBySlug(string slug)
      {
     var tag = await _tagService.GetBySlugAsync(slug);
return Ok(tag);
        }

        /// <summary>
        /// Create new tag
        /// </summary>
        [HttpPost]
        [Authorize]
        [Permission(Permissions.ContentCreate)]
  [ProducesResponseType(typeof(TagDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
      public async Task<ActionResult<TagDto>> Create([FromBody] CreateTagDto dto)
        {
    var tag = await _tagService.CreateAsync(dto);
         return CreatedAtAction(nameof(GetById), new { id = tag.Id }, tag);
        }

        /// <summary>
        /// Update tag
     /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [Permission(Permissions.ContentEdit)]
        [ProducesResponseType(typeof(TagDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TagDto>> Update(int id, [FromBody] UpdateTagDto dto)
        {
            var tag = await _tagService.UpdateAsync(id, dto);
       return Ok(tag);
        }

        /// <summary>
        /// Delete tag
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
   [Permission(Permissions.ContentDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
        {
   await _tagService.DeleteAsync(id);
        return NoContent();
        }

        /// <summary>
        /// Get popular tags
      /// </summary>
     [HttpGet("popular")]
    [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<TagDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetPopular([FromQuery] int count = 10)
        {
     var tags = await _tagService.GetPopularAsync(count);
         return Ok(tags);
  }

        /// <summary>
        /// Search tags
        /// </summary>
        [HttpGet("search")]
    [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<TagDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TagDto>>> Search([FromQuery] string searchTerm)
     {
  var tags = await _tagService.SearchAsync(searchTerm);
            return Ok(tags);
        }
    }
}
