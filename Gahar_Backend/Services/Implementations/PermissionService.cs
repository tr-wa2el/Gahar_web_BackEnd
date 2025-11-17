using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gahar_Backend.Services.Implementations
{
    /// <summary>
    /// تنفيذ خدمة الصلاحيات العام
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _dbContext;
   private readonly ILogger<PermissionService> _logger;

        public PermissionService(ApplicationDbContext dbContext, ILogger<PermissionService> logger)
 {
        _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// التحقق من أن المستخدم له صلاحية معينة
        /// </summary>
public async Task<bool> HasPermissionAsync(int userId, string permissionName)
        {
        try
        {
    var user = await _dbContext.Users
         .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
  .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
        .FirstOrDefaultAsync(u => u.Id == userId);

 if (user == null)
     return false;

        // Admin يملك جميع الصلاحيات
          if (user.UserRoles?.Any(ur => ur.Role?.Name == "Admin") == true)
 return true;

          // البحث عن الصلاحية في أدوار المستخدم
    var hasPermission = user.UserRoles?
   .SelectMany(ur => ur.Role?.RolePermissions ?? new List<RolePermission>())
           .Any(rp => rp.Permission?.Name == permissionName) ?? false;

             return hasPermission;
            }
          catch (Exception ex)
            {
           _logger.LogError(ex, "خطأ في التحقق من الصلاحية {PermissionName} للمستخدم {UserId}",
     permissionName, userId);
                return false;
      }
        }

      /// <summary>
        /// التحقق من أن المستخدم له أي من الصلاحيات المعطاة
        /// </summary>
        public async Task<bool> HasAnyPermissionAsync(int userId, params string[] permissionNames)
   {
            if (permissionNames == null || permissionNames.Length == 0)
         return true;

       foreach (var permissionName in permissionNames)
        {
         if (await HasPermissionAsync(userId, permissionName))
             return true;
       }

    return false;
     }

  /// <summary>
        /// التحقق من أن المستخدم له جميع الصلاحيات المعطاة
    /// </summary>
        public async Task<bool> HasAllPermissionsAsync(int userId, params string[] permissionNames)
  {
      if (permissionNames == null || permissionNames.Length == 0)
         return true;

            foreach (var permissionName in permissionNames)
     {
        if (!await HasPermissionAsync(userId, permissionName))
   return false;
     }

        return true;
        }

        /// <summary>
  /// الحصول على جميع الصلاحيات للمستخدم
        /// </summary>
 public async Task<List<string>> GetUserPermissionsAsync(int userId)
        {
  try
            {
        var user = await _dbContext.Users
      .Include(u => u.UserRoles)
     .ThenInclude(ur => ur.Role)
         .ThenInclude(r => r.RolePermissions)
    .ThenInclude(rp => rp.Permission)
  .FirstOrDefaultAsync(u => u.Id == userId);

          if (user == null)
   return new List<string>();

         var permissions = user.UserRoles?
    .SelectMany(ur => ur.Role?.RolePermissions ?? new List<RolePermission>())
         .Select(rp => rp.Permission?.Name)
       .Where(name => name != null)
         .Distinct()
     .ToList() ?? new List<string>();

    return permissions;
        }
  catch (Exception ex)
        {
    _logger.LogError(ex, "خطأ في الحصول على صلاحيات المستخدم {UserId}", userId);
     return new List<string>();
   }
        }

        /// <summary>
        /// التحقق من أن المستخدم له صلاحية معينة على Entity معين
        /// </summary>
        public async Task<bool> CanAccessEntityAsync(int userId, int entityId, string entityType, string action)
      {
            try
   {
        // التحقق من الصلاحية الأساسية
       var permissionName = $"{entityType}_{action}";
            if (!await HasPermissionAsync(userId, permissionName))
          return false;

          // إذا كانت العملية على مستوى النظام، لا نحتاج تحقق إضافي
  if (action.ToLower() == "view_all" || action.ToLower() == "manage")
    return true;

// التحقق من ملكية Entity
         return await IsEntityOwnerAsync(userId, entityId, entityType);
         }
 catch (Exception ex)
     {
    _logger.LogError(ex, "خطأ في التحقق من الوصول للـ {EntityType} {EntityId}",
 entityType, entityId);
   return false;
  }
        }

  /// <summary>
        /// التحقق من أن المستخدم يملك Entity معين
        /// </summary>
        public async Task<bool> IsEntityOwnerAsync(int userId, int entityId, string entityType)
        {
            try
     {
         // التحقق من ملكية أنواع مختلفة من الـ Entities
     switch (entityType.ToLower())
      {
      case "form":
       var form = await _dbContext.Forms.FirstOrDefaultAsync(f => f.Id == entityId);
                return form?.AuthorId == userId;

      case "content":
     var content = await _dbContext.Set<Content>().FirstOrDefaultAsync(c => c.Id == entityId);
             return content != null; // نحتاج تحقق من البنية

   case "page":
       var page = await _dbContext.Set<Page>().FirstOrDefaultAsync(p => p.Id == entityId);
  return page != null; // نحتاج تحقق من البنية

              case "album":
         var album = await _dbContext.Set<Album>().FirstOrDefaultAsync(a => a.Id == entityId);
         return album != null; // نحتاج تحقق من البنية

           default:
            return false;
  }
       }
            catch (Exception ex)
            {
       _logger.LogError(ex, "خطأ في التحقق من ملكية {EntityType} {EntityId}",
           entityType, entityId);
  return false;
   }
   }

        /// <summary>
        /// إنشاء صلاحية جديدة
        /// </summary>
     public async Task<bool> CreatePermissionAsync(string name, string description)
        {
          try
            {
              // التحقق من عدم وجود صلاحية بنفس الاسم
                var exists = await _dbContext.Permissions
        .AnyAsync(p => p.Name == name);

     if (exists)
      return false;

         var permission = new Permission
    {
          Name = name,
     Description = description
  };

_dbContext.Permissions.Add(permission);
         await _dbContext.SaveChangesAsync();

_logger.LogInformation("تم إنشاء صلاحية جديدة: {PermissionName}", name);
      return true;
      }
      catch (Exception ex)
            {
             _logger.LogError(ex, "خطأ في إنشاء الصلاحية {PermissionName}", name);
      return false;
    }
        }

        /// <summary>
  /// إضافة صلاحية لـ Role
        /// </summary>
        public async Task<bool> AddPermissionToRoleAsync(int roleId, string permissionName)
        {
            try
      {
      var role = await _dbContext.Roles
          .Include(r => r.RolePermissions)
      .FirstOrDefaultAsync(r => r.Id == roleId);

                if (role == null)
            return false;

          var permission = await _dbContext.Permissions
           .FirstOrDefaultAsync(p => p.Name == permissionName);

    if (permission == null)
      return false;

  // التحقق من عدم وجود الصلاحية بالفعل
     if (role.RolePermissions?.Any(rp => rp.PermissionId == permission.Id) == true)
         return false;

     var rolePermission = new RolePermission
      {
         RoleId = roleId,
  PermissionId = permission.Id
       };

          _dbContext.RolePermissions.Add(rolePermission);
                await _dbContext.SaveChangesAsync();

           _logger.LogInformation("تم إضافة الصلاحية {PermissionName} للـ Role {RoleId}",
     permissionName, roleId);
       return true;
            }
       catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إضافة الصلاحية {PermissionName} للـ Role {RoleId}",
      permissionName, roleId);
                return false;
      }
        }

   /// <summary>
     /// إزالة صلاحية من Role
  /// </summary>
        public async Task<bool> RemovePermissionFromRoleAsync(int roleId, string permissionName)
        {
            try
     {
 var permission = await _dbContext.Permissions
  .FirstOrDefaultAsync(p => p.Name == permissionName);

     if (permission == null)
     return false;

       var rolePermission = await _dbContext.RolePermissions
     .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id);

           if (rolePermission == null)
   return false;

            _dbContext.RolePermissions.Remove(rolePermission);
       await _dbContext.SaveChangesAsync();

  _logger.LogInformation("تم إزالة الصلاحية {PermissionName} من Role {RoleId}",
      permissionName, roleId);
       return true;
     }
            catch (Exception ex)
{
       _logger.LogError(ex, "خطأ في إزالة الصلاحية {PermissionName} من Role {RoleId}",
           permissionName, roleId);
       return false;
          }
 }

        /// <summary>
        /// الحصول على جميع الصلاحيات المتاحة
        /// </summary>
        public async Task<List<PermissionDto>> GetAllPermissionsAsync()
 {
      try
       {
    var permissions = await _dbContext.Permissions
     .Include(p => p.RolePermissions)
 .ThenInclude(rp => rp.Role)
             .ToListAsync();

    return permissions.Select(p => new PermissionDto
  {
          Id = p.Id,
          Name = p.Name,
 Description = p.Description,
            Roles = p.RolePermissions?
        .Select(rp => rp.Role?.Name)
  .Where(name => name != null)
      .ToList() ?? new List<string>()
     }).ToList();
}
            catch (Exception ex)
 {
       _logger.LogError(ex, "خطأ في الحصول على جميع الصلاحيات");
    return new List<PermissionDto>();
            }
    }
    }
}
