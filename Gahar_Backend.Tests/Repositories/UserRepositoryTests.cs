using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace Gahar_Backend.Tests.Repositories
{
    public class UserRepositoryTests : IDisposable
 {
  private readonly ApplicationDbContext _context;
     private readonly UserRepository _userRepository;

     public UserRepositoryTests()
        {
       var options = new DbContextOptionsBuilder<ApplicationDbContext>()
.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    .Options;

      _context = new ApplicationDbContext(options);
         _userRepository = new UserRepository(_context);

   SeedData();
   }

     private void SeedData()
        {
       var role = new Role { Id = 1, Name = "Admin", DisplayName = "Administrator" };
  _context.Roles.Add(role);

    var user = new User
    {
Id = 1,
         Username = "testuser",
     Email = "test@example.com",
      PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
       IsActive = true
      };
_context.Users.Add(user);

     var userRole = new UserRole { Id = 1, UserId = 1, RoleId = 1 };
       _context.UserRoles.Add(userRole);

        var permission = new Permission { Id = 1, Name = "Users.Create", Module = "Users" };
   _context.Permissions.Add(permission);

 var rolePermission = new RolePermission { Id = 1, RoleId = 1, PermissionId = 1 };
 _context.RolePermissions.Add(rolePermission);

   _context.SaveChanges();
 }

  [Fact]
 public async Task GetByIdAsync_ShouldReturnUser()
     {
     // Act
    var user = await _userRepository.GetByIdAsync(1);

   // Assert
  user.Should().NotBeNull();
     user!.Username.Should().Be("testuser");
  }

     [Fact]
        public async Task GetByEmailAsync_ShouldReturnUser()
   {
       // Act
     var user = await _userRepository.GetByEmailAsync("test@example.com");

       // Assert
   user.Should().NotBeNull();
   user!.Username.Should().Be("testuser");
     user.UserRoles.Should().NotBeEmpty();
        }

  [Fact]
      public async Task GetByUsernameAsync_ShouldReturnUser()
 {
       // Act
     var user = await _userRepository.GetByUsernameAsync("testuser");

       // Assert
       user.Should().NotBeNull();
  user!.Email.Should().Be("test@example.com");
}

 [Fact]
        public async Task ExistsByEmailAsync_WithExistingEmail_ShouldReturnTrue()
  {
    // Act
     var exists = await _userRepository.ExistsByEmailAsync("test@example.com");

// Assert
     exists.Should().BeTrue();
        }

   [Fact]
   public async Task ExistsByEmailAsync_WithNonExistingEmail_ShouldReturnFalse()
  {
   // Act
       var exists = await _userRepository.ExistsByEmailAsync("nonexisting@example.com");

  // Assert
     exists.Should().BeFalse();
 }

        [Fact]
 public async Task ExistsByUsernameAsync_WithExistingUsername_ShouldReturnTrue()
        {
// Act
    var exists = await _userRepository.ExistsByUsernameAsync("testuser");

      // Assert
      exists.Should().BeTrue();
}

     [Fact]
   public async Task CreateAsync_ShouldCreateNewUser()
{
       // Arrange
       var newUser = new User
   {
     Username = "newuser",
      Email = "new@example.com",
 PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
       };

   // Act
       var createdUser = await _userRepository.CreateAsync(newUser);

     // Assert
    createdUser.Should().NotBeNull();
  createdUser.Id.Should().BeGreaterThan(0);
       
    var userInDb = await _context.Users.FindAsync(createdUser.Id);
   userInDb.Should().NotBeNull();
 }

     [Fact]
        public async Task UpdateAsync_ShouldUpdateUser()
        {
  // Arrange
    var user = await _userRepository.GetByIdAsync(1);
   user!.FirstName = "Updated";

   // Act
    var updatedUser = await _userRepository.UpdateAsync(user);

     // Assert
    updatedUser.FirstName.Should().Be("Updated");
       
var userInDb = await _context.Users.FindAsync(1);
  userInDb!.FirstName.Should().Be("Updated");
        }

 [Fact]
  public async Task DeleteAsync_ShouldSoftDeleteUser()
        {
      // Act
   var result = await _userRepository.DeleteAsync(1);

// Assert
result.Should().BeTrue();

  var user = await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == 1);
       user.Should().NotBeNull();
       user!.IsDeleted.Should().BeTrue();
       user.DeletedAt.Should().NotBeNull();
        }

        [Fact]
 public async Task GetUserRolesAsync_ShouldReturnUserRoles()
{
    // Act
var roles = await _userRepository.GetUserRolesAsync(1);

// Assert
  roles.Should().NotBeEmpty();
   roles.Should().Contain("Admin");
 }

        [Fact]
 public async Task GetUserPermissionsAsync_ShouldReturnUserPermissions()
  {
   // Act
var permissions = await _userRepository.GetUserPermissionsAsync(1);

       // Assert
       permissions.Should().NotBeEmpty();
      permissions.Should().Contain("Users.Create");
}

   [Fact]
public async Task AssignRoleAsync_ShouldAssignRoleToUser()
  {
// Arrange
  var newUser = new User
    {
       Username = "newuser",
       Email = "new@example.com",
PasswordHash = "hash"
   };
    await _userRepository.CreateAsync(newUser);

// Act
  var result = await _userRepository.AssignRoleAsync(newUser.Id, "Admin");

       // Assert
       result.Should().BeTrue();
       
  var roles = await _userRepository.GetUserRolesAsync(newUser.Id);
       roles.Should().Contain("Admin");
        }

        [Fact]
 public async Task RemoveRoleAsync_ShouldRemoveRoleFromUser()
{
       // Act
  var result = await _userRepository.RemoveRoleAsync(1, "Admin");

// Assert
   result.Should().BeTrue();
       
     var roles = await _userRepository.GetUserRolesAsync(1);
     roles.Should().NotContain("Admin");
  }

  public void Dispose()
   {
     _context.Database.EnsureDeleted();
   _context.Dispose();
        }
    }
}
