using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Utilities;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace Gahar_Backend.Tests.Utilities
{
  public class DataSeederTests : IDisposable
    {
        private readonly ApplicationDbContext _context;

      public DataSeederTests()
{
 var options = new DbContextOptionsBuilder<ApplicationDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

      _context = new ApplicationDbContext(options);
        }

[Fact]
   public async Task SeedAsync_ShouldSeedAllData()
    {
    // Act
    await DataSeeder.SeedAsync(_context);

            // Assert
      var languages = await _context.Languages.ToListAsync();
    languages.Should().HaveCount(2);

      var roles = await _context.Roles.ToListAsync();
          roles.Should().HaveCount(4);

    var permissions = await _context.Permissions.ToListAsync();
     permissions.Should().HaveCountGreaterThanOrEqualTo(24); // At least 24 permissions

      var users = await _context.Users.ToListAsync();
   users.Should().HaveCount(1);

   var userRoles = await _context.UserRoles.ToListAsync();
         userRoles.Should().HaveCount(1);

         var rolePermissions = await _context.RolePermissions.ToListAsync();
rolePermissions.Should().HaveCountGreaterThanOrEqualTo(24); // SuperAdmin has all permissions
     }

        [Fact]
      public async Task SeedAsync_ShouldSeedLanguagesCorrectly()
  {
   // Act
            await DataSeeder.SeedAsync(_context);

 // Assert
var arabic = await _context.Languages.FirstOrDefaultAsync(l => l.Code == "ar");
      arabic.Should().NotBeNull();
            arabic!.Name.Should().NotBeNullOrEmpty();
          arabic.IsDefault.Should().BeTrue();
    arabic.Direction.Should().Be("rtl");

            var english = await _context.Languages.FirstOrDefaultAsync(l => l.Code == "en");
 english.Should().NotBeNull();
            english!.Name.Should().Be("English");
            english.IsDefault.Should().BeFalse();
english.Direction.Should().Be("ltr");
     }

     [Fact]
        public async Task SeedAsync_ShouldSeedRolesCorrectly()
{
 // Act
        await DataSeeder.SeedAsync(_context);

      // Assert
  var superAdmin = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "SuperAdmin");
    superAdmin.Should().NotBeNull();
    superAdmin!.DisplayName.Should().NotBeNullOrEmpty();
        superAdmin.IsSystemRole.Should().BeTrue();

            var admin = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
    admin.Should().NotBeNull();

            var editor = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Editor");
    editor.Should().NotBeNull();

          var viewer = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Viewer");
   viewer.Should().NotBeNull();
        }

        [Fact]
     public async Task SeedAsync_ShouldSeedPermissionsCorrectly()
 {
 // Act
            await DataSeeder.SeedAsync(_context);

  // Assert
    var userPermissions = await _context.Permissions
       .Where(p => p.Module == "Users")
          .ToListAsync();
userPermissions.Should().HaveCount(4);

      var rolePermissions = await _context.Permissions
.Where(p => p.Module == "Roles")
      .ToListAsync();
        rolePermissions.Should().HaveCount(4);

         var contentTypePermissions = await _context.Permissions
      .Where(p => p.Module == "ContentTypes")
         .ToListAsync();
            contentTypePermissions.Should().HaveCount(4);
      }

  [Fact]
 public async Task SeedAsync_ShouldCreateSuperAdminUser()
   {
  // Act
 await DataSeeder.SeedAsync(_context);

            // Assert
  var superAdmin = await _context.Users
        .FirstOrDefaultAsync(u => u.Email == "admin@gahar.sa");

     superAdmin.Should().NotBeNull();
         superAdmin!.Username.Should().Be("superadmin");
 superAdmin.FirstName.Should().Be("Super");
       superAdmin.LastName.Should().Be("Admin");
        superAdmin.IsActive.Should().BeTrue();
    superAdmin.EmailConfirmed.Should().BeTrue();
    
   // Verify password hash
  BCrypt.Net.BCrypt.Verify("Admin@123", superAdmin.PasswordHash).Should().BeTrue();
        }

        [Fact]
      public async Task SeedAsync_ShouldAssignSuperAdminRole()
 {
   // Act
  await DataSeeder.SeedAsync(_context);

     // Assert
        var superAdmin = await _context.Users
              .Include(u => u.UserRoles)
      .ThenInclude(ur => ur.Role)
    .FirstOrDefaultAsync(u => u.Email == "admin@gahar.sa");

   superAdmin.Should().NotBeNull();
   superAdmin!.UserRoles.Should().HaveCount(1);
      superAdmin.UserRoles.First().Role.Name.Should().Be("SuperAdmin");
        }

    [Fact]
        public async Task SeedAsync_ShouldAssignAllPermissionsToSuperAdmin()
        {
   // Act
          await DataSeeder.SeedAsync(_context);

  // Assert
            var superAdminRole = await _context.Roles
         .Include(r => r.RolePermissions)
  .FirstOrDefaultAsync(r => r.Name == "SuperAdmin");

    superAdminRole.Should().NotBeNull();
        superAdminRole!.RolePermissions.Should().HaveCountGreaterThanOrEqualTo(24);
   }

 [Fact]
        public async Task SeedAsync_WhenCalledMultipleTimes_ShouldNotDuplicateData()
    {
 // Act
  await DataSeeder.SeedAsync(_context);
  await DataSeeder.SeedAsync(_context);
     await DataSeeder.SeedAsync(_context);

      // Assert
          var languages = await _context.Languages.ToListAsync();
  languages.Should().HaveCount(2);

  var roles = await _context.Roles.ToListAsync();
         roles.Should().HaveCount(4);

            var users = await _context.Users.ToListAsync();
    users.Should().HaveCount(1);
        }

 [Fact]
        public async Task SeedAsync_ShouldSetCorrectPermissionDescriptions()
        {
  // Act
      await DataSeeder.SeedAsync(_context);

          // Assert
          var viewUsersPermission = await _context.Permissions
             .FirstOrDefaultAsync(p => p.Name == "Users.View");
    viewUsersPermission.Should().NotBeNull();
            viewUsersPermission!.Description.Should().NotBeNullOrEmpty();

  var createUsersPermission = await _context.Permissions
      .FirstOrDefaultAsync(p => p.Name == "Users.Create");
createUsersPermission.Should().NotBeNull();
    createUsersPermission!.Description.Should().NotBeNullOrEmpty();
        }

      [Fact]
    public async Task SeedAsync_ShouldGroupPermissionsByModule()
    {
     // Act
        await DataSeeder.SeedAsync(_context);

     // Assert
      var modules = await _context.Permissions
   .Select(p => p.Module)
         .Distinct()
     .ToListAsync();

      modules.Should().Contain(new[] { "Users", "Roles", "ContentTypes", "Content", "Pages", "Forms", "AuditLogs" });
        }

     public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
