using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IContentTypeRepository : IGenericRepository<ContentType>
{
    Task<ContentType?> GetBySlugAsync(string slug);
    Task<ContentType?> GetWithFieldsAsync(int id);
    Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
    Task<IEnumerable<ContentType>> GetActiveAsync();
 IQueryable<ContentType> GetQueryable();
    Task AddAsync(ContentType entity);
    void Update(ContentType entity);
    Task SaveChangesAsync();
}

public interface IContentTypeFieldRepository : IGenericRepository<ContentTypeField>
{
    Task<IEnumerable<ContentTypeField>> GetByContentTypeIdAsync(int contentTypeId);
    Task<ContentTypeField?> GetByKeyAsync(int contentTypeId, string fieldKey);
    IQueryable<ContentTypeField> GetQueryable();
    Task AddAsync(ContentTypeField entity);
    void Update(ContentTypeField entity);
    Task SaveChangesAsync();
}

public interface IContentRepository : IGenericRepository<Content>
{
    Task<Content?> GetBySlugAsync(string slug, int contentTypeId);
    Task<bool> SlugExistsAsync(string slug, int contentTypeId, int? excludeId = null);
    Task<IEnumerable<Content>> GetPublishedAsync(int? contentTypeId = null);
    Task<IEnumerable<Content>> GetFeaturedAsync(int count = 10);
    IQueryable<Content> GetQueryable();
    Task AddAsync(Content entity);
    void Update(Content entity);
    Task SaveChangesAsync();
}

public interface IContentFieldValueRepository : IGenericRepository<ContentFieldValue>
{
    Task<IEnumerable<ContentFieldValue>> GetByContentIdAsync(int contentId);
    Task<ContentFieldValue?> GetByFieldKeyAsync(int contentId, string fieldKey);
    IQueryable<ContentFieldValue> GetQueryable();
    Task AddAsync(ContentFieldValue entity);
    void Update(ContentFieldValue entity);
    Task SaveChangesAsync();
}

public interface ITagRepository : IGenericRepository<Tag>
{
 Task<Tag?> GetBySlugAsync(string slug);
    Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
    Task<IEnumerable<Tag>> SearchAsync(string searchTerm);
    IQueryable<Tag> GetQueryable();
    Task AddAsync(Tag entity);
    void Update(Tag entity);
 Task SaveChangesAsync();
}

public interface IContentTagRepository : IGenericRepository<ContentTag>
{
    Task<IEnumerable<Tag>> GetTagsByContentIdAsync(int contentId);
    Task<IEnumerable<Content>> GetContentsByTagIdAsync(int tagId);
    IQueryable<ContentTag> GetQueryable();
    Task AddAsync(ContentTag entity);
    Task SaveChangesAsync();
}

public interface ILayoutRepository : IGenericRepository<Layout>
{
    Task<Layout?> GetDefaultAsync();
    IQueryable<Layout> GetQueryable();
    Task AddAsync(Layout entity);
  void Update(Layout entity);
    Task SaveChangesAsync();
}
