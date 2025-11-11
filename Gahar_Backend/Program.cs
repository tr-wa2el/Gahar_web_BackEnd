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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

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

// Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

// Exception Handling Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
