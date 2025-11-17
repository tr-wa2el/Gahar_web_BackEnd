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
    /// تنفيذ خدمة الصلاحيات حسب الأدوار
    /// </summary>
    public class RoleBasedAccessService : IRoleBasedAccessService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<RoleBasedAccessService> _logger;

        public RoleBasedAccessService(ApplicationDbContext dbContext, ILogger<RoleBasedAccessService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// التحقق من أن المستخدم له دور معين
        /// </summary>
        public async Task<bool> HasRoleAsync(int userId, string roleName)
        {
            try
            {
                var hasRole = await _dbContext.UserRoles
                    .Include(ur => ur.Role)
                    .AnyAsync(ur => ur.UserId == userId && ur.Role.Name == roleName);

                return hasRole;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في التحقق من الدور {RoleName} للمستخدم {UserId}",
                    roleName, userId);
                return false;
            }
        }

        /// <summary>
        /// الحصول على أدوار المستخدم
        /// </summary>
        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            try
            {
                var roles = await _dbContext.UserRoles
                    .Include(ur => ur.Role)
                    .Where(ur => ur.UserId == userId)
                    .Select(ur => ur.Role.Name)
                    .ToListAsync();

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في الحصول على أدوار المستخدم {UserId}", userId);
                return new List<string>();
            }
        }

        /// <summary>
        /// التحقق من أن المستخدم له أي من الأدوار المعطاة
        /// </summary>
        public async Task<bool> HasAnyRoleAsync(int userId, params string[] roleNames)
        {
            try
            {
                if (roleNames == null || roleNames.Length == 0)
                    return false;

                var hasRole = await _dbContext.UserRoles
                    .Include(ur => ur.Role)
                    .AnyAsync(ur => ur.UserId == userId &&
                        roleNames.Contains(ur.Role.Name));

                return hasRole;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في التحقق من الأدوار للمستخدم {UserId}", userId);
                return false;
            }
        }

        /// <summary>
        /// التحقق من أن المستخدم هو Admin أو SuperAdmin
        /// </summary>
        public async Task<bool> IsAdminAsync(int userId)
        {
            try
            {
                return await HasAnyRoleAsync(userId, "Admin", "SuperAdmin");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في التحقق من أن المستخدم Admin {UserId}", userId);
                return false;
            }
        }
    }
}
