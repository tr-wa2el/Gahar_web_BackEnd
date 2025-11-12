using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    /// <summary>
  /// Stores dynamic field values for content
/// </summary>
    public class ContentFieldValue : BaseEntity
    {
    /// <summary>
        /// ID of the content
        /// </summary>
   public int ContentId { get; set; }
     
    /// <summary>
        /// Navigation property to content
        /// </summary>
        public Content Content { get; set; } = null!;

   /// <summary>
        /// ID of the content type field definition
        /// </summary>
 public int ContentTypeFieldId { get; set; }
        
 /// <summary>
  /// Navigation property to field definition
        /// </summary>
        public ContentTypeField ContentTypeField { get; set; } = null!;

        /// <summary>
  /// Field key for quick lookup
        /// </summary>
        [Required]
     [StringLength(100)]
        public string FieldKey { get; set; } = string.Empty;

  /// <summary>
        /// The actual value (text, number, date, JSON, etc.)
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
   /// Language ID for translated values (optional)
 /// </summary>
   public int? LanguageId { get; set; }
   
        /// <summary>
        /// Navigation property to language
        /// </summary>
  public Language? Language { get; set; }
    }
}
