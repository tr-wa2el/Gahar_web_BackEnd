# ?? ????? ????? - ???? ????

**?????:** ????? ??????? ????? ???? ????

---

## ?? ???? ?????????

1. [???? ????: Content Management Feature](#????-????-content-management-feature)
2. [???? ????: Authentication Flow](#????-????-authentication-flow)
3. [???? ????: Form Builder](#????-????-form-builder)
4. [???? ????: Album with Bulk Upload](#????-????-album-with-bulk-upload)
5. [???? ????: Multi-Layout System](#????-????-multi-layout-system)
6. [Common Scenarios & Solutions](#common-scenarios--solutions)

---

## 1. ???? ????: Content Management Feature

### ?????????
> Admin ???? ????? ??? ???? ???????? ???????????? ?? ???? ?????? ????? ?? Layout ????

### 1.1 Entity Definition

**Content.cs:**
```csharp
namespace GAHAR.Core.Entities
{
    public class Content
    {
        public Guid Id { get; set; }
        public Guid ContentTypeId { get; set; }
public bool IsPublished { get; set; }
        public DateTime? PublishedAt { get; set; }
public string CreatedById { get; set; }
  public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public ContentType ContentType { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ICollection<ContentTranslation> Translations { get; set; } = new List<ContentTranslation>();
    }
}
```

**ContentTranslation.cs:**
```csharp
namespace GAHAR.Core.Entities
{
  public class ContentTranslation
    {
        public Guid Id { get; set; }
        public Guid ContentId { get; set; }
 public string Language { get; set; } // "ar" or "en"
     public string Slug { get; set; }
   public string Title { get; set; }
public string Body { get; set; }
public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? OgTitle { get; set; }
        public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
        public string? FieldsData { get; set; } // JSON: { "featuredImage": "...", "author": "..." }
        public Guid? LayoutId { get; set; }
   public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

// Navigation Properties
        public Content Content { get; set; }
        public Layout Layout { get; set; }
    }
}
```

### 1.2 Repository Implementation

**IContentRepository.cs:**
```csharp
namespace GAHAR.Core.Interfaces
{
    public interface IContentRepository : IRepository<Content>
    {
        Task<Content?> GetBySlugAsync(string slug, string language);
        Task<PagedResult<Content>> GetPublishedAsync(
          string contentType,
            string language,
    int page,
            int pageSize);
 Task<int> GetCountAsync(string contentType);
    }
}
```

**ContentRepository.cs:**
```csharp
namespace GAHAR.Infrastructure.Repositories
{
    public class ContentRepository : GenericRepository<Content>, IContentRepository
    {
        public ContentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Content?> GetBySlugAsync(string slug, string language)
        {
   return await _context.Content
                .Include(c => c.ContentType)
     .Include(c => c.Translations.Where(t => t.Language == language))
        .Include(c => c.CreatedBy)
          .FirstOrDefaultAsync(c => c.Translations.Any(t => t.Slug == slug && t.Language == language));
        }

      public async Task<PagedResult<Content>> GetPublishedAsync(
            string contentType,
 string language,
    int page,
     int pageSize)
        {
            var query = _context.Content
       .Where(c => c.ContentType.Name == contentType && c.IsPublished)
    .Include(c => c.Translations.Where(t => t.Language == language))
      .OrderByDescending(c => c.PublishedAt);

            var totalCount = await query.CountAsync();
 var items = await query
     .Skip((page - 1) * pageSize)
         .Take(pageSize)
     .AsNoTracking()
     .ToListAsync();

            return new PagedResult<Content>
    {
     Items = items,
TotalCount = totalCount,
 Page = page,
                PageSize = pageSize
       };
        }

        public async Task<int> GetCountAsync(string contentType)
        {
         return await _context.Content
           .CountAsync(c => c.ContentType.Name == contentType && c.IsPublished);
        }
    }
}
```

### 1.3 Service Layer

**IContentService.cs:**
```csharp
namespace GAHAR.Core.Interfaces
{
    public interface IContentService
    {
        Task<ContentResponse> CreateAsync(string contentType, CreateContentRequest request);
        Task<ContentResponse> GetBySlugAsync(string contentType, string slug, string language);
     Task<PagedResponse<ContentSummary>> GetAllAsync(
    string contentType,
 string language,
       int page,
   int pageSize);
    Task<ContentResponse> UpdateAsync(Guid id, string language, UpdateContentRequest request);
        Task DeleteAsync(Guid id);
        Task PublishAsync(Guid id);
    }
}
```

**ContentService.cs:**
```csharp
namespace GAHAR.Core.Services
{
    public class ContentService : IContentService
    {
  private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISlugGenerator _slugGenerator;
        private readonly ICacheService _cacheService;
    private readonly ILogger<ContentService> _logger;

  public ContentService(
   IUnitOfWork unitOfWork,
            IMapper mapper,
  ISlugGenerator slugGenerator,
         ICacheService cacheService,
            ILogger<ContentService> logger)
  {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
   _slugGenerator = slugGenerator;
  _cacheService = cacheService;
            _logger = logger;
        }

      public async Task<ContentResponse> CreateAsync(string contentType, CreateContentRequest request)
        {
      // 1. ?????? ?? ???? ContentType
     var contentTypeEntity = await _unitOfWork.ContentTypeRepository
       .GetByNameAsync(contentType);
  
   if (contentTypeEntity == null)
            {
throw new NotFoundException($"Content type '{contentType}' not found");
         }

        // 2. ????? Content entity
            var content = new Content
            {
    Id = Guid.NewGuid(),
  ContentTypeId = contentTypeEntity.Id,
           IsPublished = request.IsPublished,
 PublishedAt = request.IsPublished ? DateTime.UtcNow : null,
        CreatedById = request.UserId,
         CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // 3. ????? Translations
     foreach (var (lang, translation) in request.Translations)
 {
   var slug = string.IsNullOrWhiteSpace(translation.Slug)
           ? _slugGenerator.Generate(translation.Title)
             : translation.Slug;

      content.Translations.Add(new ContentTranslation
     {
      Id = Guid.NewGuid(),
 ContentId = content.Id,
  Language = lang,
        Slug = slug,
         Title = translation.Title,
             Body = translation.Body,
           MetaTitle = translation.MetaTitle ?? translation.Title,
     MetaDescription = translation.MetaDescription,
   OgTitle = translation.OgTitle ?? translation.Title,
    OgDescription = translation.OgDescription,
                 OgImage = translation.OgImage,
           FieldsData = JsonSerializer.Serialize(request.CustomFields),
       LayoutId = request.LayoutId,
     CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        });
  }

    // 4. ??? ?? Database
            await _unitOfWork.ContentRepository.AddAsync(content);
        await _unitOfWork.SaveChangesAsync();

    _logger.LogInformation(
          "Content created: {ContentType} - {Title}",
         contentType,
           content.Translations.First().Title);

       // 5. ????? Response
         return _mapper.Map<ContentResponse>(content);
        }

        public async Task<ContentResponse> GetBySlugAsync(string contentType, string slug, string language)
        {
            // 1. Try cache first
            var cacheKey = $"content:{contentType}:{slug}:{language}";
     var cached = await _cacheService.GetAsync<ContentResponse>(cacheKey);
      
            if (cached != null)
 {
                _logger.LogInformation("Content served from cache: {Slug}", slug);
 return cached;
     }

            // 2. Get from database
 var content = await _unitOfWork.ContentRepository.GetBySlugAsync(slug, language);
            
 if (content == null)
            {
  throw new NotFoundException($"Content with slug '{slug}' not found");
 }

       // 3. Map to response
            var response = _mapper.Map<ContentResponse>(content);

            // 4. Cache for 10 minutes
        await _cacheService.SetAsync(
           cacheKey,
      response,
       TimeSpan.FromMinutes(10));

            return response;
        }

        public async Task<PagedResponse<ContentSummary>> GetAllAsync(
            string contentType,
            string language,
      int page,
  int pageSize)
      {
     var result = await _unitOfWork.ContentRepository.GetPublishedAsync(
        contentType,
        language,
             page,
    pageSize);

   return new PagedResponse<ContentSummary>
    {
       Items = _mapper.Map<List<ContentSummary>>(result.Items),
   TotalCount = result.TotalCount,
     Page = page,
          PageSize = pageSize
       };
}

        public async Task PublishAsync(Guid id)
        {
   var content = await _unitOfWork.ContentRepository.GetByIdAsync(id);
  
            if (content == null)
       {
       throw new NotFoundException($"Content with ID {id} not found");
         }

content.IsPublished = true;
            content.PublishedAt = DateTime.UtcNow;
 content.UpdatedAt = DateTime.UtcNow;

      await _unitOfWork.ContentRepository.UpdateAsync(content);
    await _unitOfWork.SaveChangesAsync();

            // Invalidate cache
    foreach (var translation in content.Translations)
          {
      var cacheKey = $"content:{content.ContentType.Name}:{translation.Slug}:{translation.Language}";
        await _cacheService.RemoveAsync(cacheKey);
     }

 _logger.LogInformation("Content published: {Id}", id);
   }
    }
}
```

### 1.4 API Controller

**ContentController.cs:**
```csharp
namespace GAHAR.API.Controllers
{
    [ApiController]
    [Route("api/content")]
 [Authorize]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;
  private readonly ILogger<ContentController> _logger;

        public ContentController(
    IContentService contentService,
       ILogger<ContentController> logger)
  {
            _contentService = contentService;
      _logger = logger;
        }

        /// <summary>
        /// Creates a new content item
        /// </summary>
    /// <param name="type">Content type (e.g., "news", "events")</param>
        /// <param name="request">Content creation request</param>
        /// <returns>Created content</returns>
        [HttpPost("{type}")]
    [Authorize(Policy = "CanEditContent")]
        [ProducesResponseType(typeof(ContentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
public async Task<IActionResult> Create(
    string type,
       [FromBody] CreateContentRequest request)
        {
            try
   {
   // Add current user ID to request
                request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var content = await _contentService.CreateAsync(type, request);

             _logger.LogInformation(
         "Content created by user {UserId}: {ContentId}",
          request.UserId,
            content.Id);

                return CreatedAtAction(
    nameof(GetBySlug),
       new { type, slug = content.Slug, lang = request.DefaultLanguage },
   content);
        }
            catch (NotFoundException ex)
      {
         return NotFound(new ErrorResponse
       {
          StatusCode = StatusCodes.Status404NotFound,
         Message = ex.Message
        });
       }
   catch (ValidationException ex)
          {
  return BadRequest(new ErrorResponse
    {
        StatusCode = StatusCodes.Status400BadRequest,
           Message = ex.Message,
        Errors = ex.Errors
           });
       }
  }

        /// <summary>
        /// Gets content by slug
        /// </summary>
        /// <param name="type">Content type</param>
        /// <param name="slug">Content slug</param>
        /// <param name="lang">Language (ar/en)</param>
        /// <returns>Content details</returns>
 [HttpGet("{type}/{slug}")]
   [AllowAnonymous]
        [ResponseCache(Duration = 600, VaryByQueryKeys = new[] { "lang" })]
        [ProducesResponseType(typeof(ContentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetBySlug(
   string type,
     string slug,
[FromQuery] string lang = "ar")
        {
            try
            {
    var content = await _contentService.GetBySlugAsync(type, slug, lang);
  return Ok(content);
 }
            catch (NotFoundException ex)
            {
                return NotFound(new ErrorResponse
         {
 StatusCode = StatusCodes.Status404NotFound,
        Message = ex.Message
                });
   }
        }

     /// <summary>
        /// Gets paginated list of content
        /// </summary>
        /// <param name="type">Content type</param>
        /// <param name="lang">Language</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Items per page</param>
        /// <returns>Paginated content list</returns>
        [HttpGet("{type}")]
[AllowAnonymous]
        [ResponseCache(Duration = 300, VaryByQueryKeys = new[] { "lang", "page", "pageSize" })]
        public async Task<IActionResult> GetAll(
      string type,
  [FromQuery] string lang = "ar",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
     if (pageSize > 100) pageSize = 100;

            var result = await _contentService.GetAllAsync(type, lang, page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Publishes content
        /// </summary>
     /// <param name="type">Content type</param>
        /// <param name="id">Content ID</param>
  [HttpPut("{type}/{id}/publish")]
  [Authorize(Policy = "CanPublishContent")]
    public async Task<IActionResult> Publish(string type, Guid id)
        {
       try
      {
    await _contentService.PublishAsync(id);
      return Ok(new { message = "Content published successfully" });
       }
     catch (NotFoundException ex)
            {
      return NotFound(new ErrorResponse
    {
        StatusCode = StatusCodes.Status404NotFound,
      Message = ex.Message
    });
}
        }
    }
}
```

### 1.5 DTOs

**CreateContentRequest.cs:**
```csharp
namespace GAHAR.Shared.DTOs.Content
{
 public class CreateContentRequest
    {
        [Required]
        [RegularExpression("^(ar|en)$")]
        public string DefaultLanguage { get; set; }

        [Required]
        public Dictionary<string, TranslationDto> Translations { get; set; }

        public bool IsPublished { get; set; } = false;

      public Guid? LayoutId { get; set; }

      public Dictionary<string, object>? CustomFields { get; set; }

        // Set by controller
        [JsonIgnore]
        public string UserId { get; set; }
    }

    public class TranslationDto
    {
        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public string? Slug { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? OgTitle { get; set; }
        public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
    }
}
```

**ContentResponse.cs:**
```csharp
namespace GAHAR.Shared.DTOs.Content
{
    public class ContentResponse
    {
        public Guid Id { get; set; }
   public string Slug { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
 public string Language { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedAt { get; set; }
   public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

// SEO
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    public string? OgTitle { get; set; }
      public string? OgDescription { get; set; }
        public string? OgImage { get; set; }

        // Layout
 public LayoutSummary? Layout { get; set; }

      // Custom Fields
   public Dictionary<string, object>? CustomFields { get; set; }

    // Author
        public AuthorSummary Author { get; set; }
    }

    public class ContentSummary
    {
        public Guid Id { get; set; }
   public string Slug { get; set; }
      public string Title { get; set; }
        public string Excerpt { get; set; }
        public string? FeaturedImage { get; set; }
        public DateTime? PublishedAt { get; set; }
 }
}
```

### 1.6 Unit Tests

**ContentServiceTests.cs:**
```csharp
namespace GAHAR.Tests.Unit.Services
{
    public class ContentServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ISlugGenerator> _slugGeneratorMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<ContentService>> _loggerMock;
        private readonly ContentService _sut;

        public ContentServiceTests()
        {
    _unitOfWorkMock = new Mock<IUnitOfWork>();
  _mapperMock = new Mock<IMapper>();
      _slugGeneratorMock = new Mock<ISlugGenerator>();
    _cacheServiceMock = new Mock<ICacheService>();
       _loggerMock = new Mock<ILogger<ContentService>>();

  _sut = new ContentService(
            _unitOfWorkMock.Object,
   _mapperMock.Object,
    _slugGeneratorMock.Object,
    _cacheServiceMock.Object,
  _loggerMock.Object);
  }

        [Fact]
        public async Task CreateAsync_WithValidData_ShouldReturnCreatedContent()
        {
         // Arrange
       var contentType = "news";
            var request = new CreateContentRequest
          {
        DefaultLanguage = "ar",
   Translations = new Dictionary<string, TranslationDto>
                {
          ["ar"] = new TranslationDto
    {
              Title = "??? ????",
 Body = "????? ?????"
         }
   },
       IsPublished = false,
       UserId = "user-123"
     };

            var contentTypeEntity = new ContentType
        {
          Id = Guid.NewGuid(),
                Name = contentType
       };

   _unitOfWorkMock.Setup(u => u.ContentTypeRepository.GetByNameAsync(contentType))
          .ReturnsAsync(contentTypeEntity);

    _slugGeneratorMock.Setup(s => s.Generate("??? ????"))
    .Returns("khabr-jadeed");

       _unitOfWorkMock.Setup(u => u.ContentRepository.AddAsync(It.IsAny<Content>()))
    .ReturnsAsync((Content c) => c);

     _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
           .ReturnsAsync(1);

            var expectedResponse = new ContentResponse
          {
     Id = Guid.NewGuid(),
       Title = "??? ????",
         Slug = "khabr-jadeed"
    };

   _mapperMock.Setup(m => m.Map<ContentResponse>(It.IsAny<Content>()))
             .Returns(expectedResponse);

            // Act
       var result = await _sut.CreateAsync(contentType, request);

         // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("??? ????");
        result.Slug.Should().Be("khabr-jadeed");

      _unitOfWorkMock.Verify(u => u.ContentRepository.AddAsync(It.IsAny<Content>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            _slugGeneratorMock.Verify(s => s.Generate("??? ????"), Times.Once);
      }

     [Fact]
        public async Task CreateAsync_WithInvalidContentType_ShouldThrowNotFoundException()
        {
   // Arrange
            var contentType = "invalid-type";
            var request = new CreateContentRequest
    {
 DefaultLanguage = "ar",
   Translations = new Dictionary<string, TranslationDto>
  {
["ar"] = new TranslationDto
         {
             Title = "Test",
              Body = "Test"
 }
      }
            };

     _unitOfWorkMock.Setup(u => u.ContentTypeRepository.GetByNameAsync(contentType))
             .ReturnsAsync((ContentType)null);

            // Act & Assert
     await Assert.ThrowsAsync<NotFoundException>(
  () => _sut.CreateAsync(contentType, request));
        }

  [Fact]
        public async Task GetBySlugAsync_WithCachedContent_ShouldReturnFromCache()
        {
   // Arrange
            var contentType = "news";
       var slug = "test-slug";
 var language = "ar";
            var cacheKey = $"content:{contentType}:{slug}:{language}";

         var cachedContent = new ContentResponse
   {
        Id = Guid.NewGuid(),
      Title = "Cached Content",
       Slug = slug
          };

            _cacheServiceMock.Setup(c => c.GetAsync<ContentResponse>(cacheKey))
     .ReturnsAsync(cachedContent);

        // Act
     var result = await _sut.GetBySlugAsync(contentType, slug, language);

          // Assert
     result.Should().NotBeNull();
            result.Title.Should().Be("Cached Content");

// Verify that repository was NOT called
 _unitOfWorkMock.Verify(
                u => u.ContentRepository.GetBySlugAsync(It.IsAny<string>(), It.IsAny<string>()),
    Times.Never);
        }

     [Fact]
  public async Task GetBySlugAsync_WithNoCachedContent_ShouldFetchFromDatabaseAndCache()
     {
         // Arrange
            var contentType = "news";
      var slug = "test-slug";
        var language = "ar";
    var cacheKey = $"content:{contentType}:{slug}:{language}";

         var content = new Content
      {
   Id = Guid.NewGuid(),
            Translations = new List<ContentTranslation>
  {
        new ContentTranslation
     {
    Title = "Test Content",
             Slug = slug,
   Language = language
        }
     }
            };

            var expectedResponse = new ContentResponse
            {
        Id = content.Id,
             Title = "Test Content",
           Slug = slug
  };

   _cacheServiceMock.Setup(c => c.GetAsync<ContentResponse>(cacheKey))
         .ReturnsAsync((ContentResponse)null);

 _unitOfWorkMock.Setup(u => u.ContentRepository.GetBySlugAsync(slug, language))
    .ReturnsAsync(content);

    _mapperMock.Setup(m => m.Map<ContentResponse>(content))
      .Returns(expectedResponse);

          // Act
            var result = await _sut.GetBySlugAsync(contentType, slug, language);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Test Content");

            _unitOfWorkMock.Verify(
       u => u.ContentRepository.GetBySlugAsync(slug, language),
                Times.Once);

   _cacheServiceMock.Verify(
              c => c.SetAsync(cacheKey, expectedResponse, It.IsAny<TimeSpan>()),
  Times.Once);
        }
    }
}
```

### 1.7 Integration Test

**ContentEndpointsTests.cs:**
```csharp
namespace GAHAR.Tests.Integration
{
    public class ContentEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
      private readonly WebApplicationFactory<Program> _factory;

    public ContentEndpointsTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
       _client = factory.CreateClient();
        }

 [Fact]
 public async Task CreateContent_WithValidData_Returns201Created()
        {
   // Arrange
            var request = new CreateContentRequest
            {
          DefaultLanguage = "ar",
        Translations = new Dictionary<string, TranslationDto>
      {
          ["ar"] = new TranslationDto
        {
          Title = "??? ??????",
          Body = "????? ????? ????????"
      }
},
IsPublished = false
   };

            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = 
     new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.PostAsJsonAsync("/api/content/news", request);

 // Assert
       response.StatusCode.Should().Be(HttpStatusCode.Created);

 var content = await response.Content.ReadFromJsonAsync<ContentResponse>();
  content.Should().NotBeNull();
      content.Title.Should().Be("??? ??????");
         content.Slug.Should().NotBeNullOrEmpty();
    }

        [Fact]
        public async Task GetContentBySlug_WithValidSlug_Returns200OK()
        {
            // Arrange
            var slug = "test-news-slug";

   // Act
            var response = await _client.GetAsync($"/api/content/news/{slug}?lang=ar");

            // Assert
 response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ContentResponse>();
        content.Should().NotBeNull();
   content.Slug.Should().Be(slug);
        }

        [Fact]
        public async Task GetContentBySlug_WithInvalidSlug_Returns404NotFound()
        {
            // Arrange
            var invalidSlug = "non-existent-slug";

            // Act
   var response = await _client.GetAsync($"/api/content/news/{invalidSlug}?lang=ar");

        // Assert
      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<string> GetAuthTokenAsync()
        {
            var loginRequest = new LoginRequest
{
     Email = "admin@gahar.sa",
            Password = "Admin@123"
     };

 var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

            return result.AccessToken;
}
    }
}
```

### 1.8 ????????? ?? ??? Frontend

**JavaScript/TypeScript:**
```typescript
// 1. Create Content
async function createContent() {
    const request = {
     defaultLanguage: 'ar',
        translations: {
      ar: {
   title: '??? ????',
   body: '????? ?????...',
       metaTitle: '??? ???? - ????',
 metaDescription: '??? ????? ?????'
      },
            en: {
        title: 'New News',
    body: 'News content...',
           metaTitle: 'New News - GAHAR',
    metaDescription: 'News description for SEO'
          }
},
  isPublished: false,
    customFields: {
   featuredImage: '/uploads/news-image.jpg',
            author: 'Admin',
       tags: ['health', 'accreditation']
  }
    };

    const response = await fetch('https://api.gahar.sa/api/content/news', {
 method: 'POST',
        headers: {
         'Content-Type': 'application/json',
  'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(request)
    });

    if (response.ok) {
        const content = await response.json();
        console.log('Content created:', content);
    }
}

// 2. Get Content by Slug
async function getContent(slug: string, lang: string = 'ar') {
    const response = await fetch(
        `https://api.gahar.sa/api/content/news/${slug}?lang=${lang}`
    );

    if (response.ok) {
        const content = await response.json();
        return content;
    }
}

// 3. Get All Content (Paginated)
async function getAllContent(lang: string = 'ar', page: number = 1) {
    const response = await fetch(
        `https://api.gahar.sa/api/content/news?lang=${lang}&page=${page}&pageSize=10`
    );

    if (response.ok) {
      const result = await response.json();
        console.log(`Total: ${result.totalCount}`);
        console.log(`Page ${result.page} of ${result.totalPages}`);
        return result.items;
    }
}

// 4. Publish Content
async function publishContent(id: string) {
    const response = await fetch(
        `https://api.gahar.sa/api/content/news/${id}/publish`,
        {
    method: 'PUT',
    headers: {
     'Authorization': `Bearer ${token}`
        }
        }
    );

    if (response.ok) {
     console.log('Content published successfully');
    }
}
```

---

## 2. ???? ????: Authentication Flow

### ?????????
> ?????? ???? ????? ???? ????? ???? ??? JWT token? ??????? ?????? ?? protected endpoints

### 2.1 Register Flow

**RegisterRequest.cs:**
```csharp
public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string FullName { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }
}
```

**AuthController.cs (Register):**
```csharp
[HttpPost("register")]
[AllowAnonymous]
public async Task<IActionResult> Register([FromBody] RegisterRequest request)
{
    // 1. Check if user already exists
    var existingUser = await _userManager.FindByEmailAsync(request.Email);
    if (existingUser != null)
    {
        return BadRequest(new ErrorResponse
    {
            StatusCode = StatusCodes.Status400BadRequest,
        Message = "User with this email already exists"
   });
    }

    // 2. Create user
    var user = new ApplicationUser
    {
      UserName = request.Email,
        Email = request.Email,
   FullName = request.FullName,
        PhoneNumber = request.PhoneNumber,
        CreatedAt = DateTime.UtcNow,
        IsActive = true
    };

    var result = await _userManager.CreateAsync(user, request.Password);

    if (!result.Succeeded)
    {
        return BadRequest(new ErrorResponse
        {
            StatusCode = StatusCodes.Status400BadRequest,
      Message = "User registration failed",
            Errors = result.Errors.ToDictionary(
    e => e.Code,
      e => new[] { e.Description })
        });
    }

    // 3. Assign default role
    await _userManager.AddToRoleAsync(user, "Viewer");

    // 4. Send confirmation email (optional)
    // var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    // await _emailService.SendConfirmationEmailAsync(user.Email, token);

    _logger.LogInformation("User {Email} registered successfully", user.Email);

    return Ok(new
    {
     userId = user.Id,
        email = user.Email,
        message = "User registered successfully"
    });
}
```

### 2.2 Login Flow

**LoginRequest.cs:**
```csharp
public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public bool RememberMe { get; set; } = false;
}
```

**TokenResponse.cs:**
```csharp
public class TokenResponse
{
public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
  public string TokenType { get; set; } = "Bearer";
    public UserDto User { get; set; }
}

public class UserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public List<string> Roles { get; set; }
}
```

**AuthController.cs (Login):**
```csharp
[HttpPost("login")]
[AllowAnonymous]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    // 1. Find user
    var user = await _userManager.FindByEmailAsync(request.Email);
    
    if (user == null || !user.IsActive)
    {
     return Unauthorized(new ErrorResponse
        {
            StatusCode = StatusCodes.Status401Unauthorized,
      Message = "Invalid email or password"
        });
    }

    // 2. Check password
    var result = await _signInManager.CheckPasswordSignInAsync(
        user,
 request.Password,
   lockoutOnFailure: true);

    if (!result.Succeeded)
    {
     if (result.IsLockedOut)
{
            return Unauthorized(new ErrorResponse
        {
    StatusCode = StatusCodes.Status401Unauthorized,
         Message = "Account is locked due to multiple failed login attempts. Try again later."
     });
   }

        return Unauthorized(new ErrorResponse
        {
            StatusCode = StatusCodes.Status401Unauthorized,
  Message = "Invalid email or password"
        });
    }

 // 3. Generate tokens
    var roles = await _userManager.GetRolesAsync(user);
  var accessToken = _tokenGenerator.GenerateToken(user, roles);
 var refreshToken = _tokenGenerator.GenerateRefreshToken();

    // 4. Save refresh token to database
    user.RefreshToken = refreshToken;
user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
    user.LastLoginAt = DateTime.UtcNow;
    await _userManager.UpdateAsync(user);

    _logger.LogInformation("User {Email} logged in successfully", user.Email);

 // 5. Return tokens
    return Ok(new TokenResponse
    {
        AccessToken = accessToken,
        RefreshToken = refreshToken,
     ExpiresIn = 3600, // 1 hour
     User = new UserDto
        {
            Id = user.Id,
       Email = user.Email,
   FullName = user.FullName,
       Roles = roles.ToList()
        }
    });
}
```

### 2.3 Refresh Token Flow

**RefreshTokenRequest.cs:**
```csharp
public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}
```

**AuthController.cs (RefreshToken):**
```csharp
[HttpPost("refresh-token")]
[AllowAnonymous]
public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
{
    // 1. Find user by refresh token
    var users = await _userManager.Users
        .Where(u => u.RefreshToken == request.RefreshToken)
        .ToListAsync();

    var user = users.FirstOrDefault();

    if (user == null)
    {
        return Unauthorized(new ErrorResponse
   {
            StatusCode = StatusCodes.Status401Unauthorized,
  Message = "Invalid refresh token"
        });
    }

    // 2. Check if refresh token is expired
    if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
    {
        return Unauthorized(new ErrorResponse
 {
        StatusCode = StatusCodes.Status401Unauthorized,
            Message = "Refresh token expired"
        });
    }

    // 3. Generate new tokens
    var roles = await _userManager.GetRolesAsync(user);
    var newAccessToken = _tokenGenerator.GenerateToken(user, roles);
var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

    // 4. Update refresh token
    user.RefreshToken = newRefreshToken;
    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
    await _userManager.UpdateAsync(user);

    _logger.LogInformation("Tokens refreshed for user {Email}", user.Email);

    // 5. Return new tokens
    return Ok(new TokenResponse
    {
        AccessToken = newAccessToken,
   RefreshToken = newRefreshToken,
     ExpiresIn = 3600,
        User = new UserDto
      {
      Id = user.Id,
            Email = user.Email,
     FullName = user.FullName,
        Roles = roles.ToList()
        }
    });
}
```

### 2.4 ??????? Token ?? Frontend

**auth.service.ts (Angular):**
```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

interface TokenResponse {
    accessToken: string;
    refreshToken: string;
  expiresIn: number;
    user: {
        id: string;
        email: string;
        fullName: string;
        roles: string[];
    };
}

@Injectable({ providedIn: 'root' })
export class AuthService {
    private baseUrl = 'https://api.gahar.sa/api/auth';
    private currentUserSubject: BehaviorSubject<any>;
    public currentUser: Observable<any>;

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<any>(
            JSON.parse(localStorage.getItem('currentUser') || 'null')
      );
        this.currentUser = this.currentUserSubject.asObservable();
  }

    public get currentUserValue() {
        return this.currentUserSubject.value;
    }

    public get token(): string {
      return this.currentUserValue?.accessToken || '';
    }

    register(email: string, password: string, confirmPassword: string, fullName: string) {
      return this.http.post<any>(`${this.baseUrl}/register`, {
            email,
            password,
 confirmPassword,
            fullName
        });
  }

    login(email: string, password: string): Observable<TokenResponse> {
      return this.http.post<TokenResponse>(`${this.baseUrl}/login`, { email, password })
            .pipe(
           tap(response => {
    // Store user details and jwt token in local storage
    localStorage.setItem('currentUser', JSON.stringify(response));
      localStorage.setItem('accessToken', response.accessToken);
             localStorage.setItem('refreshToken', response.refreshToken);
 this.currentUserSubject.next(response);
         })
   );
    }

    logout() {
        // Remove user from local storage
  localStorage.removeItem('currentUser');
        localStorage.removeItem('accessToken');
localStorage.removeItem('refreshToken');
        this.currentUserSubject.next(null);
    }

    refreshToken(): Observable<TokenResponse> {
    const refreshToken = localStorage.getItem('refreshToken');
        
        return this.http.post<TokenResponse>(`${this.baseUrl}/refresh-token`, { refreshToken })
            .pipe(
            tap(response => {
  localStorage.setItem('currentUser', JSON.stringify(response));
        localStorage.setItem('accessToken', response.accessToken);
     localStorage.setItem('refreshToken', response.refreshToken);
 this.currentUserSubject.next(response);
       })
   );
    }

    isAuthenticated(): boolean {
        return !!this.token;
 }

    hasRole(role: string): boolean {
        return this.currentUserValue?.user?.roles?.includes(role) || false;
    }
}
```

**auth.interceptor.ts (Angular):**
```typescript
import { Injectable } from '@angular/core';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor,
    HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, filter, take, switchMap } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    private isRefreshing = false;
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

    constructor(private authService: AuthService) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // Add authorization header with jwt token if available
        const token = this.authService.token;
        
      if (token) {
          request = this.addToken(request, token);
        }

        return next.handle(request).pipe(
            catchError(error => {
       if (error instanceof HttpErrorResponse && error.status === 401) {
  return this.handle401Error(request, next);
        } else {
       return throwError(() => error);
       }
    })
        );
    }

    private addToken(request: HttpRequest<any>, token: string) {
   return request.clone({
            setHeaders: {
    Authorization: `Bearer ${token}`
            }
      });
    }

    private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
        if (!this.isRefreshing) {
   this.isRefreshing = true;
            this.refreshTokenSubject.next(null);

            return this.authService.refreshToken().pipe(
          switchMap((response: any) => {
    this.isRefreshing = false;
       this.refreshTokenSubject.next(response.accessToken);
             return next.handle(this.addToken(request, response.accessToken));
      }),
      catchError((err) => {
    this.isRefreshing = false;
       this.authService.logout();
   return throwError(() => err);
          })
  );
        } else {
            return this.refreshTokenSubject.pipe(
         filter(token => token != null),
   take(1),
    switchMap(jwt => {
          return next.handle(this.addToken(request, jwt));
   })
  );
        }
    }
}
```

**auth.guard.ts (Angular):**
```typescript
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
  private authService: AuthService
    ) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (this.authService.isAuthenticated()) {
            // Check if route requires specific role
   const requiredRole = route.data['role'];
   
            if (requiredRole && !this.authService.hasRole(requiredRole)) {
       // Role not authorized, redirect to home
      this.router.navigate(['/']);
     return false;
 }

      // Authorized
   return true;
        }

        // Not logged in, redirect to login page with return url
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}
```

**??????? ?? Routes:**
```typescript
const routes: Routes = [
    {
        path: 'admin',
        canActivate: [AuthGuard],
        data: { role: 'Admin' },
        children: [
          { path: 'dashboard', component: DashboardComponent },
     { path: 'content', component: ContentManagementComponent },
    { path: 'users', component: UserManagementComponent }
        ]
    },
    {
        path: 'editor',
  canActivate: [AuthGuard],
        data: { role: 'Editor' },
      children: [
    { path: 'content', component: ContentEditorComponent }
  ]
    }
];
```

---

*??????? ???? ????? ?????? ?? ????? ??????...*

---

**??????:** ??? ??? ?? ??? ??????? ???????. ????? ???:
1. ? ???? ???? ?? Content Management Feature (?? Entity ??? Frontend)
2. ? ???? ???? ?? Authentication Flow (Register, Login, Refresh Token)

???? ????? ?????? ?? ??? ????? ??? ????? ?????? ?? ???????!

**?? ???? ?? ?????**
