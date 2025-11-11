using Gahar_Backend.Models.Entities;
using FluentAssertions;
using Xunit;

namespace Gahar_Backend.Tests.Entities
{
    public class UserTests
    {
        [Fact]
     public void User_ShouldInitializeWithDefaultValues()
      {
            // Arrange & Act
     var user = new User();

        // Assert
 user.Username.Should().NotBeNull();
            user.Email.Should().NotBeNull();
  user.PasswordHash.Should().NotBeNull();
            user.IsActive.Should().BeTrue();
      user.EmailConfirmed.Should().BeFalse();
   user.FailedLoginAttempts.Should().Be(0);
      user.UserRoles.Should().NotBeNull().And.BeEmpty();
   user.AuditLogs.Should().NotBeNull().And.BeEmpty();
          user.IsDeleted.Should().BeFalse();
            user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
  public void User_ShouldSetProperties_Correctly()
    {
            // Arrange
        var user = new User
      {
     Username = "testuser",
     Email = "test@example.com",
             PasswordHash = "hashedpassword",
        FirstName = "Test",
        LastName = "User",
    PhoneNumber = "1234567890"
      };

            // Assert
       user.Username.Should().Be("testuser");
         user.Email.Should().Be("test@example.com");
            user.PasswordHash.Should().Be("hashedpassword");
      user.FirstName.Should().Be("Test");
          user.LastName.Should().Be("User");
            user.PhoneNumber.Should().Be("1234567890");
     }

        [Fact]
        public void User_ShouldHandleAccountLocking()
        {
  // Arrange
            var user = new User();
var lockTime = DateTime.UtcNow.AddMinutes(30);

            // Act
    user.LockedUntil = lockTime;
            user.FailedLoginAttempts = 5;

            // Assert
            user.LockedUntil.Should().Be(lockTime);
  user.FailedLoginAttempts.Should().Be(5);
  }
    }
}
