# 👨‍💻 خطة تطوير المطور الأول (Developer 1)
## Features Package A - CMS Core & Content Management

**تاريخ الإنشاء:** 11 نوفمبر 2025  
**المدة المتوقعة:** 4-6 أسابيع  
**التبعيات:** يجب إكمال الجزء المشترك أولاً

---

## 📋 جدول المحتويات
1. [نظرة عامة](#نظرة-عامة)
2. [Feature 1: نظام أنواع المحتوى (Content Types)](#feature-1-نظام-أنواع-المحتوى)
3. [Feature 2: نظام المحتوى الديناميكي (Dynamic Content)](#feature-2-نظام-المحتوى-الديناميكي)
4. [Feature 3: نظام التخطيطات (Layouts)](#feature-3-نظام-التخطيطات)
5. [Feature 4: نظام رفع الملفات والصور](#feature-4-نظام-رفع-الملفات-والصور)
6. [Feature 5: نظام الألبومات (Albums)](#feature-5-نظام-الألبومات)

---

## نظرة عامة

### 🎯 الهدف
تطوير النواة الأساسية لنظام إدارة المحتوى (CMS Core) الذي يسمح بإنشاء وإدارة محتوى ديناميكي بالكامل.

### 📦 الـ Features المسؤول عنها
1. ✅ Content Types Management (إدارة أنواع المحتوى)
2. ✅ Dynamic Content Management (إدارة المحتوى الديناميكي)
3. ✅ Layout System (نظام التخطيطات)
4. ✅ File & Image Management (إدارة الملفات والصور)
5. ✅ Albums System (نظام الألبومات)

### 🔗 التكامل مع المطور الثاني
- لا توجد تبعيات مباشرة
- يوفر APIs يستخدمها Page Builder
- نظام المحتوى مستقل تماماً

---

## Feature 1: نظام أنواع المحتوى (Content Types)

### 📝 الوصف
نظام يسمح بإنشاء أقسام محتوى مخصصة (أخبار، فعاليات، خدمات) بدون كتابة كود.

### 🗄️ Database Models

#### 1.1 ContentType Model

**الملف:** `Models/Entities/ContentType.cs`

```csharp
public class ContentType : TranslatableEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } // e.g., "News", "Events"

    [Required]
    [StringLength(100)]
    public string Slug { get; set; } // e.g., "news", "events"

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string Icon { get; set; } = "FileText"; // Lucide icon name

    public bool IsSinglePage { get; set; } = false; // Single page like "About Us"

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    // SEO Settings
    [StringLength(200)]
    public string? MetaTitleTemplate { get; set; } // e.g., "{Title} - Gahar"

    [StringLength(500)]
    public string? MetaDescriptionTemplate { get; set; }

    // Navigation Properties
    public ICollection<ContentTypeField> Fields { get; set; } = new List<ContentTypeField>();
    public ICollection<Content> Contents { get; set; } = new List<Content>();
}
```

#### 1.2 ContentTypeField Model

**الملف:** `Models/Entities/ContentTypeField.cs`

```csharp
public class ContentTypeField : TranslatableEntity
{
    public int ContentTypeId { get; set; }
    public ContentType ContentType { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } // e.g., "EventDate", "Location"

    [Required]
    [StringLength(100)]
    public string FieldKey { get; set; } // e.g., "event_date", "location"

    [Required]
    [StringLength(50)]
    public string FieldType { get; set; } // Text, Textarea, RichText, Date, Image, File, etc.

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsRequired { get; set; } = false;

    public bool IsTranslatable { get; set; } = true;

    public bool ShowInList { get; set; } = true; // Show in content list

    public int DisplayOrder { get; set; }

    public string? ValidationRules { get; set; } // JSON: {"minLength": 10, "maxLength": 500}

    public string? DefaultValue { get; set; }

    [StringLength(500)]
    public string? Placeholder { get; set; }

    // For Dropdown, Radio, Checkbox
    public string? Options { get; set; } // JSON: ["Option1", "Option2"]
}
```

### 🎯 API Endpoints

#### 1.3 ContentTypesController

**الملف:** `Controllers/ContentTypesController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContentTypesController : ControllerBase
{
    private readonly IContentTypeService _contentTypeService;

    public ContentTypesController(IContentTypeService contentTypeService)
    {
        _contentTypeService = contentTypeService;
    }

    // GET: api/contenttypes
    [HttpGet]
    [Permission(Permissions.ContentTypesView)]
    public async Task<ActionResult<IEnumerable<ContentTypeDto>>> GetAll()
    {
        var contentTypes = await _contentTypeService.GetAllAsync();
        return Ok(contentTypes);
    }

    // GET: api/contenttypes/{id}
    [HttpGet("{id}")]
    [Permission(Permissions.ContentTypesView)]
    public async Task<ActionResult<ContentTypeDetailDto>> GetById(int id)
    {
        var contentType = await _contentTypeService.GetByIdAsync(id);
        return Ok(contentType);
    }

    // GET: api/contenttypes/slug/{slug}
    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<ActionResult<ContentTypeDto>> GetBySlug(string slug)
    {
        var contentType = await _contentTypeService.GetBySlugAsync(slug);
        return Ok(contentType);
    }

    // POST: api/contenttypes
    [HttpPost]
    [Permission(Permissions.ContentTypesCreate)]
    public async Task<ActionResult<ContentTypeDto>> Create([FromBody] CreateContentTypeDto dto)
    {
        var contentType = await _contentTypeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = contentType.Id }, contentType);
    }

    // PUT: api/contenttypes/{id}
    [HttpPut("{id}")]
    [Permission(Permissions.ContentTypesEdit)]
    public async Task<ActionResult<ContentTypeDto>> Update(int id, [FromBody] UpdateContentTypeDto dto)
    {
        var contentType = await _contentTypeService.UpdateAsync(id, dto);
        return Ok(contentType);
    }

    // DELETE: api/contenttypes/{id}
    [HttpDelete("{id}")]
    [Permission(Permissions.ContentTypesDelete)]
    public async Task<ActionResult> Delete(int id)
    {
        await _contentTypeService.DeleteAsync(id);
        return NoContent();
    }

    // POST: api/contenttypes/{id}/fields
    [HttpPost("{id}/fields")]
    [Permission(Permissions.ContentTypesEdit)]
    public async Task<ActionResult<ContentTypeFieldDto>> AddField(int id, [FromBody] CreateContentTypeFieldDto dto)
    {
        var field = await _contentTypeService.AddFieldAsync(id, dto);
        return Ok(field);
    }

    // PUT: api/contenttypes/{id}/fields/{fieldId}
    [HttpPut("{id}/fields/{fieldId}")]
    [Permission(Permissions.ContentTypesEdit)]
    public async Task<ActionResult<ContentTypeFieldDto>> UpdateField(int id, int fieldId, [FromBody] UpdateContentTypeFieldDto dto)
    {
        var field = await _contentTypeService.UpdateFieldAsync(id, fieldId, dto);
        return Ok(field);
    }

    // DELETE: api/contenttypes/{id}/fields/{fieldId}
    [HttpDelete("{id}/fields/{fieldId}")]
    [Permission(Permissions.ContentTypesEdit)]
    public async Task<ActionResult> DeleteField(int id, int fieldId)
    {
        await _contentTypeService.DeleteFieldAsync(id, fieldId);
        return NoContent();
    }

    // POST: api/contenttypes/{id}/reorder-fields
    [HttpPost("{id}/reorder-fields")]
    [Permission(Permissions.ContentTypesEdit)]
    public async Task<ActionResult> ReorderFields(int id, [FromBody] ReorderFieldsDto dto)
    {
        await _contentTypeService.ReorderFieldsAsync(id, dto.FieldIds);
        return NoContent();
    }
}
```

### 📊 DTOs

**الملف:** `Models/DTOs/ContentType/ContentTypeDto.cs`

```csharp
public class ContentTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string? Description { get; set; }
    public string Icon { get; set; }
    public bool IsSinglePage { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public int ContentCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ContentTypeDetailDto : ContentTypeDto
{
    public List<ContentTypeFieldDto> Fields { get; set; }
    public string? MetaTitleTemplate { get; set; }
    public string? MetaDescriptionTemplate { get; set; }
}

public class ContentTypeFieldDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FieldKey { get; set; }
    public string FieldType { get; set; }
    public string? Description { get; set; }
    public bool IsRequired { get; set; }
    public bool IsTranslatable { get; set; }
    public bool ShowInList { get; set; }
    public int DisplayOrder { get; set; }
    public string? ValidationRules { get; set; }
    public string? DefaultValue { get; set; }
    public string? Placeholder { get; set; }
    public List<string>? Options { get; set; }
}

public class CreateContentTypeDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Slug { get; set; }

    public string? Description { get; set; }
    public string Icon { get; set; } = "FileText";
    public bool IsSinglePage { get; set; } = false;
    public string? MetaTitleTemplate { get; set; }
    public string? MetaDescriptionTemplate { get; set; }
}

public class UpdateContentTypeDto : CreateContentTypeDto
{
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}

public class CreateContentTypeFieldDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string FieldKey { get; set; }

    [Required]
    public string FieldType { get; set; }

    public string? Description { get; set; }
    public bool IsRequired { get; set; } = false;
    public bool IsTranslatable { get; set; } = true;
    public bool ShowInList { get; set; } = true;
    public string? ValidationRules { get; set; }
    public string? DefaultValue { get; set; }
    public string? Placeholder { get; set; }
    public List<string>? Options { get; set; }
}

public class UpdateContentTypeFieldDto : CreateContentTypeFieldDto
{
    public int DisplayOrder { get; set; }
}

public class ReorderFieldsDto
{
    [Required]
    public List<int> FieldIds { get; set; }
}
```

### 🔧 Services

**الملف:** `Services/Interfaces/IContentTypeService.cs`

```csharp
public interface IContentTypeService
{
    Task<IEnumerable<ContentTypeDto>> GetAllAsync();
    Task<ContentTypeDetailDto> GetByIdAsync(int id);
    Task<ContentTypeDto> GetBySlugAsync(string slug);
    Task<ContentTypeDto> CreateAsync(CreateContentTypeDto dto);
    Task<ContentTypeDto> UpdateAsync(int id, UpdateContentTypeDto dto);
    Task DeleteAsync(int id);
    Task<ContentTypeFieldDto> AddFieldAsync(int contentTypeId, CreateContentTypeFieldDto dto);
    Task<ContentTypeFieldDto> UpdateFieldAsync(int contentTypeId, int fieldId, UpdateContentTypeFieldDto dto);
    Task DeleteFieldAsync(int contentTypeId, int fieldId);
    Task ReorderFieldsAsync(int contentTypeId, List<int> fieldIds);
}
```

**الملف:** `Services/Implementations/ContentTypeService.cs`

```csharp
public class ContentTypeService : IContentTypeService
{
    private readonly IContentTypeRepository _repository;
    private readonly ITranslationService _translationService;
    private readonly IAuditLogService _auditLogService;

    public ContentTypeService(
        IContentTypeRepository repository,
        ITranslationService translationService,
        IAuditLogService auditLogService)
    {
        _repository = repository;
        _translationService = translationService;
        _auditLogService = auditLogService;
    }

    public async Task<IEnumerable<ContentTypeDto>> GetAllAsync()
    {
        var contentTypes = await _repository.GetAllWithContentCountAsync();
        return contentTypes.Select(ct => MapToDto(ct));
    }

    public async Task<ContentTypeDetailDto> GetByIdAsync(int id)
    {
        var contentType = await _repository.GetByIdWithFieldsAsync(id);
        if (contentType == null)
            throw new NotFoundException($"Content type with id {id} not found");

        return MapToDetailDto(contentType);
    }

    public async Task<ContentTypeDto> CreateAsync(CreateContentTypeDto dto)
    {
        // Validate slug uniqueness
        if (await _repository.SlugExistsAsync(dto.Slug))
            throw new BadRequestException($"Slug '{dto.Slug}' already exists");

        var contentType = new ContentType
        {
            Name = dto.Name,
            Slug = dto.Slug,
            Description = dto.Description,
            Icon = dto.Icon,
            IsSinglePage = dto.IsSinglePage,
            MetaTitleTemplate = dto.MetaTitleTemplate,
            MetaDescriptionTemplate = dto.MetaDescriptionTemplate
        };

        await _repository.CreateAsync(contentType);
        await _auditLogService.LogAsync(null, "Create", "ContentType", contentType.Id, $"Created content type: {contentType.Name}");

        return MapToDto(contentType);
    }

    // Implement other methods...

    private ContentTypeDto MapToDto(ContentType contentType)
    {
        return new ContentTypeDto
        {
            Id = contentType.Id,
            Name = contentType.Name,
            Slug = contentType.Slug,
            Description = contentType.Description,
            Icon = contentType.Icon,
            IsSinglePage = contentType.IsSinglePage,
            IsActive = contentType.IsActive,
            DisplayOrder = contentType.DisplayOrder,
            ContentCount = contentType.Contents?.Count ?? 0,
            CreatedAt = contentType.CreatedAt
        };
    }
}
```

### ✅ Checklist - Feature 1

- [ ] إنشاء Models: ContentType, ContentTypeField
- [ ] إنشاء Entity Configurations
- [ ] تشغيل Migration
- [ ] إنشاء Repository و Interface
- [ ] إنشاء Service و Interface
- [ ] إنشاء DTOs
- [ ] إنشاء Controller
- [ ] كتابة Unit Tests
- [ ] اختبار APIs في Swagger
- [ ] توثيق API

---

## Feature 2: نظام المحتوى الديناميكي (Dynamic Content)

### 📝 الوصف
نظام يسمح بإنشاء وإدارة محتوى لأي نوع محتوى تم إنشاؤه مسبقاً.

### 🗄️ Database Models

#### 2.1 Content Model

**الملف:** `Models/Entities/Content.cs`

```csharp
public class Content : TranslatableEntity
{
    public int ContentTypeId { get; set; }
    public ContentType ContentType { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [Required]
    [StringLength(200)]
    public string Slug { get; set; }

    public string? Summary { get; set; }

    public string? Body { get; set; } // Rich text content

    [StringLength(500)]
    public string? FeaturedImage { get; set; }

    public int? LayoutId { get; set; }
    public Layout? Layout { get; set; }

    // SEO
    [StringLength(200)]
    public string? MetaTitle { get; set; }

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    public string? MetaKeywords { get; set; }

    [StringLength(200)]
    public string? OgTitle { get; set; }

    [StringLength(500)]
    public string? OgDescription { get; set; }

    [StringLength(500)]
    public string? OgImage { get; set; }

    // Publishing
    public string Status { get; set; } = "Draft"; // Draft, Published, Scheduled, Archived

    public DateTime? PublishedAt { get; set; }

    public DateTime? ScheduledAt { get; set; }

    public int? AuthorId { get; set; }
    public User? Author { get; set; }

    // Stats
    public int ViewCount { get; set; } = 0;

    public bool IsFeatured { get; set; } = false;

    public bool AllowComments { get; set; } = true;

    // Navigation Properties
    public ICollection<ContentFieldValue> FieldValues { get; set; } = new List<ContentFieldValue>();
    public ICollection<ContentTag> Tags { get; set; } = new List<ContentTag>();
}
```

#### 2.2 ContentFieldValue Model

**الملف:** `Models/Entities/ContentFieldValue.cs`

```csharp
public class ContentFieldValue : BaseEntity
{
    public int ContentId { get; set; }
    public Content Content { get; set; }

    public int ContentTypeFieldId { get; set; }
    public ContentTypeField ContentTypeField { get; set; }

    [Required]
    [StringLength(100)]
    public string FieldKey { get; set; }

    public string? Value { get; set; } // Stores the actual value (text, date, JSON, etc.)

    public int? LanguageId { get; set; }
    public Language? Language { get; set; }
}
```

#### 2.3 Tag & ContentTag Models

**الملف:** `Models/Entities/Tag.cs`

```csharp
public class Tag : TranslatableEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Slug { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    // Navigation Properties
    public ICollection<ContentTag> ContentTags { get; set; } = new List<ContentTag>();
}

public class ContentTag : BaseEntity
{
    public int ContentId { get; set; }
    public Content Content { get; set; }

    public int TagId { get; set; }
    public Tag Tag { get; set; }
}
```

### 🎯 API Endpoints

#### 2.2 ContentsController

**الملف:** `Controllers/ContentsController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class ContentsController : ControllerBase
{
    private readonly IContentService _contentService;

    public ContentsController(IContentService contentService)
    {
        _contentService = contentService;
    }

    // GET: api/contents?contentTypeId=1&status=Published&page=1&pageSize=10
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<ContentListDto>>> GetAll([FromQuery] ContentFilterDto filter)
    {
        var contents = await _contentService.GetAllAsync(filter);
        return Ok(contents);
    }

    // GET: api/contents/{id}
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ContentDetailDto>> GetById(int id, [FromQuery] string? lang = "ar")
    {
        var content = await _contentService.GetByIdAsync(id, lang);
        return Ok(content);
    }

    // GET: api/contents/slug/{slug}?contentType=news&lang=ar
    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<ActionResult<ContentDetailDto>> GetBySlug(string slug, [FromQuery] string contentType, [FromQuery] string? lang = "ar")
    {
        var content = await _contentService.GetBySlugAsync(slug, contentType, lang);
        return Ok(content);
    }

    // POST: api/contents
    [HttpPost]
    [Authorize]
    [Permission(Permissions.ContentCreate)]
    public async Task<ActionResult<ContentDto>> Create([FromBody] CreateContentDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var content = await _contentService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = content.Id }, content);
    }

    // PUT: api/contents/{id}
    [HttpPut("{id}")]
    [Authorize]
    [Permission(Permissions.ContentEdit)]
    public async Task<ActionResult<ContentDto>> Update(int id, [FromBody] UpdateContentDto dto)
    {
        var content = await _contentService.UpdateAsync(id, dto);
        return Ok(content);
    }

    // DELETE: api/contents/{id}
    [HttpDelete("{id}")]
    [Authorize]
    [Permission(Permissions.ContentDelete)]
    public async Task<ActionResult> Delete(int id)
    {
        await _contentService.DeleteAsync(id);
        return NoContent();
    }

    // POST: api/contents/{id}/publish
    [HttpPost("{id}/publish")]
    [Authorize]
    [Permission(Permissions.ContentPublish)]
    public async Task<ActionResult> Publish(int id)
    {
        await _contentService.PublishAsync(id);
        return NoContent();
    }

    // POST: api/contents/{id}/unpublish
    [HttpPost("{id}/unpublish")]
    [Authorize]
    [Permission(Permissions.ContentPublish)]
    public async Task<ActionResult> Unpublish(int id)
    {
        await _contentService.UnpublishAsync(id);
        return NoContent();
    }

    // POST: api/contents/{id}/duplicate
    [HttpPost("{id}/duplicate")]
    [Authorize]
    [Permission(Permissions.ContentCreate)]
    public async Task<ActionResult<ContentDto>> Duplicate(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var content = await _contentService.DuplicateAsync(id, userId);
        return Ok(content);
    }

    // PUT: api/contents/{id}/move-to-type/{targetContentTypeId}
    [HttpPut("{id}/move-to-type/{targetContentTypeId}")]
    [Authorize]
    [Permission(Permissions.ContentEdit)]
    public async Task<ActionResult> MoveToContentType(int id, int targetContentTypeId)
    {
        await _contentService.MoveToContentTypeAsync(id, targetContentTypeId);
        return NoContent();
    }

    // POST: api/contents/{id}/increment-views
    [HttpPost("{id}/increment-views")]
    [AllowAnonymous]
    public async Task<ActionResult> IncrementViews(int id)
    {
        await _contentService.IncrementViewsAsync(id);
        return NoContent();
    }
}
```

### 📊 DTOs

**الملف:** `Models/DTOs/Content/ContentDto.cs`

```csharp
public class ContentListDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string? Summary { get; set; }
    public string? FeaturedImage { get; set; }
    public string Status { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; }
    public bool IsFeatured { get; set; }
    public string ContentTypeName { get; set; }
    public string? AuthorName { get; set; }
    public List<TagDto> Tags { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ContentDetailDto : ContentListDto
{
    public string? Body { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public Dictionary<string, object> CustomFields { get; set; } // Dynamic fields
    public LayoutDto? Layout { get; set; }
}

public class CreateContentDto
{
    [Required]
    public int ContentTypeId { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Slug { get; set; }

    public string? Summary { get; set; }
    public string? Body { get; set; }
    public string? FeaturedImage { get; set; }
    public int? LayoutId { get; set; }

    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; }

    // Publishing
    public string Status { get; set; } = "Draft";
    public DateTime? ScheduledAt { get; set; }
    public bool IsFeatured { get; set; } = false;
    public bool AllowComments { get; set; } = true;

    // Tags
    public List<int>? TagIds { get; set; }

    // Custom Fields
    public Dictionary<string, object>? CustomFields { get; set; }

    // Translations
    public Dictionary<string, ContentTranslationDto>? Translations { get; set; }
}

public class UpdateContentDto : CreateContentDto
{
}

public class ContentTranslationDto
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string? Summary { get; set; }
    public string? Body { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}

public class ContentFilterDto
{
    public int? ContentTypeId { get; set; }
    public string? Status { get; set; }
    public bool? IsFeatured { get; set; }
    public int? AuthorId { get; set; }
    public List<int>? TagIds { get; set; }
    public string? SearchTerm { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Language { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortOrder { get; set; } = "desc";
}

public class PagedResult<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}
```

### ✅ Checklist - Feature 2

- [ ] إنشاء Models: Content, ContentFieldValue, Tag, ContentTag
- [ ] إنشاء Entity Configurations
- [ ] تشغيل Migration
- [ ] إنشاء Repository و Interface
- [ ] إنشاء Service و Interface
- [ ] إنشاء DTOs
- [ ] إنشاء Controller
- [ ] تطبيق Pagination
- [ ] تطبيق Filtering & Search
- [ ] كتابة Unit Tests
- [ ] اختبار APIs

---

## Feature 3: نظام التخطيطات (Layouts)

### 📝 الوصف
نظام يسمح بإنشاء تخطيطات مختلفة لعرض المحتوى.

### 🗄️ Database Models

#### 3.1 Layout Model

**الملف:** `Models/Entities/Layout.cs`

```csharp
public class Layout : TranslatableEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } // e.g., "Default Article", "Featured News"

    [StringLength(500)]
    public string? Description { get; set; }

    public string Configuration { get; set; } // JSON configuration

    public bool IsDefault { get; set; } = false;

    public bool IsActive { get; set; } = true;

    [StringLength(500)]
    public string? PreviewImage { get; set; }

    // Navigation Properties
    public ICollection<Content> Contents { get; set; } = new List<Content>();
}
```

### 🎯 API Endpoints

**الملف:** `Controllers/LayoutsController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LayoutsController : ControllerBase
{
    private readonly ILayoutService _layoutService;

    public LayoutsController(ILayoutService layoutService)
    {
        _layoutService = layoutService;
    }

    // GET: api/layouts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LayoutDto>>> GetAll()
    {
        var layouts = await _layoutService.GetAllAsync();
        return Ok(layouts);
    }

    // GET: api/layouts/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<LayoutDetailDto>> GetById(int id)
    {
        var layout = await _layoutService.GetByIdAsync(id);
        return Ok(layout);
    }

    // POST: api/layouts
    [HttpPost]
    [Permission(Permissions.ContentTypesEdit)]
    public async Task<ActionResult<LayoutDto>> Create([FromBody] CreateLayoutDto dto)
    {
        var layout = await _layoutService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = layout.Id }, layout);
    }

    // PUT: api/layouts/{id}
    [HttpPut("{id}")]
    [Permission(Permissions.ContentTypesEdit)]
    public async Task<ActionResult<LayoutDto>> Update(int id, [FromBody] UpdateLayoutDto dto)
    {
        var layout = await _layoutService.UpdateAsync(id, dto);
        return Ok(layout);
    }

    // DELETE: api/layouts/{id}
    [HttpDelete("{id}")]
    [Permission(Permissions.ContentTypesEdit)]
    public async Task<ActionResult> Delete(int id)
    {
        await _layoutService.DeleteAsync(id);
        return NoContent();
    }

    // POST: api/layouts/{id}/set-default
    [HttpPost("{id}/set-default")]
    [Permission(Permissions.ContentTypesEdit)]
    public async Task<ActionResult> SetAsDefault(int id)
    {
        await _layoutService.SetAsDefaultAsync(id);
        return NoContent();
    }
}
```

### ✅ Checklist - Feature 3

- [ ] إنشاء Layout Model
- [ ] إنشاء Entity Configuration
- [ ] تشغيل Migration
- [ ] إنشاء Repository و Interface
- [ ] إنشاء Service و Interface
- [ ] إنشاء DTOs
- [ ] إنشاء Controller
- [ ] كتابة Unit Tests

---

## Feature 4: نظام رفع الملفات والصور

### 📝 الوصف
نظام متقدم لرفع ومعالجة الملفات والصور مع تحويل تلقائي لـ WebP.

### 🗄️ Database Models

#### 4.1 Media Model

**الملف:** `Models/Entities/Media.cs`

```csharp
public class Media : BaseEntity
{
    [Required]
    [StringLength(200)]
    public string FileName { get; set; }

    [Required]
    [StringLength(500)]
    public string FilePath { get; set; }

    [StringLength(500)]
    public string? ThumbnailPath { get; set; }

    [StringLength(500)]
    public string? WebPPath { get; set; } // WebP version

    [Required]
    [StringLength(100)]
    public string MimeType { get; set; }

    public long FileSize { get; set; } // in bytes

    public long? WebPFileSize { get; set; }

    public int? Width { get; set; }

    public int? Height { get; set; }

    [StringLength(200)]
    public string? Alt { get; set; }

    [StringLength(500)]
    public string? Caption { get; set; }

    [StringLength(100)]
    public string MediaType { get; set; } // Image, Video, Document, Audio

    public int? UploadedBy { get; set; }
    public User? Uploader { get; set; }

    public bool IsProcessed { get; set; } = false;

    public DateTime? ProcessedAt { get; set; }
}
```

### 🎯 API Endpoints

**الملف:** `Controllers/MediaController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    // POST: api/media/upload
    [HttpPost("upload")]
    public async Task<ActionResult<MediaDto>> Upload([FromForm] IFormFile file, [FromForm] string? alt = null)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var media = await _mediaService.UploadAsync(file, userId, alt);
        return Ok(media);
    }

    // POST: api/media/upload-multiple
    [HttpPost("upload-multiple")]
    public async Task<ActionResult<List<MediaDto>>> UploadMultiple([FromForm] List<IFormFile> files)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var mediaList = await _mediaService.UploadMultipleAsync(files, userId);
        return Ok(mediaList);
    }

    // GET: api/media?type=Image&page=1&pageSize=20
    [HttpGet]
    public async Task<ActionResult<PagedResult<MediaDto>>> GetAll([FromQuery] MediaFilterDto filter)
    {
        var media = await _mediaService.GetAllAsync(filter);
        return Ok(media);
    }

    // GET: api/media/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<MediaDto>> GetById(int id)
    {
        var media = await _mediaService.GetByIdAsync(id);
        return Ok(media);
    }

    // PUT: api/media/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<MediaDto>> Update(int id, [FromBody] UpdateMediaDto dto)
    {
        var media = await _mediaService.UpdateAsync(id, dto);
        return Ok(media);
    }

    // DELETE: api/media/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _mediaService.DeleteAsync(id);
        return NoContent();
    }

    // POST: api/media/{id}/regenerate-webp
    [HttpPost("{id}/regenerate-webp")]
    public async Task<ActionResult> RegenerateWebP(int id)
    {
        await _mediaService.RegenerateWebPAsync(id);
        return NoContent();
    }
}
```

### 🔧 Services

**الملف:** `Services/Implementations/MediaService.cs`

```csharp
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;

public class MediaService : IMediaService
{
    private readonly IMediaRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly string _uploadPath;

    public MediaService(IMediaRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
        _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        if (!Directory.Exists(_uploadPath))
            Directory.CreateDirectory(_uploadPath);
    }

    public async Task<MediaDto> UploadAsync(IFormFile file, int userId, string? alt = null)
    {
        // Validate file
        if (file == null || file.Length == 0)
            throw new BadRequestException("No file uploaded");

        // Generate unique filename
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(_uploadPath, fileName);

        // Save original file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Get file info
        var fileInfo = new FileInfo(filePath);
        var media = new Media
        {
            FileName = file.FileName,
            FilePath = $"/uploads/{fileName}",
            MimeType = file.ContentType,
            FileSize = fileInfo.Length,
            MediaType = GetMediaType(file.ContentType),
            UploadedBy = userId,
            Alt = alt ?? file.FileName
        };

        // Process if image
        if (media.MediaType == "Image")
        {
            await ProcessImageAsync(media, filePath);
        }

        await _repository.CreateAsync(media);

        return MapToDto(media);
    }

    private async Task ProcessImageAsync(Media media, string filePath)
    {
        using var image = await Image.LoadAsync(filePath);

        // Get dimensions
        media.Width = image.Width;
        media.Height = image.Height;

        // Generate thumbnail
        var thumbnailFileName = $"thumb_{Path.GetFileName(filePath)}";
        var thumbnailPath = Path.Combine(_uploadPath, thumbnailFileName);
        
        using (var thumbnail = image.Clone(ctx => ctx.Resize(new ResizeOptions
        {
            Size = new Size(300, 300),
            Mode = ResizeMode.Max
        })))
        {
            await thumbnail.SaveAsync(thumbnailPath);
        }
        media.ThumbnailPath = $"/uploads/{thumbnailFileName}";

        // Generate WebP version
        var webpFileName = $"{Path.GetFileNameWithoutExtension(filePath)}.webp";
        var webpPath = Path.Combine(_uploadPath, webpFileName);
        
        var encoder = new WebpEncoder { Quality = 85 };
        await image.SaveAsync(webpPath, encoder);

        var webpInfo = new FileInfo(webpPath);
        media.WebPPath = $"/uploads/{webpFileName}";
        media.WebPFileSize = webpInfo.Length;
        media.IsProcessed = true;
        media.ProcessedAt = DateTime.UtcNow;
    }

    private string GetMediaType(string mimeType)
    {
        if (mimeType.StartsWith("image/")) return "Image";
        if (mimeType.StartsWith("video/")) return "Video";
        if (mimeType.StartsWith("audio/")) return "Audio";
        return "Document";
    }

    // Implement other methods...
}
```

### ✅ Checklist - Feature 4

- [ ] إنشاء Media Model
- [ ] إنشاء Entity Configuration
- [ ] تشغيل Migration
- [ ] إنشاء Repository و Interface
- [ ] إنشاء Service مع معالجة الصور
- [ ] تطبيق WebP Conversion
- [ ] إنشاء Thumbnail Generator
- [ ] إنشاء DTOs
- [ ] إنشاء Controller
- [ ] كتابة Unit Tests
- [ ] اختبار رفع الملفات

---

## Feature 5: نظام الألبومات (Albums)

### 📝 الوصف
نظام لإنشاء ألبومات صور مع رفع متعدد وعرض ذكي.

### 🗄️ Database Models

#### 5.1 Album Model

**الملف:** `Models/Entities/Album.cs`

```csharp
public class Album : TranslatableEntity
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [Required]
    [StringLength(200)]
    public string Slug { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    public int? CoverImageId { get; set; }
    public Media? CoverImage { get; set; }

    public bool IsPublished { get; set; } = true;

    public DateTime? PublishedAt { get; set; }

    public int ViewCount { get; set; } = 0;

    public int? CreatedBy { get; set; }
    public User? Creator { get; set; }

    // Navigation Properties
    public ICollection<AlbumMedia> AlbumMedias { get; set; } = new List<AlbumMedia>();
}
```

#### 5.2 AlbumMedia Model

**الملف:** `Models/Entities/AlbumMedia.cs`

```csharp
public class AlbumMedia : BaseEntity
{
    public int AlbumId { get; set; }
    public Album Album { get; set; }

    public int MediaId { get; set; }
    public Media Media { get; set; }

    public int DisplayOrder { get; set; }

    [StringLength(200)]
    public string? Caption { get; set; }

    public bool IsFeatured { get; set; } = false;
}
```

### 🎯 API Endpoints

**الملف:** `Controllers/AlbumsController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class AlbumsController : ControllerBase
{
    private readonly IAlbumService _albumService;

    public AlbumsController(IAlbumService albumService)
    {
        _albumService = albumService;
    }

    // GET: api/albums
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<AlbumDto>>> GetAll([FromQuery] AlbumFilterDto filter)
    {
        var albums = await _albumService.GetAllAsync(filter);
        return Ok(albums);
    }

    // GET: api/albums/{id}
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<AlbumDetailDto>> GetById(int id, [FromQuery] string? lang = "ar")
    {
        var album = await _albumService.GetByIdAsync(id, lang);
        return Ok(album);
    }

    // GET: api/albums/slug/{slug}
    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<ActionResult<AlbumDetailDto>> GetBySlug(string slug, [FromQuery] string? lang = "ar")
    {
        var album = await _albumService.GetBySlugAsync(slug, lang);
        return Ok(album);
    }

    // POST: api/albums
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<AlbumDto>> Create([FromBody] CreateAlbumDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var album = await _albumService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = album.Id }, album);
    }

    // PUT: api/albums/{id}
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<AlbumDto>> Update(int id, [FromBody] UpdateAlbumDto dto)
    {
        var album = await _albumService.UpdateAsync(id, dto);
        return Ok(album);
    }

    // DELETE: api/albums/{id}
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        await _albumService.DeleteAsync(id);
        return NoContent();
    }

    // POST: api/albums/{id}/media
    [HttpPost("{id}/media")]
    [Authorize]
    public async Task<ActionResult> AddMedia(int id, [FromBody] AddMediaToAlbumDto dto)
    {
        await _albumService.AddMediaAsync(id, dto);
        return NoContent();
    }

    // DELETE: api/albums/{id}/media/{mediaId}
    [HttpDelete("{id}/media/{mediaId}")]
    [Authorize]
    public async Task<ActionResult> RemoveMedia(int id, int mediaId)
    {
        await _albumService.RemoveMediaAsync(id, mediaId);
        return NoContent();
    }

    // POST: api/albums/{id}/reorder-media
    [HttpPost("{id}/reorder-media")]
    [Authorize]
    public async Task<ActionResult> ReorderMedia(int id, [FromBody] ReorderMediaDto dto)
    {
        await _albumService.ReorderMediaAsync(id, dto.MediaIds);
        return NoContent();
    }

    // POST: api/albums/{id}/increment-views
    [HttpPost("{id}/increment-views")]
    [AllowAnonymous]
    public async Task<ActionResult> IncrementViews(int id)
    {
        await _albumService.IncrementViewsAsync(id);
        return NoContent();
    }
}
```

### ✅ Checklist - Feature 5

- [ ] إنشاء Models: Album, AlbumMedia
- [ ] إنشاء Entity Configurations
- [ ] تشغيل Migration
- [ ] إنشاء Repository و Interface
- [ ] إنشاء Service و Interface
- [ ] إنشاء DTOs
- [ ] إنشاء Controller
- [ ] تطبيق Dynamic Collage Algorithm
- [ ] كتابة Unit Tests
- [ ] اختبار APIs

---

## 📦 NuGet Packages الإضافية للمطور 1

```xml
<PackageReference Include="SixLabors.ImageSharp" Version="3.1.0" />
<PackageReference Include="SixLabors.ImageSharp.Web" Version="3.1.0" />
```

---

## ✅ Checklist شامل للمطور 1

### Week 1-2: Content Types & Content
- [ ] Feature 1: Content Types (كامل)
- [ ] Feature 2: Content Management (كامل)
- [ ] Migration & Database Setup
- [ ] Basic API Testing

### Week 3: Layouts & Media
- [ ] Feature 3: Layouts System
- [ ] Feature 4: Media Upload (بداية)
- [ ] WebP Conversion Implementation

### Week 4-5: Media & Albums
- [ ] Feature 4: Media Management (إكمال)
- [ ] Feature 5: Albums System (كامل)
- [ ] Advanced Image Processing

### Week 6: Testing & Documentation
- [ ] Unit Tests لجميع Features
- [ ] Integration Tests
- [ ] API Documentation (Swagger)
- [ ] Code Review & Refactoring

---

## 🎯 الأولويات

**Priority 1 (أسبوع 1-2):**
- Content Types
- Content Management

**Priority 2 (أسبوع 3-4):**
- Layouts
- Media Upload & Processing

**Priority 3 (أسبوع 5):**
- Albums System

**Priority 4 (أسبوع 6):**
- Testing & Documentation

---

**تاريخ الإنشاء:** 11 نوفمبر 2025  
**آخر تحديث:** 11 نوفمبر 2025  
**الحالة:** 📝 جاهز للتنفيذ
