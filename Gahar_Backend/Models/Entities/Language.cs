using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    public class Language : BaseEntity
    {
     [Required]
        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        [Required]
      [StringLength(100)]
   public string Name { get; set; } = string.Empty;

 public bool IsDefault { get; set; } = false;

        public bool IsActive { get; set; } = true;

        [StringLength(10)]
        public string? Direction { get; set; } = "ltr";

        // Navigation Properties
        public ICollection<Translation> Translations { get; set; } = new List<Translation>();
    }
}
