# 🚀 GAHAR CMS - Backend Technical Specification (Advanced .NET 9 Architecture)

## 📋 Table of Contents
1. [Architecture Overview](#architecture-overview)
2. [Project Structure](#project-structure)
3. [Database Schema](#database-schema)
4. [API Endpoints Reference](#api-endpoints-reference)
5. [Authentication & Authorization](#authentication--authorization)
6. [Event-Driven Architecture](#event-driven-architecture)
7. [Infrastructure Services](#infrastructure-services)
8. [Deployment & DevOps](#deployment--devops)

---

## 🏗️ Architecture Overview

### Technology Stack
- **Framework:** .NET 9 (Minimal APIs)
- **Database:** PostgreSQL 16 / SQL Server 2022
- **ORM:** Entity Framework Core 9
- **Authentication:** ASP.NET Core Identity + JWT
- **Message Queue:** RabbitMQ / Azure Service Bus
- **Background Jobs:** Hangfire
- **Logging:** Serilog (structured logging)
- **Caching:** Redis / In-Memory Cache
- **File Storage:** Azure Blob Storage / AWS S3 / Local Storage
- **Search:** Elasticsearch / .NET AI Semantic Search
- **Real-time:** SignalR

### Architecture Pattern
- **Clean Architecture** (Domain-Driven Design)
- **CQRS** (Command Query Responsibility Segregation) for complex operations
- **Event-Driven Architecture** for decoupled services
- **Repository Pattern** with Unit of Work
- **Vertical Slice Architecture** for feature modules

---

## 📁 Project Structure

```
GAHAR.CMS/
│
├── src/
│   ├── GAHAR.API/                          # Web API Layer
│   │   ├── Program.cs                       # Minimal API setup
│   │   ├── Endpoints/                       # Feature-based endpoints
│   │   │   ├── Auth/
│   │   │   │   ├── LoginEndpoint.cs
│   │   │   │   ├── RegisterEndpoint.cs
│   │   │   │   └── RefreshTokenEndpoint.cs
│   │   │   ├── Content/
│   │   │   │   ├── CreateContentEndpoint.cs
│   │   │   │   ├── GetContentEndpoint.cs
│   │   │   │   ├── UpdateContentEndpoint.cs
│   │   │   │   └── DeleteContentEndpoint.cs
│   │   │   ├── Pages/
│   │   │   │   ├── CreatePageEndpoint.cs
│   │   │   │   ├── GetPageEndpoint.cs
│   │   │   │   └── UpdatePageStructureEndpoint.cs
│   │   │   ├── Facilities/
│   │   │   │   ├── UploadFacilitiesExcelEndpoint.cs
│   │   │   │   ├── GetFacilitiesEndpoint.cs
│   │   │   │   └── SearchFacilitiesEndpoint.cs
│   │   │   ├── Certificates/
│   │   │   │   └── ValidateCertificateEndpoint.cs
│   │   │   ├── Forms/
│   │   │   │   ├── CreateFormEndpoint.cs
│   │   │   │   └── SubmitFormEndpoint.cs
│   │   │   ├── Search/
│   │   │   │   └── SearchEndpoint.cs
│   │   │   └── Files/
│   │   │       ├── UploadFileEndpoint.cs
│   │   │       └── DeleteFileEndpoint.cs
│   │   ├── Middleware/
│   │   │   ├── GlobalExceptionMiddleware.cs
│   │   │   ├── RequestLoggingMiddleware.cs
│   │   │   └── LanguageMiddleware.cs
│   │   ├── Filters/
│   │   │   └── ValidationFilter.cs
│   │   └── Extensions/
│   │       ├── ServiceCollectionExtensions.cs
│   │       └── WebApplicationExtensions.cs
│   │
│   ├── GAHAR.Core/                         # Domain & Business Logic
│   │   ├── Entities/                       # Domain Models
│   │   │   ├── Content.cs
│   │   │   ├── ContentTranslation.cs
│   │   │   ├── ContentType.cs
│   │   │   ├── Page.cs
│   │   │   ├── PageTranslation.cs
│   │   │   ├── Facility.cs
│   │   │   ├── FacilityTranslation.cs
│   │   │   ├── Certificate.cs
│   │   │   ├── Form.cs
│   │   │   ├── FormSubmission.cs
│   │   │   ├── User.cs (extends IdentityUser)
│   │   │   ├── Role.cs (extends IdentityRole)
│   │   │   └── AuditLog.cs
│   │   ├── Interfaces/                     # Repository Interfaces
│   │   │   ├── IContentRepository.cs
│   │   │   ├── IPageRepository.cs
│   │   │   ├── IFacilityRepository.cs
│   │   │   ├── ICertificateRepository.cs
│   │   │   ├── IFormRepository.cs
│   │   │   └── IUnitOfWork.cs
│   │   ├── Services/                       # Domain Services
│   │   │   ├── ContentService.cs
│   │   │   ├── PageBuilderService.cs
│   │   │   ├── TranslationService.cs
│   │   │   ├── FacilityService.cs
│   │   │   ├── CertificateValidationService.cs
│   │   │   └── FormService.cs
│   │   ├── Specifications/                 # Query Specifications
│   │   │   ├── ContentBySlugSpec.cs
│   │   │   └── PublishedContentSpec.cs
│   │   └── Exceptions/                     # Custom Exceptions
│   │       ├── NotFoundException.cs
│   │       ├── ValidationException.cs
│   │       └── UnauthorizedException.cs
│   │
│   ├── GAHAR.Infrastructure/               # Infrastructure Layer
│   │   ├── Data/
│   │   │   ├── AppDbContext.cs
│   │   │   ├── Configurations/             # EF Configurations
│   │   │   │   ├── ContentConfiguration.cs
│   │   │   │   ├── ContentTranslationConfiguration.cs
│   │   │   │   └── ...
│   │   │   ├── Migrations/
│   │   │   └── Seed/
│   │   │       └── DataSeeder.cs
│   │   ├── Repositories/                   # Repository Implementations
│   │   │   ├── GenericRepository.cs
│   │   │   ├── ContentRepository.cs
│   │   │   ├── PageRepository.cs
│   │   │   ├── FacilityRepository.cs
│   │   │   └── UnitOfWork.cs
│   │   ├── Identity/
│   │   │   ├── JwtTokenGenerator.cs
│   │   │   └── IdentityService.cs
│   │   ├── Messaging/                      # Event Bus
│   │   │   ├── RabbitMQ/
│   │   │   │   ├── RabbitMQEventBus.cs
│   │   │   │   └── RabbitMQConnection.cs
│   │   │   └── Consumers/
│   │   │       ├── FormSubmissionConsumer.cs
│   │   │       └── NewSubscriberConsumer.cs
│   │   ├── BackgroundJobs/                 # Hangfire Jobs
│   │   │   ├── FacilityImportJob.cs
│   │   │   ├── SitemapGeneratorJob.cs
│   │   │   └── NewsletterJob.cs
│   │   ├── Storage/
│   │   │   ├── IFileStorage.cs
│   │   │   ├── AzureBlobStorage.cs
│   │   │   └── LocalFileStorage.cs
│   │   ├── Caching/
│   │   │   ├── ICacheService.cs
│   │   │   └── RedisCacheService.cs
│   │   ├── Search/
│   │   │   ├── ISearchService.cs
│   │   │   └── ElasticsearchService.cs
│   │   └── External/                       # External API Integrations
│   │       ├── TranslationAPI.cs
│   │       └── EmailService.cs
│   │
│   └── GAHAR.Shared/                       # Shared Contracts
│       ├── DTOs/
│       │   ├── Auth/
│       │   │   ├── LoginRequest.cs
│       │   │   ├── RegisterRequest.cs
│       │   │   └── TokenResponse.cs
│       │   ├── Content/
│       │   │   ├── CreateContentRequest.cs
│       │   │   ├── UpdateContentRequest.cs
│       │   │   ├── ContentResponse.cs
│       │   │   └── ContentTranslationDto.cs
│       │   ├── Pages/
│       │   │   ├── PageRequest.cs
│       │   │   ├── PageResponse.cs
│       │   │   └── BlockDto.cs
│       │   └── ...
│       ├── Events/                         # Event Contracts
│       │   ├── FormSubmittedEvent.cs
│       │   ├── ContentPublishedEvent.cs
│       │   └── NewSubscriberEvent.cs
│       ├── Constants/
│       │   ├── Roles.cs
│       │   ├── Policies.cs
│       │   └── Languages.cs
│       └── Helpers/
│           ├── SlugGenerator.cs
│           └── ValidationHelpers.cs
│
└── tests/
    ├── GAHAR.Tests.Unit/
    ├── GAHAR.Tests.Integration/
    └── GAHAR.Tests.Performance/
```

---

## 🗄️ Database Schema

### Core Tables

#### 1. ContentTypes
```sql
CREATE TABLE ContentTypes (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    DisplayName NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    FieldsSchema NVARCHAR(MAX) NOT NULL, -- JSON schema
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);

-- Example: News, Events, Services, Team Members
```

#### 2. Content
```sql
CREATE TABLE Content (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ContentTypeId UNIQUEIDENTIFIER NOT NULL,
    IsPublished BIT DEFAULT 0,
    PublishedAt DATETIME2 NULL,
    CreatedById NVARCHAR(450) NOT NULL, -- FK to AspNetUsers
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Content_ContentType FOREIGN KEY (ContentTypeId) 
        REFERENCES ContentTypes(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Content_User FOREIGN KEY (CreatedById) 
        REFERENCES AspNetUsers(Id)
);

CREATE INDEX IX_Content_ContentTypeId ON Content(ContentTypeId);
CREATE INDEX IX_Content_IsPublished ON Content(IsPublished);
```

#### 3. ContentTranslations
```sql
CREATE TABLE ContentTranslations (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ContentId UNIQUEIDENTIFIER NOT NULL,
    Language NVARCHAR(5) NOT NULL, -- 'ar' or 'en'
    Slug NVARCHAR(500) NOT NULL,
    Title NVARCHAR(500) NOT NULL,
    Body NVARCHAR(MAX),
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    MetaKeywords NVARCHAR(500),
    
    -- Social Media Metadata (Open Graph & Twitter Cards)
    OgTitle NVARCHAR(200), -- Can differ from MetaTitle
    OgDescription NVARCHAR(500),
    OgImage NVARCHAR(1000), -- URL to social share image (1200x630 recommended)
    TwitterCard NVARCHAR(50) DEFAULT 'summary_large_image', -- 'summary', 'summary_large_image', 'player'
    
    FieldsData NVARCHAR(MAX), -- JSON dynamic fields
    LayoutId UNIQUEIDENTIFIER NULL, -- FK to Layouts (for multi-layout support)
    
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_ContentTranslations_Content FOREIGN KEY (ContentId) 
        REFERENCES Content(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_ContentTranslations_Slug_Lang UNIQUE (Slug, Language)
);

CREATE INDEX IX_ContentTranslations_ContentId ON ContentTranslations(ContentId);
CREATE INDEX IX_ContentTranslations_Language ON ContentTranslations(Language);
CREATE INDEX IX_ContentTranslations_Slug ON ContentTranslations(Slug);
CREATE INDEX IX_ContentTranslations_LayoutId ON ContentTranslations(LayoutId);
```

#### 4. Pages (Block-Based Page Builder)
```sql
CREATE TABLE Pages (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IsPublished BIT DEFAULT 0,
    PublishedAt DATETIME2 NULL,
    CreatedById NVARCHAR(450) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Pages_User FOREIGN KEY (CreatedById) 
        REFERENCES AspNetUsers(Id)
);
```

#### 4.1. Layouts (Multi-Layout System for Content Display)
```sql
CREATE TABLE Layouts (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL UNIQUE, -- e.g., 'DefaultNewsLayout', 'FeaturedArticleLayout', 'MinimalLayout'
    DisplayName NVARCHAR(200) NOT NULL, -- e.g., 'Default News Layout', 'Featured Article Layout'
    Description NVARCHAR(MAX),
    PreviewImage NVARCHAR(1000), -- Screenshot/preview of layout
    ContentTypeId UNIQUEIDENTIFIER NULL, -- NULL = available for all, otherwise specific to content type
    LayoutStructure NVARCHAR(MAX) NOT NULL, -- JSON: defines HTML structure with placeholders
    IsActive BIT DEFAULT 1,
    IsDefault BIT DEFAULT 0, -- Default layout for content type
    CreatedById NVARCHAR(450) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Layouts_ContentType FOREIGN KEY (ContentTypeId) 
        REFERENCES ContentTypes(Id) ON DELETE SET NULL,
    CONSTRAINT FK_Layouts_User FOREIGN KEY (CreatedById) 
        REFERENCES AspNetUsers(Id)
);

CREATE INDEX IX_Layouts_ContentTypeId ON Layouts(ContentTypeId);
CREATE INDEX IX_Layouts_IsActive ON Layouts(IsActive);

-- Example Layout Structure (JSON):
/*
{
  "sections": [
    {
      "id": "header",
      "type": "container",
      "className": "article-header bg-gray-100 p-8",
      "children": [
        { "id": "title", "type": "field", "field": "title", "tag": "h1", "className": "text-4xl font-bold" },
        { "id": "meta", "type": "container", "className": "flex gap-4 text-sm text-gray-600 mt-4",
          "children": [
            { "type": "field", "field": "publishedAt", "format": "date" },
            { "type": "field", "field": "author" }
          ]
        }
      ]
    },
    {
      "id": "featured-image",
      "type": "field",
      "field": "featuredImage",
      "tag": "img",
      "className": "w-full h-auto rounded-lg my-8"
    },
    {
      "id": "body",
      "type": "field",
      "field": "body",
      "tag": "div",
      "className": "prose prose-lg max-w-none"
    },
    {
      "id": "tags",
      "type": "field",
      "field": "tags",
      "tag": "div",
      "className": "flex gap-2 mt-8",
      "itemClassName": "bg-blue-100 px-3 py-1 rounded-full text-sm"
    }
  ]
}
*/
```

#### 4.2. LayoutFields (Available Fields for Layout Builder)
```sql
CREATE TABLE LayoutFields (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    LayoutId UNIQUEIDENTIFIER NOT NULL,
    FieldName NVARCHAR(100) NOT NULL, -- e.g., 'title', 'body', 'featuredImage', 'publishedAt'
    FieldType NVARCHAR(50) NOT NULL, -- 'text', 'richtext', 'image', 'date', 'array', 'custom'
    DisplayName NVARCHAR(200) NOT NULL,
    IsRequired BIT DEFAULT 0,
    DefaultValue NVARCHAR(MAX),
    ValidationRules NVARCHAR(MAX), -- JSON
    DisplayOrder INT DEFAULT 0,
    CONSTRAINT FK_LayoutFields_Layout FOREIGN KEY (LayoutId) 
        REFERENCES Layouts(Id) ON DELETE CASCADE
);

CREATE INDEX IX_LayoutFields_LayoutId ON LayoutFields(LayoutId);
```

#### 5. PageTranslations
```sql
CREATE TABLE PageTranslations (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PageId UNIQUEIDENTIFIER NOT NULL,
    Language NVARCHAR(5) NOT NULL,
    Slug NVARCHAR(500) NOT NULL,
    Title NVARCHAR(500) NOT NULL,
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    Structure NVARCHAR(MAX) NOT NULL, -- JSON blocks array
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_PageTranslations_Page FOREIGN KEY (PageId) 
        REFERENCES Pages(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_PageTranslations_Slug_Lang UNIQUE (Slug, Language)
);

CREATE INDEX IX_PageTranslations_PageId ON PageTranslations(PageId);
CREATE INDEX IX_PageTranslations_Slug ON PageTranslations(Slug);
```

### GAHAR-Specific Tables

#### 6. MediaFiles (Centralized Media Library with WebP Support)
```sql
CREATE TABLE MediaFiles (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    OriginalFileName NVARCHAR(500) NOT NULL,
    OriginalUrl NVARCHAR(1000) NOT NULL, -- Original uploaded file (jpg, png, etc.)
    WebPUrl NVARCHAR(1000), -- Auto-generated WebP version
    ThumbnailUrl NVARCHAR(1000), -- Thumbnail (300x300)
    ThumbnailWebPUrl NVARCHAR(1000), -- Thumbnail in WebP format
    FileSize BIGINT NOT NULL, -- Original file size in bytes
    WebPFileSize BIGINT, -- WebP file size (usually 25-35% smaller)
    MimeType NVARCHAR(100) NOT NULL,
    Width INT, -- For images
    Height INT,
    AspectRatio DECIMAL(10, 4),
    AltText NVARCHAR(500),
    Title NVARCHAR(500),
    Description NVARCHAR(MAX),
    UploadedById NVARCHAR(450) NOT NULL,
    UploadedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_MediaFiles_User FOREIGN KEY (UploadedById) 
        REFERENCES AspNetUsers(Id)
);

CREATE INDEX IX_MediaFiles_UploadedById ON MediaFiles(UploadedById);
CREATE INDEX IX_MediaFiles_MimeType ON MediaFiles(MimeType);
CREATE INDEX IX_MediaFiles_UploadedAt ON MediaFiles(UploadedAt);

-- WebP Conversion Pipeline (Automatic on Upload):
-- 1. User uploads original (jpg, png, gif)
-- 2. Backend validates and saves original to storage
-- 3. ImageSharp/SkiaSharp generates WebP version (quality: 85%)
-- 4. Generate thumbnail (300x300) in both original format and WebP
-- 5. Calculate file size savings
-- 6. Store all URLs in database
-- 7. Frontend uses <picture> tag with WebP fallback:
--    <picture>
--      <source srcset="image.webp" type="image/webp">
--      <img src="image.jpg" alt="...">
--    </picture>
-- Result: 25-35% smaller file sizes, faster page loads
```

#### 7. Facilities
```sql
CREATE TABLE Facilities (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FacilityCode NVARCHAR(50) NOT NULL UNIQUE,
    Latitude DECIMAL(10, 7),
    Longitude DECIMAL(10, 7),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);

CREATE INDEX IX_Facilities_FacilityCode ON Facilities(FacilityCode);
CREATE INDEX IX_Facilities_Location ON Facilities(Latitude, Longitude);
```

#### 8. FacilityTranslations
```sql
CREATE TABLE FacilityTranslations (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FacilityId UNIQUEIDENTIFIER NOT NULL,
    Language NVARCHAR(5) NOT NULL,
    Slug NVARCHAR(500) NOT NULL,
    Name NVARCHAR(500) NOT NULL,
    Description NVARCHAR(MAX),
    Address NVARCHAR(1000),
    City NVARCHAR(200),
    Region NVARCHAR(200),
    CONSTRAINT FK_FacilityTranslations_Facility FOREIGN KEY (FacilityId) 
        REFERENCES Facilities(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_FacilityTranslations_Slug_Lang UNIQUE (Slug, Language)
);

CREATE INDEX IX_FacilityTranslations_FacilityId ON FacilityTranslations(FacilityId);
CREATE INDEX IX_FacilityTranslations_Slug ON FacilityTranslations(Slug);
CREATE FULLTEXT INDEX ON FacilityTranslations(Name, Description, Address);
```

#### 8. Certificates
```sql
CREATE TABLE Certificates (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CertificateNumber NVARCHAR(100) NOT NULL UNIQUE,
    FacilityId UNIQUEIDENTIFIER NULL,
    IssueDate DATE NOT NULL,
    ExpiryDate DATE NOT NULL,
    Status NVARCHAR(20) NOT NULL, -- 'Valid', 'Expired', 'Revoked'
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Certificates_Facility FOREIGN KEY (FacilityId) 
        REFERENCES Facilities(Id) ON DELETE SET NULL
);

CREATE UNIQUE INDEX IX_Certificates_Number ON Certificates(CertificateNumber);
CREATE INDEX IX_Certificates_Status ON Certificates(Status);
CREATE INDEX IX_Certificates_ExpiryDate ON Certificates(ExpiryDate);
```

#### 9. Layouts (Multi-Layout System for Content Display)
```sql
CREATE TABLE Layouts (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL UNIQUE, -- e.g., 'DefaultNewsLayout', 'FeaturedArticleLayout', 'MinimalLayout'
    DisplayName NVARCHAR(200) NOT NULL, -- e.g., 'تخطيط الأخبار الافتراضي', 'تخطيط المقال المميز'
    Description NVARCHAR(MAX),
    PreviewImage NVARCHAR(1000), -- Screenshot/preview of layout
    ContentTypeId UNIQUEIDENTIFIER NULL, -- NULL = available for all, otherwise specific to content type
    LayoutStructure NVARCHAR(MAX) NOT NULL, -- JSON: defines HTML structure with placeholders
    CssClasses NVARCHAR(MAX), -- Custom CSS classes for this layout
    IsActive BIT DEFAULT 1,
    IsDefault BIT DEFAULT 0, -- Default layout for content type
    DisplayOrder INT DEFAULT 0,
    CreatedById NVARCHAR(450) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Layouts_ContentType FOREIGN KEY (ContentTypeId) 
        REFERENCES ContentTypes(Id) ON DELETE SET NULL,
    CONSTRAINT FK_Layouts_User FOREIGN KEY (CreatedById) 
        REFERENCES AspNetUsers(Id)
);

CREATE INDEX IX_Layouts_ContentTypeId ON Layouts(ContentTypeId);
CREATE INDEX IX_Layouts_IsActive ON Layouts(IsActive);
CREATE INDEX IX_Layouts_DisplayOrder ON Layouts(DisplayOrder);

-- Example Layout Structure (JSON):
/*
{
  "version": "1.0",
  "container": {
    "className": "max-w-4xl mx-auto py-12 px-4"
  },
  "sections": [
    {
      "id": "header",
      "type": "container",
      "className": "mb-8",
      "children": [
        {
          "id": "title",
          "type": "field",
          "field": "title",
          "tag": "h1",
          "className": "text-4xl font-bold text-gray-900 mb-4"
        },
        {
          "id": "meta",
          "type": "container",
          "className": "flex flex-wrap gap-4 text-sm text-gray-600",
          "children": [
            {
              "type": "field",
              "field": "publishedAt",
              "format": "date",
              "className": "flex items-center gap-2"
            },
            {
              "type": "field",
              "field": "author",
              "prefix": "By: ",
              "className": "flex items-center gap-2"
            },
            {
              "type": "field",
              "field": "category",
              "tag": "span",
              "className": "bg-blue-100 text-blue-800 px-3 py-1 rounded-full"
            }
          ]
        }
      ]
    },
    {
      "id": "featured-image",
      "type": "field",
      "field": "featuredImage",
      "tag": "picture",
      "className": "w-full rounded-lg overflow-hidden my-8",
      "responsive": true,
      "webp": true
    },
    {
      "id": "body",
      "type": "field",
      "field": "body",
      "tag": "div",
      "className": "prose prose-lg prose-blue max-w-none"
    },
    {
      "id": "tags",
      "type": "field",
      "field": "tags",
      "tag": "div",
      "className": "flex flex-wrap gap-2 mt-8",
      "itemClassName": "bg-gray-100 hover:bg-gray-200 px-3 py-1 rounded-full text-sm transition-colors"
    },
    {
      "id": "social-share",
      "type": "component",
      "component": "SocialShareButtons",
      "className": "mt-8 pt-8 border-t"
    }
  ]
}
*/
```

#### 10. LayoutFields (Available Fields for Layout Builder)
```sql
CREATE TABLE LayoutFields (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    LayoutId UNIQUEIDENTIFIER NOT NULL,
    FieldName NVARCHAR(100) NOT NULL, -- e.g., 'title', 'body', 'featuredImage', 'publishedAt'
    FieldType NVARCHAR(50) NOT NULL, -- 'text', 'richtext', 'image', 'date', 'array', 'custom'
    DisplayName NVARCHAR(200) NOT NULL,
    IsRequired BIT DEFAULT 0,
    DefaultValue NVARCHAR(MAX),
    ValidationRules NVARCHAR(MAX), -- JSON
    DisplayOrder INT DEFAULT 0,
    CONSTRAINT FK_LayoutFields_Layout FOREIGN KEY (LayoutId) 
        REFERENCES Layouts(Id) ON DELETE CASCADE
);

CREATE INDEX IX_LayoutFields_LayoutId ON LayoutFields(LayoutId);
```

#### 11. Forms
```sql
CREATE TABLE Forms (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    IsActive BIT DEFAULT 1,
    CreatedById NVARCHAR(450) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Forms_User FOREIGN KEY (CreatedById) 
        REFERENCES AspNetUsers(Id)
);

CREATE INDEX IX_Forms_IsActive ON Forms(IsActive);
```

#### 11.1. Menus (Navigation Menus with Icons)
```sql
CREATE TABLE Menus (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL, -- e.g., 'Main Menu', 'Footer Menu', 'Sidebar Menu'
    Location NVARCHAR(100) NOT NULL, -- 'header', 'footer', 'sidebar', 'mobile'
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);

CREATE INDEX IX_Menus_Location ON Menus(Location);
CREATE INDEX IX_Menus_IsActive ON Menus(IsActive);
```

#### 11.2. MenuItems (Menu Items with Icons)
```sql
CREATE TABLE MenuItems (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    MenuId UNIQUEIDENTIFIER NOT NULL,
    ParentId UNIQUEIDENTIFIER NULL, -- For nested menus (dropdown)
    
    -- Icon Settings
    IconType NVARCHAR(50), -- 'lucide', 'heroicons', 'fontawesome', 'custom', 'emoji'
    IconName NVARCHAR(100), -- e.g., 'Home', 'User', 'Settings' (for icon libraries)
    IconSvg NVARCHAR(MAX), -- Custom SVG code (if IconType = 'custom')
    IconEmoji NVARCHAR(10), -- Emoji character (if IconType = 'emoji')
    IconColor NVARCHAR(50), -- Hex color or CSS color name
    IconSize NVARCHAR(20) DEFAULT 'md', -- 'sm', 'md', 'lg', 'xl'
    ShowIcon BIT DEFAULT 1, -- Show/hide icon
    
    -- Link Settings
    LinkType NVARCHAR(50) NOT NULL, -- 'internal', 'external', 'page', 'content', 'custom'
    Url NVARCHAR(1000), -- For external links or custom URLs
    PageId UNIQUEIDENTIFIER, -- Link to Pages table
    ContentType NVARCHAR(100), -- 'news', 'events', etc.
    ContentId UNIQUEIDENTIFIER, -- Link to specific content
    
    OpenInNewTab BIT DEFAULT 0,
    CssClass NVARCHAR(500), -- Additional CSS classes
    DisplayOrder INT NOT NULL,
    IsActive BIT DEFAULT 1,
    
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_MenuItems_Menu FOREIGN KEY (MenuId) 
        REFERENCES Menus(Id) ON DELETE CASCADE,
    CONSTRAINT FK_MenuItems_Parent FOREIGN KEY (ParentId) 
        REFERENCES MenuItems(Id) ON DELETE NO ACTION
);

CREATE INDEX IX_MenuItems_MenuId ON MenuItems(MenuId);
CREATE INDEX IX_MenuItems_ParentId ON MenuItems(ParentId);
CREATE INDEX IX_MenuItems_DisplayOrder ON MenuItems(MenuId, DisplayOrder);
CREATE INDEX IX_MenuItems_IsActive ON MenuItems(IsActive);

-- Supported Icon Libraries:
-- 1. Lucide Icons (React): 1000+ icons
-- 2. Heroicons: 200+ icons
-- 3. Font Awesome: 2000+ icons
-- 4. Custom SVG: any SVG code
-- 5. Emoji: any Unicode emoji
```

#### 11.3. MenuItemTranslations
```sql
CREATE TABLE MenuItemTranslations (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    MenuItemId UNIQUEIDENTIFIER NOT NULL,
    Language NVARCHAR(5) NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500), -- Tooltip or subtitle
    CONSTRAINT FK_MenuItemTranslations_MenuItem FOREIGN KEY (MenuItemId) 
        REFERENCES MenuItems(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_MenuItemTranslations_MenuItem_Lang UNIQUE (MenuItemId, Language)
);

CREATE INDEX IX_MenuItemTranslations_MenuItemId ON MenuItemTranslations(MenuItemId);
```

#### 12. FormFields (Drag & Drop Fields)
```sql
CREATE TABLE FormFields (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FormId UNIQUEIDENTIFIER NOT NULL,
    FieldType NVARCHAR(50) NOT NULL, -- 'text', 'email', 'number', 'select', 'checkbox', 'radio', 'file', 'date', 'textarea'
    FieldName NVARCHAR(100) NOT NULL, -- Technical name (e.g., 'fullName')
    Label NVARCHAR(500) NOT NULL, -- Display label
    Placeholder NVARCHAR(500),
    DefaultValue NVARCHAR(MAX),
    HelpText NVARCHAR(1000),
    IsRequired BIT DEFAULT 0,
    DisplayOrder INT NOT NULL,
    Width INT DEFAULT 100, -- Column width percentage (50 for half-width, 100 for full-width)
    
    -- Field-specific options (JSON)
    -- For 'select'/'radio'/'checkbox': { "options": [{"label": "Option 1", "value": "opt1"}] }
    -- For 'file': { "maxSize": 5242880, "allowedTypes": ["jpg", "png", "pdf"] }
    -- For 'text'/'textarea': { "minLength": 10, "maxLength": 500 }
    FieldOptions NVARCHAR(MAX),
    
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_FormFields_Form FOREIGN KEY (FormId) 
        REFERENCES Forms(Id) ON DELETE CASCADE
);

CREATE INDEX IX_FormFields_FormId ON FormFields(FormId);
CREATE INDEX IX_FormFields_DisplayOrder ON FormFields(FormId, DisplayOrder);
```

#### 9.2. ValidationRules (Advanced Validation)
```sql
CREATE TABLE ValidationRules (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FormFieldId UNIQUEIDENTIFIER NOT NULL,
    RuleType NVARCHAR(50) NOT NULL, -- 'required', 'minLength', 'maxLength', 'pattern', 'min', 'max', 'email', 'url', 'custom'
    RuleValue NVARCHAR(MAX), -- For 'pattern': regex, for 'minLength': number, etc.
    ErrorMessage NVARCHAR(500) NOT NULL, -- Custom error message
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_ValidationRules_FormField FOREIGN KEY (FormFieldId) 
        REFERENCES FormFields(Id) ON DELETE CASCADE
);

CREATE INDEX IX_ValidationRules_FormFieldId ON ValidationRules(FormFieldId);
```

#### 9.3. ConditionalLogic (Show/Hide Fields Based on Other Fields)
```sql
CREATE TABLE ConditionalLogic (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FormFieldId UNIQUEIDENTIFIER NOT NULL, -- Field to show/hide
    TriggerFieldId UNIQUEIDENTIFIER NOT NULL, -- Field that triggers the condition
    Condition NVARCHAR(50) NOT NULL, -- 'equals', 'notEquals', 'contains', 'greaterThan', 'lessThan'
    TriggerValue NVARCHAR(MAX) NOT NULL, -- Value to compare against
    Action NVARCHAR(20) NOT NULL, -- 'show' or 'hide'
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_ConditionalLogic_FormField FOREIGN KEY (FormFieldId) 
        REFERENCES FormFields(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_ConditionalLogic_TriggerField FOREIGN KEY (TriggerFieldId) 
        REFERENCES FormFields(Id) ON DELETE NO ACTION
);

CREATE INDEX IX_ConditionalLogic_FormFieldId ON ConditionalLogic(FormFieldId);
CREATE INDEX IX_ConditionalLogic_TriggerFieldId ON ConditionalLogic(TriggerFieldId);
```

#### 10. FormSubmissions
```sql
CREATE TABLE FormSubmissions (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FormId UNIQUEIDENTIFIER NOT NULL,
    SubmissionData NVARCHAR(MAX) NOT NULL, -- JSON
    SubmitterEmail NVARCHAR(200),
    SubmitterName NVARCHAR(200),
    IPAddress NVARCHAR(50),
    UserAgent NVARCHAR(500),
    Status NVARCHAR(20) DEFAULT 'Pending', -- 'Pending', 'Reviewed', 'Processed'
    SubmittedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_FormSubmissions_Form FOREIGN KEY (FormId) 
        REFERENCES Forms(Id) ON DELETE CASCADE
);

CREATE INDEX IX_FormSubmissions_FormId ON FormSubmissions(FormId);
CREATE INDEX IX_FormSubmissions_SubmittedAt ON FormSubmissions(SubmittedAt);
```

### Audit & Logging Tables

#### 11. Albums (Photo Galleries)
```sql
CREATE TABLE Albums (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IsPublished BIT DEFAULT 0,
    PublishedAt DATETIME2 NULL,
    CoverImageId UNIQUEIDENTIFIER NULL, -- FK to AlbumImages
    DisplayOrder INT DEFAULT 0,
    CreatedById NVARCHAR(450) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Albums_User FOREIGN KEY (CreatedById) 
        REFERENCES AspNetUsers(Id)
);

CREATE INDEX IX_Albums_IsPublished ON Albums(IsPublished);
CREATE INDEX IX_Albums_DisplayOrder ON Albums(DisplayOrder);
```

#### 11.1. AlbumTranslations
```sql
CREATE TABLE AlbumTranslations (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AlbumId UNIQUEIDENTIFIER NOT NULL,
    Language NVARCHAR(5) NOT NULL,
    Slug NVARCHAR(500) NOT NULL,
    Title NVARCHAR(500) NOT NULL,
    Description NVARCHAR(MAX),
    MetaTitle NVARCHAR(200),
    MetaDescription NVARCHAR(500),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_AlbumTranslations_Album FOREIGN KEY (AlbumId) 
        REFERENCES Albums(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_AlbumTranslations_Slug_Lang UNIQUE (Slug, Language)
);

CREATE INDEX IX_AlbumTranslations_AlbumId ON AlbumTranslations(AlbumId);
CREATE INDEX IX_AlbumTranslations_Slug ON AlbumTranslations(Slug);
```

#### 11.2. AlbumImages
```sql
CREATE TABLE AlbumImages (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AlbumId UNIQUEIDENTIFIER NOT NULL,
    FileUrl NVARCHAR(1000) NOT NULL, -- Full URL or path
    ThumbnailUrl NVARCHAR(1000), -- Auto-generated thumbnail
    FileName NVARCHAR(500) NOT NULL,
    FileSize BIGINT NOT NULL, -- Bytes
    MimeType NVARCHAR(100) NOT NULL,
    Width INT NOT NULL, -- Image width in pixels
    Height INT NOT NULL, -- Image height in pixels
    AspectRatio DECIMAL(10, 4) NOT NULL, -- Width / Height (for collage algorithm)
    DisplayOrder INT DEFAULT 0,
    UploadedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_AlbumImages_Album FOREIGN KEY (AlbumId) 
        REFERENCES Albums(Id) ON DELETE CASCADE
);

CREATE INDEX IX_AlbumImages_AlbumId ON AlbumImages(AlbumId);
CREATE INDEX IX_AlbumImages_DisplayOrder ON AlbumImages(AlbumId, DisplayOrder);
CREATE INDEX IX_AlbumImages_AspectRatio ON AlbumImages(AspectRatio);

-- Add foreign key for cover image
ALTER TABLE Albums
ADD CONSTRAINT FK_Albums_CoverImage FOREIGN KEY (CoverImageId) 
    REFERENCES AlbumImages(Id) ON DELETE NO ACTION;
```

#### 11.3. AlbumImageTranslations (Optional: for image captions)
```sql
CREATE TABLE AlbumImageTranslations (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AlbumImageId UNIQUEIDENTIFIER NOT NULL,
    Language NVARCHAR(5) NOT NULL,
    Caption NVARCHAR(1000),
    AltText NVARCHAR(500), -- For SEO and accessibility
    CONSTRAINT FK_AlbumImageTranslations_AlbumImage FOREIGN KEY (AlbumImageId) 
        REFERENCES AlbumImages(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_AlbumImageTranslations_Image_Lang UNIQUE (AlbumImageId, Language)
);

CREATE INDEX IX_AlbumImageTranslations_AlbumImageId ON AlbumImageTranslations(AlbumImageId);
```

#### 12. AuditLogs
```sql
CREATE TABLE AuditLogs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId NVARCHAR(450),
    Action NVARCHAR(100) NOT NULL,
    EntityType NVARCHAR(100),
    EntityId NVARCHAR(100),
    Changes NVARCHAR(MAX), -- JSON
    IPAddress NVARCHAR(50),
    Timestamp DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_AuditLogs_User FOREIGN KEY (UserId) 
        REFERENCES AspNetUsers(Id) ON DELETE SET NULL
);

CREATE INDEX IX_AuditLogs_UserId ON AuditLogs(UserId);
CREATE INDEX IX_AuditLogs_Timestamp ON AuditLogs(Timestamp);
CREATE INDEX IX_AuditLogs_EntityType ON AuditLogs(EntityType);
```

---

## 🔌 API Endpoints Reference

### Authentication & Authorization

#### POST `/api/auth/register`
Register a new user.
```json
Request:
{
  "email": "admin@gahar.sa",
  "password": "SecurePass123!",
  "fullName": "Admin User",
  "phoneNumber": "+966501234567"
}

Response: 201 Created
{
  "userId": "guid",
  "email": "admin@gahar.sa",
  "message": "User registered successfully"
}
```

#### POST `/api/auth/login`
Authenticate and receive JWT tokens.
```json
Request:
{
  "email": "admin@gahar.sa",
  "password": "SecurePass123!"
}

Response: 200 OK
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "guid",
  "expiresIn": 3600,
  "tokenType": "Bearer"
}
```

#### POST `/api/auth/refresh-token`
Refresh expired access token.

#### POST `/api/auth/logout`
Revoke refresh token.

---

### Media Management

#### POST `/api/media/upload`
Upload media file (automatic WebP conversion).
```
Request: multipart/form-data
file: image.jpg
altText: "صورة المقال"
title: "عنوان الصورة"

Response: 201 Created
{
  "id": "guid",
  "originalFileName": "image.jpg",
  "originalUrl": "https://cdn.gahar.sa/media/2025/11/image.jpg",
  "webPUrl": "https://cdn.gahar.sa/media/2025/11/image.webp",
  "thumbnailUrl": "https://cdn.gahar.sa/media/2025/11/thumb_image.jpg",
  "thumbnailWebPUrl": "https://cdn.gahar.sa/media/2025/11/thumb_image.webp",
  "fileSize": 2048576, // 2MB
  "webPFileSize": 716800, // 700KB (65% reduction!)
  "mimeType": "image/jpeg",
  "width": 1920,
  "height": 1080,
  "aspectRatio": 1.7778,
  "altText": "صورة المقال",
  "uploadedAt": "2025-11-10T10:00:00Z",
  "savings": {
    "bytes": 1331776,
    "percentage": 65,
    "message": "WebP version is 65% smaller than original"
  }
}

// Backend Processing (ImageSharp):
// 1. Validate file type (jpg, png, gif, bmp)
// 2. Validate file size (max 10MB)
// 3. Save original to storage
// 4. Generate WebP version (quality: 85%)
// 5. Generate thumbnail (300x300) in both formats
// 6. Calculate dimensions and aspect ratio
// 7. Store all URLs in MediaFiles table
// 8. Return complete media object
```

#### GET `/api/media?page=1&pageSize=20&search=&type=`
List all media files.
```json
Response: 200 OK
{
  "items": [
    {
      "id": "guid",
      "originalFileName": "conference.jpg",
      "thumbnailWebPUrl": "https://cdn.gahar.sa/media/2025/11/thumb_conference.webp",
      "fileSize": 2048576,
      "webPFileSize": 716800,
      "mimeType": "image/jpeg",
      "uploadedAt": "2025-11-10T10:00:00Z"
    }
  ],
  "totalCount": 150,
  "page": 1,
  "pageSize": 20
}
```

#### GET `/api/media/{id}`
Get media file details.

#### DELETE `/api/media/{id}`
Delete media file (removes all versions from storage).

---

### Content Management

#### POST `/api/content/{contentType}`
Create new content with default translation.
```json
Request:
POST /api/content/news

{
  "defaultLanguage": "ar",
  "isPublished": false,
  "translations": {
    "ar": {
      "title": "أخبار جديدة",
      "slug": "akhbar-jadeeda",
      "body": "محتوى الخبر...",
      "metaTitle": "أخبار جديدة - غاهر",
      "metaDescription": "وصف الخبر للسيو"
    }
  },
  "customFields": {
    "featuredImage": "/uploads/news-1.jpg",
    "author": "Admin",
    "tags": ["صحة", "جودة"]
  },
  "layoutId": "guid-of-default-news-layout", // Optional: select layout
  "ogImage": "https://cdn.gahar.sa/media/2025/11/news-social.webp" // Optional: custom OG image
}

Response: 201 Created
{
  "id": "guid",
  "contentType": "news",
  "layoutId": "guid-of-default-news-layout",
  "isPublished": false,
  "createdAt": "2025-11-09T10:00:00Z"
}
```

#### GET `/api/content/{contentType}?lang=ar&page=1&pageSize=10`
Fetch paginated content.
```json
Response: 200 OK
{
  "items": [
    {
      "id": "guid",
      "slug": "akhbar-jadeeda",
      "title": "أخبار جديدة",
      "excerpt": "محتوى مختصر...",
      "createdAt": "2025-11-09T10:00:00Z"
    }
  ],
  "totalCount": 50,
  "page": 1,
  "pageSize": 10
}
```

#### GET `/api/content/{contentType}/{slug}?lang=ar`
Get single content by slug (with layout).
```json
Response: 200 OK
{
  "id": "guid",
  "slug": "akhbar-jadeeda",
  "title": "أخبار جديدة",
  "body": "محتوى الخبر الكامل...",
  "metaTitle": "أخبار جديدة - غاهر",
  "metaDescription": "وصف الخبر للسيو",
  "ogTitle": "أخبار جديدة - غاهر",
  "ogDescription": "شاهد آخر الأخبار من غاهر",
  "ogImage": "https://cdn.gahar.sa/media/2025/11/news-social.webp",
  "twitterCard": "summary_large_image",
  "layout": {
    "id": "guid",
    "name": "DefaultNewsLayout",
    "displayName": "تخطيط الأخبار الافتراضي",
    "structure": { /* JSON layout structure */ }
  },
  "customFields": { 
    "featuredImage": {
      "originalUrl": "https://cdn.gahar.sa/media/2025/11/news.jpg",
      "webPUrl": "https://cdn.gahar.sa/media/2025/11/news.webp",
      "thumbnailWebPUrl": "https://cdn.gahar.sa/media/2025/11/thumb_news.webp",
      "altText": "صورة الخبر",
      "width": 1920,
      "height": 1080
    },
    "author": "Admin",
    "tags": ["صحة", "جودة"]
  },
  "createdAt": "2025-11-09T10:00:00Z"
}
```

#### PUT `/api/content/{contentType}/{id}/layout`
Change content layout.
```json
Request:
{
  "layoutId": "guid-of-featured-article-layout"
}

Response: 200 OK
{
  "message": "Layout updated successfully",
  "newLayout": {
    "id": "guid",
    "name": "FeaturedArticleLayout",
    "displayName": "تخطيط المقال المميز"
  }
}
```

#### PUT `/api/content/{contentType}/{id}?lang=en`
Update specific language translation (including social metadata).
```json
Request:
{
  "title": "New News",
  "slug": "new-news",
  "body": "News content...",
  "metaTitle": "New News - GAHAR",
  "metaDescription": "SEO description",
  "ogTitle": "Breaking News - GAHAR",
  "ogDescription": "Check out the latest news from GAHAR",
  "ogImage": "https://cdn.gahar.sa/media/2025/11/news-en-social.webp"
}

Response: 200 OK
```

#### PUT `/api/content/{contentType}/{id}/move`
Move content to different content type (e.g., move from "news" to "events").
```json
Request:
{
  "targetContentType": "events"
}

Response: 200 OK
{
  "id": "guid",
  "oldContentType": "news",
  "newContentType": "events",
  "message": "Content moved successfully",
  "updatedAt": "2025-11-10T15:30:00Z"
}

// Use case: Article was created in "news" by mistake, admin moves it to "events"
// - Content ID remains the same
// - All translations are preserved
// - Custom fields are migrated (compatible fields kept, incompatible fields logged)
// - Audit log records the move action
```

#### DELETE `/api/content/{contentType}/{id}`
Delete content (soft delete).

---

### Page Builder

#### POST `/api/pages`
Create new page.
```json
Request:
{
  "defaultLanguage": "ar",
  "translations": {
    "ar": {
      "slug": "home",
      "title": "الصفحة الرئيسية",
      "metaTitle": "غاهر - الصفحة الرئيسية",
      "structure": [
        {
          "id": "block-1",
          "blockType": "HeroBanner",
          "props": {
            "backgroundImage": "/uploads/hero-bg.jpg",
            "title": "مرحباً بكم في غاهر",
            "subtitle": "نظام اعتماد المنشآت الصحية",
            "ctaText": "اعرف المزيد",
            "ctaLink": "/about"
          }
        },
        {
          "id": "block-2",
          "blockType": "LatestNews",
          "props": {
            "title": "آخر الأخبار",
            "count": 6,
            "layout": "grid"
          }
        }
      ]
    }
  }
}

Response: 201 Created
```

#### GET `/api/pages/{slug}?lang=ar`
Fetch page structure.
```json
Response: 200 OK
{
  "id": "guid",
  "slug": "home",
  "title": "الصفحة الرئيسية",
  "structure": [ ... ],
  "isPublished": true
}
```

#### PUT `/api/pages/{id}?lang=ar`
Update page structure.

---

### Layouts Management

#### GET `/api/layouts?contentType=news`
List all layouts (optionally filter by content type).
```json
Response: 200 OK
{
  "items": [
    {
      "id": "guid-1",
      "name": "DefaultNewsLayout",
      "displayName": "تخطيط الأخبار الافتراضي",
      "description": "تخطيط بسيط وواضح للأخبار",
      "previewImage": "https://cdn.gahar.sa/layouts/default-news-preview.jpg",
      "contentType": "news",
      "isDefault": true,
      "isActive": true
    },
    {
      "id": "guid-2",
      "name": "FeaturedArticleLayout",
      "displayName": "تخطيط المقال المميز",
      "description": "تخطيط واسع مع صورة بارزة كبيرة",
      "previewImage": "https://cdn.gahar.sa/layouts/featured-article-preview.jpg",
      "contentType": "news",
      "isDefault": false,
      "isActive": true
    },
    {
      "id": "guid-3",
      "name": "MinimalLayout",
      "displayName": "تخطيط بسيط",
      "description": "تخطيط نصي بدون صور",
      "previewImage": "https://cdn.gahar.sa/layouts/minimal-preview.jpg",
      "contentType": null,
      "isDefault": false,
      "isActive": true
    }
  ],
  "totalCount": 3
}
```

#### POST `/api/layouts`
Create new layout (via Layout Builder).
```json
Request:
{
  "name": "CustomNewsLayout",
  "displayName": "تخطيط أخبار مخصص",
  "description": "تخطيط مخصص مع عمودين",
  "contentTypeId": "guid-of-news-content-type",
  "previewImage": "base64-encoded-screenshot",
  "layoutStructure": {
    "version": "1.0",
    "container": {
      "className": "max-w-6xl mx-auto py-12"
    },
    "sections": [
      {
        "id": "header",
        "type": "container",
        "className": "grid grid-cols-1 md:grid-cols-2 gap-8 mb-8",
        "children": [
          {
            "id": "featured-image",
            "type": "field",
            "field": "featuredImage",
            "tag": "picture",
            "className": "rounded-lg overflow-hidden",
            "webp": true
          },
          {
            "id": "title-meta",
            "type": "container",
            "className": "flex flex-col justify-center",
            "children": [
              {
                "type": "field",
                "field": "title",
                "tag": "h1",
                "className": "text-3xl font-bold mb-4"
              },
              {
                "type": "field",
                "field": "publishedAt",
                "format": "date",
                "className": "text-gray-600"
              }
            ]
          }
        ]
      },
      {
        "id": "body",
        "type": "field",
        "field": "body",
        "tag": "div",
        "className": "prose max-w-none"
      }
    ]
  },
  "fields": [
    {
      "fieldName": "title",
      "fieldType": "text",
      "displayName": "العنوان",
      "isRequired": true
    },
    {
      "fieldName": "featuredImage",
      "fieldType": "image",
      "displayName": "الصورة البارزة",
      "isRequired": true
    },
    {
      "fieldName": "body",
      "fieldType": "richtext",
      "displayName": "المحتوى",
      "isRequired": true
    },
    {
      "fieldName": "publishedAt",
      "fieldType": "date",
      "displayName": "تاريخ النشر",
      "isRequired": false
    }
  ]
}

Response: 201 Created
{
  "id": "guid",
  "name": "CustomNewsLayout",
  "displayName": "تخطيط أخبار مخصص",
  "createdAt": "2025-11-10T10:00:00Z"
}
```

#### GET `/api/layouts/{id}`
Get layout details (for editing).

#### PUT `/api/layouts/{id}`
Update layout structure.

#### DELETE `/api/layouts/{id}`
Delete layout (only if not in use by any content).

#### PUT `/api/layouts/{id}/set-default`
Set as default layout for content type.
```json
Request:
{
  "contentTypeId": "guid-of-news-content-type"
}

Response: 200 OK
{
  "message": "Layout set as default for 'news' content type"
}
```

#### POST `/api/layouts/{id}/duplicate`
Duplicate existing layout for customization.
```json
Response: 201 Created
{
  "id": "new-guid",
  "name": "CustomNewsLayout (Copy)",
  "message": "Layout duplicated successfully"
}
```

---

### Facilities Management

#### POST `/api/facilities/upload`
Upload Excel file with facilities data.
```
Request: multipart/form-data
file: facilities.xlsx

Response: 202 Accepted
{
  "jobId": "guid",
  "message": "File uploaded and processing started",
  "estimatedTime": "5 minutes"
}
```

#### GET `/api/facilities/upload-status/{jobId}`
Check upload processing status.

#### GET `/api/facilities?lang=ar&page=1&city=الرياض`
List facilities with filters.
```json
Response: 200 OK
{
  "items": [
    {
      "id": "guid",
      "facilityCode": "FAC-001",
      "slug": "مستشفى-الملك-فهد",
      "name": "مستشفى الملك فهد",
      "city": "الرياض",
      "location": {
        "lat": 24.7136,
        "lng": 46.6753
      }
    }
  ],
  "totalCount": 120
}
```

#### GET `/api/facilities/{slug}?lang=ar`
Get facility details.

#### GET `/api/facilities/geojson?lang=ar`
Get facilities as GeoJSON for map display.
```json
Response: 200 OK
{
  "type": "FeatureCollection",
  "features": [
    {
      "type": "Feature",
      "geometry": {
        "type": "Point",
        "coordinates": [46.6753, 24.7136]
      },
      "properties": {
        "id": "guid",
        "name": "مستشفى الملك فهد",
        "facilityCode": "FAC-001"
      }
    }
  ]
}
```

---

### Certificate Validation

#### GET `/api/certificates/validate/{certificateNumber}?lang=ar`
Validate certificate (high-performance, public endpoint).
```json
Response: 200 OK
{
  "isValid": true,
  "certificateNumber": "CERT-2025-001",
  "status": "Valid",
  "facility": {
    "name": "مستشفى الملك فهد",
    "facilityCode": "FAC-001"
  },
  "issueDate": "2024-01-01",
  "expiryDate": "2026-01-01",
  "message": "الشهادة صالحة"
}
```

---

### Forms & Submissions

#### POST `/api/forms`
Create new form (returns empty form with ID).
```json
Request:
{
  "name": "Contact Form",
  "description": "Get in touch with us",
  "isActive": true
}

Response: 201 Created
{
  "id": "guid",
  "name": "Contact Form",
  "description": "Get in touch with us",
  "fields": [],
  "createdAt": "2025-11-10T10:00:00Z"
}
```

#### POST `/api/forms/{formId}/fields`
Add field to form (used by Form Builder drag & drop).
```json
Request:
{
  "fieldType": "text",
  "fieldName": "fullName",
  "label": "الاسم الكامل",
  "placeholder": "أدخل اسمك الكامل",
  "isRequired": true,
  "displayOrder": 1,
  "width": 100,
  "fieldOptions": {},
  "validationRules": [
    {
      "ruleType": "minLength",
      "ruleValue": "3",
      "errorMessage": "الاسم يجب أن يكون 3 أحرف على الأقل"
    },
    {
      "ruleType": "maxLength",
      "ruleValue": "100",
      "errorMessage": "الاسم يجب ألا يتجاوز 100 حرف"
    }
  ]
}

Response: 201 Created
{
  "id": "guid",
  "fieldType": "text",
  "fieldName": "fullName",
  "label": "الاسم الكامل",
  "isRequired": true,
  "displayOrder": 1,
  "validationRules": [...]
}
```

#### PUT `/api/forms/{formId}/fields/{fieldId}`
Update field (reorder, change properties, update validations).
```json
Request:
{
  "label": "الاسم الكامل (مطلوب)",
  "displayOrder": 2,
  "width": 50,
  "validationRules": [
    {
      "ruleType": "required",
      "errorMessage": "هذا الحقل مطلوب"
    }
  ]
}

Response: 200 OK
```

#### DELETE `/api/forms/{formId}/fields/{fieldId}`
Delete field from form.

#### POST `/api/forms/{formId}/fields/{fieldId}/conditional-logic`
Add conditional logic (show/hide field based on another field).
```json
Request:
{
  "triggerFieldId": "guid-of-citizenship-field",
  "condition": "equals",
  "triggerValue": "سعودي",
  "action": "show"
}

Response: 201 Created
{
  "id": "guid",
  "formFieldId": "current-field-id",
  "triggerFieldId": "guid-of-citizenship-field",
  "condition": "equals",
  "triggerValue": "سعودي",
  "action": "show"
}

// Example: "رقم الهوية الوطنية" field only shows when "الجنسية" = "سعودي"
```

#### GET `/api/forms`
List all forms.
```json
Response: 200 OK
{
  "items": [
    {
      "id": "guid",
      "name": "Contact Form",
      "description": "Get in touch with us",
      "fieldsCount": 5,
      "submissionsCount": 120,
      "isActive": true,
      "createdAt": "2025-11-01T10:00:00Z"
    }
  ],
  "totalCount": 10
}
```

#### GET `/api/forms/{id}`
Get complete form structure with all fields, validations, and conditional logic.
```json
Response: 200 OK
{
  "id": "guid",
  "name": "Contact Form",
  "description": "Get in touch with us",
  "isActive": true,
  "fields": [
    {
      "id": "guid",
      "fieldType": "text",
      "fieldName": "fullName",
      "label": "الاسم الكامل",
      "placeholder": "أدخل اسمك الكامل",
      "isRequired": true,
      "displayOrder": 1,
      "width": 100,
      "fieldOptions": {},
      "validationRules": [
        {
          "id": "guid",
          "ruleType": "minLength",
          "ruleValue": "3",
          "errorMessage": "الاسم يجب أن يكون 3 أحرف على الأقل"
        }
      ],
      "conditionalLogic": []
    },
    {
      "id": "guid",
      "fieldType": "select",
      "fieldName": "citizenship",
      "label": "الجنسية",
      "isRequired": true,
      "displayOrder": 2,
      "width": 50,
      "fieldOptions": {
        "options": [
          { "label": "سعودي", "value": "saudi" },
          { "label": "غير سعودي", "value": "non-saudi" }
        ]
      },
      "validationRules": [],
      "conditionalLogic": []
    },
    {
      "id": "guid",
      "fieldType": "text",
      "fieldName": "nationalId",
      "label": "رقم الهوية الوطنية",
      "isRequired": false,
      "displayOrder": 3,
      "width": 50,
      "fieldOptions": {},
      "validationRules": [
        {
          "ruleType": "pattern",
          "ruleValue": "^[0-9]{10}$",
          "errorMessage": "رقم الهوية يجب أن يكون 10 أرقام"
        }
      ],
      "conditionalLogic": [
        {
          "triggerFieldId": "guid-of-citizenship-field",
          "condition": "equals",
          "triggerValue": "saudi",
          "action": "show"
        }
      ]
    },
    {
      "id": "guid",
      "fieldType": "email",
      "fieldName": "email",
      "label": "البريد الإلكتروني",
      "isRequired": true,
      "displayOrder": 4,
      "width": 100,
      "validationRules": [
        {
          "ruleType": "email",
          "errorMessage": "البريد الإلكتروني غير صحيح"
        }
      ]
    },
    {
      "id": "guid",
      "fieldType": "file",
      "fieldName": "attachment",
      "label": "المرفقات",
      "isRequired": false,
      "displayOrder": 5,
      "width": 100,
      "fieldOptions": {
        "maxSize": 5242880,
        "allowedTypes": ["pdf", "jpg", "png", "docx"]
      },
      "validationRules": [
        {
          "ruleType": "custom",
          "ruleValue": "fileSize",
          "errorMessage": "حجم الملف يجب ألا يتجاوز 5 ميجابايت"
        }
      ]
    }
  ]
}
```

#### PUT `/api/forms/{id}/reorder`
Reorder all fields (after drag & drop in Form Builder).
```json
Request:
{
  "fieldOrders": [
    { "fieldId": "guid-1", "displayOrder": 1 },
    { "fieldId": "guid-2", "displayOrder": 2 },
    { "fieldId": "guid-3", "displayOrder": 3 }
  ]
}

Response: 200 OK
```

#### POST `/api/forms/{id}/duplicate`
Duplicate form (copy all fields, validations, conditional logic).
```json
Response: 201 Created
{
  "id": "new-guid",
  "name": "Contact Form (Copy)",
  "fieldsCount": 5
}
```

#### POST `/api/forms/{id}/submit`
Submit form data (with validation).
```json
Request:
{
  "fullName": "Ahmed Mohammed",
  "citizenship": "saudi",
  "nationalId": "1234567890",
  "email": "ahmed@example.com",
  "attachment": "file-guid-from-upload"
}

Response: 201 Created
{
  "submissionId": "guid",
  "message": "Form submitted successfully"
}

// If validation fails:
Response: 400 Bad Request
{
  "errors": {
    "fullName": ["الاسم يجب أن يكون 3 أحرف على الأقل"],
    "email": ["البريد الإلكتروني غير صحيح"],
    "nationalId": ["رقم الهوية يجب أن يكون 10 أرقام"]
  }
}
```

#### GET `/api/forms/{id}/submissions`
Get all submissions for a form.
```json
Response: 200 OK
{
  "items": [
    {
      "id": "guid",
      "submitterName": "Ahmed Mohammed",
      "submitterEmail": "ahmed@example.com",
      "submittedAt": "2025-11-10T14:30:00Z",
      "status": "Pending",
      "data": {
        "fullName": "Ahmed Mohammed",
        "citizenship": "saudi",
        "nationalId": "1234567890",
        "email": "ahmed@example.com"
      }
    }
  ],
  "totalCount": 120,
  "page": 1,
  "pageSize": 20
}
```

#### GET `/api/forms/{formId}/submissions/{submissionId}`
Get single submission details.

#### PUT `/api/forms/{formId}/submissions/{submissionId}/status`
Update submission status.
```json
Request:
{
  "status": "Reviewed"
}

Response: 200 OK
```

#### GET `/api/forms/{id}/export`
Export form submissions to Excel.
```
Response: File Download (Excel)
```

---

### Albums & Photo Galleries

#### POST `/api/albums`
Create new album.
```json
Request:
{
  "defaultLanguage": "ar",
  "translations": {
    "ar": {
      "title": "معرض صور الاعتماد 2025",
      "slug": "ma3rad-sowar-2025",
      "description": "صور من حفل توزيع شهادات الاعتماد",
      "metaTitle": "معرض صور الاعتماد 2025 - غاهر",
      "metaDescription": "شاهد صور حفل توزيع شهادات الاعتماد"
    },
    "en": {
      "title": "Accreditation Ceremony 2025",
      "slug": "accreditation-ceremony-2025",
      "description": "Photos from the accreditation certificate ceremony",
      "metaTitle": "Accreditation Ceremony 2025 - GAHAR",
      "metaDescription": "View photos from the accreditation certificate ceremony"
    }
  }
}

Response: 201 Created
{
  "id": "guid",
  "slug": "ma3rad-sowar-2025",
  "title": "معرض صور الاعتماد 2025",
  "imagesCount": 0,
  "createdAt": "2025-11-10T10:00:00Z"
}
```

#### POST `/api/albums/{albumId}/images/bulk-upload`
Upload multiple images to album (bulk upload with progress tracking).
```
Request: multipart/form-data
files: [image1.jpg, image2.jpg, image3.jpg, ...] (up to 50 images per request)

Response: 202 Accepted
{
  "jobId": "guid",
  "totalFiles": 15,
  "message": "Images are being processed",
  "estimatedTime": "30 seconds"
}

// Background job processes each image:
// 1. Validate file type (jpg, png, gif, webp)
// 2. Validate file size (max 10MB per image)
// 3. Generate thumbnail (300x300)
// 4. Extract dimensions (width, height)
// 5. Calculate aspect ratio
// 6. Optimize and compress
// 7. Upload to storage (Azure Blob / S3)
// 8. Save to database
```

#### GET `/api/albums/{albumId}/images/upload-progress/{jobId}`
Check bulk upload progress (Server-Sent Events or polling).
```json
Response: 200 OK
{
  "jobId": "guid",
  "status": "processing", // "processing", "completed", "failed"
  "totalFiles": 15,
  "processedFiles": 8,
  "failedFiles": 0,
  "progress": 53, // Percentage
  "currentFile": "image_8.jpg",
  "errors": [],
  "completedAt": null
}

// When completed:
{
  "jobId": "guid",
  "status": "completed",
  "totalFiles": 15,
  "processedFiles": 15,
  "failedFiles": 0,
  "progress": 100,
  "errors": [],
  "completedAt": "2025-11-10T10:01:30Z",
  "images": [
    {
      "id": "guid",
      "fileUrl": "https://cdn.gahar.sa/albums/2025/img1.jpg",
      "thumbnailUrl": "https://cdn.gahar.sa/albums/2025/thumb_img1.jpg",
      "width": 1920,
      "height": 1080,
      "aspectRatio": 1.7778
    },
    // ... 14 more images
  ]
}
```

#### POST `/api/albums/{albumId}/images`
Upload single image to album.
```
Request: multipart/form-data
file: image.jpg
caption_ar: "وزير الصحة يسلم الشهادة"
caption_en: "Minister of Health presenting certificate"
altText_ar: "صورة توزيع الشهادات"
altText_en: "Certificate distribution photo"

Response: 201 Created
{
  "id": "guid",
  "albumId": "album-guid",
  "fileUrl": "https://cdn.gahar.sa/albums/2025/img1.jpg",
  "thumbnailUrl": "https://cdn.gahar.sa/albums/2025/thumb_img1.jpg",
  "fileName": "ceremony_photo.jpg",
  "fileSize": 2048576,
  "mimeType": "image/jpeg",
  "width": 1920,
  "height": 1080,
  "aspectRatio": 1.7778,
  "displayOrder": 1,
  "captions": {
    "ar": "وزير الصحة يسلم الشهادة",
    "en": "Minister of Health presenting certificate"
  },
  "uploadedAt": "2025-11-10T10:00:00Z"
}
```

#### GET `/api/albums?lang=ar&page=1&pageSize=10`
List all albums.
```json
Response: 200 OK
{
  "items": [
    {
      "id": "guid",
      "slug": "ma3rad-sowar-2025",
      "title": "معرض صور الاعتماد 2025",
      "description": "صور من حفل توزيع شهادات الاعتماد",
      "coverImage": {
        "url": "https://cdn.gahar.sa/albums/2025/cover.jpg",
        "thumbnailUrl": "https://cdn.gahar.sa/albums/2025/thumb_cover.jpg"
      },
      "imagesCount": 25,
      "isPublished": true,
      "publishedAt": "2025-11-01T10:00:00Z",
      "createdAt": "2025-11-01T09:00:00Z"
    }
  ],
  "totalCount": 12,
  "page": 1,
  "pageSize": 10
}
```

#### GET `/api/albums/{slug}?lang=ar`
Get album with all images (for collage layout).
```json
Response: 200 OK
{
  "id": "guid",
  "slug": "ma3rad-sowar-2025",
  "title": "معرض صور الاعتماد 2025",
  "description": "صور من حفل توزيع شهادات الاعتماد",
  "metaTitle": "معرض صور الاعتماد 2025 - غاهر",
  "metaDescription": "شاهد صور حفل توزيع شهادات الاعتماد",
  "images": [
    {
      "id": "guid-1",
      "fileUrl": "https://cdn.gahar.sa/albums/2025/img1.jpg",
      "thumbnailUrl": "https://cdn.gahar.sa/albums/2025/thumb_img1.jpg",
      "width": 1920,
      "height": 1080,
      "aspectRatio": 1.7778, // Landscape
      "caption": "وزير الصحة يسلم الشهادة",
      "altText": "صورة توزيع الشهادات",
      "displayOrder": 1
    },
    {
      "id": "guid-2",
      "fileUrl": "https://cdn.gahar.sa/albums/2025/img2.jpg",
      "thumbnailUrl": "https://cdn.gahar.sa/albums/2025/thumb_img2.jpg",
      "width": 1080,
      "height": 1920,
      "aspectRatio": 0.5625, // Portrait
      "caption": "المستشفى المعتمد",
      "altText": "صورة المستشفى",
      "displayOrder": 2
    },
    {
      "id": "guid-3",
      "fileUrl": "https://cdn.gahar.sa/albums/2025/img3.jpg",
      "thumbnailUrl": "https://cdn.gahar.sa/albums/2025/thumb_img3.jpg",
      "width": 1200,
      "height": 1200,
      "aspectRatio": 1.0, // Square
      "caption": "لوحة الشرف",
      "altText": "لوحة شرف المعتمدين",
      "displayOrder": 3
    }
    // ... more images with various aspect ratios
  ],
  "imagesCount": 25,
  "isPublished": true,
  "publishedAt": "2025-11-01T10:00:00Z"
}

// Frontend uses aspectRatio to calculate collage layout
// Algorithm balances rows to avoid gaps
```

#### PUT `/api/albums/{albumId}/images/reorder`
Reorder images in album.
```json
Request:
{
  "imageOrders": [
    { "imageId": "guid-1", "displayOrder": 1 },
    { "imageId": "guid-2", "displayOrder": 2 },
    { "imageId": "guid-3", "displayOrder": 3 }
  ]
}

Response: 200 OK
```

#### PUT `/api/albums/{albumId}/cover-image/{imageId}`
Set album cover image.
```json
Response: 200 OK
{
  "albumId": "guid",
  "coverImageId": "image-guid",
  "message": "Cover image updated"
}
```

#### DELETE `/api/albums/{albumId}/images/{imageId}`
Delete image from album.
```json
Response: 200 OK
{
  "message": "Image deleted successfully"
}
// Also deletes file from storage (Azure Blob / S3)
```

#### PUT `/api/albums/{albumId}/images/{imageId}/caption`
Update image caption (multilingual).
```json
Request:
{
  "captions": {
    "ar": "وزير الصحة في حفل التكريم",
    "en": "Minister of Health at the ceremony"
  },
  "altText": {
    "ar": "صورة الحفل",
    "en": "Ceremony photo"
  }
}

Response: 200 OK
```

#### DELETE `/api/albums/{id}`
Delete album (cascades to all images).

---

### Navigation & Menus Management

#### GET `/api/menus?location=header`
List menus by location.
```json
Response: 200 OK
{
  "items": [
    {
      "id": "guid",
      "name": "Main Menu",
      "location": "header",
      "isActive": true,
      "itemsCount": 8
    }
  ]
}
```

#### GET `/api/menus/{id}/items?lang=ar`
Get menu with all items and icons (hierarchical structure).
```json
Response: 200 OK
{
  "id": "guid",
  "name": "Main Menu",
  "location": "header",
  "items": [
    {
      "id": "guid-1",
      "title": "الرئيسية",
      "description": "العودة للصفحة الرئيسية",
      "icon": {
        "type": "lucide",
        "name": "Home",
        "color": "#1E40AF",
        "size": "md",
        "showIcon": true
      },
      "link": {
        "type": "internal",
        "url": "/ar",
        "openInNewTab": false
      },
      "displayOrder": 1,
      "children": [] // No dropdown
    },
    {
      "id": "guid-2",
      "title": "عن غاهر",
      "description": "تعرف على الهيئة",
      "icon": {
        "type": "lucide",
        "name": "Building",
        "color": "#1E40AF",
        "size": "md",
        "showIcon": true
      },
      "link": {
        "type": "page",
        "url": "/ar/about",
        "openInNewTab": false
      },
      "displayOrder": 2,
      "children": [
        {
          "id": "guid-2-1",
          "title": "من نحن",
          "description": null,
          "icon": {
            "type": "lucide",
            "name": "Users",
            "color": "#6B7280",
            "size": "sm",
            "showIcon": true
          },
          "link": {
            "type": "page",
            "url": "/ar/about/who-we-are"
          },
          "displayOrder": 1
        },
        {
          "id": "guid-2-2",
          "title": "الرؤية والرسالة",
          "icon": {
            "type": "lucide",
            "name": "Target",
            "color": "#6B7280",
            "size": "sm"
          },
          "link": {
            "type": "page",
            "url": "/ar/about/vision-mission"
          },
          "displayOrder": 2
        }
      ]
    },
    {
      "id": "guid-3",
      "title": "الأخبار",
      "icon": {
        "type": "lucide",
        "name": "Newspaper",
        "color": "#1E40AF",
        "size": "md"
      },
      "link": {
        "type": "content",
        "contentType": "news",
        "url": "/ar/news"
      },
      "displayOrder": 3,
      "children": []
    },
    {
      "id": "guid-4",
      "title": "الفعاليات",
      "icon": {
        "type": "emoji",
        "emoji": "📅",
        "showIcon": true
      },
      "link": {
        "type": "content",
        "contentType": "events",
        "url": "/ar/events"
      },
      "displayOrder": 4
    },
    {
      "id": "guid-5",
      "title": "تواصل معنا",
      "icon": {
        "type": "custom",
        "svg": "<svg>...</svg>",
        "color": "#10B981"
      },
      "link": {
        "type": "page",
        "url": "/ar/contact"
      },
      "displayOrder": 5
    }
  ]
}
```

#### POST `/api/menus`
Create new menu.
```json
Request:
{
  "name": "Footer Menu",
  "location": "footer"
}

Response: 201 Created
```

#### POST `/api/menus/{menuId}/items`
Add menu item with icon.
```json
Request:
{
  "translations": {
    "ar": {
      "title": "الخدمات",
      "description": "تصفح خدماتنا"
    },
    "en": {
      "title": "Services",
      "description": "Browse our services"
    }
  },
  "icon": {
    "type": "lucide",
    "name": "Briefcase",
    "color": "#1E40AF",
    "size": "md",
    "showIcon": true
  },
  "link": {
    "type": "page",
    "url": "/services",
    "openInNewTab": false
  },
  "parentId": null,
  "displayOrder": 3,
  "cssClass": "menu-item-services"
}

Response: 201 Created
{
  "id": "guid",
  "menuId": "menu-guid",
  "title": "الخدمات",
  "iconType": "lucide",
  "iconName": "Briefcase",
  "createdAt": "2025-11-10T10:00:00Z"
}
```

#### PUT `/api/menus/{menuId}/items/{itemId}`
Update menu item (including icon).
```json
Request:
{
  "translations": {
    "ar": {
      "title": "الخدمات المحدثة"
    }
  },
  "icon": {
    "type": "lucide",
    "name": "Star", // Changed icon
    "color": "#F59E0B",
    "size": "lg"
  }
}

Response: 200 OK
```

#### PUT `/api/menus/{menuId}/items/reorder`
Reorder menu items.
```json
Request:
{
  "itemOrders": [
    { "itemId": "guid-1", "displayOrder": 1 },
    { "itemId": "guid-2", "displayOrder": 2 },
    { "itemId": "guid-3", "displayOrder": 3 }
  ]
}

Response: 200 OK
```

#### DELETE `/api/menus/{menuId}/items/{itemId}`
Delete menu item.

#### GET `/api/icons?library=lucide&search=home&page=1&pageSize=50`
Search available icons (for icon picker).
```json
Response: 200 OK
{
  "library": "lucide",
  "items": [
    {
      "name": "Home",
      "category": "Buildings",
      "tags": ["house", "main", "dashboard"],
      "svg": "<svg>...</svg>",
      "preview": "data:image/svg+xml;base64,..."
    },
    {
      "name": "House",
      "category": "Buildings",
      "tags": ["home", "residence"],
      "svg": "<svg>...</svg>",
      "preview": "data:image/svg+xml;base64,..."
    }
  ],
  "totalCount": 45,
  "libraries": [
    {
      "id": "lucide",
      "name": "Lucide Icons",
      "count": 1200,
      "url": "https://lucide.dev"
    },
    {
      "id": "heroicons",
      "name": "Heroicons",
      "count": 230,
      "url": "https://heroicons.com"
    },
    {
      "id": "fontawesome",
      "name": "Font Awesome",
      "count": 2000,
      "url": "https://fontawesome.com"
    }
  ]
}
```

---

### Search

#### GET `/api/search?q=مستشفى&lang=ar&type=facilities&page=1`
Global multilingual search.
```json
Response: 200 OK
{
  "query": "مستشفى",
  "results": [
    {
      "type": "facility",
      "id": "guid",
      "title": "مستشفى الملك فهد",
      "excerpt": "مستشفى متخصص في...",
      "url": "/ar/facilities/مستشفى-الملك-فهد",
      "score": 0.95
    },
    {
      "type": "content",
      "id": "guid",
      "title": "افتتاح مستشفى جديد",
      "excerpt": "تم افتتاح مستشفى...",
      "url": "/ar/news/افتتاح-مستشفى-جديد",
      "score": 0.87
    }
  ],
  "totalCount": 15
}
```

---

### File Management

#### POST `/api/files/upload`
Upload file (image, PDF, etc.).
```
Request: multipart/form-data
file: document.pdf
folder: "certificates" (optional)

Response: 201 Created
{
  "fileId": "guid",
  "url": "/uploads/certificates/document-abc123.pdf",
  "fileName": "document.pdf",
  "fileSize": 2048576,
  "mimeType": "application/pdf"
}
```

#### DELETE `/api/files/{fileId}`
Delete uploaded file.

---

### Translation Utility

#### POST `/api/utils/translate`
Auto-translate content using AI.
```json
Request:
{
  "text": "Welcome to GAHAR",
  "sourceLang": "en",
  "targetLang": "ar"
}

Response: 200 OK
{
  "translatedText": "مرحباً بكم في غاهر",
  "confidence": 0.98
}
```

---

## 🔐 Authentication & Authorization

### JWT Token Structure
```json
{
  "sub": "user-id",
  "email": "admin@gahar.sa",
  "name": "Admin User",
  "roles": ["SuperAdmin", "Editor"],
  "permissions": ["content.create", "content.publish"],
  "iat": 1699523400,
  "exp": 1699527000
}
```

### Roles
- **SuperAdmin:** Full system access
- **Admin:** Manage content, users, and settings
- **Editor:** Create and edit content (cannot publish)
- **Viewer:** Read-only access
- **Public:** Anonymous access to published content

### Policies
```csharp
// Policy definitions in Program.cs
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanPublishContent", policy =>
        policy.RequireRole("SuperAdmin", "Admin"));
    
    options.AddPolicy("CanManageUsers", policy =>
        policy.RequireRole("SuperAdmin"));
    
    options.AddPolicy("CanEditContent", policy =>
        policy.RequireRole("SuperAdmin", "Admin", "Editor"));
});
```

### Endpoint Protection
```csharp
// Example endpoint with authorization
app.MapPost("/api/content/{type}", async (
    string type,
    CreateContentRequest request,
    IContentService contentService) =>
{
    var content = await contentService.CreateAsync(type, request);
    return Results.Created($"/api/content/{type}/{content.Id}", content);
})
.RequireAuthorization("CanEditContent");
```

---

## 📡 Event-Driven Architecture

### Event Bus Configuration
```csharp
// RabbitMQ setup in Program.cs
builder.Services.AddSingleton<IRabbitMQConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new RabbitMQConnection(config["RabbitMQ:HostName"]);
});

builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();
```

### Event Contracts (GAHAR.Shared.Events)

#### FormSubmittedEvent
```csharp
public record FormSubmittedEvent(
    Guid SubmissionId,
    Guid FormId,
    string FormName,
    string SubmitterEmail,
    string SubmitterName,
    Dictionary<string, object> SubmissionData,
    DateTime SubmittedAt
);
```

#### ContentPublishedEvent
```csharp
public record ContentPublishedEvent(
    Guid ContentId,
    string ContentType,
    string Slug,
    string Language,
    DateTime PublishedAt
);
```

#### NewSubscriberEvent
```csharp
public record NewSubscriberEvent(
    string Email,
    string Name,
    string Language,
    DateTime SubscribedAt
);
```

### Publishing Events
```csharp
// In FormService.cs
public async Task<FormSubmission> SubmitFormAsync(Guid formId, Dictionary<string, object> data)
{
    var submission = new FormSubmission
    {
        FormId = formId,
        SubmissionData = JsonSerializer.Serialize(data),
        SubmittedAt = DateTime.UtcNow
    };
    
    await _unitOfWork.FormSubmissions.AddAsync(submission);
    await _unitOfWork.SaveChangesAsync();
    
    // Publish event
    var @event = new FormSubmittedEvent(
        submission.Id,
        formId,
        submission.Form.Name,
        data.GetValueOrDefault("email")?.ToString(),
        data.GetValueOrDefault("name")?.ToString(),
        data,
        submission.SubmittedAt
    );
    
    await _eventBus.PublishAsync(@event);
    
    return submission;
}
```

### Consuming Events
```csharp
// FormSubmissionConsumer.cs
public class FormSubmissionConsumer : IEventConsumer<FormSubmittedEvent>
{
    private readonly IEmailService _emailService;
    private readonly IHubContext<NotificationHub> _hubContext;
    
    public async Task HandleAsync(FormSubmittedEvent @event)
    {
        // Send email notification
        await _emailService.SendAsync(new EmailMessage
        {
            To = @event.SubmitterEmail,
            Subject = "Form Submission Received",
            Body = $"Thank you {@event.SubmitterName}, we received your submission."
        });
        
        // Send real-time notification to admin dashboard
        await _hubContext.Clients.Group("Admins").SendAsync(
            "NewFormSubmission",
            new { @event.FormName, @event.SubmitterName, @event.SubmittedAt }
        );
    }
}
```

---

## 🛠️ Infrastructure Services

### Caching Strategy
```csharp
// RedisCacheService.cs
public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;
    
    public async Task<T?> GetAsync<T>(string key)
    {
        var db = _redis.GetDatabase();
        var value = await db.StringGetAsync(key);
        return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value);
    }
    
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var db = _redis.GetDatabase();
        var serialized = JsonSerializer.Serialize(value);
        await db.StringSetAsync(key, serialized, expiry);
    }
}

// Usage in ContentService
public async Task<ContentResponse> GetBySlugAsync(string slug, string language)
{
    var cacheKey = $"content:{slug}:{language}";
    
    var cached = await _cacheService.GetAsync<ContentResponse>(cacheKey);
    if (cached != null) return cached;
    
    var content = await _repository.GetBySlugAsync(slug, language);
    var response = _mapper.Map<ContentResponse>(content);
    
    await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));
    
    return response;
}
```

### Background Jobs (Hangfire)
```csharp
// Program.cs
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddHangfireServer();

// FacilityImportJob.cs
public class FacilityImportJob
{
    public async Task ProcessExcelFileAsync(string filePath)
    {
        using var workbook = new XLWorkbook(filePath);
        var worksheet = workbook.Worksheet(1);
        
        foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header
        {
            var facility = new Facility
            {
                FacilityCode = row.Cell(1).GetString(),
                Latitude = row.Cell(4).GetValue<decimal>(),
                Longitude = row.Cell(5).GetValue<decimal>()
            };
            
            var translationAr = new FacilityTranslation
            {
                FacilityId = facility.Id,
                Language = "ar",
                Name = row.Cell(2).GetString(),
                Slug = SlugGenerator.Generate(row.Cell(2).GetString()),
                City = row.Cell(6).GetString()
            };
            
            await _facilityRepository.AddAsync(facility, translationAr);
        }
        
        await _unitOfWork.SaveChangesAsync();
    }
}

// Schedule job
BackgroundJob.Enqueue<FacilityImportJob>(job => 
    job.ProcessExcelFileAsync("/uploads/facilities.xlsx"));
```

### Logging (Serilog)
```csharp
// Program.cs
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "GAHAR.CMS")
    .WriteTo.Console()
    .WriteTo.File("logs/gahar-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.PostgreSQL(
        connectionString: builder.Configuration.GetConnectionString("Default"),
        tableName: "Logs",
        needAutoCreateTable: true)
    .CreateLogger();

builder.Host.UseSerilog();

// Usage in services
_logger.LogInformation(
    "Content {ContentType} created with ID {ContentId} by user {UserId}",
    contentType, content.Id, userId);
```

---

## 🚀 Deployment & DevOps

### Docker Support
```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["GAHAR.API/GAHAR.API.csproj", "GAHAR.API/"]
RUN dotnet restore "GAHAR.API/GAHAR.API.csproj"
COPY . .
WORKDIR "/src/GAHAR.API"
RUN dotnet build "GAHAR.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GAHAR.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GAHAR.API.dll"]
```

```yaml
# docker-compose.yml
version: '3.8'

services:
  gahar-api:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5000:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__Default=Host=postgres;Database=gahar_cms;Username=postgres;Password=postgres
      - RabbitMQ__HostName=rabbitmq
      - Redis__ConnectionString=redis:6379
    depends_on:
      - postgres
      - rabbitmq
      - redis

  postgres:
    image: postgres:16
    environment:
      POSTGRES_DB: gahar_cms
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
    volumes:
      - postgres_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"

  rabbitmq:
    image: rabbitmq:3-management
    ports:
      - "5672:5672"
      - "15672:15672"
    volumes:
      - rabbitmq_data:/var/lib/rabbitmq

  redis:
    image: redis:7
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data

volumes:
  postgres_data:
  rabbitmq_data:
  redis_data:
```

### CI/CD Pipeline (GitHub Actions)
```yaml
# .github/workflows/deploy.yml
name: Deploy GAHAR CMS

on:
  push:
    branches: [ main ]

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '9.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore -c Release
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
    
    - name: Publish
      run: dotnet publish GAHAR.API/GAHAR.API.csproj -c Release -o ./publish
    
    - name: Build Docker image
      run: docker build -t gahar-cms:latest .
    
    - name: Push to registry
      run: |
        echo "${{ secrets.DOCKER_PASSWORD }}" | docker login -u "${{ secrets.DOCKER_USERNAME }}" --password-stdin
        docker tag gahar-cms:latest ${{ secrets.DOCKER_REGISTRY }}/gahar-cms:latest
        docker push ${{ secrets.DOCKER_REGISTRY }}/gahar-cms:latest
```

### Environment Configuration
```json
// appsettings.Production.json
{
  "ConnectionStrings": {
    "Default": "Host=prod-db.gahar.sa;Database=gahar_cms;Username=gahar_user;Password=***"
  },
  "Jwt": {
    "SecretKey": "***",
    "Issuer": "https://api.gahar.sa",
    "Audience": "https://gahar.sa",
    "ExpiryMinutes": 60
  },
  "RabbitMQ": {
    "HostName": "rabbitmq.gahar.sa",
    "UserName": "gahar",
    "Password": "***"
  },
  "Redis": {
    "ConnectionString": "redis.gahar.sa:6379,password=***"
  },
  "FileStorage": {
    "Provider": "AzureBlob",
    "AzureBlob": {
      "ConnectionString": "***",
      "ContainerName": "gahar-uploads"
    }
  },
  "Email": {
    "Provider": "SendGrid",
    "SendGrid": {
      "ApiKey": "***",
      "FromEmail": "noreply@gahar.sa",
      "FromName": "GAHAR"
    }
  },
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      {
        "Name": "ApplicationInsights",
        "Args": {
          "instrumentationKey": "***"
        }
      }
    ]
  }
}
```

---

## 📊 Performance Optimization

### Native AOT for Certificate Validation
```csharp
// CertificateValidationEndpoint.cs (AOT-optimized)
[JsonSerializable(typeof(CertificateValidationResponse))]
internal partial class CertificateJsonContext : JsonSerializerContext { }

app.MapGet("/api/certificates/validate/{number}", 
    [EnableCors("PublicApi")] async (
        string number,
        ICertificateRepository repository,
        HttpContext context) =>
{
    var cert = await repository.GetByNumberAsync(number);
    
    if (cert == null)
        return Results.NotFound(new { message = "Certificate not found" });
    
    var response = new CertificateValidationResponse
    {
        IsValid = cert.Status == "Valid" && cert.ExpiryDate > DateTime.UtcNow,
        CertificateNumber = cert.CertificateNumber,
        Status = cert.Status,
        ExpiryDate = cert.ExpiryDate
    };
    
    return Results.Json(response, CertificateJsonContext.Default.CertificateValidationResponse);
})
.CacheOutput(policy => policy.Expire(TimeSpan.FromMinutes(10)));
```

### Database Indexing & Query Optimization
```csharp
// Efficient multilingual content query
public async Task<List<ContentResponse>> GetPublishedContentAsync(
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
        .Select(c => new ContentResponse
        {
            Id = c.Id,
            Slug = c.Translations.First().Slug,
            Title = c.Translations.First().Title,
            Excerpt = c.Translations.First().Body.Substring(0, 200),
            PublishedAt = c.PublishedAt.Value
        })
        .AsNoTracking()
        .ToListAsync();
}
```

---

## 🧪 Testing Strategy

### Unit Tests
```csharp
// ContentServiceTests.cs
public class ContentServiceTests
{
    private readonly Mock<IContentRepository> _repositoryMock;
    private readonly ContentService _service;
    
    [Fact]
    public async Task CreateContent_ShouldGenerateSlug_WhenNotProvided()
    {
        // Arrange
        var request = new CreateContentRequest
        {
            DefaultLanguage = "ar",
            Translations = new Dictionary<string, TranslationDto>
            {
                ["ar"] = new() { Title = "محتوى جديد" }
            }
        };
        
        // Act
        var result = await _service.CreateAsync("news", request);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("محتوى-جديد", result.Translations["ar"].Slug);
    }
}
```

### Integration Tests
```csharp
// ContentEndpointsTests.cs
public class ContentEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    [Fact]
    public async Task GetContent_ReturnsArabicContent_WhenLanguageIsAr()
    {
        // Arrange
        var request = new HttpRequestMessage(
            HttpMethod.Get, 
            "/api/content/news/test-slug?lang=ar");
        
        // Act
        var response = await _client.SendAsync(request);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<ContentResponse>();
        Assert.NotNull(content);
        Assert.Equal("ar", content.Language);
    }
}
```

---

## 📚 Additional Resources

- **Swagger Documentation:** `/swagger` (development only)
- **Hangfire Dashboard:** `/hangfire` (admin only)
- **Health Checks:** `/health`
- **Metrics:** `/metrics` (Prometheus format)

---

## ✅ Backend Deliverables Checklist

- [ ] Solution structure with 4 projects created
- [ ] Database schema implemented with migrations
- [ ] ASP.NET Core Identity + JWT authentication
- [ ] All CRUD endpoints for Content, Pages, Facilities
- [ ] Translation Table Pattern fully implemented
- [ ] Event Bus with RabbitMQ configured
- [ ] Hangfire background jobs setup
- [ ] File upload service with Azure Blob
- [ ] Redis caching layer
- [ ] Serilog structured logging
- [ ] Global error handling middleware
- [ ] Rate limiting configured
- [ ] Swagger documentation
- [ ] Unit tests (80% coverage)
- [ ] Integration tests for critical paths
- [ ] Docker containerization
- [ ] CI/CD pipeline configured
- [ ] Production environment variables
- [ ] Performance optimization (caching, indexing)
- [ ] Security audit completed

---

**Document Version:** 1.0  
**Last Updated:** November 9, 2025  
**Maintained By:** Backend Development Team
