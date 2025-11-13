# 📖 Feature 1: Page Builder System - README

**Status:** ✅ **COMPLETE - READY FOR TESTING**  
**Last Updated:** 11 يناير 2025  
**Version:** 1.0.0  

---

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- SQL Server 2019+
- Visual Studio 2022 or VS Code

### Setup

1. **Clone Repository**
   ```bash
   git clone https://github.com/tr-wa2el/Gahar_web_BackEnd.git
   cd Gahar_Backend
   ```

2. **Install Dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure Database**
- Update `appsettings.json` with connection string
   - Run migrations:
   ```bash
dotnet ef database update
   ```

4. **Run Application**
   ```bash
   dotnet run
   ```

5. **Access Swagger**
 - Open: `https://localhost:7XXX/swagger`
   - All endpoints documented

---

## 📁 Project Structure

```
Controllers/
├── PagesController.cs (13 endpoints)

Models/
├── Entities/
│   ├── Page.cs
│ └── PageBlock.cs
└── DTOs/
    ├── Common/
    │   ├── PagedResult.cs
    │   └── PageFilterDto.cs
    └── Page/
    └── PageDtos.cs (9 DTOs)

Repositories/
├── Interfaces/
│   ├── IPageRepository.cs
│   └── IPageBlockRepository.cs
└── Implementations/
    ├── PageRepository.cs
    └── PageBlockRepository.cs

Services/
├── Interfaces/
│   └── IPageService.cs
└── Implementations/
    └── PageService.cs (13 methods)

Constants/
├── BlockTypes.cs (14 types)
└── Permissions.cs

Data/
├── Configurations/
│   ├── PageConfiguration.cs
│   └── PageBlockConfiguration.cs
├── ApplicationDbContext.cs
└── Migrations/

docs/
├── FEATURE_1_COMPLETE.md
├── FEATURE_1_TESTING_GUIDE.md
└── FEATURE_1_FINAL_SUMMARY.md
```

---

## 🎯 API Endpoints

### Pages Management

#### List Pages
```bash
GET /api/pages?pageNumber=1&pageSize=10&searchTerm=&isPublished=true&sortBy=createdAt&sortOrder=desc
```

#### Get Page
```bash
GET /api/pages/{id}
```

#### Get Page by Slug (Public)
```bash
GET /api/pages/slug/{slug}
```

#### Create Page
```bash
POST /api/pages
Content-Type: application/json
Authorization: Bearer {token}

{
  "title": "Page Title",
  "slug": "page-slug",
  "description": "Description",
  "metaTitle": "Meta Title",
  "metaDescription": "Meta Description",
"template": "Default",
  "showTitle": true,
  "showBreadcrumbs": true
}
```

#### Update Page
```bash
PUT /api/pages/{id}
Content-Type: application/json
Authorization: Bearer {token}

{
  "title": "Updated Title",
  "slug": "page-slug",
  ...
  "isPublished": true
}
```

#### Delete Page
```bash
DELETE /api/pages/{id}
Authorization: Bearer {token}
```

### Publishing

#### Publish Page
```bash
POST /api/pages/{id}/publish
Authorization: Bearer {token}
```

#### Unpublish Page
```bash
POST /api/pages/{id}/unpublish
Authorization: Bearer {token}
```

### Block Management

#### Add Block
```bash
POST /api/pages/{id}/blocks
Content-Type: application/json
Authorization: Bearer {token}

{
  "blockType": "HeroSection",
  "configuration": {
    "backgroundImage": "/uploads/hero.jpg",
    "heading": "Welcome",
    "subheading": "Subtitle",
    "textAlign": "center"
  },
  "isVisible": true,
  "cssClass": "hero-main",
  "customId": "hero-1"
}
```

#### Update Block
```bash
PUT /api/pages/{id}/blocks/{blockId}
Authorization: Bearer {token}

{
"blockType": "TextBlock",
  "configuration": {...},
  "displayOrder": 1,
  "isVisible": true
}
```

#### Delete Block
```bash
DELETE /api/pages/{id}/blocks/{blockId}
Authorization: Bearer {token}
```

#### Reorder Blocks
```bash
POST /api/pages/{id}/reorder-blocks
Authorization: Bearer {token}

{
  "blockIds": [3, 1, 2]
}
```

#### Duplicate Page
```bash
POST /api/pages/{id}/duplicate
Authorization: Bearer {token}
```

---

## 🧱 Block Types (14 Available)

1. **HeroSection** - Hero banner with background and CTA
2. **TextBlock** - Rich text content
3. **ImageGallery** - Image grid layout
4. **CtaButton** - Call to action button
5. **StatsCounter** - Statistics display
6. **TeamGrid** - Team members grid
7. **FaqAccordion** - FAQ section
8. **ContactForm** - Contact form
9. **EmbeddedVideo** - Embedded video player
10. **Timeline** - Timeline display
11. **CustomHtml** - Custom HTML block
12. **ContentList** - List of content items
13. **LatestNews** - Latest news display
14. **FeaturedContent** - Featured content showcase

---

## 🔐 Permissions

Required permissions for endpoints:

| Endpoint | Permission | Required |
|----------|-----------|----------|
| GET /api/pages | Pages.View | No* |
| GET /api/pages/{id} | Pages.View | No* |
| GET /api/pages/slug/{slug} | None | No |
| POST /api/pages | Pages.Create | Yes |
| PUT /api/pages/{id} | Pages.Edit | Yes |
| DELETE /api/pages/{id} | Pages.Delete | Yes |
| POST /api/pages/{id}/publish | Pages.Publish | Yes |
| POST /api/pages/{id}/blocks | Pages.Edit | Yes |
| PUT /api/pages/{id}/blocks/{blockId} | Pages.Edit | Yes |
| DELETE /api/pages/{id}/blocks/{blockId} | Pages.Delete | Yes |
| POST /api/pages/{id}/reorder-blocks | Pages.Edit | Yes |
| POST /api/pages/{id}/duplicate | Pages.Create | Yes |

*\* Authentication required for admin panels, not for public API*

---

## 📊 Data Model

### Page Entity
```csharp
public class Page : TranslatableEntity
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string? Description { get; set; }
    
    // SEO
public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    
    // OpenGraph
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
    
    // Publishing
    public bool IsPublished { get; set; }
  public DateTime? PublishedAt { get; set; }
    
    // Author
    public int? AuthorId { get; set; }
    public User? Author { get; set; }
    
    // Template
    public string Template { get; set; }
    public bool ShowTitle { get; set; }
  public bool ShowBreadcrumbs { get; set; }
    
    // Navigation
    public ICollection<PageBlock> Blocks { get; set; }
}
```

### PageBlock Entity
```csharp
public class PageBlock : BaseEntity
{
    public int PageId { get; set; }
    public Page Page { get; set; }
  
    public string BlockType { get; set; }
    public string Configuration { get; set; } // JSON
    public int DisplayOrder { get; set; }
  public bool IsVisible { get; set; }
    
    public string? CssClass { get; set; }
    public string? CustomId { get; set; }
}
```

---

## 💻 Code Examples

### Create a Page with Blocks

```csharp
// Create page
var createPageDto = new CreatePageDto
{
    Title = "Welcome",
    Slug = "welcome",
    Description = "Welcome to our site",
    MetaTitle = "Welcome Page",
    MetaDescription = "Welcome to our site description",
    Template = "Default",
    ShowTitle = true,
    ShowBreadcrumbs = true
};

// Send via API
POST /api/pages
Authorization: Bearer {token}
{
    // ... createPageDto fields
}

// Response: 201 Created
{
    "id": 1,
    "title": "Welcome",
    "slug": "welcome",
    ...
}
```

### Add Block to Page

```csharp
var createBlockDto = new CreatePageBlockDto
{
    BlockType = "HeroSection",
    Configuration = new
    {
        backgroundImage = "/uploads/hero.jpg",
        heading = "مرحباً",
        subheading = "Welcome",
 textAlign = "center"
    },
    IsVisible = true
};

POST /api/pages/1/blocks
Authorization: Bearer {token}
{
    // ... createBlockDto fields
}
```

### Get Published Page

```csharp
// Get published page by slug (no authentication needed)
GET /api/pages/slug/welcome

Response: 200 OK
{
    "id": 1,
    "title": "Welcome",
    "slug": "welcome",
    "isPublished": true,
    "blocks": [
        {
         "id": 1,
    "blockType": "HeroSection",
            "configuration": {...},
            "displayOrder": 1
        },
        ...
    ]
}
```

---

## 🧪 Testing

### Using Swagger UI
1. Go to `https://localhost:7XXX/swagger`
2. Click on any endpoint
3. Click "Try it out"
4. Fill in parameters
5. Click "Execute"

### Using Postman
1. Import Postman collection (coming soon)
2. Set Base URL: `https://localhost:7XXX`
3. Add Authorization header with JWT token
4. Execute requests

### Using cURL
```bash
# Create page
curl -X POST https://localhost:7XXX/api/pages \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "title": "Test",
    "slug": "test",
    ...
  }'

# Get page
curl https://localhost:7XXX/api/pages/1

# Publish page
curl -X POST https://localhost:7XXX/api/pages/1/publish \
  -H "Authorization: Bearer {token}"
```

---

## 🔧 Database Migration

### Manual Migration
```bash
# Create migration
dotnet ef migrations add AddPageBuilderTables

# Apply migration
dotnet ef database update

# Rollback migration (if needed)
dotnet ef database update -1
```

### Automatic Migration (on startup)
```csharp
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
}
```

---

## 📝 Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GaharDB;User Id=sa;Password=YourPassword;TrustServerCertificate=true"
  },
  "JwtSettings": {
    "Issuer": "https://gahar.sa",
    "Audience": "https://gahar.sa",
    "SecretKey": "your-secret-key-min-32-characters",
    "ExpirationMinutes": 60
  }
}
```

---

## 🐛 Troubleshooting

### Migration Error
**Error:** "The DbContext of type 'ApplicationDbContext' does not include the requested..."

**Solution:** Run `dotnet ef database update` to apply pending migrations

### Slug Already Exists
**Error:** "Slug 'page-slug' already exists"

**Solution:** Use a unique slug for the page

### Page Not Published
**Error:** "Page is not published"

**Solution:** Publish the page first with `POST /api/pages/{id}/publish`

### Invalid Block Type
**Error:** "Invalid block type: CustomType"

**Solution:** Use one of the 14 supported block types

### Unauthorized Access
**Error:** "401 Unauthorized"

**Solution:** Add valid JWT token in Authorization header

---

## 📚 Additional Resources

### Documentation Files
- `FEATURE_1_COMPLETE.md` - Complete feature overview
- `FEATURE_1_TESTING_GUIDE.md` - 30+ test cases
- `FEATURE_1_FINAL_SUMMARY.md` - Project summary

### Code Files
- Entities: `/Models/Entities/`
- DTOs: `/Models/DTOs/`
- Controllers: `/Controllers/`
- Services: `/Services/`
- Repositories: `/Repositories/`

### External Resources
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Swagger/OpenAPI](https://swagger.io)

---

## 🚀 Performance Optimization

### Database Indexes
- ✅ Unique index on Pages.Slug
- ✅ Composite index on PageBlocks(PageId, DisplayOrder)

### Query Optimization
- ✅ Explicit Include() for related data
- ✅ Pagination for large datasets
- ✅ Filtered queries at database level

### Caching (Future)
- Plan to add Redis caching for published pages
- Cache invalidation on publish

---

## 📈 Scalability

### Current Support
- ✅ Unlimited pages
- ✅ Unlimited blocks per page
- ✅ 14 block types
- ✅ Pagination support
- ✅ Filtering and sorting

### Future Enhancements
- [ ] Page versioning
- [ ] Page scheduling
- [ ] Page templates
- [ ] Block templates
- [ ] Bulk operations
- [ ] Real-time collaboration

---

## 🎯 Success Criteria

- ✅ All 13 endpoints working
- ✅ Full CRUD operations
- ✅ Block management
- ✅ Publishing workflow
- ✅ SEO metadata
- ✅ Pagination and filtering
- ✅ Error handling
- ✅ Security and authorization
- ✅ API documentation
- ✅ Code documentation

---

## 📞 Support

### Questions?
- Check the documentation files
- Review code comments
- Check Swagger documentation

### Report Issues?
- Create GitHub issue
- Include error message
- Include reproduction steps

### Request Features?
- Create GitHub discussion
- Include use case
- Include priority

---

## 📄 License

This project is part of Gahar CMS and follows the repository's license.

---

## 👥 Contributors

- Developed by: Copilot
- Date: 11 يناير 2025
- Status: ✅ Complete

---

## 🎉 Ready to Use!

Feature 1: Page Builder System is complete and ready for:
- ✅ Integration testing
- ✅ UAT (User Acceptance Testing)
- ✅ Production deployment
- ✅ Feature 2-6 development

**Enjoy building amazing pages! 🚀**

---

**Last Update:** 11 يناير 2025  
**Version:** 1.0.0-complete  
**Status:** ✅ Ready for Testing
