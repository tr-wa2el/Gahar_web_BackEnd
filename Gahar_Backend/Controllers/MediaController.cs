using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gahar_Backend.Models.DTOs.Media;
using Gahar_Backend.Services.Interfaces;
using System.Security.Claims;

namespace Gahar_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MediaController : ControllerBase
  {
        private readonly IMediaService _mediaService;
        private readonly ILogger<MediaController> _logger;

    public MediaController(
  IMediaService mediaService,
           ILogger<MediaController> logger)
        {
            _mediaService = mediaService;
 _logger = logger;
   }

       /// <summary>
    /// Upload a single file
        /// </summary>
        /// <param name="request">Upload request with file and optional alt text</param>
   /// <returns>Uploaded media information</returns>
       [HttpPost("upload")]
 [ProducesResponseType(typeof(MediaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Upload([FromForm] MediaUploadDto request)
        {
   if (request.File == null || request.File.Length == 0)
        {
   return BadRequest(new { message = "No file provided" });
      }

         try
   {
       var userId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : (int?)null;

      var media = await _mediaService.UploadAsync(
   request.File,
          request.Alt,
    userId);

     _logger.LogInformation("File uploaded successfully: {FileName}", request.File.FileName);

    return CreatedAtAction(nameof(GetById), new { id = media.Id }, media);
        }
      catch (Exception ex)
    {
   _logger.LogError(ex, "Error uploading file");
         return BadRequest(new { message = ex.Message });
}
  }

     /// <summary>
       /// Upload multiple files
  /// </summary>
        /// <param name="request">Upload request with files collection</param>
    /// <returns>List of uploaded media information</returns>
        [HttpPost("upload-multiple")]
      [ProducesResponseType(typeof(IEnumerable<MediaDto>), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
       [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      public async Task<IActionResult> UploadMultiple([FromForm] BulkMediaUploadDto request)
     {
          if (request.Files == null || request.Files.Count == 0)
        {
          return BadRequest(new { message = "No files provided" });
   }

     try
     {
          var userId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : (int?)null;

   var mediaList = await _mediaService.UploadMultipleAsync(request.Files, userId);

     _logger.LogInformation("Multiple files uploaded: {Count} files", request.Files.Count);

  return CreatedAtAction(nameof(GetAll), mediaList);
       }
    catch (Exception ex)
      {
      _logger.LogError(ex, "Error uploading multiple files");
      return BadRequest(new { message = ex.Message });
          }
   }

      /// <summary>
  /// Get all media files with optional filtering
      /// </summary>
  /// <param name="mediaType">Filter by media type (Image, Video, Document, Audio)</param>
  /// <param name="searchTerm">Search term for file name or alt text</param>
/// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Items per page (default: 20)</param>
    /// <returns>Paginated list of media files</returns>
     [HttpGet]
  [AllowAnonymous]
   [ResponseCache(Duration = 300, VaryByQueryKeys = new[] { "mediaType", "searchTerm", "page", "pageSize" })]
    [ProducesResponseType(typeof(PagedResponse), StatusCodes.Status200OK)]
       public async Task<IActionResult> GetAll(
     [FromQuery] string? mediaType = null,
   [FromQuery] string? searchTerm = null,
          [FromQuery] int page = 1,
       [FromQuery] int pageSize = 20)
 {
         if (pageSize > 100) pageSize = 100;

       try
       {
            var (items, totalCount) = await _mediaService.GetAllAsync(mediaType, searchTerm, page, pageSize);

          var response = new PagedResponse
 {
      Items = items,
     TotalCount = totalCount,
    Page = page,
     PageSize = pageSize,
 TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
    };

         return Ok(response);
      }
     catch (Exception ex)
    {
          _logger.LogError(ex, "Error retrieving media files");
          return BadRequest(new { message = ex.Message });
        }
     }

       /// <summary>
        /// Get media by ID
    /// </summary>
      /// <param name="id">Media ID</param>
       /// <returns>Media information</returns>
[HttpGet("{id}")]
     [AllowAnonymous]
      [ProducesResponseType(typeof(MediaDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetById(int id)
   {
 try
   {
    var media = await _mediaService.GetByIdAsync(id);

    if (media == null)
        {
       return NotFound(new { message = "Media not found" });
         }

           return Ok(media);
  }
        catch (Exception ex)
  {
   _logger.LogError(ex, "Error retrieving media: {MediaId}", id);
      return BadRequest(new { message = ex.Message });
         }
    }

   /// <summary>
      /// Update media metadata
      /// </summary>
    /// <param name="id">Media ID</param>
     /// <param name="dto">Update information</param>
    /// <returns>Updated media information</returns>
        [HttpPut("{id}")]
       [ProducesResponseType(typeof(MediaDto), StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
     [ProducesResponseType(StatusCodes.Status400BadRequest)]
     public async Task<IActionResult> Update(int id, [FromBody] UpdateMediaDto dto)
        {
        try
      {
  var media = await _mediaService.UpdateAsync(id, dto);

    _logger.LogInformation("Media updated: {MediaId}", id);

     return Ok(media);
       }
     catch (Exception ex)
    {
     _logger.LogError(ex, "Error updating media: {MediaId}", id);
 return BadRequest(new { message = ex.Message });
   }
     }

        /// <summary>
      /// Delete media file
     /// </summary>
       /// <param name="id">Media ID</param>
      /// <returns>Success response</returns>
    [HttpDelete("{id}")]
       [ProducesResponseType(StatusCodes.Status200OK)]
 [ProducesResponseType(StatusCodes.Status404NotFound)]
   public async Task<IActionResult> Delete(int id)
   {
      try
     {
 var result = await _mediaService.DeleteAsync(id);

 _logger.LogInformation("Media deleted: {MediaId}", id);

   return Ok(new { message = "Media deleted successfully", success = result });
      }
        catch (Exception ex)
  {
   _logger.LogError(ex, "Error deleting media: {MediaId}", id);
          return BadRequest(new { message = ex.Message });
        }
     }

     /// <summary>
   /// Search media files
   /// </summary>
     /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching media files</returns>
   [HttpGet("search/{searchTerm}")]
  [AllowAnonymous]
       [ProducesResponseType(typeof(IEnumerable<MediaDto>), StatusCodes.Status200OK)]
       public async Task<IActionResult> Search(string searchTerm)
       {
        try
      {
    var results = await _mediaService.SearchAsync(searchTerm);

      _logger.LogInformation("Search performed: {SearchTerm}", searchTerm);

  return Ok(results);
        }
     catch (Exception ex)
  {
  _logger.LogError(ex, "Error searching media");
             return BadRequest(new { message = ex.Message });
      }
    }

        /// <summary>
   /// Regenerate WebP version of an image
 /// </summary>
      /// <param name="id">Media ID</param>
  /// <param name="dto">Regeneration settings</param>
   /// <returns>Updated media information</returns>
        [HttpPost("{id}/regenerate-webp")]
   [ProducesResponseType(typeof(MediaDto), StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> RegenerateWebP(int id, [FromBody] RegenerateWebPDto dto)
        {
    try
  {
           var media = await _mediaService.RegenerateWebPAsync(id, dto.Quality);

     _logger.LogInformation("WebP regenerated for media: {MediaId}", id);

      return Ok(media);
       }
        catch (Exception ex)
 {
 _logger.LogError(ex, "Error regenerating WebP: {MediaId}", id);
      return BadRequest(new { message = ex.Message });
 }
  }

    /// <summary>
    /// Get media statistics
  /// </summary>
    /// <returns>Media statistics</returns>
        [HttpGet("stats/overview")]
       [AllowAnonymous]
  [ProducesResponseType(typeof(MediaStatsDto), StatusCodes.Status200OK)]
   public async Task<IActionResult> GetStats()
      {
       try
     {
        var stats = await _mediaService.GetStatsAsync();
  return Ok(stats);
       }
         catch (Exception ex)
    {
       _logger.LogError(ex, "Error retrieving media statistics");
      return BadRequest(new { message = ex.Message });
        }
     }

    public class PagedResponse
    {
      public IEnumerable<MediaDto> Items { get; set; }
        public int TotalCount { get; set; }
       public int Page { get; set; }
      public int PageSize { get; set; }
       public int TotalPages { get; set; }
    }
    }
}
