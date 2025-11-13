using Gahar_Backend.Models.DTOs.Layout;

namespace Gahar_Backend.Services.Interfaces
{
    /// <summary>
    /// Service interface for Layout business logic
    /// </summary>
    public interface ILayoutService
    {
      /// <summary>
        /// Gets all layouts
   /// </summary>
        Task<IEnumerable<LayoutDto>> GetAllLayoutsAsync();

  /// <summary>
  /// Gets all active layouts
  /// </summary>
    Task<IEnumerable<LayoutDto>> GetActiveLayoutsAsync();

        /// <summary>
 /// Gets layout by ID
    /// </summary>
        Task<LayoutDto> GetLayoutByIdAsync(int id);

        /// <summary>
  /// Gets layout with statistics
  /// </summary>
        Task<LayoutWithStatsDto> GetLayoutWithStatsAsync(int id);

        /// <summary>
        /// Gets the default layout
        /// </summary>
  Task<LayoutDto?> GetDefaultLayoutAsync();

        /// <summary>
        /// Creates a new layout
        /// </summary>
     Task<LayoutDto> CreateLayoutAsync(CreateLayoutDto createDto);

        /// <summary>
 /// Updates an existing layout
        /// </summary>
        Task<LayoutDto> UpdateLayoutAsync(int id, UpdateLayoutDto updateDto);

   /// <summary>
        /// Deletes a layout
/// </summary>
      Task<bool> DeleteLayoutAsync(int id);

 /// <summary>
   /// Sets a layout as default
     /// </summary>
    Task SetAsDefaultAsync(int id);

        /// <summary>
        /// Validates layout configuration JSON
 /// </summary>
   bool ValidateConfiguration(object configuration);
 }
}
