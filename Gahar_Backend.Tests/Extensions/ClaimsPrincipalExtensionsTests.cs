using Gahar_Backend.Extensions;
using FluentAssertions;
using System.Security.Claims;
using Xunit;

namespace Gahar_Backend.Tests.Extensions
{
    public class ClaimsPrincipalExtensionsTests
{
  private ClaimsPrincipal CreateTestUser(int userId = 1, string email = "test@example.com", string username = "testuser")
  {
var claims = new List<Claim>
  {
      new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
      new Claim(ClaimTypes.Email, email),
 new Claim(ClaimTypes.Name, username),
      new Claim(ClaimTypes.Role, "Admin"),
      new Claim(ClaimTypes.Role, "Editor"),
        new Claim("Permission", "Users.Create"),
 new Claim("Permission", "Users.Edit")
            };

        var identity = new ClaimsIdentity(claims, "TestAuth");
   return new ClaimsPrincipal(identity);
        }

        [Fact]
        public void GetUserId_ShouldReturnCorrectUserId()
        {
  // Arrange
  var user = CreateTestUser(userId: 123);

 // Act
     var userId = user.GetUserId();

        // Assert
    userId.Should().Be(123);
        }

        [Fact]
public void GetUserId_WithNoUserIdClaim_ShouldReturnZero()
        {
     // Arrange
      var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test@example.com") };
 var identity = new ClaimsIdentity(claims, "TestAuth");
     var user = new ClaimsPrincipal(identity);

       // Act
  var userId = user.GetUserId();

   // Assert
  userId.Should().Be(0);
 }

 [Fact]
        public void GetUserEmail_ShouldReturnCorrectEmail()
{
        // Arrange
   var user = CreateTestUser(email: "john@example.com");

   // Act
      var email = user.GetUserEmail();

     // Assert
   email.Should().Be("john@example.com");
   }

   [Fact]
  public void GetUserEmail_WithNoEmailClaim_ShouldReturnEmptyString()
 {
     // Arrange
    var claims = new List<Claim> { new Claim(ClaimTypes.Name, "test") };
    var identity = new ClaimsIdentity(claims, "TestAuth");
var user = new ClaimsPrincipal(identity);

 // Act
            var email = user.GetUserEmail();

            // Assert
    email.Should().BeEmpty();
        }

     [Fact]
        public void GetUsername_ShouldReturnCorrectUsername()
        {
// Arrange
            var user = CreateTestUser(username: "johndoe");

     // Act
     var username = user.GetUsername();

       // Assert
  username.Should().Be("johndoe");
        }

      [Fact]
   public void GetUserRoles_ShouldReturnAllRoles()
     {
    // Arrange
var user = CreateTestUser();

     // Act
     var roles = user.GetUserRoles();

          // Assert
      roles.Should().HaveCount(2);
  roles.Should().Contain("Admin");
  roles.Should().Contain("Editor");
        }

        [Fact]
public void GetUserRoles_WithNoRoles_ShouldReturnEmptyList()
{
   // Arrange
     var claims = new List<Claim> { new Claim(ClaimTypes.Name, "test") };
 var identity = new ClaimsIdentity(claims, "TestAuth");
  var user = new ClaimsPrincipal(identity);

 // Act
  var roles = user.GetUserRoles();

    // Assert
            roles.Should().BeEmpty();
}

 [Fact]
public void GetUserPermissions_ShouldReturnAllPermissions()
        {
  // Arrange
      var user = CreateTestUser();

   // Act
       var permissions = user.GetUserPermissions();

      // Assert
      permissions.Should().HaveCount(2);
    permissions.Should().Contain("Users.Create");
      permissions.Should().Contain("Users.Edit");
        }

        [Fact]
public void HasPermission_WithExistingPermission_ShouldReturnTrue()
  {
  // Arrange
      var user = CreateTestUser();

 // Act
   var hasPermission = user.HasPermission("Users.Create");

 // Assert
            hasPermission.Should().BeTrue();
     }

 [Fact]
  public void HasPermission_WithNonExistingPermission_ShouldReturnFalse()
  {
  // Arrange
       var user = CreateTestUser();

            // Act
         var hasPermission = user.HasPermission("Users.Delete");

// Assert
     hasPermission.Should().BeFalse();
 }

        [Fact]
public void HasRole_WithExistingRole_ShouldReturnTrue()
     {
      // Arrange
         var user = CreateTestUser();

       // Act
            var hasRole = user.HasRole("Admin");

    // Assert
   hasRole.Should().BeTrue();
     }

        [Fact]
        public void HasRole_WithNonExistingRole_ShouldReturnFalse()
   {
   // Arrange
   var user = CreateTestUser();

 // Act
    var hasRole = user.HasRole("SuperAdmin");

   // Assert
     hasRole.Should().BeFalse();
     }

  [Fact]
public void HasAnyRole_WithOneMatchingRole_ShouldReturnTrue()
   {
   // Arrange
var user = CreateTestUser();

   // Act
    var hasAnyRole = user.HasAnyRole("Admin", "SuperAdmin");

   // Assert
  hasAnyRole.Should().BeTrue();
        }

        [Fact]
   public void HasAnyRole_WithNoMatchingRoles_ShouldReturnFalse()
  {
// Arrange
var user = CreateTestUser();

 // Act
            var hasAnyRole = user.HasAnyRole("SuperAdmin", "Viewer");

 // Assert
            hasAnyRole.Should().BeFalse();
        }

 [Fact]
 public void HasAllRoles_WithAllMatchingRoles_ShouldReturnTrue()
        {
     // Arrange
            var user = CreateTestUser();

        // Act
         var hasAllRoles = user.HasAllRoles("Admin", "Editor");

       // Assert
  hasAllRoles.Should().BeTrue();
 }

        [Fact]
      public void HasAllRoles_WithSomeMissingRoles_ShouldReturnFalse()
      {
  // Arrange
            var user = CreateTestUser();

 // Act
            var hasAllRoles = user.HasAllRoles("Admin", "SuperAdmin");

            // Assert
       hasAllRoles.Should().BeFalse();
 }
    }
}
