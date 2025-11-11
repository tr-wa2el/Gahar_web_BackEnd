using Gahar_Backend.Models.DTOs.Auth;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Implementations;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;
using Moq;
using FluentAssertions;
using Xunit;

namespace Gahar_Backend.Tests.Services
{
    public class AuthServiceTests
    {
 private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IAuditLogService> _auditLogServiceMock;
        private readonly AuthService _authService;

     public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
     _jwtServiceMock = new Mock<IJwtService>();
  _auditLogServiceMock = new Mock<IAuditLogService>();
         _authService = new AuthService(
 _userRepositoryMock.Object,
      _jwtServiceMock.Object,
    _auditLogServiceMock.Object
      );
        }

   [Fact]
  public async Task LoginAsync_WithValidCredentials_ShouldReturnAuthResponse()
   {
       // Arrange
  var loginDto = new LoginDto
       {
     Email = "test@example.com",
   Password = "password123"
    };

      var user = new User
    {
  Id = 1,
       Email = "test@example.com",
    Username = "testuser",
  PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
    IsActive = true
      };

   var roles = new List<string> { "Admin" };
          var permissions = new List<string> { "Users.Create" };

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(loginDto.Email))
        .ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.GetUserRolesAsync(user.Id))
    .ReturnsAsync(roles);
    _userRepositoryMock.Setup(r => r.GetUserPermissionsAsync(user.Id))
       .ReturnsAsync(permissions);
_jwtServiceMock.Setup(j => j.GenerateAccessToken(user, roles, permissions))
        .Returns("access_token");
 _jwtServiceMock.Setup(j => j.GenerateRefreshToken())
 .Returns("refresh_token");

   // Act
    var result = await _authService.LoginAsync(loginDto);

  // Assert
          result.Should().NotBeNull();
        result.AccessToken.Should().Be("access_token");
     result.RefreshToken.Should().Be("refresh_token");
        result.User.Email.Should().Be("test@example.com");
        }

        [Fact]
        public async Task LoginAsync_WithInvalidEmail_ShouldThrowUnauthorizedException()
        {
      // Arrange
       var loginDto = new LoginDto
    {
      Email = "invalid@example.com",
      Password = "password123"
   };

     _userRepositoryMock.Setup(r => r.GetByEmailAsync(loginDto.Email))
       .ReturnsAsync((User?)null);

  // Act & Assert
       await Assert.ThrowsAsync<UnauthorizedException>(() => 
          _authService.LoginAsync(loginDto));
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ShouldThrowUnauthorizedException()
 {
    // Arrange
   var loginDto = new LoginDto
            {
    Email = "test@example.com",
      Password = "wrongpassword"
   };

   var user = new User
   {
           Id = 1,
Email = "test@example.com",
      PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword")
       };

      _userRepositoryMock.Setup(r => r.GetByEmailAsync(loginDto.Email))
    .ReturnsAsync(user);

   // Act & Assert
    await Assert.ThrowsAsync<UnauthorizedException>(() => 
  _authService.LoginAsync(loginDto));
    }

 [Fact]
  public async Task LoginAsync_WithLockedAccount_ShouldThrowUnauthorizedException()
        {
     // Arrange
       var loginDto = new LoginDto
    {
         Email = "test@example.com",
      Password = "password123"
};

  var user = new User
       {
  Id = 1,
     Email = "test@example.com",
       PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
       LockedUntil = DateTime.UtcNow.AddMinutes(30)
   };

_userRepositoryMock.Setup(r => r.GetByEmailAsync(loginDto.Email))
        .ReturnsAsync(user);

     // Act & Assert
    await Assert.ThrowsAsync<UnauthorizedException>(() => 
     _authService.LoginAsync(loginDto));
        }

     [Fact]
        public async Task RegisterAsync_WithValidData_ShouldReturnTrue()
        {
  // Arrange
   var registerDto = new RegisterDto
            {
      Username = "newuser",
      Email = "new@example.com",
      Password = "password123",
     FirstName = "New",
    LastName = "User"
  };

    _userRepositoryMock.Setup(r => r.ExistsByEmailAsync(registerDto.Email))
     .ReturnsAsync(false);
      _userRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<User>()))
     .ReturnsAsync((User u) => u);
      _userRepositoryMock.Setup(r => r.AssignRoleAsync(It.IsAny<int>(), "Viewer"))
    .ReturnsAsync(true);

   // Act
     var result = await _authService.RegisterAsync(registerDto);

     // Assert
 result.Should().BeTrue();
     _userRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);
       _userRepositoryMock.Verify(r => r.AssignRoleAsync(It.IsAny<int>(), "Viewer"), Times.Once);
}

      [Fact]
        public async Task RegisterAsync_WithExistingEmail_ShouldThrowBadRequestException()
        {
   // Arrange
         var registerDto = new RegisterDto
      {
    Username = "newuser",
      Email = "existing@example.com",
   Password = "password123"
       };

   _userRepositoryMock.Setup(r => r.ExistsByEmailAsync(registerDto.Email))
      .ReturnsAsync(true);

  // Act & Assert
   await Assert.ThrowsAsync<BadRequestException>(() => 
       _authService.RegisterAsync(registerDto));
 }

        [Fact]
    public async Task ChangePasswordAsync_WithValidData_ShouldReturnTrue()
        {
       // Arrange
     var userId = 1;
       var changePasswordDto = new ChangePasswordDto
    {
 CurrentPassword = "oldpassword",
    NewPassword = "newpassword123"
     };

   var user = new User
   {
      Id = userId,
    Email = "test@example.com",
    PasswordHash = BCrypt.Net.BCrypt.HashPassword("oldpassword")
    };

       _userRepositoryMock.Setup(r => r.GetByIdAsync(userId))
    .ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<User>()))
       .ReturnsAsync((User u) => u);

            // Act
 var result = await _authService.ChangePasswordAsync(userId, changePasswordDto);

 // Assert
    result.Should().BeTrue();
   _userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

  [Fact]
        public async Task ChangePasswordAsync_WithInvalidCurrentPassword_ShouldThrowBadRequestException()
        {
  // Arrange
    var userId = 1;
    var changePasswordDto = new ChangePasswordDto
 {
 CurrentPassword = "wrongpassword",
       NewPassword = "newpassword123"
 };

   var user = new User
       {
 Id = userId,
    Email = "test@example.com",
         PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword")
    };

_userRepositoryMock.Setup(r => r.GetByIdAsync(userId))
         .ReturnsAsync(user);

   // Act & Assert
await Assert.ThrowsAsync<BadRequestException>(() => 
          _authService.ChangePasswordAsync(userId, changePasswordDto));
        }
    }
}
