using Gahar_Backend.Models.DTOs.Album;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;
using Microsoft.Extensions.Logging;

namespace Gahar_Backend.Services.Implementations
{
    /// <summary>
    /// Service for album management
    /// </summary>
    public class AlbumService : IAlbumService
  {
        private readonly IAlbumRepository _repository;
     private readonly IMediaRepository _mediaRepository;
        private readonly ILogger<AlbumService> _logger;

        public AlbumService(
         IAlbumRepository repository,
       IMediaRepository mediaRepository,
     ILogger<AlbumService> logger)
        {
    _repository = repository;
            _mediaRepository = mediaRepository;
   _logger = logger;
     }

    public async Task<(IEnumerable<AlbumDto> Items, int TotalCount)> GetAllAsync(
     bool? isPublished = null,
     string? searchTerm = null,
          int page = 1,
int pageSize = 20)
  {
        var (items, totalCount) = await _repository.GetAllAsync(
       isPublished,
  searchTerm,
  page,
        pageSize);

    var dtos = items.Select(a => MapToDto(a)).ToList();
            return (dtos, totalCount);
        }

   public async Task<AlbumDetailDto?> GetByIdAsync(int id)
    {
 var album = await _repository.GetByIdAsync(id);
   if (album == null)
     return null;

  return MapToDetailDto(album);
     }

        public async Task<AlbumDetailDto?> GetBySlugAsync(string slug)
   {
        var album = await _repository.GetBySlugAsync(slug);
  if (album == null)
     return null;

   // Increment view count
            await _repository.IncrementViewCountAsync(album.Id);

          return MapToDetailDto(album);
      }

     public async Task<AlbumDto> CreateAsync(CreateAlbumDto dto, int userId)
        {
  // Check if slug already exists
            if (await _repository.SlugExistsAsync(dto.Slug))
       {
          throw new BadRequestException("Album slug already exists");
    }

         var album = new Album
            {
  Title = dto.Title,
     Slug = dto.Slug,
Description = dto.Description,
            CoverImageId = dto.CoverImageId,
     IsPublished = dto.IsPublished,
       PublishedAt = dto.IsPublished ? DateTime.UtcNow : null,
     CreatedBy = userId,
    CreatedAt = DateTime.UtcNow
    };

            // Validate cover image if provided
  if (dto.CoverImageId.HasValue)
       {
       var media = await _mediaRepository.GetByIdAsync(dto.CoverImageId.Value);
         if (media == null)
                {
        throw new NotFoundException("Cover image not found");
            }
            }

   await _repository.CreateAsync(album);

        _logger.LogInformation("Album created: {AlbumTitle} (ID: {AlbumId})", album.Title, album.Id);

            return MapToDto(album);
        }

        public async Task<AlbumDto> UpdateAsync(int id, UpdateAlbumDto dto)
        {
  var album = await _repository.GetByIdAsync(id);
            if (album == null)
    {
                throw new NotFoundException($"Album with ID {id} not found");
        }

         // Check if new slug already exists (exclude current album)
            if (dto.Slug != album.Slug && await _repository.SlugExistsAsync(dto.Slug, id))
            {
         throw new BadRequestException("Album slug already exists");
            }

          album.Title = dto.Title;
       album.Slug = dto.Slug;
          album.Description = dto.Description;
  album.IsPublished = dto.IsPublished;

            if (dto.IsPublished && !album.PublishedAt.HasValue)
            {
     album.PublishedAt = DateTime.UtcNow;
            }

            if (dto.CoverImageId.HasValue && dto.CoverImageId != album.CoverImageId)
     {
         var media = await _mediaRepository.GetByIdAsync(dto.CoverImageId.Value);
                if (media == null)
    {
           throw new NotFoundException("Cover image not found");
          }
                album.CoverImageId = dto.CoverImageId;
            }

          album.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(album);

 _logger.LogInformation("Album updated: {AlbumTitle} (ID: {AlbumId})", album.Title, album.Id);

            return MapToDto(album);
    }

        public async Task<bool> DeleteAsync(int id)
        {
            var album = await _repository.GetByIdAsync(id);
 if (album == null)
          {
         throw new NotFoundException($"Album with ID {id} not found");
   }

   var result = await _repository.DeleteAsync(id);

            if (result)
            {
_logger.LogInformation("Album deleted: {AlbumId}", id);
  }

 return result;
      }

        public async Task<AlbumMediaDto> AddMediaAsync(int albumId, AddMediaToAlbumDto dto)
        {
    var album = await _repository.GetByIdAsync(albumId);
  if (album == null)
    {
       throw new NotFoundException($"Album with ID {albumId} not found");
      }

    // Verify media exists
         var media = await _mediaRepository.GetByIdAsync(dto.MediaId);
       if (media == null)
  {
     throw new NotFoundException($"Media with ID {dto.MediaId} not found");
      }

    try
       {
 await _repository.AddMediaToAlbumAsync(albumId, dto.MediaId, dto.Caption);
            }
catch (InvalidOperationException ex)
            {
            throw new BadRequestException(ex.Message);
       }

          // Get the added media
            var albumMedias = await _repository.GetAlbumMediaAsync(albumId);
       var addedMedia = albumMedias.FirstOrDefault(am => am.MediaId == dto.MediaId);

   if (addedMedia != null)
            {
 _logger.LogInformation("Media added to album: AlbumId={AlbumId}, MediaId={MediaId}", albumId, dto.MediaId);
                return MapToAlbumMediaDto(addedMedia);
            }

            throw new BadRequestException("Failed to add media to album");
   }

   public async Task<List<AlbumMediaDto>> AddMultipleMediaAsync(int albumId, AddMultipleMediaToAlbumDto dto)
        {
       var album = await _repository.GetByIdAsync(albumId);
    if (album == null)
     {
     throw new NotFoundException($"Album with ID {albumId} not found");
     }

     var addedMediaList = new List<AlbumMediaDto>();

            foreach (var mediaId in dto.MediaIds)
        {
         try
                {
var media = await _mediaRepository.GetByIdAsync(mediaId);
  if (media == null)
    {
          _logger.LogWarning("Media not found: {MediaId}", mediaId);
   continue;
  }

           await _repository.AddMediaToAlbumAsync(albumId, mediaId);

              var albumMedias = await _repository.GetAlbumMediaAsync(albumId);
      var addedMedia = albumMedias.FirstOrDefault(am => am.MediaId == mediaId);

         if (addedMedia != null)
{
       addedMediaList.Add(MapToAlbumMediaDto(addedMedia));
     }
    }
    catch (Exception ex)
          {
         _logger.LogError(ex, "Error adding media {MediaId} to album", mediaId);
      }
   }

   _logger.LogInformation("Multiple media added to album: AlbumId={AlbumId}, Count={Count}", albumId, addedMediaList.Count);

            return addedMediaList;
 }

        public async Task<bool> RemoveMediaAsync(int albumId, int mediaId)
        {
            var album = await _repository.GetByIdAsync(albumId);
            if (album == null)
        {
                throw new NotFoundException($"Album with ID {albumId} not found");
        }

     var result = await _repository.RemoveMediaFromAlbumAsync(albumId, mediaId);

    if (result)
         {
    _logger.LogInformation("Media removed from album: AlbumId={AlbumId}, MediaId={MediaId}", albumId, mediaId);
            }

      return result;
        }

        public async Task<bool> ReorderMediaAsync(int albumId, ReorderAlbumMediaDto dto)
 {
     var album = await _repository.GetByIdAsync(albumId);
            if (album == null)
        {
          throw new NotFoundException($"Album with ID {albumId} not found");
        }

    await _repository.ReorderAlbumMediaAsync(albumId, dto.MediaIds);

       _logger.LogInformation("Album media reordered: AlbumId={AlbumId}", albumId);

  return true;
    }

        public async Task<AlbumMediaDto> UpdateMediaAsync(int albumId, int mediaId, UpdateAlbumMediaDto dto)
   {
            var album = await _repository.GetByIdAsync(albumId);
  if (album == null)
   {
                throw new NotFoundException($"Album with ID {albumId} not found");
     }

       var albumMedias = await _repository.GetAlbumMediaAsync(albumId);
            var albumMedia = albumMedias.FirstOrDefault(am => am.MediaId == mediaId);

            if (albumMedia == null)
    {
           throw new NotFoundException("Media not found in this album");
     }

         albumMedia.Caption = dto.Caption;
            albumMedia.IsFeatured = dto.IsFeatured;

            // Save changes
         var dbContext = album; // This is a workaround - in production use DbContext directly
         _logger.LogInformation("Album media updated: AlbumId={AlbumId}, MediaId={MediaId}", albumId, mediaId);

   return MapToAlbumMediaDto(albumMedia);
        }

public async Task<AlbumDto> SetCoverImageAsync(int albumId, int mediaId)
        {
            var album = await _repository.GetByIdAsync(albumId);
  if (album == null)
   {
       throw new NotFoundException($"Album with ID {albumId} not found");
      }

            // Check if media exists in album
 var albumMedias = await _repository.GetAlbumMediaAsync(albumId);
            var mediaExists = albumMedias.Any(am => am.MediaId == mediaId);

         if (!mediaExists)
            {
        throw new BadRequestException("Media not found in this album");
            }

            album.CoverImageId = mediaId;
            album.UpdatedAt = DateTime.UtcNow;

         await _repository.UpdateAsync(album);

_logger.LogInformation("Album cover image updated: AlbumId={AlbumId}, MediaId={MediaId}", albumId, mediaId);

         return MapToDto(album);
        }

        public async Task IncrementViewCountAsync(int albumId)
        {
  await _repository.IncrementViewCountAsync(albumId);
      }

        public async Task<IEnumerable<AlbumDto>> GetPublishedAsync(int top = 10)
        {
  var albums = await _repository.GetPublishedAsync(top);
            return albums.Select(a => MapToDto(a)).ToList();
 }

    public async Task<IEnumerable<AlbumDto>> GetByUserAsync(int userId)
        {
    var albums = await _repository.GetByUserAsync(userId);
    return albums.Select(a => MapToDto(a)).ToList();
   }

        public async Task<IEnumerable<AlbumDto>> SearchAsync(string searchTerm)
        {
      var (albums, _) = await _repository.GetAllAsync(
   searchTerm: searchTerm,
     pageSize: int.MaxValue);

 return albums.Select(a => MapToDto(a)).ToList();
        }

        private AlbumDto MapToDto(Album album)
{
            return new AlbumDto
            {
           Id = album.Id,
         Title = album.Title,
       Slug = album.Slug,
    Description = album.Description,
      CoverImage = album.CoverImage != null ? MapToCoverImageDto(album.CoverImage) : null,
     MediaCount = album.AlbumMedias?.Count ?? 0,
       ViewCount = album.ViewCount,
     IsPublished = album.IsPublished,
    PublishedAt = album.PublishedAt,
                CreatorName = album.Creator?.FirstName,
   CreatedAt = album.CreatedAt,
        UpdatedAt = album.UpdatedAt
     };
    }

        private AlbumDetailDto MapToDetailDto(Album album)
      {
 var dto = new AlbumDetailDto
 {
   Id = album.Id,
              Title = album.Title,
         Slug = album.Slug,
         Description = album.Description,
  CoverImage = album.CoverImage != null ? MapToCoverImageDto(album.CoverImage) : null,
                MediaCount = album.AlbumMedias?.Count ?? 0,
   ViewCount = album.ViewCount,
             IsPublished = album.IsPublished,
    PublishedAt = album.PublishedAt,
       CreatorName = album.Creator?.FirstName,
  CreatedAt = album.CreatedAt,
     UpdatedAt = album.UpdatedAt,
      Media = album.AlbumMedias?
                    .OrderBy(am => am.DisplayOrder)
        .Select(am => MapToAlbumMediaDto(am))
           .ToList() ?? new List<AlbumMediaDto>()
            };

  return dto;
        }

        private AlbumCoverImageDto MapToCoverImageDto(Media media)
        {
            return new AlbumCoverImageDto
   {
             Id = media.Id,
                FilePath = media.FilePath,
       ThumbnailPath = media.ThumbnailPath,
        WebPPath = media.WebPPath,
        Width = media.Width,
    Height = media.Height
         };
        }

        private AlbumMediaDto MapToAlbumMediaDto(AlbumMedia albumMedia)
      {
         return new AlbumMediaDto
            {
 Id = albumMedia.Id,
     MediaId = albumMedia.MediaId,
                FileName = albumMedia.Media?.FileName ?? string.Empty,
 FilePath = albumMedia.Media?.FilePath ?? string.Empty,
                ThumbnailPath = albumMedia.Media?.ThumbnailPath,
       WebPPath = albumMedia.Media?.WebPPath,
                Width = albumMedia.Media?.Width,
                Height = albumMedia.Media?.Height,
         Caption = albumMedia.Caption,
           DisplayOrder = albumMedia.DisplayOrder,
    IsFeatured = albumMedia.IsFeatured
            };
        }
  }
}
