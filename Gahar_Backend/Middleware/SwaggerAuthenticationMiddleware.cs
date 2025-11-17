using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Gahar_Backend.Middleware
{
    /// <summary>
    /// Middleware to protect Swagger endpoints with admin authentication
    /// Allows only authenticated users with admin role to access Swagger documentation
    /// </summary>
    public class SwaggerAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SwaggerAuthenticationMiddleware> _logger;

        public SwaggerAuthenticationMiddleware(RequestDelegate next, ILogger<SwaggerAuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the request is for Swagger endpoints
            var path = context.Request.Path.Value?.ToLower() ?? string.Empty;

            if (path.Contains("/swagger") || path.Contains("/api-docs"))
            {
                // In development, allow access to Swagger
                if (!context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
                {
                    // Check if user is authenticated
                    if (!context.User.Identity?.IsAuthenticated ?? true)
                    {
                        _logger.LogWarning("Unauthorized access attempt to Swagger at {Path} from {RemoteIpAddress}", 
   path, context.Connection.RemoteIpAddress);
             
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new { message = "Unauthorized access. Please provide a valid admin token." });
                        return;
                    }

                    // Check for admin role claim using ClaimTypes.Role
                    var hasAdminRole = context.User.Claims.Any(c => 
       c.Type == ClaimTypes.Role && c.Value == "Admin") ||
         context.User.IsInRole("Admin");

                    if (!hasAdminRole)
                    {
                        _logger.LogWarning("Non-admin user {UserId} attempted to access Swagger", 
        context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown");
           
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsJsonAsync(new { message = "Forbidden. Admin access required." });
                        return;
                    }

                    _logger.LogInformation("Admin user {UserId} accessing Swagger", 
context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown");
                }
                else
                {
                    _logger.LogInformation("Swagger access in Development mode from {RemoteIpAddress}", 
     context.Connection.RemoteIpAddress);
                }
            }

            await _next(context);
        }
    }
}
