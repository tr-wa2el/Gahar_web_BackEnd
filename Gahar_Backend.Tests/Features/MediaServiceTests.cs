using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Models.DTOs.Media;
using Gahar_Backend.Services.Implementations;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Repositories.Implementations;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Utilities.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Gahar_Backend.Tests.Features
{
    public class MediaServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediaRepository _mediaRepository;
  private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ILogger<MediaService>> _loggerMock;
        private readonly IMediaService _mediaService;

        public MediaServiceTests()
        {
            // Setup In-Memory Database
     var options = new DbContextOptionsBuilder<ApplicationDbContext>()
.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

  _context = new ApplicationDbContext(options);

            // Setup repository and service
     _mediaRepository = new MediaRepository(_context);
        _configurationMock = new Mock<IConfiguration>();
 _loggerMock = new Mock<ILogger<MediaService>>();

    _mediaService = new MediaService(
  _mediaRepository,
     _configurationMock.Object,
                _loggerMock.Object);

   // Seed test data
            SeedTestData();
        }

      private void SeedTestData()
 {
         var media = new List<Media>
            {
      new Media
                {
     Id = 1,
       FileName = "image1.jpg",
    FilePath = "/uploads/guid_image1.jpg",
    ThumbnailPath = "/uploads/thumb_guid_image1.jpg",
 WebPPath = "/uploads/guid_image1.webp",
    MimeType = "image/jpeg",
     FileSize = 524288,
    WebPFileSize = 245678,
     Width = 1920,
               Height = 1080,
        Alt = "Test Image 1",
            MediaType = "Image",
   FileExtension = ".jpg",
            IsProcessed = true,
        CreatedAt = DateTime.UtcNow,
             UploadedBy = 1
      },
           new Media
     {
       Id = 2,
  FileName = "video1.mp4",
          FilePath = "/uploads/guid_video1.mp4",
     MimeType = "video/mp4",
          FileSize = 10485760,
      MediaType = "Video",
              FileExtension = ".mp4",
            IsProcessed = false,
              CreatedAt = DateTime.UtcNow,
    UploadedBy = 1
   },
         new Media
           {
   Id = 3,
   FileName = "document.pdf",
      FilePath = "/uploads/guid_document.pdf",
  MimeType = "application/pdf",
        FileSize = 2097152,
   MediaType = "Document",
        FileExtension = ".pdf",
           IsProcessed = false,
   CreatedAt = DateTime.UtcNow,
       UploadedBy = 1
}
            };

         _context.Media.AddRange(media);
        _context.SaveChanges();
        }

        #region GetAllAsync Tests

        [Fact]
   public async Task GetAllAsync_ShouldReturnAllMedia()
      {
            // Act
  var (items, totalCount) = await _mediaService.GetAllAsync();

       // Assert
       totalCount.Should().Be(3);
   items.Should().HaveCount(3);
          items.Should().AllBeOfType<MediaDto>();
        }

        [Fact]
        public async Task GetAllAsync_WithMediaTypeFilter_ShouldReturnFiltered()
        {
       // Act
      var (items, totalCount) = await _mediaService.GetAllAsync(mediaType: "Image");

            // Assert
   totalCount.Should().Be(1);
       items.Should().HaveCount(1);
    items.First().MediaType.Should().Be("Image");
        }

        [Fact]
        public async Task GetAllAsync_WithSearchTerm_ShouldReturnFiltered()
        {
          // Act
            var (items, totalCount) = await _mediaService.GetAllAsync(searchTerm: "image");

  // Assert
         totalCount.Should().Be(1);
        items.Should().HaveCount(1);
            items.First().FileName.Should().Contain("image");
        }

        [Fact]
        public async Task GetAllAsync_WithPagination_ShouldReturnPagedResults()
 {
   // Act
   var (items, totalCount) = await _mediaService.GetAllAsync(page: 1, pageSize: 2);

         // Assert
            totalCount.Should().Be(3);
    items.Should().HaveCount(2);
        }

 #endregion

        #region GetByIdAsync Tests

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnMedia()
 {
            // Act
    var result = await _mediaService.GetByIdAsync(1);

       // Assert
            result.Should().NotBeNull();
       result.Id.Should().Be(1);
      result.FileName.Should().Be("image1.jpg");
        result.MediaType.Should().Be("Image");
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
        // Act
      var result = await _mediaService.GetByIdAsync(999);

     // Assert
  result.Should().BeNull();
        }

   #endregion

   #region UpdateAsync Tests

  [Fact]
        public async Task UpdateAsync_WithValidData_ShouldUpdateMedia()
        {
   // Arrange
     var updateDto = new UpdateMediaDto
        {
           FileName = "updated_image.jpg",
            Alt = "Updated Alt Text"
        };

   // Act
      var result = await _mediaService.UpdateAsync(1, updateDto);

            // Assert
            result.Should().NotBeNull();
    result.FileName.Should().Be("updated_image.jpg");
          result.Alt.Should().Be("Updated Alt Text");
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldThrowNotFoundException()
   {
       // Arrange
        var updateDto = new UpdateMediaDto { FileName = "test.jpg" };

          // Act
   Func<Task> act = async () => await _mediaService.UpdateAsync(999, updateDto);

  // Assert
       await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateAsync_WithDuplicateFileName_ShouldThrowBadRequestException()
        {
            // Arrange
 var updateDto = new UpdateMediaDto { FileName = "video1.mp4" };

          // Act
        Func<Task> act = async () => await _mediaService.UpdateAsync(1, updateDto);

     // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

      #endregion

        #region DeleteAsync Tests

      [Fact]
  public async Task DeleteAsync_WithValidId_ShouldDeleteMedia()
 {
      // Act
            var result = await _mediaService.DeleteAsync(2);

            // Assert
            result.Should().BeTrue();

  // Verify deletion
      var deleted = await _mediaService.GetByIdAsync(2);
     deleted.Should().BeNull();
    }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldThrowNotFoundException()
        {
    // Act
   Func<Task> act = async () => await _mediaService.DeleteAsync(999);

// Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

   #endregion

        #region SearchAsync Tests

      [Fact]
 public async Task SearchAsync_WithMatchingTerm_ShouldReturnMatches()
        {
  // Act
  var result = await _mediaService.SearchAsync("image");

   // Assert
 result.Should().NotBeEmpty();
            result.Should().AllSatisfy(m => m.FileName.ToLower().Should().Contain("image"));
        }

        [Fact]
  public async Task SearchAsync_WithNoMatches_ShouldReturnEmpty()
   {
            // Act
            var result = await _mediaService.SearchAsync("nonexistent");

            // Assert
    result.Should().BeEmpty();
  }

      [Fact]
        public async Task SearchAsync_ByAltText_ShouldReturnMatches()
        {
            // Act
     var result = await _mediaService.SearchAsync("Test Image");

            // Assert
      result.Should().NotBeEmpty();
            result.First().Alt.Should().Contain("Test Image");
 }

 #endregion

        #region ValidateFileAsync Tests

  [Fact]
        public async Task ValidateFileAsync_WithValidFile_ShouldReturnTrue()
 {
    // Arrange
            var fileMock = new Mock<IFormFile>();
  fileMock.Setup(f => f.FileName).Returns("test.jpg");
        fileMock.Setup(f => f.Length).Returns(1024 * 100); // 100 KB

      // Act
    var (isValid, errorMessage) = await _mediaService.ValidateFileAsync(fileMock.Object);

     // Assert
         isValid.Should().BeTrue();
            errorMessage.Should().BeNull();
        }

     [Fact]
        public async Task ValidateFileAsync_WithEmptyFile_ShouldReturnFalse()
{
      // Arrange
   var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("test.jpg");
        fileMock.Setup(f => f.Length).Returns(0);

            // Act
            var (isValid, errorMessage) = await _mediaService.ValidateFileAsync(fileMock.Object);

          // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Contain("empty");
        }

     [Fact]
      public async Task ValidateFileAsync_WithOversizedFile_ShouldReturnFalse()
        {
            // Arrange
         var fileMock = new Mock<IFormFile>();
      fileMock.Setup(f => f.FileName).Returns("test.jpg");
            fileMock.Setup(f => f.Length).Returns(20 * 1024 * 1024); // 20 MB

        // Act
            var (isValid, errorMessage) = await _mediaService.ValidateFileAsync(fileMock.Object);

      // Assert
            isValid.Should().BeFalse();
  errorMessage.Should().Contain("exceeds");
        }

        [Fact]
        public async Task ValidateFileAsync_WithUnsupportedFileType_ShouldReturnFalse()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
    fileMock.Setup(f => f.FileName).Returns("test.exe");
            fileMock.Setup(f => f.Length).Returns(1024);

  // Act
            var (isValid, errorMessage) = await _mediaService.ValidateFileAsync(fileMock.Object);

      // Assert
   isValid.Should().BeFalse();
          errorMessage.Should().Contain("not allowed");
        }

      #endregion

        #region GetStatsAsync Tests

        [Fact]
        public async Task GetStatsAsync_ShouldReturnCorrectStats()
        {
 // Act
            var stats = await _mediaService.GetStatsAsync();

            // Assert
       stats.Should().NotBeNull();
   stats.TotalFiles.Should().Be(3);
    stats.ImageCount.Should().Be(1);
         stats.VideoCount.Should().Be(1);
       stats.DocumentCount.Should().Be(1);
      stats.AudioCount.Should().Be(0);
 stats.TotalStorageSize.Should().Be(524288 + 10485760 + 2097152);
 }

        #endregion

        #region IntegrationTests

        [Fact]
        public async Task FullWorkflow_CreateUpdateDelete_ShouldWorkCorrectly()
        {
     // 1. Get initial count
          var (initialItems, initialCount) = await _mediaService.GetAllAsync();
   var initialTotal = initialCount;

            // 2. Create media record
            var testMedia = new Media
{
     FileName = "workflow_test.jpg",
 FilePath = "/uploads/workflow_test.jpg",
      MimeType = "image/jpeg",
     FileSize = 100000,
          MediaType = "Image",
           FileExtension = ".jpg",
          IsProcessed = true,
  CreatedAt = DateTime.UtcNow
       };

     await _mediaRepository.CreateAsync(testMedia);

            // 3. Verify creation
        var created = await _mediaService.GetByIdAsync(testMedia.Id);
       created.Should().NotBeNull();
            created.FileName.Should().Be("workflow_test.jpg");

        // 4. Update media
            var updateDto = new UpdateMediaDto
            {
         Alt = "Workflow Test Image"
     };

          var updated = await _mediaService.UpdateAsync(testMedia.Id, updateDto);
            updated.Alt.Should().Be("Workflow Test Image");

            // 5. Verify update
var verified = await _mediaService.GetByIdAsync(testMedia.Id);
      verified.Alt.Should().Be("Workflow Test Image");

         // 6. Delete media
         var deleted = await _mediaService.DeleteAsync(testMedia.Id);
 deleted.Should().BeTrue();

  // 7. Verify deletion
      var afterDelete = await _mediaService.GetByIdAsync(testMedia.Id);
            afterDelete.Should().BeNull();
     }

        #endregion

        public void Dispose()
        {
            _context?.Dispose();
  }
    }
}
