using Gahar_Backend.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using FluentAssertions;
using Xunit;
using System.Security.Claims;

namespace Gahar_Backend.Tests.Filters
{
    public class PermissionAttributeTests
    {
     private readonly PermissionAttribute _attribute;
        private readonly AuthorizationFilterContext _context;

  public PermissionAttributeTests()
     {
    _attribute = new PermissionAttribute("Users.Create");

       var actionContext = new ActionContext(
      new DefaultHttpContext(),
     new RouteData(),
      new ActionDescriptor()
);
       
       _context = new AuthorizationFilterContext(
  actionContext,
      new List<IFilterMetadata>()
);
 }

        [Fact]
  public void OnAuthorization_WithUnauthenticatedUser_ShouldReturnUnauthorized()
     {
// Arrange
       _context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

       // Act
   _attribute.OnAuthorization(_context);

    // Assert
 _context.Result.Should().BeOfType<UnauthorizedResult>();
 }

        [Fact]
        public void OnAuthorization_WithAuthenticatedUserWithoutPermission_ShouldReturnForbidden()
        {
     // Arrange
   var claims = new List<Claim>
            {
      new Claim(ClaimTypes.NameIdentifier, "1"),
    new Claim(ClaimTypes.Name, "testuser")
      };
var identity = new ClaimsIdentity(claims, "TestAuth");
       _context.HttpContext.User = new ClaimsPrincipal(identity);

            // Act
    _attribute.OnAuthorization(_context);

       // Assert
   _context.Result.Should().BeOfType<ForbidResult>();
     }

     [Fact]
 public void OnAuthorization_WithAuthenticatedUserWithPermission_ShouldAllowAccess()
     {
  // Arrange
     var claims = new List<Claim>
   {
     new Claim(ClaimTypes.NameIdentifier, "1"),
 new Claim(ClaimTypes.Name, "testuser"),
       new Claim("Permission", "Users.Create"),
  new Claim("Permission", "Users.Edit")
      };
       var identity = new ClaimsIdentity(claims, "TestAuth");
  _context.HttpContext.User = new ClaimsPrincipal(identity);

  // Act
       _attribute.OnAuthorization(_context);

 // Assert
       _context.Result.Should().BeNull();
 }

     [Fact]
     public void OnAuthorization_WithAuthenticatedUserWithDifferentPermission_ShouldReturnForbidden()
        {
  // Arrange
   var claims = new List<Claim>
            {
 new Claim(ClaimTypes.NameIdentifier, "1"),
  new Claim(ClaimTypes.Name, "testuser"),
       new Claim("Permission", "Users.View"),
   new Claim("Permission", "Users.Edit")
   };
       var identity = new ClaimsIdentity(claims, "TestAuth");
  _context.HttpContext.User = new ClaimsPrincipal(identity);

     // Act
    _attribute.OnAuthorization(_context);

       // Assert
  _context.Result.Should().BeOfType<ForbidResult>();
        }

 [Fact]
        public void OnAuthorization_WithMultiplePermissions_ShouldCheckCorrectOne()
   {
// Arrange
var attribute = new PermissionAttribute("Users.Delete");
       
       var claims = new List<Claim>
      {
     new Claim(ClaimTypes.NameIdentifier, "1"),
       new Claim("Permission", "Users.Create"),
    new Claim("Permission", "Users.Edit"),
       new Claim("Permission", "Users.Delete")
     };
  var identity = new ClaimsIdentity(claims, "TestAuth");
  _context.HttpContext.User = new ClaimsPrincipal(identity);

  // Act
attribute.OnAuthorization(_context);

 // Assert
  _context.Result.Should().BeNull();
 }

     [Fact]
 public void PermissionAttribute_ShouldHaveCorrectAttributeUsage()
    {
       // Arrange & Act
    var attributes = typeof(PermissionAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false);

  // Assert
            attributes.Should().NotBeEmpty();
  var attributeUsage = (AttributeUsageAttribute)attributes[0];
  attributeUsage.ValidOn.Should().HaveFlag(AttributeTargets.Class);
       attributeUsage.ValidOn.Should().HaveFlag(AttributeTargets.Method);
}
    }
}
