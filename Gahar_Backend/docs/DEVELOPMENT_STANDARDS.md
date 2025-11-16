# ?? ?????? ??????? ? Best Practices - ???? ????

**?????:** ???? ???? ?????? ???????? ???????? ??????

---

## ?? ???? ?????????

1. [?????? ?????](#??????-?????)
2. [?????? Git](#??????-git)
3. [?????? ???????](#??????-???????)
4. [?????? ??? APIs](#??????-???-apis)
5. [?????? Database](#??????-database)
6. [?????? ??????](#??????-??????)
7. [?????? ??????](#??????-??????)
8. [?????? ???????](#??????-???????)
9. [Code Review Checklist](#code-review-checklist)
10. [Testing Standards](#testing-standards)

---

## 1. ?????? ?????

### 1.1 C# Coding Standards

#### File Organization
```csharp
// 1. Using statements (????? ???????)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GAHAR.Core.Entities;
using GAHAR.Core.Interfaces;

// 2. Namespace
namespace GAHAR.API.Controllers
{
    // 3. Class
    [ApiController]
    [Route("api/[controller]")]
    public class ContentController : ControllerBase
    {
        // 4. Private fields
      private readonly IContentService _contentService;
        private readonly ILogger<ContentController> _logger;

   // 5. Constructor
        public ContentController(
         IContentService contentService,
            ILogger<ContentController> logger)
        {
            _contentService = contentService;
     _logger = logger;
      }

        // 6. Public methods
        [HttpGet]
      public async Task<IActionResult> GetAll()
  {
            // Implementation
    }

  // 7. Private methods
  private void ValidateRequest()
        {
// Implementation
  }
    }
}
```

#### Naming Conventions
```csharp
// ? ??
public class ContentService : IContentService
{
    private readonly IContentRepository _contentRepository;
    private const int MaxPageSize = 100;
    
    public async Task<ContentDto> GetContentByIdAsync(Guid contentId)
    {
    var content = await _contentRepository.GetByIdAsync(contentId);
   return MapToDto(content);
    }
    
    private ContentDto MapToDto(Content content)
    {
   // Implementation
    }
}

// ? ???
public class contentservice : IcontentService
{
    private IContentRepository contentRepository;
    private const int maxpagesize = 100;
    
    public Task<ContentDto> GetContent(Guid id)
    {
        var Content = contentRepository.GetById(id);
  return map(Content);
    }
    
    private ContentDto map(Content c)
    {
        // Implementation
    }
}
```

#### Code Formatting
```csharp
// ? ??: ??????? ?????? ?????
public async Task<IActionResult> CreateContent([FromBody] CreateContentRequest request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    try
    {
        var content = await _contentService.CreateAsync(request);
        return CreatedAtAction(nameof(GetContent), new { id = content.Id }, content);
    }
    catch (ValidationException ex)
    {
    _logger.LogWarning(ex, "Validation failed for content creation");
        return BadRequest(new { message = ex.Message, errors = ex.Errors });
 }
}

// ? ???: ????? ???? ????
public async Task<IActionResult> CreateContent([FromBody] CreateContentRequest request){
if(!ModelState.IsValid){return BadRequest(ModelState);}
try{var content=await _contentService.CreateAsync(request);return CreatedAtAction(nameof(GetContent),new{id=content.Id},content);}
catch(ValidationException ex){_logger.LogWarning(ex,"Validation failed");return BadRequest(new{message=ex.Message});}
}
```

#### SOLID Principles

**1. Single Responsibility Principle (SRP)**
```csharp
// ? ??: ?? class ?? ??????? ?????
public class ContentService : IContentService
{
    public async Task<Content> CreateAsync(CreateContentRequest request)
    {
      // Business logic ??? content ???
    }
}

public class SlugGenerator
{
    public string Generate(string title)
    {
        // Slug generation logic ???
    }
}

// ? ???: ContentService ???? ??? ???
public class ContentService
{
    public async Task<Content> CreateAsync(CreateContentRequest request)
    {
      // Generate slug (SRP violation)
        var slug = GenerateSlug(request.Title);
        
        // Send email (SRP violation)
        await SendNotificationEmail(request.Author);
        
    // Log to file (SRP violation)
        LogToFile($"Content created: {request.Title}");
        
        // Save to database
        return await _repository.AddAsync(content);
    }
}
```

**2. Dependency Inversion Principle (DIP)**
```csharp
// ? ??: Depend on abstractions
public class ContentService
{
    private readonly IContentRepository _repository;
    private readonly ISlugGenerator _slugGenerator;
    private readonly IEmailService _emailService;
    
    public ContentService(
        IContentRepository repository,
     ISlugGenerator slugGenerator,
        IEmailService emailService)
    {
        _repository = repository;
        _slugGenerator = slugGenerator;
        _emailService = emailService;
    }
}

// ? ???: Depend on concrete classes
public class ContentService
{
    private readonly ContentRepository _repository; // Concrete
    private readonly SlugGenerator _slugGenerator; // Concrete
    
    public ContentService()
    {
        _repository = new ContentRepository(); // ? new keyword
        _slugGenerator = new SlugGenerator(); // ? new keyword
}
}
```

### 1.2 Error Handling

#### ??????? Custom Exceptions
```csharp
// ? ??
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}

public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }
    
    public ValidationException(Dictionary<string, string[]> errors)
        : base("One or more validation errors occurred")
    {
 Errors = errors;
    }
}

// ?? ??? Service
public async Task<Content> GetByIdAsync(Guid id)
{
    var content = await _repository.GetByIdAsync(id);
    
    if (content == null)
    {
        throw new NotFoundException($"Content with ID {id} not found");
    }
    
    return content;
}
```

#### Global Exception Handling
```csharp
// ?? Middleware
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public async Task InvokeAsync(HttpContext context)
    {
    try
        {
            await _next(context);
   }
        catch (Exception ex)
   {
      _logger.LogError(ex, "An unhandled exception occurred");
   await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = exception switch
        {
            NotFoundException notFoundEx => new
      {
  statusCode = StatusCodes.Status404NotFound,
    message = notFoundEx.Message
     },
       ValidationException validationEx => new
      {
      statusCode = StatusCodes.Status400BadRequest,
       message = validationEx.Message,
     errors = validationEx.Errors
            },
            UnauthorizedException => new
   {
       statusCode = StatusCodes.Status401Unauthorized,
         message = "Unauthorized access"
            },
  _ => new
 {
      statusCode = StatusCodes.Status500InternalServerError,
                message = "An internal server error occurred"
 }
        };

        context.Response.StatusCode = response.statusCode;
      await context.Response.WriteAsJsonAsync(response);
    }
}
```

### 1.3 Async/Await Best Practices

```csharp
// ? ??
public async Task<Content> GetContentAsync(Guid id)
{
    var content = await _repository.GetByIdAsync(id);
    
    if (content == null)
  {
        throw new NotFoundException($"Content not found");
    }
    
    return content;
}

// ? ??: ??????? Task.WhenAll ???????? ?????????
public async Task<ContentWithRelatedData> GetContentWithRelatedAsync(Guid id)
{
    var contentTask = _repository.GetByIdAsync(id);
    var tagsTask = _tagRepository.GetByContentIdAsync(id);
    var commentsTask = _commentRepository.GetByContentIdAsync(id);
    
    await Task.WhenAll(contentTask, tagsTask, commentsTask);
    
  return new ContentWithRelatedData
 {
        Content = await contentTask,
    Tags = await tagsTask,
    Comments = await commentsTask
    };
}

// ? ???: ??????? .Result ?? .Wait()
public Content GetContent(Guid id)
{
    var content = _repository.GetByIdAsync(id).Result; // ? Deadlock risk
    return content;
}

// ? ???: async void (??? ?? Event Handlers)
public async void DeleteContent(Guid id) // ?
{
    await _repository.DeleteAsync(id);
}
```

---

## 2. ?????? Git

### 2.1 Branching Strategy

```
main
??? develop
 ??? feature/content-management
    ??? feature/page-builder
    ??? feature/form-builder
    ??? bugfix/content-slug-generation
    ??? hotfix/security-vulnerability
```

**???????:**
- `main` - Production only (protected)
- `develop` - Development branch (protected)
- `feature/*` - New features
- `bugfix/*` - Bug fixes
- `hotfix/*` - Urgent production fixes

### 2.2 Commit Message Format

```bash
# Format
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat` - ???? ?????
- `fix` - ????? bug
- `docs` - ?????
- `style` - ????? ????? (?? ???? ??? ???????)
- `refactor` - ????? ?????
- `perf` - ????? ??????
- `test` - ????? tests
- `chore` - ???? ????/???????

**?????:**
```bash
# ? ??
feat(content): add multi-layout support for content
fix(auth): resolve JWT token expiration issue
docs(api): update content API documentation
refactor(repository): optimize content query performance
test(content): add unit tests for ContentService

# ? ???
Added new feature
Fixed bug
Updated code
```

**Commit Message ????:**
```bash
feat(content): add multi-layout support for content

- Add Layout entity and repository
- Implement layout selection in content creation
- Add migration for Layout table
- Update ContentDto to include layout information

Closes #123
```

### 2.3 Pull Request Guidelines

**PR Template:**
```markdown
## Description
[??? ???? ?????????]

## Type of Change
- [ ] Bug fix (non-breaking change which fixes an issue)
- [ ] New feature (non-breaking change which adds functionality)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] Documentation update

## How Has This Been Tested?
- [ ] Unit tests
- [ ] Integration tests
- [ ] Manual testing

## Checklist:
- [ ] My code follows the code style of this project
- [ ] I have performed a self-review of my own code
- [ ] I have commented my code, particularly in hard-to-understand areas
- [ ] I have made corresponding changes to the documentation
- [ ] My changes generate no new warnings
- [ ] I have added tests that prove my fix is effective or that my feature works
- [ ] New and existing unit tests pass locally with my changes
- [ ] Any dependent changes have been merged and published

## Screenshots (if applicable):
[??? screenshots]

## Related Issues:
Closes #[issue number]
```

**PR Review Process:**
1. Developer creates PR
2. Automated tests run (CI)
3. Code review by peer
4. Address feedback
5. Approve & Merge

---

## 3. ?????? ???????

### 3.1 C# Naming Conventions

| Type | Convention | Example |
|------|-----------|---------|
| **Class** | PascalCase | `ContentService` |
| **Interface** | IPascalCase | `IContentRepository` |
| **Method** | PascalCase | `GetContentByIdAsync` |
| **Property** | PascalCase | `ContentTitle` |
| **Private Field** | _camelCase | `_contentRepository` |
| **Parameter** | camelCase | `contentId` |
| **Local Variable** | camelCase | `content` |
| **Constant** | PascalCase | `MaxPageSize` |
| **Enum** | PascalCase | `ContentStatus` |
| **Enum Value** | PascalCase | `Published` |

### 3.2 Database Naming Conventions

| Type | Convention | Example |
|------|-----------|---------|
| **Table** | PascalCase (Plural) | `ContentTranslations` |
| **Column** | PascalCase | `ContentId`, `CreatedAt` |
| **Primary Key** | `Id` | `Id` |
| **Foreign Key** | `[Table]Id` | `ContentId` |
| **Index** | `IX_[Table]_[Column]` | `IX_Content_IsPublished` |
| **Constraint** | `FK_[Table]_[Referenced]` | `FK_Content_ContentType` |

### 3.3 API Naming Conventions

```
# ? ??
GET    /api/content/{type}   - List all content
GET    /api/content/{type}/{slug}       - Get by slug
POST   /api/content/{type}          - Create
PUT    /api/content/{type}/{id}         - Update
DELETE /api/content/{type}/{id}         - Delete
PUT    /api/content/{type}/{id}/publish - Publish (action)

# ? ???
GET    /api/GetAllContent
POST   /api/CreateNewContent
PUT    /api/UpdateContentById
DELETE /api/RemoveContent
```

---

## 4. ?????? ??? APIs

### 4.1 RESTful Design

**HTTP Methods:**
- `GET` - Retrieve (?? ???? ????????)
- `POST` - Create
- `PUT` - Update (????)
- `PATCH` - Update (????)
- `DELETE` - Delete

**Status Codes:**
```csharp
// Success
200 OK       - GET, PUT, PATCH (successful)
201 Created          - POST (successful)
204 No Content       - DELETE (successful)

// Client Errors
400 Bad Request      - Validation error
401 Unauthorized     - Not authenticated
403 Forbidden        - Not authorized
404 Not Found        - Resource not found
409 Conflict      - Duplicate/conflict
422 Unprocessable    - Validation error (complex)

// Server Errors
500 Internal Error   - Server error
503 Service Unavailable - Service down
```

### 4.2 Request/Response Standards

**Request DTO:**
```csharp
public class CreateContentRequest
{
    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Title { get; set; }

    [Required]
    public string Body { get; set; }

    [Required]
    [RegularExpression("^(ar|en)$")]
    public string DefaultLanguage { get; set; }

    public Dictionary<string, TranslationDto> Translations { get; set; }
    
    public Dictionary<string, object>? CustomFields { get; set; }
}
```

**Response DTO:**
```csharp
public class ContentResponse
{
    public Guid Id { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

**Paginated Response:**
```csharp
public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}
```

**Error Response:**
```csharp
public class ErrorResponse
{
    public int StatusCode { get; set; }
  public string Message { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
    public string? TraceId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
```

### 4.3 API Versioning

```csharp
// URL Versioning (recommended)
[Route("api/v1/[controller]")]

// Query String Versioning
[Route("api/[controller]?version=1.0")]

// Header Versioning
Request Headers: api-version: 1.0
```

### 4.4 Filtering, Sorting, Pagination

```csharp
[HttpGet]
public async Task<IActionResult> GetAll(
    [FromQuery] string? search = null,
    [FromQuery] string? sortBy = "createdAt",
    [FromQuery] string? sortOrder = "desc",
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] bool? isPublished = null)
{
    var query = _context.Content.AsQueryable();

    // Filtering
    if (!string.IsNullOrWhiteSpace(search))
    {
 query = query.Where(c => c.Title.Contains(search));
    }

    if (isPublished.HasValue)
    {
        query = query.Where(c => c.IsPublished == isPublished.Value);
    }

    // Sorting
    query = sortOrder?.ToLower() == "asc"
        ? query.OrderBy(GetSortProperty(sortBy))
        : query.OrderByDescending(GetSortProperty(sortBy));

    // Pagination
    var totalCount = await query.CountAsync();
    var items = await query
        .Skip((page - 1) * pageSize)
     .Take(pageSize)
        .ToListAsync();

    return Ok(new PagedResponse<Content>
    {
   Items = items,
      TotalCount = totalCount,
        Page = page,
        PageSize = pageSize
    });
}
```

---

## 5. ?????? Database

### 5.1 Migration Guidelines

```bash
# ????? Migration
dotnet ef migrations add AddLayoutSupport --startup-project ../GAHAR.API

# ?????? Migration ??? ???????
# ??? ??? Migration ???????

# ????? Migration
dotnet ef database update --startup-project ../GAHAR.API

# Rollback Migration
dotnet ef database update PreviousMigrationName --startup-project ../GAHAR.API

# ????? ??? Migration (??? ?? ????? ???)
dotnet ef migrations remove --startup-project ../GAHAR.API
```

**Migration File Example:**
```csharp
public partial class AddLayoutSupport : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
    migrationBuilder.CreateTable(
   name: "Layouts",
        columns: table => new
     {
    Id = table.Column<Guid>(nullable: false),
     Name = table.Column<string>(maxLength: 200, nullable: false),
    DisplayName = table.Column<string>(maxLength: 200, nullable: false),
                LayoutStructure = table.Column<string>(type: "nvarchar(max)", nullable: false),
  IsActive = table.Column<bool>(nullable: false, defaultValue: true),
  CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()")
            },
          constraints: table =>
            {
             table.PrimaryKey("PK_Layouts", x => x.Id);
    });

      migrationBuilder.CreateIndex(
          name: "IX_Layouts_IsActive",
            table: "Layouts",
            column: "IsActive");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Layouts");
    }
}
```

### 5.2 Indexing Strategy

```csharp
// Foreign Keys - Always indexed
builder.HasIndex(c => c.ContentTypeId);
builder.HasIndex(c => c.CreatedById);

// Frequently queried columns
builder.HasIndex(c => c.IsPublished);
builder.HasIndex(c => c.PublishedAt);

// Unique constraints
builder.HasIndex(t => new { t.Slug, t.Language })
    .IsUnique();

// Composite indexes for complex queries
builder.HasIndex(c => new { c.ContentTypeId, c.IsPublished, c.PublishedAt });

// Full-text search (SQL Server)
migrationBuilder.Sql(
  "CREATE FULLTEXT INDEX ON ContentTranslations(Title, Body) KEY INDEX PK_ContentTranslations");
```

### 5.3 Query Optimization

```csharp
// ? ??: AsNoTracking ??? Read-only queries
public async Task<IEnumerable<Content>> GetPublishedAsync()
{
    return await _context.Content
        .Where(c => c.IsPublished)
        .AsNoTracking() // ?
        .ToListAsync();
}

// ? ??: Explicit loading
public async Task<Content> GetWithTranslationsAsync(Guid id)
{
    return await _context.Content
      .Include(c => c.Translations) // ?
    .FirstOrDefaultAsync(c => c.Id == id);
}

// ? ??: Projection (select only needed fields)
public async Task<IEnumerable<ContentSummary>> GetSummariesAsync()
{
    return await _context.Content
        .Where(c => c.IsPublished)
        .Select(c => new ContentSummary
      {
          Id = c.Id,
Title = c.Translations.First().Title,
        PublishedAt = c.PublishedAt
        })
        .AsNoTracking()
        .ToListAsync();
}

// ? ???: N+1 Problem
public async Task<IEnumerable<Content>> GetAllWithTranslations()
{
    var contents = await _context.Content.ToListAsync();
 
    foreach (var content in contents)
    {
  content.Translations = await _context.ContentTranslations
        .Where(t => t.ContentId == content.Id)
            .ToListAsync(); // ? N+1 queries
    }
    
    return contents;
}
```

---

## 6. ?????? ??????

### 6.1 Input Validation

```csharp
// ? ??: Server-side validation
public class CreateContentRequestValidator : AbstractValidator<CreateContentRequest>
{
    public CreateContentRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
       .Length(3, 200).WithMessage("Title must be between 3 and 200 characters")
      .Must(BeValidTitle).WithMessage("Title contains invalid characters");

        RuleFor(x => x.Body)
   .NotEmpty().WithMessage("Body is required")
    .MaximumLength(10000).WithMessage("Body is too long");

        RuleFor(x => x.DefaultLanguage)
      .Must(lang => lang == "ar" || lang == "en")
     .WithMessage("Language must be 'ar' or 'en'");
    }

private bool BeValidTitle(string title)
    {
      // ?? ????? ??? HTML tags ?? SQL injection patterns
   return !Regex.IsMatch(title, @"<[^>]+>|';|--");
    }
}
```

### 6.2 SQL Injection Prevention

```csharp
// ? ??: Parameterized queries (EF Core handles this)
public async Task<Content> GetByIdAsync(Guid id)
{
    return await _context.Content
 .FirstOrDefaultAsync(c => c.Id == id); // ? Safe
}

// ? ??: Explicit parameters in raw SQL
public async Task<IEnumerable<Content>> SearchAsync(string keyword)
{
    return await _context.Content
        .FromSqlRaw("SELECT * FROM Content WHERE Title LIKE {0}", $"%{keyword}%")
        .ToListAsync(); // ? Parameterized
}

// ? ???: String concatenation (SQL Injection risk)
public async Task<IEnumerable<Content>> SearchAsync(string keyword)
{
    var sql = $"SELECT * FROM Content WHERE Title LIKE '%{keyword}%'"; // ? Vulnerable
    return await _context.Content.FromSqlRaw(sql).ToListAsync();
}
```

### 6.3 XSS Prevention

```csharp
// ? ??: HTML Encoding
using System.Web;

public string SanitizeHtml(string input)
{
    return HttpUtility.HtmlEncode(input);
}

// ? ??: Using HtmlSanitizer library
using Ganss.XSS;

public string SanitizeRichText(string html)
{
    var sanitizer = new HtmlSanitizer();
    sanitizer.AllowedTags.Add("b");
    sanitizer.AllowedTags.Add("i");
    sanitizer.AllowedTags.Add("p");
    // ... add allowed tags
    
    return sanitizer.Sanitize(html);
}
```

### 6.4 Authentication & Authorization

```csharp
// ? ??: Policy-based authorization
[Authorize(Policy = "CanPublishContent")]
[HttpPut("{id}/publish")]
public async Task<IActionResult> Publish(Guid id)
{
    // Only SuperAdmin and Admin can access
}

// ? ??: Resource-based authorization
public async Task<IActionResult> Update(Guid id, UpdateContentRequest request)
{
    var content = await _contentService.GetByIdAsync(id);
    
    // Check if user owns the content or is Admin
    if (content.CreatedById != User.FindFirstValue(ClaimTypes.NameIdentifier) 
        && !User.IsInRole("Admin"))
  {
        return Forbid();
    }
    
    // Update content
}

// ? ??: Secure password hashing (Identity handles this)
var result = await _userManager.CreateAsync(user, password);
```

### 6.5 Secrets Management

```csharp
// ? ???: Hard-coded secrets
public class EmailService
{
    private const string ApiKey = "sk_live_abc123xyz"; // ? Never do this
}

// ? ??: Use Configuration
public class EmailService
{
    private readonly string _apiKey;
  
    public EmailService(IConfiguration configuration)
    {
        _apiKey = configuration["SendGrid:ApiKey"]; // ?
    }
}

// ? ????: Use Azure Key Vault or AWS Secrets Manager
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{keyVaultName}.vault.azure.net/"),
    new DefaultAzureCredential());
```

---

## 7. ?????? ??????

### 7.1 Caching Strategy

```csharp
// ? Redis Caching
public class ContentService
{
    private readonly IDistributedCache _cache;
    private readonly IContentRepository _repository;

    public async Task<Content> GetByIdAsync(Guid id)
    {
        var cacheKey = $"content:{id}";
        
        // Try cache first
        var cachedContent = await _cache.GetStringAsync(cacheKey);
   if (cachedContent != null)
{
            return JsonSerializer.Deserialize<Content>(cachedContent);
        }

        // Cache miss - get from database
   var content = await _repository.GetByIdAsync(id);
   
   // Cache for 10 minutes
        var cacheOptions = new DistributedCacheEntryOptions
        {
     AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };
        
        await _cache.SetStringAsync(
            cacheKey, 
   JsonSerializer.Serialize(content),
  cacheOptions);
        
        return content;
    }
    
    public async Task UpdateAsync(Content content)
 {
        await _repository.UpdateAsync(content);
        
        // Invalidate cache
        var cacheKey = $"content:{content.Id}";
        await _cache.RemoveAsync(cacheKey);
    }
}
```

### 7.2 Async Operations

```csharp
// ? ??: Parallel operations
public async Task<DashboardData> GetDashboardDataAsync()
{
    var contentCountTask = _contentRepository.GetCountAsync();
    var pageCountTask = _pageRepository.GetCountAsync();
    var submissionCountTask = _submissionRepository.GetCountAsync();
    
    await Task.WhenAll(contentCountTask, pageCountTask, submissionCountTask);
    
    return new DashboardData
    {
 ContentCount = await contentCountTask,
 PageCount = await pageCountTask,
        SubmissionCount = await submissionCountTask
    };
}
```

### 7.3 Pagination

```csharp
// ? ??: Always paginate large datasets
[HttpGet]
public async Task<IActionResult> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
{
    // Validate page size
    if (pageSize > 100) pageSize = 100;
    if (pageSize < 1) pageSize = 20;
    
 var totalCount = await _repository.GetCountAsync();
    var items = await _repository.GetPagedAsync(page, pageSize);
    
    return Ok(new PagedResponse<Content>
    {
  Items = items,
        TotalCount = totalCount,
        Page = page,
        PageSize = pageSize
    });
}
```

---

## 8. ?????? ???????

### 8.1 XML Documentation

```csharp
/// <summary>
/// Creates a new content item
/// </summary>
/// <param name="type">Content type (e.g., "news", "events")</param>
/// <param name="request">Content creation request</param>
/// <returns>Created content with ID</returns>
/// <exception cref="ValidationException">Thrown when validation fails</exception>
/// <exception cref="NotFoundException">Thrown when content type not found</exception>
/// <response code="201">Content created successfully</response>
/// <response code="400">Validation error</response>
/// <response code="404">Content type not found</response>
[HttpPost("{type}")]
[ProducesResponseType(typeof(ContentResponse), StatusCodes.Status201Created)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
public async Task<IActionResult> Create(
    string type,
    [FromBody] CreateContentRequest request)
{
    // Implementation
}
```

### 8.2 README Documentation

**?? feature ????? README:**
```markdown
# Content Management Feature

## Overview
This feature allows creating, updating, and managing multilingual content.

## Endpoints

### Create Content
```http
POST /api/content/{type}
Content-Type: application/json
Authorization: Bearer {token}

{
  "defaultLanguage": "ar",
  "translations": {
    "ar": {
      "title": "????? ????",
    "body": "..."
    }
  }
}
```

## Database Schema
- `Content` - Main content table
- `ContentTranslation` - Multilingual translations
- `ContentType` - Content type definitions

## Dependencies
- `GAHAR.Core` - Domain models
- `GAHAR.Infrastructure` - Data access
- `AutoMapper` - DTO mapping

## Testing
```bash
dotnet test GAHAR.Tests.Unit/ContentServiceTests.cs
```

## Performance Considerations
- Content is cached in Redis for 10 minutes
- Queries use AsNoTracking for read-only operations
- Indexes on ContentTypeId and IsPublished
```

---

## 9. Code Review Checklist

### ? Before Creating PR

- [ ] Code compiles without errors/warnings
- [ ] All tests pass
- [ ] Code follows naming conventions
- [ ] No hard-coded values (use configuration)
- [ ] Error handling implemented
- [ ] Logging added for important operations
- [ ] XML documentation added for public APIs
- [ ] No sensitive data in code
- [ ] Git commit messages are clear
- [ ] Branch is up-to-date with develop

### ? During Code Review

**Functionality:**
- [ ] Code does what it's supposed to do
- [ ] Edge cases handled
- [ ] Error scenarios handled

**Code Quality:**
- [ ] SOLID principles followed
- [ ] No code duplication (DRY)
- [ ] No god classes/methods
- [ ] Appropriate design patterns used
- [ ] Async/await used correctly

**Performance:**
- [ ] No N+1 query problems
- [ ] Appropriate caching
- [ ] Database queries optimized
- [ ] Pagination implemented for large datasets

**Security:**
- [ ] Input validation present
- [ ] SQL injection prevented
- [ ] XSS prevention implemented
- [ ] Authentication/Authorization correct
- [ ] No sensitive data exposed

**Testing:**
- [ ] Unit tests present (80%+ coverage)
- [ ] Integration tests for critical paths
- [ ] Tests are meaningful (not just for coverage)

**Documentation:**
- [ ] XML documentation present
- [ ] README updated if needed
- [ ] API documentation updated

---

## 10. Testing Standards

### 10.1 Unit Test Structure

```csharp
// Naming: MethodName_Scenario_ExpectedBehavior
public class ContentServiceTests
{
    [Fact]
    public async Task CreateContentAsync_WithValidData_ShouldReturnCreatedContent()
    {
        // Arrange
  var mockRepository = new Mock<IContentRepository>();
    var mockUnitOfWork = new Mock<IUnitOfWork>();
 mockUnitOfWork.Setup(u => u.ContentRepository).Returns(mockRepository.Object);
        
     var service = new ContentService(mockUnitOfWork.Object);
        var request = new CreateContentRequest
        {
    // ... test data
        };

        // Act
 var result = await service.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
      mockRepository.Verify(r => r.AddAsync(It.IsAny<Content>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var mockRepository = new Mock<IContentRepository>();
      mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Content)null);
        
        var service = new ContentService(mockUnitOfWork.Object);

    // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetByIdAsync(Guid.NewGuid()));
    }
}
```

### 10.2 Test Coverage Goals

- **Unit Tests:** 85%+ coverage
- **Integration Tests:** Critical paths
- **E2E Tests:** Main workflows

### 10.3 Test Data Management

```csharp
// Test Data Builders
public class ContentBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _title = "Test Content";
    private bool _isPublished = false;

    public ContentBuilder WithId(Guid id)
    {
        _id = id;
        return this;
 }

    public ContentBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public ContentBuilder Published()
    {
  _isPublished = true;
      return this;
    }

    public Content Build()
    {
        return new Content
      {
            Id = _id,
     Title = _title,
   IsPublished = _isPublished,
  CreatedAt = DateTime.UtcNow
        };
    }
}

// Usage
var content = new ContentBuilder()
    .WithTitle("My Test Content")
    .Published()
    .Build();
```

---

## ?? ???????

**??? ???????? ???? ???????? - ?? ??????? ???????!**

**?????????:**
1. ? **??????** - ?? ????? ?????
2. ? **??????** - ???????? ?????
3. ? **???? ?????** - ??????? ??????????
4. ? **???????** - ?????? ?????? ??????????

**??? ????:**
> "????? ?? ????. ????? ?? ????. ?????? ?? ?????."

---

**?? ????? ??? ?????? ??????:** ???? ?????? ???????  
**???????:** 10 ?????? 2025  
**???????:** 1.0

**???????????:** ????? ?? Tech Lead
