using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gahar_Backend.Models.DTOs.Album;
using Gahar_Backend.Services.Interfaces;
using System.Security.Claims;

namespace Gahar_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumsController : ControllerBase
    {
      private readonly IAlbumService _albumService;
   private readonly ILogger<AlbumsController> _logger;

        public AlbumsController(
      IAlbumService albumService,
       ILogger<AlbumsController> logger)
   {
   _albumService = albumService;
     _logger = logger;
       }

        /// <summary>
      /// Get all albums with optional filtering
     /// </summary>
   /// <param name="isPublished">Filter by published status</param>
      /// <param name="searchTerm">Search term for title/description</param>
      /// <param name="page">Page number (default: 1)</param>
          /// <param name="pageSize">Items per page (default: 20)</param>
      /// <returns>Paginated list of albums</returns>
   [HttpGet]
    [AllowAnonymous]
     [ResponseCache(Duration = 300, VaryByQueryKeys = new[] { "isPublished", "searchTerm", "page", "pageSize" })]
        [ProducesResponseType(typeof(PagedResponse<AlbumDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetAll(
 [FromQuery] bool? isPublished = null,
      [FromQuery] string? searchTerm = null,
          [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
        {
   if (pageSize > 100) pageSize = 100;

  try
{
       var (items, totalCount) = await _albumService.GetAllAsync(
  isPublished,
            searchTerm,
   page,
pageSize);

      var response = new PagedResponse<AlbumDto>
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
 _logger.LogError(ex, "Error retrieving albums");
  return BadRequest(new { message = ex.Message });
       }
        }

        /// <summary>
        /// Get album by ID with full details
  /// </summary>
    /// <param name="id">Album ID</param>
   /// <returns>Album details with media</returns>
    [HttpGet("{id}")]
         [AllowAnonymous]
    [ProducesResponseType(typeof(AlbumDetailDto), StatusCodes.Status200OK)]
 [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> GetById(int id)
        {
    try
   {
          var album = await _albumService.GetByIdAsync(id);

     if (album == null)
    {
   return NotFound(new { message = "Album not found" });
  }

    return Ok(album);
    }
    catch (Exception ex)
         {
         _logger.LogError(ex, "Error retrieving album: {AlbumId}", id);
      return BadRequest(new { message = ex.Message });
  }
     }

        /// <summary>
        /// Get album by slug
  /// </summary>
     /// <param name="slug">Album slug</param>
  /// <returns>Album details</returns>
[HttpGet("slug/{slug}")]
     [AllowAnonymous]
    [ProducesResponseType(typeof(AlbumDetailDto), StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug(string slug)
        {
         try
         {
      var album = await _albumService.GetBySlugAsync(slug);

      if (album == null)
     {
   return NotFound(new { message = "Album not found" });
          }

    return Ok(album);
       }
      catch (Exception ex)
   {
 _logger.LogError(ex, "Error retrieving album by slug: {Slug}", slug);
 return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
     /// Create new album
     /// </summary>
 /// <param name="dto">Album creation data</param>
  /// <returns>Created album</returns>
   [HttpPost]
      [Authorize]
 [ProducesResponseType(typeof(AlbumDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateAlbumDto dto)
{
 try
  {
    var userId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;

       if (userId == 0)
    {
   return Unauthorized();
      }

      var album = await _albumService.CreateAsync(dto, userId);

 _logger.LogInformation("Album created: {AlbumTitle}", dto.Title);

     return CreatedAtAction(nameof(GetById), new { id = album.Id }, album);
 }
   catch (Exception ex)
  {
        _logger.LogError(ex, "Error creating album");
      return BadRequest(new { message = ex.Message });
       }
     }

        /// <summary>
   /// Update album
        /// </summary>
      /// <param name="id">Album ID</param>
    /// <param name="dto">Album update data</param>
    /// <returns>Updated album</returns>
  [HttpPut("{id}")]
        [Authorize]
[ProducesResponseType(typeof(AlbumDto), StatusCodes.Status200OK)]
 [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<IActionResult> Update(int id, [FromBody] UpdateAlbumDto dto)
 {
  try
    {
    var album = await _albumService.UpdateAsync(id, dto);

      _logger.LogInformation("Album updated: {AlbumId}", id);

   return Ok(album);
 }
       catch (Exception ex)
       {
    _logger.LogError(ex, "Error updating album: {AlbumId}", id);
       return BadRequest(new { message = ex.Message });
  }
    }

        /// <summary>
  /// Delete album
     /// </summary>
       /// <param name="id">Album ID</param>
    /// <returns>Success response</returns>
     [HttpDelete("{id}")]
    [Authorize]
         [ProducesResponseType(StatusCodes.Status200OK)]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
 public async Task<IActionResult> Delete(int id)
        {
  try
    {
     var result = await _albumService.DeleteAsync(id);

          _logger.LogInformation("Album deleted: {AlbumId}", id);

 return Ok(new { message = "Album deleted successfully", success = result });
       }
   catch (Exception ex)
        {
    _logger.LogError(ex, "Error deleting album: {AlbumId}", id);
         return BadRequest(new { message = ex.Message });
           }
    }

   /// <summary>
        /// Add media to album
        /// </summary>
 /// <param name="id">Album ID</param>
    /// <param name="dto">Media to add</param>
  /// <returns>Added media</returns>
      [HttpPost("{id}/media")]
       [Authorize]
 [ProducesResponseType(typeof(AlbumMediaDto), StatusCodes.Status200OK)]
     [ProducesResponseType(StatusCodes.Status404NotFound)]
       [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<IActionResult> AddMedia(int id, [FromBody] AddMediaToAlbumDto dto)
     {
        try
  {
    var media = await _albumService.AddMediaAsync(id, dto);

  _logger.LogInformation("Media added to album: AlbumId={AlbumId}, MediaId={MediaId}", id, dto.MediaId);

      return Ok(media);
        }
   catch (Exception ex)
    {
    _logger.LogError(ex, "Error adding media to album: {AlbumId}", id);
      return BadRequest(new { message = ex.Message });
  }
    }

    /// <summary>
    /// Add multiple media to album
  /// </summary>
      /// <param name="id">Album ID</param>
         /// <param name="dto">Media IDs to add</param>
 /// <returns>Added media list</returns>
  [HttpPost("{id}/media/bulk")]
       [Authorize]
    [ProducesResponseType(typeof(List<AlbumMediaDto>), StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
     public async Task<IActionResult> AddMultipleMedia(int id, [FromBody] AddMultipleMediaToAlbumDto dto)
     {
  try
     {
   var mediaList = await _albumService.AddMultipleMediaAsync(id, dto);

_logger.LogInformation("Multiple media added to album: AlbumId={AlbumId}, Count={Count}", id, mediaList.Count);

    return Ok(mediaList);
        }
  catch (Exception ex)
      {
  _logger.LogError(ex, "Error adding multiple media to album: {AlbumId}", id);
 return BadRequest(new { message = ex.Message });
         }
      }

     /// <summary>
   /// Remove media from album
     /// </summary>
      /// <param name="id">Album ID</param>
    /// <param name="mediaId">Media ID</param>
 /// <returns>Success response</returns>
    [HttpDelete("{id}/media/{mediaId}")]
        [Authorize]
  [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
 public async Task<IActionResult> RemoveMedia(int id, int mediaId)
 {
 try
  {
    var result = await _albumService.RemoveMediaAsync(id, mediaId);

       _logger.LogInformation("Media removed from album: AlbumId={AlbumId}, MediaId={MediaId}", id, mediaId);

    return Ok(new { message = "Media removed successfully", success = result });
      }
     catch (Exception ex)
    {
_logger.LogError(ex, "Error removing media from album: {AlbumId}", id);
 return BadRequest(new { message = ex.Message });
        }
  }

        /// <summary>
  /// Reorder media in album
      /// </summary>
     /// <param name="id">Album ID</param>
    /// <param name="dto">New media order</param>
    /// <returns>Success response</returns>
[HttpPost("{id}/reorder-media")]
      [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> ReorderMedia(int id, [FromBody] ReorderAlbumMediaDto dto)
        {
 try
   {
     var result = await _albumService.ReorderMediaAsync(id, dto);

_logger.LogInformation("Album media reordered: {AlbumId}", id);

         return Ok(new { message = "Media reordered successfully", success = result });
      }
   catch (Exception ex)
   {
   _logger.LogError(ex, "Error reordering media: {AlbumId}", id);
   return BadRequest(new { message = ex.Message });
         }
      }

    /// <summary>
     /// Set album cover image
  /// </summary>
      /// <param name="id">Album ID</param>
  /// <param name="mediaId">Media ID to set as cover</param>
        /// <returns>Updated album</returns>
    [HttpPost("{id}/cover/{mediaId}")]
     [Authorize]
    [ProducesResponseType(typeof(AlbumDto), StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
   public async Task<IActionResult> SetCoverImage(int id, int mediaId)
        {
        try
        {
  var album = await _albumService.SetCoverImageAsync(id, mediaId);

         _logger.LogInformation("Album cover image set: AlbumId={AlbumId}, MediaId={MediaId}", id, mediaId);

      return Ok(album);
 }
   catch (Exception ex)
     {
  _logger.LogError(ex, "Error setting cover image: {AlbumId}", id);
        return BadRequest(new { message = ex.Message });
   }
  }

        /// <summary>
   /// Get published albums
    /// </summary>
     /// <param name="top">Number of albums to return</param>
        /// <returns>List of published albums</returns>
   [HttpGet("published/top")]
      [AllowAnonymous]
     [ResponseCache(Duration = 600)]
        [ProducesResponseType(typeof(IEnumerable<AlbumDto>), StatusCodes.Status200OK)]
   public async Task<IActionResult> GetPublished([FromQuery] int top = 10)
     {
    try
  {
  if (top > 50) top = 50;

    var albums = await _albumService.GetPublishedAsync(top);
         return Ok(albums);
        }
    catch (Exception ex)
   {
_logger.LogError(ex, "Error retrieving published albums");
       return BadRequest(new { message = ex.Message });
  }
  }

      /// <summary>
    /// Search albums
        /// </summary>
    /// <param name="searchTerm">Search term</param>
/// <returns>Search results</returns>
[HttpGet("search/{searchTerm}")]
     [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<AlbumDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(string searchTerm)
        {
   try
     {
  var results = await _albumService.SearchAsync(searchTerm);

 _logger.LogInformation("Albums searched: {SearchTerm}", searchTerm);

        return Ok(results);
      }
    catch (Exception ex)
         {
  _logger.LogError(ex, "Error searching albums");
        return BadRequest(new { message = ex.Message });
    }
  }

   public class PagedResponse<T>
       {
  public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
   public int Page { get; set; }
        public int PageSize { get; set; }
   public int TotalPages { get; set; }
 }
    }
}
