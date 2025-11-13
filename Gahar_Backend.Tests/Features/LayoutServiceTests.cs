using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Models.DTOs.Layout;
using Gahar_Backend.Services.Implementations;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Repositories.Implementations;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Tests.Features;

/// <summary>
/// Unit tests for LayoutService
/// </summary>
public class LayoutServiceTests : IDisposable
{
 private readonly ApplicationDbContext _context;
    private readonly ILayoutRepository _layoutRepository;
    private readonly ILayoutService _layoutService;

    public LayoutServiceTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
 .Options;

        _context = new ApplicationDbContext(options);
        _layoutRepository = new LayoutRepository(_context);
        _layoutService = new LayoutService(_layoutRepository);

   // Seed test data
    SeedTestData();
    }

    private void SeedTestData()
    {
        var layouts = new List<Layout>
        {
      new Layout
 {
    Id = 1,
                Name = "Standard Article",
        Description = "Default layout for articles",
Configuration = "{\"showAuthor\":true,\"showDate\":true,\"imageSize\":\"medium\"}",
    IsDefault = true,
                IsActive = true,
        CreatedAt = DateTime.UtcNow
          },
     new Layout
        {
      Id = 2,
     Name = "Featured Article",
     Description = "Layout for featured content",
       Configuration = "{\"showAuthor\":true,\"showDate\":true,\"imageSize\":\"large\"}",
           IsDefault = false,
         IsActive = true,
              CreatedAt = DateTime.UtcNow
        },
     new Layout
     {
          Id = 3,
                Name = "News List",
              Description = "Compact layout for news",
        Configuration = "{\"showAuthor\":false,\"showDate\":true,\"imageSize\":\"small\"}",
  IsDefault = false,
        IsActive = false,
           CreatedAt = DateTime.UtcNow
       }
};

   _context.Layouts.AddRange(layouts);
        _context.SaveChanges();
    }

    #region GetAllLayoutsAsync Tests

    [Fact]
    public async Task GetAllLayoutsAsync_ShouldReturnAllLayouts()
    {
        // Act
        var result = await _layoutService.GetAllLayoutsAsync();

// Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().AllBeOfType<LayoutDto>();
    }

    #endregion

    #region GetActiveLayoutsAsync Tests

    [Fact]
    public async Task GetActiveLayoutsAsync_ShouldReturnOnlyActiveLayouts()
    {
        // Act
        var result = await _layoutService.GetActiveLayoutsAsync();

        // Assert
  result.Should().NotBeNull();
    result.Should().HaveCount(2);
        result.Should().AllSatisfy(l => l.IsActive.Should().BeTrue());
    }

    [Fact]
    public async Task GetActiveLayoutsAsync_ShouldReturnDefaultFirst()
    {
 // Act
        var result = (await _layoutService.GetActiveLayoutsAsync()).ToList();

        // Assert
   result.Should().NotBeEmpty();
        result.First().IsDefault.Should().BeTrue();
        result.First().Name.Should().Be("Standard Article");
    }

    #endregion

    #region GetLayoutByIdAsync Tests

    [Fact]
    public async Task GetLayoutByIdAsync_WithValidId_ShouldReturnLayout()
    {
        // Arrange
        var layoutId = 1;

        // Act
   var result = await _layoutService.GetLayoutByIdAsync(layoutId);

    // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(layoutId);
        result.Name.Should().Be("Standard Article");
    result.Configuration.Should().NotBeNull();
    }

    [Fact]
    public async Task GetLayoutByIdAsync_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
     var invalidId = 999;

        // Act
        Func<Task> act = async () => await _layoutService.GetLayoutByIdAsync(invalidId);

     // Assert
     await act.Should().ThrowAsync<NotFoundException>()
        .WithMessage($"Layout with ID {invalidId} not found");
  }

    #endregion

    #region GetLayoutWithStatsAsync Tests

    [Fact]
public async Task GetLayoutWithStatsAsync_ShouldReturnLayoutWithContentCount()
    {
  // Arrange
        var layoutId = 1;

      // Act
var result = await _layoutService.GetLayoutWithStatsAsync(layoutId);

     // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LayoutWithStatsDto>();
        result.Id.Should().Be(layoutId);
        result.ContentCount.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task GetLayoutWithStatsAsync_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
     var invalidId = 999;

        // Act
        Func<Task> act = async () => await _layoutService.GetLayoutWithStatsAsync(invalidId);

   // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    #endregion

    #region GetDefaultLayoutAsync Tests

    [Fact]
    public async Task GetDefaultLayoutAsync_ShouldReturnDefaultLayout()
    {
        // Act
        var result = await _layoutService.GetDefaultLayoutAsync();

        // Assert
        result.Should().NotBeNull();
        result!.IsDefault.Should().BeTrue();
        result.Name.Should().Be("Standard Article");
    }

    #endregion

    #region CreateLayoutAsync Tests

    [Fact]
    public async Task CreateLayoutAsync_WithValidData_ShouldCreateLayout()
    {
        // Arrange
  var createDto = new CreateLayoutDto
        {
        Name = "Blog Post",
            Description = "Layout for blog posts",
  Configuration = new { showAuthor = true, showComments = true },
         IsActive = true,
            PreviewImage = "/images/blog-preview.jpg"
 };

        // Act
        var result = await _layoutService.CreateLayoutAsync(createDto);

 // Assert
   result.Should().NotBeNull();
        result.Name.Should().Be(createDto.Name);
        result.Description.Should().Be(createDto.Description);
        result.IsActive.Should().BeTrue();
        result.IsDefault.Should().BeFalse();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateLayoutAsync_WithDuplicateName_ShouldThrowValidationException()
    {
        // Arrange
      var createDto = new CreateLayoutDto
        {
            Name = "Standard Article", // Duplicate name
  Description = "Test",
    Configuration = new { test = true },
            IsActive = true
        };

        // Act
        Func<Task> act = async () => await _layoutService.CreateLayoutAsync(createDto);

        // Assert
      await act.Should().ThrowAsync<ValidationException>()
     .WithMessage("*already exists*");
}

    [Fact]
    public async Task CreateLayoutAsync_WithInvalidConfiguration_ShouldThrowValidationException()
    {
        // Arrange
        var createDto = new CreateLayoutDto
        {
       Name = "Invalid Layout",
         Description = "Test",
   Configuration = null!, // Invalid configuration
       IsActive = true
      };

        // Act
    Func<Task> act = async () => await _layoutService.CreateLayoutAsync(createDto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
     .WithMessage("*configuration*");
    }

    #endregion

    #region UpdateLayoutAsync Tests

    [Fact]
    public async Task UpdateLayoutAsync_WithValidData_ShouldUpdateLayout()
    {
        // Arrange
        var layoutId = 2;
        var updateDto = new UpdateLayoutDto
        {
            Name = "Updated Featured Article",
   Description = "Updated description",
            Configuration = new { showAuthor = false, imageSize = "xlarge" },
            IsActive = true,
         PreviewImage = "/images/updated.jpg"
        };

   // Act
     var result = await _layoutService.UpdateLayoutAsync(layoutId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(layoutId);
        result.Name.Should().Be(updateDto.Name);
     result.Description.Should().Be(updateDto.Description);
      result.PreviewImage.Should().Be(updateDto.PreviewImage);
    }

    [Fact]
    public async Task UpdateLayoutAsync_WithInvalidId_ShouldThrowNotFoundException()
{
        // Arrange
        var invalidId = 999;
        var updateDto = new UpdateLayoutDto
  {
            Name = "Test",
          Configuration = new { test = true },
        IsActive = true
        };

  // Act
        Func<Task> act = async () => await _layoutService.UpdateLayoutAsync(invalidId, updateDto);

    // Assert
   await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
public async Task UpdateLayoutAsync_WithDuplicateName_ShouldThrowValidationException()
    {
        // Arrange
   var layoutId = 2;
        var updateDto = new UpdateLayoutDto
        {
   Name = "Standard Article", // Name of another layout
      Configuration = new { test = true },
            IsActive = true
        };

        // Act
        Func<Task> act = async () => await _layoutService.UpdateLayoutAsync(layoutId, updateDto);

   // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*already exists*");
    }

    #endregion

    #region DeleteLayoutAsync Tests

 [Fact]
    public async Task DeleteLayoutAsync_WithValidId_ShouldDeleteLayout()
    {
     // Arrange
        var layoutId = 2; // Non-default, no content

     // Act
        var result = await _layoutService.DeleteLayoutAsync(layoutId);

  // Assert
      result.Should().BeTrue();

    // Verify soft deletion by checking GetAllLayoutsAsync
        var allLayouts = await _layoutService.GetAllLayoutsAsync();
      allLayouts.Should().NotContain(l => l.Id == layoutId);
    }

    [Fact]
    public async Task DeleteLayoutAsync_WithDefaultLayout_ShouldThrowValidationException()
    {
        // Arrange
     var defaultLayoutId = 1;

        // Act
      Func<Task> act = async () => await _layoutService.DeleteLayoutAsync(defaultLayoutId);

     // Assert
        await act.Should().ThrowAsync<ValidationException>()
    .WithMessage("*default layout*");
    }

 [Fact]
    public async Task DeleteLayoutAsync_WithInvalidId_ShouldThrowNotFoundException()
    {
      // Arrange
    var invalidId = 999;

        // Act
        Func<Task> act = async () => await _layoutService.DeleteLayoutAsync(invalidId);

     // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    #endregion

    #region SetAsDefaultAsync Tests

    [Fact]
    public async Task SetAsDefaultAsync_WithActiveLayout_ShouldSetAsDefault()
    {
        // Arrange
      var layoutId = 2; // Active non-default layout

  // Act
  await _layoutService.SetAsDefaultAsync(layoutId);

        // Assert
        var defaultLayout = await _layoutService.GetDefaultLayoutAsync();
        defaultLayout.Should().NotBeNull();
        defaultLayout!.Id.Should().Be(layoutId);

        // Verify old default is no longer default
        var oldDefault = await _layoutService.GetLayoutByIdAsync(1);
        oldDefault.IsDefault.Should().BeFalse();
  }

    [Fact]
    public async Task SetAsDefaultAsync_WithInactiveLayout_ShouldThrowValidationException()
    {
        // Arrange
    var inactiveLayoutId = 3;

     // Act
        Func<Task> act = async () => await _layoutService.SetAsDefaultAsync(inactiveLayoutId);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
          .WithMessage("*inactive layout*");
    }

    [Fact]
    public async Task SetAsDefaultAsync_WithInvalidId_ShouldThrowNotFoundException()
    {
   // Arrange
        var invalidId = 999;

        // Act
        Func<Task> act = async () => await _layoutService.SetAsDefaultAsync(invalidId);

//
// Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task SetAsDefaultAsync_ShouldEnsureOnlyOneDefault()
    {
        // Arrange
        var newDefaultId = 2;

 // Act
        await _layoutService.SetAsDefaultAsync(newDefaultId);

        // Assert
        var allLayouts = await _layoutService.GetAllLayoutsAsync();
var defaultLayouts = allLayouts.Where(l => l.IsDefault).ToList();

        defaultLayouts.Should().HaveCount(1);
        defaultLayouts.First().Id.Should().Be(newDefaultId);
    }

    #endregion

    #region ValidateConfiguration Tests

    [Fact]
    public void ValidateConfiguration_WithValidJson_ShouldReturnTrue()
    {
        // Arrange
        var validConfig = new { showAuthor = true, imageSize = "large" };

  // Act
        var result = _layoutService.ValidateConfiguration(validConfig);

   // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ValidateConfiguration_WithEmptyObject_ShouldReturnTrue()
    {
        // Arrange
        var emptyConfig = new { };

        // Act
        var result = _layoutService.ValidateConfiguration(emptyConfig);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ValidateConfiguration_WithComplexObject_ShouldReturnTrue()
    {
      // Arrange
        var complexConfig = new
        {
    template = "blog-post",
            settings = new
  {
    showAuthor = true,
        showDate = true,
  widgets = new[] { "recent", "popular" }
        }
        };

        // Act
     var result = _layoutService.ValidateConfiguration(complexConfig);

        // Assert
        result.Should().BeTrue();
    }

  #endregion

    #region Integration Tests

    [Fact]
    public async Task FullWorkflow_CreateUpdateSetDefaultDelete_ShouldWorkCorrectly()
    {
       // 1. Create new layout
    var createDto = new CreateLayoutDto
     {
         Name = "Workflow Test Layout",
    Description = "Test layout",
         Configuration = new { test = true },
      IsActive = true
       };

       var created = await _layoutService.CreateLayoutAsync(createDto);
  created.Should().NotBeNull();
    created.Name.Should().Be(createDto.Name);

 // 2. Update layout
         var updateDto = new UpdateLayoutDto
       {
Name = "Updated Workflow Layout",
 Description = "Updated description",
       Configuration = new { test = false, updated = true },
      IsActive = true
      };

   var updated = await _layoutService.UpdateLayoutAsync(created.Id, updateDto);
      updated.Name.Should().Be(updateDto.Name);

      // 3. Set as default
   await _layoutService.SetAsDefaultAsync(created.Id);
      var defaultLayout = await _layoutService.GetDefaultLayoutAsync();
  defaultLayout!.Id.Should().Be(created.Id);

  // 4. Set another as default (so we can delete this one)
  await _layoutService.SetAsDefaultAsync(1);

    // 5. Delete layout
      var deleted = await _layoutService.DeleteLayoutAsync(created.Id);
 deleted.Should().BeTrue();

        // Verify deletion through GetAllLayoutsAsync
     var allLayouts = await _layoutService.GetAllLayoutsAsync();
   allLayouts.Should().NotContain(l => l.Id == created.Id);
    }

    #endregion

    public void Dispose()
    {
_context?.Dispose();
    }
}
