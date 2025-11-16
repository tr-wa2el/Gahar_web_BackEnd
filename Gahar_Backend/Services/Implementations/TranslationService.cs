using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Services.Implementations
{
    public class TranslationService : ITranslationService
 {
  private readonly ApplicationDbContext _context;

     public TranslationService(ApplicationDbContext context)
  {
        _context = context;
        }

        public async Task<string> GetTranslationAsync(string entityType, int entityId, string fieldName, string languageCode)
        {
      var language = await _context.Languages.FirstOrDefaultAsync(l => l.Code == languageCode);
     if (language == null)
 return string.Empty;

    var translation = await _context.Translations
       .FirstOrDefaultAsync(t => 
   t.EntityType == entityType &&
        t.EntityId == entityId &&
        t.FieldName == fieldName &&
           t.LanguageId == language.Id);

       return translation?.Value ?? string.Empty;
 }

     public async Task SaveTranslationAsync(string entityType, int entityId, string fieldName, string languageCode, string value)
        {
       var language = await _context.Languages.FirstOrDefaultAsync(l => l.Code == languageCode);
       if (language == null)
     throw new Exception("Language not found");

    var translation = await _context.Translations
     .FirstOrDefaultAsync(t =>
 t.EntityType == entityType &&
    t.EntityId == entityId &&
          t.FieldName == fieldName &&
    t.LanguageId == language.Id);

       if (translation == null)
       {
     translation = new Translation
    {
    EntityType = entityType,
      EntityId = entityId,
     FieldName = fieldName,
          LanguageId = language.Id,
        Value = value
       };
 await _context.Translations.AddAsync(translation);
        }
 else
{
   translation.Value = value;
   _context.Translations.Update(translation);
      }

  await _context.SaveChangesAsync();
 }

  public async Task<Dictionary<string, string>> GetAllTranslationsAsync(string entityType, int entityId, string languageCode)
   {
   var language = await _context.Languages.FirstOrDefaultAsync(l => l.Code == languageCode);
   if (language == null)
   return new Dictionary<string, string>();

            var translations = await _context.Translations
  .Where(t => 
  t.EntityType == entityType &&
       t.EntityId == entityId &&
     t.LanguageId == language.Id)
   .ToDictionaryAsync(t => t.FieldName, t => t.Value);

     return translations;
        }
    }
}
