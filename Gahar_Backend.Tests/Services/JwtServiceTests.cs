using Gahar_Backend.Models.Entities;
using Gahar_Backend.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Moq;
using FluentAssertions;
using Xunit;
using System.Security.Claims;

namespace Gahar_Backend.Tests.Services
{
    public class JwtServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
   private readonly JwtService _jwtService;

  public JwtServiceTests()
  {
          _configurationMock = new Mock<IConfiguration>();
            SetupConfiguration();
     _jwtService = new JwtService(_configurationMock.Object);
      }

        private void SetupConfiguration()
    {
            _configurationMock.Setup(c => c["JwtSettings:SecretKey"])
         .Returns("YourSuperSecretKeyHere_MinLength32Characters!");
   _configurationMock.Setup(c => c["JwtSettings:Issuer"])
        .Returns("GaharBackend");
  _configurationMock.Setup(c => c["JwtSettings:Audience"])
          .Returns("GaharClients");
    _configurationMock.Setup(c => c["JwtSettings:AccessTokenExpirationMinutes"])
       .Returns("60");
        }

[Fact]
   public void GenerateAccessToken_ShouldReturnValidToken()
     {
    // Arrange
        var user = new User
            {
    Id = 1,
  Username = "testuser",
      Email = "test@example.com",
 FirstName = "Test",
      LastName = "User"
    };
    var roles = new List<string> { "Admin" };
  var permissions = new List<string> { "Users.Create", "Users.Edit" };

   // Act
       var token = _jwtService.GenerateAccessToken(user, roles, permissions);

         // Assert
   token.Should().NotBeNullOrEmpty();
  }

        [Fact]
 public void GenerateRefreshToken_ShouldReturnValidToken()
  {
     // Act
          var token = _jwtService.GenerateRefreshToken();

// Assert
            token.Should().NotBeNullOrEmpty();
      token.Length.Should().BeGreaterThan(0);
    }

        [Fact]
        public void ValidateToken_WithValidToken_ShouldReturnClaimsPrincipal()
   {
        // Arrange
   var user = new User
    {
    Id = 1,
  Username = "testuser",
    Email = "test@example.com"
        };
  var roles = new List<string> { "Admin" };
            var permissions = new List<string> { "Users.Create" };
        var token = _jwtService.GenerateAccessToken(user, roles, permissions);

            // Act
   var principal = _jwtService.ValidateToken(token);

       // Assert
            principal.Should().NotBeNull();
      principal!.Identity!.IsAuthenticated.Should().BeTrue();
        }

        [Fact]
public void ValidateToken_WithInvalidToken_ShouldReturnNull()
        {
       // Arrange
     var invalidToken = "invalid.token.here";

      // Act
   var principal = _jwtService.ValidateToken(invalidToken);

       // Assert
     principal.Should().BeNull();
 }
    }
}
