using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Extensions;

namespace Gahar_Backend.Attributes
{
    /// <summary>
    /// Rate limiting attribute for controllers and actions
    /// Supports different limits for admin and non-admin users
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RateLimitAttribute : Attribute, IAsyncActionFilter
 {
 private readonly int _maxRequestsForAdmin;
    private readonly int _maxRequestsForNonAdmin;
        private readonly int _windowSizeInSeconds;

 /// <summary>
        /// Initialize rate limit attribute
        /// </summary>
  /// <param name="maxRequestsForAdmin">Max requests for admin users (default: 1000)</param>
        /// <param name="maxRequestsForNonAdmin">Max requests for non-admin users (default: 1)</param>
        /// <param name="windowSizeInSeconds">Time window in seconds (default: 180 = 3 minutes)</param>
        public RateLimitAttribute(int maxRequestsForAdmin = 1000, int maxRequestsForNonAdmin = 1, int windowSizeInSeconds = 180)
        {
          _maxRequestsForAdmin = maxRequestsForAdmin;
       _maxRequestsForNonAdmin = maxRequestsForNonAdmin;
       _windowSizeInSeconds = windowSizeInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
var rateLimitService = context.HttpContext.RequestServices.GetRequiredService<IRateLimitService>();
        var user = context.HttpContext.User;

            // Get request identifier
  string identifier = GetRequestIdentifier(context.HttpContext);

         // Determine max requests based on user role
        int maxRequests = _maxRequestsForNonAdmin;
 
            if (user.HasRole("SuperAdmin") || user.HasRole("Admin"))
            {
    maxRequests = _maxRequestsForAdmin;
        }

      // Check rate limit
      var isAllowed = await rateLimitService.IsRequestAllowedAsync(identifier, maxRequests, _windowSizeInSeconds);

        if (!isAllowed)
   {
          var resetTime = await rateLimitService.GetResetTimeAsync(identifier);
     var remaining = await rateLimitService.GetRemainingRequestsAsync(identifier, maxRequests);

       context.Result = new ObjectResult(new
            {
  statusCode = StatusCodes.Status429TooManyRequests,
          message = "تم تجاوز حد الطلبات المسموح به",
     detail = $"يمكنك إرسال {maxRequests} طلب فقط خلال {_windowSizeInSeconds} ثانية",
   retryAfter = _windowSizeInSeconds,
       resetTime = resetTime
      })
      {
      StatusCode = StatusCodes.Status429TooManyRequests
     };

        context.HttpContext.Response.Headers.Add("Retry-After", _windowSizeInSeconds.ToString());
  return;
            }

            await next();
        }

        /// <summary>
        /// Get request identifier (User ID or IP Address)
     /// </summary>
        private string GetRequestIdentifier(HttpContext context)
     {
            // Try to get user ID from claims
       var userIdClaim = context.User?.FindFirst("sub")?.Value ??
       context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

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
    }
}
