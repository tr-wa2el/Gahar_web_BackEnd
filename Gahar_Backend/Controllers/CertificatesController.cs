using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Gahar_Backend.Constants;
using Gahar_Backend.Filters;
using Gahar_Backend.Models.DTOs.Certificate;
using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Controllers;

/// <summary>
/// إدارة الشهادات والتصاريح المهنية
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CertificatesController : ControllerBase
{
    private readonly ICertificateService _certificateService;
    private readonly ILogger<CertificatesController> _logger;

    /// <summary>
    /// تهيئة متحكم الشهادات
    /// </summary>
    public CertificatesController(ICertificateService certificateService, ILogger<CertificatesController> logger)
    {
 _certificateService = certificateService;
        _logger = logger;
    }

  #region Certificate Management

    /// <summary>
    /// الحصول على جميع الشهادات مع التصفية
    /// </summary>
    /// <param name="filter">معاملات التصفية والبحث</param>
/// <returns>قائمة الشهادات مع معلومات الترقيم</returns>
[HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<CertificateListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<CertificateListDto>>> GetAll([FromQuery] CertificateFilterDto filter)
    {
        _logger.LogInformation("الحصول على جميع الشهادات");
        var certificates = await _certificateService.GetAllAsync(filter);
        return Ok(certificates);
    }

    /// <summary>
    /// الحصول على شهادة بواسطة المعرف
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
  /// <returns>تفاصيل الشهادة كاملة</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CertificateDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CertificateDetailDto>> GetById(int id)
    {
        _logger.LogInformation("الحصول على الشهادة {CertificateId}", id);
        var certificate = await _certificateService.GetByIdAsync(id);
    return Ok(certificate);
    }

    /// <summary>
    /// الحصول على شهادة بواسطة الكود (Slug)
    /// </summary>
    /// <param name="slug">الكود المختصر للشهادة</param>
    /// <returns>تفاصيل الشهادة المنشورة</returns>
    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CertificateDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CertificateDetailDto>> GetBySlug(string slug)
    {
        _logger.LogInformation("الحصول على الشهادة {Slug}", slug);
        var certificate = await _certificateService.GetBySlugAsync(slug);
return Ok(certificate);
    }

    /// <summary>
    /// إنشاء شهادة جديدة
    /// </summary>
    /// <param name="dto">بيانات الشهادة الجديدة</param>
    /// <returns>الشهادة المنشأة</returns>
    [HttpPost]
    [Authorize]
    [Permission(Permissions.CertificatesCreate)]
    [ProducesResponseType(typeof(CertificateDetailDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<CertificateDetailDto>> Create([FromBody] CreateCertificateDto dto)
    {
  var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    _logger.LogInformation("إنشاء شهادة جديدة: {Name}", dto.Name);
 var certificate = await _certificateService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = certificate.Id }, certificate);
    }

    /// <summary>
    /// تحديث بيانات الشهادة
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
    /// <param name="dto">البيانات الجديدة</param>
    /// <returns>الشهادة المحدثة</returns>
    [HttpPut("{id}")]
    [Authorize]
    [Permission(Permissions.CertificatesEdit)]
    [ProducesResponseType(typeof(CertificateDetailDto), StatusCodes.Status200OK)]
 public async Task<ActionResult<CertificateDetailDto>> Update(int id, [FromBody] UpdateCertificateDto dto)
    {
        _logger.LogInformation("تحديث الشهادة {CertificateId}", id);
        var certificate = await _certificateService.UpdateAsync(id, dto);
        return Ok(certificate);
    }

    /// <summary>
    /// حذف الشهادة
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
    [HttpDelete("{id}")]
    [Authorize]
  [Permission(Permissions.CertificatesDelete)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("حذف الشهادة {CertificateId}", id);
        await _certificateService.DeleteAsync(id);
        return NoContent();
    }

  /// <summary>
    /// نشر الشهادة
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
    [HttpPost("{id}/publish")]
    [Authorize]
    [Permission(Permissions.CertificatesPublish)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Publish(int id)
    {
_logger.LogInformation("نشر الشهادة {CertificateId}", id);
        await _certificateService.PublishAsync(id);
        return NoContent();
    }

    /// <summary>
    /// إلغاء نشر الشهادة
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
    [HttpPost("{id}/unpublish")]
    [Authorize]
    [Permission(Permissions.CertificatesPublish)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Unpublish(int id)
    {
        _logger.LogInformation("إلغاء نشر الشهادة {CertificateId}", id);
        await _certificateService.UnpublishAsync(id);
        return NoContent();
    }

    #endregion

    #region Categories

    /// <summary>
    /// إضافة فئة للشهادة
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
/// <param name="dto">بيانات الفئة</param>
    [HttpPost("{id}/categories")]
    [Authorize]
    [Permission(Permissions.CertificatesEdit)]
    [ProducesResponseType(typeof(CertificateCategoryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CertificateCategoryDto>> AddCategory(int id, [FromBody] CreateCertificateCategoryDto dto)
    {
        _logger.LogInformation("إضافة فئة للشهادة {CertificateId}", id);
  var category = await _certificateService.AddCategoryAsync(id, dto);
 return Ok(category);
    }

    /// <summary>
 /// تحديث الفئة
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
    /// <param name="categoryId">معرف الفئة</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("{id}/categories/{categoryId}")]
    [Authorize]
    [Permission(Permissions.CertificatesEdit)]
  [ProducesResponseType(typeof(CertificateCategoryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CertificateCategoryDto>> UpdateCategory(int id, int categoryId, [FromBody] UpdateCertificateCategoryDto dto)
    {
        _logger.LogInformation("تحديث الفئة {CategoryId} في الشهادة {CertificateId}", categoryId, id);
    var category = await _certificateService.UpdateCategoryAsync(id, categoryId, dto);
      return Ok(category);
    }

    /// <summary>
    /// حذف الفئة
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
    /// <param name="categoryId">معرف الفئة</param>
    [HttpDelete("{id}/categories/{categoryId}")]
    [Authorize]
    [Permission(Permissions.CertificatesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteCategory(int id, int categoryId)
    {
        _logger.LogInformation("حذف الفئة {CategoryId} من الشهادة {CertificateId}", categoryId, id);
        await _certificateService.DeleteCategoryAsync(id, categoryId);
     return NoContent();
    }

    #endregion

    #region Requirements

    /// <summary>
    /// إضافة متطلب للشهادة
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
    /// <param name="dto">بيانات المتطلب</param>
  [HttpPost("{id}/requirements")]
    [Authorize]
    [Permission(Permissions.CertificatesEdit)]
 [ProducesResponseType(typeof(CertificateRequirementDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CertificateRequirementDto>> AddRequirement(int id, [FromBody] CreateCertificateRequirementDto dto)
    {
        _logger.LogInformation("إضافة متطلب للشهادة {CertificateId}", id);
    var requirement = await _certificateService.AddRequirementAsync(id, dto);
        return Ok(requirement);
    }

    /// <summary>
    /// تحديث المتطلب
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
    /// <param name="requirementId">معرف المتطلب</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("{id}/requirements/{requirementId}")]
    [Authorize]
 [Permission(Permissions.CertificatesEdit)]
    [ProducesResponseType(typeof(CertificateRequirementDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CertificateRequirementDto>> UpdateRequirement(int id, int requirementId, [FromBody] UpdateCertificateRequirementDto dto)
    {
        _logger.LogInformation("تحديث المتطلب {RequirementId} في الشهادة {CertificateId}", requirementId, id);
        var requirement = await _certificateService.UpdateRequirementAsync(id, requirementId, dto);
      return Ok(requirement);
    }

    /// <summary>
    /// حذف المتطلب
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
    /// <param name="requirementId">معرف المتطلب</param>
    [HttpDelete("{id}/requirements/{requirementId}")]
    [Authorize]
    [Permission(Permissions.CertificatesDelete)]
 [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteRequirement(int id, int requirementId)
    {
        _logger.LogInformation("حذف المتطلب {RequirementId} من الشهادة {CertificateId}", requirementId, id);
        await _certificateService.DeleteRequirementAsync(id, requirementId);
        return NoContent();
    }

    #endregion

    #region Holders

    /// <summary>
    /// إضافة حامل شهادة
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
    /// <param name="dto">بيانات حامل الشهادة</param>
    [HttpPost("{id}/holders")]
    [Authorize]
    [Permission(Permissions.CertificatesEdit)]
    [ProducesResponseType(typeof(CertificateHolderDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CertificateHolderDto>> AddHolder(int id, [FromBody] CreateCertificateHolderDto dto)
    {
   _logger.LogInformation("إضافة حامل شهادة للشهادة {CertificateId}", id);
    var holder = await _certificateService.AddHolderAsync(id, dto);
        return Ok(holder);
    }

    /// <summary>
    /// تحديث بيانات حامل الشهادة
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
    /// <param name="holderId">معرف حامل الشهادة</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("{id}/holders/{holderId}")]
    [Authorize]
    [Permission(Permissions.CertificatesEdit)]
    [ProducesResponseType(typeof(CertificateHolderDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CertificateHolderDto>> UpdateHolder(int id, int holderId, [FromBody] UpdateCertificateHolderDto dto)
    {
        _logger.LogInformation("تحديث حامل الشهادة {HolderId} في الشهادة {CertificateId}", holderId, id);
      var holder = await _certificateService.UpdateHolderAsync(id, holderId, dto);
  return Ok(holder);
    }

    /// <summary>
    /// التحقق من حامل الشهادة
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
    /// <param name="holderId">معرف حامل الشهادة</param>
    /// <param name="isVerified">هل تم التحقق؟</param>
 /// <param name="notes">ملاحظات التحقق</param>
    [HttpPost("{id}/holders/{holderId}/verify")]
    [Authorize]
    [Permission(Permissions.CertificatesEdit)]
    [ProducesResponseType(typeof(CertificateHolderDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CertificateHolderDto>> VerifyHolder(int id, int holderId, [FromQuery] bool isVerified, [FromQuery] string? notes)
    {
        _logger.LogInformation("التحقق من حامل الشهادة {HolderId}", holderId);
        var holder = await _certificateService.VerifyHolderAsync(id, holderId, isVerified, notes);
      return Ok(holder);
    }

    /// <summary>
    /// حذف حامل الشهادة
    /// </summary>
    /// <param name="id">معرف الشهادة</param>
    /// <param name="holderId">معرف حامل الشهادة</param>
    [HttpDelete("{id}/holders/{holderId}")]
    [Authorize]
    [Permission(Permissions.CertificatesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteHolder(int id, int holderId)
  {
      _logger.LogInformation("حذف حامل الشهادة {HolderId} من الشهادة {CertificateId}", holderId, id);
        await _certificateService.DeleteHolderAsync(id, holderId);
        return NoContent();
    }

    /// <summary>
  /// البحث عن حاملي الشهادات
    /// </summary>
    /// <param name="searchTerm">كلمة البحث</param>
    [HttpGet("holders/search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<CertificateHolderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CertificateHolderDto>>> SearchHolders([FromQuery] string searchTerm)
{
        _logger.LogInformation("البحث عن حاملي الشهادات: {SearchTerm}", searchTerm);
  var holders = await _certificateService.SearchHoldersAsync(searchTerm);
        return Ok(holders);
    }

    #endregion
}
