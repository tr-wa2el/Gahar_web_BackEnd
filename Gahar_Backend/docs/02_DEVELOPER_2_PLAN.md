# 👨‍💻 خطة تطوير المطور الثاني (Developer 2)
## Features Package B - Page Builder, Forms & Advanced Features

**تاريخ الإنشاء:** 11 نوفمبر 2025  
**المدة المتوقعة:** 4-6 أسابيع  
**التبعيات:** يجب إكمال الجزء المشترك أولاً

---

## 📋 جدول المحتويات
1. [نظرة عامة](#نظرة-عامة)
2. [Feature 1: Page Builder](#feature-1-page-builder)
3. [Feature 2: Form Builder](#feature-2-form-builder)
4. [Feature 3: Navigation Menus with Icons](#feature-3-navigation-menus-with-icons)
5. [Feature 4: Facilities & Certificates Management](#feature-4-facilities--certificates-management)
6. [Feature 5: SEO & Analytics](#feature-5-seo--analytics)

---

## نظرة عامة

### 🎯 الهدف
تطوير مكونات بناء الصفحات والنماذج، وإدارة المنشآت والشهادات، ونظام SEO المتقدم.

### 📦 الـ Features المسؤول عنها
1. ✅ Page Builder (بناء الصفحات بالسحب والإفلات)
2. ✅ Form Builder (بناء النماذج المتقدمة)
3. ✅ Navigation Menus with Icons (قوائم التنقل مع الأيقونات)
4. ✅ Facilities & Certificates Management (إدارة المنشآت والشهادات)
5. ✅ SEO & Analytics (تحسين محركات البحث والإحصائيات)

### 🔗 التكامل مع المطور الأول
- يستخدم نظام Media المطور من Developer 1
- لا توجد تبعيات مباشرة أخرى
- العمل مستقل تماماً

---

## Feature 1: Page Builder

### 📝 الوصف
نظام بناء صفحات متقدم بالسحب والإفلات مع مكونات (Blocks) جاهزة.

### 🗄️ Database Models

#### 1.1 Page Model

**الملف:** `Models/Entities/Page.cs`

```csharp
public class Page : TranslatableEntity
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [Required]
    [StringLength(200)]
    public string Slug { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

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
    public bool IsPublished { get; set; } = false;

    public DateTime? PublishedAt { get; set; }

    public int? AuthorId { get; set; }
    public User? Author { get; set; }

    // Template
    [StringLength(50)]
    public string Template { get; set; } = "Default"; // Default, FullWidth, Sidebar

    public bool ShowTitle { get; set; } = true;

    public bool ShowBreadcrumbs { get; set; } = true;

    // Navigation Properties
    public ICollection<PageBlock> Blocks { get; set; } = new List<PageBlock>();
}
```

#### 1.2 PageBlock Model

**الملف:** `Models/Entities/PageBlock.cs`

```csharp
public class PageBlock : BaseEntity
{
    public int PageId { get; set; }
    public Page Page { get; set; }

    [Required]
    [StringLength(50)]
    public string BlockType { get; set; } // HeroSection, TextBlock, ImageGallery, CTA, etc.

    [Required]
    public string Configuration { get; set; } // JSON configuration

    public int DisplayOrder { get; set; }

    public bool IsVisible { get; set; } = true;

    [StringLength(100)]
    public string? CssClass { get; set; }

    [StringLength(100)]
    public string? CustomId { get; set; }
}
```

#### 1.3 BlockType Definition (في كود)

**الملف:** `Constants/BlockTypes.cs`

```csharp
public static class BlockTypes
{
    public const string HeroSection = "HeroSection";
    public const string TextBlock = "TextBlock";
    public const string ImageGallery = "ImageGallery";
    public const string CtaButton = "CtaButton";
    public const string StatsCounter = "StatsCounter";
    public const string TeamGrid = "TeamGrid";
    public const string FaqAccordion = "FaqAccordion";
    public const string ContactForm = "ContactForm";
    public const string EmbeddedVideo = "EmbeddedVideo";
    public const string Timeline = "Timeline";
    public const string CustomHtml = "CustomHtml";
    public const string ContentList = "ContentList"; // Display list of contents
    public const string LatestNews = "LatestNews";
    public const string FeaturedContent = "FeaturedContent";
}
```

### 🎯 API Endpoints

#### 1.3 PagesController

**الملف:** `Controllers/PagesController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class PagesController : ControllerBase
{
    private readonly IPageService _pageService;

    public PagesController(IPageService pageService)
    {
        _pageService = pageService;
    }

    // GET: api/pages
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<PageListDto>>> GetAll([FromQuery] PageFilterDto filter)
    {
        var pages = await _pageService.GetAllAsync(filter);
        return Ok(pages);
    }

    // GET: api/pages/{id}
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<PageDetailDto>> GetById(int id, [FromQuery] string? lang = "ar")
    {
        var page = await _pageService.GetByIdAsync(id, lang);
        return Ok(page);
    }

    // GET: api/pages/slug/{slug}
    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<ActionResult<PageDetailDto>> GetBySlug(string slug, [FromQuery] string? lang = "ar")
    {
        var page = await _pageService.GetBySlugAsync(slug, lang);
        return Ok(page);
    }

    // POST: api/pages
    [HttpPost]
    [Authorize]
    [Permission(Permissions.PagesCreate)]
    public async Task<ActionResult<PageDto>> Create([FromBody] CreatePageDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var page = await _pageService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = page.Id }, page);
    }

    // PUT: api/pages/{id}
    [HttpPut("{id}")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    public async Task<ActionResult<PageDto>> Update(int id, [FromBody] UpdatePageDto dto)
    {
        var page = await _pageService.UpdateAsync(id, dto);
        return Ok(page);
    }

    // DELETE: api/pages/{id}
    [HttpDelete("{id}")]
    [Authorize]
    [Permission(Permissions.PagesDelete)]
    public async Task<ActionResult> Delete(int id)
    {
        await _pageService.DeleteAsync(id);
        return NoContent();
    }

    // POST: api/pages/{id}/publish
    [HttpPost("{id}/publish")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    public async Task<ActionResult> Publish(int id)
    {
        await _pageService.PublishAsync(id);
        return NoContent();
    }

    // POST: api/pages/{id}/unpublish
    [HttpPost("{id}/unpublish")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    public async Task<ActionResult> Unpublish(int id)
    {
        await _pageService.UnpublishAsync(id);
        return NoContent();
    }

    // POST: api/pages/{id}/blocks
    [HttpPost("{id}/blocks")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    public async Task<ActionResult<PageBlockDto>> AddBlock(int id, [FromBody] CreatePageBlockDto dto)
    {
        var block = await _pageService.AddBlockAsync(id, dto);
        return Ok(block);
    }

    // PUT: api/pages/{id}/blocks/{blockId}
    [HttpPut("{id}/blocks/{blockId}")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    public async Task<ActionResult<PageBlockDto>> UpdateBlock(int id, int blockId, [FromBody] UpdatePageBlockDto dto)
    {
        var block = await _pageService.UpdateBlockAsync(id, blockId, dto);
        return Ok(block);
    }

    // DELETE: api/pages/{id}/blocks/{blockId}
    [HttpDelete("{id}/blocks/{blockId}")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    public async Task<ActionResult> DeleteBlock(int id, int blockId)
    {
        await _pageService.DeleteBlockAsync(id, blockId);
        return NoContent();
    }

    // POST: api/pages/{id}/reorder-blocks
    [HttpPost("{id}/reorder-blocks")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    public async Task<ActionResult> ReorderBlocks(int id, [FromBody] ReorderBlocksDto dto)
    {
        await _pageService.ReorderBlocksAsync(id, dto.BlockIds);
        return NoContent();
    }

    // POST: api/pages/{id}/duplicate
    [HttpPost("{id}/duplicate")]
    [Authorize]
    [Permission(Permissions.PagesCreate)]
    public async Task<ActionResult<PageDto>> Duplicate(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var page = await _pageService.DuplicateAsync(id, userId);
        return Ok(page);
    }
}
```

### 📊 DTOs

**الملف:** `Models/DTOs/Page/PageDto.cs`

```csharp
public class PageListDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string? Description { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? AuthorName { get; set; }
    public int BlockCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PageDetailDto : PageListDto
{
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
    public string Template { get; set; }
    public bool ShowTitle { get; set; }
    public bool ShowBreadcrumbs { get; set; }
    public List<PageBlockDto> Blocks { get; set; }
}

public class PageBlockDto
{
    public int Id { get; set; }
    public string BlockType { get; set; }
    public object Configuration { get; set; } // Dynamic JSON
    public int DisplayOrder { get; set; }
    public bool IsVisible { get; set; }
    public string? CssClass { get; set; }
    public string? CustomId { get; set; }
}

public class CreatePageDto
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Slug { get; set; }

    public string? Description { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
    public string Template { get; set; } = "Default";
    public bool ShowTitle { get; set; } = true;
    public bool ShowBreadcrumbs { get; set; } = true;
    public Dictionary<string, PageTranslationDto>? Translations { get; set; }
}

public class UpdatePageDto : CreatePageDto
{
    public bool IsPublished { get; set; }
}

public class CreatePageBlockDto
{
    [Required]
    public string BlockType { get; set; }

    [Required]
    public object Configuration { get; set; }

    public bool IsVisible { get; set; } = true;
    public string? CssClass { get; set; }
    public string? CustomId { get; set; }
}

public class UpdatePageBlockDto : CreatePageBlockDto
{
    public int DisplayOrder { get; set; }
}

public class ReorderBlocksDto
{
    [Required]
    public List<int> BlockIds { get; set; }
}

public class PageTranslationDto
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string? Description { get; set; }
}
```

### 🎨 Block Configuration Examples

#### Hero Section Configuration:
```json
{
  "backgroundImage": "/uploads/hero-bg.jpg",
  "heading": "Welcome to Gahar",
  "subheading": "Saudi Commission for Health Specialties",
  "ctaText": "Learn More",
  "ctaLink": "/about",
  "textAlign": "center",
  "overlay": true,
  "overlayOpacity": 0.5
}
```

#### Stats Counter Configuration:
```json
{
  "items": [
    {
      "value": 500,
      "suffix": "+",
      "label": "Accredited Facilities",
      "icon": "Building"
    },
    {
      "value": 1000,
      "suffix": "+",
      "label": "Issued Certificates",
      "icon": "Award"
    }
  ],
  "animationDuration": 2000
}
```

### ✅ Checklist - Feature 1

- [ ] إنشاء Models: Page, PageBlock
- [ ] إنشاء Entity Configurations
- [ ] تشغيل Migration
- [ ] إنشاء Repository و Interface
- [ ] إنشاء Service و Interface
- [ ] إنشاء DTOs
- [ ] إنشاء Controller
- [ ] تطبيق Block Configurations
- [ ] كتابة Unit Tests
- [ ] اختبار Page Builder

---

## Feature 2: Form Builder

### 📝 الوصف
نظام متقدم لبناء نماذج مخصصة مع قواعد التحقق والمنطق الشرطي.

### 🗄️ Database Models

#### 2.1 Form Model

**الملف:** `Models/Entities/Form.cs`

```csharp
public class Form : TranslatableEntity
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Slug { get; set; }

    public bool IsActive { get; set; } = true;

    public bool RequireAuth { get; set; } = false;

    public bool AllowMultipleSubmissions { get; set; } = true;

    [StringLength(500)]
    public string? SuccessMessage { get; set; }

    [StringLength(500)]
    public string? SuccessRedirectUrl { get; set; }

    [StringLength(500)]
    public string? NotificationEmail { get; set; } // Send submissions to this email

    public bool SaveSubmissions { get; set; } = true;

    // Navigation Properties
    public ICollection<FormField> Fields { get; set; } = new List<FormField>();
    public ICollection<FormSubmission> Submissions { get; set; } = new List<FormSubmission>();
}
```

#### 2.2 FormField Model

**الملف:** `Models/Entities/FormField.cs`

```csharp
public class FormField : TranslatableEntity
{
    public int FormId { get; set; }
    public Form Form { get; set; }

    [Required]
    [StringLength(100)]
    public string Label { get; set; }

    [Required]
    [StringLength(100)]
    public string FieldKey { get; set; } // Unique key for this field

    [Required]
    [StringLength(50)]
    public string FieldType { get; set; } // Text, Email, Number, Date, Textarea, Dropdown, Checkbox, Radio, FileUpload

    [StringLength(500)]
    public string? Placeholder { get; set; }

    [StringLength(1000)]
    public string? HelpText { get; set; }

    public string? DefaultValue { get; set; }

    public bool IsRequired { get; set; } = false;

    public int DisplayOrder { get; set; }

    // Validation Rules
    public string? ValidationRules { get; set; } // JSON

    // For Dropdown, Radio, Checkbox
    public string? Options { get; set; } // JSON array

    // Conditional Logic
    public string? ConditionalLogic { get; set; } // JSON: Show/hide based on other field values

    // Styling
    [StringLength(50)]
    public string? Width { get; set; } = "full"; // full, half, third

    [StringLength(100)]
    public string? CssClass { get; set; }
}
```

#### 2.3 FormSubmission Model

**الملف:** `Models/Entities/FormSubmission.cs`

```csharp
public class FormSubmission : BaseEntity
{
    public int FormId { get; set; }
    public Form Form { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    [Required]
    public string FormData { get; set; } // JSON of all field values

    [StringLength(45)]
    public string? IpAddress { get; set; }

    [StringLength(500)]
    public string? UserAgent { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Reviewed, Approved, Rejected

    public DateTime? ReviewedAt { get; set; }

    public int? ReviewedBy { get; set; }
    public User? Reviewer { get; set; }

    [StringLength(1000)]
    public string? ReviewNotes { get; set; }
}
```

### 🎯 API Endpoints

#### 2.4 FormsController

**الملف:** `Controllers/FormsController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class FormsController : ControllerBase
{
    private readonly IFormService _formService;

    public FormsController(IFormService formService)
    {
        _formService = formService;
    }

    // GET: api/forms
    [HttpGet]
    [Authorize]
    [Permission(Permissions.FormsView)]
    public async Task<ActionResult<IEnumerable<FormDto>>> GetAll()
    {
        var forms = await _formService.GetAllAsync();
        return Ok(forms);
    }

    // GET: api/forms/{id}
    [HttpGet("{id}")]
    [Authorize]
    [Permission(Permissions.FormsView)]
    public async Task<ActionResult<FormDetailDto>> GetById(int id)
    {
        var form = await _formService.GetByIdAsync(id);
        return Ok(form);
    }

    // GET: api/forms/slug/{slug}
    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<ActionResult<FormDetailDto>> GetBySlug(string slug, [FromQuery] string? lang = "ar")
    {
        var form = await _formService.GetBySlugAsync(slug, lang);
        return Ok(form);
    }

    // POST: api/forms
    [HttpPost]
    [Authorize]
    [Permission(Permissions.FormsCreate)]
    public async Task<ActionResult<FormDto>> Create([FromBody] CreateFormDto dto)
    {
        var form = await _formService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = form.Id }, form);
    }

    // PUT: api/forms/{id}
    [HttpPut("{id}")]
    [Authorize]
    [Permission(Permissions.FormsEdit)]
    public async Task<ActionResult<FormDto>> Update(int id, [FromBody] UpdateFormDto dto)
    {
        var form = await _formService.UpdateAsync(id, dto);
        return Ok(form);
    }

    // DELETE: api/forms/{id}
    [HttpDelete("{id}")]
    [Authorize]
    [Permission(Permissions.FormsDelete)]
    public async Task<ActionResult> Delete(int id)
    {
        await _formService.DeleteAsync(id);
        return NoContent();
    }

    // POST: api/forms/{id}/fields
    [HttpPost("{id}/fields")]
    [Authorize]
    [Permission(Permissions.FormsEdit)]
    public async Task<ActionResult<FormFieldDto>> AddField(int id, [FromBody] CreateFormFieldDto dto)
    {
        var field = await _formService.AddFieldAsync(id, dto);
        return Ok(field);
    }

    // PUT: api/forms/{id}/fields/{fieldId}
    [HttpPut("{id}/fields/{fieldId}")]
    [Authorize]
    [Permission(Permissions.FormsEdit)]
    public async Task<ActionResult<FormFieldDto>> UpdateField(int id, int fieldId, [FromBody] UpdateFormFieldDto dto)
    {
        var field = await _formService.UpdateFieldAsync(id, fieldId, dto);
        return Ok(field);
    }

    // DELETE: api/forms/{id}/fields/{fieldId}
    [HttpDelete("{id}/fields/{fieldId}")]
    [Authorize]
    [Permission(Permissions.FormsEdit)]
    public async Task<ActionResult> DeleteField(int id, int fieldId)
    {
        await _formService.DeleteFieldAsync(id, fieldId);
        return NoContent();
    }

    // POST: api/forms/{id}/reorder-fields
    [HttpPost("{id}/reorder-fields")]
    [Authorize]
    [Permission(Permissions.FormsEdit)]
    public async Task<ActionResult> ReorderFields(int id, [FromBody] ReorderFieldsDto dto)
    {
        await _formService.ReorderFieldsAsync(id, dto.FieldIds);
        return NoContent();
    }

    // POST: api/forms/{id}/submit
    [HttpPost("{id}/submit")]
    public async Task<ActionResult<FormSubmissionResponseDto>> Submit(int id, [FromBody] SubmitFormDto dto)
    {
        var userId = User.Identity?.IsAuthenticated == true
            ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
            : (int?)null;

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = Request.Headers["User-Agent"].ToString();

        var response = await _formService.SubmitAsync(id, dto, userId, ipAddress, userAgent);
        return Ok(response);
    }

    // GET: api/forms/{id}/submissions
    [HttpGet("{id}/submissions")]
    [Authorize]
    [Permission(Permissions.FormsView)]
    public async Task<ActionResult<PagedResult<FormSubmissionDto>>> GetSubmissions(
        int id,
        [FromQuery] FormSubmissionFilterDto filter)
    {
        var submissions = await _formService.GetSubmissionsAsync(id, filter);
        return Ok(submissions);
    }

    // GET: api/forms/{id}/submissions/{submissionId}
    [HttpGet("{id}/submissions/{submissionId}")]
    [Authorize]
    [Permission(Permissions.FormsView)]
    public async Task<ActionResult<FormSubmissionDetailDto>> GetSubmission(int id, int submissionId)
    {
        var submission = await _formService.GetSubmissionByIdAsync(id, submissionId);
        return Ok(submission);
    }

    // PUT: api/forms/{id}/submissions/{submissionId}/status
    [HttpPut("{id}/submissions/{submissionId}/status")]
    [Authorize]
    [Permission(Permissions.FormsEdit)]
    public async Task<ActionResult> UpdateSubmissionStatus(
        int id,
        int submissionId,
        [FromBody] UpdateSubmissionStatusDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _formService.UpdateSubmissionStatusAsync(submissionId, dto, userId);
        return NoContent();
    }

    // GET: api/forms/{id}/export-submissions
    [HttpGet("{id}/export-submissions")]
    [Authorize]
    [Permission(Permissions.FormsView)]
    public async Task<IActionResult> ExportSubmissions(int id, [FromQuery] string format = "csv")
    {
        var fileBytes = await _formService.ExportSubmissionsAsync(id, format);
        var fileName = $"form_{id}_submissions_{DateTime.UtcNow:yyyyMMdd}.{format}";
        var contentType = format == "csv" ? "text/csv" : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        
        return File(fileBytes, contentType, fileName);
    }
}
```

### 📊 DTOs

**الملف:** `Models/DTOs/Form/FormDto.cs`

```csharp
public class FormDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Slug { get; set; }
    public bool IsActive { get; set; }
    public int SubmissionCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class FormDetailDto : FormDto
{
    public bool RequireAuth { get; set; }
    public bool AllowMultipleSubmissions { get; set; }
    public string? SuccessMessage { get; set; }
    public string? SuccessRedirectUrl { get; set; }
    public string? NotificationEmail { get; set; }
    public bool SaveSubmissions { get; set; }
    public List<FormFieldDto> Fields { get; set; }
}

public class FormFieldDto
{
    public int Id { get; set; }
    public string Label { get; set; }
    public string FieldKey { get; set; }
    public string FieldType { get; set; }
    public string? Placeholder { get; set; }
    public string? HelpText { get; set; }
    public string? DefaultValue { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public object? ValidationRules { get; set; }
    public List<string>? Options { get; set; }
    public object? ConditionalLogic { get; set; }
    public string? Width { get; set; }
    public string? CssClass { get; set; }
}

public class CreateFormDto
{
    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public string Slug { get; set; }

    public bool RequireAuth { get; set; } = false;
    public bool AllowMultipleSubmissions { get; set; } = true;
    public string? SuccessMessage { get; set; }
    public string? SuccessRedirectUrl { get; set; }
    public string? NotificationEmail { get; set; }
    public bool SaveSubmissions { get; set; } = true;
}

public class UpdateFormDto : CreateFormDto
{
    public bool IsActive { get; set; } = true;
}

public class CreateFormFieldDto
{
    [Required]
    public string Label { get; set; }

    [Required]
    public string FieldKey { get; set; }

    [Required]
    public string FieldType { get; set; }

    public string? Placeholder { get; set; }
    public string? HelpText { get; set; }
    public string? DefaultValue { get; set; }
    public bool IsRequired { get; set; } = false;
    public object? ValidationRules { get; set; }
    public List<string>? Options { get; set; }
    public object? ConditionalLogic { get; set; }
    public string? Width { get; set; } = "full";
    public string? CssClass { get; set; }
}

public class UpdateFormFieldDto : CreateFormFieldDto
{
    public int DisplayOrder { get; set; }
}

public class SubmitFormDto
{
    [Required]
    public Dictionary<string, object> FormData { get; set; }
}

public class FormSubmissionResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string? RedirectUrl { get; set; }
    public int? SubmissionId { get; set; }
}

public class FormSubmissionDto
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? IpAddress { get; set; }
}

public class FormSubmissionDetailDto : FormSubmissionDto
{
    public Dictionary<string, object> FormData { get; set; }
    public string? UserAgent { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewerName { get; set; }
    public string? ReviewNotes { get; set; }
}

public class UpdateSubmissionStatusDto
{
    [Required]
    public string Status { get; set; }

    public string? ReviewNotes { get; set; }
}

public class FormSubmissionFilterDto
{
    public string? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
```

### ✅ Checklist - Feature 2

- [ ] إنشاء Models: Form, FormField, FormSubmission
- [ ] إنشاء Entity Configurations
- [ ] تشغيل Migration
- [ ] إنشاء Repository و Interface
- [ ] إنشاء Service و Interface
- [ ] تطبيق Validation Rules
- [ ] تطبيق Conditional Logic
- [ ] إنشاء DTOs
- [ ] إنشاء Controller
- [ ] تطبيق Email Notifications
- [ ] تطبيق Export (CSV/Excel)
- [ ] كتابة Unit Tests

---

## Feature 3: Navigation Menus with Icons

### 📝 الوصف
نظام قوائم تنقل متقدم مع دعم الأيقونات والقوائم المنسدلة.

### 🗄️ Database Models

#### 3.1 Menu Model

**الملف:** `Models/Entities/Menu.cs`

```csharp
public class Menu : TranslatableEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    public string Location { get; set; } // Header, Footer, Sidebar, Mobile

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public ICollection<MenuItem> Items { get; set; } = new List<MenuItem>();
}
```

#### 3.2 MenuItem Model

**الملف:** `Models/Entities/MenuItem.cs`

```csharp
public class MenuItem : TranslatableEntity
{
    public int MenuId { get; set; }
    public Menu Menu { get; set; }

    public int? ParentId { get; set; }
    public MenuItem? Parent { get; set; }

    [Required]
    [StringLength(100)]
    public string Label { get; set; }

    [Required]
    [StringLength(50)]
    public string LinkType { get; set; } // InternalPage, ContentType, Content, ExternalUrl, CustomUrl

    [StringLength(500)]
    public string? LinkValue { get; set; } // Page slug, content type slug, URL, etc.

    public bool OpenInNewTab { get; set; } = false;

    // Icon Configuration
    [StringLength(50)]
    public string? IconType { get; set; } // Lucide, Emoji, CustomSvg

    [StringLength(100)]
    public string? IconValue { get; set; } // Icon name or emoji

    [StringLength(20)]
    public string? IconColor { get; set; } // Hex color

    [StringLength(20)]
    public string? IconSize { get; set; } // sm, md, lg, xl

    public bool ShowIcon { get; set; } = true;

    public int DisplayOrder { get; set; }

    public bool IsVisible { get; set; } = true;

    [StringLength(100)]
    public string? CssClass { get; set; }

    // Navigation Properties
    public ICollection<MenuItem> Children { get; set; } = new List<MenuItem>();
}
```

### 🎯 API Endpoints

**الملف:** `Controllers/MenusController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class MenusController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenusController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    // GET: api/menus
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<MenuDto>>> GetAll()
    {
        var menus = await _menuService.GetAllAsync();
        return Ok(menus);
    }

    // GET: api/menus/{id}
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<MenuDetailDto>> GetById(int id)
    {
        var menu = await _menuService.GetByIdAsync(id);
        return Ok(menu);
    }

    // GET: api/menus/location/{location}?lang=ar
    [HttpGet("location/{location}")]
    [AllowAnonymous]
    public async Task<ActionResult<MenuDetailDto>> GetByLocation(string location, [FromQuery] string? lang = "ar")
    {
        var menu = await _menuService.GetByLocationAsync(location, lang);
        return Ok(menu);
    }

    // POST: api/menus
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<MenuDto>> Create([FromBody] CreateMenuDto dto)
    {
        var menu = await _menuService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = menu.Id }, menu);
    }

    // PUT: api/menus/{id}
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<MenuDto>> Update(int id, [FromBody] UpdateMenuDto dto)
    {
        var menu = await _menuService.UpdateAsync(id, dto);
        return Ok(menu);
    }

    // DELETE: api/menus/{id}
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        await _menuService.DeleteAsync(id);
        return NoContent();
    }

    // POST: api/menus/{id}/items
    [HttpPost("{id}/items")]
    [Authorize]
    public async Task<ActionResult<MenuItemDto>> AddItem(int id, [FromBody] CreateMenuItemDto dto)
    {
        var item = await _menuService.AddItemAsync(id, dto);
        return Ok(item);
    }

    // PUT: api/menus/{id}/items/{itemId}
    [HttpPut("{id}/items/{itemId}")]
    [Authorize]
    public async Task<ActionResult<MenuItemDto>> UpdateItem(int id, int itemId, [FromBody] UpdateMenuItemDto dto)
    {
        var item = await _menuService.UpdateItemAsync(id, itemId, dto);
        return Ok(item);
    }

    // DELETE: api/menus/{id}/items/{itemId}
    [HttpDelete("{id}/items/{itemId}")]
    [Authorize]
    public async Task<ActionResult> DeleteItem(int id, int itemId)
    {
        await _menuService.DeleteItemAsync(id, itemId);
        return NoContent();
    }

    // POST: api/menus/{id}/reorder-items
    [HttpPost("{id}/reorder-items")]
    [Authorize]
    public async Task<ActionResult> ReorderItems(int id, [FromBody] ReorderMenuItemsDto dto)
    {
        await _menuService.ReorderItemsAsync(id, dto.ItemIds);
        return NoContent();
    }
}
```

### 📊 DTOs

**الملف:** `Models/DTOs/Menu/MenuDto.cs`

```csharp
public class MenuDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int ItemCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class MenuDetailDto : MenuDto
{
    public List<MenuItemDto> Items { get; set; }
}

public class MenuItemDto
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Label { get; set; }
    public string LinkType { get; set; }
    public string? LinkValue { get; set; }
    public string? Url { get; set; } // Resolved URL
    public bool OpenInNewTab { get; set; }
    public MenuItemIconDto? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsVisible { get; set; }
    public string? CssClass { get; set; }
    public List<MenuItemDto>? Children { get; set; }
}

public class MenuItemIconDto
{
    public string? IconType { get; set; }
    public string? IconValue { get; set; }
    public string? IconColor { get; set; }
    public string? IconSize { get; set; }
    public bool ShowIcon { get; set; }
}

public class CreateMenuDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Location { get; set; }

    public string? Description { get; set; }
}

public class UpdateMenuDto : CreateMenuDto
{
    public bool IsActive { get; set; } = true;
}

public class CreateMenuItemDto
{
    public int? ParentId { get; set; }

    [Required]
    public string Label { get; set; }

    [Required]
    public string LinkType { get; set; }

    [Required]
    public string LinkValue { get; set; }

    public bool OpenInNewTab { get; set; } = false;
    public string? IconType { get; set; }
    public string? IconValue { get; set; }
    public string? IconColor { get; set; }
    public string? IconSize { get; set; } = "md";
    public bool ShowIcon { get; set; } = true;
    public string? CssClass { get; set; }
    public Dictionary<string, MenuItemTranslationDto>? Translations { get; set; }
}

public class UpdateMenuItemDto : CreateMenuItemDto
{
    public int DisplayOrder { get; set; }
    public bool IsVisible { get; set; } = true;
}

public class ReorderMenuItemsDto
{
    [Required]
    public List<int> ItemIds { get; set; }
}

public class MenuItemTranslationDto
{
    public string Label { get; set; }
}
```

### ✅ Checklist - Feature 3

- [ ] إنشاء Models: Menu, MenuItem
- [ ] إنشاء Entity Configurations
- [ ] تشغيل Migration
- [ ] إنشاء Repository و Interface
- [ ] إنشاء Service و Interface مع URL Resolution
- [ ] إنشاء DTOs
- [ ] إنشاء Controller
- [ ] كتابة Unit Tests

---

## Feature 4: Facilities & Certificates Management

### 📝 الوصف
نظام إدارة المنشآت والشهادات مع التحقق العام.

### 🗄️ Database Models

#### 4.1 Facility Model

**الملف:** `Models/Entities/Facility.cs`

```csharp
public class Facility : TranslatableEntity
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Code { get; set; } // Unique facility code

    [StringLength(50)]
    public string FacilityType { get; set; } // Hospital, Clinic, HealthCenter

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? Region { get; set; }

    [StringLength(20)]
    public string? PostalCode { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(500)]
    public string? Website { get; set; }

    // GPS Coordinates
    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = "Active"; // Active, Suspended, Expired

    public DateTime? AccreditedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    [StringLength(500)]
    public string? Logo { get; set; }

    public string? Description { get; set; }

    // Contact Person
    [StringLength(100)]
    public string? ContactPersonName { get; set; }

    [StringLength(100)]
    public string? ContactPersonTitle { get; set; }

    [StringLength(20)]
    public string? ContactPersonPhone { get; set; }

    [StringLength(200)]
    public string? ContactPersonEmail { get; set; }

    // Navigation Properties
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    public ICollection<FacilityMedia> Media { get; set; } = new List<FacilityMedia>();
}
```

#### 4.2 Certificate Model

**الملف:** `Models/Entities/Certificate.cs`

```csharp
public class Certificate : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string CertificateNumber { get; set; } // Unique certificate number

    public int FacilityId { get; set; }
    public Facility Facility { get; set; }

    [Required]
    [StringLength(50)]
    public string CertificateType { get; set; } // Accreditation, Renewal, etc.

    public DateTime IssuedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = "Valid"; // Valid, Expired, Revoked

    [StringLength(500)]
    public string? CertificateFile { get; set; } // PDF file

    [StringLength(1000)]
    public string? Notes { get; set; }

    public int? IssuedBy { get; set; }
    public User? Issuer { get; set; }

    public DateTime? RevokedAt { get; set; }

    [StringLength(1000)]
    public string? RevokedReason { get; set; }
}
```

#### 4.3 FacilityMedia Model

**الملف:** `Models/Entities/FacilityMedia.cs`

```csharp
public class FacilityMedia : BaseEntity
{
    public int FacilityId { get; set; }
    public Facility Facility { get; set; }

    public int MediaId { get; set; }
    public Media Media { get; set; }

    public int DisplayOrder { get; set; }

    [StringLength(200)]
    public string? Caption { get; set; }

    public bool IsPrimary { get; set; } = false;
}
```

### 🎯 API Endpoints

**الملف:** `Controllers/FacilitiesController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class FacilitiesController : ControllerBase
{
    private readonly IFacilityService _facilityService;

    public FacilitiesController(IFacilityService facilityService)
    {
        _facilityService = facilityService;
    }

    // GET: api/facilities
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<FacilityDto>>> GetAll([FromQuery] FacilityFilterDto filter)
    {
        var facilities = await _facilityService.GetAllAsync(filter);
        return Ok(facilities);
    }

    // GET: api/facilities/{id}
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<FacilityDetailDto>> GetById(int id, [FromQuery] string? lang = "ar")
    {
        var facility = await _facilityService.GetByIdAsync(id, lang);
        return Ok(facility);
    }

    // GET: api/facilities/code/{code}
    [HttpGet("code/{code}")]
    [AllowAnonymous]
    public async Task<ActionResult<FacilityDto>> GetByCode(string code)
    {
        var facility = await _facilityService.GetByCodeAsync(code);
        return Ok(facility);
    }

    // POST: api/facilities
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<FacilityDto>> Create([FromBody] CreateFacilityDto dto)
    {
        var facility = await _facilityService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = facility.Id }, facility);
    }

    // PUT: api/facilities/{id}
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<FacilityDto>> Update(int id, [FromBody] UpdateFacilityDto dto)
    {
        var facility = await _facilityService.UpdateAsync(id, dto);
        return Ok(facility);
    }

    // DELETE: api/facilities/{id}
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        await _facilityService.DeleteAsync(id);
        return NoContent();
    }

    // GET: api/facilities/map
    [HttpGet("map")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<FacilityMapDto>>> GetForMap([FromQuery] FacilityMapFilterDto filter)
    {
        var facilities = await _facilityService.GetForMapAsync(filter);
        return Ok(facilities);
    }
}
```

**الملف:** `Controllers/CertificatesController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class CertificatesController : ControllerBase
{
    private readonly ICertificateService _certificateService;

    public CertificatesController(ICertificateService certificateService)
    {
        _certificateService = certificateService;
    }

    // GET: api/certificates/verify/{certificateNumber}
    [HttpGet("verify/{certificateNumber}")]
    [AllowAnonymous]
    public async Task<ActionResult<CertificateVerificationDto>> Verify(string certificateNumber)
    {
        var result = await _certificateService.VerifyAsync(certificateNumber);
        return Ok(result);
    }

    // GET: api/certificates
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedResult<CertificateDto>>> GetAll([FromQuery] CertificateFilterDto filter)
    {
        var certificates = await _certificateService.GetAllAsync(filter);
        return Ok(certificates);
    }

    // GET: api/certificates/{id}
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<CertificateDetailDto>> GetById(int id)
    {
        var certificate = await _certificateService.GetByIdAsync(id);
        return Ok(certificate);
    }

    // POST: api/certificates
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CertificateDto>> Create([FromBody] CreateCertificateDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var certificate = await _certificateService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = certificate.Id }, certificate);
    }

    // PUT: api/certificates/{id}
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<CertificateDto>> Update(int id, [FromBody] UpdateCertificateDto dto)
    {
        var certificate = await _certificateService.UpdateAsync(id, dto);
        return Ok(certificate);
    }

    // POST: api/certificates/{id}/revoke
    [HttpPost("{id}/revoke")]
    [Authorize]
    public async Task<ActionResult> Revoke(int id, [FromBody] RevokeCertificateDto dto)
    {
        await _certificateService.RevokeAsync(id, dto);
        return NoContent();
    }

    // DELETE: api/certificates/{id}
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        await _certificateService.DeleteAsync(id);
        return NoContent();
    }

    // GET: api/certificates/expiring-soon
    [HttpGet("expiring-soon")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<CertificateDto>>> GetExpiringSoon([FromQuery] int days = 30)
    {
        var certificates = await _certificateService.GetExpiringSoonAsync(days);
        return Ok(certificates);
    }
}
```

### ✅ Checklist - Feature 4

- [ ] إنشاء Models: Facility, Certificate, FacilityMedia
- [ ] إنشاء Entity Configurations
- [ ] تشغيل Migration
- [ ] إنشاء Repository و Interface
- [ ] إنشاء Service و Interface
- [ ] إنشاء DTOs
- [ ] إنشاء Controllers
- [ ] تطبيق Certificate Verification
- [ ] تطبيق Map API
- [ ] كتابة Unit Tests

---

## Feature 5: SEO & Analytics

### 📝 الوصف
نظام SEO متقدم مع دعم Sitemap, Schema.org, والإحصائيات.

### 🗄️ Database Models

#### 5.1 SeoSettings Model

**الملف:** `Models/Entities/SeoSettings.cs`

```csharp
public class SeoSettings : BaseEntity
{
    [StringLength(200)]
    public string? DefaultMetaTitle { get; set; }

    [StringLength(500)]
    public string? DefaultMetaDescription { get; set; }

    public string? DefaultMetaKeywords { get; set; }

    [StringLength(500)]
    public string? DefaultOgImage { get; set; }

    [StringLength(100)]
    public string? TwitterHandle { get; set; }

    [StringLength(100)]
    public string? FacebookAppId { get; set; }

    public string? GoogleAnalyticsId { get; set; }

    public string? GoogleTagManagerId { get; set; }

    public string? StructuredData { get; set; } // JSON-LD for organization

    public bool EnableSitemap { get; set; } = true;

    public bool EnableRobotsTxt { get; set; } = true;

    public string? CustomRobotsTxt { get; set; }
}
```

#### 5.2 Redirect Model

**الملف:** `Models/Entities/Redirect.cs`

```csharp
public class Redirect : BaseEntity
{
    [Required]
    [StringLength(500)]
    public string FromUrl { get; set; }

    [Required]
    [StringLength(500)]
    public string ToUrl { get; set; }

    public int StatusCode { get; set; } = 301; // 301, 302, 307

    public bool IsActive { get; set; } = true;

    public int HitCount { get; set; } = 0;

    public DateTime? LastHitAt { get; set; }
}
```

### 🎯 API Endpoints

**الملف:** `Controllers/SeoController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class SeoController : ControllerBase
{
    private readonly ISeoService _seoService;

    public SeoController(ISeoService seoService)
    {
        _seoService = seoService;
    }

    // GET: api/seo/settings
    [HttpGet("settings")]
    [Authorize]
    public async Task<ActionResult<SeoSettingsDto>> GetSettings()
    {
        var settings = await _seoService.GetSettingsAsync();
        return Ok(settings);
    }

    // PUT: api/seo/settings
    [HttpPut("settings")]
    [Authorize]
    public async Task<ActionResult<SeoSettingsDto>> UpdateSettings([FromBody] UpdateSeoSettingsDto dto)
    {
        var settings = await _seoService.UpdateSettingsAsync(dto);
        return Ok(settings);
    }

    // GET: api/seo/sitemap.xml
    [HttpGet("sitemap.xml")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSitemap([FromQuery] string? lang = "ar")
    {
        var xml = await _seoService.GenerateSitemapAsync(lang);
        return Content(xml, "application/xml");
    }

    // GET: api/seo/robots.txt
    [HttpGet("robots.txt")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRobotsTxt()
    {
        var txt = await _seoService.GenerateRobotsTxtAsync();
        return Content(txt, "text/plain");
    }

    // GET: api/seo/redirects
    [HttpGet("redirects")]
    [Authorize]
    public async Task<ActionResult<PagedResult<RedirectDto>>> GetRedirects([FromQuery] RedirectFilterDto filter)
    {
        var redirects = await _seoService.GetRedirectsAsync(filter);
        return Ok(redirects);
    }

    // POST: api/seo/redirects
    [HttpPost("redirects")]
    [Authorize]
    public async Task<ActionResult<RedirectDto>> CreateRedirect([FromBody] CreateRedirectDto dto)
    {
        var redirect = await _seoService.CreateRedirectAsync(dto);
        return Ok(redirect);
    }

    // PUT: api/seo/redirects/{id}
    [HttpPut("redirects/{id}")]
    [Authorize]
    public async Task<ActionResult<RedirectDto>> UpdateRedirect(int id, [FromBody] UpdateRedirectDto dto)
    {
        var redirect = await _seoService.UpdateRedirectAsync(id, dto);
        return Ok(redirect);
    }

    // DELETE: api/seo/redirects/{id}
    [HttpDelete("redirects/{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteRedirect(int id)
    {
        await _seoService.DeleteRedirectAsync(id);
        return NoContent();
    }

    // POST: api/seo/analyze-url
    [HttpPost("analyze-url")]
    [Authorize]
    public async Task<ActionResult<SeoAnalysisDto>> AnalyzeUrl([FromBody] AnalyzeUrlDto dto)
    {
        var analysis = await _seoService.AnalyzeUrlAsync(dto.Url);
        return Ok(analysis);
    }
}
```

### ✅ Checklist - Feature 5

- [ ] إنشاء Models: SeoSettings, Redirect
- [ ] إنشاء Entity Configurations
- [ ] تشغيل Migration
- [ ] إنشاء Repository و Interface
- [ ] إنشاء Service و Interface
- [ ] تطبيق Sitemap Generator
- [ ] تطبيق Robots.txt Generator
- [ ] تطبيق Redirect Middleware
- [ ] إنشاء DTOs
- [ ] إنشاء Controller
- [ ] كتابة Unit Tests

---

## 📦 NuGet Packages الإضافية للمطور 2

```xml
<!-- لا توجد packages إضافية مطلوبة -->
<!-- جميع الـ packages الأساسية موجودة في Base -->
```

---

## ✅ Checklist شامل للمطور 2

### Week 1-2: Page Builder
- [ ] Feature 1: Page Builder (كامل)
- [ ] Block Types Implementation
- [ ] Migration & Database Setup
- [ ] Basic API Testing

### Week 3: Form Builder
- [ ] Feature 2: Form Builder (كامل)
- [ ] Validation Rules
- [ ] Conditional Logic
- [ ] Email Notifications

### Week 4: Menus & Facilities
- [ ] Feature 3: Navigation Menus
- [ ] Feature 4: Facilities Management (بداية)

### Week 5: Certificates & SEO
- [ ] Feature 4: Certificates (إكمال)
- [ ] Feature 5: SEO System (بداية)

### Week 6: Testing & Documentation
- [ ] Feature 5: SEO (إكمال)
- [ ] Unit Tests لجميع Features
- [ ] Integration Tests
- [ ] API Documentation

---

## 🎯 الأولويات

**Priority 1 (أسبوع 1-2):**
- Page Builder

**Priority 2 (أسبوع 3):**
- Form Builder

**Priority 3 (أسبوع 4-5):**
- Navigation Menus
- Facilities & Certificates

**Priority 4 (أسبوع 5-6):**
- SEO & Analytics
- Testing & Documentation

---

**تاريخ الإنشاء:** 11 نوفمبر 2025  
**آخر تحديث:** 11 نوفمبر 2025  
**الحالة:** 📝 جاهز للتنفيذ
