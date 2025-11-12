# 👨‍💻 خطة تطوير المطور الأول (Developer 1)
## Features Package A - CMS Core & Content Management

**تاريخ الإنشاء:** 11 يناير 2025  
**المدة المتوقعة:** 4-6 أسابيع  
**التبعيات:** ✅ الجزء المشترك مكتمل  
**الحالة:** 📋 جاهز للتنفيذ

---

## 📋 جدول المحتويات
1. [نظرة عامة](#نظرة-عامة)
2. [الـ Features المفصلة](#الـ-features-المفصلة)
3. [الجدول الزمني](#الجدول-الزمني)
4. [الملفات المطلوبة](#الملفات-المطلوبة)

---

## نظرة عامة

### 🎯 الهدف
تطوير النواة الأساسية لنظام إدارة المحتوى (CMS Core) الذي يسمح بإنشاء وإدارة محتوى ديناميكي بالكامل.

### 📦 الـ Features المسؤول عنها
1. ✅ **Content Types Management** - إدارة أنواع المحتوى
2. ✅ **Dynamic Content Management** - إدارة المحتوى الديناميكي
3. ✅ **Layout System** - نظام التخطيطات
4. ✅ **File & Image Management** - إدارة الملفات والصور
5. ✅ **Albums System** - نظام الألبومات

### 🔗 التكامل مع المطور الثاني
- لا توجد تبعيات مباشرة
- يوفر APIs يستخدمها Page Builder
- نظام المحتوى مستقل تماماً

---

## الـ Features المفصلة

### 📂 Feature Documentation Files

كل Feature له ملف توثيق مفصل منفصل:

| Feature | الملف | الأولوية | المدة |
|---------|------|----------|-------|
| Content Types System | [`features/01_ContentTypes_Feature.md`](features/01_ContentTypes_Feature.md) | P1 | 3-4 أيام |
| Dynamic Content System | [`features/02_DynamicContent_Feature.md`](features/02_DynamicContent_Feature.md) | P1 | 5-7 أيام |
| Layouts System | [`features/03_Layouts_Feature.md`](features/03_Layouts_Feature.md) | P2 | 2-3 أيام |
| Media Management | [`features/04_Media_Feature.md`](features/04_Media_Feature.md) | P2 | 4-5 أيام |
| Albums System | [`features/05_Albums_Feature.md`](features/05_Albums_Feature.md) | P3 | 3-4 أيام |

📚 **الفهرس الشامل:** [`features/README.md`](features/README.md)

---

## الجدول الزمني

### 🗓️ Week 1-2: Core Content Features (Priority 1)
**Duration:** 8-11 days

#### Week 1 (Days 1-4): Content Types
- ✅ إنشاء Models & Configurations
- ✅ تطوير Repository & Service
- ✅ إنشاء DTOs & Controller
- ✅ كتابة Unit Tests (10+ tests)
- ✅ اختبار APIs

**Deliverable:** نظام Content Types كامل وعامل

#### Week 2 (Days 5-11): Dynamic Content
- ✅ إنشاء Models (Content, Tags, FieldValues)
- ✅ تطوير Repository مع Filtering & Pagination
- ✅ تطوير Service مع Custom Fields Handling
- ✅ إنشاء Controller مع جميع Endpoints
- ✅ كتابة Unit Tests (15+ tests)
- ✅ اختبار الفلترة والبحث

**Deliverable:** نظام محتوى ديناميكي كامل

---

### 🗓️ Week 3-4: Display & Media (Priority 2)
**Duration:** 6-8 days

#### Week 3 (Days 12-14): Layouts System
- ✅ إنشاء Layout Model
- ✅ تطوير CRUD operations
- ✅ JSON Configuration System
- ✅ كتابة Unit Tests (8+ tests)

**Deliverable:** نظام تخطيطات جاهز

#### Week 3-4 (Days 15-20): Media Management
- ✅ إنشاء Media Model
- ✅ تطوير File Upload Service
- ✅ تطبيق Image Processing (SixLabors.ImageSharp)
- ✅ تطبيق WebP Conversion
- ✅ تطبيق Thumbnail Generation
- ✅ إنشاء Media Library Management
- ✅ كتابة Unit Tests (12+ tests)

**Deliverable:** نظام إدارة ملفات متقدم

---

### 🗓️ Week 5: Albums System (Priority 3)
**Duration:** 3-4 days

#### Week 5 (Days 21-24): Albums
- ✅ إنشاء Album Models
- ✅ تطوير Album Management
- ✅ تطبيق Media Organization
- ✅ تطبيق Reordering System
- ✅ كتابة Unit Tests (10+ tests)

**Deliverable:** نظام ألبومات كامل

---

### 🗓️ Week 6: Testing & Polish
**Duration:** 5 days

#### Testing Phase
- ✅ مراجعة جميع Unit Tests
- ✅ كتابة Integration Tests
- ✅ اختبار الأداء
- ✅ Code Review
- ✅ إصلاح Bugs

#### Documentation Phase
- ✅ توثيق APIs في Swagger
- ✅ إنشاء Postman Collection
- ✅ كتابة Usage Examples
- ✅ تحديث README

**Deliverable:** نظام CMS Core كامل ومختبر

---

## الملفات المطلوبة

### 📁 Models Structure
```
Gahar_Backend/Models/Entities/
├── ContentType.cs ✅
├── ContentTypeField.cs ✅
├── Content.cs ✅
├── ContentFieldValue.cs ✅
├── Tag.cs ✅
├── ContentTag.cs ✅
├── Layout.cs ✅
├── Media.cs ✅
├── Album.cs ✅
└── AlbumMedia.cs ✅
```

### 📁 DTOs Structure
```
Gahar_Backend/Models/DTOs/
├── ContentType/
│   └── ContentTypeDto.cs
├── Content/
│   └── ContentDto.cs
├── Layout/
│   └── LayoutDto.cs
├── Media/
│   └── MediaDto.cs
└── Album/
    └── AlbumDto.cs
```

### 📁 Repository Structure
```
Gahar_Backend/Repositories/
├── Interfaces/
│   ├── IContentTypeRepository.cs
│   ├── IContentRepository.cs
│   ├── ILayoutRepository.cs
│   ├── IMediaRepository.cs
│   └── IAlbumRepository.cs
└── Implementations/
    ├── ContentTypeRepository.cs
    ├── ContentRepository.cs
    ├── LayoutRepository.cs
    ├── MediaRepository.cs
    └── AlbumRepository.cs
```

### 📁 Service Structure
```
Gahar_Backend/Services/
├── Interfaces/
│   ├── IContentTypeService.cs
│   ├── IContentService.cs
│   ├── ILayoutService.cs
│   ├── IMediaService.cs
│   └── IAlbumService.cs
└── Implementations/
    ├── ContentTypeService.cs
    ├── ContentService.cs
    ├── LayoutService.cs
    ├── MediaService.cs
    └── AlbumService.cs
```

### 📁 Controller Structure
```
Gahar_Backend/Controllers/
├── ContentTypesController.cs
├── ContentsController.cs
├── LayoutsController.cs
├── MediaController.cs
└── AlbumsController.cs
```

### 📁 Tests Structure
```
Gahar_Backend.Tests/Features/
├── ContentTypeServiceTests.cs (10+ tests)
├── ContentServiceTests.cs (15+ tests)
├── LayoutServiceTests.cs (8+ tests)
├── MediaServiceTests.cs (12+ tests)
└── AlbumServiceTests.cs (10+ tests)
```

---

## 📦 NuGet Packages الإضافية

```xml
<!-- Image Processing -->
<PackageReference Include="SixLabors.ImageSharp" Version="3.1.5" />
<PackageReference Include="SixLabors.ImageSharp.Web" Version="3.1.3" />
```

**⚠️ ملاحظة:** تحديث من النسخة 3.1.0 إلى 3.1.5 لحل الثغرات الأمنية

---

## ✅ Checklist شامل

### Week 1: Content Types ✅
- [ ] Models & Configurations
- [ ] Repository & Interface
- [ ] Service & Interface
- [ ] DTOs
- [ ] Controller
- [ ] 10+ Unit Tests
- [ ] API Testing

### Week 2: Dynamic Content ✅
- [ ] Models (Content, Tags, FieldValues)
- [ ] Configurations
- [ ] Repository with Filtering
- [ ] Service with Custom Fields
- [ ] DTOs
- [ ] Controller
- [ ] 15+ Unit Tests
- [ ] Search & Filter Testing

### Week 3: Layouts ✅
- [ ] Layout Model
- [ ] Configuration
- [ ] Repository
- [ ] Service
- [ ] DTOs
- [ ] Controller
- [ ] 8+ Unit Tests

### Week 3-4: Media Management ✅
- [ ] Media Model
- [ ] Configuration
- [ ] Repository
- [ ] Upload Service
- [ ] Image Processing
- [ ] WebP Conversion
- [ ] Thumbnail Generation
- [ ] DTOs
- [ ] Controller
- [ ] 12+ Unit Tests

### Week 5: Albums ✅
- [ ] Album Models
- [ ] Configurations
- [ ] Repository
- [ ] Service
- [ ] DTOs
- [ ] Controller
- [ ] Reordering Logic
- [ ] 10+ Unit Tests

### Week 6: Final Testing ✅
- [ ] All Unit Tests Passing (55+ tests)
- [ ] Integration Tests
- [ ] Performance Testing
- [ ] Swagger Documentation
- [ ] Postman Collection
- [ ] Code Review
- [ ] Bug Fixes

---

## 🎯 الأولويات

**Priority 1 (أسبوع 1-2):**
- ✅ Content Types System
- ✅ Dynamic Content Management

**Priority 2 (أسبوع 3-4):**
- ✅ Layouts System
- ✅ Media Upload & Processing

**Priority 3 (أسبوع 5):**
- ✅ Albums System

**Priority 4 (أسبوع 6):**
- ✅ Testing & Documentation

---

## 📊 التقدم المتوقع

| Week | Features | Completion % |
|------|----------|--------------|
| 1 | Content Types | 20% |
| 2 | Dynamic Content | 40% |
| 3 | Layouts | 50% |
| 4 | Media (Part 1) | 65% |
| 5 | Media (Part 2) + Albums | 85% |
| 6 | Testing & Documentation | 100% |

---

## 🔗 روابط مفيدة

- [Feature Documentation Index](features/README.md)
- [Content Types Feature](features/01_ContentTypes_Feature.md)
- [Dynamic Content Feature](features/02_DynamicContent_Feature.md)
- [Layouts Feature](features/03_Layouts_Feature.md)
- [Media Management Feature](features/04_Media_Feature.md)
- [Albums Feature](features/05_Albums_Feature.md)

---

## 📞 الدعم والمساعدة

### Architecture Reference
- Base Foundation Documentation ✅
- Entity Configurations Examples ✅
- Existing Services (Auth, Translation, AuditLog) ✅

### Code Standards
- Follow existing patterns in AuthController
- Use Repository pattern consistently
- Implement comprehensive error handling
- Write XML documentation comments

### Testing Standards
- Minimum 10 tests per service
- Cover happy path and edge cases
- Use In-Memory database for tests
- Mock external dependencies

---

**تاريخ الإنشاء:** 11 يناير 2025  
**آخر تحديث:** 11 يناير 2025  
**الإصدار:** 2.0.0  
**الحالة:** ✅ جاهز للتنفيذ
