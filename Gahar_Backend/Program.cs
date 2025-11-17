using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Gahar_Backend.Data;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Services.Implementations;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Repositories.Implementations;
using Gahar_Backend.Middleware;
using Gahar_Backend.Utilities;
using StackExchange.Redis;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

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

// Redis Configuration (Optional - comment out if not using Redis)
// builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
// {
//  var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis")!);
//     return ConnectionMultiplexer.Connect(configuration);
// });

// Register Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IEmailService, EmailService>();
// builder.Services.AddScoped<ICacheService, CacheService>(); // Uncomment when Redis is configured
builder.Services.AddScoped<ITranslationService, TranslationService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();

// Department Access Control Service
builder.Services.AddScoped<IDepartmentAccessService, DepartmentAccessService>();

// Advanced Permission Services
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IDataAccessService, DataAccessService>();
builder.Services.AddScoped<IDepartmentPermissionService, DepartmentPermissionService>();
builder.Services.AddScoped<IRoleBasedAccessService, RoleBasedAccessService>();

// Feature 1: Page Builder Services
builder.Services.AddScoped<IPageService, PageService>();

// Feature 2: Form Builder Services
builder.Services.AddScoped<IFormService, FormService>();

// Feature 3: Navigation Menu Services
builder.Services.AddScoped<IMenuService, MenuService>();

// Feature 4: Facilities Management Services
builder.Services.AddScoped<IFacilityService, FacilityService>();

// Feature 5: Certificates Management Services
builder.Services.AddScoped<ICertificateService, CertificateService>();

// Feature 6: SEO & Analytics Services
builder.Services.AddScoped<ISeoAnalyticsService, SeoAnalyticsService>();

// Feature 10: Short Links with QR Code
builder.Services.AddScoped<IShortLinkService, ShortLinkService>();
builder.Services.AddScoped<IQrCodeService, QrCodeService>();

// Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Feature 1: Page Builder Repositories
builder.Services.AddScoped<IPageRepository, PageRepository>();
builder.Services.AddScoped<IPageBlockRepository, PageBlockRepository>();

// Feature 2: Form Builder Repositories
builder.Services.AddScoped<IFormRepository, FormRepository>();
builder.Services.AddScoped<IFormFieldRepository, FormFieldRepository>();
builder.Services.AddScoped<IFormSubmissionRepository, FormSubmissionRepository>();

// Feature 3: Navigation Menu Repositories
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();

// Feature 4: Facilities Management Repositories
builder.Services.AddScoped<IFacilityRepository, FacilityRepository>();
builder.Services.AddScoped<IFacilityDepartmentRepository, FacilityDepartmentRepository>();
builder.Services.AddScoped<IFacilityServiceRepository, FacilityServiceRepository>();
builder.Services.AddScoped<IFacilityImageRepository, FacilityImageRepository>();
builder.Services.AddScoped<IFacilityReviewRepository, FacilityReviewRepository>();

// Feature 5: Certificates Management Repositories
builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
builder.Services.AddScoped<ICertificateCategoryRepository, CertificateCategoryRepository>();
builder.Services.AddScoped<ICertificateRequirementRepository, CertificateRequirementRepository>();
builder.Services.AddScoped<ICertificateHolderRepository, CertificateHolderRepository>();

// Feature 6: SEO & Analytics Repositories
builder.Services.AddScoped<ISeoMetadataRepository, SeoMetadataRepository>();
builder.Services.AddScoped<IPageAnalyticsRepository, PageAnalyticsRepository>();
builder.Services.AddScoped<IAnalyticsEventRepository, AnalyticsEventRepository>();
builder.Services.AddScoped<IKeywordRepository, KeywordRepository>();

// Feature 10: Short Links Repositories
builder.Services.AddScoped<IShortLinkRepository, ShortLinkRepository>();
builder.Services.AddScoped<IShortLinkAnalyticsRepository, ShortLinkAnalyticsRepository>();

// Add HttpClientFactory for QR Code service
builder.Services.AddHttpClient();

// Rate Limiting Service
builder.Services.AddScoped<IRateLimitService, RateLimitService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
      Title = "Gahar Backend API",
        Version = "v1",
        Description = "Backend API for Gahar CMS System"
    });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
 Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
   {
     new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
 Reference = new Microsoft.OpenApi.Models.OpenApiReference
          {
      Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
   Id = "Bearer"
    }
            },
     Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
 await context.Database.MigrateAsync();
        await DataSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
      options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gahar Backend API v1");
        options.DefaultModelsExpandDepth(2);
        options.DefaultModelExpandDepth(2);
        // Set default tag expansion
  options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseRateLimiting(); // Rate Limiting Middleware - يجب أن يكون قبل الـ Authentication

// Exception Handling Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwaggerAuthentication(); // Add Swagger authentication middleware

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
