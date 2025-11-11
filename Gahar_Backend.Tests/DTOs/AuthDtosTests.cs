using Gahar_Backend.Models.DTOs.Auth;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Gahar_Backend.Tests.DTOs
{
    public class AuthDtosTests
    {
 private List<ValidationResult> ValidateModel(object model)
  {
  var validationResults = new List<ValidationResult>();
       var validationContext = new ValidationContext(model, null, null);
Validator.TryValidateObject(model, validationContext, validationResults, true);
  return validationResults;
 }

        [Fact]
        public void LoginDto_WithValidData_ShouldPassValidation()
 {
       // Arrange
    var loginDto = new LoginDto
       {
       Email = "test@example.com",
Password = "password123"
       };

     // Act
     var validationResults = ValidateModel(loginDto);

   // Assert
       validationResults.Should().BeEmpty();
        }

     [Fact]
        public void LoginDto_WithInvalidEmail_ShouldFailValidation()
        {
// Arrange
     var loginDto = new LoginDto
      {
    Email = "invalid-email",
       Password = "password123"
   };

       // Act
       var validationResults = ValidateModel(loginDto);

       // Assert
  validationResults.Should().NotBeEmpty();
     validationResults.Should().Contain(vr => vr.MemberNames.Contains("Email"));
 }

        [Fact]
 public void LoginDto_WithEmptyEmail_ShouldFailValidation()
        {
// Arrange
var loginDto = new LoginDto
 {
      Email = "",
    Password = "password123"
   };

 // Act
   var validationResults = ValidateModel(loginDto);

  // Assert
 validationResults.Should().NotBeEmpty();
        }

     [Fact]
public void LoginDto_WithEmptyPassword_ShouldFailValidation()
 {
     // Arrange
  var loginDto = new LoginDto
       {
 Email = "test@example.com",
     Password = ""
 };

  // Act
     var validationResults = ValidateModel(loginDto);

       // Assert
  validationResults.Should().NotBeEmpty();
 }

        [Fact]
   public void RegisterDto_WithValidData_ShouldPassValidation()
        {
     // Arrange
  var registerDto = new RegisterDto
   {
      Username = "testuser",
       Email = "test@example.com",
      Password = "password123",
    FirstName = "Test",
     LastName = "User"
      };

  // Act
var validationResults = ValidateModel(registerDto);

// Assert
       validationResults.Should().BeEmpty();
        }

     [Fact]
public void RegisterDto_WithShortPassword_ShouldFailValidation()
        {
   // Arrange
       var registerDto = new RegisterDto
   {
     Username = "testuser",
 Email = "test@example.com",
Password = "short",
FirstName = "Test"
       };

       // Act
     var validationResults = ValidateModel(registerDto);

       // Assert
   validationResults.Should().NotBeEmpty();
   validationResults.Should().Contain(vr => vr.MemberNames.Contains("Password"));
        }

 [Fact]
  public void RegisterDto_WithInvalidEmail_ShouldFailValidation()
 {
       // Arrange
       var registerDto = new RegisterDto
  {
Username = "testuser",
       Email = "invalid-email",
      Password = "password123"
   };

  // Act
       var validationResults = ValidateModel(registerDto);

       // Assert
   validationResults.Should().NotBeEmpty();
   validationResults.Should().Contain(vr => vr.MemberNames.Contains("Email"));
 }

        [Fact]
        public void RegisterDto_WithEmptyUsername_ShouldFailValidation()
   {
   // Arrange
       var registerDto = new RegisterDto
   {
Username = "",
 Email = "test@example.com",
   Password = "password123"
    };

  // Act
     var validationResults = ValidateModel(registerDto);

// Assert
validationResults.Should().NotBeEmpty();
        }

 [Fact]
public void ChangePasswordDto_WithValidData_ShouldPassValidation()
  {
// Arrange
   var changePasswordDto = new ChangePasswordDto
       {
       CurrentPassword = "oldpassword",
NewPassword = "newpassword123"
   };

  // Act
var validationResults = ValidateModel(changePasswordDto);

       // Assert
       validationResults.Should().BeEmpty();
    }

        [Fact]
        public void ChangePasswordDto_WithShortNewPassword_ShouldFailValidation()
     {
   // Arrange
       var changePasswordDto = new ChangePasswordDto
       {
 CurrentPassword = "oldpassword",
     NewPassword = "short"
   };

  // Act
var validationResults = ValidateModel(changePasswordDto);

// Assert
validationResults.Should().NotBeEmpty();
 validationResults.Should().Contain(vr => vr.MemberNames.Contains("NewPassword"));
 }

[Fact]
      public void ResetPasswordDto_WithValidData_ShouldPassValidation()
  {
   // Arrange
  var resetPasswordDto = new ResetPasswordDto
       {
       Email = "test@example.com",
    Token = "reset-token",
  NewPassword = "newpassword123"
};

       // Act
var validationResults = ValidateModel(resetPasswordDto);

   // Assert
validationResults.Should().BeEmpty();
 }

  [Fact]
        public void ResetPasswordDto_WithInvalidEmail_ShouldFailValidation()
        {
// Arrange
       var resetPasswordDto = new ResetPasswordDto
   {
    Email = "invalid-email",
Token = "reset-token",
     NewPassword = "newpassword123"
   };

       // Act
  var validationResults = ValidateModel(resetPasswordDto);

   // Assert
validationResults.Should().NotBeEmpty();
     validationResults.Should().Contain(vr => vr.MemberNames.Contains("Email"));
        }

 [Fact]
  public void AuthResponseDto_ShouldSetPropertiesCorrectly()
  {
       // Arrange & Act
    var authResponse = new AuthResponseDto
   {
       AccessToken = "access_token",
     RefreshToken = "refresh_token",
       ExpiresAt = DateTime.UtcNow.AddHours(1),
       User = new UserDto
       {
       Id = 1,
     Username = "testuser",
  Email = "test@example.com"
   }
     };

  // Assert
  authResponse.AccessToken.Should().Be("access_token");
authResponse.RefreshToken.Should().Be("refresh_token");
  authResponse.User.Should().NotBeNull();
  authResponse.User.Username.Should().Be("testuser");
      }

     [Fact]
   public void UserDto_ShouldSetPropertiesCorrectly()
        {
  // Arrange & Act
  var userDto = new UserDto
   {
     Id = 1,
  Username = "testuser",
Email = "test@example.com",
       FirstName = "Test",
   LastName = "User",
     Roles = new List<string> { "Admin", "Editor" }
       };

// Assert
  userDto.Id.Should().Be(1);
  userDto.Username.Should().Be("testuser");
  userDto.Email.Should().Be("test@example.com");
  userDto.FirstName.Should().Be("Test");
userDto.LastName.Should().Be("User");
   userDto.Roles.Should().HaveCount(2);
   userDto.Roles.Should().Contain("Admin");
        }
}
}
