using System.Collections.Concurrent;
using System.Security.Claims;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Extensions;

namespace Gahar_Backend.Middleware;

/// <summary>
/// Global Rate Limiting Middleware for entire application
/// 
/// Rules:
/// - Non-admin users (POST/PUT): 1 request per 180 seconds (3 minutes)
/// - Non-admin users (GET/DELETE/PATCH): 50 requests per 180 seconds
/// - Admin users: 1000 requests per 180 seconds
/// - Excluded endpoints: /health, /swagger, /auth/login, /auth/register
/// </summary>
public class RateLimitingMiddleware
{
 private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;

    // Store request counts: key = userId_or_IP, value = (count, resetTime)
    private static readonly ConcurrentDictionary<string, (int count, DateTime resetTime)> _requestCounts = new();

    // Rate limit windows and counts per user type and HTTP method
    private const int NonAdminWriteLimit = 1; // POST, PUT: 1 request
    private const int NonAdminReadLimit = 50;           // GET, DELETE, PATCH: 50 requests
    private const int AdminLimit = 1000;        // All methods: 1000 requests
    private const int WindowSizeInSeconds = 180;        // 3 minutes

    public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _logger = logger;

        // Start cleanup task
        _ = CleanupExpiredEntries();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip rate limiting for excluded endpoints
        if (ShouldSkipRateLimit(context.Request))
   {
            await _next(context);
            return;
        }

        // Get identifier (User ID or IP Address)
        string identifier = GetRequestIdentifier(context);

// Determine rate limit based on user role and HTTP method
        int maxRequests = GetMaxRequests(context, identifier);

        // Check rate limit
        if (!IsRequestAllowed(identifier, maxRequests))
        {
            _logger.LogWarning($"Rate limit exceeded for identifier: {identifier}, method: {context.Request.Method}");

            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.ContentType = "application/json";

            var resetTime = GetResetTime(identifier);
   var response = new
     {
     statusCode = StatusCodes.Status429TooManyRequests,
      message = "تم تجاوز حد الطلبات المسموح به",
    detail = $"يمكنك إرسال {maxRequests} طلب فقط خلال {WindowSizeInSeconds} ثانية",
     retryAfter = WindowSizeInSeconds,
      resetTime = resetTime
  };

     context.Response.Headers.Add("Retry-After", WindowSizeInSeconds.ToString());
  await context.Response.WriteAsJsonAsync(response);
     return;
        }

        await _next(context);
    }

    /// <summary>
    /// Determine if request should skip rate limiting
    /// </summary>
    private bool ShouldSkipRateLimit(HttpRequest request)
    {
        // Skip health check endpoints
        if (request.Path.StartsWithSegments("/health"))
            return true;

        // Skip Swagger/API documentation
        if (request.Path.StartsWithSegments("/swagger") ||
            request.Path.StartsWithSegments("/api-docs"))
            return true;

        // Skip authentication endpoints
        if (request.Path.StartsWithSegments("/api/auth/login") ||
request.Path.StartsWithSegments("/api/auth/register") ||
  request.Path.StartsWithSegments("/api/auth/refresh"))
            return true;

        // Skip OPTIONS requests (CORS preflight)
  if (request.Method == HttpMethods.Options)
            return true;

        return false;
    }

    /// <summary>
    /// Get request identifier (User ID or IP Address)
    /// </summary>
    private string GetRequestIdentifier(HttpContext context)
    {
        // Try to get user ID from claims (authenticated users)
        var userIdClaim = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
    context.User?.FindFirst("sub")?.Value;

      if (!string.IsNullOrEmpty(userIdClaim))
   {
            return $"user_{userIdClaim}";
     }

        // Fall back to IP address for anonymous requests
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(ipAddress))
  {
            ipAddress = context.Request.Headers["X-Forwarded-For"].ToString().Split(',').FirstOrDefault() ?? "unknown";
     }

        return $"ip_{ipAddress}";
    }

    /// <summary>
    /// Get maximum requests allowed based on user role and HTTP method
    /// </summary>
    private int GetMaxRequests(HttpContext context, string identifier)
    {
        // Check if user is authenticated and has admin role
 if (IsAdminUser(context))
        {
          return AdminLimit;
    }

   // Non-admin users have stricter limits based on HTTP method
        string method = context.Request.Method.ToUpper();

// POST and PUT operations are more restricted (1 request per 3 minutes)
        if (method == HttpMethods.Post || method == HttpMethods.Put)
        {
          return NonAdminWriteLimit;
      }

        // GET, DELETE, PATCH operations have higher limit (50 requests per 3 minutes)
        return NonAdminReadLimit;
    }

    /// <summary>
    /// Check if user has admin role
    /// </summary>
    private bool IsAdminUser(HttpContext context)
    {
        // Check for SuperAdmin or Admin roles
    return context.User?.IsInRole("SuperAdmin") == true ||
               context.User?.IsInRole("Admin") == true;
    }

    /// <summary>
    /// Check if request is allowed
    /// </summary>
    private bool IsRequestAllowed(string identifier, int maxRequests)
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
        return entry.count <= maxRequests;
    }

    /// <summary>
    /// Get reset time for a rate limit identifier
    /// </summary>
    private DateTime? GetResetTime(string identifier)
    {
        if (_requestCounts.TryGetValue(identifier, out var entry))
        {
        var now = DateTime.UtcNow;
            if (now < entry.resetTime)
   {
      return entry.resetTime;
    }
    }

        return null;
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

        if (expiredKeys.Count > 0)
      {
   _logger.LogInformation($"Rate limit cleanup: removed {expiredKeys.Count} expired entries");
    }
        }
    catch (Exception ex)
     {
     _logger.LogError(ex, "Error in rate limit cleanup task");
    }
      }
  }
}
