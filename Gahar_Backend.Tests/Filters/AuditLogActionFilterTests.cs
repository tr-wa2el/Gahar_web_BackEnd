using Gahar_Backend.Filters;
using Gahar_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using FluentAssertions;
using Xunit;
using System.Security.Claims;

namespace Gahar_Backend.Tests.Filters
{
    public class AuditLogActionFilterTests
    {
 private readonly Mock<IAuditLogService> _auditLogServiceMock;
   private readonly AuditLogActionFilter _filter;
 private readonly ActionExecutingContext _executingContext;
 private readonly ActionExecutedContext _executedContext;

        public AuditLogActionFilterTests()
   {
  _auditLogServiceMock = new Mock<IAuditLogService>();
  _filter = new AuditLogActionFilter(_auditLogServiceMock.Object);

       var actionContext = new ActionContext(
new DefaultHttpContext(),
  new RouteData(),
   new ActionDescriptor { DisplayName = "TestAction" }
      );

          _executingContext = new ActionExecutingContext(
    actionContext,
       new List<IFilterMetadata>(),
     new Dictionary<string, object?>(),
      new object()
     );

_executedContext = new ActionExecutedContext(
 actionContext,
       new List<IFilterMetadata>(),
    new object()
 );
   }

        [Fact]
  public async Task OnActionExecutionAsync_WithSuccessfulExecution_ShouldLogAudit()
 {
       // Arrange
       var claims = new List<Claim>
            {
    new Claim(ClaimTypes.NameIdentifier, "1"),
  new Claim(ClaimTypes.Name, "testuser")
   };
  var identity = new ClaimsIdentity(claims, "TestAuth");
  _executingContext.HttpContext.User = new ClaimsPrincipal(identity);

       ActionExecutionDelegate next = () => Task.FromResult(_executedContext);

// Act
       await _filter.OnActionExecutionAsync(_executingContext, next);

  // Assert
     _auditLogServiceMock.Verify(
    s => s.LogAsync(
  1,
        It.IsAny<string>(),
   "Action",
  null,
   It.IsAny<string>(),
  null,
null
     ),
       Times.Once
  );
        }

        [Fact]
 public async Task OnActionExecutionAsync_WithUnauthenticatedUser_ShouldLogWithNullUserId()
 {
// Arrange
       _executingContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
  ActionExecutionDelegate next = () => Task.FromResult(_executedContext);

       // Act
  await _filter.OnActionExecutionAsync(_executingContext, next);

  // Assert
  _auditLogServiceMock.Verify(
       s => s.LogAsync(
      null,
      It.IsAny<string>(),
   "Action",
       null,
    It.IsAny<string>(),
     null,
  null
  ),
    Times.Once
       );
 }

        [Fact]
      public async Task OnActionExecutionAsync_WithException_ShouldNotLogAudit()
 {
       // Arrange
  var claims = new List<Claim>
       {
     new Claim(ClaimTypes.NameIdentifier, "1")
       };
  var identity = new ClaimsIdentity(claims, "TestAuth");
     _executingContext.HttpContext.User = new ClaimsPrincipal(identity);

            _executedContext.Exception = new Exception("Test exception");

ActionExecutionDelegate next = () => Task.FromResult(_executedContext);

   // Act
       await _filter.OnActionExecutionAsync(_executingContext, next);

  // Assert
       _auditLogServiceMock.Verify(
  s => s.LogAsync(
      It.IsAny<int?>(),
      It.IsAny<string>(),
     It.IsAny<string>(),
  It.IsAny<int?>(),
     It.IsAny<string>(),
     It.IsAny<object?>(),
It.IsAny<object?>()
    ),
  Times.Never
);
        }

      [Fact]
 public async Task OnActionExecutionAsync_ShouldLogActionDisplayName()
  {
    // Arrange
  var actionDescriptor = new ActionDescriptor { DisplayName = "CustomAction" };
var actionContext = new ActionContext(
            new DefaultHttpContext(),
  new RouteData(),
  actionDescriptor
       );

       var executingContext = new ActionExecutingContext(
 actionContext,
       new List<IFilterMetadata>(),
new Dictionary<string, object?>(),
      new object()
  );

       var executedContext = new ActionExecutedContext(
       actionContext,
 new List<IFilterMetadata>(),
      new object()
     );

  ActionExecutionDelegate next = () => Task.FromResult(executedContext);

       // Act
  await _filter.OnActionExecutionAsync(executingContext, next);

// Assert
  _auditLogServiceMock.Verify(
   s => s.LogAsync(
     It.IsAny<int?>(),
   "CustomAction",
        "Action",
       null,
      It.IsAny<string>(),
 null,
      null
       ),
  Times.Once
  );
        }

     [Fact]
public async Task OnActionExecutionAsync_ShouldIncludeActionDescriptionInLog()
  {
   // Arrange
  var claims = new List<Claim>
  {
       new Claim(ClaimTypes.NameIdentifier, "1")
  };
var identity = new ClaimsIdentity(claims, "TestAuth");
       _executingContext.HttpContext.User = new ClaimsPrincipal(identity);

ActionExecutionDelegate next = () => Task.FromResult(_executedContext);

       // Act
  await _filter.OnActionExecutionAsync(_executingContext, next);

// Assert
  _auditLogServiceMock.Verify(
   s => s.LogAsync(
   It.IsAny<int?>(),
  It.IsAny<string>(),
   "Action",
 null,
   It.Is<string>(desc => desc.Contains("Action executed:")),
 null,
      null
  ),
Times.Once
  );
        }
    }
}
