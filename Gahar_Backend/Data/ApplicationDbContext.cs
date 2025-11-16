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

        // CMS Core Tables (Developer 1 Features)
        public DbSet<ContentType> ContentTypes { get; set; }
     public DbSet<ContentTypeField> ContentTypeFields { get; set; }
        public DbSet<Content> Contents { get; set; }
 public DbSet<ContentFieldValue> ContentFieldValues { get; set; }
 public DbSet<Tag> Tags { get; set; }
        public DbSet<ContentTag> ContentTags { get; set; }
 public DbSet<Layout> Layouts { get; set; }
        public DbSet<Media> Media { get; set; }
 public DbSet<Album> Albums { get; set; }
        public DbSet<AlbumMedia> AlbumMedias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
    base.OnModelCreating(modelBuilder);

      // Apply all configurations from assembly
     modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

   // Global Query Filters
   modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Layout>().HasQueryFilter(l => !l.IsDeleted);
    modelBuilder.Entity<ContentType>().HasQueryFilter(ct => !ct.IsDeleted);
         modelBuilder.Entity<Content>().HasQueryFilter(c => !c.IsDeleted);
     modelBuilder.Entity<Media>().HasQueryFilter(m => !m.IsDeleted);
    modelBuilder.Entity<Album>().HasQueryFilter(a => !a.IsDeleted);
     
   // Seed initial data
 SeedData(modelBuilder);
  }

   private void SeedData(ModelBuilder modelBuilder)
  {
            // Seed Languages
modelBuilder.Entity<Language>().HasData(
     new Language { Id = 1, Code = "ar", Name = "العربية", IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
      new Language { Id = 2, Code = "en", Name = "English", IsDefault = false, IsActive = true, CreatedAt = DateTime.UtcNow }
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
