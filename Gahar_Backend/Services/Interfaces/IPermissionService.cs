using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gahar_Backend.Services
{
public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(int userId, string permissionName);
      Task<bool> HasAnyPermissionAsync(int userId, params string[] permissionNames);
        Task<bool> HasAllPermissionsAsync(int userId, params string[] permissionNames);
      Task<List<string>> GetUserPermissionsAsync(int userId);
 Task<bool> CanAccessEntityAsync(int userId, int entityId, string entityType, string action);
        Task<bool> IsEntityOwnerAsync(int userId, int entityId, string entityType);
        Task<bool> CreatePermissionAsync(string name, string description);
        Task<bool> AddPermissionToRoleAsync(int roleId, string permissionName);
        Task<bool> RemovePermissionFromRoleAsync(int roleId, string permissionName);
        Task<List<PermissionDto>> GetAllPermissionsAsync();
    }

    public interface IDataAccessService
    {
        Task<bool> IsInSameDepartmentAsync(int userId, int entityAuthorId);
        Task<Expression<Func<T, bool>>> GetAccessFilterAsync<T>(int userId) where T : class;
        Task<bool> CanViewEntityAsync(int userId, int entityAuthorId, string accessLevel);
        Task<bool> CanEditEntityAsync(int userId, int entityAuthorId, string accessLevel);
   Task<bool> CanDeleteEntityAsync(int userId, int entityAuthorId, string accessLevel);
    }

    public interface IDepartmentPermissionService
    {
  Task<bool> CanViewDepartmentDataAsync(int userId, Guid departmentId);
  Task<bool> CanManageDepartmentAsync(int userId, Guid departmentId);
        Task<List<Guid>> GetAccessibleDepartmentsAsync(int userId);
    }

    public interface IRoleBasedAccessService
    {
        Task<bool> HasRoleAsync(int userId, string roleName);
      Task<List<string>> GetUserRolesAsync(int userId);
      Task<bool> HasAnyRoleAsync(int userId, params string[] roleNames);
   Task<bool> IsAdminAsync(int userId);
    }

public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<string> Roles { get; set; } = new();
    }

    public class RolePermissionsDto
    {
        public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
        public List<PermissionDto> Permissions { get; set; } = new();
    }

    public class AccessCheckDto
    {
  public int UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public bool CanAccess { get; set; }
public string? Reason { get; set; }
    }
}
