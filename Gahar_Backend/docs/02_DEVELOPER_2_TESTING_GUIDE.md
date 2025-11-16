# 🧪 دليل اختبارات التكامل - Integration Testing Guide
## Developer 2 Features - Complete Testing Strategy

**تاريخ الإنشاء:** 11 يناير 2025  
**الهدف:** ضمان جودة الكود وخلوه من الأخطاء

---

## 📋 جدول المحتويات
1. [نظرة عامة](#نظرة-عامة)
2. [Setup & Prerequisites](#setup--prerequisites)
3. [Feature 1: Page Builder Tests](#feature-1-page-builder-tests)
4. [Feature 2: Form Builder Tests](#feature-2-form-builder-tests)
5. [Feature 3: Navigation Menu Tests](#feature-3-navigation-menu-tests)
6. [Feature 4: Facilities Tests](#feature-4-facilities-tests)
5. [Feature 5: Certificates Tests](#feature-5-certificates-tests)
6. [Feature 6: SEO & Analytics Tests](#feature-6-seo--analytics-tests)
7. [Integration Tests](#integration-tests)
8. [Performance Tests](#performance-tests)

---

## نظرة عامة

### 🎯 استراتيجية الاختبار

**3 مستويات من الاختبار:**

1. **Unit Tests** (اختبارات الوحدة)
   - اختبار Services بشكل منفصل
   - استخدام Mock للـ Repositories

2. **Integration Tests** (اختبارات التكامل)
   - اختبار API Endpoints
   - استخدام Database حقيقية (In-Memory أو Test DB)

3. **End-to-End Tests** (اختبارات شاملة)
   - اختبار سيناريوهات كاملة
   - مثال: إنشاء صفحة → إضافة blocks → نشر → الحذف

### ✅ Checklist عام للاختبارات

لكل Feature:
- [ ] Unit Tests للـ Services
- [ ] Integration Tests للـ Controllers
- [ ] Happy Path Tests (السيناريو الناجح)
- [ ] Error Handling Tests (سيناريوهات الفشل)
- [ ] Edge Cases Tests (حالات خاصة)
- [ ] Performance Tests (اختيارية)

---

## Setup & Prerequisites

### Required Tools

```bash
# Swagger UI (Built-in)
# متاح على: https://localhost:7xxx/swagger

# Postman (Optional)
# تنزيل من: https://www.postman.com/

# xUnit (للـ Unit Tests)
dotnet add package xunit
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package Moq
```

### Test Database Setup

**Option 1: In-Memory Database**
```csharp
// في Testing Project
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));
```

**Option 2: Separate Test Database**
```json
// appsettings.Testing.json
{
  "ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=Gahar_Test;..."
  }
}
```

### Authentication Setup

**للاختبار مع Authentication:**

1. **الحصول على JWT Token:**
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@gahar.sa",
  "password": "Admin@123"
}
```

2. **استخدام Token في الـ Requests:**
```http
Authorization: Bearer {token}
```

---

## Feature 1: Page Builder Tests

### Test Suite Overview

| Test Category | عدد الاختبارات | الأولوية |
|---------------|----------------|----------|
| CRUD Operations | 8 | عالية ⭐⭐⭐ |
| Block Management | 6 | عالية ⭐⭐⭐ |
| Publishing | 4 | متوسطة ⭐⭐ |
| Validation | 5 | عالية ⭐⭐⭐ |
| Multi-language | 3 | متوسطة ⭐⭐ |
| **Total** | **26** | - |

---

### 1.1 CRUD Operations Tests

#### Test 1.1.1: Create Page - Success ✅

**Endpoint:** `POST /api/pages`

**Request:**
```json
{
  "title": "Test Page",
  "slug": "test-page",
  "description": "This is a test page",
  "template": "Default",
  "showTitle": true,
  "showBreadcrumbs": true
}
```

**Expected Response:** `201 Created`
```json
{
  "id": 1,
  "title": "Test Page",
  "slug": "test-page",
  "isPublished": false,
  "authorName": "Admin User",
  "blockCount": 0,
  "createdAt": "2025-01-11T..."
}
```

**Assertions:**
- ✅ Status Code = 201
- ✅ Response contains `id`
- ✅ `isPublished` = false
- ✅ `slug` = "test-page"
- ✅ Database contains new record

---

#### Test 1.1.2: Create Page - Duplicate Slug ❌

**Endpoint:** `POST /api/pages`

**Request:**
```json
{
  "title": "Another Page",
  "slug": "test-page",  // Same slug as before
  "description": "Test"
}
```

**Expected Response:** `400 Bad Request`
```json
{
  "message": "Slug 'test-page' already exists"
}
```

**Assertions:**
- ✅ Status Code = 400
- ✅ Error message mentions slug
- ✅ Database count unchanged

---

#### Test 1.1.3: Create Page - Unauthorized ❌

**Endpoint:** `POST /api/pages`
**Headers:** No Authorization

**Expected Response:** `401 Unauthorized`

**Assertions:**
- ✅ Status Code = 401
- ✅ Database unchanged

---

#### Test 1.1.4: Get All Pages - With Filters ✅

**Endpoint:** `GET /api/pages?IsPublished=false&PageSize=5`

**Expected Response:** `200 OK`
```json
{
  "items": [...],
  "totalCount": 10,
  "pageNumber": 1,
  "pageSize": 5,
  "totalPages": 2,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

**Assertions:**
- ✅ Status Code = 200
- ✅ `items` array length ≤ 5
- ✅ All items have `isPublished` = false
- ✅ Pagination data correct

---

#### Test 1.1.5: Get Page by ID - Success ✅

**Endpoint:** `GET /api/pages/1`

**Expected Response:** `200 OK`
```json
{
  "id": 1,
  "title": "Test Page",
  "blocks": [],
  ...
}
```

**Assertions:**
- ✅ Status Code = 200
- ✅ Response contains all page data
- ✅ `blocks` array exists

---

#### Test 1.1.6: Get Page by ID - Not Found ❌

**Endpoint:** `GET /api/pages/99999`

**Expected Response:** `404 Not Found`

**Assertions:**
- ✅ Status Code = 404

---

#### Test 1.1.7: Get Page by Slug - Success ✅

**Endpoint:** `GET /api/pages/slug/test-page`

**Prerequisites:**
- Page must be published first

**Expected Response:** `200 OK`

**Assertions:**
- ✅ Status Code = 200
- ✅ Correct page returned

---

#### Test 1.1.8: Get Page by Slug - Unpublished ❌

**Endpoint:** `GET /api/pages/slug/test-page`

**Prerequisites:**
- Page exists but not published

**Expected Response:** `400 Bad Request`

**Assertions:**
- ✅ Status Code = 400
- ✅ Error message: "Page is not published"

---

### 1.2 Block Management Tests

#### Test 1.2.1: Add Block - Hero Section ✅

**Endpoint:** `POST /api/pages/1/blocks`

**Request:**
```json
{
  "blockType": "HeroSection",
  "configuration": {
    "backgroundImage": "/uploads/hero.jpg",
    "heading": "Welcome",
    "subheading": "Test Subheading",
    "ctaText": "Learn More",
    "ctaLink": "/about"
  },
  "isVisible": true
}
```

**Expected Response:** `200 OK`
```json
{
  "id": 1,
  "blockType": "HeroSection",
  "configuration": {...},
  "displayOrder": 1,
  "isVisible": true
}
```

**Assertions:**
- ✅ Status Code = 200
- ✅ Block created in database
- ✅ `displayOrder` = 1 (first block)
- ✅ Configuration JSON valid

---

#### Test 1.2.2: Add Block - Invalid Type ❌

**Endpoint:** `POST /api/pages/1/blocks`

**Request:**
```json
{
  "blockType": "InvalidType",
  "configuration": {}
}
```

**Expected Response:** `400 Bad Request`

**Assertions:**
- ✅ Status Code = 400
- ✅ Error message mentions invalid block type

---

#### Test 1.2.3: Add Multiple Blocks - Check Order ✅

**Steps:**
1. Add TextBlock
2. Add ImageGallery
3. Add StatsCounter

**Expected:**
- TextBlock → displayOrder = 1
- ImageGallery → displayOrder = 2
- StatsCounter → displayOrder = 3

**Assertions:**
- ✅ All blocks created
- ✅ Order correct

---

#### Test 1.2.4: Update Block ✅

**Endpoint:** `PUT /api/pages/1/blocks/1`

**Request:**
```json
{
  "blockType": "HeroSection",
  "configuration": {
    "heading": "Updated Heading"
  },
  "displayOrder": 1,
  "isVisible": false
}
```

**Expected Response:** `200 OK`

**Assertions:**
- ✅ Block updated
- ✅ `isVisible` = false

---

#### Test 1.2.5: Reorder Blocks ✅

**Endpoint:** `POST /api/pages/1/reorder-blocks`

**Request:**
```json
{
  "blockIds": [3, 1, 2]  // Reverse order
}
```

**Expected Response:** `204 No Content`

**Verification:**
```http
GET /api/pages/1
```

**Assertions:**
- ✅ Blocks reordered correctly
- ✅ displayOrder updated

---

#### Test 1.2.6: Delete Block ✅

**Endpoint:** `DELETE /api/pages/1/blocks/1`

**Expected Response:** `204 No Content`

**Assertions:**
- ✅ Block deleted from database
- ✅ Other blocks remain

---

### 1.3 Publishing Tests

#### Test 1.3.1: Publish Page ✅

**Endpoint:** `POST /api/pages/1/publish`

**Expected Response:** `204 No Content`

**Verification:**
```http
GET /api/pages/1
```

**Assertions:**
- ✅ `isPublished` = true
- ✅ `publishedAt` is set

---

#### Test 1.3.2: Unpublish Page ✅

**Endpoint:** `POST /api/pages/1/unpublish`

**Expected Response:** `204 No Content`

**Assertions:**
- ✅ `isPublished` = false

---

#### Test 1.3.3: Published Page - Public Access ✅

**Endpoint:** `GET /api/pages/slug/test-page`
**Headers:** No Authorization

**Expected Response:** `200 OK`

**Assertions:**
- ✅ Public can access published pages

---

#### Test 1.3.4: Unpublished Page - Public Access ❌

**Endpoint:** `GET /api/pages/slug/test-page`
**Prerequisites:** Page unpublished

**Expected Response:** `400 Bad Request`

**Assertions:**
- ✅ Public cannot access unpublished pages

---

### 1.4 Validation Tests

#### Test 1.4.1: Required Fields Missing ❌

**Request:**
```json
{
  "description": "No title or slug"
}
```

**Expected:** `400 Bad Request`

---

#### Test 1.4.2: Slug Format Validation ❌

**Request:**
```json
{
  "title": "Test",
  "slug": "Invalid Slug With Spaces!"
}
```

**Expected:** Depends on implementation
- Accept and auto-convert, or
- Reject with validation error

---

#### Test 1.4.3: Max Length Validation ❌

**Request:**
```json
{
  "title": "Very long title exceeding 200 characters...",
  "slug": "test"
}
```

**Expected:** `400 Bad Request`

---

### 1.5 Multi-language Tests

#### Test 1.5.1: Create with Translations ✅

**Request:**
```json
{
  "title": "English Title",
  "slug": "english-slug",
  "translations": {
    "ar": {
      "title": "عنوان عربي",
      "slug": "arabic-slug",
      "description": "وصف عربي"
    },
    "en": {
      "title": "English Title",
      "slug": "english-slug"
    }
  }
}
```

**Assertions:**
- ✅ Translations saved
- ✅ Multiple language versions available

---

#### Test 1.5.2: Get Page with Language ✅

**Request:**
```http
GET /api/pages/1?lang=ar
```

**Expected:**
- Arabic title displayed if available
- Fallback to default if not

---

### 1.6 Edge Cases

#### Test 1.6.1: Duplicate Page ✅

**Endpoint:** `POST /api/pages/1/duplicate`

**Assertions:**
- ✅ New page created
- ✅ Slug unique (e.g., "test-page-copy")
- ✅ All blocks duplicated
- ✅ New page unpublished

---

#### Test 1.6.2: Delete Page with Blocks ✅

**Endpoint:** `DELETE /api/pages/1`

**Prerequisites:** Page has multiple blocks

**Assertions:**
- ✅ Page deleted
- ✅ All blocks deleted (Cascade)

---

#### Test 1.6.3: Update Slug - Check Published URL ✅

**Steps:**
1. Publish page with slug "old-slug"
2. Update slug to "new-slug"
3. Try accessing "old-slug"

**Expected:**
- Old slug should 404
- New slug should work

---

### ✅ Feature 1 Testing Checklist

- [ ] All 26 tests executed
- [ ] Happy paths pass
- [ ] Error cases handled
- [ ] Edge cases covered
- [ ] Database state correct after each test
- [ ] No memory leaks
- [ ] Performance acceptable

---

## Feature 2: Form Builder Tests

### Test Suite Overview

| Test Category | عدد الاختبارات | الأولوية |
|---------------|----------------|----------|
| Form CRUD | 6 | عالية ⭐⭐⭐ |
| Field Management | 8 | عالية ⭐⭐⭐ |
| Form Submission | 10 | عالية ⭐⭐⭐ |
| Validation | 6 | عالية ⭐⭐⭐ |
| Conditional Logic | 4 | متوسطة ⭐⭐ |
| Export | 2 | متوسطة ⭐⭐ |
| **Total** | **36** | - |

### Key Tests

#### Test 2.1: Create Form with Fields ✅
```json
{
  "name": "Contact Form",
  "slug": "contact",
  "fields": [
  {
      "label": "Full Name",
      "fieldKey": "fullName",
      "fieldType": "Text",
    "isRequired": true
    },
  {
      "label": "Email",
      "fieldKey": "email",
      "fieldType": "Email",
      "isRequired": true
    }
  ]
}
```

#### Test 2.2: Submit Form - Public ✅
```http
POST /api/forms/1/submit

{
  "formData": {
  "fullName": "John Doe",
    "email": "john@example.com"
  }
}
```

#### Test 2.3: Conditional Logic ✅
```json
{
  "fieldKey": "reason",
  "conditionalLogic": {
    "show": true,
    "when": {
      "fieldKey": "type",
      "operator": "equals",
      "value": "complaint"
    }
  }
}
```

**Test:** Submit form with type="complaint" → reason field required

#### Test 2.4: Export Submissions ✅
```http
GET /api/forms/1/export-submissions?format=csv
```

**Assertions:**
- ✅ CSV file downloaded
- ✅ Contains all submissions
- ✅ Headers correct

---

## Feature 3: Navigation Menu Tests

### Key Tests

#### Test 3.1: Create Multi-level Menu ✅
```json
{
  "name": "Main Menu",
  "location": "Header",
  "items": [
    {
      "label": "Home",
      "linkType": "InternalPage",
      "linkValue": "home",
      "icon": {
        "iconType": "Lucide",
     "iconValue": "Home"
      }
    },
    {
      "label": "About",
      "linkType": "InternalPage",
      "linkValue": "about",
      "children": [
   {
       "label": "Our Team",
     "linkType": "InternalPage",
          "linkValue": "team"
}
      ]
    }
  ]
}
```

#### Test 3.2: Get Menu by Location ✅
```http
GET /api/menus/location/Header
```

**Assertions:**
- ✅ Returns correct menu
- ✅ Items in correct order
- ✅ Children nested correctly
- ✅ URLs resolved

---

## Feature 4: Facilities Tests

### Key Tests

#### Test 4.1: Register Facility ✅
```json
{
  "name": "مستشفى الملك فيصل",
  "code": "FAC-001",
  "facilityType": "Hospital",
  "city": "الرياض",
  "latitude": 24.7136,
  "longitude": 46.6753
}
```

#### Test 4.2: Search Facilities ✅
```http
GET /api/facilities?City=الرياض&FacilityType=Hospital
```

#### Test 4.3: Get Facilities for Map ✅
```http
GET /api/facilities/map?Region=الرياض
```

**Response:**
```json
{
  "facilities": [
    {
      "id": 1,
      "name": "...",
      "latitude": 24.7136,
      "longitude": 46.6753,
      "status": "Active"
    }
  ]
}
```

---

## Feature 5: Certificates Tests

### Key Tests

#### Test 5.1: Issue Certificate ✅
```json
{
  "facilityId": 1,
  "certificateNumber": "CERT-2025-001",
  "certificateType": "Accreditation",
  "issuedAt": "2025-01-01",
  "expiresAt": "2026-01-01"
}
```

#### Test 5.2: Verify Certificate - Public ✅
```http
GET /api/certificates/verify/CERT-2025-001
```

**Response:**
```json
{
  "isValid": true,
  "certificateNumber": "CERT-2025-001",
  "facilityName": "مستشفى الملك فيصل",
  "status": "Valid",
  "daysUntilExpiry": 365
}
```

#### Test 5.3: Auto-expire Check ✅
- Set system date to future
- Verify status changes to "Expired"

#### Test 5.4: Revoke Certificate ✅
```http
POST /api/certificates/1/revoke

{
  "revokedReason": "Facility closed"
}
```

---

## Feature 6: SEO & Analytics Tests

### Key Tests

#### Test 6.1: Generate Sitemap ✅
```http
GET /api/seo/sitemap.xml
```

**Assertions:**
- ✅ Valid XML
- ✅ Contains all published pages
- ✅ Includes facilities
- ✅ Priority and changefreq correct

#### Test 6.2: Redirect Middleware ✅
```http
GET /old-url
```

**Expected:**
- Redirect to `/new-url` (301)
- Hit count incremented

#### Test 6.3: Robots.txt ✅
```http
GET /api/seo/robots.txt
```

**Expected:**
```
User-agent: *
Allow: /
Sitemap: https://example.com/sitemap.xml
```

---

## Integration Tests (Cross-Feature)

### Test I.1: Complete Page Lifecycle ✅

**Scenario:**
1. Create page
2. Add hero block
3. Add stats block
4. Publish
5. Verify in sitemap
6. Access via slug (public)
7. Unpublish
8. Verify 404 on public access
9. Delete
10. Verify removed from sitemap

---

### Test I.2: Facility → Certificate Flow ✅

**Scenario:**
1. Create facility
2. Issue certificate
3. Verify certificate (public)
4. Update facility status
5. Revoke certificate
6. Verify shows as revoked

---

### Test I.3: Form with File Upload ✅

**Scenario:**
1. Create form with file field
2. Submit with file
3. Verify file saved
4. Export submissions
5. Check file reference in export

---

## Performance Tests

### Test P.1: Large Dataset ⚡

**Setup:**
- Create 1000 pages
- Each with 10 blocks

**Tests:**
- List pages (paginated)
- Search pages
- Get single page

**Targets:**
- List: < 500ms
- Search: < 1s
- Get: < 200ms

---

### Test P.2: Concurrent Submissions ⚡

**Setup:**
- 100 concurrent form submissions

**Target:**
- All successful
- No deadlocks
- < 2s average response

---

## Testing Tools & Scripts

### Postman Collection

**إنشاء Collection:**
1. Import API from Swagger
2. Add Environment variables
3. Add Pre-request scripts for auth

**Sample Pre-request Script:**
```javascript
// Get JWT Token
pm.sendRequest({
    url: 'https://localhost:7xxx/api/auth/login',
    method: 'POST',
    header: 'Content-Type:application/json',
body: {
        mode: 'raw',
        raw: JSON.stringify({
 email: 'admin@gahar.sa',
   password: 'Admin@123'
    })
    }
}, function (err, res) {
    const token = res.json().token;
    pm.environment.set('jwt_token', token);
});
```

---

### Automated Testing Script

**PowerShell Script:**
```powershell
# test-all-features.ps1

$baseUrl = "https://localhost:7001/api"
$token = "" # Get from login

# Test Page Creation
$response = Invoke-RestMethod -Uri "$baseUrl/pages" `
    -Method Post `
    -Headers @{Authorization="Bearer $token"} `
 -Body '{"title":"Test","slug":"test"}' `
    -ContentType "application/json"

Write-Host "Page created: $($response.id)"

# Add more tests...
```

---

## ✅ Complete Testing Checklist

### Before Testing
- [ ] Database migrated
- [ ] Seed data loaded
- [ ] Application running
- [ ] Swagger accessible

### During Testing
- [ ] Document all test results
- [ ] Take screenshots of failures
- [ ] Log errors
- [ ] Track performance metrics

### After Testing
- [ ] All critical tests pass
- [ ] Known issues documented
- [ ] Performance acceptable
- [ ] Ready for deployment

---

## 📊 Test Results Template

```markdown
## Test Results - Feature X

**Date:** 2025-01-11
**Tester:** Developer Name
**Environment:** Development

### Summary
- Total Tests: 26
- Passed: 24
- Failed: 2
- Skipped: 0

### Failed Tests
1. Test 1.2.3 - Add Multiple Blocks
   - **Issue:** DisplayOrder not incrementing
   - **Fix:** Updated PageBlockRepository
   - **Status:** Fixed

2. Test 1.5.1 - Create with Translations
   - **Issue:** Translation not saving
   - **Fix:** Pending investigation
   - **Status:** Open

### Performance
- Average Response Time: 150ms
- Max Response Time: 800ms
- Database Queries: Optimized

### Recommendations
- Add index on Pages.Slug
- Cache published pages
```

---

**تم إنشاء هذا الدليل في:** 11 يناير 2025  
**آخر تحديث:** 11 يناير 2025  
**الحالة:** 🧪 دليل شامل جاهز للاستخدام
