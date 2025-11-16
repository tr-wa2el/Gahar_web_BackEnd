using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for Album operations
    /// </summary>
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ApplicationDbContext _context;

   public AlbumRepository(ApplicationDbContext context)
        {
     _context = context;
    }

    public async Task<(IEnumerable<Album> Items, int TotalCount)> GetAllAsync(
 bool? isPublished = null,
        string? searchTerm = null,
  int page = 1,
    int pageSize = 20,
  string sortBy = "createdAt",
      string sortOrder = "desc")
        {
var query = _context.Albums.AsQueryable();

     // Filter by published status
    if (isPublished.HasValue)
{
      query = query.Where(a => a.IsPublished == isPublished.Value);
 }

         // Search
 if (!string.IsNullOrWhiteSpace(searchTerm))
     {
        query = query.Where(a =>
     a.Title.Contains(searchTerm) ||
        a.Description != null && a.Description.Contains(searchTerm));
            }

         // Sorting
    query = sortOrder.ToLower() == "asc"
        ? sortBy.ToLower() switch
{
      "title" => query.OrderBy(a => a.Title),
         "mediacount" => query.OrderBy(a => a.AlbumMedias.Count),
              "viewcount" => query.OrderBy(a => a.ViewCount),
  _ => query.OrderBy(a => a.CreatedAt)
     }
          : sortBy.ToLower() switch
     {
 "title" => query.OrderByDescending(a => a.Title),
            "mediacount" => query.OrderByDescending(a => a.AlbumMedias.Count),
       "viewcount" => query.OrderByDescending(a => a.ViewCount),
  _ => query.OrderByDescending(a => a.CreatedAt)
       };

        var totalCount = await query.CountAsync();

  var items = await query
          .Include(a => a.CoverImage)
   .Include(a => a.AlbumMedias)
  .ThenInclude(am => am.Media)
.Skip((page - 1) * pageSize)
  .Take(pageSize)
  .AsNoTracking()
         .ToListAsync();

       return (items, totalCount);
      }

     public async Task<Album?> GetByIdAsync(int id)
   {
 return await _context.Albums
 .Include(a => a.CoverImage)
       .Include(a => a.Creator)
       .Include(a => a.AlbumMedias)
  .ThenInclude(am => am.Media)
 .FirstOrDefaultAsync(a => a.Id == id);
        }

     public async Task<Album?> GetBySlugAsync(string slug)
     {
        return await _context.Albums
     .Include(a => a.CoverImage)
     .Include(a => a.AlbumMedias)
    .ThenInclude(am => am.Media)
   .FirstOrDefaultAsync(a => a.Slug == slug && a.IsPublished);
        }

        public async Task<Album> CreateAsync(Album album)
      {
     await _context.Albums.AddAsync(album);
   await _context.SaveChangesAsync();
     return album;
}

    public async Task<Album> UpdateAsync(Album album)
       {
      _context.Albums.Update(album);
       await _context.SaveChangesAsync();
  return album;
        }

    public async Task<bool> DeleteAsync(int id)
      {
   var album = await _context.Albums.FindAsync(id);
         if (album == null)
      return false;

   _context.Albums.Remove(album);
   await _context.SaveChangesAsync();
    return true;
     }

  public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
   {
        var query = _context.Albums.Where(a => a.Slug == slug);

       if (excludeId.HasValue)
      {
    query = query.Where(a => a.Id != excludeId.Value);
    }

    return await query.AnyAsync();
    }

     public async Task<List<AlbumMedia>> GetAlbumMediaAsync(int albumId)
      {
   return await _context.AlbumMedias
     .Where(am => am.AlbumId == albumId)
   .Include(am => am.Media)
    .OrderBy(am => am.DisplayOrder)
    .AsNoTracking()
    .ToListAsync();
        }

      public async Task AddMediaToAlbumAsync(int albumId, int mediaId, string? caption = null)
        {
    // Check if media already exists in album
      var exists = await _context.AlbumMedias
   .AnyAsync(am => am.AlbumId == albumId && am.MediaId == mediaId);

         if (exists)
            throw new InvalidOperationException("Media already exists in this album");

     // Get max display order
         var maxOrder = await _context.AlbumMedias
     .Where(am => am.AlbumId == albumId)
  .MaxAsync(am => (int?)am.DisplayOrder) ?? 0;

 var albumMedia = new AlbumMedia
    {
AlbumId = albumId,
             MediaId = mediaId,
    DisplayOrder = maxOrder + 1,
   Caption = caption
          };

        await _context.AlbumMedias.AddAsync(albumMedia);
     await _context.SaveChangesAsync();
        }

 public async Task<bool> RemoveMediaFromAlbumAsync(int albumId, int mediaId)
        {
    var albumMedia = await _context.AlbumMedias
     .FirstOrDefaultAsync(am => am.AlbumId == albumId && am.MediaId == mediaId);

  if (albumMedia == null)
           return false;

     _context.AlbumMedias.Remove(albumMedia);
    await _context.SaveChangesAsync();

    // Reset display orders
       var remainingMedia = await _context.AlbumMedias
       .Where(am => am.AlbumId == albumId)
        .OrderBy(am => am.DisplayOrder)
        .ToListAsync();

     for (int i = 0; i < remainingMedia.Count; i++)
    {
      remainingMedia[i].DisplayOrder = i;
  }

           await _context.SaveChangesAsync();

    return true;
        }

 public async Task ReorderAlbumMediaAsync(int albumId, List<int> mediaIds)
     {
         for (int i = 0; i < mediaIds.Count; i++)
  {
     var albumMedia = await _context.AlbumMedias
     .FirstOrDefaultAsync(am => am.AlbumId == albumId && am.MediaId == mediaIds[i]);

       if (albumMedia != null)
              {
          albumMedia.DisplayOrder = i;
       }
    }

        await _context.SaveChangesAsync();
     }

       public async Task IncrementViewCountAsync(int albumId)
       {
  var album = await _context.Albums.FindAsync(albumId);
       if (album != null)
   {
       album.ViewCount++;
               await _context.SaveChangesAsync();
     }
       }

    public async Task<IEnumerable<Album>> GetPublishedAsync(int top = 10)
     {
        return await _context.Albums
 .Where(a => a.IsPublished)
  .Include(a => a.CoverImage)
      .OrderByDescending(a => a.ViewCount)
      .Take(top)
       .AsNoTracking()
     .ToListAsync();
  }

  public async Task<IEnumerable<Album>> GetByUserAsync(int userId)
{
  return await _context.Albums
 .Where(a => a.CreatedBy == userId)
      .Include(a => a.CoverImage)
           .OrderByDescending(a => a.CreatedAt)
       .AsNoTracking()
      .ToListAsync();
        }

       public async Task<int> GetMediaCountAsync(int albumId)
       {
  return await _context.AlbumMedias
 .CountAsync(am => am.AlbumId == albumId);
   }
    }
}
