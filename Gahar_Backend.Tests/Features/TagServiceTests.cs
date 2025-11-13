using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Models.DTOs.Content;
using Gahar_Backend.Services.Implementations;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Repositories.Implementations;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Tests.Features
{
    public class TagServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ITagRepository _tagRepository;
        private readonly Mock<IAuditLogService> _auditLogServiceMock;
  private readonly ITagService _tagService;

        public TagServiceTests()
        {
      // Setup In-Memory Database
     var options = new DbContextOptionsBuilder<ApplicationDbContext>()
  .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    .Options;

_context = new ApplicationDbContext(options);

   // Seed test data
            SeedTestData();

            // Setup repository and service
        _tagRepository = new TagRepository(_context);
            _auditLogServiceMock = new Mock<IAuditLogService>();
          _tagService = new TagService(_tagRepository, _auditLogServiceMock.Object);
      }

        private void SeedTestData()
  {
        // Add languages
       var arabicLanguage = new Language
            {
                Id = 1,
         Code = "ar",
       Name = "Arabic",
     IsDefault = true,
     IsActive = true
            };

   _context.Languages.Add(arabicLanguage);
            _context.SaveChanges();
        }

        [Fact]
        public async Task CreateAsync_WithValidData_ShouldCreateTag()
        {
     // Arrange
         var createDto = new CreateTagDto
      {
      Name = "Health",
      Slug = "health",
                Description = "Health related content",
   Color = "#00A86B"
            };

            // Act
  var result = await _tagService.CreateAsync(createDto);

 // Assert
    result.Should().NotBeNull();
   result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("Health");
      result.Slug.Should().Be("health");
            result.Description.Should().Be("Health related content");
            result.Color.Should().Be("#00A86B");
 result.UsageCount.Should().Be(0);
        }

        [Fact]
        public async Task CreateAsync_WithDuplicateSlug_ShouldThrowBadRequestException()
        {
            // Arrange
            var createDto1 = new CreateTagDto
    {
              Name = "Tag 1",
         Slug = "duplicate-slug",
                Description = "First tag"
  };

 await _tagService.CreateAsync(createDto1);

  var createDto2 = new CreateTagDto
     {
                Name = "Tag 2",
    Slug = "duplicate-slug",
                Description = "Second tag"
   };

   // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(
    async () => await _tagService.CreateAsync(createDto2)
     );
        }

        [Fact]
        public async Task CreateAsync_WithoutColor_ShouldCreateTagSuccessfully()
        {
         // Arrange
          var createDto = new CreateTagDto
    {
                Name = "No Color Tag",
                Slug = "no-color-tag",
     Description = "Tag without color"
        };

  // Act
        var result = await _tagService.CreateAsync(createDto);

         // Assert
            result.Should().NotBeNull();
  result.Color.Should().BeNull();
        }

   [Fact]
        public async Task GetAllAsync_ShouldReturnAllTags()
        {
        // Arrange
            await _tagService.CreateAsync(new CreateTagDto
          {
          Name = "Tag 1",
       Slug = "tag-1"
      });

    await _tagService.CreateAsync(new CreateTagDto
        {
    Name = "Tag 2",
     Slug = "tag-2"
            });

            await _tagService.CreateAsync(new CreateTagDto
    {
       Name = "Tag 3",
    Slug = "tag-3"
  });

            // Act
            var result = await _tagService.GetAllAsync();

   // Assert
   result.Should().HaveCount(3);
        }

     [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnTag()
        {
     // Arrange
 var created = await _tagService.CreateAsync(new CreateTagDto
            {
         Name = "Get Test Tag",
   Slug = "get-test-tag"
            });

// Act
var result = await _tagService.GetByIdAsync(created.Id);

            // Assert
            result.Should().NotBeNull();
  result.Id.Should().Be(created.Id);
         result.Name.Should().Be("Get Test Tag");
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldThrowNotFoundException()
        {
// Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
         async () => await _tagService.GetByIdAsync(999)
            );
        }

        [Fact]
        public async Task GetBySlugAsync_WithValidSlug_ShouldReturnTag()
        {
      // Arrange
            await _tagService.CreateAsync(new CreateTagDto
            {
     Name = "Slug Test Tag",
       Slug = "slug-test-tag"
        });

            // Act
          var result = await _tagService.GetBySlugAsync("slug-test-tag");

     // Assert
       result.Should().NotBeNull();
          result.Slug.Should().Be("slug-test-tag");
     }

        [Fact]
        public async Task GetBySlugAsync_WithInvalidSlug_ShouldThrowNotFoundException()
 {
       // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
          async () => await _tagService.GetBySlugAsync("non-existent-slug")
 );
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldUpdateTag()
      {
   // Arrange
   var created = await _tagService.CreateAsync(new CreateTagDto
  {
           Name = "Original Name",
        Slug = "original-slug",
       Description = "Original description",
         Color = "#FF0000"
            });

        var updateDto = new UpdateTagDto
     {
     Name = "Updated Name",
       Slug = "updated-slug",
          Description = "Updated description",
        Color = "#00FF00"
      };

            // Act
            var result = await _tagService.UpdateAsync(created.Id, updateDto);

 // Assert
       result.Should().NotBeNull();
     result.Name.Should().Be("Updated Name");
            result.Slug.Should().Be("updated-slug");
            result.Description.Should().Be("Updated description");
result.Color.Should().Be("#00FF00");
        }

   [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldThrowNotFoundException()
      {
    // Arrange
            var updateDto = new UpdateTagDto
            {
                Name = "Updated Name",
    Slug = "updated-slug"
            };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
        async () => await _tagService.UpdateAsync(999, updateDto)
            );
    }

        [Fact]
        public async Task UpdateAsync_WithDuplicateSlug_ShouldThrowBadRequestException()
        {
      // Arrange
            await _tagService.CreateAsync(new CreateTagDto
            {
    Name = "Tag 1",
     Slug = "existing-slug"
  });

       var tag2 = await _tagService.CreateAsync(new CreateTagDto
            {
Name = "Tag 2",
        Slug = "tag-2"
          });

  var updateDto = new UpdateTagDto
      {
        Name = "Tag 2 Updated",
    Slug = "existing-slug" // Try to use existing slug
            };

         // Act & Assert
    await Assert.ThrowsAsync<BadRequestException>(
   async () => await _tagService.UpdateAsync(tag2.Id, updateDto)
      );
        }

      [Fact]
 public async Task DeleteAsync_WithValidId_ShouldSoftDeleteTag()
     {
      // Arrange
            var created = await _tagService.CreateAsync(new CreateTagDto
            {
        Name = "Delete Test Tag",
        Slug = "delete-test-tag"
 });

        // Act
    await _tagService.DeleteAsync(created.Id);

    // Assert
   var deletedTag = await _context.Tags
                .IgnoreQueryFilters()
.FirstOrDefaultAsync(t => t.Id == created.Id);

   deletedTag.Should().NotBeNull();
            deletedTag.IsDeleted.Should().BeTrue();
        }

        [Fact]
  public async Task DeleteAsync_WithInvalidId_ShouldThrowNotFoundException()
        {
// Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
       async () => await _tagService.DeleteAsync(999)
       );
   }

        [Fact]
        public async Task GetPopularAsync_ShouldReturnTagsSortedByUsageCount()
        {
      // Arrange
      var tag1 = await _tagService.CreateAsync(new CreateTagDto
    {
  Name = "Tag 1",
       Slug = "tag-1"
    });

            var tag2 = await _tagService.CreateAsync(new CreateTagDto
    {
           Name = "Tag 2",
           Slug = "tag-2"
          });

var tag3 = await _tagService.CreateAsync(new CreateTagDto
   {
              Name = "Tag 3",
        Slug = "tag-3"
            });

    // Manually set usage counts
            var dbTag1 = await _context.Tags.FindAsync(tag1.Id);
            var dbTag2 = await _context.Tags.FindAsync(tag2.Id);
            var dbTag3 = await _context.Tags.FindAsync(tag3.Id);

  dbTag1.UsageCount = 5;
            dbTag2.UsageCount = 10;
          dbTag3.UsageCount = 3;

         await _context.SaveChangesAsync();

            // Act
   var result = await _tagService.GetPopularAsync(2);

         // Assert
    result.Should().HaveCount(2);
    result.First().Name.Should().Be("Tag 2"); // Most used
  result.First().UsageCount.Should().Be(10);
    result.Last().Name.Should().Be("Tag 1"); // Second most used
        result.Last().UsageCount.Should().Be(5);
}

        [Fact]
        public async Task GetPopularAsync_WithNoTags_ShouldReturnEmptyList()
        {
   // Act
        var result = await _tagService.GetPopularAsync(10);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchAsync_WithMatchingTerm_ShouldReturnMatchingTags()
        {
        // Arrange
        await _tagService.CreateAsync(new CreateTagDto
    {
  Name = "Health Care",
   Slug = "health-care"
      });

          await _tagService.CreateAsync(new CreateTagDto
            {
      Name = "Healthcare Technology",
    Slug = "healthcare-tech"
   });

            await _tagService.CreateAsync(new CreateTagDto
            {
       Name = "Education",
              Slug = "education"
  });

        // Act
        var result = await _tagService.SearchAsync("health");

            // Assert
  result.Should().HaveCount(2);
            result.Should().OnlyContain(t => t.Name.ToLower().Contains("health"));
        }

  [Fact]
        public async Task SearchAsync_WithNoMatches_ShouldReturnEmptyList()
        {
            // Arrange
         await _tagService.CreateAsync(new CreateTagDto
     {
      Name = "Technology",
            Slug = "technology"
   });

            // Act
  var result = await _tagService.SearchAsync("nonexistent");

        // Assert
        result.Should().BeEmpty();
        }

      [Fact]
        public async Task SearchAsync_WithEmptyTerm_ShouldReturnAllTags()
        {
            // Arrange
      await _tagService.CreateAsync(new CreateTagDto
        {
    Name = "Tag 1",
                Slug = "tag-1"
    });

            await _tagService.CreateAsync(new CreateTagDto
            {
    Name = "Tag 2",
       Slug = "tag-2"
            });

            // Act
            var result = await _tagService.SearchAsync("");

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
      public async Task SearchAsync_CaseInsensitive_ShouldReturnMatches()
        {
     // Arrange
   await _tagService.CreateAsync(new CreateTagDto
{
          Name = "TECHNOLOGY",
            Slug = "technology"
            });

   await _tagService.CreateAsync(new CreateTagDto
      {
  Name = "technology news",
             Slug = "tech-news"
            });

 // Act
            var result = await _tagService.SearchAsync("TECH");

      // Assert
  result.Should().HaveCount(2);
        }

  [Fact]
        public async Task CreateAsync_WithSpecialCharactersInName_ShouldCreateSuccessfully()
        {
       // Arrange
      var createDto = new CreateTagDto
     {
      Name = "الصحة العامة", // Arabic text
    Slug = "public-health",
 Description = "عن الصحة العامة"
            };

  // Act
            var result = await _tagService.CreateAsync(createDto);

            // Assert
       result.Should().NotBeNull();
         result.Name.Should().Be("الصحة العامة");
   }

        [Fact]
        public async Task UpdateAsync_PreservingUsageCount_ShouldNotResetCount()
        {
          // Arrange
            var created = await _tagService.CreateAsync(new CreateTagDto
    {
             Name = "Usage Test Tag",
     Slug = "usage-test"
    });

// Set usage count
        var dbTag = await _context.Tags.FindAsync(created.Id);
       dbTag.UsageCount = 25;
     await _context.SaveChangesAsync();

       var updateDto = new UpdateTagDto
    {
  Name = "Updated Usage Test",
     Slug = "updated-usage-test"
};

     // Act
         var result = await _tagService.UpdateAsync(created.Id, updateDto);

            // Assert
            result.UsageCount.Should().Be(25); // Should preserve usage count
   }

        [Fact]
        public async Task GetAllAsync_WithDeletedTags_ShouldNotIncludeDeleted()
   {
 // Arrange
            var tag1 = await _tagService.CreateAsync(new CreateTagDto
      {
Name = "Active Tag",
       Slug = "active-tag"
            });

      var tag2 = await _tagService.CreateAsync(new CreateTagDto
            {
      Name = "To Delete Tag",
    Slug = "to-delete-tag"
  });

  await _tagService.DeleteAsync(tag2.Id);

 // Act
  var result = await _tagService.GetAllAsync();

       // Assert
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Active Tag");
  }

     public void Dispose()
     {
      _context?.Dispose();
        }
    }
}
