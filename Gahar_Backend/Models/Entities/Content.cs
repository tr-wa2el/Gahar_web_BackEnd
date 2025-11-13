using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    /// <summary>
    /// Represents a content item of any content type
    /// </summary>
    public class Content : TranslatableEntity
    {
        /// <summary>
        /// ID of the content type
        /// </summary>
        public int ContentTypeId { get; set; }
        
        /// <summary>
        /// Navigation property to content type
        /// </summary>
        public ContentType ContentType { get; set; } = null!;

        /// <summary>
        /// Title of the content
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// URL-friendly slug
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Short summary/excerpt
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// Main content body (Rich Text)
        /// </summary>
        public string? Body { get; set; }

        /// <summary>
        /// Featured image URL
        /// </summary>
        [StringLength(500)]
        public string? FeaturedImage { get; set; }

        /// <summary>
        /// Layout ID (optional)
        /// </summary>
        public int? LayoutId { get; set; }

        /// <summary>
        /// Navigation property to layout
        /// </summary>
        public Layout? Layout { get; set; }

        #region SEO Properties
        
        /// <summary>
        /// Meta title for SEO
        /// </summary>
        [StringLength(200)]
        public string? MetaTitle { get; set; }

        /// <summary>
        /// Meta description for SEO
        /// </summary>
        [StringLength(500)]
        public string? MetaDescription { get; set; }

        /// <summary>
        /// Meta keywords for SEO
        /// </summary>
        public string? MetaKeywords { get; set; }

        /// <summary>
        /// Open Graph title
        /// </summary>
        [StringLength(200)]
        public string? OgTitle { get; set; }

        /// <summary>
        /// Open Graph description
        /// </summary>
        [StringLength(500)]
        public string? OgDescription { get; set; }

        /// <summary>
        /// Open Graph image
        /// </summary>
        [StringLength(500)]
        public string? OgImage { get; set; }

        #endregion

        #region Publishing Properties

        /// <summary>
        /// Content status (Draft, Published, Scheduled, Archived)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = ContentStatus.Draft;

        /// <summary>
        /// Date and time when content was published
        /// </summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// Date and time when content should be published
        /// </summary>
        public DateTime? ScheduledAt { get; set; }

        /// <summary>
        /// ID of the author (user)
        /// </summary>
        public int? AuthorId { get; set; }
     
        /// <summary>
        /// Navigation property to author
        /// </summary>
        public User? Author { get; set; }

        #endregion

        #region Stats & Settings

        /// <summary>
        /// Number of views
        /// </summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// Indicates if this content is featured
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// Indicates if comments are allowed
        /// </summary>
        public bool AllowComments { get; set; } = true;

        #endregion

        #region Navigation Properties

        /// <summary>
        /// Custom field values
        /// </summary>
        public ICollection<ContentFieldValue> FieldValues { get; set; } = new List<ContentFieldValue>();

        /// <summary>
        /// Tags associated with this content
        /// </summary>
        public ICollection<ContentTag> ContentTags { get; set; } = new List<ContentTag>();

        #endregion
    }

    /// <summary>
    /// Content status constants
    /// </summary>
    public static class ContentStatus
    {
        public const string Draft = "Draft";
        public const string Published = "Published";
        public const string Scheduled = "Scheduled";
        public const string Archived = "Archived";
    }
}
