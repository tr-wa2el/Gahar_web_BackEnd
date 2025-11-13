using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Models.DTOs.Content;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Implementations;

public class ContentService : IContentService
{
    private readonly IContentRepository _contentRepo;
    private readonly IContentTypeRepository _contentTypeRepo;
  private readonly IContentFieldValueRepository _fieldValueRepo;
    private readonly IContentTagRepository _contentTagRepo;
    private readonly ITagRepository _tagRepo;
 private readonly IAuditLogService _auditLogService;

    public ContentService(
 IContentRepository contentRepo,
        IContentTypeRepository contentTypeRepo,
      IContentFieldValueRepository fieldValueRepo,
        IContentTagRepository contentTagRepo,
        ITagRepository tagRepo,
        IAuditLogService auditLogService)
    {
  _contentRepo = contentRepo;
        _contentTypeRepo = contentTypeRepo;
  _fieldValueRepo = fieldValueRepo;
      _contentTagRepo = contentTagRepo;
 _tagRepo = tagRepo;
        _auditLogService = auditLogService;
    }

    #region Content Management

    public async Task<PagedResult<ContentListDto>> GetAllAsync(ContentFilterDto filter)
    {
        var query = _contentRepo.GetQueryable();

        // Apply filters
 if (filter.ContentTypeId.HasValue)
            query = query.Where(c => c.ContentTypeId == filter.ContentTypeId.Value);

     if (!string.IsNullOrEmpty(filter.Status))
   query = query.Where(c => c.Status == filter.Status);

        if (filter.IsFeatured.HasValue)
    query = query.Where(c => c.IsFeatured == filter.IsFeatured.Value);

        if (filter.AuthorId.HasValue)
       query = query.Where(c => c.AuthorId == filter.AuthorId.Value);

     if (!string.IsNullOrEmpty(filter.SearchTerm))
            query = query.Where(c => c.Title.Contains(filter.SearchTerm) || c.Summary!.Contains(filter.SearchTerm));

  if (filter.FromDate.HasValue)
            query = query.Where(c => c.CreatedAt >= filter.FromDate.Value);

   if (filter.ToDate.HasValue)
         query = query.Where(c => c.CreatedAt <= filter.ToDate.Value);

        // Get total count
        var totalCount = await query.CountAsync();

  // Apply sorting
        query = filter.SortOrder?.ToLower() == "asc"
           ? query.OrderBy(c => EF.Property<object>(c, filter.SortBy ?? "CreatedAt"))
            : query.OrderByDescending(c => EF.Property<object>(c, filter.SortBy ?? "CreatedAt"));

   // Apply pagination
        var items = await query
     .Skip((filter.Page - 1) * filter.PageSize)
    .Take(filter.PageSize)
         .Include(c => c.ContentType)
           .Include(c => c.Author)
    .Include(c => c.Tags).ThenInclude(ct => ct.Tag)
           .ToListAsync();

  return new PagedResult<ContentListDto>
        {
        Items = items.Select(MapToListDto).ToList(),
    TotalCount = totalCount,
 Page = filter.Page,
         PageSize = filter.PageSize
        };
    }

public async Task<ContentDetailDto> GetByIdAsync(int id, string? language = null)
    {
       var content = await _contentRepo.GetByIdAsync(id);
 if (content == null)
          throw new NotFoundException($"محتوى برقم {id} غير موجود");

 await _contentRepo.GetQueryable()
         .Where(c => c.Id == id)
        .Include(c => c.ContentType)
   .Include(c => c.Author)
      .Include(c => c.Layout)
     .Include(c => c.FieldValues)
      .Include(c => c.Tags).ThenInclude(ct => ct.Tag)
         .LoadAsync();

        return MapToDetailDto(content);
    }

    public async Task<ContentDetailDto> GetBySlugAsync(string slug, string contentType, string? language = null)
    {
      var type = await _contentTypeRepo.GetBySlugAsync(contentType);
        if (type == null)
       throw new NotFoundException($"نوع المحتوى '{contentType}' غير موجود");

  var content = await _contentRepo.GetBySlugAsync(slug, type.Id);
      if (content == null)
throw new NotFoundException($"محتوى برقم '{slug}' غير موجود");

        return MapToDetailDto(content);
 }

    public async Task<ContentDto> CreateAsync(CreateContentDto dto, int userId)
    {
     // Validate content type
    var contentType = await _contentTypeRepo.GetByIdAsync(dto.ContentTypeId);
  if (contentType == null)
            throw new NotFoundException("نوع المحتوى غير موجود");

      // Validate slug
      if (await _contentRepo.SlugExistsAsync(dto.Slug, dto.ContentTypeId))
throw new BadRequestException($"الكود '{dto.Slug}' موجود بالفعل");

    var content = new Content
        {
  ContentTypeId = dto.ContentTypeId,
   Title = dto.Title,
            Slug = dto.Slug,
           Summary = dto.Summary,
         Body = dto.Body,
  FeaturedImage = dto.FeaturedImage,
           LayoutId = dto.LayoutId,
    MetaTitle = dto.MetaTitle,
 MetaDescription = dto.MetaDescription,
 MetaKeywords = dto.MetaKeywords,
          OgTitle = dto.OgTitle,
           OgDescription = dto.OgDescription,
        OgImage = dto.OgImage,
   Status = dto.Status,
        ScheduledAt = dto.ScheduledAt,
          AuthorId = userId,
         IsFeatured = dto.IsFeatured,
    AllowComments = dto.AllowComments
        };

    await _contentRepo.AddAsync(content);
 await _contentRepo.SaveChangesAsync();

      // Add field values
        if (dto.CustomFields?.Any() == true)
        {
    var fields = await _contentTypeRepo.GetWithFieldsAsync(dto.ContentTypeId);
       foreach (var (key, value) in dto.CustomFields)
            {
  var field = fields?.Fields.FirstOrDefault(f => f.FieldKey == key);
        if (field != null)
          {
     var fieldValue = new ContentFieldValue
    {
   ContentId = content.Id,
              ContentTypeFieldId = field.Id,
      FieldKey = key,
      Value = value?.ToString()
  };
       await _fieldValueRepo.AddAsync(fieldValue);
   }
  }
         await _fieldValueRepo.SaveChangesAsync();
     }

        // Add tags
     if (dto.TagIds?.Any() == true)
        {
     foreach (var tagId in dto.TagIds)
{
var contentTag = new ContentTag
     {
          ContentId = content.Id,
       TagId = tagId
    };
   await _contentTagRepo.AddAsync(contentTag);
   }
     await _contentTagRepo.SaveChangesAsync();
      }

      await _auditLogService.LogAsync(userId, "Create", "Content", content.Id, $"إنشاء محتوى: {content.Title}");

    return MapToDto(content);
    }

    public async Task<ContentDto> UpdateAsync(int id, UpdateContentDto dto)
    {
  var content = await _contentRepo.GetByIdAsync(id);
      if (content == null)
      throw new NotFoundException($"محتوى برقم {id} غير موجود");

      if (dto.Slug != content.Slug && await _contentRepo.SlugExistsAsync(dto.Slug, dto.ContentTypeId, id))
      throw new BadRequestException($"الكود '{dto.Slug}' موجود بالفعل");

    content.Title = dto.Title;
content.Slug = dto.Slug;
   content.Summary = dto.Summary;
    content.Body = dto.Body;
        content.FeaturedImage = dto.FeaturedImage;
 content.LayoutId = dto.LayoutId;
        content.MetaTitle = dto.MetaTitle;
      content.MetaDescription = dto.MetaDescription;
  content.MetaKeywords = dto.MetaKeywords;
     content.OgTitle = dto.OgTitle;
      content.OgDescription = dto.OgDescription;
 content.OgImage = dto.OgImage;
        content.Status = dto.Status;
    content.ScheduledAt = dto.ScheduledAt;
     content.IsFeatured = dto.IsFeatured;
        content.AllowComments = dto.AllowComments;

     _contentRepo.Update(content);
        await _contentRepo.SaveChangesAsync();

   return MapToDto(content);
 }

    public async Task DeleteAsync(int id)
  {
      var content = await _contentRepo.GetByIdAsync(id);
  if (content == null)
    throw new NotFoundException($"محتوى برقم {id} غير موجود");

      content.IsDeleted = true;
        _contentRepo.Update(content);
 await _contentRepo.SaveChangesAsync();
    }

    #endregion

#region Publishing

    public async Task PublishAsync(int id)
    {
        var content = await _contentRepo.GetByIdAsync(id);
    if (content == null)
  throw new NotFoundException($"محتوى برقم {id} غير موجود");

  content.Status = "Published";
        content.PublishedAt = DateTime.UtcNow;
        _contentRepo.Update(content);
   await _contentRepo.SaveChangesAsync();
 }

   public async Task UnpublishAsync(int id)
    {
        var content = await _contentRepo.GetByIdAsync(id);
     if (content == null)
    throw new NotFoundException($"محتوى برقم {id} غير موجود");

      content.Status = "Draft";
        content.PublishedAt = null;
        _contentRepo.Update(content);
  await _contentRepo.SaveChangesAsync();
    }

    public async Task ScheduleAsync(int id, DateTime publishDate)
    {
        var content = await _contentRepo.GetByIdAsync(id);
    if (content == null)
           throw new NotFoundException($"محتوى برقم {id} غير موجود");

        content.Status = "Scheduled";
        content.ScheduledAt = publishDate;
  _contentRepo.Update(content);
     await _contentRepo.SaveChangesAsync();
    }

    public async Task<IEnumerable<ContentListDto>> GetScheduledContentAsync()
    {
     var now = DateTime.UtcNow;
     var query = _contentRepo.GetQueryable()
          .Where(c => c.Status == "Scheduled" && c.ScheduledAt <= now);

      var contents = await query
           .Include(c => c.ContentType)
        .Include(c => c.Author)
      .Include(c => c.Tags).ThenInclude(ct => ct.Tag)
    .ToListAsync();

     return contents.Select(MapToListDto);
    }

    #endregion

    #region Features

    public async Task DuplicateAsync(int id, int userId)
    {
        var original = await _contentRepo.GetByIdAsync(id);
       if (original == null)
       throw new NotFoundException($"محتوى برقم {id} غير موجود");

        var duplicate = new Content
    {
       ContentTypeId = original.ContentTypeId,
 Title = $"{original.Title} (نسخة)",
  Slug = $"{original.Slug}-copy-{Guid.NewGuid().ToString().Substring(0, 8)}",
    Summary = original.Summary,
           Body = original.Body,
    FeaturedImage = original.FeaturedImage,
    LayoutId = original.LayoutId,
       MetaTitle = original.MetaTitle,
     MetaDescription = original.MetaDescription,
    MetaKeywords = original.MetaKeywords,
          OgTitle = original.OgTitle,
          OgDescription = original.OgDescription,
  OgImage = original.OgImage,
          Status = "Draft",
       AuthorId = userId,
          AllowComments = original.AllowComments
  };

     await _contentRepo.AddAsync(duplicate);
     await _contentRepo.SaveChangesAsync();
    }

    public async Task MoveToContentTypeAsync(int id, int targetContentTypeId)
    {
      var content = await _contentRepo.GetByIdAsync(id);
   if (content == null)
    throw new NotFoundException($"محتوى برقم {id} غير موجود");

       var targetType = await _contentTypeRepo.GetByIdAsync(targetContentTypeId);
if (targetType == null)
       throw new NotFoundException("نوع المحتوى المستهدف غير موجود");

        content.ContentTypeId = targetContentTypeId;
     _contentRepo.Update(content);
      await _contentRepo.SaveChangesAsync();
    }

  public async Task IncrementViewsAsync(int id)
 {
        var content = await _contentRepo.GetByIdAsync(id);
        if (content != null)
       {
    content.ViewCount++;
     _contentRepo.Update(content);
     await _contentRepo.SaveChangesAsync();
       }
    }

    #endregion

  #region Search & Filter

    public async Task<PagedResult<ContentListDto>> SearchAsync(string searchTerm, ContentFilterDto filter)
    {
     filter.SearchTerm = searchTerm;
        return await GetAllAsync(filter);
   }

    public async Task<IEnumerable<ContentListDto>> GetFeaturedAsync(int count = 10)
    {
      var contents = await _contentRepo.GetFeaturedAsync(count);
    return contents.Select(MapToListDto);
    }

   public async Task<IEnumerable<ContentListDto>> GetRecentAsync(int count = 10)
    {
   var query = _contentRepo.GetQueryable()
   .Where(c => c.Status == "Published" && c.PublishedAt <= DateTime.UtcNow)
     .OrderByDescending(c => c.PublishedAt)
      .Take(count);

var contents = await query
         .Include(c => c.ContentType)
   .Include(c => c.Author)
 .Include(c => c.Tags).ThenInclude(ct => ct.Tag)
  .ToListAsync();

   return contents.Select(MapToListDto);
  }

    #endregion

    #region Helper Methods

    private ContentListDto MapToListDto(Content c)
    {
  return new ContentListDto
        {
  Id = c.Id,
    Title = c.Title,
    Slug = c.Slug,
 Summary = c.Summary,
    FeaturedImage = c.FeaturedImage,
  Status = c.Status,
   PublishedAt = c.PublishedAt,
 ViewCount = c.ViewCount,
    IsFeatured = c.IsFeatured,
      ContentTypeName = c.ContentType?.Name ?? string.Empty,
    AuthorName = c.Author?.FirstName,
   Tags = c.Tags?.Select(ct => new TagDto
{
      Id = ct.Tag.Id,
 Name = ct.Tag.Name,
    Slug = ct.Tag.Slug
      }).ToList() ?? new(),
        CreatedAt = c.CreatedAt
        };
    }

    private ContentDetailDto MapToDetailDto(Content c)
    {
        var customFields = c.FieldValues?
        .ToDictionary(
     fv => fv.FieldKey,
     fv => (object)(fv.Value ?? string.Empty))
            ?? new Dictionary<string, object>();

   return new ContentDetailDto
      {
     Id = c.Id,
 Title = c.Title,
    Slug = c.Slug,
        Summary = c.Summary,
          Body = c.Body,
   FeaturedImage = c.FeaturedImage,
   Status = c.Status,
  PublishedAt = c.PublishedAt,
   ViewCount = c.ViewCount,
    IsFeatured = c.IsFeatured,
 ContentTypeName = c.ContentType?.Name ?? string.Empty,
     AuthorName = c.Author?.FirstName,
  Tags = c.Tags?.Select(ct => new TagDto
         {
     Id = ct.Tag.Id,
  Name = ct.Tag.Name,
      Slug = ct.Tag.Slug
        }).ToList() ?? new(),
     CreatedAt = c.CreatedAt,
    MetaTitle = c.MetaTitle,
         MetaDescription = c.MetaDescription,
        MetaKeywords = c.MetaKeywords,
 CustomFields = customFields
  };
  }

  private ContentDto MapToDto(Content c)
    {
 return new ContentDto
        {
 Id = c.Id,
           ContentTypeId = c.ContentTypeId,
      Title = c.Title,
   Slug = c.Slug,
          Summary = c.Summary,
        Body = c.Body,
 FeaturedImage = c.FeaturedImage,
      LayoutId = c.LayoutId,
      Status = c.Status,
    PublishedAt = c.PublishedAt,
    ScheduledAt = c.ScheduledAt,
   AuthorId = c.AuthorId,
  ViewCount = c.ViewCount,
        IsFeatured = c.IsFeatured,
  AllowComments = c.AllowComments
 };
    }

    #endregion
}
