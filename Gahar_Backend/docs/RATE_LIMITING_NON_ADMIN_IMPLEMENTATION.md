# Rate Limiting Implementation - Non-Admin User Restrictions

## Overview
This document describes the implementation of rate limiting for non-admin users in the Gahar Backend API. The feature restricts non-admin users to **1 request per 3 minutes** (180 seconds) for write operations (POST, PUT) while admin users have higher limits.

## Architecture

### 1. RateLimitAttribute
**Location:** `Gahar_Backend\Attributes\RateLimitAttribute.cs`

A custom attribute that implements `IAsyncActionFilter` to apply rate limiting at the controller action level.

**Features:**
- Role-based rate limiting (different limits for admin vs. non-admin users)
- Customizable max requests and time window
- User identifier resolution (User ID or IP address)
- Returns HTTP 429 (Too Many Requests) when limit exceeded
- Provides retry-after information

**Default Values:**
- Admin users: 1000 requests per window
- Non-admin users: 1 request per window
- Window size: 180 seconds (3 minutes)

**Usage Example:**
```csharp
[HttpPost]
[RateLimit(maxRequestsForAdmin: 1000, maxRequestsForNonAdmin: 1, windowSizeInSeconds: 180)]
public async Task<ActionResult> CreateShortLink([FromBody] CreateShortLinkDto dto)
{
    // Implementation
}
```

### 2. IRateLimitService
**Location:** `Gahar_Backend\Services\Interfaces\IRateLimitService.cs`

Service interface for managing rate limit tracking and validation.

**Methods:**
- `IsRequestAllowedAsync()` - Check if request is allowed for an identifier
- `GetRemainingRequestsAsync()` - Get remaining requests in current window
- `GetResetTimeAsync()` - Get when the current window resets
- `ResetAsync()` - Reset rate limit for an identifier
- `GetInfoAsync()` - Get detailed rate limit information

### 3. RateLimitService
**Location:** `Gahar_Backend\Services\Implementations\RateLimitService.cs`

In-memory implementation of rate limiting service using `ConcurrentDictionary`.

**Key Features:**
- Thread-safe in-memory storage
- Automatic window expiration
- Periodic cleanup of expired entries
- No external dependencies (no Redis required)

## Implementation Details

### Applied to ShortLinksController

Rate limiting is applied to the following endpoints:

#### 1. Create Short Link
```csharp
[HttpPost]
[RateLimit(maxRequestsForAdmin: 1000, maxRequestsForNonAdmin: 1, windowSizeInSeconds: 180)]
public async Task<ActionResult<ShortLinkDto>> CreateShortLink([FromBody] CreateShortLinkDto dto)
```

**Restrictions:**
- Non-admin users: 1 request per 3 minutes
- Admin users: 1000 requests per 3 minutes
- HTTP Status: 429 Too Many Requests when exceeded

#### 2. Update Short Link
```csharp
[HttpPut("{id}")]
[RateLimit(maxRequestsForAdmin: 1000, maxRequestsForNonAdmin: 1, windowSizeInSeconds: 180)]
public async Task<ActionResult<ShortLinkDto>> UpdateShortLink(int id, [FromBody] UpdateShortLinkDto dto)
```

**Restrictions:**
- Non-admin users: 1 request per 3 minutes
- Admin users: 1000 requests per 3 minutes
- HTTP Status: 429 Too Many Requests when exceeded

#### 3. Regenerate QR Code
```csharp
[HttpPost("{id}/regenerate-qr")]
[RateLimit(maxRequestsForAdmin: 1000, maxRequestsForNonAdmin: 1, windowSizeInSeconds: 180)]
public async Task<ActionResult<QrCodeResultDto>> RegenerateQrCode(int id, [FromQuery] string? logoUrl = null)
```

**Restrictions:**
- Non-admin users: 1 request per 3 minutes
- Admin users: 1000 requests per 3 minutes
- HTTP Status: 429 Too Many Requests when exceeded

## How It Works

### Request Flow

1. **Request arrives at controller action**
   ```
   POST /api/shortlinks
   Authorization: Bearer <token>
```

2. **RateLimitAttribute filter executes**
   - Extracts user ID from JWT claims
   - Creates identifier: `user_{userId}`
   - Determines max requests based on user roles
   - Checks rate limit via `IRateLimitService`

3. **Rate Limit Check**
   ```
   For non-admin users:
   - Check if identifier has exceeded 1 request in 180 seconds
   - If exceeded: Return 429 with retry-after header
   - If allowed: Continue to controller action
   ```

4. **Successful Response**
   ```json
   {
     "id": 1,
     "shortCode": "abc123",
     "originalUrl": "https://example.com/very/long/url",
     "qrCode": "data:image/png;base64,..."
   }
   ```

5. **Rate Limit Exceeded Response**
   ```json
   {
     "statusCode": 429,
     "message": "تم تجاوز حد الطلبات المسموح به",
     "detail": "يمكنك إرسال 1 طلب فقط خلال 180 ثانية",
     "retryAfter": 180,
     "resetTime": "2024-01-15T10:05:30Z"
   }
   ```

### Role Detection

The attribute checks user roles in the following order:
1. `SuperAdmin` role
2. `Admin` role

If either role is present, the user receives admin limits.

### Identifier Resolution

The system identifies users by:
1. **User ID** (from JWT claims) - Primary method
   - Claim name: `sub` or `nameid`
   - Format: `user_{userId}`

2. **IP Address** (fallback) - For anonymous requests
   - Extracted from `RemoteIpAddress` or `X-Forwarded-For` header
   - Format: `ip_{ipAddress}`

## Testing

### Test Case 1: Non-Admin User - Single Request
```bash
curl -X POST http://localhost:5000/api/shortlinks \
  -H "Authorization: Bearer <non-admin-token>" \
  -H "Content-Type: application/json" \
  -d '{"originalUrl": "https://example.com"}'

# Response: 201 Created ✓
```

### Test Case 2: Non-Admin User - Second Request (Too Soon)
```bash
curl -X POST http://localhost:5000/api/shortlinks \
  -H "Authorization: Bearer <non-admin-token>" \
  -H "Content-Type: application/json" \
  -d '{"originalUrl": "https://example.com"}'

# Response: 429 Too Many Requests
# Headers: Retry-After: 180
```

### Test Case 3: Admin User - Multiple Requests
```bash
# Send multiple requests (up to 1000)
for i in {1..10}; do
  curl -X POST http://localhost:5000/api/shortlinks \
    -H "Authorization: Bearer <admin-token>" \
    -H "Content-Type: application/json" \
    -d '{"originalUrl": "https://example.com/'$i'"}'
done

# All responses: 201 Created ✓
```

## Configuration

### Modifying Rate Limit Parameters

To change rate limit settings, modify the `[RateLimit]` attribute on the controller actions:

```csharp
[RateLimit(
    maxRequestsForAdmin: 500,      // Admin users: 500 requests
    maxRequestsForNonAdmin: 2,      // Non-admin users: 2 requests
    windowSizeInSeconds: 300   // Time window: 5 minutes
)]
```

### For Different Endpoints

Apply the attribute to any controller action:

```csharp
// Allow 3 requests per 5 minutes for non-admin users
[RateLimit(maxRequestsForNonAdmin: 3, windowSizeInSeconds: 300)]
[HttpPost("upload")]
public async Task<ActionResult> Upload([FromForm] IFormFile file)
{
    // Implementation
}
```

## Storage & Performance

### In-Memory Storage
- Uses `ConcurrentDictionary<string, (int count, DateTime resetTime)>`
- Lightweight and fast
- No external dependencies
- Perfect for single-server deployments

### Cleanup
- Automatic periodic cleanup of expired entries
- Entries older than the window size are automatically removed
- No database calls required

### Scalability
For distributed deployments (multiple servers), consider:
1. **Redis-based implementation** - For cross-server rate limiting
2. **Database storage** - For persistence across restarts
3. **API Gateway** - CloudFlare, Azure API Management, etc.

## Best Practices

1. **Use Meaningful Limits**: Adjust limits based on your API usage patterns
2. **Log Rate Limiting Events**: Enable logging for monitoring
3. **Inform Users**: Return clear error messages with retry-after info
4. **Monitor**: Track rate limit metrics to understand user behavior
5. **Scale Appropriately**: Consider Redis for distributed systems

## Error Handling

### 429 Response Format
```json
{
  "statusCode": 429,
  "message": "تم تجاوز حد الطلبات المسموح به",
  "detail": "يمكنك إرسال 1 طلب فقط خلال 180 ثانية",
  "retryAfter": 180,
  "resetTime": "2024-01-15T10:05:30Z"
}
```

### Response Headers
- `Retry-After`: Seconds to wait before retrying (matches window size)

## Security Considerations

1. **User Identification**: Uses secure JWT claims from authenticated tokens
2. **IP Address Fallback**: Only used for anonymous/unauthenticated requests
3. **Role-Based Differentiation**: Prevents privilege escalation
4. **Window-Based Resets**: Automatic time-window management prevents permanent lockouts

## Future Enhancements

1. **Redis Backend**: Replace in-memory storage for distributed deployments
2. **Custom Strategies**: Allow different limits for different endpoints
3. **User-Tier Based Limits**: Different limits for different user plans
4. **Rate Limit Headers**: Include `X-RateLimit-*` headers in all responses
5. **Metrics & Monitoring**: Detailed statistics about rate limit usage
6. **Whitelist/Blacklist**: Exception lists for specific users/IPs

## Troubleshooting

### Users Getting 429 Too Soon
1. Verify the token contains correct role claims
2. Check `ClaimTypes.NameIdentifier` is present in JWT
3. Review the rate limit window settings

### Rate Limits Not Applied
1. Ensure `RateLimitAttribute` is added to the action
2. Verify `IRateLimitService` is registered in DI container
3. Check that the request is authenticated (not `[AllowAnonymous]`)

### Memory Usage Concerns
1. Check cleanup task is running (automatic via service)
2. Monitor for stuck entries (shouldn't happen, but possible in edge cases)
3. Consider implementing Redis for production deployments with many users
