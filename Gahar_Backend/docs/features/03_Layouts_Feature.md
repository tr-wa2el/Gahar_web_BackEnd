# 📦 Feature 3: نظام التخطيطات (Layouts System)

**المطور:** Developer 1  
**الأولوية:** Priority 2 (Week 3)  
**المدة المتوقعة:** 2-3 أيام  
**الحالة:** 🟡 جاهز للتطوير

---

## نظرة عامة

### 🎯 الهدف
تطوير نظام تخطيطات مرن يسمح بإنشاء قوالب عرض مختلفة للمحتوى مع تكوينات قابلة للتخصيص.

### 📦 المخرجات
- إدارة تخطيطات CRUD
- تكوينات JSON مخصصة
- تعيين تخطيط افتراضي
- معاينة التخطيطات

---

## Implementation Summary

### Models Created
- ✅ `Layout.cs` - Basic layout entity

### Database Configuration
```csharp
// Entity Configuration
builder.ToTable("Layouts");
builder.HasIndex(l => l.Name).IsUnique();
builder.Property(l => l.Configuration).IsRequired();
```

### API Endpoints
```
GET    /api/layouts          - Get all layouts
GET    /api/layouts/{id}     - Get layout by ID
POST /api/layouts      - Create layout
PUT    /api/layouts/{id}     - Update layout
DELETE /api/layouts/{id}     - Delete layout
POST   /api/layouts/{id}/set-default - Set as default
```

---

## Quick Implementation Guide

### 1. Model (Already Created)
```csharp
public class Layout : TranslatableEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Configuration { get; set; } // JSON
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public string? PreviewImage { get; set; }
    public ICollection<Content> Contents { get; set; }
}
```

### 2. DTOs Location
`Models/DTOs/Layout/LayoutDto.cs`

### 3. Repository Interface
`Repositories/Interfaces/ILayoutRepository.cs`

### 4. Service Interface
`Services/Interfaces/ILayoutService.cs`

### 5. Controller
`Controllers/LayoutsController.cs`

---

## Testing Checklist

- [ ] Create layout
- [ ] Get all layouts
- [ ] Get layout by ID
- [ ] Update layout
- [ ] Delete layout
- [ ] Set default layout
- [ ] Layout configuration validation

---

## Usage Example

```json
// Create Layout Request
{
  "name": "Featured Article",
  "description": "Layout for featured articles",
  "configuration": {
    "showAuthor": true,
    "showDate": true,
    "showTags": true,
    "imageSize": "large"
  },
  "isActive": true
}
```

---

**Status:** ✅ Ready for Development  
**Estimated Time:** 2-3 days
