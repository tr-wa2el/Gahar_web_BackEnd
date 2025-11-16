using System.Net;
using System.Text.Json;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Middleware
{
    public class ExceptionHandlingMiddleware
    {
  private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

     public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
    _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
    {
  try
   {
       await _next(context);
  }
            catch (Exception ex)
 {
    _logger.LogError(ex, "An unhandled exception occurred");
      await HandleExceptionAsync(context, ex);
    }
 }

     private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
    var statusCode = exception switch
 {
       UnauthorizedException => StatusCodes.Status401Unauthorized,
         ForbiddenException => StatusCodes.Status403Forbidden,
     NotFoundException => StatusCodes.Status404NotFound,
    BadRequestException => StatusCodes.Status400BadRequest,
   _ => StatusCodes.Status500InternalServerError
      };

    var response = new
  {
   StatusCode = statusCode,
  Message = exception.Message,
   Details = exception.InnerException?.Message
       };

  context.Response.ContentType = "application/json";
  context.Response.StatusCode = statusCode;

       return context.Response.WriteAsJsonAsync(response);
 }
    }
}
