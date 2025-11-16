using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Extensions;

namespace Gahar_Backend.Controllers;

/// <summary>
/// Controller for rate limiting information
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RateLimitController : ControllerBase
{
    private readonly IRateLimitService _rateLimitService;
private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<RateLimitController> _logger;

    public RateLimitController(
        IRateLimitService rateLimitService,
IHttpContextAccessor httpContextAccessor,
    ILogger<RateLimitController> logger)
    {
        _rateLimitService = rateLimitService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Get current rate limit status
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RateLimitInfo>> GetRateLimitStatus()
    {
        try
        {
    var identifier = GetRequestIdentifier();
          var info = await _rateLimitService.GetInfoAsync(identifier, maxRequests: 100, windowSeconds: 60);
     
          // Add headers
    var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
            {
        httpContext.Response.Headers.Add("X-RateLimit-Limit", "100");
   httpContext.Response.Headers.Add("X-RateLimit-Remaining", info.RemainingRequests.ToString());
         if (info.ResetTime.HasValue)
      {
           var resetTimestamp = new DateTimeOffset(info.ResetTime.Value).ToUnixTimeSeconds();
       httpContext.Response.Headers.Add("X-RateLimit-Reset", resetTimestamp.ToString());
   }
            }

   return Ok(info);
        }
        catch (Exception ex)
        {
    _logger.LogError(ex, "Error getting rate limit status");
  return BadRequest(new { message = "فشل الحصول على حالة حد الطلبات" });
        }
    }

    /// <summary>
    /// Get remaining requests
    /// </summary>
    [HttpGet("remaining")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> GetRemainingRequests()
    {
        try
        {
       var identifier = GetRequestIdentifier();
          var remaining = await _rateLimitService.GetRemainingRequestsAsync(identifier, maxRequests: 100);
            
            return Ok(new
       {
         remaining = remaining,
    limit = 100,
        window = "1 دقيقة"
   });
        }
        catch (Exception ex)
        {
       _logger.LogError(ex, "Error getting remaining requests");
            return BadRequest(new { message = "فشل الحصول على الطلبات المتبقية" });
     }
    }

 /// <summary>
    /// Reset rate limit (Admin only)
    /// </summary>
    [HttpPost("reset")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> ResetRateLimit()
    {
        try
     {
            // Only allow admins or self
            var userId = User.GetUserId();
       var identifier = GetRequestIdentifier();

   // Check if user is admin (you can add proper authorization logic here)
         var userIdFromClaim = User.FindFirst("sub")?.Value ?? User.FindFirst("nameid")?.Value;
     
       if (!identifier.Contains(userIdFromClaim ?? ""))
    {
                return Unauthorized(new { message = "ليس لديك صلاحية لإعادة تعيين حد الطلبات لمستخدم آخر" });
    }

   await _rateLimitService.ResetAsync(identifier);
            
  return Ok(new { message = "تم إعادة تعيين حد الطلبات بنجاح" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting rate limit");
     return BadRequest(new { message = "فشل إعادة تعيين حد الطلبات" });
        }
    }

    /// <summary>
    /// Get rate limit reset time
    /// </summary>
    [HttpGet("reset-time")]
  [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> GetResetTime()
    {
      try
        {
    var identifier = GetRequestIdentifier();
            var resetTime = await _rateLimitService.GetResetTimeAsync(identifier);
   
    if (!resetTime.HasValue)
       {
      return Ok(new
                {
     resetTime = (DateTime?)null,
            message = "لا توجد قيود حالية على الطلبات"
      });
  }

    var secondsUntilReset = (int)(resetTime.Value - DateTime.UtcNow).TotalSeconds;
 
      return Ok(new
       {
          resetTime = resetTime,
       secondsUntilReset = Math.Max(0, secondsUntilReset),
        timestamp = new DateTimeOffset(resetTime.Value).ToUnixTimeSeconds()
      });
        }
        catch (Exception ex)
  {
  _logger.LogError(ex, "Error getting reset time");
         return BadRequest(new { message = "فشل الحصول على وقت إعادة التعيين" });
      }
    }

    /// <summary>
    /// Get request identifier
    /// </summary>
    private string GetRequestIdentifier()
    {
     // Try to get user ID from claims
        var userIdClaim = User?.FindFirst("sub")?.Value ?? 
        User?.FindFirst("nameid")?.Value;
        
    if (!string.IsNullOrEmpty(userIdClaim))
        {
      return $"user_{userIdClaim}";
      }

  // Fall back to IP address
      var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
 if (string.IsNullOrEmpty(ipAddress))
    {
  ipAddress = _httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].ToString().Split(',').FirstOrDefault() ?? "unknown";
        }

     return $"ip_{ipAddress}";
    }
}
