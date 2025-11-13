using System.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;
using Gahar_Backend.Models.DTOs.Media;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Image = SixLabors.ImageSharp.Image;

namespace Gahar_Backend.Services.Implementations
{
    /// <summary>
    /// Service for media management with image processing
/// </summary>
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _repository;
   private readonly IConfiguration _configuration;
     private readonly ILogger<MediaService> _logger;

        // Supported media types
     private readonly Dictionary<string, string> _mediaTypeExtensions = new()
        {
  // Images
       { ".jpg", "Image" },
     { ".jpeg", "Image" },
   { ".png", "Image" },
      { ".gif", "Image" },
   { ".webp", "Image" },
   { ".bmp", "Image" },
    // Videos
       { ".mp4", "Video" },
    { ".webm", "Video" },
    { ".avi", "Video" },
      { ".mov", "Video" },
 // Documents
  { ".pdf", "Document" },
        { ".docx", "Document" },
     { ".doc", "Document" },
   // Audio
       { ".mp3", "Audio" },
    { ".wav", "Audio" },
  { ".ogg", "Audio" }
        };

        private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB
    private const int ThumbnailSize = 300;
  private const int WebPQuality = 85;

    public MediaService(
     IMediaRepository repository,
    IConfiguration configuration,
            ILogger<MediaService> logger)
       {
          _repository = repository;
_configuration = configuration;
         _logger = logger;
        }

        public async Task<MediaDto> UploadAsync(IFormFile file, string? alt = null, int? userId = null)
    {
           // 1. Validate file
var (isValid, errorMessage) = await ValidateFileAsync(file);
      if (!isValid)
     {
    throw new BadRequestException($"File validation failed: {errorMessage}");
           }

  // 2. Generate unique file name
    var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
  var fileExtension = Path.GetExtension(file.FileName).ToLower();
   var mediaType = GetMediaType(fileExtension);

      // 3. Save original file
      var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    Directory.CreateDirectory(uploadPath);

   var filePath = Path.Combine(uploadPath, uniqueFileName);
   using (var stream = new FileStream(filePath, FileMode.Create))
  {
          await file.CopyToAsync(stream);
         }

   // 4. Create media entity
        var media = new Media
   {
      FileName = file.FileName,
  FilePath = $"/uploads/{uniqueFileName}",
  MimeType = file.ContentType ?? "application/octet-stream",
   FileSize = file.Length,
     Alt = alt,
         MediaType = mediaType,
 UploadedBy = userId,
FileExtension = fileExtension,
      IsProcessed = false,
        CreatedAt = DateTime.UtcNow
       };

      // 5. Process image if applicable
   if (mediaType == "Image")
     {
     try
          {
     await ProcessImageAsync(media, filePath);
     }
       catch (Exception ex)
   {
      _logger.LogError(ex, "Error processing image: {FileName}", file.FileName);
     media.IsProcessed = false;
       }
      }

       // 6. Save to database
    await _repository.CreateAsync(media);

  _logger.LogInformation(
         "File uploaded: {FileName} ({FileSize} bytes)", 
    file.FileName, 
    file.Length);

      return MapToDto(media);
   }

        public async Task<IEnumerable<MediaDto>> UploadMultipleAsync(IFormFileCollection files, int? userId = null)
  {
           var uploadedMedia = new List<MediaDto>();

        foreach (var file in files)
  {
            try
         {
  var media = await UploadAsync(file, null, userId);
        uploadedMedia.Add(media);
       }
  catch (Exception ex)
         {
_logger.LogError(ex, "Error uploading file: {FileName}", file.FileName);
  }
   }

      return uploadedMedia;
  }

   public async Task<(IEnumerable<MediaDto> Items, int TotalCount)> GetAllAsync(
  string? mediaType = null,
    string? searchTerm = null,
            int page = 1,
    int pageSize = 20)
      {
         var (items, totalCount) = await _repository.GetAllAsync(
  mediaType,
     searchTerm,
   page,
      pageSize);

        var dtos = items.Select(m => MapToDto(m)).ToList();
       return (dtos, totalCount);
       }

     public async Task<MediaDto?> GetByIdAsync(int id)
       {
         var media = await _repository.GetByIdAsync(id);
   if (media == null)
     return null;

        return MapToDto(media);
   }

 public async Task<MediaDto> UpdateAsync(int id, UpdateMediaDto dto)
 {
          var media = await _repository.GetByIdAsync(id);
  if (media == null)
         {
        throw new NotFoundException($"Media with ID {id} not found");
     }

        if (!string.IsNullOrWhiteSpace(dto.FileName))
     {
             // Check if new file name already exists
         if (await _repository.FileNameExistsAsync(dto.FileName, id))
           {
           throw new BadRequestException("File name already exists");
    }

          media.FileName = dto.FileName;
        }

     if (!string.IsNullOrWhiteSpace(dto.Alt))
       {
  media.Alt = dto.Alt;
      }

    media.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(media);

   _logger.LogInformation("Media updated: {MediaId}", id);

    return MapToDto(media);
      }

 public async Task<bool> DeleteAsync(int id)
     {
         var media = await _repository.GetByIdAsync(id);
      if (media == null)
      {
     throw new NotFoundException($"Media with ID {id} not found");
        }

   try
       {
  // Delete files from disk
          if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", media.FilePath.TrimStart('/'))))
    {
 File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", media.FilePath.TrimStart('/')));
          }

    if (!string.IsNullOrWhiteSpace(media.ThumbnailPath) &&
      File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", media.ThumbnailPath.TrimStart('/'))))
        {
 File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", media.ThumbnailPath.TrimStart('/')));
            }

       if (!string.IsNullOrWhiteSpace(media.WebPPath) &&
      File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", media.WebPPath.TrimStart('/'))))
   {
        File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", media.WebPPath.TrimStart('/')));
         }
      }
       catch (Exception ex)
    {
    _logger.LogError(ex, "Error deleting media files: {MediaId}", id);
         }

       // Delete from database
   var result = await _repository.DeleteAsync(id);

    _logger.LogInformation("Media deleted: {MediaId}", id);

      return result;
    }

   public async Task<IEnumerable<MediaDto>> SearchAsync(string searchTerm)
    {
       var results = await _repository.SearchAsync(searchTerm);
     return results.Select(m => MapToDto(m)).ToList();
     }

  public async Task<MediaDto> RegenerateWebPAsync(int id, int quality = 85)
      {
        var media = await _repository.GetByIdAsync(id);
 if (media == null)
       {
         throw new NotFoundException($"Media with ID {id} not found");
         }

       if (media.MediaType != "Image")
      {
         throw new BadRequestException("WebP regeneration is only for images");
      }

      var originalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", media.FilePath.TrimStart('/'));

     if (!File.Exists(originalPath))
      {
  throw new NotFoundException("Original file not found");
        }

  try
     {
           // Delete old WebP if exists
         if (!string.IsNullOrWhiteSpace(media.WebPPath))
   {
              var oldWebPPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", media.WebPPath.TrimStart('/'));
           if (File.Exists(oldWebPPath))
      {
       File.Delete(oldWebPPath);
   }
}

        // Generate new WebP
      var webPPath = await GenerateWebPAsync(originalPath, quality);
  var webPFileSize = new FileInfo(webPPath).Length;

   media.WebPPath = $"/uploads/{Path.GetFileName(webPPath)}";
        media.WebPFileSize = webPFileSize;
  media.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(media);

    _logger.LogInformation("WebP regenerated for media: {MediaId}", id);

   return MapToDto(media);
      }
      catch (Exception ex)
  {
      _logger.LogError(ex, "Error regenerating WebP for media: {MediaId}", id);
       throw new BadRequestException($"Failed to regenerate WebP: {ex.Message}");
     }
   }

       public async Task<(bool IsValid, string? ErrorMessage)> ValidateFileAsync(IFormFile file)
    {
     if (file == null || file.Length == 0)
      {
         return (false, "File is empty");
        }

       if (file.Length > MaxFileSize)
 {
           return (false, $"File size exceeds maximum allowed size of {MaxFileSize / 1024 / 1024} MB");
     }

      var fileExtension = Path.GetExtension(file.FileName).ToLower();
    if (!_mediaTypeExtensions.ContainsKey(fileExtension))
     {
          return (false, $"File type '{fileExtension}' is not allowed");
      }

       return await Task.FromResult((true, null as string));
 }

    public async Task<MediaStatsDto> GetStatsAsync()
   {
        var allMedia = await _repository.GetAllAsync(pageSize: int.MaxValue);

  var stats = new MediaStatsDto
       {
        TotalFiles = allMedia.TotalCount,
   ImageCount = await _repository.GetCountByTypeAsync("Image"),
         VideoCount = await _repository.GetCountByTypeAsync("Video"),
      DocumentCount = await _repository.GetCountByTypeAsync("Document"),
          AudioCount = await _repository.GetCountByTypeAsync("Audio"),
   TotalStorageSize = allMedia.Items.Sum(m => m.FileSize),
       WebPTotalSize = allMedia.Items.Where(m => m.WebPFileSize.HasValue).Sum(m => m.WebPFileSize ?? 0)
      };

   return stats;
   }

    private async Task ProcessImageAsync(Media media, string filePath)
    {
        using var image = await Image.LoadAsync(filePath);

  // Get dimensions
 media.Width = image.Width;
   media.Height = image.Height;

    // Generate thumbnail
      var thumbnailPath = await GenerateThumbnailAsync(filePath, ThumbnailSize);
       media.ThumbnailPath = $"/uploads/{Path.GetFileName(thumbnailPath)}";

      // Generate WebP
   var webPPath = await GenerateWebPAsync(filePath, WebPQuality);
 media.WebPPath = $"/uploads/{Path.GetFileName(webPPath)}";
    media.WebPFileSize = new FileInfo(webPPath).Length;

     media.IsProcessed = true;
    }

   private async Task<string> GenerateThumbnailAsync(string imagePath, int size)
     {
      using var image = await Image.LoadAsync(imagePath);

       image.Mutate(x => x.Resize(new ResizeOptions
        {
        Size = new SixLabors.ImageSharp.Size(size, size),
        Mode = ResizeMode.Max
         }));

      var thumbnailFileName = $"thumb_{Guid.NewGuid()}_{Path.GetFileName(imagePath)}";
var thumbnailPath = Path.Combine(
 Path.GetDirectoryName(imagePath) ?? "",
          thumbnailFileName);

    await image.SaveAsync(thumbnailPath);

  return thumbnailPath;
   }

  private async Task<string> GenerateWebPAsync(string imagePath, int quality)
    {
   using var image = await Image.LoadAsync(imagePath);

       var webPEncoder = new WebpEncoder { Quality = quality };

     var webPFileName = $"{Path.GetFileNameWithoutExtension(imagePath)}.webp";
 var webPPath = Path.Combine(
           Path.GetDirectoryName(imagePath) ?? "",
          webPFileName);

        await image.SaveAsync(webPPath, webPEncoder);

     return webPPath;
      }

    private string GetMediaType(string fileExtension)
    {
       return _mediaTypeExtensions.TryGetValue(fileExtension.ToLower(), out var type)
      ? type
      : "Unknown";
    }

private MediaDto MapToDto(Media media)
    {
        return new MediaDto
 {
     Id = media.Id,
         FileName = media.FileName,
  FilePath = media.FilePath,
     ThumbnailPath = media.ThumbnailPath,
    WebPPath = media.WebPPath,
          MimeType = media.MimeType,
     FileSize = media.FileSize,
      WebPFileSize = media.WebPFileSize,
     Width = media.Width,
  Height = media.Height,
   Alt = media.Alt,
     MediaType = media.MediaType,
    FileExtension = media.FileExtension,
          IsProcessed = media.IsProcessed,
    UploadedBy = media.UploadedBy,
         CreatedAt = media.CreatedAt,
      UpdatedAt = media.UpdatedAt
        };
   }
    }
}
