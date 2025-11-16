using System.Collections.Concurrent;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Services.Implementations;

/// <summary>
/// Service implementation for rate limiting (In-Memory)
/// </summary>
public class RateLimitService : IRateLimitService
{
    private readonly ILogger<RateLimitService> _logger;
    private static readonly ConcurrentDictionary<string, (int count, DateTime resetTime)> _requestCounts = new();

  public RateLimitService(ILogger<RateLimitService> logger)
    {
_logger = logger;
      
        // Start periodic cleanup
        _ = CleanupExpiredEntriesAsync();
    }

    /// <summary>
  /// Check if request is allowed
    /// </summary>
    public async Task<bool> IsRequestAllowedAsync(string identifier, int maxRequests = 100, int windowSeconds = 60)
    {
        if (string.IsNullOrEmpty(identifier))
        {
         return false;
        }

        return await Task.Run(() =>
    {
     var now = DateTime.UtcNow;

  var entry = _requestCounts.AddOrUpdate(
         identifier,
    (1, now.AddSeconds(windowSeconds)),
  (key, existing) =>
        {
      // Check if window has expired
       if (now >= existing.resetTime)
        {
    // Reset window
 return (1, now.AddSeconds(windowSeconds));
    }
           else
  {
         // Increment counter
        return (existing.count + 1, existing.resetTime);
        }
      });

 // Check if limit exceeded
 return entry.count <= maxRequests;
        });
    }

    /// <summary>
    /// Get remaining requests
    /// </summary>
 public async Task<int> GetRemainingRequestsAsync(string identifier, int maxRequests = 100)
    {
        return await Task.Run(() =>
        {
 if (!_requestCounts.TryGetValue(identifier, out var entry))
            {
        return maxRequests;
       }

            // Check if window has expired
            if (DateTime.UtcNow >= entry.resetTime)
        {
       return maxRequests;
        }

      return Math.Max(0, maxRequests - entry.count);
        });
    }

    /// <summary>
    /// Get reset time for rate limit
    /// </summary>
    public async Task<DateTime?> GetResetTimeAsync(string identifier)
    {
   return await Task.Run(() =>
        {
   if (!_requestCounts.TryGetValue(identifier, out var entry))
       {
        return (DateTime?)null;
        }

    // Check if window has expired
if (DateTime.UtcNow >= entry.resetTime)
          {
      return (DateTime?)null;
            }

        return entry.resetTime;
   });
    }

    /// <summary>
    /// Reset rate limit for identifier
    /// </summary>
    public async Task ResetAsync(string identifier)
    {
    await Task.Run(() =>
        {
     _requestCounts.TryRemove(identifier, out _);
    _logger.LogInformation($"Rate limit reset for identifier: {identifier}");
        });
    }

    /// <summary>
    /// Get rate limit info
    /// </summary>
    public async Task<RateLimitInfo> GetInfoAsync(string identifier, int maxRequests = 100, int windowSeconds = 60)
    {
        return await Task.Run(() =>
        {
            var now = DateTime.UtcNow;

   if (!_requestCounts.TryGetValue(identifier, out var entry))
            {
    return new RateLimitInfo
      {
   Identifier = identifier,
   CurrentRequests = 0,
             MaxRequests = maxRequests,
  RemainingRequests = maxRequests,
      ResetTime = now.AddSeconds(windowSeconds),
           ResetInSeconds = windowSeconds,
   IsLimited = false
                };
    }

        // Check if window has expired
     if (now >= entry.resetTime)
     {
       return new RateLimitInfo
       {
    Identifier = identifier,
   CurrentRequests = 0,
       MaxRequests = maxRequests,
          RemainingRequests = maxRequests,
         ResetTime = now.AddSeconds(windowSeconds),
          ResetInSeconds = windowSeconds,
   IsLimited = false
                };
     }

            var remaining = Math.Max(0, maxRequests - entry.count);
            var resetInSeconds = (int)(entry.resetTime - now).TotalSeconds;

            return new RateLimitInfo
        {
     Identifier = identifier,
 CurrentRequests = entry.count,
                MaxRequests = maxRequests,
 RemainingRequests = remaining,
    ResetTime = entry.resetTime,
            ResetInSeconds = resetInSeconds,
     IsLimited = remaining == 0
      };
        });
    }

    /// <summary>
    /// Cleanup expired entries periodically
  /// </summary>
    private async Task CleanupExpiredEntriesAsync()
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
