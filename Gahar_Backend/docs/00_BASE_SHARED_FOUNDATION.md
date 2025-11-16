# 🏗️ الجزء المشترك الأساسي (Base Foundation)
## يتم تطويره قبل بدء المطورين في الـ Features

**تاريخ الإنشاء:** 11 نوفمبر 2025  
**المدة المتوقعة:** 1-2 أسبوع  
**الأولوية:** 🔴 حرجة - يجب إكمالها أولاً

---

## 📋 جدول المحتويات
1. [البنية التحتية الأساسية](#1-البنية-التحتية-الأساسية)
2. [قاعدة البيانات الأساسية](#2-قاعدة-البيانات-الأساسية)
3. [نظام المصادقة والتفويض](#3-نظام-المصادقة-والتفويض)
4. [خدمات مشتركة](#4-خدمات-مشتركة)
5. [Middleware والـ Filters](#5-middleware-والـ-filters)
6. [التكوينات العامة](#6-التكوينات-العامة)

---

## 1. البنية التحتية الأساسية

### 1.1 هيكل المشروع (Project Structure)

```
Gahar_Backend/
├── Controllers/           # API Controllers
├── Models/               # Domain Models & Entities
│   ├── Entities/         # Database Entities
│   ├── DTOs/            # Data Transfer Objects
│   └── ViewModels/      # Response Models
├── Data/                 # Database Context & Configurations
│   ├── ApplicationDbContext.cs
│   ├── Configurations/  # Entity Configurations
│   └── Migrations/      # EF Migrations
├── Services/            # Business Logic
│   ├── Interfaces/      # Service Interfaces
│   └── Implementations/ # Service Implementations
├── Repositories/        # Data Access Layer
│   ├── Interfaces/
│   └── Implementations/
├── Middleware/          # Custom Middleware
├── Filters/             # Action Filters
├── Utilities/           # Helper Classes
├── Constants/           # Constants & Enums
└── Extensions/          # Extension Methods
```

**المهام:**
- ✅ إنشاء هيكل المجلدات
- ✅ إعداد `.editorconfig` للـ coding standards
- ✅ إعداد `appsettings.json` و `appsettings.Development.json`

---

### 1.2 إعداد قاعدة البيانات (Database Setup)

**الملف:** `Data/ApplicationDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;

namespace Gahar_Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Base Tables (Common for all features)
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Translation> Translations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations from assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Global Query Filters
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            
            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Languages
            modelBuilder.Entity<Language>().HasData(
                new Language { Id = 1, Code = "ar", Name = "العربية", IsDefault = true, IsActive = true },
                new Language { Id = 2, Code = "en", Name = "English", IsDefault = false, IsActive = true }
            );

            // Seed Super Admin Role
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "SuperAdmin", DisplayName = "مدير النظام", CreatedAt = DateTime.UtcNow }
            );
        }

        // Override SaveChanges for automatic audit
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
                else
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
```

---

## 2. قاعدة البيانات الأساسية

### 2.1 Base Entity

**الملف:** `Models/Entities/BaseEntity.cs`

```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}
```

### 2.2 Translatable Entity

**الملف:** `Models/Entities/TranslatableEntity.cs`

```csharp
public abstract class TranslatableEntity : BaseEntity
{
    // Navigation property for translations
    public ICollection<Translation> Translations { get; set; } = new List<Translation>();
}
```

### 2.3 نماذج المصادقة والتفويض

**الملف:** `Models/Entities/User.cs`

```csharp
public class User : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Username { get; set; }

    [Required]
    [StringLength(200)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;

    public bool EmailConfirmed { get; set; } = false;

    public DateTime? LastLoginAt { get; set; }

    public int FailedLoginAttempts { get; set; } = 0;

    public DateTime? LockedUntil { get; set; }

    // Navigation Properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
```

**الملف:** `Models/Entities/Role.cs`

```csharp
public class Role : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } // SuperAdmin, Admin, Editor, etc.

    [Required]
    [StringLength(100)]
    public string DisplayName { get; set; } // Display name in UI

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsSystemRole { get; set; } = false; // Can't be deleted

    // Navigation Properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
```

**الملف:** `Models/Entities/Permission.cs`

```csharp
public class Permission : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } // e.g., "ContentTypes.Create", "Pages.Edit"

    [Required]
    [StringLength(50)]
    public string Module { get; set; } // e.g., "ContentTypes", "Pages", "Forms"

    [StringLength(500)]
    public string? Description { get; set; }

    // Navigation Properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
```

**الملف:** `Models/Entities/UserRole.cs`

```csharp
public class UserRole : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; }

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
```

**الملف:** `Models/Entities/RolePermission.cs`

```csharp
public class RolePermission : BaseEntity
{
    public int RoleId { get; set; }
    public Role Role { get; set; }

    public int PermissionId { get; set; }
    public Permission Permission { get; set; }

    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
}
```

### 2.4 نظام التدقيق (Audit Log)

**الملف:** `Models/Entities/AuditLog.cs`

```csharp
public class AuditLog : BaseEntity
{
    public int? UserId { get; set; }
    public User? User { get; set; }

    [Required]
    [StringLength(50)]
    public string Action { get; set; } // Create, Update, Delete, Login, etc.

    [Required]
    [StringLength(100)]
    public string EntityType { get; set; } // Content, Page, Form, etc.

    public int? EntityId { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public string? OldValues { get; set; } // JSON

    public string? NewValues { get; set; } // JSON

    [StringLength(45)]
    public string? IpAddress { get; set; }

    [StringLength(500)]
    public string? UserAgent { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
```

### 2.5 نظام الترجمة

**الملف:** `Models/Entities/Language.cs`

```csharp
public class Language : BaseEntity
{
    [Required]
    [StringLength(5)]
    public string Code { get; set; } // ar, en

    [Required]
    [StringLength(100)]
    public string Name { get; set; } // العربية, English

    public bool IsDefault { get; set; } = false;

    public bool IsActive { get; set; } = true;

    [StringLength(10)]
    public string? Direction { get; set; } = "ltr"; // ltr, rtl

    // Navigation Properties
    public ICollection<Translation> Translations { get; set; } = new List<Translation>();
}
```

**الملف:** `Models/Entities/Translation.cs`

```csharp
public class Translation : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string EntityType { get; set; } // ContentType, Content, Page, etc.

    public int EntityId { get; set; }

    [Required]
    [StringLength(50)]
    public string FieldName { get; set; } // Title, Description, etc.

    public int LanguageId { get; set; }
    public Language Language { get; set; }

    [Required]
    public string Value { get; set; } // The translated text
}
```

---

## 3. نظام المصادقة والتفويض

### 3.1 JWT Configuration

**الملف:** `appsettings.json`

```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyHere_MinLength32Characters!",
    "Issuer": "GaharBackend",
    "Audience": "GaharClients",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

### 3.2 JWT Service

**الملف:** `Services/Interfaces/IJwtService.cs`

```csharp
public interface IJwtService
{
    string GenerateAccessToken(User user, IEnumerable<string> roles, IEnumerable<string> permissions);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateToken(string token);
}
```

**الملف:** `Services/Implementations/JwtService.cs`

```csharp
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(User user, IEnumerable<string> roles, IEnumerable<string> permissions)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("FirstName", user.FirstName ?? ""),
            new Claim("LastName", user.LastName ?? "")
        };

        // Add roles
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Add permissions
        foreach (var permission in permissions)
        {
            claims.Add(new Claim("Permission", permission));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JwtSettings:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
```

### 3.3 Authentication Service

**الملف:** `Services/Interfaces/IAuthService.cs`

```csharp
public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    Task<bool> LogoutAsync(int userId);
    Task<bool> RegisterAsync(RegisterDto registerDto);
    Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}
```

**الملف:** `Services/Implementations/AuthService.cs`

```csharp
using BCrypt.Net;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IAuditLogService _auditLogService;

    public AuthService(
        IUserRepository userRepository,
        IJwtService jwtService,
        IAuditLogService auditLogService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _auditLogService = auditLogService;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);

        if (user == null)
            throw new UnauthorizedException("Invalid email or password");

        // Check if account is locked
        if (user.LockedUntil.HasValue && user.LockedUntil > DateTime.UtcNow)
            throw new UnauthorizedException($"Account is locked until {user.LockedUntil}");

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            // Increment failed attempts
            user.FailedLoginAttempts++;
            
            if (user.FailedLoginAttempts >= 5)
            {
                user.LockedUntil = DateTime.UtcNow.AddMinutes(30);
            }

            await _userRepository.UpdateAsync(user);

            throw new UnauthorizedException("Invalid email or password");
        }

        // Reset failed attempts
        user.FailedLoginAttempts = 0;
        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Get roles and permissions
        var roles = await _userRepository.GetUserRolesAsync(user.Id);
        var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);

        // Generate tokens
        var accessToken = _jwtService.GenerateAccessToken(user, roles, permissions);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Save refresh token (implement RefreshToken entity if needed)

        // Log audit
        await _auditLogService.LogAsync(user.Id, "Login", "User", user.Id, "User logged in successfully");

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList()
            }
        };
    }

    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        // Check if user exists
        if (await _userRepository.ExistsByEmailAsync(registerDto.Email))
            throw new BadRequestException("Email already exists");

        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PhoneNumber = registerDto.PhoneNumber
        };

        await _userRepository.CreateAsync(user);

        // Assign default role (e.g., Viewer)
        await _userRepository.AssignRoleAsync(user.Id, "Viewer");

        return true;
    }

    // Implement other methods...
}
```

---

## 4. خدمات مشتركة

### 4.1 File Upload Service

**الملف:** `Services/Interfaces/IFileUploadService.cs`

```csharp
public interface IFileUploadService
{
    Task<string> UploadImageAsync(IFormFile file, string folder = "images");
    Task<string> UploadFileAsync(IFormFile file, string folder = "files");
    Task<bool> DeleteFileAsync(string filePath);
    Task<string> ConvertToWebPAsync(string imagePath);
}
```

### 4.2 Email Service

**الملف:** `Services/Interfaces/IEmailService.cs`

```csharp
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendTemplateEmailAsync(string to, string templateName, object model);
}
```

### 4.3 Cache Service

**الملف:** `Services/Interfaces/ICacheService.cs`

```csharp
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
    Task RemoveByPatternAsync(string pattern);
}
```

### 4.4 Translation Service

**الملف:** `Services/Interfaces/ITranslationService.cs`

```csharp
public interface ITranslationService
{
    Task<string> GetTranslationAsync(string entityType, int entityId, string fieldName, string languageCode);
    Task SaveTranslationAsync(string entityType, int entityId, string fieldName, string languageCode, string value);
    Task<Dictionary<string, string>> GetAllTranslationsAsync(string entityType, int entityId, string languageCode);
}
```

### 4.5 Audit Log Service

**الملف:** `Services/Interfaces/IAuditLogService.cs`

```csharp
public interface IAuditLogService
{
    Task LogAsync(int? userId, string action, string entityType, int? entityId, string? description = null, object? oldValues = null, object? newValues = null);
    Task<IEnumerable<AuditLog>> GetLogsAsync(AuditLogFilterDto filter);
}
```

---

## 5. Middleware والـ Filters

### 5.1 Exception Handling Middleware

**الملف:** `Middleware/ExceptionHandlingMiddleware.cs`

```csharp
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            ForbiddenException => StatusCodes.Status403Forbidden,
            NotFoundException => StatusCodes.Status404NotFound,
            BadRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = new
        {
            StatusCode = statusCode,
            Message = exception.Message,
            Details = exception.InnerException?.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsJsonAsync(response);
    }
}
```

### 5.2 Permission Authorization Attribute

**الملف:** `Filters/PermissionAttribute.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class PermissionAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _permission;

    public PermissionAttribute(string permission)
    {
        _permission = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var permissions = user.Claims
            .Where(c => c.Type == "Permission")
            .Select(c => c.Value)
            .ToList();

        if (!permissions.Contains(_permission))
        {
            context.Result = new ForbidResult();
        }
    }
}
```

### 5.3 Audit Log Action Filter

**الملف:** `Filters/AuditLogActionFilter.cs`

```csharp
public class AuditLogActionFilter : IAsyncActionFilter
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogActionFilter(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executedContext = await next();

        if (executedContext.Exception == null)
        {
            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var action = context.ActionDescriptor.DisplayName;

            await _auditLogService.LogAsync(
                userId != null ? int.Parse(userId) : null,
                action ?? "Unknown",
                "Action",
                null,
                $"Action executed: {action}"
            );
        }
    }
}
```

---

## 6. التكوينات العامة

### 6.1 Program.cs Configuration

**الملف:** `Program.cs`

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
        };
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://gahar.sa")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Register Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<ITranslationService, TranslationService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();

// Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
```

### 6.2 Connection String

**الملف:** `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=GaharDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## 7. Custom Exceptions

**الملف:** `Utilities/Exceptions/CustomExceptions.cs`

```csharp
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }
}

public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}
```

---

## 8. DTOs (Data Transfer Objects)

### 8.1 Authentication DTOs

**الملف:** `Models/DTOs/Auth/LoginDto.cs`

```csharp
public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class RegisterDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
}

public class AuthResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<string> Roles { get; set; }
}
```

---

## 9. Repository Pattern

### 9.1 Generic Repository

**الملف:** `Repositories/Interfaces/IGenericRepository.cs`

```csharp
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
```

**الملف:** `Repositories/Implementations/GenericRepository.cs`

```csharp
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return false;

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.AnyAsync(e => e.Id == id);
    }
}
```

---

## 10. Constants & Enums

**الملف:** `Constants/Permissions.cs`

```csharp
public static class Permissions
{
    // Content Types
    public const string ContentTypesView = "ContentTypes.View";
    public const string ContentTypesCreate = "ContentTypes.Create";
    public const string ContentTypesEdit = "ContentTypes.Edit";
    public const string ContentTypesDelete = "ContentTypes.Delete";

    // Content
    public const string ContentView = "Content.View";
    public const string ContentCreate = "Content.Create";
    public const string ContentEdit = "Content.Edit";
    public const string ContentDelete = "Content.Delete";
    public const string ContentPublish = "Content.Publish";

    // Pages
    public const string PagesView = "Pages.View";
    public const string PagesCreate = "Pages.Create";
    public const string PagesEdit = "Pages.Edit";
    public const string PagesDelete = "Pages.Delete";

    // Forms
    public const string FormsView = "Forms.View";
    public const string FormsCreate = "Forms.Create";
    public const string FormsEdit = "Forms.Edit";
    public const string FormsDelete = "Forms.Delete";

    // Users
    public const string UsersView = "Users.View";
    public const string UsersCreate = "Users.Create";
    public const string UsersEdit = "Users.Edit";
    public const string UsersDelete = "Users.Delete";
}
```

---

## ✅ Checklist للجزء المشترك

### البنية التحتية
- [ ] إنشاء هيكل المشروع بالكامل
- [ ] إعداد `ApplicationDbContext`
- [ ] إنشاء `BaseEntity` و `TranslatableEntity`
- [ ] إعداد Entity Configurations

### المصادقة والتفويض
- [ ] إنشاء Models: User, Role, Permission, UserRole, RolePermission
- [ ] إنشاء JWT Service
- [ ] إنشاء Auth Service
- [ ] إعداد JWT في Program.cs

### الخدمات المشتركة
- [ ] File Upload Service
- [ ] Email Service
- [ ] Cache Service (Redis)
- [ ] Translation Service
- [ ] Audit Log Service

### Middleware والفلاتر
- [ ] Exception Handling Middleware
- [ ] Permission Authorization Attribute
- [ ] Audit Log Action Filter

### قاعدة البيانات
- [ ] إنشاء Models للترجمة: Language, Translation
- [ ] إنشاء AuditLog Model
- [ ] تشغيل Migration الأولى
- [ ] Seed بيانات أولية (Languages, SuperAdmin Role)

### التكوينات
- [ ] إعداد CORS
- [ ] إعداد Swagger
- [ ] إعداد Connection Strings
- [ ] إعداد JWT Settings

### Repository Pattern
- [ ] إنشاء Generic Repository
- [ ] إنشاء User Repository

### DTOs & ViewModels
- [ ] Auth DTOs (Login, Register, AuthResponse)
- [ ] User DTOs

### Constants
- [ ] Permissions Constants
- [ ] Custom Exceptions

---

## 📦 NuGet Packages المطلوبة

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="StackExchange.Redis" Version="2.7.10" />
<PackageReference Include="SixLabors.ImageSharp" Version="3.1.0" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
```

---

## 🎯 النتيجة النهائية

بعد إكمال هذا الجزء المشترك، سيكون لديك:

✅ **بنية تحتية قوية** جاهزة للتوسع  
✅ **نظام مصادقة وتفويض** كامل ومحمي  
✅ **خدمات مشتركة** يمكن استخدامها في جميع Features  
✅ **Middleware وفلاتر** للأمان والتدقيق  
✅ **Repository Pattern** للوصول للبيانات  
✅ **نظام ترجمة** جاهز  
✅ **Audit Logging** لتتبع جميع العمليات  

**الآن المطورين جاهزين للبدء في تطوير Features المستقلة! 🚀**

---

**تاريخ الإنشاء:** 11 نوفمبر 2025  
**آخر تحديث:** 11 نوفمبر 2025  
**الحالة:** 📝 جاهز للتنفيذ
