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
    /// تنفيذ خدمة التحكم في الوصول حسب البيانات
    /// </summary>
public class DataAccessService : IDataAccessService
    {
      private readonly ApplicationDbContext _dbContext;
   private readonly ILogger<DataAccessService> _logger;
    private readonly IRoleBasedAccessService _roleBasedAccessService;

        public DataAccessService(
       ApplicationDbContext dbContext,
            ILogger<DataAccessService> logger,
   IRoleBasedAccessService roleBasedAccessService)
        {
   _dbContext = dbContext;
     _logger = logger;
          _roleBasedAccessService = roleBasedAccessService;
      }

        /// <summary>
   /// التحقق من أن المستخدم في نفس القسم
    /// </summary>
    public async Task<bool> IsInSameDepartmentAsync(int userId, int entityAuthorId)
      {
           try
    {
      var user = await _dbContext.Users
.FirstOrDefaultAsync(u => u.Id == userId);

 var owner = await _dbContext.Users
         .FirstOrDefaultAsync(u => u.Id == entityAuthorId);

 if (user == null || owner == null)
      return false;

  return user.DepartmentId == owner.DepartmentId;
         }
   catch (Exception ex)
      {
      _logger.LogError(ex, "خطأ في التحقق من نفس القسم");
    return false;
      }
  }

        /// <summary>
    /// الحصول على Filter لـ Query يقصر البيانات حسب الصلاحيات
        /// </summary>
        public async Task<Expression<Func<T, bool>>> GetAccessFilterAsync<T>(int userId) 
        where T : class
     {
      // التحقق من أن المستخدم Admin
 var isAdmin = await _roleBasedAccessService.IsAdminAsync(userId);
      if (isAdmin)
   {
       // Admin يشوف جميع البيانات
      return x => true;
           }

      // الحصول على معلومات المستخدم
 var user = await _dbContext.Users
   .FirstOrDefaultAsync(u => u.Id == userId);

         if (user == null)
         {
        // بدون بيانات للمستخدم، لا يرى شيء
     return x => false;
      }

 // بناء Filter حسب نوع الكيان
        return BuildAccessFilter<T>(userId, user.DepartmentId);
        }

/// <summary>
        /// بناء Filter حسب نوع الكيان والقسم
         /// </summary>
        private Expression<Func<T, bool>> BuildAccessFilter<T>(int userId, Guid? userDepartmentId)
  where T : class
    {
  var entityType = typeof(T);

      // إذا كان الكيان لديه حقل DepartmentId، استخدم القسم
     if (entityType == typeof(Form))
 {
    Expression<Func<T, bool>> filter = x => ((Form)(object)x).DepartmentId == userDepartmentId;
   return filter;
 }

    // بشكل افتراضي، لا يرى شيء
return x => false;
        }

        /// <summary>
  /// التحقق من أن المستخدم يمكنه رؤية Entity معين
      /// </summary>
        public async Task<bool> CanViewEntityAsync(int userId, int entityAuthorId, string accessLevel)
 {
   try
   {
     var isAdmin = await _roleBasedAccessService.IsAdminAsync(userId);
         if (isAdmin)
       return true;

         // إذا كان المستخدم هو المالك
          if (userId == entityAuthorId)
     return true;

     // حسب مستوى الوصول
 return accessLevel?.ToLower() switch
   {
        "public" => true,
               "internal" => await IsInSameDepartmentAsync(userId, entityAuthorId),
     "private" => userId == entityAuthorId,
         _ => false
         };
        }
         catch (Exception ex)
 {
       _logger.LogError(ex, "خطأ في التحقق من الرؤية");
          return false;
            }
       }

  /// <summary>
/// التحقق من أن المستخدم يمكنه تعديل Entity معين
        /// </summary>
      public async Task<bool> CanEditEntityAsync(int userId, int entityAuthorId, string accessLevel)
 {
        try
 {
            var isAdmin = await _roleBasedAccessService.IsAdminAsync(userId);
    if (isAdmin)
            return true;

    // فقط المالك يمكنه التعديل
      return userId == entityAuthorId;
     }
            catch (Exception ex)
   {
        _logger.LogError(ex, "خطأ في التحقق من التعديل");
     return false;
     }
        }

        /// <summary>
      /// التحقق من أن المستخدم يمكنه حذف Entity معين
        /// </summary>
      public async Task<bool> CanDeleteEntityAsync(int userId, int entityAuthorId, string accessLevel)
 {
      try
     {
          var isAdmin = await _roleBasedAccessService.IsAdminAsync(userId);
   if (isAdmin)
       return true;

// فقط المالك يمكنه الحذف
        return userId == entityAuthorId;
    }
 catch (Exception ex)
        {
    _logger.LogError(ex, "خطأ في التحقق من الحذف");
        return false;
       }
    }
  }
}
