# 🔐 Quick Start: Admin Swagger Access

## What Changed?
Your Swagger documentation is now **secured with admin authentication**. Only authenticated users with the **Admin role** can access it in production.

## 3 Steps to Access Swagger as Admin

### Step 1️⃣: Login
Send a POST request to get your admin token:

```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@example.com",
    "password": "your-password"
  }'
```

**Copy the `accessToken` from the response**

---

### Step 2️⃣: Open Swagger UI
Navigate to:
- **Development**: `http://localhost:5000/swagger/index.html`
- **Production**: `https://your-api.com/swagger/index.html`

---

### Step 3️⃣: Authorize with Your Token
1. Click the **"Authorize"** 🔒 button (top right)
2. Paste your token with the prefix "Bearer ":
   ```
   Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
 ```
3. Click **"Authorize"**
4. ✅ Done! You now have full API access

---

## Environment Behavior

| Environment | Swagger Access | Token Required |
|-------------|---|---|
| **Development** | ✅ Fully open | ❌ No |
| **Production** | 🔒 Admin only | ✅ Yes |

---

## Troubleshooting

### ❌ "401 Unauthorized"
- **Cause**: Token missing or invalid
- **Fix**: Login again and copy the fresh token

### ❌ "403 Forbidden"
- **Cause**: Token is valid but user is not Admin
- **Fix**: Login with an admin account

### ❌ "Token not working"
- **Cause**: Token format incorrect or expired
- **Fix**: 
  1. Ensure "Bearer " prefix is included
  2. Get a fresh token (tokens expire after ~15 minutes)

---

## Sample Admin Account
Check your database for an admin user:
```sql
SELECT * FROM Users WHERE Role = 'Admin'
```

If none exist, create one using the registration endpoint or directly in the database.

---

## API Endpoints Summary

| Endpoint | Method | Auth | Purpose |
|----------|--------|------|---------|
| `/api/auth/login` | POST | ❌ | Get JWT token |
| `/swagger/index.html` | GET | ✅ Admin | View API docs |
| `/swagger/v1/swagger.json` | GET | ✅ Admin | Get OpenAPI spec |

---

## Using Swagger Authorize Feature

### In Browser UI:
1. Click "Authorize" button
2. Enter: `Bearer <your-token>`
3. All subsequent requests include the token

### In curl:
```bash
curl -H "Authorization: Bearer <your-token>" \
  https://your-api.com/swagger/index.html
```

---

## Implementation Details

### What's New:
✅ `SwaggerAuthenticationMiddleware.cs` - Protects Swagger with JWT  
✅ `SwaggerAuthenticationMiddlewareExtensions.cs` - Registration helper  
✅ Updated `Program.cs` - Middleware integration  

### Role Claim Format:
```csharp
// JWT includes this claim for admins:
new Claim(ClaimTypes.Role, "Admin")
```

### Logging:
All access attempts are logged:
- ✅ Successful admin access
- ⚠️ Unauthorized attempts
- ⛔ Non-admin access attempts

---

## Security Features

✓ JWT token-based authentication  
✓ Role-based access control  
✓ Development mode bypass (full access)  
✓ Request logging for audit trail  
✓ HTTPS support  

---

## Questions?

For detailed documentation, see: `SWAGGER_ADMIN_ACCESS.md`
