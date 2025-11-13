using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gahar_Backend.Constants;
using Gahar_Backend.Filters;
using Gahar_Backend.Models.DTOs.Content;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Controllers;

/// <summary>
/// إدارة المحتوى الديناميكي
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContentsController : ControllerBase
{
    private readonly IContentService _contentService;
    private readonly ILogger<ContentsController> _logger;

    /// <summary>
    /// تهيئة متحكم المحتوى
    /// </summary>
    public ContentsController(IContentService contentService, ILogger<ContentsController> logger)
    {
  _contentService = contentService;
_logger = logger;
    }

    /// <summary>
    /// الحصول على جميع المحتوى مع التصفية والترقيم
    /// </summary>
    /// <param name="filter">معايير البحث والتصفية</param>
    [HttpGet]
    [AllowAnonymous]
  [ProducesResponseType(typeof(PagedResult<ContentListDto>), StatusCodes.Status200OK)]
   public async Task<ActionResult<PagedResult<ContentListDto>>> GetAll([FromQuery] ContentFilterDto filter)
    {
        _logger.LogInformation("الحصول على المحتوى - الصفحة {Page}", filter.Page);
  var contents = await _contentService.GetAllAsync(filter);
        return Ok(contents);
    }

    /// <summary>
    /// الحصول على محتوى بواسطة المعرف
    /// </summary>
    /// <param name="id">معرف المحتوى</param>
    /// <param name="lang">اللغة</param>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ContentDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ContentDetailDto>> GetById(int id, [FromQuery] string? lang = "ar")
    {
        _logger.LogInformation("الحصول على المحتوى {ContentId}", id);
        var content = await _contentService.GetByIdAsync(id, lang);
      return Ok(content);
    }

    /// <summary>
    /// الحصول على محتوى بواسطة الكود المختصر
    /// </summary>
    /// <param name="slug">الكود المختصر للمحتوى</param>
    /// <param name="contentType">نوع المحتوى</param>
/// <param name="lang">اللغة</param>
    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ContentDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ContentDetailDto>> GetBySlug(string slug, [FromQuery] string contentType, [FromQuery] string? lang = "ar")
    {
        _logger.LogInformation("الحصول على المحتوى {Slug} من النوع {ContentType}", slug, contentType);
        var content = await _contentService.GetBySlugAsync(slug, contentType, lang);
        return Ok(content);
    }

    /// <summary>
    /// إنشاء محتوى جديد
    /// </summary>
/// <param name="dto">بيانات المحتوى الجديد</param>
    [HttpPost]
    [Authorize]
    [Permission(Permissions.ContentCreate)]
    [ProducesResponseType(typeof(ContentDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<ContentDto>> Create([FromBody] CreateContentDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        _logger.LogInformation("إنشاء محتوى جديد: {Title}", dto.Title);
   var content = await _contentService.CreateAsync(dto, userId);
    return CreatedAtAction(nameof(GetById), new { id = content.Id }, content);
    }

    /// <summary>
    /// تحديث محتوى
    /// </summary>
    /// <param name="id">معرف المحتوى</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("{id}")]
    [Authorize]
    [Permission(Permissions.ContentEdit)]
 [ProducesResponseType(typeof(ContentDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ContentDto>> Update(int id, [FromBody] UpdateContentDto dto)
  {
  _logger.LogInformation("تحديث المحتوى {ContentId}", id);
   var content = await _contentService.UpdateAsync(id, dto);
        return Ok(content);
    }

    /// <summary>
    /// حذف محتوى
    /// </summary>
    /// <param name="id">معرف المحتوى</param>
    [HttpDelete("{id}")]
    [Authorize]
    [Permission(Permissions.ContentDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("حذف المحتوى {ContentId}", id);
        await _contentService.DeleteAsync(id);
     return NoContent();
    }

 /// <summary>
    /// نشر محتوى
    /// </summary>
    /// <param name="id">معرف المحتوى</param>
    [HttpPost("{id}/publish")]
    [Authorize]
    [Permission(Permissions.ContentPublish)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> Publish(int id)
    {
        _logger.LogInformation("نشر المحتوى {ContentId}", id);
        await _contentService.PublishAsync(id);
        return NoContent();
    }

    /// <summary>
    /// إلغاء نشر محتوى
    /// </summary>
    /// <param name="id">معرف المحتوى</param>
    [HttpPost("{id}/unpublish")]
    [Authorize]
    [Permission(Permissions.ContentPublish)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Unpublish(int id)
    {
   _logger.LogInformation("إلغاء نشر المحتوى {ContentId}", id);
        await _contentService.UnpublishAsync(id);
        return NoContent();
    }

    /// <summary>
    /// جدولة محتوى للنشر في تاريخ معين
    /// </summary>
 /// <param name="id">معرف المحتوى</param>
    /// <param name="publishDate">تاريخ النشر</param>
    [HttpPost("{id}/schedule")]
    [Authorize]
 [Permission(Permissions.ContentPublish)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Schedule(int id, [FromQuery] DateTime publishDate)
    {
        _logger.LogInformation("جدولة نشر المحتوى {ContentId} في {PublishDate}", id, publishDate);
        await _contentService.ScheduleAsync(id, publishDate);
    return NoContent();
    }

    /// <summary>
  /// نسخ محتوى
  /// </summary>
    /// <param name="id">معرف المحتوى</param>
    [HttpPost("{id}/duplicate")]
    [Authorize]
    [Permission(Permissions.ContentCreate)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Duplicate(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        _logger.LogInformation("نسخ المحتوى {ContentId}", id);
  await _contentService.DuplicateAsync(id, userId);
    return NoContent();
    }

    /// <summary>
  /// نقل محتوى إلى نوع محتوى آخر
    /// </summary>
    /// <param name="id">معرف المحتوى</param>
    /// <param name="targetContentTypeId">معرف نوع المحتوى المستهدف</param>
    [HttpPut("{id}/move-to-type/{targetContentTypeId}")]
    [Authorize]
    [Permission(Permissions.ContentEdit)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> MoveToContentType(int id, int targetContentTypeId)
    {
      _logger.LogInformation("نقل المحتوى {ContentId} إلى النوع {TargetContentTypeId}", id, targetContentTypeId);
        await _contentService.MoveToContentTypeAsync(id, targetContentTypeId);
        return NoContent();
    }

  /// <summary>
    /// زيادة عدد المشاهدات
    /// </summary>
    /// <param name="id">معرف المحتوى</param>
  [HttpPost("{id}/increment-views")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> IncrementViews(int id)
{
        _logger.LogInformation("زيادة عدد المشاهدات للمحتوى {ContentId}", id);
      await _contentService.IncrementViewsAsync(id);
 return NoContent();
    }

    /// <summary>
    /// البحث عن محتوى
    /// </summary>
    /// <param name="searchTerm">كلمة البحث</param>
    /// <param name="filter">معايير البحث الإضافية</param>
    [HttpGet("search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<ContentListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ContentListDto>>> Search(string searchTerm, [FromQuery] ContentFilterDto filter)
    {
        _logger.LogInformation("البحث عن المحتوى: {SearchTerm}", searchTerm);
        var results = await _contentService.SearchAsync(searchTerm, filter);
        return Ok(results);
    }

    /// <summary>
    /// الحصول على المحتوى المميز
    /// </summary>
  /// <param name="count">عدد النتائج</param>
    [HttpGet("featured")]
    [AllowAnonymous]
[ProducesResponseType(typeof(IEnumerable<ContentListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ContentListDto>>> GetFeatured([FromQuery] int count = 10)
    {
  _logger.LogInformation("الحصول على {Count} محتوى مميز", count);
        var contents = await _contentService.GetFeaturedAsync(count);
        return Ok(contents);
    }

    /// <summary>
    /// الحصول على أحدث محتوى
    /// </summary>
    /// <param name="count">عدد النتائج</param>
    [HttpGet("recent")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<ContentListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ContentListDto>>> GetRecent([FromQuery] int count = 10)
    {
  _logger.LogInformation("الحصول على {Count} محتوى أحدث", count);
        var contents = await _contentService.GetRecentAsync(count);
        return Ok(contents);
    }
}
