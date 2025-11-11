namespace Gahar_Backend.Services.Interfaces
{
    public interface ITranslationService
  {
       Task<string> GetTranslationAsync(string entityType, int entityId, string fieldName, string languageCode);
   Task SaveTranslationAsync(string entityType, int entityId, string fieldName, string languageCode, string value);
   Task<Dictionary<string, string>> GetAllTranslationsAsync(string entityType, int entityId, string languageCode);
    }
}
