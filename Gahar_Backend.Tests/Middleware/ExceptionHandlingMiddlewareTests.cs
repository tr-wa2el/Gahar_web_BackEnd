using Gahar_Backend.Middleware;
using Gahar_Backend.Utilities.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using Xunit;
using System.Text.Json;

namespace Gahar_Backend.Tests.Middleware
{
    public class ExceptionHandlingMiddlewareTests
    {
  private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
 private readonly ExceptionHandlingMiddleware _middleware;

 public ExceptionHandlingMiddlewareTests()
{
      _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
 _middleware = new ExceptionHandlingMiddleware(
       (innerHttpContext) => Task.CompletedTask,
    _loggerMock.Object
    );
    }

     [Fact]
        public async Task InvokeAsync_WithNoException_ShouldContinuePipeline()
   {
  // Arrange
  var context = new DefaultHttpContext();
   var nextCalled = false;

  var middleware = new ExceptionHandlingMiddleware(
     (innerHttpContext) =>
    {
  nextCalled = true;
   return Task.CompletedTask;
       },
      _loggerMock.Object
     );

// Act
     await middleware.InvokeAsync(context);

   // Assert
nextCalled.Should().BeTrue();
  }

     [Fact]
 public async Task InvokeAsync_WithNotFoundException_ShouldReturn404()
     {
// Arrange
       var context = new DefaultHttpContext();
context.Response.Body = new MemoryStream();

  var middleware = new ExceptionHandlingMiddleware(
       (innerHttpContext) => throw new NotFoundException("Resource not found"),
      _loggerMock.Object
);

   // Act
await middleware.InvokeAsync(context);

  // Assert
       context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
context.Response.ContentType.Should().StartWith("application/json");
  }

  [Fact]
public async Task InvokeAsync_WithUnauthorizedException_ShouldReturn401()
  {
   // Arrange
       var context = new DefaultHttpContext();
       context.Response.Body = new MemoryStream();

var middleware = new ExceptionHandlingMiddleware(
 (innerHttpContext) => throw new UnauthorizedException("Unauthorized access"),
    _loggerMock.Object
    );

    // Act
await middleware.InvokeAsync(context);

  // Assert
context.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
  }

 [Fact]
        public async Task InvokeAsync_WithForbiddenException_ShouldReturn403()
 {
  // Arrange
  var context = new DefaultHttpContext();
   context.Response.Body = new MemoryStream();

       var middleware = new ExceptionHandlingMiddleware(
    (innerHttpContext) => throw new ForbiddenException("Access forbidden"),
   _loggerMock.Object
  );

// Act
    await middleware.InvokeAsync(context);

    // Assert
   context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
 }

      [Fact]
      public async Task InvokeAsync_WithBadRequestException_ShouldReturn400()
    {
  // Arrange
  var context = new DefaultHttpContext();
       context.Response.Body = new MemoryStream();

  var middleware = new ExceptionHandlingMiddleware(
(innerHttpContext) => throw new BadRequestException("Bad request"),
   _loggerMock.Object
       );

 // Act
       await middleware.InvokeAsync(context);

// Assert
   context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
}

        [Fact]
        public async Task InvokeAsync_WithGenericException_ShouldReturn500()
 {
   // Arrange
var context = new DefaultHttpContext();
 context.Response.Body = new MemoryStream();

 var middleware = new ExceptionHandlingMiddleware(
   (innerHttpContext) => throw new Exception("Internal server error"),
       _loggerMock.Object
       );

       // Act
  await middleware.InvokeAsync(context);

// Assert
       context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
 }

     [Fact]
 public async Task InvokeAsync_WithException_ShouldLogError()
     {
// Arrange
       var context = new DefaultHttpContext();
context.Response.Body = new MemoryStream();
    var exception = new Exception("Test exception");

  var middleware = new ExceptionHandlingMiddleware(
   (innerHttpContext) => throw exception,
  _loggerMock.Object
  );

    // Act
  await middleware.InvokeAsync(context);

  // Assert
  _loggerMock.Verify(
      x => x.Log(
      LogLevel.Error,
   It.IsAny<EventId>(),
    It.IsAny<It.IsAnyType>(),
     exception,
 It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
       Times.Once);
 }

 [Fact]
        public async Task InvokeAsync_WithException_ShouldReturnJsonResponse()
  {
   // Arrange
  var context = new DefaultHttpContext();
     var responseBody = new MemoryStream();
     context.Response.Body = responseBody;

  var middleware = new ExceptionHandlingMiddleware(
     (innerHttpContext) => throw new NotFoundException("Resource not found"),
       _loggerMock.Object
    );

// Act
   await middleware.InvokeAsync(context);

// Assert
    responseBody.Seek(0, SeekOrigin.Begin);
    var reader = new StreamReader(responseBody);
    var responseText = await reader.ReadToEndAsync();
       
   responseText.Should().NotBeNullOrEmpty();
   responseText.Should().Contain("Resource not found");
responseText.Should().Contain("404");
        }

 [Fact]
   public async Task InvokeAsync_WithExceptionWithInnerException_ShouldIncludeDetails()
   {
   // Arrange
var context = new DefaultHttpContext();
  var responseBody = new MemoryStream();
       context.Response.Body = responseBody;

var innerException = new Exception("Inner error");
       var outerException = new Exception("Outer error", innerException);

       var middleware = new ExceptionHandlingMiddleware(
  (innerHttpContext) => throw outerException,
       _loggerMock.Object
       );

     // Act
       await middleware.InvokeAsync(context);

       // Assert
responseBody.Seek(0, SeekOrigin.Begin);
  var reader = new StreamReader(responseBody);
       var responseText = await reader.ReadToEndAsync();
       
     responseText.Should().Contain("Outer error");
       responseText.Should().Contain("Inner error");
 }
    }
}
