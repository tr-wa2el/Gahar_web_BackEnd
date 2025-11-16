using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Gahar_Backend.Constants;
using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Form;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Implementations;

public class FormService : IFormService
{
    private readonly IFormRepository _formRepository;
    private readonly IFormFieldRepository _fieldRepository;
    private readonly IFormSubmissionRepository _submissionRepository;

  public FormService(
      IFormRepository formRepository,
IFormFieldRepository fieldRepository,
   IFormSubmissionRepository submissionRepository)
    {
        _formRepository = formRepository;
  _fieldRepository = fieldRepository;
     _submissionRepository = submissionRepository;
    }

    #region Form Management

    public async Task<PagedResult<FormListDto>> GetAllAsync(PageFilterDto filter)
    {
        var query = _formRepository.GetQueryable()
            .Include(f => f.Author)
            .Include(f => f.Fields)
        .Include(f => f.Submissions)
          .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(f =>
    f.Title.Contains(filter.SearchTerm) ||
        f.Description!.Contains(filter.SearchTerm));
        }

        if (filter.IsPublished.HasValue)
          query = query.Where(f => f.IsPublished == filter.IsPublished.Value);

 if (filter.AuthorId.HasValue)
      query = query.Where(f => f.AuthorId == filter.AuthorId.Value);

        var totalCount = await query.CountAsync();

        query = filter.SortBy?.ToLower() switch
        {
            "title" => filter.SortOrder?.ToLower() == "asc"
      ? query.OrderBy(f => f.Title)
                : query.OrderByDescending(f => f.Title),
      _ => filter.SortOrder?.ToLower() == "asc"
  ? query.OrderBy(f => f.CreatedAt)
            : query.OrderByDescending(f => f.CreatedAt)
  };

        var items = await query
      .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(f => new FormListDto
       {
            Id = f.Id,
     Title = f.Title,
          Slug = f.Slug,
           Description = f.Description,
         IsActive = f.IsActive,
           IsPublished = f.IsPublished,
                PublishedAt = f.PublishedAt,
       AuthorName = f.Author != null ? $"{f.Author.FirstName} {f.Author.LastName}".Trim() : null,
              FieldCount = f.Fields.Count,
          SubmissionCount = f.Submissions.Count,
                CreatedAt = f.CreatedAt,
        UpdatedAt = f.UpdatedAt
          })
            .ToListAsync();

        return new PagedResult<FormListDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
 }

    public async Task<FormDetailDto> GetByIdAsync(int id)
    {
        var form = await _formRepository.GetQueryable()
   .Include(f => f.Author)
        .Include(f => f.Fields.OrderBy(ff => ff.DisplayOrder))
            .FirstOrDefaultAsync(f => f.Id == id);

        if (form == null)
            throw new NotFoundException($"Form with ID {id} not found");

        return MapToDetailDto(form);
    }

  public async Task<FormDetailDto> GetBySlugAsync(string slug)
    {
        var form = await _formRepository.GetBySlugAsync(slug);

        if (form == null)
            throw new NotFoundException($"Form with slug '{slug}' not found");

        if (!form.IsPublished || !form.IsActive)
            throw new BadRequestException("Form is not available");

        return MapToDetailDto(form);
    }

    public async Task<FormDetailDto> CreateAsync(CreateFormDto dto, int authorId)
    {
    if (await _formRepository.SlugExistsAsync(dto.Slug))
       throw new BadRequestException($"Slug '{dto.Slug}' already exists");

        var form = new Form
   {
            Title = dto.Title,
     Slug = dto.Slug,
  Description = dto.Description,
            FormConfiguration = dto.FormConfiguration,
            AllowMultipleSubmissions = dto.AllowMultipleSubmissions,
            SuccessMessage = dto.SuccessMessage,
            ErrorMessage = dto.ErrorMessage,
  RedirectUrl = dto.RedirectUrl,
            SendNotificationEmail = dto.SendNotificationEmail,
    NotificationEmail = dto.NotificationEmail,
            SendSubmitterEmail = dto.SendSubmitterEmail,
            SubmitterEmailField = dto.SubmitterEmailField,
            AuthorId = authorId,
    IsActive = true,
  IsPublished = false
        };

      await _formRepository.AddAsync(form);
      await _formRepository.SaveChangesAsync();

        return await GetByIdAsync(form.Id);
    }

    public async Task<FormDetailDto> UpdateAsync(int id, UpdateFormDto dto)
    {
      var form = await _formRepository.GetByIdAsync(id);
        if (form == null)
            throw new NotFoundException($"Form with ID {id} not found");

        if (dto.Slug != form.Slug && await _formRepository.SlugExistsAsync(dto.Slug, id))
      throw new BadRequestException($"Slug '{dto.Slug}' already exists");

        form.Title = dto.Title;
        form.Slug = dto.Slug;
        form.Description = dto.Description;
        form.FormConfiguration = dto.FormConfiguration;
        form.AllowMultipleSubmissions = dto.AllowMultipleSubmissions;
        form.SuccessMessage = dto.SuccessMessage;
      form.ErrorMessage = dto.ErrorMessage;
   form.RedirectUrl = dto.RedirectUrl;
        form.SendNotificationEmail = dto.SendNotificationEmail;
        form.NotificationEmail = dto.NotificationEmail;
        form.SendSubmitterEmail = dto.SendSubmitterEmail;
      form.SubmitterEmailField = dto.SubmitterEmailField;
        form.IsActive = dto.IsActive;
        form.IsPublished = dto.IsPublished;

        _formRepository.Update(form);
  await _formRepository.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
      var form = await _formRepository.GetByIdAsync(id);
        if (form == null)
   throw new NotFoundException($"Form with ID {id} not found");

        _formRepository.Delete(form);
        await _formRepository.SaveChangesAsync();
    }

    public async Task PublishAsync(int id)
    {
        var form = await _formRepository.GetByIdAsync(id);
        if (form == null)
       throw new NotFoundException($"Form with ID {id} not found");

  form.IsPublished = true;
        form.PublishedAt = DateTime.UtcNow;

     _formRepository.Update(form);
   await _formRepository.SaveChangesAsync();
    }

 public async Task UnpublishAsync(int id)
    {
        var form = await _formRepository.GetByIdAsync(id);
        if (form == null)
     throw new NotFoundException($"Form with ID {id} not found");

  form.IsPublished = false;

        _formRepository.Update(form);
     await _formRepository.SaveChangesAsync();
    }

    #endregion

    #region Form Field Management

    public async Task<FormFieldDto> AddFieldAsync(int formId, CreateFormFieldDto dto)
    {
  var form = await _formRepository.GetByIdAsync(formId);
        if (form == null)
    throw new NotFoundException($"Form with ID {formId} not found");

        if (!FormFieldTypes.IsValid(dto.FieldType))
            throw new BadRequestException($"Invalid field type: {dto.FieldType}");

        var maxOrder = await _fieldRepository.GetQueryable()
        .Where(f => f.FormId == formId)
   .MaxAsync(f => (int?)f.DisplayOrder) ?? 0;

      var field = new FormField
        {
            FormId = formId,
            Label = dto.Label,
       FieldName = dto.FieldName,
       FieldType = dto.FieldType,
        FieldConfiguration = dto.FieldConfiguration,
    DisplayOrder = maxOrder + 1,
         IsRequired = dto.IsRequired,
      IsVisible = dto.IsVisible,
  PlaceHolder = dto.PlaceHolder,
 HelpText = dto.HelpText,
       CssClass = dto.CssClass,
      CustomId = dto.CustomId
        };

      await _fieldRepository.AddAsync(field);
  await _fieldRepository.SaveChangesAsync();

        return MapFieldToDto(field);
    }

    public async Task<FormFieldDto> UpdateFieldAsync(int formId, int fieldId, UpdateFormFieldDto dto)
    {
   var field = await _fieldRepository.GetQueryable()
    .FirstOrDefaultAsync(f => f.Id == fieldId && f.FormId == formId);

        if (field == null)
      throw new NotFoundException($"Field with ID {fieldId} not found in form {formId}");

        if (!FormFieldTypes.IsValid(dto.FieldType))
            throw new BadRequestException($"Invalid field type: {dto.FieldType}");

      field.Label = dto.Label;
        field.FieldName = dto.FieldName;
        field.FieldType = dto.FieldType;
   field.FieldConfiguration = dto.FieldConfiguration;
        field.DisplayOrder = dto.DisplayOrder;
        field.IsRequired = dto.IsRequired;
        field.IsVisible = dto.IsVisible;
     field.PlaceHolder = dto.PlaceHolder;
        field.HelpText = dto.HelpText;
        field.CssClass = dto.CssClass;
        field.CustomId = dto.CustomId;

        _fieldRepository.Update(field);
      await _fieldRepository.SaveChangesAsync();

  return MapFieldToDto(field);
    }

  public async Task DeleteFieldAsync(int formId, int fieldId)
    {
      var field = await _fieldRepository.GetQueryable()
        .FirstOrDefaultAsync(f => f.Id == fieldId && f.FormId == formId);

        if (field == null)
            throw new NotFoundException($"Field with ID {fieldId} not found in form {formId}");

        _fieldRepository.Delete(field);
        await _fieldRepository.SaveChangesAsync();
    }

    public async Task ReorderFieldsAsync(int formId, ReorderFormFieldsDto dto)
    {
        var form = await _formRepository.GetByIdAsync(formId);
        if (form == null)
     throw new NotFoundException($"Form with ID {formId} not found");

        await _fieldRepository.ReorderFieldsAsync(formId, dto.FieldIds);
    }

    #endregion

    #region Form Submission

 public async Task<FormSubmissionDto> SubmitFormAsync(int formId, SubmitFormDto dto, string? ipAddress = null)
    {
        var form = await _formRepository.GetByIdAsync(formId);
        if (form == null)
            throw new NotFoundException($"Form with ID {formId} not found");

        if (!form.IsPublished || !form.IsActive)
            throw new BadRequestException("Form is not available for submissions");

  var submission = new FormSubmission
        {
            FormId = formId,
            SubmissionData = JsonSerializer.Serialize(dto.Data),
SubmitterEmail = dto.Email,
         SubmitterIpAddress = ipAddress
        };

        await _submissionRepository.AddAsync(submission);
        await _submissionRepository.SaveChangesAsync();

        return MapSubmissionToDto(submission);
 }

public async Task<PagedResult<FormSubmissionDto>> GetSubmissionsAsync(FormSubmissionFilterDto filter)
    {
     var query = _submissionRepository.GetQueryable();

   if (filter.FormId.HasValue)
            query = query.Where(s => s.FormId == filter.FormId.Value);

        if (filter.IsRead.HasValue)
            query = query.Where(s => s.IsRead == filter.IsRead.Value);

        if (filter.IsArchived.HasValue)
            query = query.Where(s => s.IsArchived == filter.IsArchived.Value);

        if (filter.FromDate.HasValue)
            query = query.Where(s => s.CreatedAt >= filter.FromDate.Value);

        if (filter.ToDate.HasValue)
            query = query.Where(s => s.CreatedAt <= filter.ToDate.Value);

      if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
 query = query.Where(s => s.SubmitterEmail!.Contains(filter.SearchTerm));

      var totalCount = await query.CountAsync();

        query = filter.SortBy?.ToLower() switch
        {
  "email" => filter.SortOrder?.ToLower() == "asc"
                ? query.OrderBy(s => s.SubmitterEmail)
             : query.OrderByDescending(s => s.SubmitterEmail),
     _ => filter.SortOrder?.ToLower() == "asc"
         ? query.OrderBy(s => s.CreatedAt)
    : query.OrderByDescending(s => s.CreatedAt)
      };

        var items = await query
      .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
  .Select(s => MapSubmissionToDto(s))
    .ToListAsync();

   return new PagedResult<FormSubmissionDto>
        {
            Items = items,
 TotalCount = totalCount,
     PageNumber = filter.PageNumber,
  PageSize = filter.PageSize
        };
    }

    public async Task<FormSubmissionDto> GetSubmissionAsync(int submissionId)
    {
        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null)
    throw new NotFoundException($"Submission with ID {submissionId} not found");

        return MapSubmissionToDto(submission);
    }

    public async Task MarkSubmissionAsReadAsync(int submissionId)
    {
        await _submissionRepository.MarkAsReadAsync(submissionId);
    }

    public async Task ArchiveSubmissionAsync(int submissionId)
    {
        await _submissionRepository.ArchiveAsync(submissionId);
    }

    public async Task<IEnumerable<FormSubmissionDto>> GetUnreadSubmissionsAsync(int formId)
  {
        var submissions = await _submissionRepository.GetUnreadAsync(formId);
        return submissions.Select(MapSubmissionToDto);
    }

    #endregion

    #region Helper Methods

    private FormDetailDto MapToDetailDto(Form form)
    {
        return new FormDetailDto
        {
         Id = form.Id,
   Title = form.Title,
  Slug = form.Slug,
          Description = form.Description,
            IsActive = form.IsActive,
     IsPublished = form.IsPublished,
            PublishedAt = form.PublishedAt,
            AuthorName = form.Author != null ? $"{form.Author.FirstName} {form.Author.LastName}".Trim() : null,
   FieldCount = form.Fields.Count,
      SubmissionCount = form.Submissions.Count,
     CreatedAt = form.CreatedAt,
    UpdatedAt = form.UpdatedAt,
            FormConfiguration = form.FormConfiguration,
     AllowMultipleSubmissions = form.AllowMultipleSubmissions,
 SuccessMessage = form.SuccessMessage,
            ErrorMessage = form.ErrorMessage,
            RedirectUrl = form.RedirectUrl,
         SendNotificationEmail = form.SendNotificationEmail,
        NotificationEmail = form.NotificationEmail,
            SendSubmitterEmail = form.SendSubmitterEmail,
            SubmitterEmailField = form.SubmitterEmailField,
        Fields = form.Fields.OrderBy(f => f.DisplayOrder)
  .Select(MapFieldToDto)
    .ToList()
        };
    }

    private FormFieldDto MapFieldToDto(FormField field)
    {
    return new FormFieldDto
        {
       Id = field.Id,
 Label = field.Label,
       FieldName = field.FieldName,
FieldType = field.FieldType,
      FieldConfiguration = field.FieldConfiguration,
    DisplayOrder = field.DisplayOrder,
      IsRequired = field.IsRequired,
 IsVisible = field.IsVisible,
            PlaceHolder = field.PlaceHolder,
          HelpText = field.HelpText,
            CssClass = field.CssClass,
        CustomId = field.CustomId
        };
    }

    private FormSubmissionDto MapSubmissionToDto(FormSubmission submission)
    {
        return new FormSubmissionDto
    {
      Id = submission.Id,
            SubmissionData = submission.SubmissionData,
       SubmitterEmail = submission.SubmitterEmail,
        SubmitterIpAddress = submission.SubmitterIpAddress,
         IsRead = submission.IsRead,
     ReadAt = submission.ReadAt,
            IsArchived = submission.IsArchived,
   ArchivedAt = submission.ArchivedAt,
       Notes = submission.Notes,
          CreatedAt = submission.CreatedAt
     };
    }

    #endregion
}
