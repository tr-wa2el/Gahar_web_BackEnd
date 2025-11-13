using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gahar_Backend.Constants;
using Gahar_Backend.Filters;
using Gahar_Backend.Models.DTOs.Layout;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Controllers;

/// <summary>
/// إدارة التخطيطات
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LayoutsController : ControllerBase
{
    private readonly ILayoutService _layoutService;
    private readonly ILogger<LayoutsController> _logger;

    /// <summary>
    /// تهيئة متحكم التخطيطات
    /// </summary>
 public LayoutsController(ILayoutService layoutService, ILogger<LayoutsController> logger)
    {
 _layoutService = layoutService;
  _logger = logger;
    }

    /// <summary>
    /// الحصول على جميع التخطيطات
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<LayoutDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LayoutDto>>> GetAll()
    {
   _logger.LogInformation("الحصول على جميع التخطيطات");
 var layouts = await _layoutService.GetAllAsync();
        return Ok(layouts);
    }

    /// <summary>
  /// الحصول على التخطيط الافتراضي
    /// </summary>
    [HttpGet("default")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LayoutDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<LayoutDto>> GetDefault()
    {
        _logger.LogInformation("الحصول على التخطيط الافتراضي");
        var layout = await _layoutService.GetDefaultAsync();
        return Ok(layout);
    }

/// <summary>
    /// الحصول على تخطيط بواسطة المعرف
    /// </summary>
    /// <param name="id">معرف التخطيط</param>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LayoutDetailDto), StatusCodes.Status200OK)]
 public async Task<ActionResult<LayoutDetailDto>> GetById(int id)
    {
        _logger.LogInformation("الحصول على التخطيط {LayoutId}", id);
        var layout = await _layoutService.GetByIdAsync(id);
        return Ok(layout);
    }

    /// <summary>
 /// إنشاء تخطيط جديد
    /// </summary>
    /// <param name="dto">بيانات التخطيط الجديد</param>
    [HttpPost]
    [Authorize]
    [Permission(Permissions.ContentTypesEdit)]
    [ProducesResponseType(typeof(LayoutDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<LayoutDto>> Create([FromBody] CreateLayoutDto dto)
    {
        _logger.LogInformation("إنشاء تخطيط جديد: {LayoutName}", dto.Name);
        var layout = await _layoutService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = layout.Id }, layout);
    }

    /// <summary>
    /// تحديث تخطيط
    /// </summary>
    /// <param name="id">معرف التخطيط</param>
    /// <param name="dto">البيانات الجديدة</param>
    [HttpPut("{id}")]
    [Authorize]
    [Permission(Permissions.ContentTypesEdit)]
    [ProducesResponseType(typeof(LayoutDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<LayoutDto>> Update(int id, [FromBody] UpdateLayoutDto dto)
    {
        _logger.LogInformation("تحديث التخطيط {LayoutId}", id);
   var layout = await _layoutService.UpdateAsync(id, dto);
        return Ok(layout);
    }

    /// <summary>
    /// حذف تخطيط
    /// </summary>
    /// <param name="id">معرف التخطيط</param>
    [HttpDelete("{id}")]
    [Authorize]
    [Permission(Permissions.ContentTypesDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
_logger.LogInformation("حذف التخطيط {LayoutId}", id);
        await _layoutService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// تعيين تخطيط كافتراضي
    /// </summary>
  /// <param name="id">معرف التخطيط</param>
    [HttpPost("{id}/set-default")]
    [Authorize]
    [Permission(Permissions.ContentTypesEdit)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SetDefault(int id)
  {
        _logger.LogInformation("تعيين التخطيط {LayoutId} كافتراضي", id);
        await _layoutService.SetDefaultAsync(id);
        return NoContent();
    }

    /// <summary>
    /// تحديث تخطيط عدة محتويات
    /// </summary>
    /// <param name="layoutId">معرف التخطيط</param>
    /// <param name="dto">البيانات تحتوي على معرفات المحتوى</param>
  [HttpPost("{layoutId}/bulk-assign")]
    [Authorize]
 [Permission(Permissions.ContentEdit)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> BulkAssignLayout(int layoutId, [FromBody] BulkUpdateLayoutDto dto)
    {
        _logger.LogInformation("تعيين التخطيط {LayoutId} لـ {Count} محتوى", layoutId, dto.ContentIds.Count);
    await _layoutService.BulkUpdateLayoutAsync(layoutId, dto.ContentIds);
        return NoContent();
    }
}
