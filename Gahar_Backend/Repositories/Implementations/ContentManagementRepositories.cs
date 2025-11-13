using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class ContentTypeRepository : GenericRepository<ContentType>, IContentTypeRepository
{
 public ContentTypeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<ContentType?> GetBySlugAsync(string slug)
    {
    return await _context.ContentTypes
.Include(ct => ct.Fields.OrderBy(f => f.DisplayOrder))
     .FirstOrDefaultAsync(ct => ct.Slug == slug);
    }

    public async Task<ContentType?> GetWithFieldsAsync(int id)
    {
        return await _context.ContentTypes
 .Include(ct => ct.Fields.OrderBy(f => f.DisplayOrder))
      .FirstOrDefaultAsync(ct => ct.Id == id);
    }

    public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
    {
        var query = _context.ContentTypes.Where(ct => ct.Slug == slug);
 if (excludeId.HasValue)
     query = query.Where(ct => ct.Id != excludeId.Value);
        return await query.AnyAsync();
  }

    public async Task<IEnumerable<ContentType>> GetActiveAsync()
    {
        return await _context.ContentTypes
   .Where(ct => ct.IsActive)
            .OrderBy(ct => ct.DisplayOrder)
            .ToListAsync();
    }

    public IQueryable<ContentType> GetQueryable() => _context.ContentTypes.AsQueryable();
    public async Task AddAsync(ContentType entity) => await _context.ContentTypes.AddAsync(entity);
    public void Update(ContentType entity) => _context.ContentTypes.Update(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}

public class ContentTypeFieldRepository : GenericRepository<ContentTypeField>, IContentTypeFieldRepository
{
    public ContentTypeFieldRepository(ApplicationDbContext context) : base(context) { }

   public async Task<IEnumerable<ContentTypeField>> GetByContentTypeIdAsync(int contentTypeId)
    {
  return await _context.ContentTypeFields
   .Where(f => f.ContentTypeId == contentTypeId)
     .OrderBy(f => f.DisplayOrder)
            .ToListAsync();
    }

    public async Task<ContentTypeField?> GetByKeyAsync(int contentTypeId, string fieldKey)
    {
  return await _context.ContentTypeFields
            .FirstOrDefaultAsync(f => f.ContentTypeId == contentTypeId && f.FieldKey == fieldKey);
    }

    public IQueryable<ContentTypeField> GetQueryable() => _context.ContentTypeFields.AsQueryable();
    public async Task AddAsync(ContentTypeField entity) => await _context.ContentTypeFields.AddAsync(entity);
    public void Update(ContentTypeField entity) => _context.ContentTypeFields.Update(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}

public class ContentRepository : GenericRepository<Content>, IContentRepository
{
    public ContentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Content?> GetBySlugAsync(string slug, int contentTypeId)
    {
 return await _context.Contents
      .Include(c => c.FieldValues)
            .Include(c => c.Tags)
     .Include(c => c.Author)
  .FirstOrDefaultAsync(c => c.Slug == slug && c.ContentTypeId == contentTypeId);
    }

    public async Task<bool> SlugExistsAsync(string slug, int contentTypeId, int? excludeId = null)
    {
        var query = _context.Contents.Where(c => c.Slug == slug && c.ContentTypeId == contentTypeId);
        if (excludeId.HasValue)
 query = query.Where(c => c.Id != excludeId.Value);
      return await query.AnyAsync();
    }

    public async Task<IEnumerable<Content>> GetPublishedAsync(int? contentTypeId = null)
    {
        var query = _context.Contents
      .Where(c => c.Status == "Published" && c.PublishedAt <= DateTime.UtcNow);
        if (contentTypeId.HasValue)
    query = query.Where(c => c.ContentTypeId == contentTypeId);
 return await query.OrderByDescending(c => c.PublishedAt).ToListAsync();
  }

    public async Task<IEnumerable<Content>> GetFeaturedAsync(int count = 10)
    {
        return await _context.Contents
            .Where(c => c.IsFeatured && c.Status == "Published")
   .OrderByDescending(c => c.PublishedAt)
      .Take(count)
            .ToListAsync();
    }

    public IQueryable<Content> GetQueryable() => _context.Contents.AsQueryable();
    public async Task AddAsync(Content entity) => await _context.Contents.AddAsync(entity);
    public void Update(Content entity) => _context.Contents.Update(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}

public class ContentFieldValueRepository : GenericRepository<ContentFieldValue>, IContentFieldValueRepository
{
public ContentFieldValueRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<ContentFieldValue>> GetByContentIdAsync(int contentId)
    {
        return await _context.ContentFieldValues
   .Where(fv => fv.ContentId == contentId)
     .ToListAsync();
    }

    public async Task<ContentFieldValue?> GetByFieldKeyAsync(int contentId, string fieldKey)
    {
 return await _context.ContentFieldValues
     .FirstOrDefaultAsync(fv => fv.ContentId == contentId && fv.FieldKey == fieldKey);
    }

    public IQueryable<ContentFieldValue> GetQueryable() => _context.ContentFieldValues.AsQueryable();
  public async Task AddAsync(ContentFieldValue entity) => await _context.ContentFieldValues.AddAsync(entity);
    public void Update(ContentFieldValue entity) => _context.ContentFieldValues.Update(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}

public class TagRepository : GenericRepository<Tag>, ITagRepository
{
    public TagRepository(ApplicationDbContext context) : base(context) { }

 public async Task<Tag?> GetBySlugAsync(string slug)
    {
   return await _context.Tags.FirstOrDefaultAsync(t => t.Slug == slug);
    }

 public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
    {
        var query = _context.Tags.Where(t => t.Slug == slug);
        if (excludeId.HasValue)
   query = query.Where(t => t.Id != excludeId.Value);
        return await query.AnyAsync();
    }

 public async Task<IEnumerable<Tag>> SearchAsync(string searchTerm)
    {
    return await _context.Tags
            .Where(t => t.Name.Contains(searchTerm) || t.Description!.Contains(searchTerm))
  .ToListAsync();
    }

    public IQueryable<Tag> GetQueryable() => _context.Tags.AsQueryable();
 public async Task AddAsync(Tag entity) => await _context.Tags.AddAsync(entity);
   public void Update(Tag entity) => _context.Tags.Update(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}

public class ContentTagRepository : GenericRepository<ContentTag>, IContentTagRepository
{
    public ContentTagRepository(ApplicationDbContext context) : base(context) { }

  public async Task<IEnumerable<Tag>> GetTagsByContentIdAsync(int contentId)
    {
      return await _context.ContentTags
          .Where(ct => ct.ContentId == contentId)
            .Include(ct => ct.Tag)
    .Select(ct => ct.Tag)
.ToListAsync();
    }

    public async Task<IEnumerable<Content>> GetContentsByTagIdAsync(int tagId)
    {
     return await _context.ContentTags
         .Where(ct => ct.TagId == tagId)
            .Include(ct => ct.Content)
 .Select(ct => ct.Content)
     .ToListAsync();
    }

    public IQueryable<ContentTag> GetQueryable() => _context.ContentTags.AsQueryable();
    public async Task AddAsync(ContentTag entity) => await _context.ContentTags.AddAsync(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}

public class LayoutRepository : GenericRepository<Layout>, ILayoutRepository
{
    public LayoutRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Layout?> GetDefaultAsync()
    {
     return await _context.Layouts.FirstOrDefaultAsync(l => l.IsDefault && l.IsActive);
    }

    public IQueryable<Layout> GetQueryable() => _context.Layouts.AsQueryable();
    public async Task AddAsync(Layout entity) => await _context.Layouts.AddAsync(entity);
    public void Update(Layout entity) => _context.Layouts.Update(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
