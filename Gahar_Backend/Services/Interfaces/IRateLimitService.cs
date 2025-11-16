namespace Gahar_Backend.Services.Interfaces;

/// <summary>
/// Service interface for rate limiting
/// </summary>
public interface IRateLimitService
{
    /// <summary>
    /// Check if request is allowed
    /// </summary>
    Task<bool> IsRequestAllowedAsync(string identifier, int maxRequests = 100, int windowSeconds = 60);

    /// <summary>
    /// Get remaining requests
    /// </summary>
    Task<int> GetRemainingRequestsAsync(string identifier, int maxRequests = 100);

    /// <summary>
    /// Get reset time for rate limit
    /// </summary>
 Task<DateTime?> GetResetTimeAsync(string identifier);

    /// <summary>
    /// Reset rate limit for identifier
    /// </summary>
    Task ResetAsync(string identifier);

    /// <summary>
    /// Get rate limit info
    /// </summary>
 Task<RateLimitInfo> GetInfoAsync(string identifier, int maxRequests = 100, int windowSeconds = 60);
}

/// <summary>
/// Rate limit information
/// </summary>
public class RateLimitInfo
{
    public string Identifier { get; set; } = string.Empty;
 public int CurrentRequests { get; set; }
    public int MaxRequests { get; set; }
    public int RemainingRequests { get; set; }
    public DateTime? ResetTime { get; set; }
    public int ResetInSeconds { get; set; }
    public bool IsLimited { get; set; }
}
