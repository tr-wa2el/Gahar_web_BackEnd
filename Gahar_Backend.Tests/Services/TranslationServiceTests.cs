using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace Gahar_Backend.Tests.Services
{
    public class TranslationServiceTests : IDisposable
    {
    private readonly ApplicationDbContext _context;
   private readonly TranslationService _translationService;

        public TranslationServiceTests()
     {
       var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

     _context = new ApplicationDbContext(options);
       _translationService = new TranslationService(_context);

       SeedLanguages();
 }

 private void SeedLanguages()
  {
      _context.Languages.AddRange(
    new Language { Id = 1, Code = "ar", Name = "???????", IsDefault = true, IsActive = true },
     new Language { Id = 2, Code = "en", Name = "English", IsDefault = false, IsActive = true }
       );
   _context.SaveChanges();
 }

   [Fact]
      public async Task SaveTranslationAsync_ShouldCreateNewTranslation()
     {
   // Arrange
    var entityType = "ContentType";
     var entityId = 1;
       var fieldName = "Title";
var languageCode = "ar";
 var value = "????? ???????";

       // Act
    await _translationService.SaveTranslationAsync(entityType, entityId, fieldName, languageCode, value);

  // Assert
var translation = await _context.Translations.FirstOrDefaultAsync();
  translation.Should().NotBeNull();
     translation!.EntityType.Should().Be(entityType);
 translation.EntityId.Should().Be(entityId);
   translation.FieldName.Should().Be(fieldName);
    translation.Value.Should().Be(value);
        }

 [Fact]
      public async Task SaveTranslationAsync_ShouldUpdateExistingTranslation()
{
  // Arrange
  var entityType = "ContentType";
  var entityId = 1;
   var fieldName = "Title";
  var languageCode = "ar";
 var initialValue = "????? ????";
  var updatedValue = "????? ????";

   // Create initial translation
       await _translationService.SaveTranslationAsync(entityType, entityId, fieldName, languageCode, initialValue);

 // Act - Update translation
   await _translationService.SaveTranslationAsync(entityType, entityId, fieldName, languageCode, updatedValue);

            // Assert
      var translations = await _context.Translations.ToListAsync();
translations.Should().HaveCount(1);
  translations[0].Value.Should().Be(updatedValue);
 }

[Fact]
   public async Task GetTranslationAsync_WithExistingTranslation_ShouldReturnValue()
   {
         // Arrange
       var entityType = "ContentType";
   var entityId = 1;
     var fieldName = "Title";
       var languageCode = "ar";
 var value = "????? ???????";

       await _translationService.SaveTranslationAsync(entityType, entityId, fieldName, languageCode, value);

   // Act
     var result = await _translationService.GetTranslationAsync(entityType, entityId, fieldName, languageCode);

            // Assert
  result.Should().Be(value);
  }

        [Fact]
     public async Task GetTranslationAsync_WithNonExistingTranslation_ShouldReturnEmptyString()
     {
   // Arrange
       var entityType = "ContentType";
var entityId = 1;
  var fieldName = "Title";
  var languageCode = "ar";

    // Act
     var result = await _translationService.GetTranslationAsync(entityType, entityId, fieldName, languageCode);

  // Assert
result.Should().BeEmpty();
        }

 [Fact]
     public async Task GetTranslationAsync_WithInvalidLanguage_ShouldReturnEmptyString()
 {
     // Arrange
     var entityType = "ContentType";
       var entityId = 1;
var fieldName = "Title";
    var languageCode = "fr"; // Non-existing language

   // Act
       var result = await _translationService.GetTranslationAsync(entityType, entityId, fieldName, languageCode);

// Assert
   result.Should().BeEmpty();
        }

  [Fact]
        public async Task GetAllTranslationsAsync_ShouldReturnAllTranslationsForEntity()
  {
   // Arrange
   var entityType = "ContentType";
  var entityId = 1;
     var languageCode = "ar";

      await _translationService.SaveTranslationAsync(entityType, entityId, "Title", languageCode, "???????");
 await _translationService.SaveTranslationAsync(entityType, entityId, "Description", languageCode, "?????");
       await _translationService.SaveTranslationAsync(entityType, entityId, "Content", languageCode, "???????");

       // Act
     var translations = await _translationService.GetAllTranslationsAsync(entityType, entityId, languageCode);

     // Assert
     translations.Should().HaveCount(3);
     translations["Title"].Should().Be("???????");
   translations["Description"].Should().Be("?????");
       translations["Content"].Should().Be("???????");
        }

        [Fact]
public async Task GetAllTranslationsAsync_WithNoTranslations_ShouldReturnEmptyDictionary()
        {
 // Arrange
       var entityType = "ContentType";
 var entityId = 999;
    var languageCode = "ar";

  // Act
       var translations = await _translationService.GetAllTranslationsAsync(entityType, entityId, languageCode);

      // Assert
   translations.Should().BeEmpty();
 }

        [Fact]
   public async Task SaveTranslationAsync_WithInvalidLanguage_ShouldThrowException()
 {
   // Arrange
       var entityType = "ContentType";
  var entityId = 1;
       var fieldName = "Title";
       var languageCode = "invalid";
       var value = "Test";

  // Act & Assert
    await Assert.ThrowsAsync<Exception>(() => 
        _translationService.SaveTranslationAsync(entityType, entityId, fieldName, languageCode, value));
 }

 public void Dispose()
     {
  _context.Database.EnsureDeleted();
   _context.Dispose();
        }
    }
}
