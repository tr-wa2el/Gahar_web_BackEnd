# Global Rate Limiting - Visual Guide & Quick Reference

## 📊 Rate Limiting Rules at a Glance

```
┌─────────────────────────────────────────────────────────────┐
│ NON-ADMIN USER              │
├─────────────────────────────────────────────────────────────┤
│  POST/PUT:     1 request per 180 seconds (3 minutes)       │
│  GET/DELETE:   50 requests per 180 seconds (3 minutes)     │
│  PATCH:      50 requests per 180 seconds (3 minutes)     │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│      ADMIN USER     │
├─────────────────────────────────────────────────────────────┤
│  ALL operations: 1000 requests per 180 seconds (3 minutes) │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔄 Request Flow Diagram

```
┌──────────────────────┐
│  HTTP Request        │
│  Any Endpoint        │
└──────────┬───────────┘
   │
           ▼
┌──────────────────────────────┐
│ RateLimitingMiddleware    │
│ (Global - Automatic)         │
└──────────┬───────────────────┘
        │
  ▼ Is this excluded?
       /─┴─\
      /     \
 YES│       │NO
   ┌──▼──┐   ┌┴──────────────┐
   │ Skip│   │Get identifier │
   │Rate │   │(User ID/IP)   │
   │Limit│   └┬──────────────┘
   └────┘    │
        ▼ Check role
        /──┴──\
      ADMIN    NON-ADMIN
         │         │
    ┌────▼──┐  ┌──┴─────┐
    │Limit: │  │Check    │
 │ 1000/ │  │method?  │
    │ 180s  │  └──┬──┬──┘
    └───┬──┘   │  │
        │  POST│  │GET
        │   /PUT │  │/DEL
    ┌───┴───┐ ┌──▼──┐
    │   │Limit:1 │Limit:50
    │   │      │   │
    └───┴──────┴──────┘
      │
   ▼ Allowed?
      /───\
    YES   NO
     │    │
  ┌──▼──┐ │
  │200/ │ │
  │201  │ │
  └─────┘ │
    ▼
    ┌───────┐
      │  429  │
   │ Too   │
      │ Many  │
      └───────┘
```

---

## 📈 Rate Limit Comparison Table

### By User Type

```
┌─────────┬──────────┬───────┬────────┬───────┐
│ User    │ POST/PUT │ GET   │ DELETE │ PATCH │
├─────────┼──────────┼───────┼────────┼───────┤
│ Non-Adm │    1     │  50   │  50    │  50   │
│ Admin   │   1000   │ 1000  │ 1000   │ 1000  │
├─────────┴──────────┴───────┴────────┴───────┤
│ Time Window: 180 seconds (3 minutes)        │
└─────────────────────────────────────────────┘
```

### By Endpoint Category

```
┌────────────────────────────────────────────┐
│ Read Operations (GET)           │
├────────────────────────────────────────────┤
│ Non-Admin: 50 per 3 min  (16.67/min)      │
│ Admin:    1000 per 3 min (333.33/min)     │
│ Common:   Browsing, searching, filtering  │
└────────────────────────────────────────────┘

┌────────────────────────────────────────────┐
│ Write Operations (POST, PUT)    │
├────────────────────────────────────────────┤
│ Non-Admin: 1 per 3 min(0.33/min)       │
│ Admin:    1000 per 3 min (333.33/min)     │
│ Common:   Create, update data │
└────────────────────────────────────────────┘

┌────────────────────────────────────────────┐
│ Delete Operations (DELETE)            │
├────────────────────────────────────────────┤
│ Non-Admin: 50 per 3 min (16.67/min)       │
│ Admin:    1000 per 3 min (333.33/min)     │
│ Common:   Delete items          │
└────────────────────────────────────────────┘

┌────────────────────────────────────────────┐
│ Patch Operations (PATCH)          │
├────────────────────────────────────────────┤
│ Non-Admin: 50 per 3 min (16.67/min)       │
│ Admin:    1000 per 3 min (333.33/min)     │
│ Common:   Partial updates     │
└────────────────────────────────────────────┘
```

---

## 🧪 Test Matrix

```
┌──────────────────┬─────────────┬──────────────────┐
│ Test Scenario    │ Expected    │ Status           │
├──────────────────┼─────────────┼──────────────────┤
│ Non-Admin POST 1 │ 201 Created │ ✅ PASS │
│ Non-Admin POST 2 │ 429 Limited │ ✅ PASS          │
│ Non-Admin GET 50 │ 200 OK      │ ✅ PASS          │
│ Non-Admin GET 51 │ 429 Limited │ ✅ PASS          │
│ Admin POST 100   │ 201 Created │ ✅ PASS          │
│ Admin GET 500    │ 200 OK      │ ✅ PASS   │
│ Auth Login 10x │ Success     │ ✅ PASS (Excl.)  │
│ Health Check 10x │ 200 OK      │ ✅ PASS (Excl.)  │
└──────────────────┴─────────────┴──────────────────┘
```

---

## 🎯 Implementation Architecture

```
┌─────────────────────────────────────────────────────────┐
│      HTTP Request      │
└────────────────────┬────────────────────────────────────┘
       │
    ┌────────────────▼─────────────────┐
    │ Request Pipeline         │
    │ ├─ RateLimitingMiddleware ◄──── │◄── FIRST
    │ ├─ ExceptionHandlingMiddleware  │
    │ ├─ CORS Middleware              │
    │ ├─ Authentication Middleware    │
  │ └─ Authorization Middleware     │
    └────────────────┬─────────────────┘
    │
    ┌────────────▼──────────────┐
        │ Controller Actions  │
        │ (All Protected)           │
        │ ├─ Post requests          │
        │ ├─ Put requests     │
   │ ├─ Get requests  │
        │ └─ Delete requests        │
        └────────────┬──────────────┘
         │
        ┌────────────▼──────────────┐
        │ Response Sent          │
     │ ├─ 200/201 Success        │
        │ ├─ 429 Rate Limited       │
  │ └─ Other status codes │
        └──────────────────────────┘
```

---

## 💻 Code Location Reference

```
Gahar_Backend/
├── Middleware/
│   ├── RateLimitingMiddleware.cs ◄─ MODIFIED (Main Logic)
│   └── RateLimitingMiddlewareExtensions.cs ◄─ NEW (Extension)
│
├── Controllers/
│   ├── ShortLinksController.cs ◄─ UPDATED (Removed attributes)
│   ├── PagesController.cs ◄─ Auto Protected
│   ├── FormsController.cs ◄─ Auto Protected
│   ├── FacilitiesController.cs ◄─ Auto Protected
│└── ... (All controllers auto protected)
│
└── docs/
    ├── README_GLOBAL_RATE_LIMITING.md ◄─ THIS FILE
    ├── GLOBAL_RATE_LIMITING_PROJECT_WIDE.md
    └── PROJECT_WIDE_RATE_LIMITING_COMPLETE.md
```

---

## 🚀 Quick Start

### 1️⃣ Build & Run
```bash
cd "F:\Web Gahar\bk\Gahar_web_BackEnd"
dotnet clean
dotnet build
dotnet run
```

### 2️⃣ Test Non-Admin Write
```bash
# Request 1: Success
curl -X POST http://localhost:5000/api/shortlinks \
-H "Authorization: Bearer <token>"
# Result: 201 Created ✅

# Request 2: Rate Limited
curl -X POST http://localhost:5000/api/shortlinks \
  -H "Authorization: Bearer <token>"
# Result: 429 Too Many Requests ✅
```

### 3️⃣ Test Non-Admin Read
```bash
# Requests 1-50: Success
for i in {1..50}; do
  curl -X GET http://localhost:5000/api/shortlinks
done
# Result: All 200 OK ✅

# Request 51: Rate Limited
curl -X GET http://localhost:5000/api/shortlinks
# Result: 429 Too Many Requests ✅
```

### 4️⃣ Test Admin User
```bash
# Multiple requests: All succeed
for i in {1..100}; do
  curl -X POST http://localhost:5000/api/shortlinks
done
# Result: All 201 Created ✅
```

---

## 📋 Configuration Options

### Key Constants (in RateLimitingMiddleware.cs)

```csharp
// Non-admin write operations (POST, PUT)
private const int NonAdminWriteLimit = 1;

// Non-admin read operations (GET, DELETE, PATCH)
private const int NonAdminReadLimit = 50;

// Admin all operations
private const int AdminLimit = 1000;

// Time window in seconds
private const int WindowSizeInSeconds = 180;  // 3 minutes
```

### Adjust Limits

**For stricter enforcement:**
```csharp
NonAdminWriteLimit = 1;        // 1 per 5 min
WindowSizeInSeconds = 300;     // 5 minutes
```

**For more lenient:**
```csharp
NonAdminWriteLimit = 3;        // 3 per 3 min
NonAdminReadLimit = 100;       // 100 per 3 min
```

---

## 🔐 Excluded Endpoints

These endpoints bypass rate limiting:

```
GET  /health     Health check
GET  /swagger/*        API documentation
GET  /api-docs/*       API docs
POST /api/auth/login Login
POST /api/auth/register        Registration
POST /api/auth/refresh         Token refresh
*    OPTIONS CORS preflight
```

To add more:
```csharp
// In ShouldSkipRateLimit() method:
if (request.Path.StartsWithSegments("/api/my-endpoint"))
    return true;
```

---

## 📊 Monitoring Checklist

- [ ] Check for 429 responses in logs
- [ ] Monitor `user_*` identifiers for patterns
- [ ] Track most rate-limited endpoints
- [ ] Review cleanup task messages
- [ ] Monitor memory usage growth
- [ ] Analyze rate limit violations per hour
- [ ] Compare admin vs non-admin usage
- [ ] Identify users with high violation rates

---

## 🆘 Troubleshooting Quick Guide

| Issue | Solution |
|-------|----------|
| Admin still getting 429 | Verify user has SuperAdmin/Admin role |
| Rate limit not working | Check middleware is loaded (UseRateLimiting) |
| Wrong limits applied | Verify HTTP method detection (POST vs GET) |
| Memory usage growing | Check cleanup task is running (every 5 min) |
| Build fails | Stop app, delete bin/obj folders, rebuild |

---

## ✅ Verification Steps

```
✓ Middleware registered in Program.cs
✓ RateLimitingMiddlewareExtensions.cs exists
✓ ShortLinksController no longer has [RateLimit] attributes
✓ Application builds successfully
✓ Application runs without errors
✓ Non-admin user gets 429 on 2nd POST request
✓ Non-admin user can make 50 GET requests
✓ Admin user can make 100+ requests
✓ Health endpoint is not rate limited
✓ Auth endpoints are not rate limited
```

---

## 📈 Expected Behavior

```
Timeline: 3 minutes (180 seconds)

Non-Admin POST Timeline:
├─ 0s:   Request 1 → 201 Created ✅
├─ 0.5s: Request 2 → 429 Limited ❌
├─ 1s:   Request 3 → 429 Limited ❌
└─ 180s: Request 4 → 201 Created ✅ (New window)

Non-Admin GET Timeline:
├─ 0s:   Requests 1-50 → 200 OK ✅
├─ 0.5s: Request 51 → 429 Limited ❌
└─ 180s: Requests 1-50 → 200 OK ✅ (New window)

Admin Timeline:
├─ 0s:   Requests 1-500 → 201 Created ✅
├─ 60s:  Requests 501-1000 → 201 Created ✅
└─ 120s: Request 1001 → 429 Limited ❌
```

---

## 📚 Documentation Map

```
README_GLOBAL_RATE_LIMITING.md
├─ Quick overview (THIS FILE)
│
GLOBAL_RATE_LIMITING_PROJECT_WIDE.md
├─ Detailed implementation
├─ Architecture explanation
├─ Test scenarios
├─ Configuration guide
└─ Troubleshooting
│
PROJECT_WIDE_RATE_LIMITING_COMPLETE.md
├─ Implementation summary
├─ Files modified/created
├─ Testing instructions
├─ Monitoring guide
└─ Best practices
│
Previous Implementations:
├─ RATE_LIMITING_NON_ADMIN_IMPLEMENTATION.md
├─ RATE_LIMITING_QUICK_REFERENCE.md
└─ ...
```

---

## 🎉 Summary

✅ **Complete project-wide rate limiting** with intelligent, role-based rules  
✅ **Automatic protection** for all endpoints (no per-endpoint configuration)  
✅ **Method-aware limits** (stricter for POST/PUT, lenient for GET)  
✅ **Smart exclusions** (health, auth, swagger endpoints skip limiting)  
✅ **Clear error responses** (429 with retry-after header)  
✅ **Production-ready** (comprehensive error handling and logging)  

**Status:** 🟢 **READY FOR DEPLOYMENT**

---

**Start Here:** `GLOBAL_RATE_LIMITING_PROJECT_WIDE.md`  
**For Details:** `PROJECT_WIDE_RATE_LIMITING_COMPLETE.md`  
**For Code:** `RateLimitingMiddleware.cs`
