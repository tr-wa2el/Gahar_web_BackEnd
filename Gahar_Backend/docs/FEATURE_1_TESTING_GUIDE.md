# 🧪 Feature 1: Page Builder System - Integration Testing Guide

**تاريخ الإنشاء:** 11 يناير 2025  
**الهدف:** اختبار شامل لجميع Endpoints

---

## 📋 Test Checklist

### ✅ Phase 1: Setup
- [ ] تطبيق يعمل بدون أخطاء
- [ ] Swagger UI متاح
- [ ] Database متصل
- [ ] Authentication يعمل

---

## 🔵 Test Group 1: Create Page (POST /api/pages)

### Test 1.1: Create Page - Success ✅

**Request:**
```
POST /api/pages
Content-Type: application/json
Authorization: Bearer {token}

{
  "title": "Welcome Page",
  "slug": "welcome",
  "description": "Main welcome page",
  "metaTitle": "Welcome to Gahar",
  "metaDescription": "Saudi Commission for Health Specialties",
  "metaKeywords": "health, specialties, saudi",
  "ogTitle": "Gahar - Welcome",
  "ogDescription": "Welcome to Saudi Commission for Health Specialties",
  "ogImage": "/images/og-image.jpg",
  "template": "Default",
  "showTitle": true,
  "showBreadcrumbs": true
}
```

**Expected Response:**
- Status: 201 Created
- Contains: Page ID, timestamps, all fields
- Location header: /api/pages/{id}

---

### Test 1.2: Create Page - Duplicate Slug ❌

**Request:**
```
POST /api/pages
{
  "title": "Welcome Page 2",
  "slug": "welcome",  // Already exists
  ...
}
```

**Expected Response:**
- Status: 400 Bad Request
- Error: "Slug 'welcome' already exists"

---

### Test 1.3: Create Page - Missing Required Fields ❌

**Request:**
```
POST /api/pages
{
  "title": "Test Page"
  // slug is missing
}
```

**Expected Response:**
- Status: 400 Bad Request
- Validation errors

---

### Test 1.4: Create Page - Without Authentication ❌

**Request:**
```
POST /api/pages
// No Authorization header

{...}
```

**Expected Response:**
- Status: 401 Unauthorized

---

### Test 1.5: Create Page - Insufficient Permissions ❌

**Request:**
```
POST /api/pages
Authorization: Bearer {token_with_no_create_permission}

{...}
```

**Expected Response:**
- Status: 403 Forbidden

---

## 🔵 Test Group 2: Get Pages (GET /api/pages)

### Test 2.1: Get All Pages - No Filter

**Request:**
```
GET /api/pages
```

**Expected Response:**
- Status: 200 OK
- Items array (may be empty)
- TotalCount, PageNumber, PageSize, TotalPages
- HasPreviousPage, HasNextPage

---

### Test 2.2: Get All Pages - With Pagination

**Request:**
```
GET /api/pages?pageNumber=2&pageSize=5
```

**Expected Response:**
- Status: 200 OK
- 5 items (or less on last page)
- PageNumber: 2

---

### Test 2.3: Get All Pages - Filter by Published Status

**Request:**
```
GET /api/pages?isPublished=true
```

**Expected Response:**
- Status: 200 OK
- Only published pages

---

### Test 2.4: Get All Pages - Search by Title

**Request:**
```
GET /api/pages?searchTerm=Welcome
```

**Expected Response:**
- Status: 200 OK
- Only pages with "Welcome" in title

---

### Test 2.5: Get All Pages - Sort by Title

**Request:**
```
GET /api/pages?sortBy=title&sortOrder=asc
```

**Expected Response:**
- Status: 200 OK
- Pages sorted by title A-Z

---

## 🔵 Test Group 3: Get Page by ID (GET /api/pages/{id})

### Test 3.1: Get Page by ID - Success

**Request:**
```
GET /api/pages/1
```

**Expected Response:**
- Status: 200 OK
- Page details including:
  - All fields
  - Author info
  - Blocks array
  - Timestamps

---

### Test 3.2: Get Page by ID - Not Found

**Request:**
```
GET /api/pages/9999
```

**Expected Response:**
- Status: 404 Not Found
- Error: "Page with ID 9999 not found"

---

## 🔵 Test Group 4: Get Page by Slug (GET /api/pages/slug/{slug})

### Test 4.1: Get Page by Slug - Published Page

**Request:**
```
GET /api/pages/slug/welcome
```

**Expected Response (if published):**
- Status: 200 OK
- Full page details

---

### Test 4.2: Get Page by Slug - Unpublished Page

**Request:**
```
GET /api/pages/slug/unpublished-page
// (where page exists but IsPublished = false)
```

**Expected Response:**
- Status: 400 Bad Request
- Error: "Page is not published"

---

### Test 4.3: Get Page by Slug - Not Found

**Request:**
```
GET /api/pages/slug/non-existent-slug
```

**Expected Response:**
- Status: 404 Not Found

---

## 🔵 Test Group 5: Update Page (PUT /api/pages/{id})

### Test 5.1: Update Page - Success

**Request:**
```
PUT /api/pages/1
Authorization: Bearer {admin_token}

{
  "title": "Updated Welcome Page",
  "slug": "welcome",
  "description": "Updated description",
  ...
  "isPublished": false
}
```

**Expected Response:**
- Status: 200 OK
- Updated page details

---

### Test 5.2: Update Page - Change Slug to Existing

**Request:**
```
PUT /api/pages/1
{
  "slug": "another-page-slug"  // Already exists
  ...
}
```

**Expected Response:**
- Status: 400 Bad Request
- Error: "Slug already exists"

---

### Test 5.3: Update Page - Page Not Found

**Request:**
```
PUT /api/pages/9999
{...}
```

**Expected Response:**
- Status: 404 Not Found

---

## 🔵 Test Group 6: Publish/Unpublish (POST)

### Test 6.1: Publish Page

**Request:**
```
POST /api/pages/1/publish
Authorization: Bearer {admin_token}
```

**Expected Response:**
- Status: 204 No Content
- Page.IsPublished = true
- Page.PublishedAt = DateTime.UtcNow

---

### Test 6.2: Unpublish Page

**Request:**
```
POST /api/pages/1/unpublish
Authorization: Bearer {admin_token}
```

**Expected Response:**
- Status: 204 No Content
- Page.IsPublished = false

---

## 🔵 Test Group 7: Add Block (POST /api/pages/{id}/blocks)

### Test 7.1: Add HeroSection Block

**Request:**
```
POST /api/pages/1/blocks
Authorization: Bearer {admin_token}

{
  "blockType": "HeroSection",
  "configuration": {
    "backgroundImage": "/uploads/hero.jpg",
    "heading": "مرحباً",
    "subheading": "Welcome to Gahar",
    "ctaText": "Learn More",
    "ctaLink": "/about",
    "textAlign": "center",
    "overlay": true,
    "overlayOpacity": 0.5
  },
  "isVisible": true,
  "cssClass": "hero-primary",
  "customId": "hero-main"
}
```

**Expected Response:**
- Status: 200 OK
- Block with ID, DisplayOrder = 1
- Configuration serialized correctly

---

### Test 7.2: Add TextBlock

**Request:**
```
POST /api/pages/1/blocks
{
  "blockType": "TextBlock",
  "configuration": {
    "content": "<h2>Content</h2><p>Text here</p>",
    "alignment": "left"
  },
  "isVisible": true
}
```

**Expected Response:**
- Status: 200 OK
- Block created

---

### Test 7.3: Add Block - Invalid Block Type

**Request:**
```
POST /api/pages/1/blocks
{
  "blockType": "InvalidType",
  ...
}
```

**Expected Response:**
- Status: 400 Bad Request
- Error: "Invalid block type"

---

### Test 7.4: Add Block - Page Not Found

**Request:**
```
POST /api/pages/9999/blocks
{...}
```

**Expected Response:**
- Status: 404 Not Found

---

## 🔵 Test Group 8: Update Block (PUT)

### Test 8.1: Update Block - Success

**Request:**
```
PUT /api/pages/1/blocks/1
Authorization: Bearer {admin_token}

{
  "blockType": "TextBlock",
  "configuration": {
    "content": "Updated content"
  },
  "displayOrder": 2,
  "isVisible": false
}
```

**Expected Response:**
- Status: 200 OK
- Updated block

---

### Test 8.2: Update Block - Not Found

**Request:**
```
PUT /api/pages/1/blocks/9999
{...}
```

**Expected Response:**
- Status: 404 Not Found

---

## 🔵 Test Group 9: Delete Block (DELETE)

### Test 9.1: Delete Block - Success

**Request:**
```
DELETE /api/pages/1/blocks/1
Authorization: Bearer {admin_token}
```

**Expected Response:**
- Status: 204 No Content

---

### Test 9.2: Delete Block - Not Found

**Request:**
```
DELETE /api/pages/1/blocks/9999
```

**Expected Response:**
- Status: 404 Not Found

---

## 🔵 Test Group 10: Reorder Blocks (POST)

### Test 10.1: Reorder Blocks

**Request:**
```
POST /api/pages/1/reorder-blocks
Authorization: Bearer {admin_token}

{
  "blockIds": [3, 1, 2]
}
```

**Expected Response:**
- Status: 204 No Content
- Block 3 → DisplayOrder 1
- Block 1 → DisplayOrder 2
- Block 2 → DisplayOrder 3

---

## 🔵 Test Group 11: Duplicate Page (POST)

### Test 11.1: Duplicate Page

**Request:**
```
POST /api/pages/1/duplicate
Authorization: Bearer {admin_token}
```

**Expected Response:**
- Status: 200 OK
- New page with:
  - Title: "{original title} (Copy)"
  - Slug: "{original-slug}-copy" (or with counter)
  - All blocks duplicated
  - IsPublished: false
  - Same AuthorId

---

## 🔵 Test Group 12: Delete Page (DELETE)

### Test 12.1: Delete Page - Success

**Request:**
```
DELETE /api/pages/1
Authorization: Bearer {admin_token}
```

**Expected Response:**
- Status: 204 No Content
- Page marked as deleted (soft delete)
- Blocks cascade deleted

---

### Test 12.2: Delete Page - Not Found

**Request:**
```
DELETE /api/pages/9999
```

**Expected Response:**
- Status: 404 Not Found

---

## 📊 Test Execution Summary

### Total Tests: 30+
- Create Tests: 5
- Read Tests: 6
- Update Tests: 3
- Delete Tests: 3
- Block Tests: 13+

---

## ✅ Success Criteria

- [ ] All 200 responses return correct data
- [ ] All 400 responses have meaningful error messages
- [ ] All 401 responses for missing auth
- [ ] All 403 responses for insufficient permissions
- [ ] All 404 responses for missing resources
- [ ] Database reflects all changes
- [ ] Timestamps are correct
- [ ] Soft deletes work properly
- [ ] Relationships maintained
- [ ] Block display order correct

---

## 🚀 Testing Tools

### Postman Collection
```json
{
  "info": {
    "name": "Pages API",
  "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "item": [
    // Import tests here
  ]
}
```

### Swagger
- URL: `https://localhost:7XXX/swagger`
- All endpoints documented
- Try it out buttons available

---

## 📝 Notes

- Replace `{id}` with actual page IDs
- Replace `{token}` with valid JWT token
- Replace `{slug}` with actual page slugs
- Use Swagger for quick testing
- Use Postman for automated testing

---

**حالة الاختبار:** ⏳ جاهز للتنفيذ  
**آخر تحديث:** 11 يناير 2025
