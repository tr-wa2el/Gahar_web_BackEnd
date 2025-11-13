using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class MediaRepository : GenericRepository<Media>, IMediaRepository
{
    public MediaRepository(ApplicationDbContext context) : base(context) { }

   public async Task<IEnumerable<Media>> GetByMediaTypeAsync(string mediaType)
    {
   return await _context.Media
         .Where(m => m.MediaType == mediaType)
     .OrderByDescending(m => m.CreatedAt)
   .ToListAsync();
    }

    public async Task<IEnumerable<Media>> GetByUploaderAsync(int uploaderId)
    {
   return await _context.Media
          .Where(m => m.UploadedBy == uploaderId)
        .OrderByDescending(m => m.CreatedAt)
     .ToListAsync();
    }

    public async Task<IEnumerable<Media>> SearchAsync(string searchTerm)
    {
    return await _context.Media
          .Where(m => m.FileName.Contains(searchTerm) || m.Alt!.Contains(searchTerm) || m.Caption!.Contains(searchTerm))
        .OrderByDescending(m => m.CreatedAt)
.ToListAsync();
   }

 public IQueryable<Media> GetQueryable() => _context.Media.AsQueryable();
    public async Task AddAsync(Media entity) => await _context.Media.AddAsync(entity);
    public void Update(Media entity) => _context.Media.Update(entity);
   public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}

public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
{
    public AlbumRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Album?> GetBySlugAsync(string slug)
    {
     return await _context.Albums
       .Include(a => a.CoverImage)
       .Include(a => a.Creator)
   .Include(a => a.AlbumMedias).ThenInclude(am => am.Media)
         .FirstOrDefaultAsync(a => a.Slug == slug);
   }

  public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
   {
     var query = _context.Albums.Where(a => a.Slug == slug);
       if (excludeId.HasValue)
          query = query.Where(a => a.Id != excludeId.Value);
      return await query.AnyAsync();
  }

  public async Task<IEnumerable<Album>> GetPublishedAsync()
 {
  return await _context.Albums
          .Where(a => a.IsPublished && a.PublishedAt <= DateTime.UtcNow)
            .OrderByDescending(a => a.PublishedAt)
         .Include(a => a.CoverImage)
    .Include(a => a.Creator)
       .ToListAsync();
    }

  public async Task<IEnumerable<Album>> GetByCreatorAsync(int creatorId)
    {
      return await _context.Albums
          .Where(a => a.CreatedBy == creatorId)
          .OrderByDescending(a => a.CreatedAt)
       .Include(a => a.CoverImage)
     .Include(a => a.AlbumMedias)
        .ToListAsync();
   }

    public IQueryable<Album> GetQueryable() => _context.Albums.AsQueryable();
    public async Task AddAsync(Album entity) => await _context.Albums.AddAsync(entity);
  public void Update(Album entity) => _context.Albums.Update(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}

public class AlbumMediaRepository : GenericRepository<AlbumMedia>, IAlbumMediaRepository
{
  public AlbumMediaRepository(ApplicationDbContext context) : base(context) { }

  public async Task<IEnumerable<AlbumMedia>> GetByAlbumIdAsync(int albumId)
    {
 return await _context.AlbumMedias
           .Where(am => am.AlbumId == albumId)
    .OrderBy(am => am.DisplayOrder)
    .Include(am => am.Media)
        .ToListAsync();
    }

 public async Task<IEnumerable<AlbumMedia>> GetFeaturedByAlbumAsync(int albumId, int count = 5)
    {
    return await _context.AlbumMedias
          .Where(am => am.AlbumId == albumId && am.IsFeatured)
   .OrderBy(am => am.DisplayOrder)
      .Take(count)
  .Include(am => am.Media)
       .ToListAsync();
 }

    public IQueryable<AlbumMedia> GetQueryable() => _context.AlbumMedias.AsQueryable();
    public async Task AddAsync(AlbumMedia entity) => await _context.AlbumMedias.AddAsync(entity);
    public void Update(AlbumMedia entity) => _context.AlbumMedias.Update(entity);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
