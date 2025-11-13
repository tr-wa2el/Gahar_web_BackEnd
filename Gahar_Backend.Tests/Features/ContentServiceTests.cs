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
    public class ContentServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IContentRepository _contentRepository;
    private readonly IContentTypeRepository _contentTypeRepository;
        private readonly ITagRepository _tagRepository;
        private readonly Mock<IAuditLogService> _mockAuditLogService;
        private readonly IContentService _contentService;

        public ContentServiceTests()
      {
            // Setup In-Memory Database
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    .Options;

        _context = new ApplicationDbContext(options);

    // Seed test data
     SeedTestData();

    // Setup repositories
            _contentRepository = new ContentRepository(_context);
       _contentTypeRepository = new ContentTypeRepository(_context);
          _tagRepository = new TagRepository(_context);

     // Setup mock services
      _mockAuditLogService = new Mock<IAuditLogService>();

          // Create service
        _contentService = new ContentService(
    _contentRepository,
         _contentTypeRepository,
    _tagRepository,
       _mockAuditLogService.Object,
           _context
   );
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

 var englishLanguage = new Language
       {
   Id = 2,
        Code = "en",
Name = "English",
         IsDefault = false,
     IsActive = true
   };

          _context.Languages.AddRange(arabicLanguage, englishLanguage);

            // Add content type
            var contentType = new ContentType
 {
       Id = 1,
                Name = "Article",
    Slug = "article",
           Description = "Article content type",
     IsActive = true,
      Icon = "FileText"
            };

  _context.ContentTypes.Add(contentType);

   // Add tags
  var tag1 = new Tag
            {
 Id = 1,
        Name = "Health",
   Slug = "health",
 Description = "Health related articles",
Color = "#00A86B",
      UsageCount = 0
            };

         var tag2 = new Tag
    {
     Id = 2,
      Name = "Technology",
        Slug = "technology",
     Description = "Technology articles",
   Color = "#0066CC",
          UsageCount = 0
    };

            _context.Tags.AddRange(tag1, tag2);

     // Add user
     var user = new User
            {
      Id = 1,
 Username = "testuser",
       Email = "test@example.com",
    FirstName = "Test",
        LastName = "User",
     PasswordHash = "hashedpassword",
         IsActive = true
          };

 _context.Users.Add(user);

  _context.SaveChanges();
  }

     [Fact]
        public async Task CreateAsync_WithValidData_ShouldCreateContent()
    {
            // Arrange
 var createDto = new CreateContentDto
{
   ContentTypeId = 1,
       Title = "Test Article",
       Slug = "test-article",
  Summary = "Test summary",
     Body = "<p>Test body</p>",
 Status = "Draft",
       IsFeatured = false,
    AllowComments = true,
    MetaTitle = "Test Meta Title",
         MetaDescription = "Test meta description"
            };

         // Act
    var result = await _contentService.CreateAsync(createDto, 1);

        // Assert
result.Should().NotBeNull();
 result.Id.Should().BeGreaterThan(0);
            result.Title.Should().Be("Test Article");
            result.Slug.Should().Be("test-article");
result.Status.Should().Be("Draft");
         result.AuthorName.Should().Be("Test User");
     }

        [Fact]
 public async Task CreateAsync_WithDuplicateSlug_ShouldThrowBadRequestException()
 {
            // Arrange
            var createDto1 = new CreateContentDto
 {
      ContentTypeId = 1,
      Title = "First Article",
       Slug = "duplicate-slug",
       Status = "Draft"
};

      await _contentService.CreateAsync(createDto1, 1);

            var createDto2 = new CreateContentDto
    {
            ContentTypeId = 1,
  Title = "Second Article",
    Slug = "duplicate-slug",
         Status = "Draft"
        };

       // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
     async () => await _contentService.CreateAsync(createDto2, 1)
            );
    }

      [Fact]
        public async Task CreateAsync_WithInvalidContentType_ShouldThrowNotFoundException()
        {
            // Arrange
            var createDto = new CreateContentDto
   {
   ContentTypeId = 999,
    Title = "Test Article",
      Slug = "test-article",
    Status = "Draft"
        };

 // Act & Assert
     await Assert.ThrowsAsync<NotFoundException>(
         async () => await _contentService.CreateAsync(createDto, 1)
  );
        }

        [Fact]
        public async Task CreateAsync_WithPublishedStatus_ShouldSetPublishedAt()
        {
            // Arrange
          var createDto = new CreateContentDto
        {
           ContentTypeId = 1,
      Title = "Published Article",
   Slug = "published-article",
     Status = "Published"
            };

            var beforeCreate = DateTime.UtcNow;

 // Act
  var result = await _contentService.CreateAsync(createDto, 1);

     // Assert
 result.PublishedAt.Should().NotBeNull();
 result.PublishedAt.Should().BeOnOrAfter(beforeCreate);
     result.Status.Should().Be("Published");
  }

      [Fact]
        public async Task CreateAsync_WithTags_ShouldAssignTags()
        {
     // Arrange
var createDto = new CreateContentDto
      {
   ContentTypeId = 1,
                Title = "Tagged Article",
           Slug = "tagged-article",
  Status = "Draft",
         TagIds = new List<int> { 1, 2 }
         };

            // Act
    var result = await _contentService.CreateAsync(createDto, 1);

            // Assert
            result.Tags.Should().HaveCount(2);
       result.Tags.Select(t => t.Id).Should().Contain(new[] { 1, 2 });

            // Verify tag usage counts were incremented
        var tag1 = await _context.Tags.FindAsync(1);
        var tag2 = await _context.Tags.FindAsync(2);
 tag1.UsageCount.Should().Be(1);
   tag2.UsageCount.Should().Be(1);
}

      [Fact]
        public async Task CreateAsync_WithCustomFields_ShouldSaveCustomFields()
        {
            // Arrange
var customFields = new Dictionary<string, object>
      {
     { "author_bio", "Test author bio" },
              { "reading_time", 5 },
     { "difficulty", "beginner" }
         };

   var createDto = new CreateContentDto
     {
      ContentTypeId = 1,
              Title = "Custom Fields Article",
    Slug = "custom-fields-article",
         Status = "Draft",
    CustomFields = customFields
     };

            // Act
          var result = await _contentService.CreateAsync(createDto, 1);

       // Assert
        result.CustomFields.Should().HaveCount(3);
          result.CustomFields.Should().ContainKey("author_bio");
 result.CustomFields.Should().ContainKey("reading_time");
        }

    [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnContent()
        {
    // Arrange
       var createDto = new CreateContentDto
            {
              ContentTypeId = 1,
         Title = "Get Test Article",
    Slug = "get-test-article",
             Status = "Published"
  };

 var created = await _contentService.CreateAsync(createDto, 1);

   // Act
            var result = await _contentService.GetByIdAsync(created.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(created.Id);
     result.Title.Should().Be("Get Test Article");
    }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldThrowNotFoundException()
        {
 // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
  async () => await _contentService.GetByIdAsync(999)
         );
        }

        [Fact]
        public async Task GetBySlugAsync_WithValidSlug_ShouldReturnContent()
        {
        // Arrange
 var createDto = new CreateContentDto
          {
   ContentTypeId = 1,
       Title = "Slug Test Article",
        Slug = "slug-test-article",
          Status = "Published"
            };

            await _contentService.CreateAsync(createDto, 1);

            // Act
   var result = await _contentService.GetBySlugAsync("slug-test-article", 1);

// Assert
            result.Should().NotBeNull();
            result.Slug.Should().Be("slug-test-article");
      }

        [Fact]
        public async Task GetBySlugAsync_WithInvalidSlug_ShouldThrowNotFoundException()
        {
  // Act & Assert
   await Assert.ThrowsAsync<NotFoundException>(
   async () => await _contentService.GetBySlugAsync("non-existent-slug", 1)
    );
        }

        [Fact]
   public async Task UpdateAsync_WithValidData_ShouldUpdateContent()
        {
        // Arrange
    var createDto = new CreateContentDto
  {
ContentTypeId = 1,
    Title = "Original Title",
      Slug = "original-slug",
              Status = "Draft"
   };

var created = await _contentService.CreateAsync(createDto, 1);

            var updateDto = new UpdateContentDto
         {
          ContentTypeId = 1,
    Title = "Updated Title",
        Slug = "updated-slug",
              Status = "Published",
 Summary = "Updated summary"
            };

       // Act
            var result = await _contentService.UpdateAsync(created.Id, updateDto, 1);

    // Assert
    result.Should().NotBeNull();
        result.Title.Should().Be("Updated Title");
      result.Slug.Should().Be("updated-slug");
     result.Status.Should().Be("Published");
            result.Summary.Should().Be("Updated summary");
  }

    [Fact]
      public async Task UpdateAsync_ChangingToDraftToPublished_ShouldSetPublishedAt()
      {
    // Arrange
   var createDto = new CreateContentDto
       {
         ContentTypeId = 1,
      Title = "Draft Article",
            Slug = "draft-article",
Status = "Draft"
   };

       var created = await _contentService.CreateAsync(createDto, 1);
     created.PublishedAt.Should().BeNull();

      var updateDto = new UpdateContentDto
      {
          ContentTypeId = 1,
    Title = "Draft Article",
                Slug = "draft-article",
     Status = "Published"
            };

     var beforeUpdate = DateTime.UtcNow;

  // Act
  var result = await _contentService.UpdateAsync(created.Id, updateDto, 1);

  // Assert
            result.PublishedAt.Should().NotBeNull();
            result.PublishedAt.Should().BeOnOrAfter(beforeUpdate);
        }

        [Fact]
        public async Task UpdateAsync_WithNewTags_ShouldUpdateTags()
        {
            // Arrange
            var createDto = new CreateContentDto
    {
          ContentTypeId = 1,
     Title = "Tag Update Article",
  Slug = "tag-update-article",
  Status = "Draft",
           TagIds = new List<int> { 1 }
       };

     var created = await _contentService.CreateAsync(createDto, 1);

     var updateDto = new UpdateContentDto
  {
    ContentTypeId = 1,
          Title = "Tag Update Article",
         Slug = "tag-update-article",
          Status = "Draft",
          TagIds = new List<int> { 2 }
            };

            // Act
   var result = await _contentService.UpdateAsync(created.Id, updateDto, 1);

  // Assert
  result.Tags.Should().HaveCount(1);
       result.Tags.First().Id.Should().Be(2);

      // Verify tag usage counts
            var tag1 = await _context.Tags.FindAsync(1);
            var tag2 = await _context.Tags.FindAsync(2);
            tag1.UsageCount.Should().Be(0); // Decremented
            tag2.UsageCount.Should().Be(1); // Incremented
      }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldSoftDeleteContent()
      {
      // Arrange
      var createDto = new CreateContentDto
            {
   ContentTypeId = 1,
       Title = "Delete Test Article",
    Slug = "delete-test-article",
       Status = "Draft",
    TagIds = new List<int> { 1 }
      };

            var created = await _contentService.CreateAsync(createDto, 1);

        // Act
         await _contentService.DeleteAsync(created.Id);

  // Assert
  var deletedContent = await _context.Contents
             .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == created.Id);

            deletedContent.Should().NotBeNull();
       deletedContent.IsDeleted.Should().BeTrue();

    // Verify tag usage count was decremented
        var tag = await _context.Tags.FindAsync(1);
            tag.UsageCount.Should().Be(0);
        }

    [Fact]
        public async Task DuplicateAsync_WithValidId_ShouldCreateCopy()
        {
            // Arrange
  var createDto = new CreateContentDto
     {
         ContentTypeId = 1,
    Title = "Original Article",
                Slug = "original-article",
        Summary = "Original summary",
      Body = "<p>Original body</p>",
    Status = "Published",
       IsFeatured = true,
       TagIds = new List<int> { 1, 2 }
   };

    var original = await _contentService.CreateAsync(createDto, 1);

            // Act
         var duplicate = await _contentService.DuplicateAsync(original.Id, 1);

     // Assert
 duplicate.Should().NotBeNull();
            duplicate.Id.Should().NotBe(original.Id);
       duplicate.Title.Should().Contain("(Copy)");
 duplicate.Slug.Should().Contain("-copy-");
  duplicate.Status.Should().Be("Draft");
duplicate.IsFeatured.Should().BeFalse();
   duplicate.Summary.Should().Be(original.Summary);
      duplicate.Body.Should().Be(original.Body);
      duplicate.Tags.Should().HaveCount(2);
        }

        [Fact]
      public async Task PublishAsync_WithDraftContent_ShouldPublish()
   {
    // Arrange
       var createDto = new CreateContentDto
   {
       ContentTypeId = 1,
          Title = "Publish Test Article",
          Slug = "publish-test-article",
           Status = "Draft"
     };

         var created = await _contentService.CreateAsync(createDto, 1);

            // Act
            await _contentService.PublishAsync(created.Id);

     // Assert
    var published = await _contentService.GetByIdAsync(created.Id);
          published.Status.Should().Be("Published");
     published.PublishedAt.Should().NotBeNull();
   }

        [Fact]
        public async Task UnpublishAsync_WithPublishedContent_ShouldUnpublish()
        {
// Arrange
      var createDto = new CreateContentDto
            {
   ContentTypeId = 1,
                Title = "Unpublish Test Article",
                Slug = "unpublish-test-article",
     Status = "Published"
            };

  var created = await _contentService.CreateAsync(createDto, 1);

    // Act
    await _contentService.UnpublishAsync(created.Id);

  // Assert
     var unpublished = await _contentService.GetByIdAsync(created.Id);
          unpublished.Status.Should().Be("Draft");
        }

        [Fact]
      public async Task ArchiveAsync_WithContent_ShouldArchive()
{
            // Arrange
  var createDto = new CreateContentDto
            {
       ContentTypeId = 1,
      Title = "Archive Test Article",
 Slug = "archive-test-article",
         Status = "Published"
    };

          var created = await _contentService.CreateAsync(createDto, 1);

            // Act
            await _contentService.ArchiveAsync(created.Id);

  // Assert
 var archived = await _contentService.GetByIdAsync(created.Id);
            archived.Status.Should().Be("Archived");
        }

        [Fact]
        public async Task GetAllAsync_WithFilters_ShouldReturnFilteredResults()
        {
  // Arrange
       await _contentService.CreateAsync(new CreateContentDto
 {
       ContentTypeId = 1,
Title = "Article 1",
  Slug = "article-1",
      Status = "Published",
          IsFeatured = true
    }, 1);

            await _contentService.CreateAsync(new CreateContentDto
            {
         ContentTypeId = 1,
           Title = "Article 2",
      Slug = "article-2",
  Status = "Draft",
       IsFeatured = false
          }, 1);

            await _contentService.CreateAsync(new CreateContentDto
       {
    ContentTypeId = 1,
           Title = "Article 3",
     Slug = "article-3",
    Status = "Published",
      IsFeatured = false
     }, 1);

            var filter = new ContentFilterDto
   {
          Status = "Published",
    Page = 1,
       PageSize = 10
            };

        // Act
 var result = await _contentService.GetAllAsync(filter);

        // Assert
            result.Should().NotBeNull();
  result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            result.Items.Should().OnlyContain(c => c.Status == "Published");
        }

  [Fact]
    public async Task GetAllAsync_WithSearchTerm_ShouldReturnMatchingResults()
        {
// Arrange
            await _contentService.CreateAsync(new CreateContentDto
            {
     ContentTypeId = 1,
          Title = "Health Article",
     Slug = "health-article",
 Summary = "About health",
    Status = "Published"
            }, 1);

            await _contentService.CreateAsync(new CreateContentDto
          {
        ContentTypeId = 1,
           Title = "Technology Article",
              Slug = "tech-article",
     Summary = "About technology",
                Status = "Published"
            }, 1);

 var filter = new ContentFilterDto
     {
        SearchTerm = "health",
     Page = 1,
PageSize = 10
            };

            // Act
       var result = await _contentService.GetAllAsync(filter);

        // Assert
         result.Items.Should().HaveCount(1);
            result.Items.First().Title.Should().Contain("Health");
        }

        [Fact]
  public async Task GetAllAsync_WithPagination_ShouldReturnPagedResults()
        {
     // Arrange
            for (int i = 1; i <= 15; i++)
    {
     await _contentService.CreateAsync(new CreateContentDto
     {
     ContentTypeId = 1,
      Title = $"Article {i}",
        Slug = $"article-{i}",
        Status = "Published"
                }, 1);
            }

            var filter = new ContentFilterDto
            {
      Page = 2,
     PageSize = 10
            };

     // Act
      var result = await _contentService.GetAllAsync(filter);

  // Assert
          result.Items.Should().HaveCount(5);
  result.TotalCount.Should().Be(15);
      result.Page.Should().Be(2);
         result.PageSize.Should().Be(10);
            result.TotalPages.Should().Be(2);
      }

        [Fact]
   public async Task GetFeaturedAsync_ShouldReturnOnlyFeaturedContent()
        {
 // Arrange
    await _contentService.CreateAsync(new CreateContentDto
     {
      ContentTypeId = 1,
        Title = "Featured 1",
       Slug = "featured-1",
           Status = "Published",
           IsFeatured = true
    }, 1);

      await _contentService.CreateAsync(new CreateContentDto
            {
      ContentTypeId = 1,
                Title = "Not Featured",
      Slug = "not-featured",
        Status = "Published",
          IsFeatured = false
          }, 1);

            await _contentService.CreateAsync(new CreateContentDto
{
     ContentTypeId = 1,
   Title = "Featured 2",
                Slug = "featured-2",
      Status = "Published",
    IsFeatured = true
   }, 1);

     // Act
    var result = await _contentService.GetFeaturedAsync(1, 10);

         // Assert
            result.Should().HaveCount(2);
       result.Should().OnlyContain(c => c.IsFeatured);
        }

        [Fact]
        public async Task GetRecentAsync_ShouldReturnMostRecentContent()
        {
  // Arrange
        var content1 = await _contentService.CreateAsync(new CreateContentDto
            {
            ContentTypeId = 1,
                Title = "Old Article",
 Slug = "old-article",
           Status = "Published"
            }, 1);

        await Task.Delay(100); // Ensure different timestamps

          var content2 = await _contentService.CreateAsync(new CreateContentDto
     {
        ContentTypeId = 1,
         Title = "Recent Article",
          Slug = "recent-article",
       Status = "Published"
     }, 1);

     // Act
       var result = await _contentService.GetRecentAsync(1, 10);

            // Assert
     result.Should().NotBeEmpty();
         result.First().Title.Should().Be("Recent Article");
        }

        [Fact]
  public async Task IncrementViewCountAsync_ShouldIncreaseViewCount()
      {
 // Arrange
            var createDto = new CreateContentDto
  {
            ContentTypeId = 1,
         Title = "View Count Article",
       Slug = "view-count-article",
                Status = "Published"
    };

            var created = await _contentService.CreateAsync(createDto, 1);
          var initialViewCount = created.ViewCount;

          // Act
       await _contentService.IncrementViewCountAsync(created.Id);
            await _contentService.IncrementViewCountAsync(created.Id);

            // Assert
      var updated = await _contentService.GetByIdAsync(created.Id);
  updated.ViewCount.Should().Be(initialViewCount + 2);
      }

        [Fact]
        public async Task ProcessScheduledContentAsync_ShouldPublishScheduledContent()
        {
     // Arrange
      var scheduledContent = await _contentService.CreateAsync(new CreateContentDto
            {
                ContentTypeId = 1,
        Title = "Scheduled Article",
     Slug = "scheduled-article",
   Status = "Scheduled",
       ScheduledAt = DateTime.UtcNow.AddMinutes(-5) // Past date
    }, 1);

   // Act
            await _contentService.ProcessScheduledContentAsync();

      // Assert
     var published = await _contentService.GetByIdAsync(scheduledContent.Id);
      published.Status.Should().Be("Published");
          published.PublishedAt.Should().NotBeNull();
        }

        public void Dispose()
   {
            _context?.Dispose();
        }
    }
}
