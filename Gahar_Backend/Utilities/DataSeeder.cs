using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gahar_Backend.Utilities
{
    public static class DataSeeder
    {
     public static async Task SeedAsync(ApplicationDbContext context)
        {
  await SeedLanguagesAsync(context);
     await SeedRolesAsync(context);
     await SeedPermissionsAsync(context);
    await SeedSuperAdminAsync(context);
  }

        private static async Task SeedLanguagesAsync(ApplicationDbContext context)
        {
     if (await context.Languages.AnyAsync())
    return;

         var languages = new List<Language>
{
      new Language
 {
      Code = "ar",
     Name = "العربية",
IsDefault = true,
        IsActive = true,
   Direction = "rtl"
 },
    new Language
       {
      Code = "en",
  Name = "English",
       IsDefault = false,
         IsActive = true,
   Direction = "ltr"
  }
            };

await context.Languages.AddRangeAsync(languages);
       await context.SaveChangesAsync();
 }

        private static async Task SeedRolesAsync(ApplicationDbContext context)
     {
  if (await context.Roles.AnyAsync())
       return;

            var roles = new List<Role>
    {
      new Role
  {
     Name = "SuperAdmin",
 DisplayName = "مدير النظام",
      Description = "صلاحيات كاملة للنظام",
    IsSystemRole = true
   },
    new Role
     {
             Name = "Admin",
DisplayName = "مدير",
        Description = "صلاحيات إدارية",
       IsSystemRole = true
  },
  new Role
     {
       Name = "Editor",
       DisplayName = "محرر",
   Description = "صلاحيات تحرير المحتوى",
      IsSystemRole = true
     },
    new Role
  {
       Name = "Viewer",
     DisplayName = "مشاهد",
     Description = "صلاحيات المشاهدة فقط",
    IsSystemRole = true
       }
  };

await context.Roles.AddRangeAsync(roles);
         await context.SaveChangesAsync();
 }

   private static async Task SeedPermissionsAsync(ApplicationDbContext context)
   {
 if (await context.Permissions.AnyAsync())
    return;

   var permissions = new List<Permission>
    {
    // User Management
  new Permission { Name = "Users.View", Module = "Users", Description = "عرض المستخدمين" },
 new Permission { Name = "Users.Create", Module = "Users", Description = "إنشاء مستخدم جديد" },
    new Permission { Name = "Users.Edit", Module = "Users", Description = "تعديل المستخدمين" },
                new Permission { Name = "Users.Delete", Module = "Users", Description = "حذف المستخدمين" },
    
    // Role Management
     new Permission { Name = "Roles.View", Module = "Roles", Description = "عرض الأدوار" },
   new Permission { Name = "Roles.Create", Module = "Roles", Description = "إنشاء دور جديد" },
    new Permission { Name = "Roles.Edit", Module = "Roles", Description = "تعديل الأدوار" },
    new Permission { Name = "Roles.Delete", Module = "Roles", Description = "حذف الأدوار" },
   
    // Content Types
    new Permission { Name = "ContentTypes.View", Module = "ContentTypes", Description = "عرض أنواع المحتوى" },
    new Permission { Name = "ContentTypes.Create", Module = "ContentTypes", Description = "إنشاء نوع محتوى جديد" },
    new Permission { Name = "ContentTypes.Edit", Module = "ContentTypes", Description = "تعديل أنواع المحتوى" },
    new Permission { Name = "ContentTypes.Delete", Module = "ContentTypes", Description = "حذف أنواع المحتوى" },

   // Content
             new Permission { Name = "Content.View", Module = "Content", Description = "عرض المحتوى" },
    new Permission { Name = "Content.Create", Module = "Content", Description = "إنشاء محتوى جديد" },
    new Permission { Name = "Content.Edit", Module = "Content", Description = "تعديل المحتوى" },
    new Permission { Name = "Content.Delete", Module = "Content", Description = "حذف المحتوى" },
    new Permission { Name = "Content.Publish", Module = "Content", Description = "نشر المحتوى" },

                // Pages
    new Permission { Name = "Pages.View", Module = "Pages", Description = "عرض الصفحات" },
    new Permission { Name = "Pages.Create", Module = "Pages", Description = "إنشاء صفحة جديدة" },
    new Permission { Name = "Pages.Edit", Module = "Pages", Description = "تعديل الصفحات" },
   new Permission { Name = "Pages.Delete", Module = "Pages", Description = "حذف الصفحات" },
         
   // Forms
       new Permission { Name = "Forms.View", Module = "Forms", Description = "عرض النماذج" },
      new Permission { Name = "Forms.Create", Module = "Forms", Description = "إنشاء نموذج جديد" },
          new Permission { Name = "Forms.Edit", Module = "Forms", Description = "تعديل النماذج" },
    new Permission { Name = "Forms.Delete", Module = "Forms", Description = "حذف النماذج" },
  
    // Audit Logs
                new Permission { Name = "AuditLogs.View", Module = "AuditLogs", Description = "عرض سجلات التدقيق" },
            };

            await context.Permissions.AddRangeAsync(permissions);
await context.SaveChangesAsync();
}

  private static async Task SeedSuperAdminAsync(ApplicationDbContext context)
        {
   // Check if SuperAdmin user already exists
 if (await context.Users.AnyAsync(u => u.Email == "admin@gahar.sa"))
    return;

            var superAdminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "SuperAdmin");
  if (superAdminRole == null)
   return;

   var superAdmin = new User
     {
    Username = "superadmin",
   Email = "admin@gahar.sa",
       PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
    FirstName = "Super",
     LastName = "Admin",
      IsActive = true,
   EmailConfirmed = true
          };

            await context.Users.AddAsync(superAdmin);
   await context.SaveChangesAsync();

      // Assign SuperAdmin role
            var userRole = new UserRole
  {
           UserId = superAdmin.Id,
        RoleId = superAdminRole.Id
   };

   await context.UserRoles.AddAsync(userRole);
 await context.SaveChangesAsync();

        // Assign all permissions to SuperAdmin role
    var allPermissions = await context.Permissions.ToListAsync();
var rolePermissions = allPermissions.Select(p => new RolePermission
  {
  RoleId = superAdminRole.Id,
   PermissionId = p.Id
            }).ToList();

await context.RolePermissions.AddRangeAsync(rolePermissions);
       await context.SaveChangesAsync();
      }
    }
}
