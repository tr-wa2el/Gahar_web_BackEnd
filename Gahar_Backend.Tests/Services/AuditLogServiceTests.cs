using Gahar_Backend.Data;
using Gahar_Backend.Models.DTOs;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;
using Xunit;

namespace Gahar_Backend.Tests.Services
{
    public class AuditLogServiceTests : IDisposable
    {
  private readonly ApplicationDbContext _context;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
  private readonly AuditLogService _auditLogService;

     public AuditLogServiceTests()
   {
     var options = new DbContextOptionsBuilder<ApplicationDbContext>()
   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;

    _context = new ApplicationDbContext(options);
       _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
          
SetupHttpContext();

       _auditLogService = new AuditLogService(_context, _httpContextAccessorMock.Object);
        }

        private void SetupHttpContext()
  {
       var httpContext = new DefaultHttpContext();
  httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");
       httpContext.Request.Headers["User-Agent"] = "TestAgent/1.0";

          _httpContextAccessorMock.Setup(h => h.HttpContext)
    .Returns(httpContext);
 }

  [Fact]
   public async Task LogAsync_ShouldCreateAuditLog()
   {
     // Arrange
     var userId = 1;
     var action = "Create";
var entityType = "User";
    var entityId = 1;
   var description = "Created new user";

            // Act
await _auditLogService.LogAsync(userId, action, entityType, entityId, description);

  // Assert
     var logs = await _context.AuditLogs.ToListAsync();
      logs.Should().HaveCount(1);
       logs[0].UserId.Should().Be(userId);
        logs[0].Action.Should().Be(action);
     logs[0].EntityType.Should().Be(entityType);
 logs[0].EntityId.Should().Be(entityId);
      logs[0].Description.Should().Be(description);
      logs[0].IpAddress.Should().Be("127.0.0.1");
   logs[0].UserAgent.Should().Be("TestAgent/1.0");
  }

     [Fact]
        public async Task LogAsync_WithOldAndNewValues_ShouldSerializeToJson()
        {
      // Arrange
     var oldValues = new { Name = "OldName", Email = "old@example.com" };
    var newValues = new { Name = "NewName", Email = "new@example.com" };

       // Act
    await _auditLogService.LogAsync(1, "Update", "User", 1, "Updated user", oldValues, newValues);

     // Assert
     var log = await _context.AuditLogs.FirstAsync();
     log.OldValues.Should().NotBeNullOrEmpty();
   log.NewValues.Should().NotBeNullOrEmpty();
          log.OldValues.Should().Contain("OldName");
  log.NewValues.Should().Contain("NewName");
 }

  [Fact]
        public async Task GetLogsAsync_WithFilter_ShouldReturnFilteredLogs()
        {
      // Arrange
  await _auditLogService.LogAsync(1, "Create", "User", 1, "Created user 1");
    await _auditLogService.LogAsync(2, "Update", "User", 2, "Updated user 2");
       await _auditLogService.LogAsync(1, "Delete", "Role", 3, "Deleted role 3");

       var filter = new AuditLogFilterDto
    {
  UserId = 1,
    PageNumber = 1,
     PageSize = 10
};

 // Act
       var logs = await _auditLogService.GetLogsAsync(filter);

   // Assert
       logs.Should().HaveCount(2);
   logs.All(l => l.UserId == 1).Should().BeTrue();
        }

     [Fact]
public async Task GetLogsAsync_WithEntityTypeFilter_ShouldReturnFilteredLogs()
 {
  // Arrange
    await _auditLogService.LogAsync(1, "Create", "User", 1, "Created user");
   await _auditLogService.LogAsync(1, "Create", "Role", 2, "Created role");
       await _auditLogService.LogAsync(1, "Create", "Permission", 3, "Created permission");

   var filter = new AuditLogFilterDto
     {
      EntityType = "User",
       PageNumber = 1,
   PageSize = 10
   };

 // Act
var logs = await _auditLogService.GetLogsAsync(filter);

       // Assert
       logs.Should().HaveCount(1);
   logs.First().EntityType.Should().Be("User");
}

        [Fact]
 public async Task GetLogsAsync_WithDateRangeFilter_ShouldReturnFilteredLogs()
  {
  // Arrange
    await _auditLogService.LogAsync(1, "Create", "User", 1, "Log 1");
    await Task.Delay(100);
   await _auditLogService.LogAsync(1, "Update", "User", 1, "Log 2");

      var filter = new AuditLogFilterDto
       {
      StartDate = DateTime.UtcNow.AddMinutes(-1),
         EndDate = DateTime.UtcNow.AddMinutes(1),
       PageNumber = 1,
       PageSize = 10
};

  // Act
     var logs = await _auditLogService.GetLogsAsync(filter);

 // Assert
       logs.Should().HaveCount(2);
  }

     [Fact]
        public async Task GetLogsAsync_WithPagination_ShouldReturnCorrectPage()
        {
// Arrange
 for (int i = 1; i <= 15; i++)
      {
    await _auditLogService.LogAsync(1, "Create", "User", i, $"Log {i}");
     }

   var filter = new AuditLogFilterDto
       {
  PageNumber = 2,
       PageSize = 5
   };

 // Act
       var logs = await _auditLogService.GetLogsAsync(filter);

// Assert
     logs.Should().HaveCount(5);
        }

  public void Dispose()
        {
 _context.Database.EnsureDeleted();
      _context.Dispose();
 }
    }
}
