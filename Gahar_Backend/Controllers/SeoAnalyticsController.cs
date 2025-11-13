using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gahar_Backend.Constants;
using Gahar_Backend.Filters;
using Gahar_Backend.Models.DTOs.Seo;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Controllers;

/// <summary>
/// إدارة تحسين محركات البحث والتحليلات
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SeoAnalyticsController : ControllerBase
{
    private readonly ISeoAnalyticsService _seoAnalyticsService;
    private readonly ILogger<SeoAnalyticsController> _logger;

    /// <summary>
    /// تهيئة متحكم SEO والتحليلات
    /// </summary>
    public SeoAnalyticsController(ISeoAnalyticsService seoAnalyticsService, ILogger<SeoAnalyticsController> logger)
    {
        _seoAnalyticsService = seoAnalyticsService;
        _logger = logger;
    }

    #region SEO Metadata

    /// <summary>
    /// الحصول على بيانات SEO لكيان معين
    /// </summary>
    /// <param name="entityType">نوع الكيان (Page, Facility, Certificate)</param>
    /// <param name="entityId">معرف الكيان</param>
    [HttpGet("metadata")]
    [Authorize]
    [Permission(Permissions.SeoView)]
    [ProducesResponseType(typeof(SeoMetadataDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<SeoMetadataDto>> GetSeoMetadata([FromQuery] string entityType, [FromQuery] int entityId)
    {
  _logger.LogInformation("الحصول على بيانات SEO للكيان {EntityType}:{EntityId}", entityType, entityId);
        var seo = await _seoAnalyticsService.GetSeoMetadataAsync(entityType, entityId);
   return Ok(seo);
    }

    /// <summary>
    /// تحديث بيانات SEO
    /// </summary>
    /// <param name="entityType">نوع الكيان</param>
    /// <param name="entityId">معرف الكيان</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("metadata")]
    [Authorize]
    [Permission(Permissions.SeoEdit)]
    [ProducesResponseType(typeof(SeoMetadataDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<SeoMetadataDto>> UpdateSeoMetadata([FromQuery] string entityType, [FromQuery] int entityId, [FromBody] UpdateSeoMetadataDto dto)
    {
        _logger.LogInformation("تحديث بيانات SEO للكيان {EntityType}:{EntityId}", entityType, entityId);
        var seo = await _seoAnalyticsService.UpdateSeoMetadataAsync(entityType, entityId, dto);
        return Ok(seo);
    }

    /// <summary>
    /// إنشاء بيانات SEO جديدة
    /// </summary>
    /// <param name="entityType">نوع الكيان</param>
    /// <param name="entityId">معرف الكيان</param>
    /// <param name="dto">بيانات SEO</param>
   [HttpPost("metadata")]
    [Authorize]
    [Permission(Permissions.SeoEdit)]
    [ProducesResponseType(typeof(SeoMetadataDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<SeoMetadataDto>> CreateSeoMetadata([FromQuery] string entityType, [FromQuery] int entityId, [FromBody] UpdateSeoMetadataDto dto)
    {
        _logger.LogInformation("إنشاء بيانات SEO جديدة للكيان {EntityType}:{EntityId}", entityType, entityId);
        var seo = await _seoAnalyticsService.CreateSeoMetadataAsync(entityType, entityId, dto);
      return CreatedAtAction(nameof(GetSeoMetadata), new { entityType, entityId }, seo);
    }

    #endregion

    #region Page Analytics

    /// <summary>
    /// الحصول على إحصائيات صفحة معينة
    /// </summary>
    /// <param name="pageUrl">رابط الصفحة</param>
    [HttpGet("pages/{pageUrl}")]
    [Authorize]
    [Permission(Permissions.AnalyticsView)]
 [ProducesResponseType(typeof(PageAnalyticsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PageAnalyticsDto>> GetPageAnalytics(string pageUrl)
    {
        _logger.LogInformation("الحصول على إحصائيات الصفحة {PageUrl}", pageUrl);
    var analytics = await _seoAnalyticsService.GetPageAnalyticsAsync(pageUrl);
    return Ok(analytics);
 }

    /// <summary>
    /// الحصول على أكثر الصفحات زيارة
/// </summary>
    /// <param name="count">عدد الصفحات المطلوبة</param>
    [HttpGet("pages/top")]
    [Authorize]
    [Permission(Permissions.AnalyticsView)]
    [ProducesResponseType(typeof(IEnumerable<PageAnalyticsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PageAnalyticsDto>>> GetTopPages([FromQuery] int count = 10)
    {
        _logger.LogInformation("الحصول على أكثر {Count} صفحات زيارة", count);
  var pages = await _seoAnalyticsService.GetTopPagesAsync(count);
        return Ok(pages);
    }

  /// <summary>
    /// تسجيل زيارة صفحة
    /// </summary>
    /// <param name="pageUrl">رابط الصفحة</param>
    /// <param name="pageName">اسم الصفحة</param>
    [HttpPost("pages/view")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PageAnalyticsDto), StatusCodes.Status200OK)]
 public async Task<ActionResult<PageAnalyticsDto>> RecordPageView([FromQuery] string pageUrl, [FromQuery] string? pageName = null)
    {
        _logger.LogInformation("تسجيل زيارة الصفحة {PageUrl}", pageUrl);
        var analytics = await _seoAnalyticsService.RecordPageViewAsync(pageUrl, pageName);
        return Ok(analytics);
    }

    #endregion

    #region Events Tracking

    /// <summary>
    /// تتبع حدث
    /// </summary>
    /// <param name="dto">بيانات الحدث</param>
    [HttpPost("events/track")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> TrackEvent([FromBody] TrackEventDto dto)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
 var sessionId = HttpContext.Request.Cookies["session_id"];

        _logger.LogInformation("تتبع حدث {EventType}", dto.EventType);
        await _seoAnalyticsService.TrackEventAsync(dto, ipAddress, userAgent, null, sessionId);
     return NoContent();
    }

    /// <summary>
    /// الحصول على الأحداث حسب النوع
    /// </summary>
    /// <param name="eventType">نوع الحدث</param>
    /// <param name="take">عدد النتائج</param>
    [HttpGet("events/{eventType}")]
    [Authorize]
    [Permission(Permissions.AnalyticsView)]
    [ProducesResponseType(typeof(IEnumerable<AnalyticsEventDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AnalyticsEventDto>>> GetEventsByType(string eventType, [FromQuery] int take = 100)
    {
   _logger.LogInformation("الحصول على أحداث من نوع {EventType}", eventType);
        var events = await _seoAnalyticsService.GetEventsByTypeAsync(eventType, take);
    return Ok(events);
    }

    /// <summary>
    /// الحصول على أحداث الجلسة
    /// </summary>
    /// <param name="sessionId">معرف الجلسة</param>
    [HttpGet("events/session/{sessionId}")]
    [Authorize]
    [Permission(Permissions.AnalyticsView)]
    [ProducesResponseType(typeof(IEnumerable<AnalyticsEventDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AnalyticsEventDto>>> GetSessionEvents(string sessionId)
    {
     _logger.LogInformation("الحصول على أحداث الجلسة {SessionId}", sessionId);
        var events = await _seoAnalyticsService.GetSessionEventsAsync(sessionId);
        return Ok(events);
    }

    #endregion

    #region Keywords

    /// <summary>
    /// إضافة كلمة مفتاحية جديدة
    /// </summary>
    /// <param name="dto">بيانات الكلمة المفتاحية</param>
    [HttpPost("keywords")]
 [Authorize]
    [Permission(Permissions.KeywordsManage)]
    [ProducesResponseType(typeof(KeywordDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<KeywordDto>> AddKeyword([FromBody] CreateKeywordDto dto)
 {
        _logger.LogInformation("إضافة كلمة مفتاحية جديدة: {Term}", dto.Term);
        var keyword = await _seoAnalyticsService.AddKeywordAsync(dto);
        return CreatedAtAction(nameof(SearchKeywords), keyword);
    }

    /// <summary>
    /// تحديث الكلمة المفتاحية
  /// </summary>
    /// <param name="keywordId">معرف الكلمة</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("keywords/{keywordId}")]
    [Authorize]
    [Permission(Permissions.KeywordsManage)]
    [ProducesResponseType(typeof(KeywordDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<KeywordDto>> UpdateKeyword(int keywordId, [FromBody] CreateKeywordDto dto)
    {
        _logger.LogInformation("تحديث الكلمة المفتاحية {KeywordId}", keywordId);
        var keyword = await _seoAnalyticsService.UpdateKeywordAsync(keywordId, dto);
        return Ok(keyword);
    }

    /// <summary>
    /// البحث عن كلمات مفتاحية
    /// </summary>
  /// <param name="searchTerm">كلمة البحث</param>
    [HttpGet("keywords/search")]
    [Authorize]
    [Permission(Permissions.AnalyticsView)]
    [ProducesResponseType(typeof(IEnumerable<KeywordDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<KeywordDto>>> SearchKeywords([FromQuery] string searchTerm)
    {
  _logger.LogInformation("البحث عن كلمات مفتاحية: {SearchTerm}", searchTerm);
        var keywords = await _seoAnalyticsService.SearchKeywordsAsync(searchTerm);
        return Ok(keywords);
    }

    /// <summary>
  /// الحصول على الكلمات المفتاحية المستهدفة
  /// </summary>
    [HttpGet("keywords/targeted")]
    [Authorize]
    [Permission(Permissions.AnalyticsView)]
    [ProducesResponseType(typeof(IEnumerable<KeywordDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<KeywordDto>>> GetTargetedKeywords()
    {
  _logger.LogInformation("الحصول على الكلمات المفتاحية المستهدفة");
        var keywords = await _seoAnalyticsService.GetTargetedKeywordsAsync();
   return Ok(keywords);
    }

    #endregion
}
