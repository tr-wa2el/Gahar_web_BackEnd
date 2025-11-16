using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Models.DTOs.Album;
using Gahar_Backend.Services.Implementations;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Repositories.Implementations;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Utilities.Exceptions;
using Microsoft.Extensions.Logging;

namespace Gahar_Backend.Tests.Features
{
    public class AlbumServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
  private readonly IAlbumRepository _albumRepository;
        private readonly IMediaRepository _mediaRepository;
        private readonly Mock<ILogger<AlbumService>> _loggerMock;
        private readonly IAlbumService _albumService;

        public AlbumServiceTests()
        {
  // Setup In-Memory Database
     var options = new DbContextOptionsBuilder<ApplicationDbContext>()
     .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

      _context = new ApplicationDbContext(options);

            // Setup repositories and service
            _albumRepository = new AlbumRepository(_context);
          _mediaRepository = new MediaRepository(_context);
            _loggerMock = new Mock<ILogger<AlbumService>>();

            _albumService = new AlbumService(
     _albumRepository,
                _mediaRepository,
                _loggerMock.Object);

            // Seed test data
          SeedTestData();
        }

        private void SeedTestData()
   {
            // Create users
       var user = new User
        {
      Id = 1,
     FirstName = "Test",
    LastName = "User",
                Email = "test@example.com",
   PasswordHash = "hash",
 IsActive = true,
        CreatedAt = DateTime.UtcNow
    };

            _context.Users.Add(user);

   // Create media
 var media = new List<Media>
        {
new Media
                {
       Id = 1,
 FileName = "image1.jpg",
       FilePath = "/uploads/image1.jpg",
            ThumbnailPath = "/uploads/thumb_image1.jpg",
       WebPPath = "/uploads/image1.webp",
            MimeType = "image/jpeg",
   FileSize = 524288,
       WebPFileSize = 245678,
 Width = 1920,
                Height = 1080,
      MediaType = "Image",
   FileExtension = ".jpg",
   IsProcessed = true,
        CreatedAt = DateTime.UtcNow
       },
      new Media
    {
   Id = 2,
         FileName = "image2.jpg",
     FilePath = "/uploads/image2.jpg",
     ThumbnailPath = "/uploads/thumb_image2.jpg",
        WebPPath = "/uploads/image2.webp",
     MimeType = "image/jpeg",
           FileSize = 612000,
            WebPFileSize = 280000,
       Width = 1600,
Height = 900,
    MediaType = "Image",
      FileExtension = ".jpg",
          IsProcessed = true,
        CreatedAt = DateTime.UtcNow
           },
    new Media
     {
 Id = 3,
            FileName = "image3.jpg",
      FilePath = "/uploads/image3.jpg",
            ThumbnailPath = "/uploads/thumb_image3.jpg",
         WebPPath = "/uploads/image3.webp",
       MimeType = "image/jpeg",
             FileSize = 456000,
WebPFileSize = 210000,
   Width = 1280,
  Height = 720,
         MediaType = "Image",
  FileExtension = ".jpg",
        IsProcessed = true,
         CreatedAt = DateTime.UtcNow
             }
            };

          _context.Media.AddRange(media);

            // Create albums
var albums = new List<Album>
         {
   new Album
      {
           Id = 1,
 Title = "Event Photos 2024",
         Slug = "event-photos-2024",
    Description = "Photos from our annual event",
           CoverImageId = 1,
         IsPublished = true,
   PublishedAt = DateTime.UtcNow.AddDays(-10),
  ViewCount = 150,
           CreatedBy = 1,
      CreatedAt = DateTime.UtcNow.AddDays(-10)
     },
       new Album
       {
         Id = 2,
Title = "Gallery 2024",
  Slug = "gallery-2024",
                 Description = "General gallery",
     IsPublished = false,
                ViewCount = 0,
CreatedBy = 1,
        CreatedAt = DateTime.UtcNow
   }
 };

            _context.Albums.AddRange(albums);

    // Create album media
            var albumMedias = new List<AlbumMedia>
          {
   new AlbumMedia { AlbumId = 1, MediaId = 1, DisplayOrder = 0, Caption = "First photo" },
      new AlbumMedia { AlbumId = 1, MediaId = 2, DisplayOrder = 1, Caption = "Second photo" },
         new AlbumMedia { AlbumId = 1, MediaId = 3, DisplayOrder = 2, Caption = "Third photo" }
    };

        _context.AlbumMedias.AddRange(albumMedias);
         _context.SaveChanges();
        }

        #region GetAllAsync Tests

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAlbums()
        {
            // Act
            var (items, totalCount) = await _albumService.GetAllAsync();

     // Assert
            totalCount.Should().Be(2);
        items.Should().HaveCount(2);
  items.Should().AllBeOfType<AlbumDto>();
        }

        [Fact]
     public async Task GetAllAsync_WithPublishedFilter_ShouldReturnOnlyPublished()
        {
    // Act
    var (items, totalCount) = await _albumService.GetAllAsync(isPublished: true);

   // Assert
totalCount.Should().Be(1);
items.Should().HaveCount(1);
    items.First().IsPublished.Should().BeTrue();
}

      [Fact]
        public async Task GetAllAsync_WithSearchTerm_ShouldReturnMatches()
{
     // Act
 var (items, totalCount) = await _albumService.GetAllAsync(searchTerm: "Event");

         // Assert
        totalCount.Should().Be(1);
        items.Should().HaveCount(1);
 items.First().Title.Should().Contain("Event");
        }

        [Fact]
        public async Task GetAllAsync_WithPagination_ShouldReturnCorrectPage()
      {
       // Act
      var (items, totalCount) = await _albumService.GetAllAsync(page: 1, pageSize: 1);

       // Assert
            totalCount.Should().Be(2);
   items.Should().HaveCount(1);
        }

        #endregion

        #region GetByIdAsync Tests

   [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnAlbum()
        {
      // Act
            var result = await _albumService.GetByIdAsync(1);

     // Assert
       result.Should().NotBeNull();
         result.Id.Should().Be(1);
            result.Title.Should().Be("Event Photos 2024");
result.Media.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
       // Act
     var result = await _albumService.GetByIdAsync(999);

 // Assert
       result.Should().BeNull();
        }

        #endregion

        #region GetBySlugAsync Tests

   [Fact]
        public async Task GetBySlugAsync_WithValidSlug_ShouldReturnAlbum()
        {
 // Act
            var result = await _albumService.GetBySlugAsync("event-photos-2024");

            // Assert
     result.Should().NotBeNull();
       result.Title.Should().Be("Event Photos 2024");
            result.Media.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetBySlugAsync_WithInvalidSlug_ShouldReturnNull()
        {
            // Act
  var result = await _albumService.GetBySlugAsync("nonexistent-album");

    // Assert
    result.Should().BeNull();
        }

   [Fact]
  public async Task GetBySlugAsync_ShouldIncrementViewCount()
        {
  // Arrange
            var initialViewCount = (await _albumService.GetByIdAsync(1)).ViewCount;

      // Act
         await _albumService.GetBySlugAsync("event-photos-2024");

 // Assert
            var updatedAlbum = await _albumService.GetByIdAsync(1);
 updatedAlbum.ViewCount.Should().Be(initialViewCount + 1);
     }

        #endregion

        #region CreateAsync Tests

     [Fact]
        public async Task CreateAsync_WithValidData_ShouldCreateAlbum()
        {
         // Arrange
            var dto = new CreateAlbumDto
            {
   Title = "New Album",
           Slug = "new-album",
     Description = "Test album",
     IsPublished = true
            };

            // Act
    var result = await _albumService.CreateAsync(dto, 1);

     // Assert
            result.Should().NotBeNull();
      result.Title.Should().Be("New Album");
      result.Slug.Should().Be("new-album");
    result.IsPublished.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAsync_WithDuplicateSlug_ShouldThrowBadRequestException()
  {
   // Arrange
        var dto = new CreateAlbumDto
            {
             Title = "Duplicate Album",
     Slug = "event-photos-2024", // Existing slug
         Description = "Test"
            };

            // Act
          Func<Task> act = async () => await _albumService.CreateAsync(dto, 1);

    // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        #endregion

      #region UpdateAsync Tests

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldUpdateAlbum()
        {
            // Arrange
            var dto = new UpdateAlbumDto
     {
    Title = "Updated Title",
   Slug = "event-photos-2024",
 Description = "Updated description",
           IsPublished = true
            };

  // Act
            var result = await _albumService.UpdateAsync(1, dto);

  // Assert
result.Title.Should().Be("Updated Title");
  result.Description.Should().Be("Updated description");
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldThrowNotFoundException()
 {
       // Arrange
            var dto = new UpdateAlbumDto
            {
     Title = "Test",
Slug = "test",
      IsPublished = false
   };

            // Act
   Func<Task> act = async () => await _albumService.UpdateAsync(999, dto);

            // Assert
await act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

   #region DeleteAsync Tests

     [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteAlbum()
      {
     // Act
 var result = await _albumService.DeleteAsync(2);

            // Assert
            result.Should().BeTrue();
            var deleted = await _albumService.GetByIdAsync(2);
        deleted.Should().BeNull();
    }

        [Fact]
 public async Task DeleteAsync_WithInvalidId_ShouldThrowNotFoundException()
        {
     // Act
     Func<Task> act = async () => await _albumService.DeleteAsync(999);

        // Assert
      await act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

        #region AddMediaAsync Tests

        [Fact]
   public async Task AddMediaAsync_WithValidData_ShouldAddMedia()
        {
         // Arrange
            var dto = new AddMediaToAlbumDto
 {
         MediaId = 3,
       Caption = "New photo"
            };

   // Act
   var result = await _albumService.AddMediaAsync(2, dto);

    // Assert
            result.Should().NotBeNull();
     result.MediaId.Should().Be(3);
   result.Caption.Should().Be("New photo");
     }

        [Fact]
     public async Task AddMediaAsync_WithInvalidAlbum_ShouldThrowNotFoundException()
        {
    // Arrange
     var dto = new AddMediaToAlbumDto { MediaId = 1 };

// Act
            Func<Task> act = async () => await _albumService.AddMediaAsync(999, dto);

   // Assert
            await act.Should().ThrowAsync<NotFoundException>();
    }

 #endregion

        #region RemoveMediaAsync Tests

      [Fact]
        public async Task RemoveMediaAsync_WithValidData_ShouldRemoveMedia()
     {
    // Act
            var result = await _albumService.RemoveMediaAsync(1, 1);

        // Assert
      result.Should().BeTrue();
        var album = await _albumService.GetByIdAsync(1);
            album.Media.Should().HaveCount(2);
    }

        [Fact]
        public async Task RemoveMediaAsync_WithInvalidAlbum_ShouldThrowNotFoundException()
        {
   // Act
          Func<Task> act = async () => await _albumService.RemoveMediaAsync(999, 1);

      // Assert
          await act.Should().ThrowAsync<NotFoundException>();
        }

      #endregion

        #region ReorderMediaAsync Tests

        [Fact]
        public async Task ReorderMediaAsync_ShouldReorderMedia()
      {
     // Arrange
         var dto = new ReorderAlbumMediaDto
            {
    MediaIds = new List<int> { 3, 2, 1 }
            };

          // Act
     var result = await _albumService.ReorderMediaAsync(1, dto);

            // Assert
     result.Should().BeTrue();
       var album = await _albumService.GetByIdAsync(1);
        album.Media[0].MediaId.Should().Be(3);
         album.Media[1].MediaId.Should().Be(2);
       album.Media[2].MediaId.Should().Be(1);
   }

        #endregion

     #region Integration Tests

   [Fact]
        public async Task FullWorkflow_CreateUpdateDeleteAlbum_ShouldWorkCorrectly()
        {
       // 1. Create album
   var createDto = new CreateAlbumDto
   {
                Title = "Workflow Test Album",
   Slug = "workflow-test-album",
        Description = "Test album",
    IsPublished = false
            };

      var created = await _albumService.CreateAsync(createDto, 1);
         created.Should().NotBeNull();
    created.Id.Should().BeGreaterThan(0);

        // 2. Add media
            var addMediaDto = new AddMediaToAlbumDto
       {
         MediaId = 1,
                Caption = "Test photo"
            };

            var addedMedia = await _albumService.AddMediaAsync(created.Id, addMediaDto);
    addedMedia.Should().NotBeNull();

            // 3. Get album with media
      var album = await _albumService.GetByIdAsync(created.Id);
album.Media.Should().HaveCount(1);

   // 4. Update album
            var updateDto = new UpdateAlbumDto
            {
     Title = "Updated Workflow Album",
        Slug = "workflow-test-album",
            IsPublished = true
            };

  var updated = await _albumService.UpdateAsync(created.Id, updateDto);
         updated.Title.Should().Be("Updated Workflow Album");
      updated.IsPublished.Should().BeTrue();

    // 5. Remove media
            var removed = await _albumService.RemoveMediaAsync(created.Id, 1);
         removed.Should().BeTrue();

       // 6. Verify media removed
            var finalAlbum = await _albumService.GetByIdAsync(created.Id);
          finalAlbum.Media.Should().HaveCount(0);

 // 7. Delete album
            var deleted = await _albumService.DeleteAsync(created.Id);
         deleted.Should().BeTrue();
     }

        #endregion

        public void Dispose()
        {
      _context?.Dispose();
        }
    }
}
