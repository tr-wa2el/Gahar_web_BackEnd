# 📦 Feature 2: نظام المحتوى الديناميكي (Dynamic Content System)

**المطور:** Developer 1  
**الأولوية:** Priority 1 (Week 1-2)  
**المدة المتوقعة:** 5-7 أيام  
**الحالة:** 🟡 جاهز للتطوير

---

## 📋 جدول المحتويات
1. [نظرة عامة](#نظرة-عامة)
2. [Database Models](#database-models)
3. [Entity Configurations](#entity-configurations)
4. [DTOs](#dtos)
5. [Repository](#repository)
6. [Service](#service)
7. [Controller](#controller)
8. [Unit Tests](#unit-tests)
9. [Checklist](#checklist)

---

## نظرة عامة

### 🎯 الهدف
تطوير نظام محتوى ديناميكي يسمح بإنشاء وإدارة محتوى لأي نوع من أنواع المحتوى (Content Types) التي تم إنشاؤها مسبقاً، مع دعم الحقول الديناميكية والترجمة والتاقات ومحركات البحث.

### 📦 المخرجات
- نظام إدارة محتوى CRUD كامل
- دعم الحقول الديناميكية (Custom Fields)
- نظام Tags للمحتوى
- نظام Publishing (Draft, Published, Scheduled, Archived)
- SEO & Open Graph Support
- دعم الترجمة متعدد اللغات
- نظام البحث والفلترة المتقدم
- Pagination Support
- Content Duplication
- View Counter

### 🔗 التبعيات
- ✅ Content Types System (Feature 1)
- ✅ Base Foundation
- ✅ Translation System
- ✅ Audit Log System
- ⏳ Layout System (Feature 3) - Optional

---

## Database Models

### 1. Content Model

**الملف:** `Gahar_Backend/Models/Entities/Content.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    /// <summary>
    /// Represents a content item of any content type
    /// </summary>
    public class Content : TranslatableEntity
    {
        /// <summary>
      /// ID of the content type
        /// </summary>
        public int ContentTypeId { get; set; }
        
    /// <summary>
        /// Navigation property to content type
        /// </summary>
        public ContentType ContentType { get; set; } = null!;

        /// <summary>
        /// Title of the content
      /// </summary>
  [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

     /// <summary>
        /// URL-friendly slug
        /// </summary>
        [Required]
      [StringLength(200)]
     public string Slug { get; set; } = string.Empty;

  /// <summary>
     /// Short summary/excerpt
        /// </summary>
        public string? Summary { get; set; }

      /// <summary>
        /// Main content body (Rich Text)
     /// </summary>
        public string? Body { get; set; }

 /// <summary>
        /// Featured image URL
        /// </summary>
        [StringLength(500)]
        public string? FeaturedImage { get; set; }

   /// <summary>
        /// Layout ID (optional)
   /// </summary>
     public int? LayoutId { get; set; }
        
        /// <summary>
  /// Navigation property to layout
        /// </summary>
        public Layout? Layout { get; set; }

        #region SEO Properties
        
        /// <summary>
        /// Meta title for SEO
        /// </summary>
    [StringLength(200)]
     public string? MetaTitle { get; set; }

        /// <summary>
        /// Meta description for SEO
        /// </summary>
    [StringLength(500)]
      public string? MetaDescription { get; set; }

        /// <summary>
   /// Meta keywords for SEO
  /// </summary>
        public string? MetaKeywords { get; set; }

        /// <summary>
        /// Open Graph title
        /// </summary>
        [StringLength(200)]
    public string? OgTitle { get; set; }

        /// <summary>
        /// Open Graph description
        /// </summary>
     [StringLength(500)]
  public string? OgDescription { get; set; }

        /// <summary>
    /// Open Graph image
        /// </summary>
        [StringLength(500)]
 public string? OgImage { get; set; }

        #endregion

        #region Publishing Properties

        /// <summary>
        /// Content status (Draft, Published, Scheduled, Archived)
      /// </summary>
        [Required]
      [StringLength(50)]
    public string Status { get; set; } = ContentStatus.Draft;

        /// <summary>
        /// Date and time when content was published
        /// </summary>
  public DateTime? PublishedAt { get; set; }

  /// <summary>
    /// Date and time when content should be published
    /// </summary>
  public DateTime? ScheduledAt { get; set; }

        /// <summary>
        /// ID of the author (user)
        /// </summary>
        public int? AuthorId { get; set; }
     
        /// <summary>
     /// Navigation property to author
      /// </summary>
        public User? Author { get; set; }

        #endregion

     #region Stats & Settings

        /// <summary>
        /// Number of views
        /// </summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// Indicates if this content is featured
        /// </summary>
        public bool IsFeatured { get; set; } = false;

  /// <summary>
        /// Indicates if comments are allowed
        /// </summary>
      public bool AllowComments { get; set; } = true;

        #endregion

    #region Navigation Properties

        /// <summary>
    /// Custom field values
        /// </summary>
        public ICollection<ContentFieldValue> FieldValues { get; set; } = new List<ContentFieldValue>();

        /// <summary>
        /// Tags associated with this content
        /// </summary>
   public ICollection<ContentTag> Tags { get; set; } = new List<ContentTag>();

        #endregion
    }

    /// <summary>
    /// Content status constants
    /// </summary>
    public static class ContentStatus
    {
        public const string Draft = "Draft";
        public const string Published = "Published";
        public const string Scheduled = "Scheduled";
        public const string Archived = "Archived";
    }
}
```

### 2. ContentFieldValue Model

**الملف:** `Gahar_Backend/Models/Entities/ContentFieldValue.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    /// <summary>
    /// Stores dynamic field values for content
    /// </summary>
    public class ContentFieldValue : BaseEntity
    {
        /// <summary>
        /// ID of the content
        /// </summary>
        public int ContentId { get; set; }
     
        /// <summary>
        /// Navigation property to content
        /// </summary>
        public Content Content { get; set; } = null!;

        /// <summary>
        /// ID of the content type field definition
        /// </summary>
        public int ContentTypeFieldId { get; set; }
        
     /// <summary>
        /// Navigation property to field definition
 /// </summary>
        public ContentTypeField ContentTypeField { get; set; } = null!;

   /// <summary>
 /// Field key for quick lookup
        /// </summary>
        [Required]
      [StringLength(100)]
 public string FieldKey { get; set; } = string.Empty;

     /// <summary>
        /// The actual value (text, number, date, JSON, etc.)
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Language ID for translated values (optional)
        /// </summary>
        public int? LanguageId { get; set; }
        
        /// <summary>
        /// Navigation property to language
        /// </summary>
        public Language? Language { get; set; }
    }
}
```

### 3. Tag Model

**الملف:** `Gahar_Backend/Models/Entities/Tag.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    /// <summary>
    /// Represents a tag for categorizing content
    /// </summary>
    public class Tag : TranslatableEntity
    {
 /// <summary>
        /// Tag name
        /// </summary>
        [Required]
   [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// URL-friendly slug
  /// </summary>
     [Required]
        [StringLength(100)]
      public string Slug { get; set; } = string.Empty;

        /// <summary>
 /// Tag description
        /// </summary>
 [StringLength(500)]
    public string? Description { get; set; }

        /// <summary>
 /// Tag color for UI display
        /// </summary>
        [StringLength(20)]
public string? Color { get; set; }

      /// <summary>
  /// Number of times this tag has been used
        /// </summary>
        public int UsageCount { get; set; } = 0;

  /// <summary>
        /// Content items with this tag
     /// </summary>
        public ICollection<ContentTag> ContentTags { get; set; } = new List<ContentTag>();
    }
}
```

### 4. ContentTag Model (Junction Table)

**الملف:** `Gahar_Backend/Models/Entities/ContentTag.cs`

```csharp
namespace Gahar_Backend.Models.Entities
{
    /// <summary>
    /// Junction table for many-to-many relationship between Content and Tag
    /// </summary>
    public class ContentTag : BaseEntity
    {
/// <summary>
        /// ID of the content
        /// </summary>
        public int ContentId { get; set; }
    
        /// <summary>
        /// Navigation property to content
      /// </summary>
        public Content Content { get; set; } = null!;

        /// <summary>
        /// ID of the tag
        /// </summary>
        public int TagId { get; set; }
        
        /// <summary>
        /// Navigation property to tag
        /// </summary>
    public Tag Tag { get; set; } = null!;
    }
}
```

---

## Entity Configurations

### 1. ContentConfiguration

**الملف:** `Gahar_Backend/Data/Configurations/ContentConfiguration.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class ContentConfiguration : IEntityTypeConfiguration<Content>
    {
      public void Configure(EntityTypeBuilder<Content> builder)
        {
            builder.ToTable("Contents");

            builder.HasKey(c => c.Id);

       // Composite unique index on ContentTypeId and Slug
            builder.HasIndex(c => new { c.ContentTypeId, c.Slug })
        .IsUnique();

   // Index on Status for filtering
            builder.HasIndex(c => c.Status);

     // Index on PublishedAt for sorting
            builder.HasIndex(c => c.PublishedAt);

     // Index on IsFeatured for filtering
            builder.HasIndex(c => c.IsFeatured);

  // Properties
         builder.Property(c => c.Title)
     .IsRequired()
      .HasMaxLength(200);

     builder.Property(c => c.Slug)
         .IsRequired()
       .HasMaxLength(200);

  builder.Property(c => c.FeaturedImage)
      .HasMaxLength(500);

    builder.Property(c => c.Status)
       .IsRequired()
    .HasMaxLength(50)
            .HasDefaultValue(ContentStatus.Draft);

     builder.Property(c => c.ViewCount)
          .HasDefaultValue(0);

     builder.Property(c => c.IsFeatured)
       .HasDefaultValue(false);

            builder.Property(c => c.AllowComments)
 .HasDefaultValue(true);

       // SEO Properties
    builder.Property(c => c.MetaTitle)
           .HasMaxLength(200);

    builder.Property(c => c.MetaDescription)
                .HasMaxLength(500);

        builder.Property(c => c.OgTitle)
         .HasMaxLength(200);

   builder.Property(c => c.OgDescription)
  .HasMaxLength(500);

    builder.Property(c => c.OgImage)
      .HasMaxLength(500);

            builder.Property(c => c.CreatedAt)
          .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
     builder.HasOne(c => c.ContentType)
       .WithMany(ct => ct.Contents)
   .HasForeignKey(c => c.ContentTypeId)
        .OnDelete(DeleteBehavior.Restrict);

 builder.HasOne(c => c.Author)
   .WithMany()
        .HasForeignKey(c => c.AuthorId)
 .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(c => c.Layout)
       .WithMany(l => l.Contents)
 .HasForeignKey(c => c.LayoutId)
                .OnDelete(DeleteBehavior.SetNull);

  builder.HasMany(c => c.FieldValues)
        .WithOne(fv => fv.Content)
     .HasForeignKey(fv => fv.ContentId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Tags)
            .WithOne(ct => ct.Content)
             .HasForeignKey(ct => ct.ContentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
```

### 2. ContentFieldValueConfiguration

**الملف:** `Gahar_Backend/Data/Configurations/ContentFieldValueConfiguration.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class ContentFieldValueConfiguration : IEntityTypeConfiguration<ContentFieldValue>
  {
 public void Configure(EntityTypeBuilder<ContentFieldValue> builder)
        {
   builder.ToTable("ContentFieldValues");

       builder.HasKey(fv => fv.Id);

            // Composite unique index
            builder.HasIndex(fv => new { fv.ContentId, fv.ContentTypeFieldId, fv.LanguageId })
                .IsUnique();

            // Properties
            builder.Property(fv => fv.FieldKey)
            .IsRequired()
 .HasMaxLength(100);

      builder.Property(fv => fv.CreatedAt)
    .HasDefaultValueSql("GETUTCDATE()");

        // Relationships configured in ContentConfiguration and ContentTypeFieldConfiguration
        }
    }
}
```

### 3. TagConfiguration

**الملف:** `Gahar_Backend/Data/Configurations/TagConfiguration.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
builder.ToTable("Tags");

            builder.HasKey(t => t.Id);

          // Unique constraint on slug
     builder.HasIndex(t => t.Slug)
      .IsUnique();

    // Index on UsageCount for sorting
          builder.HasIndex(t => t.UsageCount);

            // Properties
builder.Property(t => t.Name)
          .IsRequired()
      .HasMaxLength(100);

       builder.Property(t => t.Slug)
    .IsRequired()
   .HasMaxLength(100);

         builder.Property(t => t.Description)
    .HasMaxLength(500);

 builder.Property(t => t.Color)
          .HasMaxLength(20);

  builder.Property(t => t.UsageCount)
                .HasDefaultValue(0);

            builder.Property(t => t.CreatedAt)
     .HasDefaultValueSql("GETUTCDATE()");

   // Relationships
        builder.HasMany(t => t.ContentTags)
       .WithOne(ct => ct.Tag)
         .HasForeignKey(ct => ct.TagId)
            .OnDelete(DeleteBehavior.Cascade);
     }
    }
}
```

### 4. ContentTagConfiguration

**الملف:** `Gahar_Backend/Data/Configurations/ContentTagConfiguration.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Data.Configurations
{
    public class ContentTagConfiguration : IEntityTypeConfiguration<ContentTag>
    {
        public void Configure(EntityTypeBuilder<ContentTag> builder)
     {
   builder.ToTable("ContentTags");

       builder.HasKey(ct => ct.Id);

            // Composite unique index
        builder.HasIndex(ct => new { ct.ContentId, ct.TagId })
       .IsUnique();

            builder.Property(ct => ct.CreatedAt)
.HasDefaultValueSql("GETUTCDATE()");

         // Relationships configured in Content and Tag configurations
   }
    }
}
```

---

## DTOs

**الملف:** `Gahar_Backend/Models/DTOs/Content/ContentDto.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.DTOs.Content
{
    /// <summary>
    /// DTO for content list item
    /// </summary>
    public class ContentListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
     public string? Summary { get; set; }
        public string? FeaturedImage { get; set; }
        public string Status { get; set; } = string.Empty;
  public DateTime? PublishedAt { get; set; }
        public int ViewCount { get; set; }
        public bool IsFeatured { get; set; }
        public string ContentTypeName { get; set; } = string.Empty;
        public int ContentTypeId { get; set; }
     public string? AuthorName { get; set; }
        public List<TagDto> Tags { get; set; } = new();
    public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO for detailed content view
    /// </summary>
    public class ContentDetailDto : ContentListDto
    {
    public string? Body { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
     public string? OgTitle { get; set; }
 public string? OgDescription { get; set; }
        public string? OgImage { get; set; }
        public Dictionary<string, object> CustomFields { get; set; } = new();
        public LayoutDto? Layout { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public bool AllowComments { get; set; }
    }

    /// <summary>
    /// DTO for creating content
    /// </summary>
    public class CreateContentDto
    {
        [Required(ErrorMessage = "Content type ID is required")]
        public int ContentTypeId { get; set; }

     [Required(ErrorMessage = "Title is required")]
[StringLength(200, ErrorMessage = "Title must not exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug is required")]
        [StringLength(200, ErrorMessage = "Slug must not exceed 200 characters")]
        [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", 
            ErrorMessage = "Slug must be lowercase letters, numbers, and hyphens only")]
        public string Slug { get; set; } = string.Empty;

    public string? Summary { get; set; }
        public string? Body { get; set; }
        
        [StringLength(500, ErrorMessage = "Featured image URL must not exceed 500 characters")]
        public string? FeaturedImage { get; set; }
        
        public int? LayoutId { get; set; }

        // SEO
        [StringLength(200, ErrorMessage = "Meta title must not exceed 200 characters")]
        public string? MetaTitle { get; set; }
        
      [StringLength(500, ErrorMessage = "Meta description must not exceed 500 characters")]
    public string? MetaDescription { get; set; }
     
        public string? MetaKeywords { get; set; }
        
        [StringLength(200, ErrorMessage = "OG title must not exceed 200 characters")]
        public string? OgTitle { get; set; }
        
        [StringLength(500, ErrorMessage = "OG description must not exceed 500 characters")]
        public string? OgDescription { get; set; }
     
  [StringLength(500, ErrorMessage = "OG image URL must not exceed 500 characters")]
      public string? OgImage { get; set; }

        // Publishing
        [Required]
        [RegularExpression("^(Draft|Published|Scheduled|Archived)$", 
            ErrorMessage = "Status must be Draft, Published, Scheduled, or Archived")]
        public string Status { get; set; } = "Draft";
     
     public DateTime? ScheduledAt { get; set; }
        public bool IsFeatured { get; set; } = false;
        public bool AllowComments { get; set; } = true;

        // Tags
        public List<int>? TagIds { get; set; }

        // Custom Fields
        public Dictionary<string, object>? CustomFields { get; set; }

     // Translations
        public Dictionary<string, ContentTranslationDto>? Translations { get; set; }
    }

    /// <summary>
    /// DTO for updating content
    /// </summary>
  public class UpdateContentDto : CreateContentDto
    {
    }

    /// <summary>
    /// DTO for content translation
    /// </summary>
    public class ContentTranslationDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        
     [Required]
  public string Slug { get; set; } = string.Empty;

        public string? Summary { get; set; }
     public string? Body { get; set; }
        public Dictionary<string, object>? CustomFields { get; set; }
    }

    /// <summary>
    /// DTO for content filtering
    /// </summary>
    public class ContentFilterDto
    {
        public int? ContentTypeId { get; set; }
   public string? Status { get; set; }
 public bool? IsFeatured { get; set; }
        public int? AuthorId { get; set; }
  public List<int>? TagIds { get; set; }
        public string? SearchTerm { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Language { get; set; } = "ar";
        
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; } = 1;
  
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;
        
        public string? SortBy { get; set; } = "CreatedAt";
        
        [RegularExpression("^(asc|desc)$", ErrorMessage = "Sort order must be 'asc' or 'desc'")]
        public string? SortOrder { get; set; } = "desc";
    }

    /// <summary>
    /// DTO for tag
    /// </summary>
public class TagDto
    {
        public int Id { get; set; }
   public string Name { get; set; } = string.Empty;
 public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Color { get; set; }
        public int UsageCount { get; set; }
    }

    /// <summary>
    /// DTO for creating tag
    /// </summary>
    public class CreateTagDto
    {
    [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must not exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug is required")]
        [StringLength(100, ErrorMessage = "Slug must not exceed 100 characters")]
        [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", 
      ErrorMessage = "Slug must be lowercase letters, numbers, and hyphens only")]
        public string Slug { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
    public string? Description { get; set; }

        [StringLength(20, ErrorMessage = "Color must not exceed 20 characters")]
        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", 
  ErrorMessage = "Color must be a valid hex color code")]
        public string? Color { get; set; }
    }

    /// <summary>
    /// DTO for layout (simplified)
  /// </summary>
    public class LayoutDto
    {
 public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    }
}
```

إكمال الملف في الرد التالي...

---

**تاريخ الإنشاء:** 11 يناير 2025  
**الحالة:** 🔄 جاري الإنشاء
