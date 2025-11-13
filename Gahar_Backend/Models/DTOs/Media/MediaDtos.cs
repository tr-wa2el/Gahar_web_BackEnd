using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Media;

/// <summary>
/// DTO لعرض الملف
/// </summary>
public class MediaDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? ThumbnailPath { get; set; }
    public string? WebPPath { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public long? WebPFileSize { get; set; }
  public int? Width { get; set; }
    public int? Height { get; set; }
    public string? Alt { get; set; }
    public string? Caption { get; set; }
  public string MediaType { get; set; } = string.Empty;
    public bool IsProcessed { get; set; }
    public int UsageCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO لتحديث بيانات الملف
/// </summary>
public class UpdateMediaDto
{
    [StringLength(200)]
    public string? Alt { get; set; }

    [StringLength(500)]
    public string? Caption { get; set; }
}

/// <summary>
/// DTO لتصفية الملفات
/// </summary>
public class MediaFilterDto
{
    public string? MediaType { get; set; }
    public string? SearchTerm { get; set; }
    public int? UploadedBy { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortOrder { get; set; } = "desc";
}

/// <summary>
/// DTO للألبوم
/// </summary>
public class AlbumDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? CoverImageId { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
 public int ViewCount { get; set; }
    public int MediaCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO لتفاصيل الألبوم
/// </summary>
public class AlbumDetailDto : AlbumDto
{
    public List<AlbumMediaDto> Medias { get; set; } = new();
    public MediaDto? CoverImage { get; set; }
}

/// <summary>
/// DTO لصورة في الألبوم
/// </summary>
public class AlbumMediaDto
{
    public int Id { get; set; }
    public int MediaId { get; set; }
   public int DisplayOrder { get; set; }
    public string? Caption { get; set; }
    public bool IsFeatured { get; set; }
    public MediaDto Media { get; set; } = null!;
}

/// <summary>
/// DTO لإنشاء ألبوم
/// </summary>
public class CreateAlbumDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

  public int? CoverImageId { get; set; }
}

/// <summary>
/// DTO لتحديث ألبوم
/// </summary>
public class UpdateAlbumDto : CreateAlbumDto
{
    public bool IsPublished { get; set; } = true;
}

/// <summary>
/// DTO لإضافة صورة للألبوم
/// </summary>
public class AddMediaToAlbumDto
{
    [Required]
    public List<int> MediaIds { get; set; } = new();

    public string? Caption { get; set; }
    public bool IsFeatured { get; set; } = false;
}

/// <summary>
/// DTO لإعادة ترتيب الصور
/// </summary>
public class ReorderMediaDto
{
    [Required]
    public List<int> MediaIds { get; set; } = new();
}

/// <summary>
/// DTO لتصفية الألبومات
/// </summary>
public class AlbumFilterDto
{
    public bool? IsPublished { get; set; }
    public string? SearchTerm { get; set; }
    public int? CreatedBy { get; set; }
   public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortOrder { get; set; } = "desc";
}
