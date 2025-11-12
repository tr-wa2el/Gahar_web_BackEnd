# 📖 Dynamic Content System - Usage Guide

**Feature:** Dynamic Content Management System  
**Version:** 1.0.0  
**Last Updated:** 12 يناير 2025

---

## 📋 Table of Contents

1. [Introduction](#introduction)
2. [Getting Started](#getting-started)
3. [Content Management](#content-management)
4. [Tag Management](#tag-management)
5. [Advanced Features](#advanced-features)
6. [API Examples](#api-examples)
7. [Best Practices](#best-practices)
8. [Troubleshooting](#troubleshooting)

---

## Introduction

The Dynamic Content System provides a flexible and powerful solution for managing any type of content in your CMS. It supports:

- **Dynamic Fields**: Create custom fields based on Content Types
- **Publishing Workflow**: Draft → Published → Scheduled → Archived
- **Tag System**: Organize content with tags
- **SEO Support**: Built-in SEO optimization
- **Multi-language**: Full translation support
- **Search & Filter**: Advanced filtering and search capabilities

---

## Getting Started

### Prerequisites

1. ✅ Feature 1 (Content Types) must be implemented
2. ✅ Database migration applied
3. ✅ User authentication configured
4. ✅ Appropriate permissions assigned

### Quick Start

1. **Create a Content Type** (if not already created)
2. **Create Content** using that content type
3. **Add Tags** to organize content
4. **Publish** your content
5. **Access** via API endpoints

---

## Content Management

### 1. Create Content

#### Endpoint
```http
POST /api/contents
Authorization: Bearer {token}
Content-Type: application/json
```

#### Request Body
```json
{
  "contentTypeId": 1,
  "title": "مقال عن الصحة العامة",
  "slug": "health-public-article",
  "summary": "ملخص قصير عن المقال",
  "body": "<p>محتوى المقال الكامل بصيغة HTML</p>",
  "featuredImage": "/uploads/images/health-article.jpg",
  "status": "Draft",
  "isFeatured": false,
  "allowComments": true,
  "metaTitle": "مقال عن الصحة العامة - الهيئة السعودية للتخصصات الصحية",
  "metaDescription": "مقال شامل عن الصحة العامة وأهميتها",
  "metaKeywords": "صحة, صحة عامة, الهيئة السعودية",
  "ogTitle": "مقال عن الصحة العامة",
  "ogDescription": "مقال شامل عن الصحة العامة",
  "ogImage": "/uploads/images/health-article-og.jpg",
  "tagIds": [1, 2, 3],
  "customFields": {
    "author_bio": "كاتب متخصص في الصحة العامة",
    "reading_time": 5,
    "difficulty_level": "متوسط"
  }
}
```

#### Response
```json
{
  "id": 1,
  "title": "مقال عن الصحة العامة",
  "slug": "health-public-article",
  "summary": "ملخص قصير عن المقال",
  "body": "<p>محتوى المقال الكامل بصيغة HTML</p>",
  "featuredImage": "/uploads/images/health-article.jpg",
  "status": "Draft",
  "publishedAt": null,
  "viewCount": 0,
  "isFeatured": false,
  "contentTypeName": "مقالات",
  "contentTypeId": 1,
  "authorName": "أحمد محمد",
  "metaTitle": "مقال عن الصحة العامة - الهيئة السعودية للتخصصات الصحية",
  "metaDescription": "مقال شامل عن الصحة العامة وأهميتها",
  "metaKeywords": "صحة, صحة عامة, الهيئة السعودية",
  "ogTitle": "مقال عن الصحة العامة",
  "ogDescription": "مقال شامل عن الصحة العامة",
"ogImage": "/uploads/images/health-article-og.jpg",
  "customFields": {
    "author_bio": "كاتب متخصص في الصحة العامة",
    "reading_time": 5,
    "difficulty_level": "متوسط"
  },
  "scheduledAt": null,
  "allowComments": true,
  "tags": [
    {
   "id": 1,
      "name": "صحة عامة",
      "slug": "health",
    "description": "مقالات عن الصحة العامة",
  "color": "#00A86B",
      "usageCount": 15
    }
  ],
  "createdAt": "2025-01-12T10:30:00Z",
  "updatedAt": null
}
```

---

### 2. Update Content

#### Endpoint
```http
PUT /api/contents/{id}
Authorization: Bearer {token}
Content-Type: application/json
```

#### Request Body
```json
{
  "contentTypeId": 1,
  "title": "مقال محدث عن الصحة العامة",
  "slug": "health-public-article-updated",
  "summary": "ملخص محدث",
  "body": "<p>محتوى محدث</p>",
  "status": "Published",
  "isFeatured": true,
  "tagIds": [1, 2, 4]
}
```

---

### 3. Get Content List (with Filtering)

#### Endpoint
```http
GET /api/contents?contentTypeId=1&status=Published&page=1&pageSize=10&sortBy=PublishedAt&sortOrder=desc
```

#### Query Parameters

| Parameter | Type | Description | Example |
|-----------|------|-------------|---------|
| contentTypeId | int? | Filter by content type | 1 |
| status | string? | Filter by status | Published |
| isFeatured | bool? | Filter featured | true |
| authorId | int? | Filter by author | 5 |
| tagIds | int[]? | Filter by tags | [1,2,3] |
| searchTerm | string? | Search in title/summary/body | "صحة" |
| fromDate | DateTime? | From date | 2025-01-01 |
| toDate | DateTime? | To date | 2025-12-31 |
| page | int | Page number | 1 |
| pageSize | int | Items per page | 10 |
| sortBy | string? | Sort field | PublishedAt |
| sortOrder | string? | Sort direction | desc |

#### Response
```json
{
  "items": [
    {
      "id": 1,
   "title": "مقال عن الصحة العامة",
      "slug": "health-public-article",
      "summary": "ملخص قصير",
    "featuredImage": "/uploads/images/health.jpg",
      "status": "Published",
      "publishedAt": "2025-01-12T10:30:00Z",
      "viewCount": 150,
    "isFeatured": true,
      "contentTypeName": "مقالات",
      "contentTypeId": 1,
      "authorName": "أحمد محمد",
      "tags": [
 {
          "id": 1,
          "name": "صحة عامة",
          "slug": "health",
     "color": "#00A86B",
 "usageCount": 15
        }
      ],
      "createdAt": "2025-01-12T10:00:00Z",
 "updatedAt": "2025-01-12T10:30:00Z"
    }
  ],
  "totalCount": 50,
  "page": 1,
  "pageSize": 10,
  "totalPages": 5,
  "hasPrevious": false,
  "hasNext": true
}
```

---

### 4. Get Content by ID

#### Endpoint
```http
GET /api/contents/{id}
```

#### Response
Returns `ContentDetailDto` with all fields including custom fields.

---

### 5. Get Content by Slug

#### Endpoint
```http
GET /api/contents/slug/{slug}?contentTypeId=1
```

Useful for SEO-friendly URLs.

---

### 6. Publish/Unpublish Content

#### Publish
```http
POST /api/contents/{id}/publish
Authorization: Bearer {token}
```

Sets status to "Published" and sets `publishedAt` to current UTC time.

#### Unpublish
```http
POST /api/contents/{id}/unpublish
Authorization: Bearer {token}
```

Sets status back to "Draft".

#### Archive
```http
POST /api/contents/{id}/archive
Authorization: Bearer {token}
```

Sets status to "Archived".

---

### 7. Duplicate Content

#### Endpoint
```http
POST /api/contents/{id}/duplicate
Authorization: Bearer {token}
```

Creates a copy of the content with:
- Title: "{Original Title} (Copy)"
- Slug: "{original-slug}-copy-{random}"
- Status: "Draft"
- All tags and custom fields copied

---

### 8. Delete Content

#### Endpoint
```http
DELETE /api/contents/{id}
Authorization: Bearer {token}
```

Soft delete (sets `IsDeleted = true`).

---

### 9. Featured Content

#### Endpoint
```http
GET /api/contents/featured?contentTypeId=1&count=5
```

Returns top featured content by content type.

---

### 10. Recent Content

#### Endpoint
```http
GET /api/contents/recent?contentTypeId=1&count=10
```

Returns most recent published content.

---

### 11. Content by Tag

#### Endpoint
```http
GET /api/contents/by-tag/{tagId}?pageSize=10&page=1
```

Returns all content with specific tag.

---

## Tag Management

### 1. Create Tag

#### Endpoint
```http
POST /api/tags
Authorization: Bearer {token}
Content-Type: application/json
```

#### Request Body
```json
{
  "name": "صحة عامة",
  "slug": "health",
  "description": "مقالات عن الصحة العامة",
  "color": "#00A86B"
}
```

#### Response
```json
{
  "id": 1,
  "name": "صحة عامة",
  "slug": "health",
  "description": "مقالات عن الصحة العامة",
  "color": "#00A86B",
  "usageCount": 0
}
```

---

### 2. Get All Tags

#### Endpoint
```http
GET /api/tags
```

Returns all tags with usage counts.

---

### 3. Get Tag by ID

#### Endpoint
```http
GET /api/tags/{id}
```

---

### 4. Get Tag by Slug

#### Endpoint
```http
GET /api/tags/slug/{slug}
```

---

### 5. Update Tag

#### Endpoint
```http
PUT /api/tags/{id}
Authorization: Bearer {token}
```

---

### 6. Delete Tag

#### Endpoint
```http
DELETE /api/tags/{id}
Authorization: Bearer {token}
```

---

### 7. Popular Tags

#### Endpoint
```http
GET /api/tags/popular?count=10
```

Returns tags sorted by usage count (most used first).

---

### 8. Search Tags

#### Endpoint
```http
GET /api/tags/search?searchTerm=صحة
```

Searches tags by name.

---

## Advanced Features

### 1. Scheduled Publishing

Set `scheduledAt` when creating/updating content:

```json
{
  "status": "Scheduled",
  "scheduledAt": "2025-01-15T12:00:00Z"
}
```

To auto-publish scheduled content, call:
```csharp
await _contentService.ProcessScheduledContentAsync();
```

**Recommendation**: Set up a background job (Hangfire/Quartz) to run this method every hour.

---

### 2. Custom Dynamic Fields

Custom fields are stored as JSON in `ContentFieldValue` table. Example:

```json
{
  "customFields": {
    "author_bio": "كاتب متخصص",
    "reading_time": 5,
  "difficulty_level": "متوسط",
    "related_links": [
      "https://example.com/link1",
    "https://example.com/link2"
    ],
    "featured_video": {
    "url": "https://youtube.com/watch?v=xyz",
      "thumbnail": "/uploads/video-thumb.jpg"
    }
  }
}
```

You can store any JSON-serializable data structure.

---

### 3. View Counter

View count is automatically incremented when:
- Anonymous user accesses content by ID
- Anonymous user accesses content by slug

```csharp
await _contentService.IncrementViewCountAsync(id);
```

---

### 4. SEO Optimization

#### Meta Tags
```json
{
  "metaTitle": "عنوان محسّن لمحركات البحث",
  "metaDescription": "وصف يجذب النقرات من نتائج البحث",
  "metaKeywords": "كلمة1, كلمة2, كلمة3"
}
```

#### Open Graph
```json
{
  "ogTitle": "عنوان للمشاركة على وسائل التواصل",
  "ogDescription": "وصف يظهر عند المشاركة",
  "ogImage": "/uploads/og-image.jpg"
}
```

#### Best Practices:
- Use unique slug for each content (per content type)
- Fill metaTitle (50-60 characters)
- Fill metaDescription (150-160 characters)
- Use relevant keywords
- Provide OG image (recommended 1200x630px)

---

### 5. Multi-language Support

The system is ready for translations using `TranslatableEntity` base class.

To add translations (future implementation):
```json
{
  "translations": {
    "en": {
      "title": "Public Health Article",
      "slug": "public-health-article",
      "summary": "Short summary",
      "body": "<p>Full content</p>"
    }
  }
}
```

---

## API Examples

### Example 1: Create News Article

```javascript
const article = {
  contentTypeId: 1, // News
  title: "الهيئة تطلق مبادرة جديدة",
  slug: "new-initiative-launch",
  summary: "أطلقت الهيئة مبادرة جديدة لتطوير القطاع الصحي",
  body: "<p>تفاصيل المبادرة...</p>",
  featuredImage: "/uploads/initiative.jpg",
  status: "Published",
  isFeatured: true,
  metaTitle: "الهيئة تطلق مبادرة جديدة للتطوير الصحي",
  metaDescription: "تفاصيل المبادرة الجديدة التي أطلقتها الهيئة",
  tagIds: [1, 5, 8],
  customFields: {
    press_release_date: "2025-01-12",
    contact_person: "أحمد محمد",
    contact_email: "ahmed@scfhs.org.sa"
  }
};

const response = await fetch('/api/contents', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': 'Bearer ' + token
  },
  body: JSON.stringify(article)
});

const result = await response.json();
console.log('Created:', result);
```

---

### Example 2: Filter Published Articles

```javascript
const filters = {
  contentTypeId: 1,
  status: 'Published',
  isFeatured: true,
  searchTerm: 'صحة',
  fromDate: '2025-01-01',
  page: 1,
  pageSize: 20,
  sortBy: 'PublishedAt',
  sortOrder: 'desc'
};

const query = new URLSearchParams(filters).toString();
const response = await fetch(`/api/contents?${query}`);
const result = await response.json();

console.log('Total:', result.totalCount);
console.log('Items:', result.items);
```

---

### Example 3: Get Content with Tags

```javascript
async function getContentByTag(tagSlug) {
  // First, get tag by slug
  const tagResponse = await fetch(`/api/tags/slug/${tagSlug}`);
  const tag = await tagResponse.json();
  
  // Then get content by tag ID
  const contentResponse = await fetch(
    `/api/contents/by-tag/${tag.id}?pageSize=10&page=1`
  );
  const contents = await contentResponse.json();
  
  return contents;
}

const healthArticles = await getContentByTag('health');
console.log('Health articles:', healthArticles);
```

---

## Best Practices

### 1. Slug Generation

**Good slugs:**
- `new-health-initiative-2025`
- `certificate-verification-guide`
- `scfhs-annual-report`

**Bad slugs:**
- `New Initiative!!!` (use lowercase, no special chars)
- `مبادرة جديدة` (use English letters)
- `article_123` (use hyphens, not underscores)

**Recommendation:** Use a slug generator library or function:
```csharp
public static string ToSlug(this string text)
{
    return text
      .ToLower()
        .Replace(" ", "-")
     .Replace("_", "-")
     // Remove non-alphanumeric except hyphens
        .Where(c => char.IsLetterOrDigit(c) || c == '-')
    .Aggregate("", (current, c) => current + c);
}
```

---

### 2. Status Workflow

Recommended workflow:
1. **Draft** → Create content, edit, review
2. **Published** → Make live
3. **Archived** → Keep for history but hide from public

Or use scheduled:
1. **Draft** → Create content
2. **Scheduled** → Set future publish date
3. System auto-publishes at scheduled time

---

### 3. Tag Usage

**Good practices:**
- Use 3-5 tags per content
- Keep tag names consistent
- Use descriptive tag names
- Add tag descriptions
- Use color coding for categories

**Example tag structure:**
- Categories: Health, Education, News (green colors)
- Topics: Diabetes, Surgery, Nursing (blue colors)
- Types: Guide, Article, Report (orange colors)

---

### 4. Custom Fields

**Naming convention:**
- Use snake_case: `reading_time`, `author_bio`
- Be consistent across content types
- Document field meanings
- Validate field values in frontend

**Example structure:**
```json
{
  "reading_time": 5,
  "difficulty_level": "beginner|intermediate|advanced",
  "author_bio": "string",
  "related_content_ids": [1, 2, 3],
  "featured_quote": "string",
  "external_links": ["url1", "url2"]
}
```

---

### 5. Performance

**Optimization tips:**
- Use pagination (don't fetch all items)
- Filter at database level (don't filter in code)
- Use indexes (already configured)
- Cache popular content
- Lazy load related entities when needed

**Example - Good:**
```csharp
// Filter in database
var contents = await _contentRepository.GetAllAsync(new ContentFilterDto
{
    Status = "Published",
    ContentTypeId = 1,
    Page = 1,
    PageSize = 20
});
```

**Example - Bad:**
```csharp
// Don't do this - loads all then filters
var allContents = await _contentRepository.GetAllAsync();
var published = allContents.Where(c => c.Status == "Published");
```

---

## Troubleshooting

### Problem: Slug already exists

**Error:**
```
400 Bad Request: Content with slug 'article-name' already exists
```

**Solution:**
- Use unique slug per content type
- Add suffix: `article-name-2`, `article-name-updated`
- Or update the existing content instead

---

### Problem: Content not appearing in list

**Checklist:**
1. Is status = "Published"? (Draft/Archived don't appear publicly)
2. Is publishedAt set? (Should be auto-set on publish)
3. Is IsDeleted = false? (Check database)
4. Are you filtering correctly?

---

### Problem: Tags not updating usage count

**Cause:** Direct database modification or bug in service.

**Solution:**
```csharp
// Manually recalculate
await _tagRepository.RecalculateUsageCountAsync(tagId);
```

---

### Problem: Custom fields not saving

**Checklist:**
1. Is JSON valid?
2. Are you using correct field keys?
3. Is ContentTypeField configured?

**Debug:**
```csharp
var json = JsonSerializer.Serialize(customFields);
Console.WriteLine(json); // Check serialized JSON
```

---

### Problem: Scheduled content not publishing

**Cause:** Background job not running.

**Solution:**
1. Set up Hangfire/Quartz background job
2. Schedule `ProcessScheduledContentAsync()` to run hourly
3. Or call manually for testing:
```csharp
await _contentService.ProcessScheduledContentAsync();
```

---

## Support

For issues or questions:
- Check API documentation in Swagger: `/swagger`
- Review code examples in this guide
- Check FEATURE_2_COMPLETION_REPORT.md for technical details
- Contact development team

---

**Document Version:** 1.0.0  
**Last Updated:** 12 يناير 2025  
**Feature Status:** ✅ Complete
