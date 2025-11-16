using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Form;

namespace Gahar_Backend.Services.Interfaces;

public interface IFormService
{
    // Form Management
    Task<PagedResult<FormListDto>> GetAllAsync(PageFilterDto filter);
    Task<FormDetailDto> GetByIdAsync(int id);
    Task<FormDetailDto> GetBySlugAsync(string slug);
    Task<FormDetailDto> CreateAsync(CreateFormDto dto, int authorId);
    Task<FormDetailDto> UpdateAsync(int id, UpdateFormDto dto);
    Task DeleteAsync(int id);
    Task PublishAsync(int id);
Task UnpublishAsync(int id);

    // Form Field Management
    Task<FormFieldDto> AddFieldAsync(int formId, CreateFormFieldDto dto);
 Task<FormFieldDto> UpdateFieldAsync(int formId, int fieldId, UpdateFormFieldDto dto);
    Task DeleteFieldAsync(int formId, int fieldId);
    Task ReorderFieldsAsync(int formId, ReorderFormFieldsDto dto);

    // Form Submission
Task<FormSubmissionDto> SubmitFormAsync(int formId, SubmitFormDto dto, string? ipAddress = null);
    Task<PagedResult<FormSubmissionDto>> GetSubmissionsAsync(FormSubmissionFilterDto filter);
    Task<FormSubmissionDto> GetSubmissionAsync(int submissionId);
    Task MarkSubmissionAsReadAsync(int submissionId);
 Task ArchiveSubmissionAsync(int submissionId);
    Task<IEnumerable<FormSubmissionDto>> GetUnreadSubmissionsAsync(int formId);
}
