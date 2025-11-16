# 🚀 دليل تنفيذ خطة المطور الثاني - مقسم إلى Features
## Developer 2 - Step-by-Step Implementation Guide

**تاريخ الإنشاء:** 11 يناير 2025  
**الهدف:** تنفيذ Features Package B بطريقة منظمة ومختبرة

---

## 📋 جدول المحتويات
1. [نظرة عامة على التقسيم](#نظرة-عامة-على-التقسيم)
2. [Feature 1: Page Builder System](#feature-1-page-builder-system)
3. [Feature 2: Form Builder System](#feature-2-form-builder-system)
4. [Feature 3: Navigation Menu System](#feature-3-navigation-menu-system)
5. [Feature 4: Facilities Management](#feature-4-facilities-management)
6. [Feature 5: Certificates Management](#feature-5-certificates-management)
7. [Feature 6: SEO & Analytics](#feature-6-seo--analytics)
8. [Integration Testing Guide](#integration-testing-guide)

---

## نظرة عامة على التقسيم

### 🎯 استراتيجية التنفيذ
كل Feature مقسم إلى **4 مراحل رئيسية**:

1. **Phase 1: Database Layer** (Models + Configurations + Migration)
2. **Phase 2: Business Logic Layer** (Repositories + Services)
3. **Phase 3: API Layer** (DTOs + Controllers)
4. **Phase 4: Testing & Validation** (Unit Tests + Integration Tests)

### ✅ Checklist عام لكل Feature
- [ ] Phase 1: Database Layer كامل
- [ ] Phase 2: Business Logic Layer كامل
- [ ] Phase 3: API Layer كامل
- [ ] Phase 4: Testing كامل
- [ ] Build بدون أخطاء
- [ ] Integration Test ناجح
- [ ] API Documentation محدثة

---

## Feature 1: Page Builder System
### المدة المتوقعة: 5-7 أيام

---

### 📌 Phase 1: Database Layer (يوم 1)

#### Step 1.1: إنشاء Models

**الملف:** `Models/Entities/Page.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class Page : TranslatableEntity
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Slug { get; set; } = string.Empty;

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

**الملف:** `Models/Entities/PageBlock.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities;

public class PageBlock : BaseEntity
{
    public int PageId { get; set; }
    public Page Page { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string BlockType { get; set; } = string.Empty;

    [Required]
    public string Configuration { get; set; } = "{}"; // JSON configuration

    public int DisplayOrder { get; set; }

    public bool IsVisible { get; set; } = true;

    [StringLength(100)]
    public string? CssClass { get; set; }

    [StringLength(100)]
    public string? CustomId { get; set; }
}
```

#### Step 1.2: إنشاء Constants

**الملف:** `Constants/BlockTypes.cs`

```csharp
namespace Gahar_Backend.Constants;

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
    public const string ContentList = "ContentList";
    public const string LatestNews = "LatestNews";
    public const string FeaturedContent = "FeaturedContent";

  public static readonly string[] AllBlockTypes = new[]
    {
        HeroSection, TextBlock, ImageGallery, CtaButton, StatsCounter,
        TeamGrid, FaqAccordion, ContactForm, EmbeddedVideo, Timeline,
        CustomHtml, ContentList, LatestNews, FeaturedContent
  };

    public static bool IsValid(string blockType)
    {
        return AllBlockTypes.Contains(blockType);
    }
}
```

#### Step 1.3: إنشاء Entity Configurations

**الملف:** `Data/Configurations/PageConfiguration.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class PageConfiguration : IEntityTypeConfiguration<Page>
{
    public void Configure(EntityTypeBuilder<Page> builder)
    {
    builder.ToTable("Pages");

     builder.HasIndex(p => p.Slug).IsUnique();

        builder.HasOne(p => p.Author)
            .WithMany()
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(p => p.Blocks)
  .WithOne(b => b.Page)
         .HasForeignKey(b => b.PageId)
    .OnDelete(DeleteBehavior.Cascade);
    }
}
```

**الملف:** `Data/Configurations/PageBlockConfiguration.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations;

public class PageBlockConfiguration : IEntityTypeConfiguration<PageBlock>
{
    public void Configure(EntityTypeBuilder<PageBlock> builder)
    {
        builder.ToTable("PageBlocks");

        builder.HasIndex(pb => new { pb.PageId, pb.DisplayOrder });
    }
}
```

#### Step 1.4: تحديث ApplicationDbContext

**إضافة إلى:** `Data/ApplicationDbContext.cs`

```csharp
// في الـ DbSets
public DbSet<Page> Pages { get; set; }
public DbSet<PageBlock> PageBlocks { get; set; }

// في OnModelCreating
modelBuilder.ApplyConfiguration(new PageConfiguration());
modelBuilder.ApplyConfiguration(new PageBlockConfiguration());
```

#### Step 1.5: إنشاء وتشغيل Migration

```bash
dotnet ef migrations add AddPageBuilderTables
dotnet ef database update
```

#### ✅ Checklist Phase 1
- [ ] إنشاء Page Model
- [ ] إنشاء PageBlock Model
- [ ] إنشاء BlockTypes Constants
- [ ] إنشاء PageConfiguration
- [ ] إنشاء PageBlockConfiguration
- [ ] تحديث ApplicationDbContext
- [ ] Migration ناجح
- [ ] Database Tables موجودة

---

### 📌 Phase 2: Business Logic Layer (يوم 2-3)

#### Step 2.1: إنشاء Common DTOs (إذا لم تكن موجودة)

**الملف:** `Models/DTOs/Common/PagedResult.cs`

```csharp
namespace Gahar_Backend.Models.DTOs.Common;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
  public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
```

**الملف:** `Models/DTOs/Common/PageFilterDto.cs`

```csharp
namespace Gahar_Backend.Models.DTOs.Common;

public class PageFilterDto
{
    public string? SearchTerm { get; set; }
    public bool? IsPublished { get; set; }
    public int? AuthorId { get; set; }
    public string? Template { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortOrder { get; set; } = "desc";
}
```

#### Step 2.2: إنشاء DTOs للـ Page

**الملف:** `Models/DTOs/Page/PageDtos.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Page;

public class PageListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
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
    public string Template { get; set; } = "Default";
    public bool ShowTitle { get; set; }
    public bool ShowBreadcrumbs { get; set; }
    public List<PageBlockDto> Blocks { get; set; } = new();
}

public class PageBlockDto
{
    public int Id { get; set; }
    public string BlockType { get; set; } = string.Empty;
    public object Configuration { get; set; } = new { };
    public int DisplayOrder { get; set; }
    public bool IsVisible { get; set; }
    public string? CssClass { get; set; }
    public string? CustomId { get; set; }
}

public class CreatePageDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Slug { get; set; } = string.Empty;

 [StringLength(1000)]
    public string? Description { get; set; }

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

    [StringLength(50)]
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
    [StringLength(50)]
    public string BlockType { get; set; } = string.Empty;

    [Required]
    public object Configuration { get; set; } = new { };

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
    public List<int> BlockIds { get; set; } = new();
}

public class PageTranslationDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }
}
```

#### Step 2.3: إنشاء Repository Interface

**الملف:** `Repositories/Interfaces/IPageRepository.cs`

```csharp
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IPageRepository : IGenericRepository<Page>
{
    Task<Page?> GetBySlugAsync(string slug, string? lang = "ar");
    Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
    Task<IEnumerable<Page>> GetPublishedPagesAsync();
    Task<IEnumerable<Page>> GetByAuthorAsync(int authorId);
}
```

**الملف:** `Repositories/Interfaces/IPageBlockRepository.cs`

```csharp
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IPageBlockRepository : IGenericRepository<PageBlock>
{
  Task<IEnumerable<PageBlock>> GetByPageIdAsync(int pageId);
    Task ReorderBlocksAsync(int pageId, List<int> blockIds);
}
```

#### Step 2.4: إنشاء Repository Implementation

**الملف:** `Repositories/Implementations/PageRepository.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class PageRepository : GenericRepository<Page>, IPageRepository
{
    public PageRepository(ApplicationDbContext context) : base(context)
    {
    }

  public async Task<Page?> GetBySlugAsync(string slug, string? lang = "ar")
  {
        return await _context.Pages
            .Include(p => p.Author)
            .Include(p => p.Blocks.OrderBy(b => b.DisplayOrder))
            .Include(p => p.Translations.Where(t => t.LanguageCode == lang))
      .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
    {
        var query = _context.Pages.Where(p => p.Slug == slug);
        
  if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Page>> GetPublishedPagesAsync()
    {
     return await _context.Pages
            .Include(p => p.Author)
       .Include(p => p.Blocks.OrderBy(b => b.DisplayOrder))
    .Where(p => p.IsPublished)
      .OrderByDescending(p => p.PublishedAt)
.ToListAsync();
    }

    public async Task<IEnumerable<Page>> GetByAuthorAsync(int authorId)
{
        return await _context.Pages
  .Include(p => p.Blocks)
            .Where(p => p.AuthorId == authorId)
            .OrderByDescending(p => p.CreatedAt)
         .ToListAsync();
 }
}
```

**الملف:** `Repositories/Implementations/PageBlockRepository.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class PageBlockRepository : GenericRepository<PageBlock>, IPageBlockRepository
{
    public PageBlockRepository(ApplicationDbContext context) : base(context)
    {
    }

 public async Task<IEnumerable<PageBlock>> GetByPageIdAsync(int pageId)
    {
        return await _context.PageBlocks
            .Where(b => b.PageId == pageId)
            .OrderBy(b => b.DisplayOrder)
     .ToListAsync();
    }

    public async Task ReorderBlocksAsync(int pageId, List<int> blockIds)
    {
        var blocks = await _context.PageBlocks
        .Where(b => b.PageId == pageId && blockIds.Contains(b.Id))
            .ToListAsync();

        for (int i = 0; i < blockIds.Count; i++)
        {
            var block = blocks.FirstOrDefault(b => b.Id == blockIds[i]);
            if (block != null)
    {
            block.DisplayOrder = i + 1;
 }
    }

        await _context.SaveChangesAsync();
    }
}
```

#### Step 2.5: إنشاء Service Interface

**الملف:** `Services/Interfaces/IPageService.cs`

```csharp
using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Page;

namespace Gahar_Backend.Services.Interfaces;

public interface IPageService
{
    Task<PagedResult<PageListDto>> GetAllAsync(PageFilterDto filter);
    Task<PageDetailDto> GetByIdAsync(int id, string? lang = "ar");
    Task<PageDetailDto> GetBySlugAsync(string slug, string? lang = "ar");
  Task<PageDetailDto> CreateAsync(CreatePageDto dto, int authorId);
    Task<PageDetailDto> UpdateAsync(int id, UpdatePageDto dto);
    Task DeleteAsync(int id);
    Task PublishAsync(int id);
    Task UnpublishAsync(int id);
    Task<PageBlockDto> AddBlockAsync(int pageId, CreatePageBlockDto dto);
    Task<PageBlockDto> UpdateBlockAsync(int pageId, int blockId, UpdatePageBlockDto dto);
    Task DeleteBlockAsync(int pageId, int blockId);
    Task ReorderBlocksAsync(int pageId, List<int> blockIds);
    Task<PageDetailDto> DuplicateAsync(int id, int authorId);
}
```

#### Step 2.6: إنشاء Service Implementation

**الملف:** `Services/Implementations/PageService.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Gahar_Backend.Constants;
using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Page;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Implementations;

public class PageService : IPageService
{
    private readonly IPageRepository _pageRepository;
    private readonly IPageBlockRepository _pageBlockRepository;

    public PageService(
        IPageRepository pageRepository,
   IPageBlockRepository pageBlockRepository)
    {
        _pageRepository = pageRepository;
        _pageBlockRepository = pageBlockRepository;
}

  public async Task<PagedResult<PageListDto>> GetAllAsync(PageFilterDto filter)
    {
        var query = _pageRepository.GetQueryable()
            .Include(p => p.Author)
            .Include(p => p.Blocks)
 .AsQueryable();

     // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
      query = query.Where(p => 
        p.Title.Contains(filter.SearchTerm) || 
          p.Description!.Contains(filter.SearchTerm));
        }

        if (filter.IsPublished.HasValue)
        {
          query = query.Where(p => p.IsPublished == filter.IsPublished.Value);
        }

        if (filter.AuthorId.HasValue)
        {
      query = query.Where(p => p.AuthorId == filter.AuthorId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Template))
        {
    query = query.Where(p => p.Template == filter.Template);
        }

 // Get total count
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = filter.SortBy?.ToLower() switch
     {
            "title" => filter.SortOrder?.ToLower() == "asc" 
   ? query.OrderBy(p => p.Title) 
     : query.OrderByDescending(p => p.Title),
   "publishedat" => filter.SortOrder?.ToLower() == "asc" 
         ? query.OrderBy(p => p.PublishedAt) 
         : query.OrderByDescending(p => p.PublishedAt),
    _ => filter.SortOrder?.ToLower() == "asc" 
            ? query.OrderBy(p => p.CreatedAt) 
     : query.OrderByDescending(p => p.CreatedAt)
        };

        // Apply pagination
        var items = await query
  .Skip((filter.PageNumber - 1) * filter.PageSize)
    .Take(filter.PageSize)
         .Select(p => new PageListDto
            {
      Id = p.Id,
        Title = p.Title,
 Slug = p.Slug,
        Description = p.Description,
          IsPublished = p.IsPublished,
         PublishedAt = p.PublishedAt,
           AuthorName = p.Author != null ? p.Author.FullName : null,
        BlockCount = p.Blocks.Count,
       CreatedAt = p.CreatedAt,
      UpdatedAt = p.UpdatedAt
        })
            .ToListAsync();

    return new PagedResult<PageListDto>
        {
       Items = items,
     TotalCount = totalCount,
            PageNumber = filter.PageNumber,
         PageSize = filter.PageSize
   };
    }

    public async Task<PageDetailDto> GetByIdAsync(int id, string? lang = "ar")
    {
      var page = await _pageRepository.GetQueryable()
      .Include(p => p.Author)
            .Include(p => p.Blocks.OrderBy(b => b.DisplayOrder))
         .Include(p => p.Translations.Where(t => t.LanguageCode == lang))
       .FirstOrDefaultAsync(p => p.Id == id);

        if (page == null)
      {
    throw new NotFoundException($"Page with ID {id} not found");
        }

        return MapToDetailDto(page);
    }

    public async Task<PageDetailDto> GetBySlugAsync(string slug, string? lang = "ar")
    {
     var page = await _pageRepository.GetBySlugAsync(slug, lang);

        if (page == null)
        {
     throw new NotFoundException($"Page with slug '{slug}' not found");
  }

   if (!page.IsPublished)
        {
  throw new BadRequestException("Page is not published");
        }

        return MapToDetailDto(page);
    }

    public async Task<PageDetailDto> CreateAsync(CreatePageDto dto, int authorId)
    {
        // Check slug uniqueness
     if (await _pageRepository.SlugExistsAsync(dto.Slug))
        {
     throw new BadRequestException($"Slug '{dto.Slug}' already exists");
        }

        var page = new Page
        {
            Title = dto.Title,
            Slug = dto.Slug,
      Description = dto.Description,
        MetaTitle = dto.MetaTitle,
     MetaDescription = dto.MetaDescription,
            MetaKeywords = dto.MetaKeywords,
            OgTitle = dto.OgTitle,
    OgDescription = dto.OgDescription,
     OgImage = dto.OgImage,
      Template = dto.Template,
    ShowTitle = dto.ShowTitle,
         ShowBreadcrumbs = dto.ShowBreadcrumbs,
      AuthorId = authorId,
  IsPublished = false
      };

        // Add translations if provided
      if (dto.Translations != null)
   {
     foreach (var (langCode, translation) in dto.Translations)
            {
         page.Translations.Add(new Translation
  {
  LanguageCode = langCode,
    FieldName = "Title",
        FieldValue = translation.Title
      });
    page.Translations.Add(new Translation
             {
             LanguageCode = langCode,
        FieldName = "Slug",
             FieldValue = translation.Slug
 });
                if (!string.IsNullOrWhiteSpace(translation.Description))
    {
        page.Translations.Add(new Translation
      {
         LanguageCode = langCode,
           FieldName = "Description",
          FieldValue = translation.Description
          });
         }
            }
  }

        await _pageRepository.AddAsync(page);
        await _pageRepository.SaveChangesAsync();

        return await GetByIdAsync(page.Id);
    }

    public async Task<PageDetailDto> UpdateAsync(int id, UpdatePageDto dto)
{
        var page = await _pageRepository.GetByIdAsync(id);
        if (page == null)
        {
  throw new NotFoundException($"Page with ID {id} not found");
        }

    // Check slug uniqueness
        if (dto.Slug != page.Slug && await _pageRepository.SlugExistsAsync(dto.Slug, id))
  {
       throw new BadRequestException($"Slug '{dto.Slug}' already exists");
        }

        page.Title = dto.Title;
      page.Slug = dto.Slug;
   page.Description = dto.Description;
  page.MetaTitle = dto.MetaTitle;
 page.MetaDescription = dto.MetaDescription;
        page.MetaKeywords = dto.MetaKeywords;
        page.OgTitle = dto.OgTitle;
        page.OgDescription = dto.OgDescription;
        page.OgImage = dto.OgImage;
        page.Template = dto.Template;
    page.ShowTitle = dto.ShowTitle;
 page.ShowBreadcrumbs = dto.ShowBreadcrumbs;
        page.IsPublished = dto.IsPublished;

 _pageRepository.Update(page);
        await _pageRepository.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
   var page = await _pageRepository.GetByIdAsync(id);
        if (page == null)
        {
    throw new NotFoundException($"Page with ID {id} not found");
        }

        _pageRepository.Delete(page);
      await _pageRepository.SaveChangesAsync();
    }

    public async Task PublishAsync(int id)
    {
        var page = await _pageRepository.GetByIdAsync(id);
        if (page == null)
        {
throw new NotFoundException($"Page with ID {id} not found");
      }

        page.IsPublished = true;
        page.PublishedAt = DateTime.UtcNow;

_pageRepository.Update(page);
        await _pageRepository.SaveChangesAsync();
    }

    public async Task UnpublishAsync(int id)
    {
        var page = await _pageRepository.GetByIdAsync(id);
        if (page == null)
    {
            throw new NotFoundException($"Page with ID {id} not found");
        }

        page.IsPublished = false;

        _pageRepository.Update(page);
        await _pageRepository.SaveChangesAsync();
    }

 public async Task<PageBlockDto> AddBlockAsync(int pageId, CreatePageBlockDto dto)
    {
     var page = await _pageRepository.GetByIdAsync(pageId);
 if (page == null)
        {
            throw new NotFoundException($"Page with ID {pageId} not found");
        }

        // Validate block type
        if (!BlockTypes.IsValid(dto.BlockType))
 {
            throw new BadRequestException($"Invalid block type: {dto.BlockType}");
      }

   var maxOrder = await _pageBlockRepository.GetQueryable()
  .Where(b => b.PageId == pageId)
   .MaxAsync(b => (int?)b.DisplayOrder) ?? 0;

        var block = new PageBlock
        {
   PageId = pageId,
 BlockType = dto.BlockType,
  Configuration = JsonSerializer.Serialize(dto.Configuration),
    DisplayOrder = maxOrder + 1,
 IsVisible = dto.IsVisible,
            CssClass = dto.CssClass,
        CustomId = dto.CustomId
      };

  await _pageBlockRepository.AddAsync(block);
        await _pageBlockRepository.SaveChangesAsync();

        return new PageBlockDto
     {
            Id = block.Id,
 BlockType = block.BlockType,
            Configuration = JsonSerializer.Deserialize<object>(block.Configuration)!,
            DisplayOrder = block.DisplayOrder,
            IsVisible = block.IsVisible,
            CssClass = block.CssClass,
   CustomId = block.CustomId
        };
    }

 public async Task<PageBlockDto> UpdateBlockAsync(int pageId, int blockId, UpdatePageBlockDto dto)
    {
        var block = await _pageBlockRepository.GetQueryable()
  .FirstOrDefaultAsync(b => b.Id == blockId && b.PageId == pageId);

    if (block == null)
  {
    throw new NotFoundException($"Block with ID {blockId} not found in page {pageId}");
        }

   // Validate block type
        if (!BlockTypes.IsValid(dto.BlockType))
      {
        throw new BadRequestException($"Invalid block type: {dto.BlockType}");
        }

        block.BlockType = dto.BlockType;
        block.Configuration = JsonSerializer.Serialize(dto.Configuration);
   block.DisplayOrder = dto.DisplayOrder;
        block.IsVisible = dto.IsVisible;
        block.CssClass = dto.CssClass;
        block.CustomId = dto.CustomId;

    _pageBlockRepository.Update(block);
        await _pageBlockRepository.SaveChangesAsync();

        return new PageBlockDto
        {
            Id = block.Id,
    BlockType = block.BlockType,
            Configuration = JsonSerializer.Deserialize<object>(block.Configuration)!,
  DisplayOrder = block.DisplayOrder,
   IsVisible = block.IsVisible,
      CssClass = block.CssClass,
       CustomId = block.CustomId
        };
    }

    public async Task DeleteBlockAsync(int pageId, int blockId)
    {
        var block = await _pageBlockRepository.GetQueryable()
            .FirstOrDefaultAsync(b => b.Id == blockId && b.PageId == pageId);

        if (block == null)
        {
            throw new NotFoundException($"Block with ID {blockId} not found in page {pageId}");
        }

        _pageBlockRepository.Delete(block);
        await _pageBlockRepository.SaveChangesAsync();
    }

    public async Task ReorderBlocksAsync(int pageId, List<int> blockIds)
    {
  var page = await _pageRepository.GetByIdAsync(pageId);
        if (page == null)
        {
  throw new NotFoundException($"Page with ID {pageId} not found");
     }

   await _pageBlockRepository.ReorderBlocksAsync(pageId, blockIds);
    }

    public async Task<PageDetailDto> DuplicateAsync(int id, int authorId)
    {
        var originalPage = await _pageRepository.GetQueryable()
            .Include(p => p.Blocks)
            .FirstOrDefaultAsync(p => p.Id == id);

   if (originalPage == null)
  {
            throw new NotFoundException($"Page with ID {id} not found");
        }

        // Generate unique slug
        var baseSlug = $"{originalPage.Slug}-copy";
        var slug = baseSlug;
        var counter = 1;
        while (await _pageRepository.SlugExistsAsync(slug))
        {
 slug = $"{baseSlug}-{counter}";
          counter++;
        }

        var newPage = new Page
        {
  Title = $"{originalPage.Title} (Copy)",
            Slug = slug,
       Description = originalPage.Description,
     MetaTitle = originalPage.MetaTitle,
     MetaDescription = originalPage.MetaDescription,
MetaKeywords = originalPage.MetaKeywords,
            OgTitle = originalPage.OgTitle,
    OgDescription = originalPage.OgDescription,
        OgImage = originalPage.OgImage,
   Template = originalPage.Template,
    ShowTitle = originalPage.ShowTitle,
 ShowBreadcrumbs = originalPage.ShowBreadcrumbs,
    AuthorId = authorId,
          IsPublished = false
     };

     // Duplicate blocks
     foreach (var block in originalPage.Blocks.OrderBy(b => b.DisplayOrder))
        {
 newPage.Blocks.Add(new PageBlock
    {
           BlockType = block.BlockType,
            Configuration = block.Configuration,
           DisplayOrder = block.DisplayOrder,
   IsVisible = block.IsVisible,
         CssClass = block.CssClass,
       CustomId = block.CustomId
            });
        }

        await _pageRepository.AddAsync(newPage);
      await _pageRepository.SaveChangesAsync();

   return await GetByIdAsync(newPage.Id);
    }

    private static PageDetailDto MapToDetailDto(Page page)
    {
     return new PageDetailDto
    {
         Id = page.Id,
            Title = page.Title,
            Slug = page.Slug,
            Description = page.Description,
            MetaTitle = page.MetaTitle,
            MetaDescription = page.MetaDescription,
            MetaKeywords = page.MetaKeywords,
        OgTitle = page.OgTitle,
   OgDescription = page.OgDescription,
     OgImage = page.OgImage,
       Template = page.Template,
      ShowTitle = page.ShowTitle,
     ShowBreadcrumbs = page.ShowBreadcrumbs,
            IsPublished = page.IsPublished,
PublishedAt = page.PublishedAt,
  AuthorName = page.Author?.FullName,
        BlockCount = page.Blocks.Count,
            CreatedAt = page.CreatedAt,
            UpdatedAt = page.UpdatedAt,
            Blocks = page.Blocks.OrderBy(b => b.DisplayOrder).Select(b => new PageBlockDto
   {
   Id = b.Id,
             BlockType = b.BlockType,
                Configuration = JsonSerializer.Deserialize<object>(b.Configuration)!,
    DisplayOrder = b.DisplayOrder,
           IsVisible = b.IsVisible,
          CssClass = b.CssClass,
                CustomId = b.CustomId
     }).ToList()
        };
  }
}
```

#### Step 2.7: تسجيل Services في DI Container

**إضافة إلى:** `Program.cs`

```csharp
// Repositories
builder.Services.AddScoped<IPageRepository, PageRepository>();
builder.Services.AddScoped<IPageBlockRepository, PageBlockRepository>();

// Services
builder.Services.AddScoped<IPageService, PageService>();
```

#### ✅ Checklist Phase 2
- [ ] إنشاء Common DTOs (PagedResult, PageFilterDto)
- [ ] إنشاء Page DTOs
- [ ] إنشاء PageRepository Interface & Implementation
- [ ] إنشاء PageBlockRepository Interface & Implementation
- [ ] إنشاء PageService Interface & Implementation
- [ ] تسجيل Services في DI
- [ ] Build بدون أخطاء

---

### 📌 Phase 3: API Layer (يوم 4)

#### Step 3.1: تحديث Permissions

**إضافة إلى:** `Constants/Permissions.cs`

```csharp
// Pages
public const string PagesView = "Pages.View";
public const string PagesCreate = "Pages.Create";
public const string PagesEdit = "Pages.Edit";
public const string PagesDelete = "Pages.Delete";
public const string PagesPublish = "Pages.Publish";
```

#### Step 3.2: إنشاء PagesController

**الملف:** `Controllers/PagesController.cs`

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Gahar_Backend.Constants;
using Gahar_Backend.Filters;
using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Page;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagesController : ControllerBase
{
    private readonly IPageService _pageService;

    public PagesController(IPageService pageService)
    {
        _pageService = pageService;
    }

    /// <summary>
 /// Get all pages with filtering and pagination
    /// </summary>
    [HttpGet]
  [AllowAnonymous]
    public async Task<ActionResult<PagedResult<PageListDto>>> GetAll([FromQuery] PageFilterDto filter)
    {
        var pages = await _pageService.GetAllAsync(filter);
        return Ok(pages);
    }

    /// <summary>
 /// Get page by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<PageDetailDto>> GetById(int id, [FromQuery] string? lang = "ar")
    {
  var page = await _pageService.GetByIdAsync(id, lang);
        return Ok(page);
    }

    /// <summary>
    /// Get page by slug (for public frontend)
    /// </summary>
    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<ActionResult<PageDetailDto>> GetBySlug(string slug, [FromQuery] string? lang = "ar")
    {
        var page = await _pageService.GetBySlugAsync(slug, lang);
        return Ok(page);
    }

    /// <summary>
    /// Create a new page
    /// </summary>
    [HttpPost]
    [Authorize]
    [Permission(Permissions.PagesCreate)]
    public async Task<ActionResult<PageDetailDto>> Create([FromBody] CreatePageDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var page = await _pageService.CreateAsync(dto, userId);
    return CreatedAtAction(nameof(GetById), new { id = page.Id }, page);
    }

    /// <summary>
    /// Update existing page
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
  [Permission(Permissions.PagesEdit)]
    public async Task<ActionResult<PageDetailDto>> Update(int id, [FromBody] UpdatePageDto dto)
    {
        var page = await _pageService.UpdateAsync(id, dto);
        return Ok(page);
    }

    /// <summary>
    /// Delete page
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    [Permission(Permissions.PagesDelete)]
    public async Task<ActionResult> Delete(int id)
    {
        await _pageService.DeleteAsync(id);
        return NoContent();
    }

  /// <summary>
    /// Publish page
    /// </summary>
    [HttpPost("{id}/publish")]
    [Authorize]
    [Permission(Permissions.PagesPublish)]
    public async Task<ActionResult> Publish(int id)
    {
    await _pageService.PublishAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Unpublish page
  /// </summary>
    [HttpPost("{id}/unpublish")]
    [Authorize]
    [Permission(Permissions.PagesPublish)]
  public async Task<ActionResult> Unpublish(int id)
    {
        await _pageService.UnpublishAsync(id);
  return NoContent();
    }

    /// <summary>
    /// Add block to page
    /// </summary>
    [HttpPost("{id}/blocks")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    public async Task<ActionResult<PageBlockDto>> AddBlock(int id, [FromBody] CreatePageBlockDto dto)
    {
        var block = await _pageService.AddBlockAsync(id, dto);
        return Ok(block);
    }

    /// <summary>
    /// Update page block
    /// </summary>
    [HttpPut("{id}/blocks/{blockId}")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    public async Task<ActionResult<PageBlockDto>> UpdateBlock(int id, int blockId, [FromBody] UpdatePageBlockDto dto)
    {
        var block = await _pageService.UpdateBlockAsync(id, blockId, dto);
        return Ok(block);
    }

    /// <summary>
    /// Delete page block
    /// </summary>
    [HttpDelete("{id}/blocks/{blockId}")]
    [Authorize]
    [Permission(Permissions.PagesDelete)]
    public async Task<ActionResult> DeleteBlock(int id, int blockId)
 {
        await _pageService.DeleteBlockAsync(id, blockId);
        return NoContent();
    }

    /// <summary>
    /// Reorder page blocks
    /// </summary>
  [HttpPost("{id}/reorder-blocks")]
    [Authorize]
    [Permission(Permissions.PagesEdit)]
    public async Task<ActionResult> ReorderBlocks(int id, [FromBody] ReorderBlocksDto dto)
    {
        await _pageService.ReorderBlocksAsync(id, dto.BlockIds);
        return NoContent();
    }

    /// <summary>
    /// Duplicate page
    /// </summary>
  [HttpPost("{id}/duplicate")]
    [Authorize]
    [Permission(Permissions.PagesCreate)]
    public async Task<ActionResult<PageDetailDto>> Duplicate(int id)
    {
  var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var page = await _pageService.DuplicateAsync(id, userId);
        return Ok(page);
    }
}
```

#### ✅ Checklist Phase 3
- [ ] تحديث Permissions
- [ ] إنشاء PagesController
- [ ] إضافة XML Documentation
- [ ] Build بدون أخطاء
- [ ] Swagger UI يعرض Endpoints بشكل صحيح

---

### 📌 Phase 4: Testing & Validation (يوم 5)

#### Step 4.1: Build و Test

```bash
# Build
dotnet build

# Run Application
dotnet run

# Test في Swagger
# افتح: https://localhost:7xxx/swagger
```

#### Step 4.2: Integration Testing Checklist

**Test Cases:**

1. **Create Page**
   - [ ] POST /api/pages - بيانات صحيحة
   - [ ] POST /api/pages - slug مكرر (يجب أن يفشل)
   - [ ] POST /api/pages - بدون authentication (يجب أن يفشل)

2. **Get Pages**
   - [ ] GET /api/pages - بدون فلاتر
   - [ ] GET /api/pages?IsPublished=true
   - [ ] GET /api/pages?SearchTerm=test
   - [ ] GET /api/pages/{id} - موجود
   - [ ] GET /api/pages/{id} - غير موجود (404)
   - [ ] GET /api/pages/slug/{slug}

3. **Update Page**
   - [ ] PUT /api/pages/{id} - بيانات صحيحة
   - [ ] PUT /api/pages/{id} - slug مكرر (يجب أن يفشل)

4. **Publish/Unpublish**
   - [ ] POST /api/pages/{id}/publish
   - [ ] POST /api/pages/{id}/unpublish

5. **Blocks Management**
   - [ ] POST /api/pages/{id}/blocks - HeroSection
   - [ ] POST /api/pages/{id}/blocks - TextBlock
   - [ ] POST /api/pages/{id}/blocks - InvalidType (يجب أن يفشل)
- [ ] PUT /api/pages/{id}/blocks/{blockId}
   - [ ] DELETE /api/pages/{id}/blocks/{blockId}
   - [ ] POST /api/pages/{id}/reorder-blocks

6. **Duplicate Page**
   - [ ] POST /api/pages/{id}/duplicate
   - [ ] التحقق من أن الـ blocks تم نسخها

7. **Delete Page**
   - [ ] DELETE /api/pages/{id}
   - [ ] التحقق من حذف الـ blocks أيضاً (Cascade)

#### Step 4.3: Sample Test Data

**Create Page Request:**
```json
{
  "title": "Welcome Page",
  "slug": "welcome",
  "description": "Main welcome page",
  "metaTitle": "Welcome to Gahar",
  "metaDescription": "Saudi Commission for Health Specialties",
  "template": "Default",
  "showTitle": true,
  "showBreadcrumbs": true
}
```

**Add Hero Block Request:**
```json
{
  "blockType": "HeroSection",
  "configuration": {
    "backgroundImage": "/uploads/hero.jpg",
    "heading": "مرحباً بكم في الهيئة",
    "subheading": "الهيئة السعودية للتخصصات الصحية",
    "ctaText": "اعرف المزيد",
    "ctaLink": "/about",
    "textAlign": "center",
    "overlay": true,
    "overlayOpacity": 0.5
  },
  "isVisible": true
}
```

**Add Stats Counter Block:**
```json
{
  "blockType": "StatsCounter",
  "configuration": {
  "items": [
      {
        "value": 500,
    "suffix": "+",
        "label": "منشأة معتمدة",
        "icon": "Building"
      },
      {
        "value": 1000,
        "suffix": "+",
      "label": "شهادة صادرة",
        "icon": "Award"
      }
    ],
    "animationDuration": 2000
  },
  "isVisible": true
}
```

#### ✅ Final Checklist - Feature 1
- [ ] جميع Test Cases ناجحة
- [ ] لا توجد أخطاء في Build
- [ ] Database Tables صحيحة
- [ ] API Responses صحيحة
- [ ] Error Handling يعمل بشكل صحيح
- [ ] Permissions تعمل بشكل صحيح
- [ ] Documentation كاملة

---

## 🎯 الخطوات التالية

بعد إتمام Feature 1 بنجاح:

1. **Review الكود** والتأكد من Quality
2. **Commit التغييرات** إلى Git
3. **الانتقال إلى Feature 2: Form Builder**

---

## 📝 ملاحظات مهمة

### Database Migrations
- قم بعمل backup قبل كل migration
- تأكد من تشغيل `dotnet ef database update` بعد كل migration

### Error Handling
- جميع الـ Exceptions محددة في `Utilities/Exceptions/CustomExceptions.cs`
- استخدم NotFoundException, BadRequestException, etc.

### Authentication & Authorization
- جميع endpoints الإدارية تحتاج Authentication
- استخدم [Permission] attribute للتحكم في الصلاحيات
- Public endpoints مثل GetBySlug تستخدم [AllowAnonymous]

### JSON Serialization
- Block Configuration يتم تخزينها كـ JSON string
- استخدم System.Text.Json للـ Serialization/Deserialization

---

**تم إنشاء هذا الدليل في:** 11 يناير 2025  
**الحالة:** ✅ جاهز للتنفيذ - Feature 1: Page Builder

---

# 📌 الملاحظات النهائية

هذا الدليل يغطي **Feature 1 فقط (Page Builder)** بشكل كامل ومفصل.

**Features المتبقية** سيتم توثيقها بنفس الطريقة في ملفات منفصلة:
- Feature 2: Form Builder
- Feature 3: Navigation Menus
- Feature 4: Facilities Management
- Feature 5: Certificates Management
- Feature 6: SEO & Analytics

كل feature سيكون له دليل مستقل مع نفس التقسيم (4 Phases) ونفس مستوى التفصيل.
