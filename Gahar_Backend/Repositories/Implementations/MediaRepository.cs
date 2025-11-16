using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for Media operations
    /// </summary>
    public class MediaRepository : IMediaRepository
    {
        private readonly ApplicationDbContext _context;

     public MediaRepository(ApplicationDbContext context)
        {
   _context = context;
       }

        public async Task<(IEnumerable<Media> Items, int TotalCount)> GetAllAsync(
            string? mediaType = null,
     string? searchTerm = null,
  int page = 1,
       int pageSize = 20,
 string sortBy = "createdAt",
   string sortOrder = "desc")
        {
     var query = _context.Media.AsQueryable();

     // Filter by media type
  if (!string.IsNullOrWhiteSpace(mediaType))
 {
    query = query.Where(m => m.MediaType == mediaType);
     }

   // Search by file name or alt text
    if (!string.IsNullOrWhiteSpace(searchTerm))
      {
      query = query.Where(m =>
m.FileName.Contains(searchTerm) ||
       m.Alt != null && m.Alt.Contains(searchTerm));
        }

   // Sorting - use explicit property mapping instead of EF.Property
     query = sortOrder.ToLower() == "asc"
    ? sortBy.ToLower() switch
            {
   "filename" => query.OrderBy(m => m.FileName),
    "mediatype" => query.OrderBy(m => m.MediaType),
    "filesize" => query.OrderBy(m => m.FileSize),
     "uploadedby" => query.OrderBy(m => m.UploadedBy),
    _ => query.OrderBy(m => m.CreatedAt) // default: createdAt
   }
: sortBy.ToLower() switch
    {
 "filename" => query.OrderByDescending(m => m.FileName),
 "mediatype" => query.OrderByDescending(m => m.MediaType),
       "filesize" => query.OrderByDescending(m => m.FileSize),
          "uploadedby" => query.OrderByDescending(m => m.UploadedBy),
  _ => query.OrderByDescending(m => m.CreatedAt) // default: createdAt
  };

          var totalCount = await query.CountAsync();

var items = await query
.Skip((page - 1) * pageSize)
     .Take(pageSize)
   .AsNoTracking()
      .ToListAsync();

      return (items, totalCount);
   }

        public async Task<Media?> GetByIdAsync(int id)
        {
      return await _context.Media
     .Include(m => m.UploadedByUser)
       .FirstOrDefaultAsync(m => m.Id == id);
        }

     public async Task<Media> CreateAsync(Media media)
  {
      await _context.Media.AddAsync(media);
 await _context.SaveChangesAsync();
    return media;
    }

        public async Task<Media> UpdateAsync(Media media)
  {
       _context.Media.Update(media);
     await _context.SaveChangesAsync();
     return media;
    }

    public async Task<bool> DeleteAsync(int id)
     {
            var media = await _context.Media.FindAsync(id);
      if (media == null)
        return false;

    _context.Media.Remove(media);
       await _context.SaveChangesAsync();
     return true;
        }

        public async Task<bool> FileNameExistsAsync(string fileName, int? excludeId = null)
        {
          var query = _context.Media.Where(m => m.FileName == fileName);

       if (excludeId.HasValue)
            {
        query = query.Where(m => m.Id != excludeId.Value);
     }

    return await query.AnyAsync();
     }

   public async Task<Media?> GetByFilePathAsync(string filePath)
    {
        return await _context.Media
      .FirstOrDefaultAsync(m => m.FilePath == filePath);
     }

        public async Task<int> GetCountByTypeAsync(string mediaType)
       {
      return await _context.Media
  .CountAsync(m => m.MediaType == mediaType);
      }

   public async Task<IEnumerable<Media>> SearchAsync(string searchTerm)
 {
          return await _context.Media
      .Where(m =>
  m.FileName.Contains(searchTerm) ||
           m.Alt != null && m.Alt.Contains(searchTerm))
      .AsNoTracking()
       .ToListAsync();
      }

        public async Task<IEnumerable<Media>> GetByUserAsync(int userId)
        {
      return await _context.Media
     .Where(m => m.UploadedBy == userId)
               .AsNoTracking()
   .ToListAsync();
        }
    }
}
