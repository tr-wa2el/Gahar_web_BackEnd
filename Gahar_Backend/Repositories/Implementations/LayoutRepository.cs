using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for Layout entity
    /// </summary>
    public class LayoutRepository : GenericRepository<Layout>, ILayoutRepository
    {
        public LayoutRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the default layout
        /// </summary>
        public async Task<Layout?> GetDefaultLayoutAsync()
        {
   return await _context.Layouts
        .FirstOrDefaultAsync(l => l.IsDefault && l.IsActive);
     }

        /// <summary>
    /// Gets layout by name
 /// </summary>
        public async Task<Layout?> GetByNameAsync(string name)
  {
   return await _context.Layouts
     .FirstOrDefaultAsync(l => l.Name == name);
        }

      /// <summary>
        /// Checks if a layout name exists
        /// </summary>
     public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
   {
            var query = _context.Layouts.Where(l => l.Name == name);

   if (excludeId.HasValue)
            {
     query = query.Where(l => l.Id != excludeId.Value);
            }

   return await query.AnyAsync();
        }

        /// <summary>
        /// Gets all active layouts
        /// </summary>
        public async Task<IEnumerable<Layout>> GetActiveLayoutsAsync()
 {
            return await _context.Layouts
                .Where(l => l.IsActive)
                .OrderByDescending(l => l.IsDefault)
            .ThenBy(l => l.Name)
   .ToListAsync();
        }

    /// <summary>
        /// Sets a layout as default (and unsets others)
        /// </summary>
        public async Task SetAsDefaultAsync(int layoutId)
        {
     // First, unset all other defaults
   var defaultLayout = await _context.Layouts.FirstOrDefaultAsync(l => l.IsDefault);
            if (defaultLayout != null)
  {
       defaultLayout.IsDefault = false;
      }

            // Then set the new default
   var layout = await _context.Layouts.FirstOrDefaultAsync(l => l.Id == layoutId);
   if (layout != null)
        {
 layout.IsDefault = true;
 }

         await _context.SaveChangesAsync();
        }

        /// <summary>
    /// Gets layout with content count
        /// </summary>
        public async Task<(Layout layout, int contentCount)> GetLayoutWithStatsAsync(int layoutId)
        {
            var layout = await GetByIdAsync(layoutId);

            // Return null layout so the service can handle it
            if (layout == null)
            {
          return (null!, 0);
            }

            var contentCount = await _context.Contents
   .CountAsync(c => c.LayoutId == layoutId);

            return (layout, contentCount);
        }
    }
}
