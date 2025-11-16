using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    /// <summary>
    /// Represents a custom field for a content type
    /// </summary>
    public class ContentTypeField : TranslatableEntity
    {
   /// <summary>
        /// ID of the parent content type
    /// </summary>
        public int ContentTypeId { get; set; }
    
        /// <summary>
   /// Navigation property to parent content type
        /// </summary>
  public ContentType ContentType { get; set; } = null!;

  /// <summary>
  /// Display name of the field (e.g., "Event Date", "Location")
        /// </summary>
     [Required]
    [StringLength(100)]
        public string Name { get; set; } = string.Empty;

    /// <summary>
 /// Unique key for the field (e.g., "event_date", "location")
     /// </summary>
        [Required]
        [StringLength(100)]
        public string FieldKey { get; set; } = string.Empty;

        /// <summary>
        /// Type of the field (Text, Textarea, RichText, Date, Image, File, etc.)
      /// </summary>
        [Required]
        [StringLength(50)]
        public string FieldType { get; set; } = string.Empty;

        /// <summary>
  /// Description/Help text for the field
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

  /// <summary>
     /// Indicates if this field is required
      /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Indicates if this field supports translation
        /// </summary>
        public bool IsTranslatable { get; set; } = true;

        /// <summary>
 /// Indicates if this field should be shown in content list
        /// </summary>
     public bool ShowInList { get; set; } = true;

      /// <summary>
        /// Display order in the form
        /// </summary>
        public int DisplayOrder { get; set; }

      /// <summary>
        /// JSON validation rules (e.g., {"minLength": 10, "maxLength": 500})
        /// </summary>
        public string? ValidationRules { get; set; }

    /// <summary>
        /// Default value for the field
        /// </summary>
        public string? DefaultValue { get; set; }

 /// <summary>
        /// Placeholder text for the input
        /// </summary>
        [StringLength(500)]
        public string? Placeholder { get; set; }

        /// <summary>
    /// Options for Dropdown, Radio, Checkbox fields (JSON array)
     /// </summary>
        public string? Options { get; set; }
    }
}
