using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Gahar_Backend.Constants;
using Gahar_Backend.Filters;
using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Form;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FormsController : ControllerBase
{
    private readonly IFormService _formService;
    private readonly ILogger<FormsController> _logger;

    public FormsController(IFormService formService, ILogger<FormsController> logger)
    {
        _formService = formService;
        _logger = logger;
    }

    #region Form Management

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<FormListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<FormListDto>>> GetAll([FromQuery] PageFilterDto filter)
    {
   _logger.LogInformation("Getting all forms");
        var forms = await _formService.GetAllAsync(filter);
        return Ok(forms);
  }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(FormDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<FormDetailDto>> GetById(int id)
    {
        _logger.LogInformation("Getting form {FormId}", id);
   var form = await _formService.GetByIdAsync(id);
        return Ok(form);
    }

 [HttpGet("slug/{slug}")]
    [AllowAnonymous]
[ProducesResponseType(typeof(FormDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<FormDetailDto>> GetBySlug(string slug)
    {
        _logger.LogInformation("Getting form by slug: {Slug}", slug);
   var form = await _formService.GetBySlugAsync(slug);
        return Ok(form);
    }

    [HttpPost]
    [Authorize]
    [Permission(Permissions.FormsCreate)]
    [ProducesResponseType(typeof(FormDetailDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<FormDetailDto>> Create([FromBody] CreateFormDto dto)
    {
   var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        _logger.LogInformation("Creating new form: {Title}", dto.Title);
     var form = await _formService.CreateAsync(dto, userId);
     return CreatedAtAction(nameof(GetById), new { id = form.Id }, form);
  }

    [HttpPut("{id}")]
    [Authorize]
    [Permission(Permissions.FormsEdit)]
    [ProducesResponseType(typeof(FormDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<FormDetailDto>> Update(int id, [FromBody] UpdateFormDto dto)
    {
    _logger.LogInformation("Updating form {FormId}", id);
        var form = await _formService.UpdateAsync(id, dto);
        return Ok(form);
    }

    [HttpDelete("{id}")]
    [Authorize]
    [Permission(Permissions.FormsDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting form {FormId}", id);
        await _formService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/publish")]
    [Authorize]
    [Permission(Permissions.FormsPublish)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Publish(int id)
    {
        _logger.LogInformation("Publishing form {FormId}", id);
        await _formService.PublishAsync(id);
   return NoContent();
    }

    [HttpPost("{id}/unpublish")]
    [Authorize]
    [Permission(Permissions.FormsPublish)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Unpublish(int id)
    {
        _logger.LogInformation("Unpublishing form {FormId}", id);
        await _formService.UnpublishAsync(id);
        return NoContent();
    }

    #endregion

    #region Form Field Management

    [HttpPost("{id}/fields")]
    [Authorize]
    [Permission(Permissions.FormsEdit)]
    [ProducesResponseType(typeof(FormFieldDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<FormFieldDto>> AddField(int id, [FromBody] CreateFormFieldDto dto)
    {
        _logger.LogInformation("Adding field to form {FormId}", id);
     var field = await _formService.AddFieldAsync(id, dto);
   return Ok(field);
    }

    [HttpPut("{id}/fields/{fieldId}")]
    [Authorize]
    [Permission(Permissions.FormsEdit)]
    [ProducesResponseType(typeof(FormFieldDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<FormFieldDto>> UpdateField(int id, int fieldId, [FromBody] UpdateFormFieldDto dto)
    {
        _logger.LogInformation("Updating field {FieldId} in form {FormId}", fieldId, id);
        var field = await _formService.UpdateFieldAsync(id, fieldId, dto);
     return Ok(field);
    }

    [HttpDelete("{id}/fields/{fieldId}")]
    [Authorize]
    [Permission(Permissions.FormsDelete)]
[ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteField(int id, int fieldId)
    {
      _logger.LogInformation("Deleting field {FieldId} from form {FormId}", fieldId, id);
        await _formService.DeleteFieldAsync(id, fieldId);
    return NoContent();
    }

    [HttpPost("{id}/reorder-fields")]
 [Authorize]
    [Permission(Permissions.FormsEdit)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ReorderFields(int id, [FromBody] ReorderFormFieldsDto dto)
    {
      _logger.LogInformation("Reordering fields in form {FormId}", id);
        await _formService.ReorderFieldsAsync(id, dto);
        return NoContent();
    }

    #endregion

    #region Form Submission

    [HttpPost("{id}/submit")]
[AllowAnonymous]
    [ProducesResponseType(typeof(FormSubmissionDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<FormSubmissionDto>> SubmitForm(int id, [FromBody] SubmitFormDto dto)
    {
        _logger.LogInformation("Form {FormId} submitted", id);
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
     var submission = await _formService.SubmitFormAsync(id, dto, ipAddress);
        return Ok(submission);
    }

    [HttpGet("{id}/submissions")]
    [Authorize]
    [Permission(Permissions.FormsSubmissions)]
    [ProducesResponseType(typeof(PagedResult<FormSubmissionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<FormSubmissionDto>>> GetSubmissions(
        int id,
        [FromQuery] FormSubmissionFilterDto filter)
    {
        _logger.LogInformation("Getting submissions for form {FormId}", id);
      filter.FormId = id;
        var submissions = await _formService.GetSubmissionsAsync(filter);
     return Ok(submissions);
    }

    [HttpGet("submissions/{submissionId}")]
    [Authorize]
    [Permission(Permissions.FormsSubmissions)]
    [ProducesResponseType(typeof(FormSubmissionDto), StatusCodes.Status200OK)]
  public async Task<ActionResult<FormSubmissionDto>> GetSubmission(int submissionId)
    {
        _logger.LogInformation("Getting submission {SubmissionId}", submissionId);
        var submission = await _formService.GetSubmissionAsync(submissionId);
  return Ok(submission);
    }

    [HttpPost("submissions/{submissionId}/read")]
    [Authorize]
  [Permission(Permissions.FormsSubmissions)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> MarkSubmissionAsRead(int submissionId)
    {
        _logger.LogInformation("Marking submission {SubmissionId} as read", submissionId);
        await _formService.MarkSubmissionAsReadAsync(submissionId);
   return NoContent();
    }

    [HttpPost("submissions/{submissionId}/archive")]
    [Authorize]
    [Permission(Permissions.FormsSubmissions)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> ArchiveSubmission(int submissionId)
    {
   _logger.LogInformation("Archiving submission {SubmissionId}", submissionId);
  await _formService.ArchiveSubmissionAsync(submissionId);
   return NoContent();
    }

    [HttpGet("{id}/submissions/unread")]
    [Authorize]
    [Permission(Permissions.FormsSubmissions)]
    [ProducesResponseType(typeof(IEnumerable<FormSubmissionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FormSubmissionDto>>> GetUnreadSubmissions(int id)
    {
      _logger.LogInformation("Getting unread submissions for form {FormId}", id);
     var submissions = await _formService.GetUnreadSubmissionsAsync(id);
    return Ok(submissions);
    }

    #endregion
}
