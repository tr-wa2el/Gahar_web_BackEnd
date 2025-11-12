# Content Types Feature - Usage Guide

## Overview
The Content Types feature allows you to dynamically create custom content types (like News, Events, Services) without writing any code. Each content type can have custom fields with validation rules.

## API Endpoints

### 1. Get All Content Types
```http
GET /api/contenttypes
Authorization: Bearer {token}
Permission: ContentTypes.View
```

**Response:**
```json
[
  {
    "id": 1,
  "name": "News",
    "slug": "news",
    "description": "News articles",
    "icon": "Newspaper",
    "isSinglePage": false,
    "isActive": true,
    "displayOrder": 0,
    "contentCount": 15,
    "createdAt": "2025-01-11T10:00:00Z"
  }
]
```

### 2. Get Content Type by ID
```http
GET /api/contenttypes/{id}
Authorization: Bearer {token}
Permission: ContentTypes.View
```

**Response:**
```json
{
  "id": 1,
  "name": "News",
  "slug": "news",
  "description": "News articles",
  "icon": "Newspaper",
  "isSinglePage": false,
  "isActive": true,
  "displayOrder": 0,
  "contentCount": 15,
  "createdAt": "2025-01-11T10:00:00Z",
  "metaTitleTemplate": "{Title} - Gahar News",
  "metaDescriptionTemplate": "{Excerpt}",
  "fields": [
    {
      "id": 1,
 "name": "Author",
      "fieldKey": "author",
      "fieldType": "Text",
      "description": "Article author name",
      "isRequired": true,
      "isTranslatable": false,
      "showInList": true,
      "displayOrder": 0,
      "validationRules": "{\"minLength\": 3, \"maxLength\": 100}",
      "defaultValue": null,
      "placeholder": "Enter author name",
  "options": null
    }
]
}
```

### 3. Get Content Type by Slug
```http
GET /api/contenttypes/slug/{slug}
Authorization: None (Public endpoint)
```

### 4. Create Content Type
```http
POST /api/contenttypes
Authorization: Bearer {token}
Permission: ContentTypes.Create
Content-Type: application/json
```

**Request Body:**
```json
{
  "name": "News",
  "slug": "news",
  "description": "News articles and updates",
  "icon": "Newspaper",
  "isSinglePage": false,
  "metaTitleTemplate": "{Title} - Gahar News",
  "metaDescriptionTemplate": "{Excerpt}"
}
```

**Response:** Content Type object

### 5. Update Content Type
```http
PUT /api/contenttypes/{id}
Authorization: Bearer {token}
Permission: ContentTypes.Edit
Content-Type: application/json
```

**Request Body:**
```json
{
  "name": "Updated News",
  "slug": "updated-news",
  "description": "Latest news and updates",
  "icon": "Newspaper",
  "isSinglePage": false,
  "isActive": true,
  "displayOrder": 1,
  "metaTitleTemplate": "{Title} - Gahar",
  "metaDescriptionTemplate": "{Excerpt}"
}
```

### 6. Delete Content Type
```http
DELETE /api/contenttypes/{id}
Authorization: Bearer {token}
Permission: ContentTypes.Delete
```

**Response:** 204 No Content

### 7. Add Field to Content Type
```http
POST /api/contenttypes/{id}/fields
Authorization: Bearer {token}
Permission: ContentTypes.Edit
Content-Type: application/json
```

**Request Body:**
```json
{
  "name": "Event Date",
  "fieldKey": "event_date",
  "fieldType": "Date",
  "description": "The date when the event will take place",
  "isRequired": true,
  "isTranslatable": false,
  "showInList": true,
  "validationRules": "{\"min\": \"2025-01-01\"}",
  "defaultValue": null,
  "placeholder": "Select event date"
}
```

### 8. Add Field with Options (Select/Radio)
```http
POST /api/contenttypes/{id}/fields
Authorization: Bearer {token}
Permission: ContentTypes.Edit
Content-Type: application/json
```

**Request Body:**
```json
{
  "name": "Priority",
  "fieldKey": "priority",
  "fieldType": "Select",
  "description": "Priority level",
  "isRequired": true,
  "isTranslatable": false,
  "showInList": true,
  "options": ["Low", "Medium", "High", "Critical"]
}
```

### 9. Update Field
```http
PUT /api/contenttypes/{id}/fields/{fieldId}
Authorization: Bearer {token}
Permission: ContentTypes.Edit
Content-Type: application/json
```

**Request Body:**
```json
{
  "name": "Updated Field Name",
  "fieldKey": "updated_field_key",
  "fieldType": "Text",
  "description": "Updated description",
  "isRequired": false,
  "isTranslatable": true,
  "showInList": true,
  "displayOrder": 0,
  "validationRules": "{\"maxLength\": 200}",
  "placeholder": "Enter text here"
}
```

### 10. Delete Field
```http
DELETE /api/contenttypes/{id}/fields/{fieldId}
Authorization: Bearer {token}
Permission: ContentTypes.Edit
```

**Response:** 204 No Content

### 11. Reorder Fields
```http
POST /api/contenttypes/{id}/reorder-fields
Authorization: Bearer {token}
Permission: ContentTypes.Edit
Content-Type: application/json
```

**Request Body:**
```json
{
  "fieldIds": [3, 1, 2]
}
```

## Supported Field Types

| Field Type | Description | Validation Rules Example |
|-----------|-------------|-------------------------|
| Text | Single-line text input | `{"minLength": 3, "maxLength": 100}` |
| Textarea | Multi-line text input | `{"maxLength": 500}` |
| RichText | HTML editor | `{"maxLength": 5000}` |
| Number | Numeric input | `{"min": 0, "max": 100}` |
| Date | Date picker | `{"min": "2025-01-01"}` |
| DateTime | Date and time picker | `{"min": "2025-01-01T00:00:00"}` |
| Boolean | Checkbox/Toggle | N/A |
| Select | Dropdown with single selection | Requires `options` array |
| MultiSelect | Dropdown with multiple selection | Requires `options` array |
| Radio | Radio buttons | Requires `options` array |
| Checkbox | Multiple checkboxes | Requires `options` array |
| Image | Image upload | `{"maxSize": 5242880, "allowedTypes": ["jpg", "png"]}` |
| File | File upload | `{"maxSize": 10485760}` |
| Color | Color picker | N/A |
| Email | Email input with validation | `{"email": true}` |
| Url | URL input with validation | `{"url": true}` |
| Phone | Phone number input | `{"pattern": "^[0-9]{10}$"}` |

## Validation Rules Format

### Text/Textarea/RichText
```json
{
  "minLength": 10,
  "maxLength": 500,
  "pattern": "^[A-Za-z]+$",
  "required": true
}
```

### Number
```json
{
  "min": 0,
  "max": 100,
  "required": true
}
```

### Date/DateTime
```json
{
  "min": "2025-01-01",
  "max": "2025-12-31",
  "required": true
}
```

### Email
```json
{
  "email": true,
  "required": true
}
```

### URL
```json
{
  "url": true,
  "required": true
}
```

### Image/File
```json
{
  "maxSize": 5242880,
  "allowedTypes": ["jpg", "png", "gif"],
  "required": true
}
```

## Usage Examples

### Example 1: Creating a News Content Type

1. **Create the content type:**
```bash
POST /api/contenttypes
{
  "name": "News",
  "slug": "news",
  "description": "News articles and updates",
  "icon": "Newspaper",
  "metaTitleTemplate": "{Title} - Gahar News"
}
```

2. **Add Title field:**
```bash
POST /api/contenttypes/1/fields
{
  "name": "Title",
  "fieldKey": "title",
  "fieldType": "Text",
  "isRequired": true,
  "isTranslatable": true,
  "showInList": true,
  "validationRules": "{\"minLength\": 10, \"maxLength\": 200}",
  "placeholder": "Enter article title"
}
```

3. **Add Content field:**
```bash
POST /api/contenttypes/1/fields
{
  "name": "Content",
  "fieldKey": "content",
  "fieldType": "RichText",
  "isRequired": true,
  "isTranslatable": true,
  "showInList": false,
  "validationRules": "{\"minLength\": 50}",
  "placeholder": "Enter article content"
}
```

4. **Add Author field:**
```bash
POST /api/contenttypes/1/fields
{
  "name": "Author",
  "fieldKey": "author",
  "fieldType": "Text",
  "isRequired": true,
  "isTranslatable": false,
  "showInList": true,
  "validationRules": "{\"maxLength\": 100}",
  "placeholder": "Enter author name"
}
```

5. **Add Featured Image field:**
```bash
POST /api/contenttypes/1/fields
{
  "name": "Featured Image",
  "fieldKey": "featured_image",
  "fieldType": "Image",
  "isRequired": false,
  "isTranslatable": false,
  "showInList": true,
  "validationRules": "{\"maxSize\": 5242880, \"allowedTypes\": [\"jpg\", \"png\"]}"
}
```

### Example 2: Creating an Events Content Type

```bash
POST /api/contenttypes
{
  "name": "Events",
  "slug": "events",
  "description": "Upcoming events",
  "icon": "Calendar"
}
```

Add fields:
- Event Name (Text, required, translatable)
- Event Date (DateTime, required)
- Location (Text, required, translatable)
- Event Type (Select with options: ["Conference", "Workshop", "Seminar"])
- Registration Link (Url)

### Example 3: Creating a Single Page (About Us)

```bash
POST /api/contenttypes
{
  "name": "About Us",
  "slug": "about-us",
  "description": "About us page",
  "icon": "Info",
  "isSinglePage": true
}
```

## Testing

Run the unit tests:
```bash
dotnet test --filter "FullyQualifiedName~ContentTypeServiceTests"
```

All 12 tests should pass:
- ✅ GetAllAsync_ShouldReturnAllContentTypes
- ✅ GetByIdAsync_WithValidId_ShouldReturnContentType
- ✅ GetByIdAsync_WithInvalidId_ShouldThrowNotFoundException
- ✅ CreateAsync_WithValidData_ShouldCreateContentType
- ✅ CreateAsync_WithDuplicateSlug_ShouldThrowBadRequestException
- ✅ UpdateAsync_WithValidData_ShouldUpdateContentType
- ✅ DeleteAsync_WithValidId_ShouldDeleteContentType
- ✅ AddFieldAsync_WithValidData_ShouldAddField
- ✅ AddFieldAsync_WithDuplicateFieldKey_ShouldThrowBadRequestException
- ✅ UpdateFieldAsync_WithValidData_ShouldUpdateField
- ✅ DeleteFieldAsync_WithValidId_ShouldDeleteField
- ✅ ReorderFieldsAsync_ShouldUpdateFieldOrders

## Notes

- All slugs are automatically converted to lowercase with hyphens
- Field keys must be lowercase with underscores
- Soft delete is implemented - deleted records are marked as `IsDeleted = true`
- All create/update/delete operations are logged in the audit log
- Content types with existing content cannot be deleted (foreign key constraint)
- Duplicate slugs and field keys are prevented at the database level

## Next Steps

After implementing the Content Types feature, proceed to:
1. **Feature 2: Dynamic Content System** - Create actual content items based on these types
2. **Feature 3: Layouts & Pages** - Create page layouts
3. **Feature 4: Media Management** - Handle file uploads
4. **Feature 5: Albums System** - Organize media in albums
