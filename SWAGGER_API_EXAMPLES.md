# 📝 Swagger Admin Access - API Examples

## Complete Workflow Example

### 1. Login Request
```http
POST /api/auth/login HTTP/1.1
Host: localhost:5000
Content-Type: application/json

{
  "email": "admin@gahar.sa",
  "password": "AdminPassword123!"
}
```

### 1a. Login Response (Success)
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI3MGYzYTc2Yy00YjM3LTQxYzctOWM5Yi0zYzFiMWYxZTUyNDEiLCJuYW1lIjoiQWRtaW4gVXNlciIsImVtYWlsIjoiYWRtaW5AZ2FoYXIuc2EiLCJyb2xlIjoiQWRtaW4iLCJpYXQiOjE3MDEwMDAwMDAsImV4cCI6MTcwMTAwOTAwMH0.abcdef123456...",
  "refreshToken": "VmFsaWRSZWZyZXNoVG9rZW5IZXJl...",
  "user": {
    "id": "70f3a76c-4b37-41c7-9c9b-3c1b1f1e5241",
    "email": "admin@gahar.sa",
    "username": "admin",
    "firstName": "Admin",
    "lastName": "User",
    "role": "Admin"
  }
}
```

### 2. Access Swagger with Token
```http
GET /swagger/index.html HTTP/1.1
Host: localhost:5000
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI3MGYzYTc2Yy00YjM3LTQxYzctOWM5Yi0zYzFiMWYxZTUyNDEiLCJuYW1lIjoiQWRtaW4gVXNlciIsImVtYWlsIjoiYWRtaW5AZ2FoYXIuc2EiLCJyb2xlIjoiQWRtaW4iLCJpYXQiOjE3MDEwMDAwMDAsImV4cCI6MTcwMTAwOTAwMH0.abcdef123456...
```

### 2a. Success Response
```
HTTP/1.1 200 OK
Content-Type: text/html; charset=utf-8

<!DOCTYPE html>
<!-- Swagger UI HTML loads here -->
```

---

## Error Scenarios

### Scenario A: Missing Token (Production)
```http
GET /swagger/index.html HTTP/1.1
Host: api.gahar.sa
```

```json
{
  "statusCode": 401,
  "message": "Unauthorized access. Please provide a valid admin token.",
  "timestamp": "2024-01-15T10:30:45Z"
}
```

---

### Scenario B: Invalid Token
```http
GET /swagger/index.html HTTP/1.1
Host: api.gahar.sa
Authorization: Bearer invalid.token.here
```

```json
{
  "statusCode": 401,
  "message": "Unauthorized access. Please provide a valid admin token.",
  "timestamp": "2024-01-15T10:30:45Z"
}
```

---

### Scenario C: Valid User Token (Not Admin)
```http
GET /swagger/index.html HTTP/1.1
Host: api.gahar.sa
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyLWlkIiwibmFtZSI6IlJlZ3VsYXIgVXNlciIsImVtYWlsIjoidXNlckBnYWhhci5zYSIsInJvbGUiOiJVc2VyIn0.token...
```

```json
{
  "statusCode": 403,
  "message": "Forbidden. Admin access required.",
  "timestamp": "2024-01-15T10:30:45Z"
}
```

---

### Scenario D: Expired Token
```http
GET /swagger/index.html HTTP/1.1
Host: api.gahar.sa
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyLWlkIiwibmFtZSI6IkFkbWluIEZzZXIiLCJlbWFpbCI6ImFkbWluQGdhaGFyLnNhIiwicm9sZSI6IkFkbWluIiwiZXhwIjoxNzAwOTkwMDAwfQ.expired...
```

```json
{
  "statusCode": 401,
  "message": "Unauthorized access. Please provide a valid admin token.",
  "timestamp": "2024-01-15T10:30:45Z"
}
```

---

## curl Examples

### Get Admin Token
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@gahar.sa",
    "password": "YourPassword123"
  }' \
  | jq '.accessToken' \
  | tr -d '"' > admin-token.txt
```

### Access Swagger with Saved Token
```bash
TOKEN=$(cat admin-token.txt)

curl -I -H "Authorization: Bearer $TOKEN" \
  "http://localhost:5000/swagger/index.html"
```

### Get OpenAPI Specification
```bash
TOKEN=$(cat admin-token.txt)

curl -H "Authorization: Bearer $TOKEN" \
  "http://localhost:5000/swagger/v1/swagger.json" \
  | jq . > openapi-spec.json
```

### Test Unauthorized Access
```bash
# Should get 401
curl -i "http://localhost:5000/swagger/index.html"
```

---

## Postman Collection

### Create New Request
1. **Method**: `POST`
2. **URL**: `http://localhost:5000/api/auth/login`
3. **Headers**: 
   ```
   Content-Type: application/json
   ```
4. **Body** (raw JSON):
 ```json
   {
     "email": "admin@gahar.sa",
     "password": "YourPassword123"
   }
   ```
5. **Send** and copy the `accessToken`

### Create Swagger Request
1. **Method**: `GET`
2. **URL**: `http://localhost:5000/swagger/index.html`
3. **Authorization Tab**:
   - **Type**: Bearer Token
   - **Token**: (paste your accessToken here)
4. **Send**

---

## JavaScript/Node.js Example

### Get Admin Token and Access Swagger
```javascript
const axios = require('axios');

const apiBase = 'http://localhost:5000';

// Step 1: Login
async function loginAsAdmin() {
  try {
    const response = await axios.post(`${apiBase}/api/auth/login`, {
 email: 'admin@gahar.sa',
      password: 'YourPassword123'
    });
    
    return response.data.accessToken;
  } catch (error) {
    console.error('Login failed:', error.response?.data);
  }
}

// Step 2: Access Swagger with Token
async function accessSwagger(token) {
  try {
    const response = await axios.get(`${apiBase}/swagger/index.html`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
    
    console.log('Swagger access successful');
    return response.data;
  } catch (error) {
    console.error('Swagger access failed:', {
      status: error.response?.status,
      message: error.response?.data?.message
    });
  }
}

// Execute
(async () => {
  const token = await loginAsAdmin();
  if (token) {
    await accessSwagger(token);
  }
})();
```

---

## PowerShell Example

```powershell
# Step 1: Login and get token
$loginBody = @{
  email = "admin@gahar.sa"
    password = "YourPassword123"
} | ConvertTo-Json

$loginResponse = Invoke-RestMethod -Uri "http://localhost:5000/api/auth/login" `
    -Method Post `
    -Headers @{"Content-Type" = "application/json"} `
    -Body $loginBody

$token = $loginResponse.accessToken
Write-Host "Token obtained: $token"

# Step 2: Access Swagger
$headers = @{
    "Authorization" = "Bearer $token"
}

try {
    $swaggerResponse = Invoke-RestMethod -Uri "http://localhost:5000/swagger/index.html" `
 -Method Get `
        -Headers $headers
    Write-Host "Swagger access successful!"
} catch {
    Write-Host "Error accessing Swagger: $($_.Exception.Message)"
    Write-Host "Status: $($_.Exception.Response.StatusCode)"
}
```

---

## Response Headers (Admin Access)

```
HTTP/1.1 200 OK
Content-Type: text/html; charset=utf-8
Content-Length: 3456
Content-Encoding: gzip
Cache-Control: public, max-age=0
Server: Kestrel
X-Content-Type-Options: nosniff
X-Frame-Options: SAMEORIGIN
Date: Mon, 15 Jan 2024 10:30:45 GMT
```

---

## Testing Middleware Behavior

### Log Messages You'll See

✅ **Successful Access**:
```
2024-01-15 10:30:45.123 +0000 [INF] Admin user 70f3a76c-4b37-41c7-9c9b-3c1b1f1e5241 accessing Swagger
```

⚠️ **Unauthorized Access**:
```
2024-01-15 10:30:46.456 +0000 [WRN] Unauthorized access attempt to Swagger at /swagger/index.html from 192.168.1.100
```

⛔ **Non-Admin Access**:
```
2024-01-15 10:30:47.789 +0000 [WRN] Non-admin user user-id-here attempted to access Swagger
```

---

## Development Mode Exception

In development, the middleware allows full Swagger access:

```
2024-01-15 10:30:45.123 +0000 [INF] Swagger access in Development mode from 127.0.0.1
```

No authentication required during development!

---

## Token Validation Details

### JWT Claims Verified by Middleware
```
{
  "sub": "70f3a76c-4b37-41c7-9c9b-3c1b1f1e5241",  ✓ User ID
  "name": "Admin User",       ✓ Name
  "email": "admin@gahar.sa",            ✓ Email
  "role": "Admin",       ✓ REQUIRED for Swagger
  "permissions": ["read", "write", "delete"],       ℹ️ Optional
  "iat": 1701000000,         ✓ Issued at
  "exp": 1701009000           ✓ Expiration (must be future)
}
```

---

## Troubleshooting Guide

| Issue | Status Code | Solution |
|-------|-------------|----------|
| No token provided | 401 | Login first and copy token |
| Invalid/malformed token | 401 | Re-login for a fresh token |
| Expired token | 401 | Token expires after 15 minutes, login again |
| Non-admin user | 403 | Use an admin account |
| Wrong bearer format | 401 | Use "Bearer TOKEN" (with space) |
| CORS issue | 403 | Check CORS settings in Program.cs |

---

## Next Steps

1. ✅ Login with admin credentials
2. ✅ Copy the accessToken
3. ✅ Use token to access Swagger
4. ✅ Test API endpoints directly from Swagger UI
5. ✅ Review logs for access audit trail
