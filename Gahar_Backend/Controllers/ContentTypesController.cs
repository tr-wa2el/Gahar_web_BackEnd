using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Gahar_Backend.Models.DTOs.ContentType;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Filters;
using Gahar_Backend.Constants;

namespace Gahar_Backend.Controllers
{
[ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContentTypesController : ControllerBase
    {
 private readonly IContentTypeService _contentTypeService;

 public ContentTypesController(IContentTypeService contentTypeService)
   {
   _contentTypeService = contentTypeService;
        }

        /// <summary>
  /// Get all content types
        /// </summary>
   [HttpGet]
  [Permission(Permissions.ContentTypesView)]
      [ProducesResponseType(typeof(IEnumerable<ContentTypeDto>), StatusCodes.Status200OK)]
  public async Task<ActionResult<IEnumerable<ContentTypeDto>>> GetAll()
  {
   var contentTypes = await _contentTypeService.GetAllAsync();
  return Ok(contentTypes);
  }

    /// <summary>
      /// Get content type by ID
   /// </summary>
   [HttpGet("{id}")]
     [Permission(Permissions.ContentTypesView)]
[ProducesResponseType(typeof(ContentTypeDetailDto), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<ContentTypeDetailDto>> GetById(int id)
   {
var contentType = await _contentTypeService.GetByIdAsync(id);
  return Ok(contentType);
   }

   /// <summary>
        /// Get content type by slug
 /// </summary>
     [HttpGet("slug/{slug}")]
   [AllowAnonymous]
      [ProducesResponseType(typeof(ContentTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
 public async Task<ActionResult<ContentTypeDto>> GetBySlug(string slug)
        {
   var contentType = await _contentTypeService.GetBySlugAsync(slug);
        return Ok(contentType);
    }

     /// <summary>
/// Create a new content type
 /// </summary>
[HttpPost]
   [Permission(Permissions.ContentTypesCreate)]
        [ProducesResponseType(typeof(ContentTypeDto), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
 public async Task<ActionResult<ContentTypeDto>> Create([FromBody] CreateContentTypeDto dto)
  {
var contentType = await _contentTypeService.CreateAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = contentType.Id }, contentType);
   }

   /// <summary>
  /// Update a content type
 /// </summary>
   [HttpPut("{id}")]
   [Permission(Permissions.ContentTypesEdit)]
   [ProducesResponseType(typeof(ContentTypeDto), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
   public async Task<ActionResult<ContentTypeDto>> Update(int id, [FromBody] UpdateContentTypeDto dto)
    {
   var contentType = await _contentTypeService.UpdateAsync(id, dto);
return Ok(contentType);
}

   /// <summary>
        /// Delete a content type
   /// </summary>
  [HttpDelete("{id}")]
     [Permission(Permissions.ContentTypesDelete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
 [ProducesResponseType(StatusCodes.Status404NotFound)]
 public async Task<ActionResult> Delete(int id)
   {
   await _contentTypeService.DeleteAsync(id);
     return NoContent();
   }

   /// <summary>
  /// Add a field to a content type
/// </summary>
   [HttpPost("{id}/fields")]
  [Permission(Permissions.ContentTypesEdit)]
 [ProducesResponseType(typeof(ContentTypeFieldDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<ContentTypeFieldDto>> AddField(int id, [FromBody] CreateContentTypeFieldDto dto)
  {
   var field = await _contentTypeService.AddFieldAsync(id, dto);
        return Ok(field);
  }

  /// <summary>
  /// Update a field in a content type
  /// </summary>
 [HttpPut("{id}/fields/{fieldId}")]
 [Permission(Permissions.ContentTypesEdit)]
[ProducesResponseType(typeof(ContentTypeFieldDto), StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
     [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<ContentTypeFieldDto>> UpdateField(
  int id, 
int fieldId, 
    [FromBody] UpdateContentTypeFieldDto dto)
   {
     var field = await _contentTypeService.UpdateFieldAsync(id, fieldId, dto);
  return Ok(field);
  }

   /// <summary>
      /// Delete a field from a content type
        /// </summary>
[HttpDelete("{id}/fields/{fieldId}")]
        [Permission(Permissions.ContentTypesEdit)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
 [ProducesResponseType(StatusCodes.Status404NotFound)]
 public async Task<ActionResult> DeleteField(int id, int fieldId)
 {
   await _contentTypeService.DeleteFieldAsync(id, fieldId);
     return NoContent();
        }

  /// <summary>
   /// Reorder fields in a content type
     /// </summary>
[HttpPost("{id}/reorder-fields")]
[Permission(Permissions.ContentTypesEdit)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult> ReorderFields(int id, [FromBody] ReorderFieldsDto dto)
  {
        await _contentTypeService.ReorderFieldsAsync(id, dto.FieldIds);
    return NoContent();
   }
    }
}
