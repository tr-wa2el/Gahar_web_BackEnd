using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gahar_Backend.Models.DTOs.ShortLinks;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Extensions;

namespace Gahar_Backend.Controllers;

/// <summary>
/// Controller for managing short links with QR code generation
/// Rate limiting is applied globally via middleware:
/// - Non-admin users: 1 POST/PUT per 3 minutes, 50 GET per 3 minutes
/// - Admin users: 1000 requests per 3 minutes
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShortLinksController : ControllerBase
{
    private readonly IShortLinkService _shortLinkService;
    private readonly ILogger<ShortLinksController> _logger;

    public ShortLinksController(IShortLinkService shortLinkService, ILogger<ShortLinksController> logger)
    {
   _shortLinkService = shortLinkService;
   _logger = logger;
    }

    /// <summary>
  /// Create a new short link
  /// Note: Non-admin users limited to 1 request per 3 minutes
    /// </summary>
    /// <param name="dto">Short link creation data</param>
 /// <returns>Created short link with QR code</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<ShortLinkDto>> CreateShortLink([FromBody] CreateShortLinkDto dto)
    {
 try
      {
    var userId = User.GetUserId();
  var result = await _shortLinkService.CreateShortLinkAsync(dto, userId);
   return CreatedAtAction(nameof(GetShortLink), new { id = result.Id }, result);
    }
 catch (Exception ex)
        {
    _logger.LogError(ex, "Error creating short link");
       return BadRequest(new { message = "فشل إنشاء الرابط المختصر" });
  }
    }

    /// <summary>
    /// Get short link by ID
    /// </summary>
    /// <param name="id">Short link ID</param>
    /// <returns>Short link details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
 [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ShortLinkDto>> GetShortLink(int id)
  {
 try
        {
   var result = await _shortLinkService.GetShortLinkAsync(id);
   return Ok(result);
    }
     catch (Exception ex)
   {
   _logger.LogError(ex, $"Error getting short link: {id}");
      return NotFound(new { message = "الرابط المختصر غير موجود" });
       }
    }

    /// <summary>
  /// Get short link by short code (public endpoint)
    /// </summary>
    [HttpGet("code/{shortCode}")]
   [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ShortLinkDto>> GetByCode(string shortCode)
    {
 try
     {
  var result = await _shortLinkService.GetShortLinkByCodeAsync(shortCode);
      return Ok(result);
   }
    catch (Exception ex)
   {
_logger.LogError(ex, $"Error getting short link by code: {shortCode}");
       return NotFound(new { message = "الرابط المختصر غير موجود" });
      }
    }

 /// <summary>
  /// Update short link
 /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
  public async Task<ActionResult<ShortLinkDto>> UpdateShortLink(int id, [FromBody] UpdateShortLinkDto dto)
    {
   try
 {
  var userId = User.GetUserId();
  var result = await _shortLinkService.UpdateShortLinkAsync(id, dto, userId);
return Ok(result);
    }
    catch (Exception ex)
     {
_logger.LogError(ex, $"Error updating short link: {id}");
    return BadRequest(new { message = "فشل تحديث الرابط المختصر" });
   }
    }

    /// <summary>
/// Delete short link
    /// </summary>
    [HttpDelete("{id}")]
 [ProducesResponseType(StatusCodes.Status204NoContent)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteShortLink(int id)
    {
 try
        {
  var userId = User.GetUserId();
  await _shortLinkService.DeleteShortLinkAsync(id, userId);
  return NoContent();
    }
        catch (Exception ex)
 {
  _logger.LogError(ex, $"Error deleting short link: {id}");
    return BadRequest(new { message = "فشل حذف الرابط المختصر" });
        }
    }

  /// <summary>
 /// Get user's short links
    /// </summary>
    [HttpGet("user/links")]
    [ProducesResponseType(StatusCodes.Status200OK)]
   public async Task<ActionResult<IEnumerable<ShortLinkDto>>> GetUserShortLinks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
 try
        {
    var userId = User.GetUserId();
var result = await _shortLinkService.GetUserShortLinksAsync(userId, page, pageSize);
        return Ok(result);
 }
 catch (Exception ex)
        {
   _logger.LogError(ex, "Error getting user short links");
    return BadRequest(new { message = "فشل استرجاع الروابط" });
 }
 }

    /// <summary>
    /// Search short links
 /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
public async Task<ActionResult<IEnumerable<ShortLinkDto>>> SearchShortLinks([FromQuery] string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
 try
        {
      var result = await _shortLinkService.SearchShortLinksAsync(searchTerm, page, pageSize);
    return Ok(result);
 }
  catch (Exception ex)
  {
  _logger.LogError(ex, $"Error searching short links: {searchTerm}");
     return BadRequest(new { message = "فشل البحث" });
     }
    }

    /// <summary>
    /// Get short links by category
    /// </summary>
  [HttpGet("category/{category}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ShortLinkDto>>> GetByCategory(string category, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
 try
  {
     var result = await _shortLinkService.GetShortLinksByCategoryAsync(category, page, pageSize);
       return Ok(result);
  }
  catch (Exception ex)
 {
_logger.LogError(ex, $"Error getting short links by category: {category}");
     return BadRequest(new { message = "فشل استرجاع الروابط" });
        }
    }

  /// <summary>
    /// Get active short links
  /// </summary>
   [HttpGet("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ShortLinkDto>>> GetActiveShortLinks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
 try
  {
    var result = await _shortLinkService.GetActiveShortLinksAsync(page, pageSize);
 return Ok(result);
    }
 catch (Exception ex)
  {
_logger.LogError(ex, "Error getting active short links");
   return BadRequest(new { message = "فشل استرجاع الروابط" });
 }
    }

    /// <summary>
    /// Get statistics for short link
    /// </summary>
    [HttpGet("{id}/statistics")]
 [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
   public async Task<ActionResult<ShortLinkStatisticsDto>> GetStatistics(int id)
    {
try
        {
  var result = await _shortLinkService.GetStatisticsAsync(id);
          return Ok(result);
        }
 catch (Exception ex)
  {
   _logger.LogError(ex, $"Error getting statistics for short link: {id}");
 return NotFound(new { message = "الرابط غير موجود" });
 }
    }

    /// <summary>
 /// Get top short links by clicks
 /// </summary>
    [HttpGet("top")]
 [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ShortLinkDto>>> GetTopShortLinks([FromQuery] int count = 10)
    {
 try
        {
  var result = await _shortLinkService.GetTopShortLinksAsync(count);
      return Ok(result);
        }
     catch (Exception ex)
    {
  _logger.LogError(ex, "Error getting top short links");
       return BadRequest(new { message = "فشل استرجاع الروابط" });
 }
    }

   /// <summary>
    /// Regenerate QR code with logo
    /// </summary>
 [HttpPost("{id}/regenerate-qr")]
    [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<QrCodeResultDto>> RegenerateQrCode(int id, [FromQuery] string? logoUrl = null)
    {
   try
        {
 var result = await _shortLinkService.RegenerateQrCodeAsync(id, logoUrl);
   return Ok(result);
   }
    catch (Exception ex)
  {
   _logger.LogError(ex, $"Error regenerating QR code for short link: {id}");
  return BadRequest(new { message = "فشل إعادة توليد رمز QR" });
  }
    }
}
