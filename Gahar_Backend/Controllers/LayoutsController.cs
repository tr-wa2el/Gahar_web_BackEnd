using Microsoft.AspNetCore.Mvc;
using Gahar_Backend.Models.DTOs.Layout;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Filters;
using Gahar_Backend.Constants;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Controllers
{
    /// <summary>
    /// API Controller for managing layouts
    /// </summary>
    [ApiController]
  [Route("api/[controller]")]
    [ServiceFilter(typeof(AuditLogActionFilter))]
    public class LayoutsController : ControllerBase
    {
     private readonly ILayoutService _layoutService;
        private readonly ILogger<LayoutsController> _logger;

    public LayoutsController(
       ILayoutService layoutService,
            ILogger<LayoutsController> logger)
  {
            _layoutService = layoutService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all layouts
        /// </summary>
        /// <returns>List of all layouts</returns>
        [HttpGet]
 [Permission(Permissions.LayoutsView)]
  [ProducesResponseType(typeof(IEnumerable<LayoutDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LayoutDto>>> GetAllLayouts([FromQuery] bool activeOnly = false)
    {
            try
  {
                var layouts = activeOnly 
       ? await _layoutService.GetActiveLayoutsAsync()
    : await _layoutService.GetAllLayoutsAsync();
       
     return Ok(layouts);
      }
   catch (Exception ex)
            {
         _logger.LogError(ex, "Error retrieving layouts");
           return StatusCode(500, "An error occurred while retrieving layouts");
     }
        }

        /// <summary>
        /// Gets a layout by ID
    /// </summary>
        /// <param name="id">Layout ID</param>
 /// <returns>Layout details</returns>
    [HttpGet("{id}")]
        [Permission(Permissions.LayoutsView)]
      [ProducesResponseType(typeof(LayoutDto), StatusCodes.Status200OK)]
     [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LayoutDto>> GetLayoutById(int id)
{
       try
  {
                var layout = await _layoutService.GetLayoutByIdAsync(id);
        return Ok(layout);
            }
            catch (KeyNotFoundException ex)
{
                _logger.LogWarning(ex, "Layout not found: {Id}", id);
       return NotFound(new { message = ex.Message });
       }
catch (Exception ex)
            {
_logger.LogError(ex, "Error retrieving layout {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the layout");
        }
   }

        /// <summary>
   /// Gets a layout with statistics
    /// </summary>
        /// <param name="id">Layout ID</param>
        /// <returns>Layout with statistics</returns>
        [HttpGet("{id}/stats")]
  [Permission(Permissions.LayoutsView)]
  [ProducesResponseType(typeof(LayoutWithStatsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LayoutWithStatsDto>> GetLayoutWithStats(int id)
        {
          try
      {
        var layout = await _layoutService.GetLayoutWithStatsAsync(id);
    return Ok(layout);
            }
  catch (KeyNotFoundException ex)
         {
             _logger.LogWarning(ex, "Layout not found: {Id}", id);
   return NotFound(new { message = ex.Message });
            }
          catch (Exception ex)
  {
                _logger.LogError(ex, "Error retrieving layout stats {Id}", id);
                return StatusCode(500, "An error occurred while retrieving layout statistics");
            }
        }

        /// <summary>
        /// Gets the default layout
        /// </summary>
  /// <returns>Default layout</returns>
        [HttpGet("default")]
        [ProducesResponseType(typeof(LayoutDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LayoutDto>> GetDefaultLayout()
        {
   try
      {
         var layout = await _layoutService.GetDefaultLayoutAsync();
    
        if (layout == null)
                {
   return NotFound(new { message = "No default layout found" });
       }
                
        return Ok(layout);
            }
       catch (Exception ex)
            {
   _logger.LogError(ex, "Error retrieving default layout");
       return StatusCode(500, "An error occurred while retrieving the default layout");
            }
     }

  /// <summary>
        /// Creates a new layout
        /// </summary>
        /// <param name="createDto">Layout creation data</param>
     /// <returns>Created layout</returns>
        [HttpPost]
    [Permission(Permissions.LayoutsCreate)]
        [ProducesResponseType(typeof(LayoutDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LayoutDto>> CreateLayout([FromBody] CreateLayoutDto createDto)
        {
            try
            {
     if (!ModelState.IsValid)
       {
      return BadRequest(ModelState);
    }

   var layout = await _layoutService.CreateLayoutAsync(createDto);
  return CreatedAtAction(nameof(GetLayoutById), new { id = layout.Id }, layout);
    }
  catch (ValidationException ex)
   {
  _logger.LogWarning(ex, "Validation error creating layout");
   return BadRequest(new { message = ex.Message });
}
       catch (Exception ex)
            {
         _logger.LogError(ex, "Error creating layout");
       return StatusCode(500, "An error occurred while creating the layout");
   }
        }

 /// <summary>
      /// Updates an existing layout
        /// </summary>
        /// <param name="id">Layout ID</param>
        /// <param name="updateDto">Layout update data</param>
        /// <returns>Updated layout</returns>
   [HttpPut("{id}")]
        [Permission(Permissions.LayoutsEdit)]
    [ProducesResponseType(typeof(LayoutDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LayoutDto>> UpdateLayout(int id, [FromBody] UpdateLayoutDto updateDto)
        {
            try
          {
      if (!ModelState.IsValid)
    {
          return BadRequest(ModelState);
        }

        var layout = await _layoutService.UpdateLayoutAsync(id, updateDto);
        return Ok(layout);
     }
      catch (KeyNotFoundException ex)
 {
      _logger.LogWarning(ex, "Layout not found: {Id}", id);
  return NotFound(new { message = ex.Message });
       }
     catch (ValidationException ex)
      {
    _logger.LogWarning(ex, "Validation error updating layout {Id}", id);
      return BadRequest(new { message = ex.Message });
         }
        catch (Exception ex)
     {
      _logger.LogError(ex, "Error updating layout {Id}", id);
    return StatusCode(500, "An error occurred while updating the layout");
   }
        }

  /// <summary>
        /// Deletes a layout
        /// </summary>
    /// <param name="id">Layout ID</param>
        /// <returns>Success status</returns>
  [HttpDelete("{id}")]
     [Permission(Permissions.LayoutsDelete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
     [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLayout(int id)
        {
            try
    {
     await _layoutService.DeleteLayoutAsync(id);
   return NoContent();
}
    catch (KeyNotFoundException ex)
       {
   _logger.LogWarning(ex, "Layout not found: {Id}", id);
       return NotFound(new { message = ex.Message });
      }
 catch (ValidationException ex)
        {
        _logger.LogWarning(ex, "Validation error deleting layout {Id}", id);
    return BadRequest(new { message = ex.Message });
    }
   catch (Exception ex)
     {
       _logger.LogError(ex, "Error deleting layout {Id}", id);
          return StatusCode(500, "An error occurred while deleting the layout");
            }
        }

    /// <summary>
        /// Sets a layout as default
        /// </summary>
        /// <param name="id">Layout ID</param>
        /// <returns>Success status</returns>
     [HttpPost("{id}/set-default")]
        [Permission(Permissions.LayoutsEdit)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetAsDefault(int id)
        {
            try
      {
     await _layoutService.SetAsDefaultAsync(id);
    return Ok(new { message = "Layout set as default successfully" });
      }
    catch (KeyNotFoundException ex)
      {
_logger.LogWarning(ex, "Layout not found: {Id}", id);
   return NotFound(new { message = ex.Message });
       }
    catch (ValidationException ex)
         {
          _logger.LogWarning(ex, "Validation error setting default layout {Id}", id);
          return BadRequest(new { message = ex.Message });
      }
   catch (Exception ex)
          {
                _logger.LogError(ex, "Error setting default layout {Id}", id);
                return StatusCode(500, "An error occurred while setting the default layout");
 }
        }
    }
}
