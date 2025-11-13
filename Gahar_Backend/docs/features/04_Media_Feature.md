# 📦 Feature 4: نظام إدارة الملفات والصور (Media Management System)

**المطور:** Developer 1  
**الأولوية:** Priority 2 (Week 3-4)  
**المدة المتوقعة:** 4-5 أيام  
**الحالة:** 🟡 جاهز للتطوير

---

## نظرة عامة

### 🎯 الهدف
تطوير نظام متقدم لرفع ومعالجة الملفات والصور مع تحويل تلقائي لـ WebP وإنشاء thumbnails.

### 📦 المخرجات
- رفع الملفات (Single & Multiple)
- معالجة الصور التلقائية
- تحويل WebP
- إنشاء Thumbnails
- إدارة المكتبة (CRUD)
- البحث والفلترة

### 🔧 التقنيات
- **SixLabors.ImageSharp** - معالجة الصور
- **WebP Encoding** - ضغط الصور

---

## Implementation Summary

### Models Created
- ✅ `Media.cs` - Media entity with metadata

### Database Configuration
```csharp
builder.HasIndex(m => m.MediaType);
builder.HasIndex(m => m.UploadedBy);
builder.HasIndex(m => m.CreatedAt);
```

### API Endpoints
```
POST   /api/media/upload       - Upload single file
POST   /api/media/upload-multiple     - Upload multiple files
GET    /api/media      - Get all media (paginated)
GET    /api/media/{id}    - Get media by ID
PUT    /api/media/{id}  - Update media metadata
DELETE /api/media/{id}       - Delete media
POST   /api/media/{id}/regenerate-webp - Regenerate WebP
```

---

## Quick Implementation Guide

### 1. Model (Already Created)
```csharp
public class Media : BaseEntity
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string? ThumbnailPath { get; set; }
    public string? WebPPath { get; set; }
    public string MimeType { get; set; }
    public long FileSize { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? Alt { get; set; }
    public string MediaType { get; set; }
    public int? UploadedBy { get; set; }
 public bool IsProcessed { get; set; }
}
```

### 2. NuGet Package Required
```xml
<PackageReference Include="SixLabors.ImageSharp" Version="3.1.5" />
```

### 3. Service Implementation Focus

**ProcessImageAsync Method:**
```csharp
private async Task ProcessImageAsync(Media media, string filePath)
{
  using var image = await Image.LoadAsync(filePath);
    
    // Get dimensions
    media.Width = image.Width;
    media.Height = image.Height;
    
    // Generate thumbnail (300x300)
    // Generate WebP version (85% quality)
    // Update media entity
}
```

### 4. File Upload Settings
```json
// appsettings.json
{
  "FileUpload": {
    "MaxFileSize": 10485760,  // 10MB
    "AllowedExtensions": [".jpg", ".jpeg", ".png", ".gif", ".webp"],
    "UploadPath": "wwwroot/uploads"
  }
}
```

---

## Media Types Supported

| Type | Extensions | Max Size |
|------|-----------|----------|
| Image | jpg, jpeg, png, gif, webp | 10 MB |
| Video | mp4, webm | 50 MB |
| Document | pdf, docx | 10 MB |
| Audio | mp3, wav | 10 MB |

---

## Testing Checklist

### Upload Tests
- [ ] Single file upload
- [ ] Multiple files upload
- [ ] File validation (size, type)
- [ ] WebP conversion
- [ ] Thumbnail generation

### Management Tests
- [ ] Get media list with pagination
- [ ] Filter by type
- [ ] Search by filename
- [ ] Update metadata
- [ ] Delete media (and files)

### Performance Tests
- [ ] Large file handling
- [ ] Concurrent uploads
- [ ] WebP regeneration

---

## Usage Example

### Upload Image
```bash
curl -X POST "https://localhost:7000/api/media/upload" \
  -H "Authorization: Bearer {token}" \
  -F "file=@image.jpg" \
  -F "alt=Sample Image"
```

### Response
```json
{
  "id": 1,
  "fileName": "image.jpg",
  "filePath": "/uploads/guid_image.jpg",
  "thumbnailPath": "/uploads/thumb_guid_image.jpg",
  "webPPath": "/uploads/guid_image.webp",
  "mimeType": "image/jpeg",
  "fileSize": 524288,
  "webPFileSize": 245678,
  "width": 1920,
"height": 1080,
  "mediaType": "Image",
  "isProcessed": true
}
```

---

## Image Processing Flow

```
Upload File
    ↓
Save Original
    ↓
Get Dimensions
    ↓
Create Thumbnail (300x300)
    ↓
Convert to WebP (85% quality)
    ↓
Save Metadata to DB
    ↓
Return Media DTO
```

---

## Security Considerations

✅ File type validation  
✅ File size limits  
✅ Unique file naming (GUID)  
✅ Virus scanning (TODO)  
✅ User authentication required  
✅ File path sanitization  

---

**Status:** ✅ Ready for Development  
**Estimated Time:** 4-5 days  
**Priority:** High (Required for Content Management)
