using System.Text.Json;
using Gahar_Backend.Models.DTOs.ContentType;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;
using Gahar_Backend.Extensions;

namespace Gahar_Backend.Services.Implementations
{
    public class ContentTypeService : IContentTypeService
    {
   private readonly IContentTypeRepository _repository;
  private readonly ITranslationService _translationService;
        private readonly IAuditLogService _auditLogService;

   public ContentTypeService(
            IContentTypeRepository repository,
      ITranslationService translationService,
     IAuditLogService auditLogService)
        {
  _repository = repository;
     _translationService = translationService;
  _auditLogService = auditLogService;
        }

        public async Task<IEnumerable<ContentTypeDto>> GetAllAsync()
   {
            var contentTypes = await _repository.GetAllWithContentCountAsync();
 return contentTypes.Select(ct => MapToDto(ct));
        }

  public async Task<ContentTypeDetailDto> GetByIdAsync(int id)
    {
     var contentType = await _repository.GetByIdWithFieldsAsync(id);
    
  if (contentType == null)
     throw new NotFoundException($"Content type with id {id} not found");

    return MapToDetailDto(contentType);
   }

  public async Task<ContentTypeDto> GetBySlugAsync(string slug)
     {
var contentType = await _repository.GetBySlugAsync(slug);
 
   if (contentType == null)
     throw new NotFoundException($"Content type with slug '{slug}' not found");

    return MapToDto(contentType);
        }

public async Task<ContentTypeDto> CreateAsync(CreateContentTypeDto dto)
    {
 // Validate slug uniqueness
 if (await _repository.SlugExistsAsync(dto.Slug))
  throw new BadRequestException($"Content type with slug '{dto.Slug}' already exists");

 var contentType = new ContentType
 {
        Name = dto.Name,
     Slug = dto.Slug.ToSlug(),
      Description = dto.Description,
       Icon = dto.Icon,
       IsSinglePage = dto.IsSinglePage,
   MetaTitleTemplate = dto.MetaTitleTemplate,
    MetaDescriptionTemplate = dto.MetaDescriptionTemplate
   };

   await _repository.CreateAsync(contentType);
  
      await _auditLogService.LogAsync(
     userId: null,
        action: "Create",
     entityType: "ContentType",
         entityId: contentType.Id,
description: $"Created content type: {contentType.Name}"
  );

  return MapToDto(contentType);
  }

  public async Task<ContentTypeDto> UpdateAsync(int id, UpdateContentTypeDto dto)
  {
var contentType = await _repository.GetByIdAsync(id);
      
      if (contentType == null)
      throw new NotFoundException($"Content type with id {id} not found");

      // Validate slug uniqueness
     if (await _repository.SlugExistsAsync(dto.Slug, id))
    throw new BadRequestException($"Content type with slug '{dto.Slug}' already exists");

      contentType.Name = dto.Name;
   contentType.Slug = dto.Slug.ToSlug();
       contentType.Description = dto.Description;
            contentType.Icon = dto.Icon;
   contentType.IsSinglePage = dto.IsSinglePage;
      contentType.IsActive = dto.IsActive;
   contentType.DisplayOrder = dto.DisplayOrder;
  contentType.MetaTitleTemplate = dto.MetaTitleTemplate;
   contentType.MetaDescriptionTemplate = dto.MetaDescriptionTemplate;

  await _repository.UpdateAsync(contentType);
      
     await _auditLogService.LogAsync(
   userId: null,
      action: "Update",
 entityType: "ContentType",
    entityId: contentType.Id,
     description: $"Updated content type: {contentType.Name}"
 );

          return MapToDto(contentType);
     }

   public async Task DeleteAsync(int id)
 {
     var contentType = await _repository.GetByIdAsync(id);
     
  if (contentType == null)
     throw new NotFoundException($"Content type with id {id} not found");

await _repository.DeleteAsync(id);
     
     await _auditLogService.LogAsync(
         userId: null,
     action: "Delete",
                entityType: "ContentType",
     entityId: id,
 description: $"Deleted content type: {contentType.Name}"
       );
}

   public async Task<ContentTypeFieldDto> AddFieldAsync(int contentTypeId, CreateContentTypeFieldDto dto)
{
            var contentType = await _repository.GetByIdAsync(contentTypeId);
    
    if (contentType == null)
      throw new NotFoundException($"Content type with id {contentTypeId} not found");

   // Validate field key uniqueness
   if (await _repository.FieldKeyExistsAsync(contentTypeId, dto.FieldKey))
throw new BadRequestException($"Field with key '{dto.FieldKey}' already exists in this content type");

     var field = new ContentTypeField
  {
      ContentTypeId = contentTypeId,
   Name = dto.Name,
     FieldKey = dto.FieldKey,
 FieldType = dto.FieldType,
    Description = dto.Description,
     IsRequired = dto.IsRequired,
       IsTranslatable = dto.IsTranslatable,
     ShowInList = dto.ShowInList,
 ValidationRules = dto.ValidationRules,
        DefaultValue = dto.DefaultValue,
 Placeholder = dto.Placeholder,
Options = dto.Options != null ? JsonSerializer.Serialize(dto.Options) : null
   };

            contentType.Fields.Add(field);
   await _repository.UpdateAsync(contentType);

  await _auditLogService.LogAsync(
     userId: null,
    action: "Create",
   entityType: "ContentTypeField",
  entityId: field.Id,
         description: $"Added field '{field.Name}' to content type '{contentType.Name}'"
     );

        return MapFieldToDto(field);
    }

  public async Task<ContentTypeFieldDto> UpdateFieldAsync(int contentTypeId, int fieldId, UpdateContentTypeFieldDto dto)
  {
   var field = await _repository.GetFieldByIdAsync(fieldId);
    
   if (field == null || field.ContentTypeId != contentTypeId)
    throw new NotFoundException($"Field with id {fieldId} not found in content type {contentTypeId}");

          // Validate field key uniqueness
   if (await _repository.FieldKeyExistsAsync(contentTypeId, dto.FieldKey, fieldId))
   throw new BadRequestException($"Field with key '{dto.FieldKey}' already exists in this content type");

  field.Name = dto.Name;
          field.FieldKey = dto.FieldKey;
  field.FieldType = dto.FieldType;
     field.Description = dto.Description;
      field.IsRequired = dto.IsRequired;
        field.IsTranslatable = dto.IsTranslatable;
   field.ShowInList = dto.ShowInList;
 field.DisplayOrder = dto.DisplayOrder;
      field.ValidationRules = dto.ValidationRules;
          field.DefaultValue = dto.DefaultValue;
field.Placeholder = dto.Placeholder;
        field.Options = dto.Options != null ? JsonSerializer.Serialize(dto.Options) : null;

// Update through context
            await _repository.UpdateAsync(await _repository.GetByIdAsync(contentTypeId) ?? throw new NotFoundException("Content type not found"));

   await _auditLogService.LogAsync(
userId: null,
action: "Update",
       entityType: "ContentTypeField",
         entityId: fieldId,
   description: $"Updated field '{field.Name}'"
   );

    return MapFieldToDto(field);
}

        public async Task DeleteFieldAsync(int contentTypeId, int fieldId)
  {
    var field = await _repository.GetFieldByIdAsync(fieldId);
   
         if (field == null || field.ContentTypeId != contentTypeId)
      throw new NotFoundException($"Field with id {fieldId} not found in content type {contentTypeId}");

          var contentType = await _repository.GetByIdWithFieldsAsync(contentTypeId);
       contentType!.Fields.Remove(field);
    
        await _repository.UpdateAsync(contentType);

await _auditLogService.LogAsync(
       userId: null,
    action: "Delete",
   entityType: "ContentTypeField",
    entityId: fieldId,
       description: $"Deleted field '{field.Name}'"
     );
        }

 public async Task ReorderFieldsAsync(int contentTypeId, List<int> fieldIds)
{
   var contentType = await _repository.GetByIdWithFieldsAsync(contentTypeId);
    
  if (contentType == null)
    throw new NotFoundException($"Content type with id {contentTypeId} not found");

   for (int i = 0; i < fieldIds.Count; i++)
  {
    var field = contentType.Fields.FirstOrDefault(f => f.Id == fieldIds[i]);
         if (field != null)
{
         field.DisplayOrder = i;
 }
 }

 await _repository.UpdateAsync(contentType);

      await _auditLogService.LogAsync(
    userId: null,
     action: "Update",
entityType: "ContentType",
     entityId: contentTypeId,
         description: "Reordered content type fields"
  );
        }

      // Mapping methods
   private ContentTypeDto MapToDto(ContentType contentType)
    {
    return new ContentTypeDto
    {
Id = contentType.Id,
   Name = contentType.Name,
  Slug = contentType.Slug,
   Description = contentType.Description,
       Icon = contentType.Icon,
   IsSinglePage = contentType.IsSinglePage,
  IsActive = contentType.IsActive,
        DisplayOrder = contentType.DisplayOrder,
   ContentCount = contentType.Contents?.Count ?? 0,
         CreatedAt = contentType.CreatedAt
};
}

 private ContentTypeDetailDto MapToDetailDto(ContentType contentType)
    {
     return new ContentTypeDetailDto
  {
Id = contentType.Id,
  Name = contentType.Name,
    Slug = contentType.Slug,
   Description = contentType.Description,
 Icon = contentType.Icon,
  IsSinglePage = contentType.IsSinglePage,
     IsActive = contentType.IsActive,
       DisplayOrder = contentType.DisplayOrder,
       ContentCount = contentType.Contents?.Count ?? 0,
   CreatedAt = contentType.CreatedAt,
MetaTitleTemplate = contentType.MetaTitleTemplate,
  MetaDescriptionTemplate = contentType.MetaDescriptionTemplate,
    Fields = contentType.Fields.Select(f => MapFieldToDto(f)).ToList()
    };
  }

     private ContentTypeFieldDto MapFieldToDto(ContentTypeField field)
   {
            List<string>? options = null;
     if (!string.IsNullOrEmpty(field.Options))
            {
  try
      {
           options = JsonSerializer.Deserialize<List<string>>(field.Options);
     }
         catch
   {
    // Ignore deserialization errors
   }
  }

     return new ContentTypeFieldDto
{
    Id = field.Id,
     Name = field.Name,
    FieldKey = field.FieldKey,
   FieldType = field.FieldType,
  Description = field.Description,
        IsRequired = field.IsRequired,
    IsTranslatable = field.IsTranslatable,
            ShowInList = field.ShowInList,
     DisplayOrder = field.DisplayOrder,
    ValidationRules = field.ValidationRules,
      DefaultValue = field.DefaultValue,
   Placeholder = field.Placeholder,
       Options = options
};
        }
    }
}
