using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gahar_Backend.Constants;
using Gahar_Backend.Filters;
using Gahar_Backend.Models.DTOs.ContentType;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Controllers;

/// <summary>
/// إدارة أنواع المحتوى والحقول والتخطيطات والوسوم
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContentTypesController : ControllerBase
{
    private readonly IContentTypeService _contentTypeService;
    private readonly ILogger<ContentTypesController> _logger;

    /// <summary>
    /// تهيئة متحكم أنواع المحتوى
    /// </summary>
    public ContentTypesController(IContentTypeService contentTypeService, ILogger<ContentTypesController> logger)
    {
  _contentTypeService = contentTypeService;
        _logger = logger;
    }

    #region ContentType Management

    /// <summary>
    /// الحصول على جميع أنواع المحتوى
    /// </summary>
  [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<ContentTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ContentTypeDto>>> GetAll()
    {
        _logger.LogInformation("الحصول على جميع أنواع المحتوى");
        var contentTypes = await _contentTypeService.GetAllAsync();
      return Ok(contentTypes);
    }

    /// <summary>
 /// الحصول على نوع محتوى بواسطة المعرف
    /// </summary>
    /// <param name="id">معرف نوع المحتوى</param>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ContentTypeDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ContentTypeDetailDto>> GetById(int id)
    {
        _logger.LogInformation("الحصول على نوع المحتوى {ContentTypeId}", id);
      var contentType = await _contentTypeService.GetByIdAsync(id);
        return Ok(contentType);
    }

 /// <summary>
  /// الحصول على نوع محتوى بواسطة الكود المختصر
/// </summary>
    /// <param name="slug">الكود المختصر</param>
    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ContentTypeDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ContentTypeDto>> GetBySlug(string slug)
    {
        _logger.LogInformation("الحصول على نوع المحتوى {Slug}", slug);
        var contentType = await _contentTypeService.GetBySlugAsync(slug);
  return Ok(contentType);
    }

    /// <summary>
    /// إنشاء نوع محتوى جديد
    /// </summary>
    /// <param name="dto">بيانات نوع المحتوى الجديد</param>
    [HttpPost]
    [Authorize]
    [Permission(Permissions.ContentTypesCreate)]
    [ProducesResponseType(typeof(ContentTypeDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<ContentTypeDto>> Create([FromBody] CreateContentTypeDto dto)
    {
        _logger.LogInformation("إنشاء نوع محتوى جديد: {Name}", dto.Name);
        var contentType = await _contentTypeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = contentType.Id }, contentType);
    }

/// <summary>
    /// تحديث نوع المحتوى
    /// </summary>
    /// <param name="id">معرف نوع المحتوى</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("{id}")]
    [Authorize]
    [Permission(Permissions.ContentTypesEdit)]
    [ProducesResponseType(typeof(ContentTypeDto), StatusCodes.Status200OK)]
 public async Task<ActionResult<ContentTypeDto>> Update(int id, [FromBody] UpdateContentTypeDto dto)
    {
  _logger.LogInformation("تحديث نوع المحتوى {ContentTypeId}", id);
        var contentType = await _contentTypeService.UpdateAsync(id, dto);
        return Ok(contentType);
    }

    /// <summary>
    /// حذف نوع المحتوى
    /// </summary>
    /// <param name="id">معرف نوع المحتوى</param>
    [HttpDelete("{id}")]
    [Authorize]
    [Permission(Permissions.ContentTypesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
  _logger.LogInformation("حذف نوع المحتوى {ContentTypeId}", id);
      await _contentTypeService.DeleteAsync(id);
        return NoContent();
    }

    #endregion

    #region Field Management

  /// <summary>
    /// إضافة حقل لنوع المحتوى
    /// </summary>
    /// <param name="id">معرف نوع المحتوى</param>
    /// <param name="dto">بيانات الحقل الجديد</param>
    [HttpPost("{id}/fields")]
    [Authorize]
    [Permission(Permissions.ContentTypesEdit)]
    [ProducesResponseType(typeof(ContentTypeFieldDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ContentTypeFieldDto>> AddField(int id, [FromBody] CreateContentTypeFieldDto dto)
    {
        _logger.LogInformation("إضافة حقل لنوع المحتوى {ContentTypeId}", id);
  var field = await _contentTypeService.AddFieldAsync(id, dto);
        return Ok(field);
    }

    /// <summary>
    /// تحديث حقل نوع المحتوى
    /// </summary>
    /// <param name="id">معرف نوع المحتوى</param>
    /// <param name="fieldId">معرف الحقل</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("{id}/fields/{fieldId}")]
    [Authorize]
    [Permission(Permissions.ContentTypesEdit)]
    [ProducesResponseType(typeof(ContentTypeFieldDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ContentTypeFieldDto>> UpdateField(int id, int fieldId, [FromBody] UpdateContentTypeFieldDto dto)
    {
        _logger.LogInformation("تحديث الحقل {FieldId} في نوع المحتوى {ContentTypeId}", fieldId, id);
     var field = await _contentTypeService.UpdateFieldAsync(id, fieldId, dto);
        return Ok(field);
    }

    /// <summary>
    /// حذف حقل من نوع المحتوى
    /// </summary>
    /// <param name="id">معرف نوع المحتوى</param>
    /// <param name="fieldId">معرف الحقل</param>
    [HttpDelete("{id}/fields/{fieldId}")]
    [Authorize]
    [Permission(Permissions.ContentTypesEdit)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteField(int id, int fieldId)
    {
        _logger.LogInformation("حذف الحقل {FieldId} من نوع المحتوى {ContentTypeId}", fieldId, id);
        await _contentTypeService.DeleteFieldAsync(id, fieldId);
    return NoContent();
    }

    /// <summary>
    /// إعادة ترتيب حقول نوع المحتوى
 /// </summary>
    /// <param name="id">معرف نوع المحتوى</param>
    /// <param name="dto">قائمة معرفات الحقول بالترتيب الجديد</param>
    [HttpPost("{id}/reorder-fields")]
    [Authorize]
[Permission(Permissions.ContentTypesEdit)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ReorderFields(int id, [FromBody] ReorderFieldsDto dto)
    {
        _logger.LogInformation("إعادة ترتيب حقول نوع المحتوى {ContentTypeId}", id);
   await _contentTypeService.ReorderFieldsAsync(id, dto.FieldIds);
        return NoContent();
    }

  #endregion

    #region Tag Management

    /// <summary>
    /// الحصول على جميع الوسوم
    /// </summary>
    [HttpGet("tags")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<TagDto>), StatusCodes.Status200OK)]
  public async Task<ActionResult<IEnumerable<TagDto>>> GetAllTags()
    {
    _logger.LogInformation("الحصول على جميع الوسوم");
      var tags = await _contentTypeService.GetAllTagsAsync();
     return Ok(tags);
    }

    /// <summary>
    /// البحث عن وسوم
    /// </summary>
    /// <param name="searchTerm">كلمة البحث</param>
[HttpGet("tags/search")]
    [AllowAnonymous]
  [ProducesResponseType(typeof(IEnumerable<TagDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TagDto>>> SearchTags([FromQuery] string searchTerm)
    {
      _logger.LogInformation("البحث عن وسوم: {SearchTerm}", searchTerm);
 var tags = await _contentTypeService.SearchTagsAsync(searchTerm);
      return Ok(tags);
    }

    /// <summary>
    /// إنشاء وسم جديد
    /// </summary>
    /// <param name="dto">بيانات الوسم الجديد</param>
    [HttpPost("tags")]
    [Authorize]
    [Permission(Permissions.ContentTypesEdit)]
  [ProducesResponseType(typeof(TagDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<TagDto>> CreateTag([FromBody] CreateTagDto dto)
    {
  _logger.LogInformation("إنشاء وسم جديد: {TagName}", dto.Name);
        var tag = await _contentTypeService.CreateTagAsync(dto);
   return CreatedAtAction(nameof(GetAllTags), tag);
    }

    /// <summary>
  /// تحديث وسم
  /// </summary>
    /// <param name="id">معرف الوسم</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("tags/{id}")]
 [Authorize]
    [Permission(Permissions.ContentTypesEdit)]
    [ProducesResponseType(typeof(TagDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<TagDto>> UpdateTag(int id, [FromBody] CreateTagDto dto)
    {
        _logger.LogInformation("تحديث الوسم {TagId}", id);
        var tag = await _contentTypeService.UpdateTagAsync(id, dto);
        return Ok(tag);
    }

    /// <summary>
    /// حذف وسم
    /// </summary>
    /// <param name="id">معرف الوسم</param>
    [HttpDelete("tags/{id}")]
    [Authorize]
    [Permission(Permissions.ContentTypesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteTag(int id)
    {
        _logger.LogInformation("حذف الوسم {TagId}", id);
        await _contentTypeService.DeleteTagAsync(id);
        return NoContent();
    }

    #endregion

    #region Layout Management

 /// <summary>
    /// الحصول على جميع التخطيطات
    /// </summary>
    [HttpGet("layouts")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<LayoutDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LayoutDto>>> GetAllLayouts()
    {
      _logger.LogInformation("الحصول على جميع التخطيطات");
        var layouts = await _contentTypeService.GetAllLayoutsAsync();
   return Ok(layouts);
    }

    /// <summary>
    /// إنشاء تخطيط جديد
    /// </summary>
    /// <param name="dto">بيانات التخطيط الجديد</param>
    [HttpPost("layouts")]
    [Authorize]
    [Permission(Permissions.ContentTypesEdit)]
    [ProducesResponseType(typeof(LayoutDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<LayoutDto>> CreateLayout([FromBody] CreateLayoutDto dto)
    {
        _logger.LogInformation("إنشاء تخطيط جديد: {LayoutName}", dto.Name);
      var layout = await _contentTypeService.CreateLayoutAsync(dto);
        return CreatedAtAction(nameof(GetAllLayouts), layout);
    }

    /// <summary>
    /// تحديث تخطيط
    /// </summary>
    /// <param name="id">معرف التخطيط</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("layouts/{id}")]
    [Authorize]
  [Permission(Permissions.ContentTypesEdit)]
    [ProducesResponseType(typeof(LayoutDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<LayoutDto>> UpdateLayout(int id, [FromBody] UpdateLayoutDto dto)
    {
     _logger.LogInformation("تحديث التخطيط {LayoutId}", id);
        var layout = await _contentTypeService.UpdateLayoutAsync(id, dto);
        return Ok(layout);
    }

    /// <summary>
    /// حذف تخطيط
    /// </summary>
    /// <param name="id">معرف التخطيط</param>
    [HttpDelete("layouts/{id}")]
    [Authorize]
    [Permission(Permissions.ContentTypesEdit)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteLayout(int id)
    {
        _logger.LogInformation("حذف التخطيط {LayoutId}", id);
        await _contentTypeService.DeleteLayoutAsync(id);
    return NoContent();
    }

    /// <summary>
    /// تعيين تخطيط كافتراضي
    /// </summary>
    /// <param name="id">معرف التخطيط</param>
    [HttpPost("layouts/{id}/set-default")]
    [Authorize]
    [Permission(Permissions.ContentTypesEdit)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SetDefaultLayout(int id)
    {
        _logger.LogInformation("تعيين التخطيط {LayoutId} كافتراضي", id);
        await _contentTypeService.SetDefaultLayoutAsync(id);
        return NoContent();
    }

    #endregion
}
