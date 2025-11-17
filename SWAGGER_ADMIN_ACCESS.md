# Swagger Admin Access Configuration Guide

## Overview
Your API now has a secure admin authentication system for Swagger documentation. This ensures that only authorized administrators can access the API documentation.

## How It Works

### Development Environment
- **Swagger Access**: Fully accessible without authentication
- **Purpose**: Easier development and testing

### Production Environment
- **Swagger Access**: Protected with JWT authentication
- **Requirement**: Only authenticated users with **Admin role** can access Swagger
- **Endpoints Protected**: `/swagger/*` and `/api-docs/*`

## How to Access Admin Swagger

### Step 1: Login to Get JWT Token
Send a POST request to the login endpoint:

```bash
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@example.com",
  "password": "your-admin-password"
}
```

**Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "...",
  "user": {
    "id": "user-id",
    "email": "admin@example.com",
    "role": "Admin"
  }
}
```

### Step 2: Access Swagger UI
1. Navigate to: `https://your-api.com/swagger/index.html`
2. Click the **"Authorize"** button (🔒 lock icon) in the top right
3. In the Authorization dialog, enter:
   ```
   Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
   ```
4. Click **"Authorize"**
5. You now have full access to all API endpoints documentation

## Configuration Details

### New Middleware
Two new files have been created to handle Swagger authentication:

1. **`Middleware/SwaggerAuthenticationMiddleware.cs`**
   - Intercepts requests to Swagger endpoints
   - Validates user authentication in production
   - Checks for "Admin" role claim
   - Logs unauthorized access attempts

2. **`Middleware/SwaggerAuthenticationMiddlewareExtensions.cs`**
   - Extension method for easy middleware registration
   - Called in `Program.cs` via `app.UseSwaggerAuthentication()`

### How Authentication Works

The middleware:
- ✅ Allows all Swagger access in Development mode
- ✅ Requires valid JWT token in Production mode
- ✅ Checks for "Admin" role in user claims
- ✅ Returns 401 (Unauthorized) if token is missing or invalid
- ✅ Returns 403 (Forbidden) if user lacks admin role
- ✅ Logs all access attempts for security audit

## Security Features

### Role-Based Access Control
```csharp
// The middleware checks for:
// 1. Valid JWT token (from Bearer scheme)
// 2. "Admin" role claim in the token
// 3. Access logs for audit trail
```

### Logging
All Swagger access attempts are logged:
- ✅ Successful admin access
- ✅ Unauthorized access attempts
- ✅ Non-admin access attempts

### Claim Checking
The middleware validates:
```csharp
// Check for admin role in two ways:
var hasAdminRole = context.User.Claims.Any(c => 
    c.Type == "role" && c.Value == "Admin") ||
    context.User.IsInRole("Admin");
```

## Testing Access Control

### Scenario 1: Without Token (Production)
```bash
curl https://your-api.com/swagger/index.html
# Response: 401 Unauthorized - "Unauthorized access. Please provide a valid admin token."
```

### Scenario 2: With Invalid Token
```bash
curl -H "Authorization: Bearer invalid-token" https://your-api.com/swagger/index.html
# Response: 401 Unauthorized
```

### Scenario 3: With Valid Token But No Admin Role
```bash
curl -H "Authorization: Bearer valid-user-token" https://your-api.com/swagger/index.html
# Response: 403 Forbidden - "Forbidden. Admin access required."
```

### Scenario 4: With Valid Admin Token
```bash
curl -H "Authorization: Bearer valid-admin-token" https://your-api.com/swagger/index.html
# Response: 200 OK - Swagger UI loads successfully
```

## Ensuring Admin Role in JWT Token

Make sure your `JwtService` includes the admin role in the token claims:

```csharp
public class JwtService : IJwtService
{
    public string GenerateToken(User user)
    {
      var claims = new List<Claim>
     {
            new Claim("sub", user.Id),
   new Claim("email", user.Email),
            new Claim("role", user.Role?.Name ?? "User"), // Include role
       // ... other claims
        };
        
        // Create token with claims...
    }
}
```

## Disabling Swagger Authentication

If you need to disable authentication temporarily:

### Option 1: Comment out middleware
```csharp
// app.UseSwaggerAuthentication(); // Commented out
```

### Option 2: Environment check
```csharp
if (!app.Environment.IsProduction())
{
    app.UseSwaggerAuthentication();
}
```

### Option 3: Configuration-based
```csharp
if (builder.Configuration.GetValue<bool>("EnableSwaggerAuth"))
{
    app.UseSwaggerAuthentication();
}
```

## Swagger UI Features

Your Swagger UI now supports:
- ✅ JWT Bearer token authorization
- ✅ Token input field in the Authorize dialog
- ✅ Persistent token storage during session
- ✅ Automatic token injection in all requests
- ✅ Role-based endpoint visibility (if using Authorize attributes)

## Best Practices

1. **Keep Tokens Secure**
   - Never share your admin token
   - Use HTTPS only
   - Set appropriate token expiration times

2. **Monitor Access Logs**
   - Review logs for unauthorized access attempts
   - Check `ILogger<SwaggerAuthenticationMiddleware>` output

3. **Use Strong Passwords**
   - Admin accounts should have complex passwords
   - Consider implementing password policies

4. **Rotate Tokens Regularly**
   - Implement token refresh mechanisms
   - Use short-lived access tokens

5. **Limit Admin Accounts**
   - Restrict number of admin users
   - Audit admin activity regularly

## Troubleshooting

### Issue: "Unauthorized access" message
**Solution**: 
1. Login again to get a fresh token
2. Verify the token is properly formatted as `Bearer <token>`
3. Check token expiration time

### Issue: "Forbidden. Admin access required" message
**Solution**:
1. Verify your user has admin role in the database
2. Check that the role claim is correctly added to JWT
3. Login with an actual admin account

### Issue: Swagger not loading in production
**Solution**:
1. Check that you're using the correct URL
2. Verify authentication middleware is enabled
3. Check logs for specific error details

## Environment Variables

No new environment variables required. The middleware uses existing configuration:
- `JwtSettings:SecretKey` - For token validation
- `JwtSettings:Issuer` - Token issuer validation
- `JwtSettings:Audience` - Token audience validation

## API Endpoints Summary

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/auth/login` | None | Login to get JWT token |
| POST | `/api/auth/register` | None | Register new user |
| GET | `/swagger/index.html` | Admin JWT | Access Swagger documentation |
| GET | `/swagger/v1/swagger.json` | Admin JWT | Get OpenAPI specification |

## Questions?

For more details about JWT configuration, see:
- `Services/Implementations/JwtService.cs`
- `Controllers/AuthController.cs`
- `Program.cs` - JWT Bearer authentication setup
