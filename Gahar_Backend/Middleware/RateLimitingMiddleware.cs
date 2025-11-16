using System.Collections.Concurrent;
using System.Diagnostics;

namespace Gahar_Backend.Middleware;

/// <summary>
/// Custom Rate Limiting Middleware
/// 100 requests per minute per user/IP
/// </summary>
public class RateLimitingMiddleware
{
  private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    
    // Store request counts: key = userId_or_IP, value = (count, resetTime)
    private static readonly ConcurrentDictionary<string, (int count, DateTime resetTime)> _requestCounts = new();
  
    private const int MaxRequestsPerMinute = 100;
    private const int WindowSizeInSeconds = 60;

public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
 
        // Start cleanup task
        _ = CleanupExpiredEntries();
    }

  public async Task InvokeAsync(HttpContext context)
    {
        // Skip rate limiting for health check endpoints
  if (context.Request.Path.StartsWithSegments("/health") ||
         context.Request.Path.StartsWithSegments("/swagger"))
   {
        await _next(context);
            return;
        }

        // Get identifier (User ID or IP Address)
      string identifier = GetRequestIdentifier(context);

        // Check rate limit
     if (!IsRequestAllowed(identifier))
        {
   _logger.LogWarning($"Rate limit exceeded for identifier: {identifier}");
      
        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
   context.Response.ContentType = "application/json";
        
    var response = new
            {
      message = "تم تجاوز حد الطلبات المسموح به",
   retryAfter = 60,
                detail = $"الحد الأقصى: {MaxRequestsPerMinute} طلب في الدقيقة"
            };
   
            context.Response.Headers.Add("Retry-After", "60");
            await context.Response.WriteAsJsonAsync(response);
            return;
        }

        await _next(context);
    }

    /// <summary>
    /// Get request identifier (User ID or IP Address)
    /// </summary>
    private string GetRequestIdentifier(HttpContext context)
    {
        // Try to get user ID from claims
      var userIdClaim = context.User?.FindFirst("sub")?.Value ?? 
         context.User?.FindFirst("nameid")?.Value;
   
     if (!string.IsNullOrEmpty(userIdClaim))
   {
 return $"user_{userIdClaim}";
   }

        // Fall back to IP address
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = context.Request.Headers["X-Forwarded-For"].ToString().Split(',').FirstOrDefault() ?? "unknown";
        }

    return $"ip_{ipAddress}";
    }

    /// <summary>
  /// Check if request is allowed
    /// </summary>
    private bool IsRequestAllowed(string identifier)
    {
        var now = DateTime.UtcNow;

      // Get or create entry
        var entry = _requestCounts.AddOrUpdate(
  identifier,
            (1, now.AddSeconds(WindowSizeInSeconds)),
            (key, existing) =>
       {
 // Check if window has expired
     if (now >= existing.resetTime)
{
// Reset window
  return (1, now.AddSeconds(WindowSizeInSeconds));
    }
    else
         {
      // Increment counter
              return (existing.count + 1, existing.resetTime);
     }
      });

     // Check if limit exceeded
        return entry.count <= MaxRequestsPerMinute;
    }

    /// <summary>
    /// Cleanup expired entries periodically
    /// </summary>
    private async Task CleanupExpiredEntries()
    {
        while (true)
        {
  try
        {
                await Task.Delay(TimeSpan.FromMinutes(5)); // Run cleanup every 5 minutes

    var now = DateTime.UtcNow;
     var expiredKeys = _requestCounts
   .Where(x => x.Value.resetTime < now)
             .Select(x => x.Key)
       .ToList();

         foreach (var key in expiredKeys)
      {
      _requestCounts.TryRemove(key, out _);
        }

       _logger.LogInformation($"Rate limit cleanup: removed {expiredKeys.Count} expired entries");
 }
   catch (Exception ex)
            {
     _logger.LogError(ex, "Error in rate limit cleanup task");
 }
        }
    }
}

/// <summary>
/// Extension method for rate limiting middleware
/// </summary>
public static class RateLimitingMiddlewareExtensions
{
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
    {
    return builder.UseMiddleware<RateLimitingMiddleware>();
    }
}
