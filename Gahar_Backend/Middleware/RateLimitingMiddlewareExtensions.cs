namespace Gahar_Backend.Middleware;

/// <summary>
/// Extension methods for rate limiting middleware
/// </summary>
public static class RateLimitingMiddlewareExtensions
{
    /// <summary>
    /// Add rate limiting middleware to the application
    /// Must be called before authentication middleware
  /// </summary>
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder app)
    {
  return app.UseMiddleware<RateLimitingMiddleware>();
    }
}
