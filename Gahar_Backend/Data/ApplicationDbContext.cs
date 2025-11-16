using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Models.Entities;

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

        // Feature 1: Page Builder
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageBlock> PageBlocks { get; set; }

        // Feature 2: Form Builder
 public DbSet<Form> Forms { get; set; }
  public DbSet<FormField> FormFields { get; set; }
  public DbSet<FormSubmission> FormSubmissions { get; set; }

        // Feature 3: Navigation Menu
  public DbSet<Menu> Menus { get; set; }
 public DbSet<MenuItem> MenuItems { get; set; }

        // Feature 4: Facilities Management
public DbSet<Facility> Facilities { get; set; }
  public DbSet<FacilityDepartment> FacilityDepartments { get; set; }
  public DbSet<FacilityService> FacilityServices { get; set; }
  public DbSet<FacilityImage> FacilityImages { get; set; }
   public DbSet<FacilityReview> FacilityReviews { get; set; }

   // Feature 5: Certificates Management
 public DbSet<Certificate> Certificates { get; set; }
  public DbSet<CertificateCategory> CertificateCategories { get; set; }
 public DbSet<CertificateRequirement> CertificateRequirements { get; set; }
    public DbSet<CertificateHolder> CertificateHolders { get; set; }

        // Feature 6: SEO & Analytics
    public DbSet<SeoMetadata> SeoMetadata { get; set; }
    public DbSet<PageAnalytics> PageAnalytics { get; set; }
    public DbSet<AnalyticsEvent> AnalyticsEvents { get; set; }
    public DbSet<Keyword> Keywords { get; set; }

        // Feature 7: Content Management
        public DbSet<ContentType> ContentTypes { get; set; }
     public DbSet<ContentTypeField> ContentTypeFields { get; set; }
        public DbSet<Content> Contents { get; set; }
   public DbSet<Tag> Tags { get; set; }
   public DbSet<ContentTag> ContentTags { get; set; }
        public DbSet<ContentFieldValue> ContentFieldValues { get; set; }

     // Feature 8: Layout Management
        public DbSet<Layout> Layouts { get; set; }

        // Feature 9: Media & Album Management
        public DbSet<Media> Media { get; set; }
        public DbSet<Album> Albums { get; set; }
   public DbSet<AlbumMedia> AlbumMedias { get; set; }

        // Feature 10: Short Links with QR Code
        public DbSet<ShortLink> ShortLinks { get; set; }
        public DbSet<ShortLinkAnalytics> ShortLinkAnalytics { get; set; }
        
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
                new Language { Id = 1, Code = "ar", Name = "???????", IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
      new Language { Id = 2, Code = "en", Name = "English", IsDefault = false, IsActive = true, CreatedAt = DateTime.UtcNow }
    );

        // Seed Super Admin Role
            modelBuilder.Entity<Role>().HasData(
             new Role { Id = 1, Name = "SuperAdmin", DisplayName = "???? ??????", CreatedAt = DateTime.UtcNow }
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
