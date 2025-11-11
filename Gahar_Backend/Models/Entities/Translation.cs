using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    public class Translation : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string EntityType { get; set; } = string.Empty;

    public int EntityId { get; set; }

        [Required]
        [StringLength(50)]
      public string FieldName { get; set; } = string.Empty;

  public int LanguageId { get; set; }
        public Language Language { get; set; } = null!;

  [Required]
      public string Value { get; set; } = string.Empty;
    }
}
