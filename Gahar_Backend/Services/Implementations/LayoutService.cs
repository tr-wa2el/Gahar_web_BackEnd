using System.Text.Json;
using Gahar_Backend.Models.DTOs.Layout;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Implementations
{
    /// <summary>
    /// Service implementation for Layout business logic
    /// </summary>
    public class LayoutService : ILayoutService
    {
        private readonly ILayoutRepository _layoutRepository;

        public LayoutService(ILayoutRepository layoutRepository)
        {
            _layoutRepository = layoutRepository;
        }

        /// <summary>
        /// Gets all layouts
        /// </summary>
        public async Task<IEnumerable<LayoutDto>> GetAllLayoutsAsync()
        {
            var layouts = await _layoutRepository.GetAllAsync();
            return layouts.Select(MapToDto);
        }

        /// <summary>
        /// Gets all active layouts
        /// </summary>
        public async Task<IEnumerable<LayoutDto>> GetActiveLayoutsAsync()
        {
            var layouts = await _layoutRepository.GetActiveLayoutsAsync();
            return layouts.Select(MapToDto);
        }

        /// <summary>
        /// Gets layout by ID
        /// </summary>
        public async Task<LayoutDto> GetLayoutByIdAsync(int id)
        {
            var layout = await _layoutRepository.GetByIdAsync(id);

            if (layout == null)
            {
                throw new NotFoundException($"Layout with ID {id} not found");
            }

            return MapToDto(layout);
        }

        /// <summary>
        /// Gets layout with statistics
        /// </summary>
        public async Task<LayoutWithStatsDto> GetLayoutWithStatsAsync(int id)
        {
            var (layout, contentCount) = await _layoutRepository.GetLayoutWithStatsAsync(id);

            if (layout == null)
            {
                throw new NotFoundException($"Layout with ID {id} not found");
            }

            return new LayoutWithStatsDto
            {
                Id = layout.Id,
                Name = layout.Name,
                Description = layout.Description,
                Configuration = ParseConfiguration(layout.Configuration),
                IsDefault = layout.IsDefault,
                IsActive = layout.IsActive,
                PreviewImage = layout.PreviewImage,
                CreatedAt = layout.CreatedAt,
                UpdatedAt = layout.UpdatedAt,
                ContentCount = contentCount
            };
        }

        /// <summary>
        /// Gets the default layout
        /// </summary>
        public async Task<LayoutDto?> GetDefaultLayoutAsync()
        {
            var layout = await _layoutRepository.GetDefaultLayoutAsync();
            return layout != null ? MapToDto(layout) : null;
        }

        /// <summary>
        /// Creates a new layout
        /// </summary>
        public async Task<LayoutDto> CreateLayoutAsync(CreateLayoutDto createDto)
        {
            // Validate configuration
            if (!ValidateConfiguration(createDto.Configuration))
            {
                throw new ValidationException("Invalid layout configuration");
            }

            // Check if name already exists
            if (await _layoutRepository.NameExistsAsync(createDto.Name))
            {
                throw new ValidationException($"Layout with name '{createDto.Name}' already exists");
            }

            var layout = new Layout
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Configuration = JsonSerializer.Serialize(createDto.Configuration),
                IsActive = createDto.IsActive,
                PreviewImage = createDto.PreviewImage,
                IsDefault = false
            };

            await _layoutRepository.CreateAsync(layout);

            return MapToDto(layout);
        }

        /// <summary>
        /// Updates an existing layout
        /// </summary>
        public async Task<LayoutDto> UpdateLayoutAsync(int id, UpdateLayoutDto updateDto)
        {
            var layout = await _layoutRepository.GetByIdAsync(id);

            if (layout == null)
            {
                throw new NotFoundException($"Layout with ID {id} not found");
            }

            // Validate configuration
            if (!ValidateConfiguration(updateDto.Configuration))
            {
                throw new ValidationException("Invalid layout configuration");
            }

            // Check if name already exists for another layout
            if (await _layoutRepository.NameExistsAsync(updateDto.Name, id))
            {
                throw new ValidationException($"Layout with name '{updateDto.Name}' already exists");
            }

            layout.Name = updateDto.Name;
            layout.Description = updateDto.Description;
            layout.Configuration = JsonSerializer.Serialize(updateDto.Configuration);
            layout.IsActive = updateDto.IsActive;
            layout.PreviewImage = updateDto.PreviewImage;

            await _layoutRepository.UpdateAsync(layout);

            return MapToDto(layout);
        }

        /// <summary>
        /// Deletes a layout
        /// </summary>
        public async Task<bool> DeleteLayoutAsync(int id)
        {
            var layout = await _layoutRepository.GetByIdAsync(id);

            if (layout == null)
            {
                throw new NotFoundException($"Layout with ID {id} not found");
            }

            // Check if it's the default layout
            if (layout.IsDefault)
            {
                throw new ValidationException("Cannot delete the default layout. Please set another layout as default first");
            }

            // Check if layout is being used by any content
            var (_, contentCount) = await _layoutRepository.GetLayoutWithStatsAsync(id);
            if (contentCount > 0)
            {
                throw new ValidationException($"Cannot delete layout. It is currently used by {contentCount} content item(s)");
            }

            return await _layoutRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Sets a layout as default
        /// </summary>
        public async Task SetAsDefaultAsync(int id)
        {
            var layout = await _layoutRepository.GetByIdAsync(id);

            if (layout == null)
            {
                throw new NotFoundException($"Layout with ID {id} not found");
            }

            if (!layout.IsActive)
            {
                throw new ValidationException("Cannot set an inactive layout as default");
            }

            await _layoutRepository.SetAsDefaultAsync(id);
        }

        /// <summary>
        /// Validates layout configuration JSON
        /// </summary>
        public bool ValidateConfiguration(object configuration)
        {
            try
            {
                // Check if configuration is null
                if (configuration == null)
                {
                    return false;
                }

                // Try to serialize the configuration
                var json = JsonSerializer.Serialize(configuration);

                // Check if JSON is empty or null
                if (string.IsNullOrWhiteSpace(json))
                {
                    return false;
                }

                // Try to deserialize it back to ensure it's valid JSON
                JsonSerializer.Deserialize<object>(json);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #region Private Helper Methods

        /// <summary>
        /// Maps Layout entity to LayoutDto
        /// </summary>
        private LayoutDto MapToDto(Layout layout)
        {
            return new LayoutDto
            {
                Id = layout.Id,
                Name = layout.Name,
                Description = layout.Description,
                Configuration = ParseConfiguration(layout.Configuration),
                IsDefault = layout.IsDefault,
                IsActive = layout.IsActive,
                PreviewImage = layout.PreviewImage,
                CreatedAt = layout.CreatedAt,
                UpdatedAt = layout.UpdatedAt
            };
        }

        /// <summary>
        /// Parses configuration JSON string to object
        /// </summary>
        private object ParseConfiguration(string configurationJson)
        {
            try
            {
                return JsonSerializer.Deserialize<object>(configurationJson) ?? new { };
            }
            catch
            {
                return new { };
            }
        }

        #endregion
    }
}
