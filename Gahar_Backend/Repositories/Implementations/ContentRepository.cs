using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Models.DTOs.Content;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations
{
    public class ContentRepository : GenericRepository<Content>, IContentRepository
    {
    public ContentRepository(ApplicationDbContext context) : base(context)
 {
        }

        public async Task<IEnumerable<Content>> GetAllAsync(ContentFilterDto filter)
        {
            var query = BuildFilterQuery(filter);
   
      // Sorting
            query = ApplySorting(query, filter.SortBy, filter.SortOrder);
            
          // Pagination
       query = query
   .Skip((filter.Page - 1) * filter.PageSize)
        .Take(filter.PageSize);

       return await query.ToListAsync();
        }

      public async Task<int> GetTotalCountAsync(ContentFilterDto filter)
        {
   var query = BuildFilterQuery(filter);
        return await query.CountAsync();
     }

        public async Task<Content?> GetByIdWithDetailsAsync(int id)
        {
     return await _dbSet
     .Include(c => c.ContentType)
     .Include(c => c.Author)
    .Include(c => c.FieldValues)
.ThenInclude(fv => fv.ContentTypeField)
       .Include(c => c.ContentTags)
     .ThenInclude(ct => ct.Tag)
        .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
 }

        public async Task<Content?> GetBySlugAsync(string slug, int contentTypeId)
        {
  return await _dbSet
   .Include(c => c.ContentType)
       .Include(c => c.Author)
   .Include(c => c.FieldValues)
   .ThenInclude(fv => fv.ContentTypeField)
       .Include(c => c.ContentTags)
            .ThenInclude(ct => ct.Tag)
       .FirstOrDefaultAsync(c => c.Slug == slug && c.ContentTypeId == contentTypeId && !c.IsDeleted);
        }

     public async Task<bool> SlugExistsAsync(string slug, int contentTypeId, int? excludeId = null)
        {
 var query = _dbSet.Where(c => c.Slug == slug && c.ContentTypeId == contentTypeId && !c.IsDeleted);
    
       if (excludeId.HasValue)
            {
   query = query.Where(c => c.Id != excludeId.Value);
            }

          return await query.AnyAsync();
    }

        public async Task<IEnumerable<Content>> GetFeaturedAsync(int contentTypeId, int count)
        {
      return await _dbSet
    .Include(c => c.ContentType)
           .Include(c => c.ContentTags)
            .ThenInclude(ct => ct.Tag)
         .Where(c => c.ContentTypeId == contentTypeId 
   && c.IsFeatured 
        && c.Status == ContentStatus.Published
  && !c.IsDeleted)
          .OrderByDescending(c => c.PublishedAt)
  .Take(count)
        .ToListAsync();
        }

   public async Task<IEnumerable<Content>> GetRecentAsync(int contentTypeId, int count)
   {
          return await _dbSet
        .Include(c => c.ContentType)
          .Include(c => c.ContentTags)
           .ThenInclude(ct => ct.Tag)
       .Where(c => c.ContentTypeId == contentTypeId 
 && c.Status == ContentStatus.Published
             && !c.IsDeleted)
     .OrderByDescending(c => c.PublishedAt)
           .Take(count)
                .ToListAsync();
    }

   public async Task<IEnumerable<Content>> GetByTagAsync(int tagId, int pageSize, int page)
        {
   return await _dbSet
   .Include(c => c.ContentType)
   .Include(c => c.ContentTags)
     .ThenInclude(ct => ct.Tag)
             .Where(c => c.ContentTags.Any(ct => ct.TagId == tagId)
      && c.Status == ContentStatus.Published
  && !c.IsDeleted)
       .OrderByDescending(c => c.PublishedAt)
                .Skip((page - 1) * pageSize)
             .Take(pageSize)
                .ToListAsync();
        }

public async Task IncrementViewCountAsync(int contentId)
        {
            var content = await _dbSet.FindAsync(contentId);
            if (content != null)
         {
                content.ViewCount++;
    await _context.SaveChangesAsync();
    }
        }

        public async Task<IEnumerable<Content>> GetScheduledContentAsync()
        {
      var now = DateTime.UtcNow;
  return await _dbSet
        .Where(c => c.Status == ContentStatus.Scheduled 
              && c.ScheduledAt <= now
       && !c.IsDeleted)
  .ToListAsync();
        }

        private IQueryable<Content> BuildFilterQuery(ContentFilterDto filter)
 {
            var query = _dbSet
                .Include(c => c.ContentType)
      .Include(c => c.Author)
           .Include(c => c.ContentTags)
            .ThenInclude(ct => ct.Tag)
   .Where(c => !c.IsDeleted)
       .AsQueryable();

        // Filter by content type
  if (filter.ContentTypeId.HasValue)
            {
    query = query.Where(c => c.ContentTypeId == filter.ContentTypeId.Value);
            }

          // Filter by status
            if (!string.IsNullOrEmpty(filter.Status))
 {
            query = query.Where(c => c.Status == filter.Status);
        }

            // Filter by featured
      if (filter.IsFeatured.HasValue)
  {
            query = query.Where(c => c.IsFeatured == filter.IsFeatured.Value);
            }

     // Filter by author
    if (filter.AuthorId.HasValue)
            {
     query = query.Where(c => c.AuthorId == filter.AuthorId.Value);
 }

// Filter by tags
      if (filter.TagIds != null && filter.TagIds.Any())
   {
       query = query.Where(c => c.ContentTags.Any(ct => filter.TagIds.Contains(ct.TagId)));
            }

          // Filter by date range
    if (filter.FromDate.HasValue)
    {
         query = query.Where(c => c.PublishedAt >= filter.FromDate.Value);
            }

        if (filter.ToDate.HasValue)
{
          query = query.Where(c => c.PublishedAt <= filter.ToDate.Value);
     }

    // Search
            if (!string.IsNullOrEmpty(filter.SearchTerm))
  {
                var searchTerm = filter.SearchTerm.ToLower();
      query = query.Where(c => 
    c.Title.ToLower().Contains(searchTerm) ||
        (c.Summary != null && c.Summary.ToLower().Contains(searchTerm)) ||
         (c.Body != null && c.Body.ToLower().Contains(searchTerm)));
            }

          return query;
     }

   private IQueryable<Content> ApplySorting(IQueryable<Content> query, string? sortBy, string? sortOrder)
        {
         var isDescending = sortOrder?.ToLower() == "desc";

            return sortBy?.ToLower() switch
            {
       "title" => isDescending ? query.OrderByDescending(c => c.Title) : query.OrderBy(c => c.Title),
    "publishedat" => isDescending ? query.OrderByDescending(c => c.PublishedAt) : query.OrderBy(c => c.PublishedAt),
           "viewcount" => isDescending ? query.OrderByDescending(c => c.ViewCount) : query.OrderBy(c => c.ViewCount),
    "updatedat" => isDescending ? query.OrderByDescending(c => c.UpdatedAt) : query.OrderBy(c => c.UpdatedAt),
            _ => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt)
   };
        }
    }
}
