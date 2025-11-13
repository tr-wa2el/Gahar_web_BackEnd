using Gahar_Backend.Models.DTOs.ContentType;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Implementations;

public class ContentTypeService : IContentTypeService
{
    private readonly IContentTypeRepository _contentTypeRepo;
    private readonly IContentTypeFieldRepository _fieldRepo;
   private readonly ITagRepository _tagRepo;
  private readonly ILayoutRepository _layoutRepo;
    private readonly IAuditLogService _auditLogService;

    public ContentTypeService(
        IContentTypeRepository contentTypeRepo,
   IContentTypeFieldRepository fieldRepo,
        ITagRepository tagRepo,
        ILayoutRepository layoutRepo,
      IAuditLogService auditLogService)
    {
        _contentTypeRepo = contentTypeRepo;
     _fieldRepo = fieldRepo;
        _tagRepo = tagRepo;
        _layoutRepo = layoutRepo;
        _auditLogService = auditLogService;
    }

  #region ContentType Management

    public async Task<IEnumerable<ContentTypeDto>> GetAllAsync()
    {
        var contentTypes = await _contentTypeRepo.GetAllAsync();
return contentTypes.Select(ct => MapToDto(ct)).ToList();
    }

    public async Task<ContentTypeDetailDto> GetByIdAsync(int id)
    {
        var contentType = await _contentTypeRepo.GetWithFieldsAsync(id);
     if (contentType == null)
     throw new NotFoundException($"Content type with ID {id} not found");

        return MapToDetailDto(contentType);
    }

    public async Task<ContentTypeDto> GetBySlugAsync(string slug)
    {
        var contentType = await _contentTypeRepo.GetBySlugAsync(slug);
      if (contentType == null)
    throw new NotFoundException($"Content type with slug '{slug}' not found");

   return MapToDto(contentType);
    }

    public async Task<ContentTypeDto> CreateAsync(CreateContentTypeDto dto)
    {
   // Validate slug
    if (await _contentTypeRepo.SlugExistsAsync(dto.Slug))
            throw new BadRequestException($"Slug '{dto.Slug}' already exists");

        var contentType = new ContentType
       {
   Name = dto.Name,
   Slug = dto.Slug,
  Description = dto.Description,
    Icon = dto.Icon,
            IsSinglePage = dto.IsSinglePage,
    MetaTitleTemplate = dto.MetaTitleTemplate,
    MetaDescriptionTemplate = dto.MetaDescriptionTemplate,
     IsActive = true,
     DisplayOrder = 0
        };

        await _contentTypeRepo.AddAsync(contentType);
        await _contentTypeRepo.SaveChangesAsync();

        await _auditLogService.LogAsync(null, "Create", "ContentType", contentType.Id, $"Created content type: {contentType.Name}");

return MapToDto(contentType);
    }

    public async Task<ContentTypeDto> UpdateAsync(int id, UpdateContentTypeDto dto)
    {
     var contentType = await _contentTypeRepo.GetByIdAsync(id);
        if (contentType == null)
            throw new NotFoundException($"Content type with ID {id} not found");

   if (dto.Slug != contentType.Slug && await _contentTypeRepo.SlugExistsAsync(dto.Slug, id))
       throw new BadRequestException($"Slug '{dto.Slug}' already exists");

        contentType.Name = dto.Name;
        contentType.Slug = dto.Slug;
        contentType.Description = dto.Description;
       contentType.Icon = dto.Icon;
    contentType.IsSinglePage = dto.IsSinglePage;
    contentType.MetaTitleTemplate = dto.MetaTitleTemplate;
  contentType.MetaDescriptionTemplate = dto.MetaDescriptionTemplate;
  contentType.IsActive = dto.IsActive;
    contentType.DisplayOrder = dto.DisplayOrder;

        _contentTypeRepo.Update(contentType);
        await _contentTypeRepo.SaveChangesAsync();

      return MapToDto(contentType);
    }

    public async Task DeleteAsync(int id)
    {
        var contentType = await _contentTypeRepo.GetByIdAsync(id);
        if (contentType == null)
      throw new NotFoundException($"Content type with ID {id} not found");

    if (contentType.Contents?.Count > 0)
      throw new BadRequestException("Cannot delete content type that has contents");

      contentType.IsDeleted = true;
        _contentTypeRepo.Update(contentType);
        await _contentTypeRepo.SaveChangesAsync();
    }

    #endregion

    #region Field Management

    public async Task<ContentTypeFieldDto> AddFieldAsync(int contentTypeId, CreateContentTypeFieldDto dto)
    {
var contentType = await _contentTypeRepo.GetByIdAsync(contentTypeId);
        if (contentType == null)
  throw new NotFoundException($"Content type with ID {contentTypeId} not found");

        // Check if field key exists
        var existingField = await _fieldRepo.GetByKeyAsync(contentTypeId, dto.FieldKey);
    if (existingField != null)
          throw new BadRequestException($"Field key '{dto.FieldKey}' already exists");

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
       Options = dto.Options != null ? System.Text.Json.JsonSerializer.Serialize(dto.Options) : null
   };

    // Get max order
        var maxOrder = (await _fieldRepo.GetByContentTypeIdAsync(contentTypeId)).Max(f => (int?)f.DisplayOrder) ?? 0;
        field.DisplayOrder = maxOrder + 1;

      await _fieldRepo.AddAsync(field);
      await _fieldRepo.SaveChangesAsync();

        return MapFieldToDto(field);
    }

    public async Task<ContentTypeFieldDto> UpdateFieldAsync(int contentTypeId, int fieldId, UpdateContentTypeFieldDto dto)
    {
        var field = await _fieldRepo.GetByIdAsync(fieldId);
  if (field == null || field.ContentTypeId != contentTypeId)
         throw new NotFoundException("Field not found");

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
        field.Options = dto.Options != null ? System.Text.Json.JsonSerializer.Serialize(dto.Options) : null;

        _fieldRepo.Update(field);
await _fieldRepo.SaveChangesAsync();

    return MapFieldToDto(field);
    }

  public async Task DeleteFieldAsync(int contentTypeId, int fieldId)
    {
        var field = await _fieldRepo.GetByIdAsync(fieldId);
        if (field == null || field.ContentTypeId != contentTypeId)
            throw new NotFoundException("Field not found");

        field.IsDeleted = true;
        _fieldRepo.Update(field);
        await _fieldRepo.SaveChangesAsync();
    }

 public async Task ReorderFieldsAsync(int contentTypeId, List<int> fieldIds)
    {
        var fields = await _fieldRepo.GetByContentTypeIdAsync(contentTypeId);

        for (int i = 0; i < fieldIds.Count; i++)
        {
    var field = fields.FirstOrDefault(f => f.Id == fieldIds[i]);
   if (field != null)
            {
         field.DisplayOrder = i;
    _fieldRepo.Update(field);
        }
    }

        await _fieldRepo.SaveChangesAsync();
    }

    #endregion

    #region Tag Management

   public async Task<TagDto> CreateTagAsync(CreateTagDto dto)
    {
        if (await _tagRepo.SlugExistsAsync(dto.Slug))
            throw new BadRequestException($"Tag slug '{dto.Slug}' already exists");

      var tag = new Tag
        {
    Name = dto.Name,
  Slug = dto.Slug,
     Description = dto.Description,
    Color = dto.Color
        };

        await _tagRepo.AddAsync(tag);
 await _tagRepo.SaveChangesAsync();

        return MapTagToDto(tag);
    }

   public async Task<TagDto> UpdateTagAsync(int id, CreateTagDto dto)
    {
        var tag = await _tagRepo.GetByIdAsync(id);
        if (tag == null)
           throw new NotFoundException("Tag not found");

   if (dto.Slug != tag.Slug && await _tagRepo.SlugExistsAsync(dto.Slug, id))
            throw new BadRequestException($"Tag slug '{dto.Slug}' already exists");

        tag.Name = dto.Name;
   tag.Slug = dto.Slug;
     tag.Description = dto.Description;
        tag.Color = dto.Color;

    _tagRepo.Update(tag);
        await _tagRepo.SaveChangesAsync();

  return MapTagToDto(tag);
    }

    public async Task DeleteTagAsync(int id)
  {
        var tag = await _tagRepo.GetByIdAsync(id);
     if (tag == null)
 throw new NotFoundException("Tag not found");

        tag.IsDeleted = true;
        _tagRepo.Update(tag);
        await _tagRepo.SaveChangesAsync();
    }

    public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
    {
        var tags = await _tagRepo.GetAllAsync();
        return tags.Select(MapTagToDto);
    }

    public async Task<IEnumerable<TagDto>> SearchTagsAsync(string searchTerm)
    {
  var tags = await _tagRepo.SearchAsync(searchTerm);
    return tags.Select(MapTagToDto);
    }

    #endregion

    #region Layout Management

    public async Task<LayoutDto> CreateLayoutAsync(CreateLayoutDto dto)
    {
   var layout = new Layout
        {
    Name = dto.Name,
  Description = dto.Description,
 Configuration = dto.Configuration,
     PreviewImage = dto.PreviewImage,
    IsActive = true,
       IsDefault = false
        };

        await _layoutRepo.AddAsync(layout);
await _layoutRepo.SaveChangesAsync();

        return MapLayoutToDto(layout);
    }

    public async Task<LayoutDto> UpdateLayoutAsync(int id, UpdateLayoutDto dto)
    {
        var layout = await _layoutRepo.GetByIdAsync(id);
       if (layout == null)
          throw new NotFoundException("Layout not found");

        layout.Name = dto.Name;
        layout.Description = dto.Description;
   layout.Configuration = dto.Configuration;
    layout.PreviewImage = dto.PreviewImage;
        layout.IsActive = dto.IsActive;

        _layoutRepo.Update(layout);
        await _layoutRepo.SaveChangesAsync();

        return MapLayoutToDto(layout);
    }

    public async Task DeleteLayoutAsync(int id)
    {
        var layout = await _layoutRepo.GetByIdAsync(id);
        if (layout == null)
            throw new NotFoundException("Layout not found");

     if (layout.IsDefault)
            throw new BadRequestException("Cannot delete default layout");

   layout.IsDeleted = true;
      _layoutRepo.Update(layout);
        await _layoutRepo.SaveChangesAsync();
    }

    public async Task<IEnumerable<LayoutDto>> GetAllLayoutsAsync()
    {
        var layouts = await _layoutRepo.GetAllAsync();
        return layouts.Select(MapLayoutToDto);
    }

    public async Task SetDefaultLayoutAsync(int layoutId)
    {
        // Remove default from all
        var currentDefault = await _layoutRepo.GetDefaultAsync();
        if (currentDefault != null)
        {
  currentDefault.IsDefault = false;
        _layoutRepo.Update(currentDefault);
        }

 var layout = await _layoutRepo.GetByIdAsync(layoutId);
        if (layout == null)
       throw new NotFoundException("Layout not found");

   layout.IsDefault = true;
        _layoutRepo.Update(layout);
        await _layoutRepo.SaveChangesAsync();
    }

    #endregion

    #region Helper Methods

    private ContentTypeDto MapToDto(ContentType ct)
    {
        return new ContentTypeDto
        {
            Id = ct.Id,
      Name = ct.Name,
           Slug = ct.Slug,
    Description = ct.Description,
     Icon = ct.Icon,
 IsSinglePage = ct.IsSinglePage,
     IsActive = ct.IsActive,
            DisplayOrder = ct.DisplayOrder,
    ContentCount = ct.Contents?.Count ?? 0,
            FieldCount = ct.Fields?.Count ?? 0,
 CreatedAt = ct.CreatedAt
   };
    }

    private ContentTypeDetailDto MapToDetailDto(ContentType ct)
    {
  var dto = new ContentTypeDetailDto
        {
    Id = ct.Id,
            Name = ct.Name,
           Slug = ct.Slug,
Description = ct.Description,
         Icon = ct.Icon,
    IsSinglePage = ct.IsSinglePage,
            IsActive = ct.IsActive,
            DisplayOrder = ct.DisplayOrder,
     ContentCount = ct.Contents?.Count ?? 0,
  FieldCount = ct.Fields?.Count ?? 0,
            CreatedAt = ct.CreatedAt,
  MetaTitleTemplate = ct.MetaTitleTemplate,
     MetaDescriptionTemplate = ct.MetaDescriptionTemplate,
         Fields = ct.Fields?.Select(MapFieldToDto).ToList() ?? new()
      };

        return dto;
    }

    private ContentTypeFieldDto MapFieldToDto(ContentTypeField f)
    {
    var options = !string.IsNullOrEmpty(f.Options)
   ? System.Text.Json.JsonSerializer.Deserialize<List<string>>(f.Options)
      : null;

        return new ContentTypeFieldDto
     {
    Id = f.Id,
     Name = f.Name,
    FieldKey = f.FieldKey,
   FieldType = f.FieldType,
    Description = f.Description,
 IsRequired = f.IsRequired,
       IsTranslatable = f.IsTranslatable,
  ShowInList = f.ShowInList,
    DisplayOrder = f.DisplayOrder,
       ValidationRules = f.ValidationRules,
 DefaultValue = f.DefaultValue,
    Placeholder = f.Placeholder,
            Options = options
        };
    }

 private TagDto MapTagToDto(Tag t)
    {
        return new TagDto
        {
   Id = t.Id,
      Name = t.Name,
            Slug = t.Slug,
   Description = t.Description,
  Color = t.Color,
          ContentCount = t.ContentTags?.Count ?? 0
        };
    }

    private LayoutDto MapLayoutToDto(Layout l)
 {
        return new LayoutDto
        {
 Id = l.Id,
      Name = l.Name,
Description = l.Description,
       Configuration = l.Configuration,
     IsDefault = l.IsDefault,
       IsActive = l.IsActive,
      PreviewImage = l.PreviewImage
        };
    }

    #endregion
}
