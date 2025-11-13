using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Repositories.Interfaces
{
    /// <summary>
 /// Repository interface for Layout entity operations
    /// </summary>
    public interface ILayoutRepository : IGenericRepository<Layout>
    {
        /// <summary>
        /// Gets the default layout
        /// </summary>
        Task<Layout?> GetDefaultLayoutAsync();

        /// <summary>
     /// Gets layout by name
  /// </summary>
        Task<Layout?> GetByNameAsync(string name);

        /// <summary>
        /// Checks if a layout name exists
        /// </summary>
Task<bool> NameExistsAsync(string name, int? excludeId = null);

  /// <summary>
        /// Gets all active layouts
        /// </summary>
    Task<IEnumerable<Layout>> GetActiveLayoutsAsync();

        /// <summary>
        /// Sets a layout as default (and unsets others)
        /// </summary>
   Task SetAsDefaultAsync(int layoutId);

        /// <summary>
        /// Gets layout with content count
/// </summary>
        Task<(Layout layout, int contentCount)> GetLayoutWithStatsAsync(int layoutId);
    }
}
