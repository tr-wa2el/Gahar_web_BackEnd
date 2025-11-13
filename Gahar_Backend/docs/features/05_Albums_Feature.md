# 📦 Feature 5: نظام الألبومات (Albums System)

**المطور:** Developer 1  
**الأولوية:** Priority 3 (Week 5)  
**المدة المتوقعة:** 3-4 أيام  
**الحالة:** 🟡 جاهز للتطوير

---

## نظرة عامة

### 🎯 الهدف
تطوير نظام ألبومات صور متقدم مع دعم رفع متعدد، إعادة ترتيب، وعرض ديناميكي.

### 📦 المخرجات
- إنشاء ألبومات مع معلومات وصفية
- إضافة/حذف صور من الألبوم
- إعادة ترتيب الصور (Drag & Drop)
- تعيين صورة الغلاف
- عداد المشاهدات
- دعم الترجمة

---

## Implementation Summary

### Models Created
- ✅ `Album.cs` - Album entity
- ✅ `AlbumMedia.cs` - Junction table

### Database Configuration
```csharp
// Album
builder.HasIndex(a => a.Slug).IsUnique();
builder.HasIndex(a => a.IsPublished);

// AlbumMedia
builder.HasIndex(am => new { am.AlbumId, am.MediaId }).IsUnique();
builder.HasIndex(am => am.DisplayOrder);
```

### API Endpoints
```
GET    /api/albums          - Get all albums
GET    /api/albums/{id}     - Get album by ID
GET    /api/albums/slug/{slug}  - Get album by slug
POST   /api/albums          - Create album
PUT    /api/albums/{id}     - Update album
DELETE /api/albums/{id}     - Delete album
POST   /api/albums/{id}/media  - Add media to album
DELETE /api/albums/{id}/media/{mediaId} - Remove media
POST   /api/albums/{id}/reorder-media - Reorder media
POST /api/albums/{id}/increment-views - Increment views
```

---

## Quick Implementation Guide

### 1. Models (Already Created)
```csharp
public class Album : TranslatableEntity
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string? Description { get; set; }
    public int? CoverImageId { get; set; }
    public Media? CoverImage { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; }
    public int? CreatedBy { get; set; }
    public ICollection<AlbumMedia> AlbumMedias { get; set; }
}

public class AlbumMedia : BaseEntity
{
    public int AlbumId { get; set; }
    public int MediaId { get; set; }
    public int DisplayOrder { get; set; }
    public string? Caption { get; set; }
    public bool IsFeatured { get; set; }
}
```

### 2. DTOs Location
`Models/DTOs/Album/AlbumDto.cs`

### 3. Repository Methods
```csharp
Task<Album?> GetBySlugAsync(string slug);
Task<List<AlbumMedia>> GetAlbumMediaAsync(int albumId);
Task AddMediaToAlbumAsync(int albumId, int mediaId, string? caption);
Task RemoveMediaFromAlbumAsync(int albumId, int mediaId);
Task ReorderAlbumMediaAsync(int albumId, List<int> mediaIds);
```

### 4. Service Implementation Focus
- Album CRUD operations
- Media management within album
- Auto cover image selection
- View counter increment
- Slug uniqueness validation

---

## Features Details

### 1. Auto Cover Image
```csharp
// If no cover image set, use first media in album
if (album.CoverImageId == null && album.AlbumMedias.Any())
{
album.CoverImageId = album.AlbumMedias
 .OrderBy(am => am.DisplayOrder)
.First().MediaId;
}
```

### 2. Display Order Management
```csharp
// Auto-assign display order when adding media
var maxOrder = album.AlbumMedias.Any() 
    ? album.AlbumMedias.Max(am => am.DisplayOrder) 
    : 0;
newMedia.DisplayOrder = maxOrder + 1;
```

### 3. Reorder Algorithm
```csharp
public async Task ReorderMediaAsync(int albumId, List<int> mediaIds)
{
    for (int i = 0; i < mediaIds.Count; i++)
 {
        var media = await GetAlbumMedia(albumId, mediaIds[i]);
    media.DisplayOrder = i;
    }
}
```

---

## Testing Checklist

### Album Management
- [ ] Create album
- [ ] Get album with media
- [ ] Update album
- [ ] Delete album (cascade media relations)
- [ ] Slug uniqueness

### Media Management
- [ ] Add single media
- [ ] Add multiple media
- [ ] Remove media
- [ ] Reorder media
- [ ] Set/change cover image

### Display & Features
- [ ] Featured media flag
- [ ] Media captions
- [ ] View counter
- [ ] Published/unpublished status
- [ ] Translation support

---

## Usage Examples

### Create Album
```json
POST /api/albums
{
  "title": "Event Photos 2024",
  "slug": "event-photos-2024",
  "description": "Photos from our annual event",
  "isPublished": true,
  "translations": {
    "en": {
 "title": "Event Photos 2024",
      "description": "Photos from our annual event"
    }
  }
}
```

### Add Media to Album
```json
POST /api/albums/1/media
{
  "mediaId": 5,
  "caption": "Opening ceremony",
  "isFeatured": true
}
```

### Reorder Media
```json
POST /api/albums/1/reorder-media
{
  "mediaIds": [5, 3, 8, 1, 2]
}
```

### Response Example
```json
{
  "id": 1,
  "title": "Event Photos 2024",
  "slug": "event-photos-2024",
  "description": "Photos from our annual event",
  "coverImage": {
    "id": 5,
    "filePath": "/uploads/cover.jpg",
    "thumbnailPath": "/uploads/thumb_cover.jpg"
  },
  "mediaCount": 25,
  "viewCount": 150,
  "isPublished": true,
  "publishedAt": "2024-01-10T10:00:00Z",
  "media": [
    {
      "id": 5,
      "filePath": "/uploads/image1.jpg",
      "caption": "Opening ceremony",
      "displayOrder": 0,
    "isFeatured": true
    }
  ]
}
```

---

## Album Display Options

### Grid Layout
```
[Image1] [Image2] [Image3]
[Image4] [Image5] [Image6]
```

### Masonry Layout
```
[Image1] [Image3]
[Image2] [Image4]
         [Image5]
```

### Carousel Layout
```
← [Featured Image] →
  [Thumb] [Thumb] [Thumb]
```

---

## Performance Considerations

✅ Lazy loading for large albums  
✅ Thumbnail images for grid view  
✅ Cache album metadata  
✅ Paginate media list  
✅ Optimize database queries with Include()  

---

## Security Considerations

✅ User authentication for create/update/delete  
✅ Public access for published albums  
✅ Validate media ownership  
✅ Sanitize captions (XSS prevention)  

---

**Status:** ✅ Ready for Development  
**Estimated Time:** 3-4 days  
**Dependencies:** Media Management System (Feature 4)
