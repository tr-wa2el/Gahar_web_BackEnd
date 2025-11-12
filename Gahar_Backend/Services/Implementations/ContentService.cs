using System.Text.Json;
using Gahar_Backend.Data;
using Gahar_Backend.Models.DTOs.Content;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;
using Gahar_Backend.Extensions;

namespace Gahar_Backend.Services.Implementations
{
    public class ContentService : IContentService
    {
        private readonly IContentRepository _contentRepository;
        private readonly IContentTypeRepository _contentTypeRepository;
   private readonly ITagRepository _tagRepository;
      private readonly IAuditLogService _auditLogService;
        private readonly ApplicationDbContext _context;

        public ContentService(
     IContentRepository contentRepository,
    IContentTypeRepository contentTypeRepository,
      ITagRepository tagRepository,
         IAuditLogService auditLogService,
     ApplicationDbContext context)
    {
            _contentRepository = contentRepository;
  _contentTypeRepository = contentTypeRepository;
     _tagRepository = tagRepository;
      _auditLogService = auditLogService;
   _context = context;
        }

        public async Task<PagedResultDto<ContentListDto>> GetAllAsync(ContentFilterDto filter)
   {
      var contents = await _contentRepository.GetAllAsync(filter);
     var totalCount = await _contentRepository.GetTotalCountAsync(filter);

  var contentDtos = contents.Select(c => MapToListDto(c)).ToList();

     return new PagedResultDto<ContentListDto>
    {
        Items = contentDtos,
          TotalCount = totalCount,
 Page = filter.Page,
    PageSize = filter.PageSize
   };
        }

      public async Task<ContentDetailDto> GetByIdAsync(int id)
 {
            var content = await _contentRepository.GetByIdWithDetailsAsync(id);
  
    if (content == null)
        throw new NotFoundException($"Content with id {id} not found");

          return MapToDetailDto(content);
 }

     public async Task<ContentDetailDto> GetBySlugAsync(string slug, int contentTypeId)
        {
     var content = await _contentRepository.GetBySlugAsync(slug, contentTypeId);
       
  if (content == null)
   throw new NotFoundException($"Content with slug '{slug}' not found");

 return MapToDetailDto(content);
        }

 public async Task<ContentDetailDto> CreateAsync(CreateContentDto dto, int? userId)
        {
   // Validate content type exists
   var contentType = await _contentTypeRepository.GetByIdAsync(dto.ContentTypeId);
 if (contentType == null)
    throw new NotFoundException($"Content type with id {dto.ContentTypeId} not found");

            // Validate slug uniqueness
  if (await _contentRepository.SlugExistsAsync(dto.Slug, dto.ContentTypeId))
         throw new BadRequestException($"Content with slug '{dto.Slug}' already exists");

   var content = new Content
 {
      ContentTypeId = dto.ContentTypeId,
    Title = dto.Title,
   Slug = dto.Slug.ToSlug(),
   Summary = dto.Summary,
    Body = dto.Body,
     FeaturedImage = dto.FeaturedImage,
     Status = dto.Status,
        ScheduledAt = dto.ScheduledAt,
         IsFeatured = dto.IsFeatured,
     AllowComments = dto.AllowComments,
       AuthorId = userId,
       MetaTitle = dto.MetaTitle,
    MetaDescription = dto.MetaDescription,
                MetaKeywords = dto.MetaKeywords,
      OgTitle = dto.OgTitle,
    OgDescription = dto.OgDescription,
       OgImage = dto.OgImage
   };

 // Set published date if status is Published
    if (content.Status == ContentStatus.Published)
     {
       content.PublishedAt = DateTime.UtcNow;
  }

     await _contentRepository.CreateAsync(content);

   // Handle tags
      if (dto.TagIds != null && dto.TagIds.Any())
     {
        await AssignTagsAsync(content.Id, dto.TagIds);
   }

  // Handle custom fields
      if (dto.CustomFields != null && dto.CustomFields.Any())
       {
      await SaveCustomFieldsAsync(content.Id, dto.CustomFields);
            }

      await _auditLogService.LogAsync(
    userId: userId,
       action: "Create",
     entityType: "Content",
     entityId: content.Id,
       description: $"Created content: {content.Title}"
   );

   return await GetByIdAsync(content.Id);
  }

public async Task<ContentDetailDto> UpdateAsync(int id, UpdateContentDto dto, int? userId)
   {
     var content = await _contentRepository.GetByIdWithDetailsAsync(id);
            
      if (content == null)
   throw new NotFoundException($"Content with id {id} not found");

   // Validate slug uniqueness
            if (await _contentRepository.SlugExistsAsync(dto.Slug, dto.ContentTypeId, id))
     throw new BadRequestException($"Content with slug '{dto.Slug}' already exists");

      var oldStatus = content.Status;

     content.Title = dto.Title;
       content.Slug = dto.Slug.ToSlug();
   content.Summary = dto.Summary;
    content.Body = dto.Body;
      content.FeaturedImage = dto.FeaturedImage;
   content.Status = dto.Status;
       content.ScheduledAt = dto.ScheduledAt;
 content.IsFeatured = dto.IsFeatured;
 content.AllowComments = dto.AllowComments;
    content.MetaTitle = dto.MetaTitle;
   content.MetaDescription = dto.MetaDescription;
      content.MetaKeywords = dto.MetaKeywords;
   content.OgTitle = dto.OgTitle;
  content.OgDescription = dto.OgDescription;
       content.OgImage = dto.OgImage;

   // Update published date if status changed to Published
      if (oldStatus != ContentStatus.Published && content.Status == ContentStatus.Published)
   {
     content.PublishedAt = DateTime.UtcNow;
            }

      await _contentRepository.UpdateAsync(content);

// Update tags
      if (dto.TagIds != null)
    {
    await UpdateTagsAsync(content.Id, dto.TagIds);
     }

   // Update custom fields
            if (dto.CustomFields != null)
     {
     await UpdateCustomFieldsAsync(content.Id, dto.CustomFields);
    }

      await _auditLogService.LogAsync(
       userId: userId,
   action: "Update",
     entityType: "Content",
     entityId: content.Id,
       description: $"Updated content: {content.Title}"
  );

     return await GetByIdAsync(content.Id);
        }

        public async Task DeleteAsync(int id)
 {
      var content = await _contentRepository.GetByIdAsync(id);
   
  if (content == null)
     throw new NotFoundException($"Content with id {id} not found");

            // Decrement tag usage counts
            foreach (var contentTag in content.ContentTags)
   {
    await _tagRepository.DecrementUsageCountAsync(contentTag.TagId);
    }

await _contentRepository.DeleteAsync(id);

        await _auditLogService.LogAsync(
     userId: null,
       action: "Delete",
       entityType: "Content",
     entityId: id,
     description: $"Deleted content: {content.Title}"
    );
        }

        public async Task<ContentDetailDto> DuplicateAsync(int id, int? userId)
 {
      var original = await _contentRepository.GetByIdWithDetailsAsync(id);
  
   if (original == null)
          throw new NotFoundException($"Content with id {id} not found");

   var duplicate = new Content
   {
     ContentTypeId = original.ContentTypeId,
      Title = $"{original.Title} (Copy)",
     Slug = $"{original.Slug}-copy-{Guid.NewGuid().ToString("N").Substring(0, 8)}",
     Summary = original.Summary,
      Body = original.Body,
       FeaturedImage = original.FeaturedImage,
      Status = ContentStatus.Draft,
    IsFeatured = false,
      AllowComments = original.AllowComments,
    AuthorId = userId,
     MetaTitle = original.MetaTitle,
      MetaDescription = original.MetaDescription,
   MetaKeywords = original.MetaKeywords,
   OgTitle = original.OgTitle,
       OgDescription = original.OgDescription,
    OgImage = original.OgImage
     };

   await _contentRepository.CreateAsync(duplicate);

      // Duplicate tags
            var tagIds = original.ContentTags.Select(ct => ct.TagId).ToList();
 if (tagIds.Any())
       {
      await AssignTagsAsync(duplicate.Id, tagIds);
   }

     // Duplicate custom fields
     if (original.FieldValues.Any())
    {
      var customFields = original.FieldValues
      .ToDictionary(
   fv => fv.FieldKey, 
          fv => (object)(fv.Value ?? string.Empty)
    );
   await SaveCustomFieldsAsync(duplicate.Id, customFields);
        }

     await _auditLogService.LogAsync(
   userId: userId,
    action: "Duplicate",
       entityType: "Content",
    entityId: duplicate.Id,
   description: $"Duplicated content from: {original.Title}"
   );

  return await GetByIdAsync(duplicate.Id);
        }

   public async Task PublishAsync(int id)
   {
            var content = await _contentRepository.GetByIdAsync(id);
  
       if (content == null)
    throw new NotFoundException($"Content with id {id} not found");

        content.Status = ContentStatus.Published;
 content.PublishedAt = DateTime.UtcNow;
       
            await _contentRepository.UpdateAsync(content);

     await _auditLogService.LogAsync(
    userId: null,
action: "Publish",
    entityType: "Content",
    entityId: id,
     description: $"Published content: {content.Title}"
    );
        }

public async Task UnpublishAsync(int id)
 {
       var content = await _contentRepository.GetByIdAsync(id);
  
     if (content == null)
       throw new NotFoundException($"Content with id {id} not found");

   content.Status = ContentStatus.Draft;
       
      await _contentRepository.UpdateAsync(content);

   await _auditLogService.LogAsync(
      userId: null,
    action: "Unpublish",
      entityType: "Content",
       entityId: id,
      description: $"Unpublished content: {content.Title}"
     );
  }

        public async Task ArchiveAsync(int id)
 {
     var content = await _contentRepository.GetByIdAsync(id);
      
      if (content == null)
throw new NotFoundException($"Content with id {id} not found");

      content.Status = ContentStatus.Archived;
    
   await _contentRepository.UpdateAsync(content);

     await _auditLogService.LogAsync(
    userId: null,
action: "Archive",
    entityType: "Content",
       entityId: id,
     description: $"Archived content: {content.Title}"
            );
        }

public async Task IncrementViewCountAsync(int id)
{
            await _contentRepository.IncrementViewCountAsync(id);
   }

  public async Task ProcessScheduledContentAsync()
    {
   var scheduledContents = await _contentRepository.GetScheduledContentAsync();
    
   foreach (var content in scheduledContents)
   {
  content.Status = ContentStatus.Published;
         content.PublishedAt = DateTime.UtcNow;
await _contentRepository.UpdateAsync(content);

   await _auditLogService.LogAsync(
   userId: null,
          action: "AutoPublish",
     entityType: "Content",
       entityId: content.Id,
       description: $"Auto-published scheduled content: {content.Title}"
     );
    }
  }

    public async Task<IEnumerable<ContentListDto>> GetFeaturedAsync(int contentTypeId, int count)
        {
     var contents = await _contentRepository.GetFeaturedAsync(contentTypeId, count);
   return contents.Select(c => MapToListDto(c));
  }

        public async Task<IEnumerable<ContentListDto>> GetRecentAsync(int contentTypeId, int count)
        {
     var contents = await _contentRepository.GetRecentAsync(contentTypeId, count);
      return contents.Select(c => MapToListDto(c));
        }

        public async Task<IEnumerable<ContentListDto>> GetByTagAsync(int tagId, int pageSize, int page)
   {
  var contents = await _contentRepository.GetByTagAsync(tagId, pageSize, page);
return contents.Select(c => MapToListDto(c));
        }

        // Private helper methods
 private async Task AssignTagsAsync(int contentId, List<int> tagIds)
 {
   foreach (var tagId in tagIds)
      {
    var contentTag = new ContentTag
    {
        ContentId = contentId,
    TagId = tagId
    };
      
     _context.ContentTags.Add(contentTag);
     await _tagRepository.IncrementUsageCountAsync(tagId);
 }
  
   await _context.SaveChangesAsync();
        }

        private async Task UpdateTagsAsync(int contentId, List<int> tagIds)
 {
   // Remove existing tags
  var existingTags = _context.ContentTags.Where(ct => ct.ContentId == contentId);
     foreach (var tag in existingTags)
{
   await _tagRepository.DecrementUsageCountAsync(tag.TagId);
            }
     _context.ContentTags.RemoveRange(existingTags);
   await _context.SaveChangesAsync();

     // Add new tags
      await AssignTagsAsync(contentId, tagIds);
 }

        private async Task SaveCustomFieldsAsync(int contentId, Dictionary<string, object> customFields)
        {
    foreach (var field in customFields)
       {
var fieldValue = new ContentFieldValue
   {
     ContentId = contentId,
     FieldKey = field.Key,
   Value = JsonSerializer.Serialize(field.Value)
     };
       
_context.ContentFieldValues.Add(fieldValue);
   }
   
         await _context.SaveChangesAsync();
        }

        private async Task UpdateCustomFieldsAsync(int contentId, Dictionary<string, object> customFields)
 {
   // Remove existing field values
   var existingValues = _context.ContentFieldValues.Where(fv => fv.ContentId == contentId);
   _context.ContentFieldValues.RemoveRange(existingValues);
  await _context.SaveChangesAsync();

       // Add new field values
  await SaveCustomFieldsAsync(contentId, customFields);
        }

private ContentListDto MapToListDto(Content content)
  {
   return new ContentListDto
            {
    Id = content.Id,
  Title = content.Title,
    Slug = content.Slug,
     Summary = content.Summary,
      FeaturedImage = content.FeaturedImage,
       Status = content.Status,
     PublishedAt = content.PublishedAt,
     ViewCount = content.ViewCount,
  IsFeatured = content.IsFeatured,
    ContentTypeName = content.ContentType?.Name ?? string.Empty,
      ContentTypeId = content.ContentTypeId,
        AuthorName = content.Author != null ? $"{content.Author.FirstName} {content.Author.LastName}".Trim() : null,
     Tags = content.ContentTags?.Select(ct => new TagDto
     {
       Id = ct.Tag.Id,
       Name = ct.Tag.Name,
       Slug = ct.Tag.Slug,
     Description = ct.Tag.Description,
        Color = ct.Tag.Color,
     UsageCount = ct.Tag.UsageCount
     }).ToList() ?? new List<TagDto>(),
   CreatedAt = content.CreatedAt,
   UpdatedAt = content.UpdatedAt
     };
        }

        private ContentDetailDto MapToDetailDto(Content content)
 {
       var customFields = content.FieldValues
   .ToDictionary(
     fv => fv.FieldKey,
     fv => JsonSerializer.Deserialize<object>(fv.Value ?? "{}") ?? new object()
    );

   return new ContentDetailDto
            {
 Id = content.Id,
       Title = content.Title,
   Slug = content.Slug,
      Summary = content.Summary,
   Body = content.Body,
      FeaturedImage = content.FeaturedImage,
    Status = content.Status,
        PublishedAt = content.PublishedAt,
     ViewCount = content.ViewCount,
    IsFeatured = content.IsFeatured,
    ContentTypeName = content.ContentType?.Name ?? string.Empty,
   ContentTypeId = content.ContentTypeId,
     AuthorName = content.Author != null ? $"{content.Author.FirstName} {content.Author.LastName}".Trim() : null,
     MetaTitle = content.MetaTitle,
      MetaDescription = content.MetaDescription,
    MetaKeywords = content.MetaKeywords,
    OgTitle = content.OgTitle,
   OgDescription = content.OgDescription,
    OgImage = content.OgImage,
   CustomFields = customFields,
    ScheduledAt = content.ScheduledAt,
     AllowComments = content.AllowComments,
   Tags = content.ContentTags?.Select(ct => new TagDto
{
  Id = ct.Tag.Id,
      Name = ct.Tag.Name,
    Slug = ct.Tag.Slug,
      Description = ct.Tag.Description,
      Color = ct.Tag.Color,
       UsageCount = ct.Tag.UsageCount
   }).ToList() ?? new List<TagDto>(),
    CreatedAt = content.CreatedAt,
   UpdatedAt = content.UpdatedAt
     };
  }
    }
}
