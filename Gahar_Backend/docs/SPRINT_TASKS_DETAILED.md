# ?? ????? ?????? ??????? - ???? ????

**?????:** ????? ???? ??? ???? ????? ????? ??????? ?????

---

## ?? Sprint 0: ????? ??????

### ????? 1: ????? Solution Structure (4 ?????)

#### Developer 1 (????) - Morning Session
**??????: ????? Solution ? Projects**
```bash
# ???????:
1. ????? Solution
   dotnet new sln -n GAHAR.CMS

2. ????? Projects
   dotnet new webapi -n GAHAR.API
   dotnet new classlib -n GAHAR.Core
   dotnet new classlib -n GAHAR.Infrastructure
   dotnet new classlib -n GAHAR.Shared

3. ????? Projects ??? Solution
   dotnet sln add GAHAR.API/GAHAR.API.csproj
   dotnet sln add GAHAR.Core/GAHAR.Core.csproj
   dotnet sln add GAHAR.Infrastructure/GAHAR.Infrastructure.csproj
   dotnet sln add GAHAR.Shared/GAHAR.Shared.csproj

4. ????? Project References
   cd GAHAR.API
   dotnet add reference ../GAHAR.Core/GAHAR.Core.csproj
   dotnet add reference ../GAHAR.Infrastructure/GAHAR.Infrastructure.csproj
   dotnet add reference ../GAHAR.Shared/GAHAR.Shared.csproj
   
   cd ../GAHAR.Infrastructure
   dotnet add reference ../GAHAR.Core/GAHAR.Core.csproj
   dotnet add reference ../GAHAR.Shared/GAHAR.Shared.csproj
   
   cd ../GAHAR.Core
   dotnet add reference ../GAHAR.Shared/GAHAR.Shared.csproj
```

**?????:** 1 ????  
**??????:** Solution Structure ????

---

#### Developer 1 (????) - Afternoon Session
**??????: ????? Packages ????????**

**GAHAR.API Packages:**
```bash
cd GAHAR.API
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Sinks.Console
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add package FluentValidation.AspNetCore
dotnet add package StackExchange.Redis
```

**GAHAR.Core Packages:**
```bash
cd ../GAHAR.Core
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

**GAHAR.Infrastructure Packages:**
```bash
cd ../GAHAR.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
# OR
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Hangfire.AspNetCore
dotnet add package Hangfire.SqlServer
dotnet add package RabbitMQ.Client
dotnet add package Azure.Storage.Blobs
dotnet add package SixLabors.ImageSharp
dotnet add package ClosedXML
```

**?????:** 1 ????  
**??????:** ???? Packages ?????

---

#### Developer 2 (????) - Morning Session
**??????: ????? Git Repository**

```bash
# 1. Initialize Git
git init
git remote add origin https://github.com/gahar/cms-backend.git

# 2. ????? .gitignore
# (??? ????? .gitignore ??? .NET)

# 3. ????? Branch Structure
git checkout -b develop
git checkout -b feature/database-setup
git checkout -b feature/authentication

# 4. Initial Commit
git checkout develop
git add .
git commit -m "chore: initial project setup"
git push -u origin develop
```

**????? CONTRIBUTING.md:**
```markdown
# Contribution Guidelines

## Branching Strategy
- `main` - Production
- `develop` - Development
- `feature/*` - New features
- `bugfix/*` - Bug fixes
- `hotfix/*` - Production hotfixes

## Commit Message Format
- `feat:` - New feature
- `fix:` - Bug fix
- `docs:` - Documentation
- `refactor:` - Code refactoring
- `test:` - Tests
- `chore:` - Build/config changes

## Pull Request Rules
1. All tests must pass
2. Code review required
3. No merge conflicts
4. Documentation updated
```

**?????:** 1.5 ????  
**??????:** Git Repository ????

---

#### Developer 2 (????) - Afternoon Session
**??????: ????? Database**

**appsettings.Development.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GAHAR_CMS;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
  "Microsoft.AspNetCore": "Warning"
    }
  },
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyForDevelopment123!",
    "Issuer": "https://localhost:7001",
    "Audience": "https://localhost:7001",
    "ExpiryMinutes": 60
  }
}
```

**????? Docker Compose:**
```yaml
version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong@Password123
    ports:
- "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data

  rabbitmq:
    image: rabbitmq:3-management
    ports:
      - "5672:5672"
      - "15672:15672"
    volumes:
 - rabbitmq_data:/var/lib/rabbitmq

volumes:
  sqlserver_data:
  redis_data:
  rabbitmq_data:
```

```bash
# ????? Docker
docker-compose up -d

# ?????? ?? ???????
docker ps
```

**?????:** 1.5 ????  
**??????:** Database ????

---

### ????? 2: ????? Program.cs ? Middleware (6 ?????)

#### Developer 1 (????)
**??????: ????? Program.cs ??????**

**GAHAR.API/Program.cs:**
```csharp
using Microsoft.EntityFrameworkCore;
using Serilog;
using GAHAR.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// ===== 1. Serilog Configuration =====
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/gahar-.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();

builder.Host.UseSerilog();

// ===== 2. Database =====
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ===== 3. Identity =====
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// ===== 4. Authentication =====
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
  ValidIssuer = builder.Configuration["Jwt:Issuer"],
   ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
 Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
});

// ===== 5. Authorization Policies =====
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanPublishContent", policy =>
 policy.RequireRole("SuperAdmin", "Admin"));
    
    options.AddPolicy("CanManageUsers", policy =>
        policy.RequireRole("SuperAdmin"));

    options.AddPolicy("CanEditContent", policy =>
        policy.RequireRole("SuperAdmin", "Admin", "Editor"));
});

// ===== 6. CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
        builder.WithOrigins("http://localhost:3000")
      .AllowAnyMethod()
       .AllowAnyHeader()
      .AllowCredentials());
});

// ===== 7. Redis Cache =====
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
});

// ===== 8. AutoMapper =====
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ===== 9. FluentValidation =====
builder.Services.AddValidatorsFromAssemblyContaining<CreateContentRequestValidator>();

// ===== 10. Services Registration =====
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IMediaService, MediaService>();

// ===== 11. Controllers =====
builder.Services.AddControllers();

// ===== 12. Swagger =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
 Title = "GAHAR CMS API", 
        Version = "v1",
        Description = "GAHAR Content Management System API"
    });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
     Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
 Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
   new OpenApiSecurityScheme
            {
 Reference = new OpenApiReference
   {
         Type = ReferenceType.SecurityScheme,
       Id = "Bearer"
       }
      },
            Array.Empty<string>()
   }
    });
});

var app = builder.Build();

// ===== Middleware Pipeline =====

// 1. Swagger (Development only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 2. HTTPS Redirection
app.UseHttpsRedirection();

// 3. CORS
app.UseCors("AllowFrontend");

// 4. Authentication
app.UseAuthentication();

// 5. Authorization
app.UseAuthorization();

// 6. Controllers
app.MapControllers();

// 7. Run
app.Run();
```

**?????:** 3 ?????  
**??????:** Program.cs ????

---

#### Developer 2 (????)
**??????: ????? Middleware**

**1. GlobalExceptionMiddleware.cs:**
```csharp
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
      _logger = logger;
    }

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
        message = notFoundEx.Message,
                details = notFoundEx.ToString()
      },
    ValidationException validationEx => new
  {
   statusCode = StatusCodes.Status400BadRequest,
              message = validationEx.Message,
      errors = validationEx.Errors
   },
            UnauthorizedException unauthorizedEx => new
          {
     statusCode = StatusCodes.Status401Unauthorized,
           message = unauthorizedEx.Message
          },
_ => new
        {
 statusCode = StatusCodes.Status500InternalServerError,
      message = "An internal server error occurred",
     details = exception.Message
        }
        };

      context.Response.StatusCode = response.statusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}
```

**2. RequestLoggingMiddleware.cs:**
```csharp
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
    RequestDelegate next,
   ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
     var stopwatch = Stopwatch.StartNew();
        
        _logger.LogInformation(
     "Request: {Method} {Path} from {IP}",
            context.Request.Method,
          context.Request.Path,
   context.Connection.RemoteIpAddress);

        await _next(context);

        stopwatch.Stop();

   _logger.LogInformation(
     "Response: {StatusCode} in {ElapsedMilliseconds}ms",
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds);
    }
}
```

**3. LanguageMiddleware.cs:**
```csharp
public class LanguageMiddleware
{
    private readonly RequestDelegate _next;

    public LanguageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // ??????? ????? ?? Query String ?? Header
        var language = context.Request.Query["lang"].FirstOrDefault() 
    ?? context.Request.Headers["Accept-Language"].FirstOrDefault() 
   ?? "ar";

        // ?????? ?? ????? ????????
  if (language != "ar" && language != "en")
            language = "ar";

        // ??? ????? ?? HttpContext
   context.Items["Language"] = language;

        await _next(context);
  }
}
```

**????? ??? Program.cs:**
```csharp
// ??? app.UseHttpsRedirection()
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<LanguageMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
```

**?????:** 3 ?????  
**??????:** Middleware Layer ????

---

### ????? 3: CI/CD ? Testing Setup (6 ?????)

#### Developer 1 (????)
**??????: ????? GitHub Actions**

**.github/workflows/dotnet-ci.yml:**
```yaml
name: .NET CI/CD

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    
 services:
      sqlserver:
   image: mcr.microsoft.com/mssql/server:2022-latest
        env:
          ACCEPT_EULA: Y
          SA_PASSWORD: Test@Password123
  ports:
    - 1433:1433
        options: >-
  --health-cmd "/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Test@Password123 -Q 'SELECT 1'"
          --health-interval 10s
          --health-timeout 5s
    --health-retries 5

    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
   with:
     dotnet-version: 8.0.x
    
    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --no-restore --configuration Release
    
    - name: Test
    run: dotnet test --no-build --configuration Release --verbosity normal --collect:"XPlat Code Coverage"
    
    - name: Upload coverage reports
      uses: codecov/codecov-action@v3
      with:
        files: '**/coverage.cobertura.xml'
   fail_ci_if_error: true

  code-quality:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
   uses: actions/setup-dotnet@v3
      with:
  dotnet-version: 8.0.x
    
    - name: Install dotnet format
    run: dotnet tool install -g dotnet-format
    
    - name: Run dotnet format
      run: dotnet format --verify-no-changes --verbosity diagnostic

  security-scan:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Run security scan
      uses: securego/gosec@master
      with:
        args: '--exclude-dir=vendor'
```

**?????:** 2 ?????  
**??????:** CI Pipeline ????

---

#### Developer 2 (????)
**??????: ????? Unit Testing**

**????? Test Project:**
```bash
dotnet new xunit -n GAHAR.Tests.Unit
cd GAHAR.Tests.Unit
dotnet add reference ../GAHAR.Core/GAHAR.Core.csproj
dotnet add reference ../GAHAR.Infrastructure/GAHAR.Infrastructure.csproj

# Packages
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package coverlet.collector
```

**???? Test:**
```csharp
public class ContentServiceTests
{
    private readonly Mock<IContentRepository> _contentRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ContentService _sut;

    public ContentServiceTests()
    {
        _contentRepositoryMock = new Mock<IContentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    _mapperMock = new Mock<IMapper>();
      
    _unitOfWorkMock.Setup(u => u.ContentRepository)
    .Returns(_contentRepositoryMock.Object);

        _sut = new ContentService(
      _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task CreateContentAsync_ShouldGenerateSlug_WhenSlugNotProvided()
    {
        // Arrange
        var request = new CreateContentRequest
        {
   DefaultLanguage = "ar",
    Translations = new Dictionary<string, TranslationDto>
            {
       ["ar"] = new TranslationDto 
        { 
           Title = "????? ????",
  Body = "????? ?????"
  }
            }
      };

        var content = new Content { Id = Guid.NewGuid() };
        
        _mapperMock.Setup(m => m.Map<Content>(It.IsAny<CreateContentRequest>()))
         .Returns(content);

        // Act
        var result = await _sut.CreateContentAsync("news", request);

        // Assert
        result.Should().NotBeNull();
   _contentRepositoryMock.Verify(
  r => r.AddAsync(It.IsAny<Content>()), 
            Times.Once);
        _unitOfWorkMock.Verify(
          u => u.SaveChangesAsync(), 
     Times.Once);
    }
}
```

**?????:** 3 ?????  
**??????:** Testing Framework ????

---

## ????? Sprint 1: Foundation

### ????? 1: Database Entities (Developer 1)

**??????: ????? ???? Entities**

**GAHAR.Core/Entities/Content.cs:**
```csharp
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
    public ICollection<ContentTranslation> Translations { get; set; }
}
```

**GAHAR.Core/Entities/ContentTranslation.cs:**
```csharp
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
    public string? MetaKeywords { get; set; }
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
    public string? TwitterCard { get; set; }
    public string? FieldsData { get; set; } // JSON
    public Guid? LayoutId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation Properties
    public Content Content { get; set; }
    public Layout Layout { get; set; }
}
```

**????? ????? Entities (10 entities):**
- ContentType
- Page
- PageTranslation
- Facility
- FacilityTranslation
- Certificate
- Form
- FormField
- FormSubmission
- MediaFile
- Album
- AlbumImage
- Menu
- MenuItem
- Layout
- LayoutField

**?????:** 6 ?????  
**??????:** ???? Entities ?????

---

### ????? 2: EF Core Configurations (Developer 1)

**??????: ????? Fluent API Configurations**

**GAHAR.Infrastructure/Data/Configurations/ContentConfiguration.cs:**
```csharp
public class ContentConfiguration : IEntityTypeConfiguration<Content>
{
    public void Configure(EntityTypeBuilder<Content> builder)
    {
        builder.ToTable("Content");
        
     builder.HasKey(c => c.Id);
        
        builder.Property(c => c.IsPublished)
            .HasDefaultValue(false);
        
   builder.Property(c => c.CreatedAt)
         .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(c => c.UpdatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
        
        // Relationships
        builder.HasOne(c => c.ContentType)
      .WithMany()
        .HasForeignKey(c => c.ContentTypeId)
          .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(c => c.CreatedBy)
            .WithMany()
        .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(c => c.Translations)
            .WithOne(t => t.Content)
            .HasForeignKey(t => t.ContentId)
  .OnDelete(DeleteBehavior.Cascade);
      
      // Indexes
      builder.HasIndex(c => c.ContentTypeId);
        builder.HasIndex(c => c.IsPublished);
        builder.HasIndex(c => c.PublishedAt);
    }
}
```

**GAHAR.Infrastructure/Data/AppDbContext.cs:**
```csharp
public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Content> Content { get; set; }
    public DbSet<ContentTranslation> ContentTranslations { get; set; }
    public DbSet<ContentType> ContentTypes { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<PageTranslation> PageTranslations { get; set; }
    public DbSet<Facility> Facilities { get; set; }
    public DbSet<FacilityTranslation> FacilityTranslations { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
    public DbSet<Form> Forms { get; set; }
    public DbSet<FormField> FormFields { get; set; }
    public DbSet<FormSubmission> FormSubmissions { get; set; }
    public DbSet<MediaFile> MediaFiles { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<AlbumImage> AlbumImages { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Layout> Layouts { get; set; }
    public DbSet<LayoutField> LayoutFields { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
 base.OnModelCreating(builder);
        
        // Apply all configurations
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
```

**????? Migration:**
```bash
cd GAHAR.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../GAHAR.API
dotnet ef database update --startup-project ../GAHAR.API
```

**?????:** 6 ?????  
**??????:** Database Schema ????

---

### ????? 3-4: Repository Pattern (Developer 1)

**??????: ????? Repository Pattern**

**GAHAR.Core/Interfaces/IRepository.cs:**
```csharp
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
```

**GAHAR.Infrastructure/Repositories/GenericRepository.cs:**
```csharp
public class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
 protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
_dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(Guid id)
    {
var entity = await GetByIdAsync(id);
        if (entity != null)
   {
  _dbSet.Remove(entity);
        }
  }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbSet.FindAsync(id) != null;
    }
}
```

**GAHAR.Core/Interfaces/IContentRepository.cs:**
```csharp
public interface IContentRepository : IRepository<Content>
{
    Task<Content?> GetBySlugAsync(string slug, string language);
    Task<IEnumerable<Content>> GetPublishedContentAsync(
        string contentType, 
     string language, 
 int page, 
        int pageSize);
    Task<int> GetTotalCountAsync(string contentType);
}
```

**GAHAR.Infrastructure/Repositories/ContentRepository.cs:**
```csharp
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
   .FirstOrDefaultAsync(c => c.Translations.Any(t => t.Slug == slug && t.Language == language));
    }

    public async Task<IEnumerable<Content>> GetPublishedContentAsync(
        string contentType, 
        string language, 
        int page, 
        int pageSize)
    {
        return await _context.Content
     .Where(c => c.ContentType.Name == contentType && c.IsPublished)
        .Include(c => c.Translations.Where(t => t.Language == language))
            .OrderByDescending(c => c.PublishedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
   .AsNoTracking()
            .ToListAsync();
  }

    public async Task<int> GetTotalCountAsync(string contentType)
    {
        return await _context.Content
  .CountAsync(c => c.ContentType.Name == contentType && c.IsPublished);
    }
}
```

**GAHAR.Core/Interfaces/IUnitOfWork.cs:**
```csharp
public interface IUnitOfWork : IDisposable
{
    IContentRepository ContentRepository { get; }
    IPageRepository PageRepository { get; }
  IFacilityRepository FacilityRepository { get; }
    ICertificateRepository CertificateRepository { get; }
    IFormRepository FormRepository { get; }
    IMediaRepository MediaRepository { get; }
    IAlbumRepository AlbumRepository { get; }
    IMenuRepository MenuRepository { get; }
    ILayoutRepository LayoutRepository { get; }
    
    Task<int> SaveChangesAsync();
}
```

**GAHAR.Infrastructure/Repositories/UnitOfWork.cs:**
```csharp
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    
    private IContentRepository? _contentRepository;
    private IPageRepository? _pageRepository;
    // ... other repositories

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IContentRepository ContentRepository =>
        _contentRepository ??= new ContentRepository(_context);

    public IPageRepository PageRepository =>
        _pageRepository ??= new PageRepository(_context);

    // ... other repository properties

    public async Task<int> SaveChangesAsync()
    {
    return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
```

**?????:** 12 ????? (?????)  
**??????:** Repository Layer ????

---

### ????? 5: Testing (Developer 1)

**??????: Unit Tests ??? Repositories**

**???? Test:**
```csharp
public class ContentRepositoryTests
{
private readonly AppDbContext _context;
    private readonly ContentRepository _sut;

    public ContentRepositoryTests()
    {
 var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new AppDbContext(options);
        _sut = new ContentRepository(_context);
    }

    [Fact]
    public async Task GetBySlugAsync_ShouldReturnContent_WhenSlugExists()
    {
        // Arrange
        var content = new Content
        {
  Id = Guid.NewGuid(),
       ContentTypeId = Guid.NewGuid(),
            IsPublished = true,
     CreatedById = "user-1",
  CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
      var translation = new ContentTranslation
 {
      Id = Guid.NewGuid(),
     ContentId = content.Id,
       Language = "ar",
    Slug = "test-slug",
  Title = "Test Title",
    Body = "Test Body",
        CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
    };
  
        await _context.Content.AddAsync(content);
     await _context.ContentTranslations.AddAsync(translation);
      await _context.SaveChangesAsync();

        // Act
     var result = await _sut.GetBySlugAsync("test-slug", "ar");

      // Assert
result.Should().NotBeNull();
        result!.Id.Should().Be(content.Id);
        result.Translations.Should().ContainSingle();
  result.Translations.First().Slug.Should().Be("test-slug");
    }

    [Fact]
    public async Task GetPublishedContentAsync_ShouldReturnOnlyPublished()
    {
        // Arrange
      // ????? ????? ????? ???? ?????
  
 // Act
        var result = await _sut.GetPublishedContentAsync("news", "ar", 1, 10);

     // Assert
        result.Should().NotBeEmpty();
        result.Should().OnlyContain(c => c.IsPublished);
    }
}
```

**?????:** 6 ?????  
**??????:** Tests Coverage > 80%

---

### ????? 1-2: Authentication (Developer 2)

**??????: ????? ASP.NET Core Identity + JWT**

**GAHAR.Core/Entities/ApplicationUser.cs:**
```csharp
public class ApplicationUser : IdentityUser
{
  public string FullName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; }
}
```

**GAHAR.Infrastructure/Identity/JwtTokenGenerator.cs:**
```csharp
public class JwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
          new(ClaimTypes.NameIdentifier, user.Id),
      new(ClaimTypes.Email, user.Email!),
    new(ClaimTypes.Name, user.FullName),
      new("fullName", user.FullName)
        };

        // ????? Roles ?? Claims
   claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
    var token = new JwtSecurityToken(
   issuer: _configuration["Jwt:Issuer"],
         audience: _configuration["Jwt:Audience"],
            claims: claims,
     expires: DateTime.UtcNow.AddMinutes(
        int.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
   signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
  }
}
```

**GAHAR.API/Controllers/AuthController.cs:**
```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtTokenGenerator _tokenGenerator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<ApplicationUser> userManager,
 SignInManager<ApplicationUser> signInManager,
    JwtTokenGenerator tokenGenerator,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenGenerator = tokenGenerator;
        _logger = logger;
    }

    [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
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
       return BadRequest(new { errors = result.Errors });
        }

        // ????? Role ???????
        await _userManager.AddToRoleAsync(user, "Viewer");

        _logger.LogInformation("User {Email} registered successfully", user.Email);

return Ok(new
        {
            userId = user.Id,
       email = user.Email,
  message = "User registered successfully"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
   var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
      {
        return Unauthorized(new { message = "Invalid email or password" });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(
      user, request.Password, lockoutOnFailure: true);

 if (!result.Succeeded)
        {
      if (result.IsLockedOut)
          {
       return Unauthorized(new { message = "Account locked. Try again later." });
            }
            
            return Unauthorized(new { message = "Invalid email or password" });
 }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenGenerator.GenerateToken(user, roles);
        var refreshToken = _tokenGenerator.GenerateRefreshToken();

        // ??? Refresh Token ?? Database (TODO)

        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        _logger.LogInformation("User {Email} logged in successfully", user.Email);

   return Ok(new
        {
     accessToken,
  refreshToken,
            expiresIn = 3600,
    tokenType = "Bearer",
            user = new
      {
              id = user.Id,
email = user.Email,
              fullName = user.FullName,
         roles
     }
        });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
      // ????? Refresh Token (TODO)
        
        await _signInManager.SignOutAsync();
        
      _logger.LogInformation("User logged out");
        
      return Ok(new { message = "Logged out successfully" });
    }
}
```

**?????:** 12 ????? (?????)  
**??????:** Authentication System ????

---

*???? ???? ???????? ??????? ????? Sprints...*

---

## ?? ??????? ????:

### ????? ?????????:
1. **Day 1-2:** Setup & Foundation ? **????**
2. **Day 3-10:** Core Features ? **?????? ?????**
3. **Day 11-15:** Advanced Features ? **???**
4. **Day 16-20:** Optimization & Testing ? **?????**

### ????? ??????:
- ? **Daily Standup:** ?? ?????? ?????
- ? **Git Commits:** ????? ???????
- ? **Code Review:** ????
- ? **Tests:** ???? ?? ?????? ?? ????
- ? **Documentation:** ???? ???????

### ??? ??????:
1. ??? ????? (???? / ??????)
2. ??? ????? ??? Scope
3. ???? Features ??? ????
4. ?? ????? ????? (?? ???? 5 ????? ??????/?????)

---

**??? ????? ????? ??? ???????? ??????? ???? 5 ????. ??? ??????? ?? ??????? ????? ??? ???? Sprints.**

**??????:** ??? ?????? ???? ??????? ??? ???? ?????? ????????? ???? ????.

**Good Luck! ??**
