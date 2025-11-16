using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Gahar_Backend.Constants;
using Gahar_Backend.Filters;
using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Facility;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Controllers;

/// <summary>
/// الإدارة الكاملة للمنشآت الصحية
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FacilitiesController : ControllerBase
{
    private readonly IFacilityService _facilityService;
    private readonly ILogger<FacilitiesController> _logger;

    /// <summary>
  /// تهيئة متحكم المنشآت
    /// </summary>
    public FacilitiesController(IFacilityService facilityService, ILogger<FacilitiesController> logger)
    {
 _facilityService = facilityService;
        _logger = logger;
    }

    #region Facility Management

    /// <summary>
    /// الحصول على جميع المنشآت مع تصفية والبحث
    /// </summary>
    /// <param name="filter">معاملات التصفية والبحث والترتيب</param>
    /// <returns>قائمة بالمنشآت مع معلومات الترقيم</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<FacilityListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<FacilityListDto>>> GetAll([FromQuery] FacilityFilterDto filter)
    {
  _logger.LogInformation("الحصول على جميع المنشآت");
        var facilities = await _facilityService.GetAllAsync(filter);
     return Ok(facilities);
    }

    /// <summary>
    /// الحصول على منشأة بواسطة المعرف
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
    /// <returns>تفاصيل المنشأة كاملة</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(FacilityDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<FacilityDetailDto>> GetById(int id)
    {
     _logger.LogInformation("الحصول على المنشأة {FacilityId}", id);
        var facility = await _facilityService.GetByIdAsync(id);
        return Ok(facility);
    }

    /// <summary>
  /// الحصول على منشأة بواسطة الكود المختصر (Slug)
 /// </summary>
    /// <param name="slug">الكود المختصر للمنشأة</param>
    /// <returns>تفاصيل المنشأة المنشورة</returns>
    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(FacilityDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<FacilityDetailDto>> GetBySlug(string slug)
    {
        _logger.LogInformation("الحصول على المنشأة من خلال الكود: {Slug}", slug);
   var facility = await _facilityService.GetBySlugAsync(slug);
  return Ok(facility);
    }

    /// <summary>
    /// إنشاء منشأة جديدة
    /// </summary>
    /// <param name="dto">بيانات المنشأة الجديدة</param>
    /// <returns>تفاصيل المنشأة المنشأة</returns>
    [HttpPost]
    [Authorize]
    [Permission(Permissions.FacilitiesCreate)]
    [ProducesResponseType(typeof(FacilityDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<FacilityDetailDto>> Create([FromBody] CreateFacilityDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        _logger.LogInformation("إنشاء منشأة جديدة: {Name}", dto.Name);
        var facility = await _facilityService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = facility.Id }, facility);
    }

  /// <summary>
    /// تحديث بيانات المنشأة
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
    /// <param name="dto">البيانات الجديدة</param>
    /// <returns>المنشأة المحدثة</returns>
    [HttpPut("{id}")]
    [Authorize]
    [Permission(Permissions.FacilitiesEdit)]
    [ProducesResponseType(typeof(FacilityDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FacilityDetailDto>> Update(int id, [FromBody] UpdateFacilityDto dto)
    {
        _logger.LogInformation("تحديث المنشأة {FacilityId}", id);
        var facility = await _facilityService.UpdateAsync(id, dto);
   return Ok(facility);
    }

    /// <summary>
    /// حذف المنشأة
    /// </summary>
 /// <param name="id">معرف المنشأة</param>
    [HttpDelete("{id}")]
    [Authorize]
    [Permission(Permissions.FacilitiesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
   _logger.LogInformation("حذف المنشأة {FacilityId}", id);
 await _facilityService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// نشر المنشأة
    /// </summary>
/// <param name="id">معرف المنشأة</param>
  [HttpPost("{id}/publish")]
    [Authorize]
    [Permission(Permissions.FacilitiesPublish)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Publish(int id)
    {
        _logger.LogInformation("نشر المنشأة {FacilityId}", id);
        await _facilityService.PublishAsync(id);
        return NoContent();
    }

    /// <summary>
    /// إلغاء نشر المنشأة
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
  [HttpPost("{id}/unpublish")]
    [Authorize]
[Permission(Permissions.FacilitiesPublish)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Unpublish(int id)
    {
_logger.LogInformation("إلغاء نشر المنشأة {FacilityId}", id);
        await _facilityService.UnpublishAsync(id);
        return NoContent();
}

    #endregion

    #region Departments

    /// <summary>
  /// إضافة قسم جديد للمنشأة
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
    /// <param name="dto">بيانات القسم الجديد</param>
    [HttpPost("{id}/departments")]
    [Authorize]
    [Permission(Permissions.FacilitiesEdit)]
    [ProducesResponseType(typeof(FacilityDepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<FacilityDepartmentDto>> AddDepartment(int id, [FromBody] CreateFacilityDepartmentDto dto)
    {
        _logger.LogInformation("إضافة قسم للمنشأة {FacilityId}", id);
        var department = await _facilityService.AddDepartmentAsync(id, dto);
        return Ok(department);
    }

  /// <summary>
    /// تحديث بيانات القسم
    /// </summary>
  /// <param name="id">معرف المنشأة</param>
    /// <param name="departmentId">معرف القسم</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("{id}/departments/{departmentId}")]
    [Authorize]
    [Permission(Permissions.FacilitiesEdit)]
    [ProducesResponseType(typeof(FacilityDepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FacilityDepartmentDto>> UpdateDepartment(int id, int departmentId, [FromBody] UpdateFacilityDepartmentDto dto)
    {
     _logger.LogInformation("تحديث القسم {DepartmentId} في المنشأة {FacilityId}", departmentId, id);
    var department = await _facilityService.UpdateDepartmentAsync(id, departmentId, dto);
   return Ok(department);
    }

    /// <summary>
    /// حذف القسم
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
    /// <param name="departmentId">معرف القسم</param>
  [HttpDelete("{id}/departments/{departmentId}")]
    [Authorize]
    [Permission(Permissions.FacilitiesDelete)]
[ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteDepartment(int id, int departmentId)
    {
        _logger.LogInformation("حذف القسم {DepartmentId} من المنشأة {FacilityId}", departmentId, id);
        await _facilityService.DeleteDepartmentAsync(id, departmentId);
        return NoContent();
    }

    #endregion

    #region Services

    /// <summary>
    /// إضافة خدمة جديدة للمنشأة
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
    /// <param name="dto">بيانات الخدمة الجديدة</param>
    [HttpPost("{id}/services")]
    [Authorize]
 [Permission(Permissions.FacilitiesEdit)]
    [ProducesResponseType(typeof(FacilityServiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FacilityServiceDto>> AddService(int id, [FromBody] CreateFacilityServiceDto dto)
    {
        _logger.LogInformation("إضافة خدمة للمنشأة {FacilityId}", id);
    var service = await _facilityService.AddServiceAsync(id, dto);
  return Ok(service);
    }

    /// <summary>
    /// تحديث بيانات الخدمة
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
    /// <param name="serviceId">معرف الخدمة</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("{id}/services/{serviceId}")]
    [Authorize]
    [Permission(Permissions.FacilitiesEdit)]
    [ProducesResponseType(typeof(FacilityServiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FacilityServiceDto>> UpdateService(int id, int serviceId, [FromBody] UpdateFacilityServiceDto dto)
    {
        _logger.LogInformation("تحديث الخدمة {ServiceId} في المنشأة {FacilityId}", serviceId, id);
        var service = await _facilityService.UpdateServiceAsync(id, serviceId, dto);
        return Ok(service);
    }

    /// <summary>
  /// حذف الخدمة
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
    /// <param name="serviceId">معرف الخدمة</param>
    [HttpDelete("{id}/services/{serviceId}")]
    [Authorize]
    [Permission(Permissions.FacilitiesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteService(int id, int serviceId)
    {
        _logger.LogInformation("حذف الخدمة {ServiceId} من المنشأة {FacilityId}", serviceId, id);
      await _facilityService.DeleteServiceAsync(id, serviceId);
        return NoContent();
    }

    #endregion

    #region Images

    /// <summary>
    /// إضافة صورة جديدة للمنشأة
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
    /// <param name="dto">بيانات الصورة الجديدة</param>
    [HttpPost("{id}/images")]
    [Authorize]
    [Permission(Permissions.FacilitiesEdit)]
    [ProducesResponseType(typeof(FacilityImageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FacilityImageDto>> AddImage(int id, [FromBody] CreateFacilityImageDto dto)
    {
      _logger.LogInformation("إضافة صورة للمنشأة {FacilityId}", id);
        var image = await _facilityService.AddImageAsync(id, dto);
        return Ok(image);
    }

    /// <summary>
    /// تحديث بيانات الصورة
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
    /// <param name="imageId">معرف الصورة</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("{id}/images/{imageId}")]
    [Authorize]
    [Permission(Permissions.FacilitiesEdit)]
    [ProducesResponseType(typeof(FacilityImageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FacilityImageDto>> UpdateImage(int id, int imageId, [FromBody] UpdateFacilityImageDto dto)
    {
        _logger.LogInformation("تحديث الصورة {ImageId} في المنشأة {FacilityId}", imageId, id);
     var image = await _facilityService.UpdateImageAsync(id, imageId, dto);
        return Ok(image);
    }

    /// <summary>
    /// حذف الصورة
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
    /// <param name="imageId">معرف الصورة</param>
    [HttpDelete("{id}/images/{imageId}")]
    [Authorize]
    [Permission(Permissions.FacilitiesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteImage(int id, int imageId)
    {
        _logger.LogInformation("حذف الصورة {ImageId} من المنشأة {FacilityId}", imageId, id);
        await _facilityService.DeleteImageAsync(id, imageId);
        return NoContent();
    }

    #endregion

    #region Reviews

    /// <summary>
    /// إضافة تقييم جديد للمنشأة (من المستخدمين)
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
    /// <param name="dto">بيانات التقييم الجديد</param>
    [HttpPost("{id}/reviews")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(FacilityReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FacilityReviewDto>> AddReview(int id, [FromBody] CreateFacilityReviewDto dto)
    {
        _logger.LogInformation("إضافة تقييم للمنشأة {FacilityId}", id);
        var review = await _facilityService.AddReviewAsync(id, dto);
        return Ok(review);
    }

    /// <summary>
    /// الموافقة على التقييم أو رفضه
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
    /// <param name="reviewId">معرف التقييم</param>
 /// <param name="dto">حالة الموافقة</param>
    [HttpPost("{id}/reviews/{reviewId}/approve")]
    [Authorize]
    [Permission(Permissions.FacilitiesEdit)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ApproveReview(int id, int reviewId, [FromBody] ApproveFacilityReviewDto dto)
    {
        _logger.LogInformation("الموافقة على التقييم {ReviewId} للمنشأة {FacilityId}", reviewId, id);
        await _facilityService.ApproveReviewAsync(id, reviewId, dto);
        return NoContent();
    }

    /// <summary>
    /// الحصول على التقييمات المعتمدة للمنشأة
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
  [HttpGet("{id}/reviews/approved")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<FacilityReviewDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FacilityReviewDto>>> GetApprovedReviews(int id)
    {
        _logger.LogInformation("الحصول على التقييمات المعتمدة للمنشأة {FacilityId}", id);
 var reviews = await _facilityService.GetApprovedReviewsAsync(id);
return Ok(reviews);
    }

 /// <summary>
    /// حذف التقييم
    /// </summary>
    /// <param name="id">معرف المنشأة</param>
    /// <param name="reviewId">معرف التقييم</param>
    [HttpDelete("{id}/reviews/{reviewId}")]
    [Authorize]
 [Permission(Permissions.FacilitiesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteReview(int id, int reviewId)
    {
        _logger.LogInformation("حذف التقييم {ReviewId} من المنشأة {FacilityId}", reviewId, id);
        await _facilityService.DeleteReviewAsync(id, reviewId);
        return NoContent();
    }

    #endregion
}
