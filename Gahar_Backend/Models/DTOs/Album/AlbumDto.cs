using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Album
{
    /// <summary>
    /// DTO for Album response
    /// </summary>
    public class AlbumDto
    {
   public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
   public AlbumCoverImageDto? CoverImage { get; set; }
        public int MediaCount { get; set; }
        public int ViewCount { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? CreatorName { get; set; }
      public DateTime CreatedAt { get; set; }
      public DateTime? UpdatedAt { get; set; }
  }

    /// <summary>
    /// DTO for Album detail response with media
    /// </summary>
    public class AlbumDetailDto : AlbumDto
    {
        public List<AlbumMediaDto> Media { get; set; } = new();
    }

    /// <summary>
    /// DTO for album cover image
    /// </summary>
    public class AlbumCoverImageDto
{
        public int Id { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string? ThumbnailPath { get; set; }
        public string? WebPPath { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }

 /// <summary>
    /// DTO for media in album
    /// </summary>
    public class AlbumMediaDto
    {
      public int Id { get; set; }
        public int MediaId { get; set; }
        public string FileName { get; set; } = string.Empty;
      public string FilePath { get; set; } = string.Empty;
     public string? ThumbnailPath { get; set; }
        public string? WebPPath { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string? Caption { get; set; }
        public int DisplayOrder { get; set; }
 public bool IsFeatured { get; set; }
    }

    /// <summary>
    /// DTO for creating an album
    /// </summary>
    public class CreateAlbumDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug is required")]
        [StringLength(200, ErrorMessage = "Slug cannot exceed 200 characters")]
        public string Slug { get; set; } = string.Empty;

     [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

  public bool IsPublished { get; set; } = false;

    public int? CoverImageId { get; set; }

    /// <summary>
        /// Translations for multilingual support
        /// </summary>
        public Dictionary<string, AlbumTranslationDto>? Translations { get; set; }
    }

    /// <summary>
    /// DTO for updating an album
    /// </summary>
    public class UpdateAlbumDto : CreateAlbumDto
    {
    }

  /// <summary>
 /// DTO for album translation
    /// </summary>
    public class AlbumTranslationDto
    {
   public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO for adding media to album
    /// </summary>
    public class AddMediaToAlbumDto
    {
  [Required(ErrorMessage = "MediaId is required")]
 public int MediaId { get; set; }

        [StringLength(500, ErrorMessage = "Caption cannot exceed 500 characters")]
        public string? Caption { get; set; }

        public bool IsFeatured { get; set; } = false;
    }

    /// <summary>
    /// DTO for bulk adding media to album
    /// </summary>
    public class AddMultipleMediaToAlbumDto
    {
        [Required(ErrorMessage = "At least one media is required")]
  public List<int> MediaIds { get; set; } = new();
    }

    /// <summary>
    /// DTO for reordering album media
    /// </summary>
    public class ReorderAlbumMediaDto
    {
    [Required(ErrorMessage = "MediaIds are required")]
        public List<int> MediaIds { get; set; } = new();
    }

    /// <summary>
    /// DTO for updating album media
    /// </summary>
    public class UpdateAlbumMediaDto
    {
      [StringLength(500, ErrorMessage = "Caption cannot exceed 500 characters")]
 public string? Caption { get; set; }

        public bool IsFeatured { get; set; }
    }

    /// <summary>
    /// DTO for album filtering and search
    /// </summary>
    public class AlbumFilterDto
    {
        [StringLength(100)]
public string? SearchTerm { get; set; }

        public bool? IsPublished { get; set; }

      public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;

        public string? SortBy { get; set; } = "createdAt";

        public string? SortOrder { get; set; } = "desc";
    }

    /// <summary>
    /// DTO for album list item
    /// </summary>
    public class AlbumListDto
    {
   public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
      public string Slug { get; set; } = string.Empty;
   public AlbumCoverImageDto? CoverImage { get; set; }
        public int MediaCount { get; set; }
        public int ViewCount { get; set; }
     public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO for summary view of album
  /// </summary>
    public class AlbumSummaryDto
    {
      public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
      public string Slug { get; set; } = string.Empty;
    public string? ThumbnailPath { get; set; }
        public int MediaCount { get; set; }
    }
}
