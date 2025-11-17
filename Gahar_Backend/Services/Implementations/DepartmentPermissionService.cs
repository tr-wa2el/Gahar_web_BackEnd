using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gahar_Backend.Services.Implementations
{
    /// <summary>
    /// تنفيذ خدمة الصلاحيات حسب الأقسام
    /// </summary>
    public class DepartmentPermissionService : IDepartmentPermissionService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DepartmentPermissionService> _logger;
        private readonly IRoleBasedAccessService _roleBasedAccessService;

        public DepartmentPermissionService(
         ApplicationDbContext dbContext,
             ILogger<DepartmentPermissionService> logger,
   IRoleBasedAccessService roleBasedAccessService)
        {
  _dbContext = dbContext;
      _logger = logger;
  _roleBasedAccessService = roleBasedAccessService;
       }

  /// <summary>
   /// التحقق من أن المستخدم يمكنه رؤية بيانات قسم معين
        /// </summary>
   public async Task<bool> CanViewDepartmentDataAsync(int userId, Guid departmentId)
  {
      try
  {
   // Admin يمكنه رؤية جميع البيانات
     var isAdmin = await _roleBasedAccessService.IsAdminAsync(userId);
       if (isAdmin)
           return true;

// الحصول على معلومات المستخدم
     var user = await _dbContext.Users
         .FirstOrDefaultAsync(u => u.Id == userId);

    if (user == null)
           return false;

  // المستخدم يمكنه رؤية بيانات قسمه فقط
        return user.DepartmentId == departmentId;
   }
           catch (Exception ex)
   {
   _logger.LogError(ex, "خطأ في التحقق من رؤية بيانات القسم {DepartmentId}", departmentId);
       return false;
  }
}

/// <summary>
        /// التحقق من أن المستخدم يمكنه إدارة قسم معين
      /// </summary>
       public async Task<bool> CanManageDepartmentAsync(int userId, Guid departmentId)
 {
        try
       {
       // Admin يمكنه إدارة جميع الأقسام
      var isAdmin = await _roleBasedAccessService.IsAdminAsync(userId);
     if (isAdmin)
           return true;

 // الحصول على معلومات المستخدم مع الأقسام
 var user = await _dbContext.Users
    .Include(u => u.Department)
    .FirstOrDefaultAsync(u => u.Id == userId);

  if (user == null)
      return false;

      // رئيس القسم يمكنه إدارة قسمه فقط
     if (user.DepartmentId == departmentId)
         return true;

        return false;
        }
      catch (Exception ex)
   {
      _logger.LogError(ex, "خطأ في التحقق من إدارة القسم {DepartmentId}", departmentId);
          return false;
 }
 }

      /// <summary>
     /// الحصول على الأقسام التي المستخدم يمكنه رؤيتها
     /// </summary>
   public async Task<List<Guid>> GetAccessibleDepartmentsAsync(int userId)
     {
 try
  {
        // Admin يمكنه رؤية جميع الأقسام
     var isAdmin = await _roleBasedAccessService.IsAdminAsync(userId);
     if (isAdmin)
    {
   return await _dbContext.Departments
      .Select(d => d.Id)
       .ToListAsync();
 }

   // الحصول على معلومات المستخدم
   var user = await _dbContext.Users
    .FirstOrDefaultAsync(u => u.Id == userId);

     if (user == null)
          return new List<Guid>();

      // المستخدم يشوف قسمه فقط
     return user.DepartmentId.HasValue
     ? new List<Guid> { user.DepartmentId.Value }
  : new List<Guid>();
        }
    catch (Exception ex)
       {
 _logger.LogError(ex, "خطأ في الحصول على الأقسام المتاحة للمستخدم {UserId}", userId);
  return new List<Guid>();
 }
}
   }
}
