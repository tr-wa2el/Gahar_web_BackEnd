namespace Gahar_Backend.Models.Entities
{
  public abstract class TranslatableEntity : BaseEntity
    {
        // Navigation property for translations
        public ICollection<Translation> Translations { get; set; } = new List<Translation>();
    }
}
