# Rate Limiting Testing Guide - Non-Admin Users

## Quick Start

This guide demonstrates how to test the rate limiting feature for non-admin users (1 request per 3 minutes).

## Prerequisites

1. Running Gahar Backend API (`https://localhost:5001` or `http://localhost:5000`)
2. Two test users:
   - **Admin User** (role: SuperAdmin or Admin)
   - **Non-Admin User** (regular user without admin roles)
3. Valid JWT tokens for both users
4. A REST client (Postman, Insomnia, curl, or VS Code REST Client)

## Test Setup

### Step 1: Get Authentication Tokens

#### For Admin User
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@gahar.sa",
  "password": "AdminPassword123!"
  }'
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "email": "admin@gahar.sa",
    "roles": ["SuperAdmin"]
  }
}
```

**Save the token as:** `ADMIN_TOKEN`

#### For Non-Admin User
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@gahar.sa",
    "password": "UserPassword123!"
  }'
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 2,
  "email": "user@gahar.sa",
    "roles": []
  }
}
```

**Save the token as:** `USER_TOKEN`

---

## Test Cases

### Test 1: Non-Admin User - First Request (Should Succeed)

**Objective:** Verify that non-admin users can send one request successfully.

**Request:**
```bash
curl -X POST http://localhost:5000/api/shortlinks \
  -H "Authorization: Bearer $USER_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
 "originalUrl": "https://example.com/very/long/url",
    "description": "Test short link",
    "category": "Testing",
    "isActive": true
  }'
```

**Expected Response:** `201 Created`

```json
{
  "id": 1,
  "shortCode": "abc123",
  "originalUrl": "https://example.com/very/long/url",
  "description": "Test short link",
  "category": "Testing",
  "qrCode": "data:image/png;base64,iVBORw0KGgoAAAANS...",
  "clicks": 0,
  "isActive": true,
  "createdAt": "2024-01-15T10:00:00Z"
}
```

---

### Test 2: Non-Admin User - Second Request (Should Be Rate Limited)

**Objective:** Verify that second request within 3 minutes is blocked.

**Request:** (Send immediately after Test 1)
```bash
curl -X POST http://localhost:5000/api/shortlinks \
  -H "Authorization: Bearer $USER_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "originalUrl": "https://example.com/another/url",
    "description": "Second test link",
    "category": "Testing",
    "isActive": true
  }'
```

**Expected Response:** `429 Too Many Requests`

```json
{
  "statusCode": 429,
  "message": "تم تجاوز حد الطلبات المسموح به",
  "detail": "يمكنك إرسال 1 طلب فقط خلال 180 ثانية",
  "retryAfter": 180,
  "resetTime": "2024-01-15T10:03:00Z"
}
```

**Response Headers:**
```
HTTP/1.1 429 Too Many Requests
Retry-After: 180
Content-Type: application/json
```

---

### Test 3: Non-Admin User - Wait and Retry (Should Succeed)

**Objective:** Verify that request is allowed after 3-minute window expires.

**Steps:**
1. Note the `resetTime` from Test 2 response
2. Wait 180 seconds (3 minutes)
3. Send the same request as Test 2

**Request:** (After waiting 180 seconds)
```bash
curl -X POST http://localhost:5000/api/shortlinks \
  -H "Authorization: Bearer $USER_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "originalUrl": "https://example.com/another/url",
    "description": "Second test link after wait",
    "category": "Testing",
  "isActive": true
  }'
```

**Expected Response:** `201 Created`

```json
{
  "id": 2,
  "shortCode": "def456",
  "originalUrl": "https://example.com/another/url",
  "description": "Second test link after wait",
  "category": "Testing",
  "qrCode": "data:image/png;base64,iVBORw0KGgoAAAANS...",
  "clicks": 0,
  "isActive": true,
  "createdAt": "2024-01-15T10:03:10Z"
}
```

---

### Test 4: Admin User - Multiple Requests (Should All Succeed)

**Objective:** Verify that admin users are not rate limited.

**Request 1:**
```bash
curl -X POST http://localhost:5000/api/shortlinks \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "originalUrl": "https://admin.example.com/link1",
    "description": "Admin test link 1",
    "category": "Admin",
    "isActive": true
  }'
```

**Expected Response:** `201 Created`

**Request 2:** (Send immediately)
```bash
curl -X POST http://localhost:5000/api/shortlinks \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "originalUrl": "https://admin.example.com/link2",
    "description": "Admin test link 2",
    "category": "Admin",
    "isActive": true
  }'
```

**Expected Response:** `201 Created`

**Request 3:** (Send immediately)
```bash
curl -X POST http://localhost:5000/api/shortlinks \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "originalUrl": "https://admin.example.com/link3",
    "description": "Admin test link 3",
    "category": "Admin",
    "isActive": true
  }'
```

**Expected Response:** `201 Created`

---

### Test 5: Update Operation Rate Limiting

**Objective:** Verify that UPDATE operations are also rate limited.

**Request 1:** (First update - should succeed)
```bash
curl -X PUT http://localhost:5000/api/shortlinks/1 \
  -H "Authorization: Bearer $USER_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "originalUrl": "https://updated.example.com/url",
    "description": "Updated description",
    "category": "Updated",
"isActive": true
  }'
```

**Expected Response:** `200 OK`

**Request 2:** (Second update - should be rate limited)
```bash
curl -X PUT http://localhost:5000/api/shortlinks/1 \
  -H "Authorization: Bearer $USER_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "originalUrl": "https://another-update.example.com/url",
    "description": "Another update",
    "category": "Updated",
    "isActive": true
  }'
```

**Expected Response:** `429 Too Many Requests`

---

### Test 6: Regenerate QR Code Rate Limiting

**Objective:** Verify that regenerate QR code endpoint is also rate limited.

**Request 1:** (First regenerate - should succeed)
```bash
curl -X POST "http://localhost:5000/api/shortlinks/1/regenerate-qr?logoUrl=https://example.com/logo.png" \
  -H "Authorization: Bearer $USER_TOKEN"
```

**Expected Response:** `200 OK`

```json
{
  "qrCode": "data:image/png;base64,iVBORw0KGgoAAAANS...",
  "updatedAt": "2024-01-15T10:05:00Z"
}
```

**Request 2:** (Second regenerate - should be rate limited)
```bash
curl -X POST "http://localhost:5000/api/shortlinks/1/regenerate-qr" \
  -H "Authorization: Bearer $USER_TOKEN"
```

**Expected Response:** `429 Too Many Requests`

---

## Postman Collection

Import this collection to test more easily:

```json
{
  "info": {
    "name": "Rate Limiting Tests",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "item": [
    {
      "name": "Login - Non-Admin",
      "request": {
        "method": "POST",
        "url": "http://localhost:5000/api/auth/login",
      "body": {
          "mode": "raw",
          "raw": "{\"email\": \"user@gahar.sa\", \"password\": \"UserPassword123!\"}"
  }
  }
    },
    {
      "name": "Create Short Link - Non-Admin (First)",
   "request": {
 "method": "POST",
        "header": [
          {
"key": "Authorization",
          "value": "Bearer {{USER_TOKEN}}"
  }
  ],
    "url": "http://localhost:5000/api/shortlinks",
        "body": {
   "mode": "raw",
       "raw": "{\"originalUrl\": \"https://example.com/test1\", \"description\": \"Test\", \"category\": \"Testing\"}"
        }
   }
    },
    {
      "name": "Create Short Link - Non-Admin (Second - Should Fail)",
      "request": {
        "method": "POST",
        "header": [
          {
      "key": "Authorization",
            "value": "Bearer {{USER_TOKEN}}"
          }
 ],
        "url": "http://localhost:5000/api/shortlinks",
        "body": {
      "mode": "raw",
          "raw": "{\"originalUrl\": \"https://example.com/test2\", \"description\": \"Test\", \"category\": \"Testing\"}"
      }
      }
    }
  ]
}
```

---

## VS Code REST Client Testing

Create a file named `test-rate-limiting.http`:

```http
@baseUrl = http://localhost:5000
@userToken = 
@adminToken = 

### Login as Non-Admin User
POST {{baseUrl}}/api/auth/login
Content-Type: application/json

{
  "email": "user@gahar.sa",
  "password": "UserPassword123!"
}

### Login as Admin User
POST {{baseUrl}}/api/auth/login
Content-Type: application/json

{
  "email": "admin@gahar.sa",
  "password": "AdminPassword123!"
}

### Non-Admin: Create Short Link (1st - Should Succeed)
POST {{baseUrl}}/api/shortlinks
Authorization: Bearer {{userToken}}
Content-Type: application/json

{
  "originalUrl": "https://example.com/test1",
  "description": "Test link 1",
  "category": "Testing",
  "isActive": true
}

### Non-Admin: Create Short Link (2nd - Should Be Rate Limited)
POST {{baseUrl}}/api/shortlinks
Authorization: Bearer {{userToken}}
Content-Type: application/json

{
  "originalUrl": "https://example.com/test2",
  "description": "Test link 2",
  "category": "Testing",
  "isActive": true
}

### Admin: Create Short Link (1st)
POST {{baseUrl}}/api/shortlinks
Authorization: Bearer {{adminToken}}
Content-Type: application/json

{
  "originalUrl": "https://admin.example.com/test1",
  "description": "Admin test 1",
  "category": "Admin",
  "isActive": true
}

### Admin: Create Short Link (2nd - Should Succeed)
POST {{baseUrl}}/api/shortlinks
Authorization: Bearer {{adminToken}}
Content-Type: application/json

{
  "originalUrl": "https://admin.example.com/test2",
  "description": "Admin test 2",
  "category": "Admin",
  "isActive": true
}
```

---

## Automated Testing with curl

Run this bash script to automate testing:

```bash
#!/bin/bash

# Configuration
API_URL="http://localhost:5000"
ADMIN_EMAIL="admin@gahar.sa"
ADMIN_PASS="AdminPassword123!"
USER_EMAIL="user@gahar.sa"
USER_PASS="UserPassword123!"

echo "=== Rate Limiting Test Suite ==="

# Get tokens
echo -e "\n1. Getting admin token..."
ADMIN_RESPONSE=$(curl -s -X POST "$API_URL/api/auth/login" \
  -H "Content-Type: application/json" \
  -d "{\"email\": \"$ADMIN_EMAIL\", \"password\": \"$ADMIN_PASS\"}")
ADMIN_TOKEN=$(echo $ADMIN_RESPONSE | grep -o '"token":"[^"]*' | cut -d'"' -f4)
echo "Admin token: ${ADMIN_TOKEN:0:20}..."

echo -e "\n2. Getting user token..."
USER_RESPONSE=$(curl -s -X POST "$API_URL/api/auth/login" \
  -H "Content-Type: application/json" \
  -d "{\"email\": \"$USER_EMAIL\", \"password\": \"$USER_PASS\"}")
USER_TOKEN=$(echo $USER_RESPONSE | grep -o '"token":"[^"]*' | cut -d'"' -f4)
echo "User token: ${USER_TOKEN:0:20}..."

# Test 1: Non-Admin First Request
echo -e "\n3. Testing Non-Admin First Request (should succeed)..."
RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$API_URL/api/shortlinks" \
  -H "Authorization: Bearer $USER_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"originalUrl": "https://example.com/test1", "description": "Test", "category": "Testing"}')
STATUS=$(echo "$RESPONSE" | tail -n1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "Status: $STATUS"
echo "Response: $BODY"

# Test 2: Non-Admin Second Request
echo -e "\n4. Testing Non-Admin Second Request (should fail with 429)..."
RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$API_URL/api/shortlinks" \
  -H "Authorization: Bearer $USER_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"originalUrl": "https://example.com/test2", "description": "Test", "category": "Testing"}')
STATUS=$(echo "$RESPONSE" | tail -n1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "Status: $STATUS"
if [ "$STATUS" = "429" ]; then
  echo "✓ PASS: Rate limit correctly applied"
  echo "Response: $BODY"
else
  echo "✗ FAIL: Expected 429, got $STATUS"
fi

# Test 3: Admin Multiple Requests
echo -e "\n5. Testing Admin Multiple Requests (should all succeed)..."
for i in {1..3}; do
  RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$API_URL/api/shortlinks" \
    -H "Authorization: Bearer $ADMIN_TOKEN" \
    -H "Content-Type: application/json" \
    -d "{\"originalUrl\": \"https://admin.example.com/test$i\", \"description\": \"Admin test\", \"category\": \"Admin\"}")
  STATUS=$(echo "$RESPONSE" | tail -n1)
  echo "Request $i Status: $STATUS"
  if [ "$STATUS" = "201" ]; then
    echo "✓ PASS"
  else
    echo "✗ FAIL"
  fi
done

echo -e "\n=== Test Suite Completed ==="
```

---

## Monitoring and Logging

### Check Logs
Monitor the application logs to see rate limiting in action:

```
[13:45:30 INF] Rate limit check: user_2, allowed=true, limit=1, window=180s
[13:45:35 INF] Rate limit check: user_2, allowed=false, limit=1, window=180s
[13:45:35 WRN] Rate limit exceeded for identifier: user_2
```

### Check Metrics
Monitor rate limit metrics in your APM tool (Application Insights, Datadog, etc.):
- Request count per user
- Rate limit violations
- Response time (429 vs 200)

---

## Troubleshooting

### Issue: Getting 429 too quickly
**Solution:** Verify that:
1. The rate limit window is correctly set (180 seconds)
2. You're using the same user token
3. Check server time sync (for `resetTime`)

### Issue: Admin user also getting rate limited
**Solution:** Verify that:
1. Admin user has `SuperAdmin` or `Admin` role
2. JWT token includes role claim
3. Check role claim names in `ClaimTypes.Role`

### Issue: Rate limit not being enforced
**Solution:** Verify that:
1. `RateLimitAttribute` is applied to the action
2. `IRateLimitService` is registered in DI container
3. Request is authenticated (not `[AllowAnonymous]`)
4. Action method is not overridden in inherited class

---

## Performance Expectations

- **First request:** ~50-100ms (with authentication)
- **Rate limit check:** ~1-2ms additional
- **429 response:** ~20-30ms
- **Memory usage:** <1MB for 1000 users

---

## Next Steps

1. **Run tests** using your preferred method
2. **Verify results** match expected outcomes
3. **Adjust limits** if needed for your use case
4. **Monitor logs** for rate limit violations
5. **Document custom limits** for your API
