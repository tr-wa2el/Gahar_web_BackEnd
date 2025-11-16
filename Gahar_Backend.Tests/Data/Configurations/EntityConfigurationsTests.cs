using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace Gahar_Backend.Tests.Data.Configurations
{
    public class EntityConfigurationsTests : IDisposable
    {
        private readonly ApplicationDbContext _context;

        public EntityConfigurationsTests()
   {
var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
     .Options;

  _context = new ApplicationDbContext(options);
        }

        [Fact]
 public async Task UserConfiguration_ShouldEnforceUniqueEmail()
 {
    // Arrange
   var user1 = new User
  {
  Username = "user1",
    Email = "test@example.com",
      PasswordHash = "hash1"
 };
    var user2 = new User
 {
Username = "user2",
Email = "test@example.com",
    PasswordHash = "hash2"
            };

     await _context.Users.AddAsync(user1);
            await _context.SaveChangesAsync();

    // Act
    await _context.Users.AddAsync(user2);
     
   // Assert - In-Memory DB doesn't enforce unique constraints, but we can verify the configuration exists
   var entityType = _context.Model.FindEntityType(typeof(User));
    var emailIndex = entityType!.GetIndexes().FirstOrDefault(i => i.Properties.Any(p => p.Name == "Email"));
        
      emailIndex.Should().NotBeNull();
emailIndex!.IsUnique.Should().BeTrue();
     }

     [Fact]
    public async Task UserConfiguration_ShouldEnforceUniqueUsername()
   {
// Arrange
      var user1 = new User
 {
    Username = "testuser",
     Email = "test1@example.com",
  PasswordHash = "hash1"
            };
            var user2 = new User
 {
Username = "testuser",
       Email = "test2@example.com",
  PasswordHash = "hash2"
       };

await _context.Users.AddAsync(user1);
          await _context.SaveChangesAsync();

   await _context.Users.AddAsync(user2);

  // Assert
   var entityType = _context.Model.FindEntityType(typeof(User));
    var usernameIndex = entityType!.GetIndexes().FirstOrDefault(i => i.Properties.Any(p => p.Name == "Username"));
            
    usernameIndex.Should().NotBeNull();
   usernameIndex!.IsUnique.Should().BeTrue();
      }

  [Fact]
 public void RoleConfiguration_ShouldHaveUniqueNameConstraint()
   {
   // Arrange & Act
var entityType = _context.Model.FindEntityType(typeof(Role));
   var nameIndex = entityType!.GetIndexes().FirstOrDefault(i => i.Properties.Any(p => p.Name == "Name"));

   // Assert
     nameIndex.Should().NotBeNull();
       nameIndex!.IsUnique.Should().BeTrue();
 }

 [Fact]
   public void PermissionConfiguration_ShouldHaveCompositeUniqueIndex()
  {
// Arrange & Act
      var entityType = _context.Model.FindEntityType(typeof(Permission));
  var compositeIndex = entityType!.GetIndexes()
          .FirstOrDefault(i => 
   i.Properties.Count == 2 &&
   i.Properties.Any(p => p.Name == "Name") &&
         i.Properties.Any(p => p.Name == "Module"));

// Assert
       compositeIndex.Should().NotBeNull();
    compositeIndex!.IsUnique.Should().BeTrue();
     }

        [Fact]
      public void LanguageConfiguration_ShouldHaveUniqueCodeConstraint()
  {
// Arrange & Act
  var entityType = _context.Model.FindEntityType(typeof(Language));
       var codeIndex = entityType!.GetIndexes().FirstOrDefault(i => i.Properties.Any(p => p.Name == "Code"));

  // Assert
  codeIndex.Should().NotBeNull();
 codeIndex!.IsUnique.Should().BeTrue();
  }

   [Fact]
  public void TranslationConfiguration_ShouldHaveCompositeUniqueIndex()
 {
   // Arrange & Act
   var entityType = _context.Model.FindEntityType(typeof(Translation));
       var compositeIndex = entityType!.GetIndexes()
    .FirstOrDefault(i => 
    i.Properties.Count == 4 &&
   i.Properties.Any(p => p.Name == "EntityType") &&
     i.Properties.Any(p => p.Name == "EntityId") &&
       i.Properties.Any(p => p.Name == "FieldName") &&
   i.Properties.Any(p => p.Name == "LanguageId"));

 // Assert
  compositeIndex.Should().NotBeNull();
   compositeIndex!.IsUnique.Should().BeTrue();
    }

 [Fact]
 public void UserRoleConfiguration_ShouldHaveCompositeUniqueIndex()
     {
// Arrange & Act
   var entityType = _context.Model.FindEntityType(typeof(UserRole));
       var compositeIndex = entityType!.GetIndexes()
    .FirstOrDefault(i => 
   i.Properties.Count == 2 &&
   i.Properties.Any(p => p.Name == "UserId") &&
   i.Properties.Any(p => p.Name == "RoleId"));

// Assert
   compositeIndex.Should().NotBeNull();
      compositeIndex!.IsUnique.Should().BeTrue();
 }

[Fact]
        public void RolePermissionConfiguration_ShouldHaveCompositeUniqueIndex()
        {
            // Arrange & Act
   var entityType = _context.Model.FindEntityType(typeof(RolePermission));
  var compositeIndex = entityType!.GetIndexes()
  .FirstOrDefault(i => 
    i.Properties.Count == 2 &&
  i.Properties.Any(p => p.Name == "RoleId") &&
    i.Properties.Any(p => p.Name == "PermissionId"));

// Assert
   compositeIndex.Should().NotBeNull();
            compositeIndex!.IsUnique.Should().BeTrue();
   }

        [Fact]
public void AuditLogConfiguration_ShouldHaveMultipleIndexes()
 {
  // Arrange & Act
   var entityType = _context.Model.FindEntityType(typeof(AuditLog));
   var indexes = entityType!.GetIndexes().ToList();

   // Assert
 indexes.Should().NotBeEmpty();
 
  // Check for specific indexes
   var userIdIndex = indexes.Any(i => i.Properties.Count == 1 && i.Properties.Any(p => p.Name == "UserId"));
var entityTypeIndex = indexes.Any(i => i.Properties.Count == 1 && i.Properties.Any(p => p.Name == "EntityType"));
  var timestampIndex = indexes.Any(i => i.Properties.Count == 1 && i.Properties.Any(p => p.Name == "Timestamp"));

       userIdIndex.Should().BeTrue();
         entityTypeIndex.Should().BeTrue();
  timestampIndex.Should().BeTrue();
        }

  [Fact]
        public void UserConfiguration_ShouldHaveCascadeDeleteForUserRoles()
        {
// Arrange & Act
    var entityType = _context.Model.FindEntityType(typeof(User));
  var navigation = entityType!.FindNavigation("UserRoles");
  var foreignKey = navigation!.ForeignKey;

// Assert
            foreignKey.DeleteBehavior.Should().Be(DeleteBehavior.Cascade);
        }

 [Fact]
        public void UserConfiguration_ShouldHaveSetNullDeleteForAuditLogs()
        {
// Arrange & Act
   var entityType = _context.Model.FindEntityType(typeof(User));
            var navigation = entityType!.FindNavigation("AuditLogs");
   var foreignKey = navigation!.ForeignKey;

// Assert
     foreignKey.DeleteBehavior.Should().Be(DeleteBehavior.SetNull);
        }

  [Fact]
        public async Task UserConfiguration_ShouldSetDefaultValues()
  {
// Arrange
var user = new User
     {
      Username = "testuser",
     Email = "test@example.com",
    PasswordHash = "hash"
  };

     // Act
await _context.Users.AddAsync(user);
  await _context.SaveChangesAsync();

      // Assert
 user.IsActive.Should().BeTrue();
user.EmailConfirmed.Should().BeFalse();
  user.FailedLoginAttempts.Should().Be(0);
  user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

 [Fact]
 public async Task LanguageConfiguration_ShouldSetDefaultDirection()
     {
  // Arrange
      var language = new Language
 {
    Code = "fr",
Name = "French"
     };

       // Act
            await _context.Languages.AddAsync(language);
      await _context.SaveChangesAsync();

 // Assert
            language.Direction.Should().Be("ltr");
        }

      public void Dispose()
     {
 _context.Database.EnsureDeleted();
   _context.Dispose();
        }
    }
}
