using Gahar_Backend.Data;
using Gahar_Backend.Models.DTOs.ContentType;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Implementations;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Implementations;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;
using Xunit;

namespace Gahar_Backend.Tests.Features
{
    public class ContentTypeServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IContentTypeRepository _repository;
        private readonly Mock<ITranslationService> _translationServiceMock;
        private readonly Mock<IAuditLogService> _auditLogServiceMock;
     private readonly ContentTypeService _service;

        public ContentTypeServiceTests()
        {
         var options = new DbContextOptionsBuilder<ApplicationDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
          .Options;

            _context = new ApplicationDbContext(options);
            _repository = new ContentTypeRepository(_context);
    _translationServiceMock = new Mock<ITranslationService>();
            _auditLogServiceMock = new Mock<IAuditLogService>();
      
         _service = new ContentTypeService(
      _repository,
        _translationServiceMock.Object,
      _auditLogServiceMock.Object
          );
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllContentTypes()
        {
            // Arrange
  var contentTypes = new List<ContentType>
            {
      new ContentType { Id = 1, Name = "News", Slug = "news", Icon = "Newspaper" },
        new ContentType { Id = 2, Name = "Events", Slug = "events", Icon = "Calendar" }
       };

          await _context.ContentTypes.AddRangeAsync(contentTypes);
await _context.SaveChangesAsync();

         // Act
            var result = await _service.GetAllAsync();

          // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(ct => ct.Name == "News");
         result.Should().Contain(ct => ct.Name == "Events");
        }

   [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnContentType()
        {
      // Arrange
       var contentType = new ContentType 
       { 
                Id = 1, 
       Name = "News", 
   Slug = "news",
 Fields = new List<ContentTypeField>
  {
         new ContentTypeField { Id = 1, Name = "Author", FieldKey = "author", FieldType = "Text" }
       }
            };

await _context.ContentTypes.AddAsync(contentType);
 await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
     result.Should().NotBeNull();
     result.Name.Should().Be("News");
            result.Fields.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldThrowNotFoundException()
        {
  // Act & Assert
    await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(999));
        }

        [Fact]
 public async Task CreateAsync_WithValidData_ShouldCreateContentType()
        {
     // Arrange
     var dto = new CreateContentTypeDto
            {
      Name = "News",
     Slug = "news",
    Description = "News articles",
    Icon = "Newspaper"
};

         // Act
            var result = await _service.CreateAsync(dto);

 // Assert
   result.Should().NotBeNull();
       result.Name.Should().Be("News");
 result.Slug.Should().Be("news");
            
   var saved = await _context.ContentTypes.FindAsync(result.Id);
            saved.Should().NotBeNull();
        }

        [Fact]
  public async Task CreateAsync_WithDuplicateSlug_ShouldThrowBadRequestException()
{
            // Arrange
  await _context.ContentTypes.AddAsync(new ContentType 
            { 
        Name = "News", 
           Slug = "news" 
    });
        await _context.SaveChangesAsync();

            var dto = new CreateContentTypeDto
        {
       Name = "News 2",
      Slug = "news"
       };

            // Act & Assert
 await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(dto));
        }

        [Fact]
  public async Task UpdateAsync_WithValidData_ShouldUpdateContentType()
   {
 // Arrange
            var contentType = new ContentType 
    { 
   Name = "News", 
          Slug = "news",
         IsActive = true
            };
            await _context.ContentTypes.AddAsync(contentType);
        await _context.SaveChangesAsync();

            var dto = new UpdateContentTypeDto
   {
           Name = "Updated News",
        Slug = "updated-news",
 IsActive = false,
     DisplayOrder = 5
        };

         // Act
 var result = await _service.UpdateAsync(contentType.Id, dto);

            // Assert
            result.Name.Should().Be("Updated News");
result.Slug.Should().Be("updated-news");
   result.IsActive.Should().BeFalse();
        }

        [Fact]
 public async Task DeleteAsync_WithValidId_ShouldDeleteContentType()
        {
      // Arrange
            var contentType = new ContentType { Name = "News", Slug = "news" };
            await _context.ContentTypes.AddAsync(contentType);
            await _context.SaveChangesAsync();

            // Act
            await _service.DeleteAsync(contentType.Id);

            // Assert
            var deleted = await _context.ContentTypes.FindAsync(contentType.Id);
    deleted.Should().NotBeNull();
            deleted!.IsDeleted.Should().BeTrue();
        }

        [Fact]
   public async Task AddFieldAsync_WithValidData_ShouldAddField()
    {
        // Arrange
      var contentType = new ContentType { Name = "News", Slug = "news" };
await _context.ContentTypes.AddAsync(contentType);
         await _context.SaveChangesAsync();

      var dto = new CreateContentTypeFieldDto
            {
            Name = "Author",
      FieldKey = "author",
       FieldType = "Text",
     IsRequired = true
            };

      // Act
            var result = await _service.AddFieldAsync(contentType.Id, dto);

            // Assert
        result.Should().NotBeNull();
          result.Name.Should().Be("Author");
            result.FieldKey.Should().Be("author");

            var updated = await _context.ContentTypes
            .Include(ct => ct.Fields)
        .FirstAsync(ct => ct.Id == contentType.Id);
            updated.Fields.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddFieldAsync_WithDuplicateFieldKey_ShouldThrowBadRequestException()
  {
  // Arrange
            var contentType = new ContentType 
 { 
          Name = "News", 
  Slug = "news",
         Fields = new List<ContentTypeField>
      {
    new ContentTypeField { Name = "Author", FieldKey = "author", FieldType = "Text" }
            }
         };
    await _context.ContentTypes.AddAsync(contentType);
  await _context.SaveChangesAsync();

   var dto = new CreateContentTypeFieldDto
        {
             Name = "Author 2",
         FieldKey = "author",
            FieldType = "Text"
            };

  // Act & Assert
          await Assert.ThrowsAsync<BadRequestException>(() => 
     _service.AddFieldAsync(contentType.Id, dto));
        }

[Fact]
        public async Task UpdateFieldAsync_WithValidData_ShouldUpdateField()
     {
  // Arrange
        var contentType = new ContentType 
   { 
           Name = "News", 
         Slug = "news",
 Fields = new List<ContentTypeField>
     {
        new ContentTypeField 
   { 
          Id = 1,
     Name = "Author", 
       FieldKey = "author", 
         FieldType = "Text" 
          }
         }
            };
            await _context.ContentTypes.AddAsync(contentType);
        await _context.SaveChangesAsync();

            var dto = new UpdateContentTypeFieldDto
          {
Name = "Article Author",
    FieldKey = "article_author",
  FieldType = "Text",
     IsRequired = true,
  DisplayOrder = 1
            };

      // Act
            var result = await _service.UpdateFieldAsync(contentType.Id, 1, dto);

          // Assert
   result.Name.Should().Be("Article Author");
      result.FieldKey.Should().Be("article_author");
            result.IsRequired.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteFieldAsync_WithValidId_ShouldDeleteField()
    {
            // Arrange
       var contentType = new ContentType 
            { 
      Name = "News", 
                Slug = "news",
   Fields = new List<ContentTypeField>
    {
      new ContentTypeField 
            { 
             Id = 1,
                Name = "Author", 
 FieldKey = "author", 
  FieldType = "Text" 
        }
     }
 };
   await _context.ContentTypes.AddAsync(contentType);
            await _context.SaveChangesAsync();

 // Act
      await _service.DeleteFieldAsync(contentType.Id, 1);

            // Assert
            var updated = await _context.ContentTypes
   .Include(ct => ct.Fields)
     .FirstAsync(ct => ct.Id == contentType.Id);
       updated.Fields.Should().BeEmpty();
        }

        [Fact]
        public async Task ReorderFieldsAsync_ShouldUpdateFieldOrders()
 {
     // Arrange
            var contentType = new ContentType 
     { 
     Name = "News", 
            Slug = "news",
              Fields = new List<ContentTypeField>
        {
        new ContentTypeField { Id = 1, Name = "Field1", FieldKey = "field1", FieldType = "Text", DisplayOrder = 0 },
 new ContentTypeField { Id = 2, Name = "Field2", FieldKey = "field2", FieldType = "Text", DisplayOrder = 1 },
          new ContentTypeField { Id = 3, Name = "Field3", FieldKey = "field3", FieldType = "Text", DisplayOrder = 2 }
    }
  };
    await _context.ContentTypes.AddAsync(contentType);
       await _context.SaveChangesAsync();

         var newOrder = new List<int> { 3, 1, 2 };

 // Act
            await _service.ReorderFieldsAsync(contentType.Id, newOrder);

            // Assert
         var updated = await _context.ContentTypes
       .Include(ct => ct.Fields)
        .FirstAsync(ct => ct.Id == contentType.Id);
 
      updated.Fields.First(f => f.Id == 3).DisplayOrder.Should().Be(0);
            updated.Fields.First(f => f.Id == 1).DisplayOrder.Should().Be(1);
        updated.Fields.First(f => f.Id == 2).DisplayOrder.Should().Be(2);
      }

        public void Dispose()
 {
       _context.Database.EnsureDeleted();
        _context.Dispose();
        }
    }
}
