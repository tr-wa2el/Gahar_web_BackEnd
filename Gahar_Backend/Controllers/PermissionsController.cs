using Gahar_Backend.Extensions;
using Gahar_Backend.Filters;
using Gahar_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gahar_Backend.Controllers
{
[ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
   private readonly IPermissionService _permissionService;
        private readonly IDataAccessService _dataAccessService;
    private readonly IDepartmentPermissionService _departmentPermissionService;
        private readonly IRoleBasedAccessService _roleBasedAccessService;
  private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(
  IPermissionService permissionService,
     IDataAccessService dataAccessService,
            IDepartmentPermissionService departmentPermissionService,
            IRoleBasedAccessService roleBasedAccessService,
      ILogger<PermissionsController> logger)
        {
         _permissionService = permissionService;
          _dataAccessService = dataAccessService;
            _departmentPermissionService = departmentPermissionService;
  _roleBasedAccessService = roleBasedAccessService;
            _logger = logger;
        }

[HttpPost("check")]
        public async Task<IActionResult> CheckPermission([FromBody] CheckPermissionRequest request)
        {
    var userId = User.GetUserId();

            if (userId == 0)
               return Unauthorized("لم يتم تسجيل الدخول");

   try
            {
           var hasPermission = await _permissionService.HasPermissionAsync(userId, request.PermissionName);

 return Ok(new
                {
        userId,
      permissionName = request.PermissionName,
    hasPermission
   });
      }
          catch (Exception ex)
  {
       _logger.LogError(ex, "خطأ في التحقق من الصلاحية");
         return StatusCode(500, "حدث خطأ أثناء التحقق من الصلاحية");
  }
        }

        [HttpGet("my-permissions")]
 public async Task<IActionResult> GetMyPermissions()
        {
 var userId = User.GetUserId();

     if (userId == 0)
    return Unauthorized("لم يتم تسجيل الدخول");

   try
         {
          var permissions = await _permissionService.GetUserPermissionsAsync(userId);
 return Ok(new
        {
   userId,
               permissions
            });
   }
           catch (Exception ex)
 {
 _logger.LogError(ex, "خطأ في الحصول على الصلاحيات");
             return StatusCode(500, "حدث خطأ أثناء الحصول على الصلاحيات");
       }
}

[HttpGet("my-roles")]
 public async Task<IActionResult> GetMyRoles()
  {
  var userId = User.GetUserId();

            if (userId == 0)
          return Unauthorized("لم يتم تسجيل الدخول");

     try
     {
      var roles = await _roleBasedAccessService.GetUserRolesAsync(userId);
         return Ok(new
          {
  userId,
   roles
           });
   }
     catch (Exception ex)
            {
      _logger.LogError(ex, "خطأ في الحصول على الأدوار");
    return StatusCode(500, "حدث خطأ أثناء الحصول على الأدوار");
              }
        }

 [HttpGet("all-permissions")]
  [RequireRole("Admin", "SuperAdmin")]
    public async Task<IActionResult> GetAllPermissions()
      {
 try
       {
              var permissions = await _permissionService.GetAllPermissionsAsync();
         return Ok(permissions);
  }
         catch (Exception ex)
    {
         _logger.LogError(ex, "خطأ في الحصول على جميع الصلاحيات");
 return StatusCode(500, "حدث خطأ أثناء الحصول على جميع الصلاحيات");
     }
        }

 [HttpPost("create")]
      [RequireRole("Admin", "SuperAdmin")]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionRequest request)
           {
  if (string.IsNullOrWhiteSpace(request.Name))
       return BadRequest("اسم الصلاحية مطلوب");

  try
   {
            var result = await _permissionService.CreatePermissionAsync(request.Name, request.Description);
    if (!result)
    return BadRequest("فشل إنشاء الصلاحية - قد تكون موجودة بالفعل");

       return Ok("تم إنشاء الصلاحية بنجاح");
 }
    catch (Exception ex)
       {
        _logger.LogError(ex, "خطأ في إنشاء الصلاحية");
    return StatusCode(500, "حدث خطأ أثناء إنشاء الصلاحية");
  }
        }

  [HttpPost("add-to-role")]
 [RequireRole("Admin", "SuperAdmin")]
 public async Task<IActionResult> AddPermissionToRole([FromBody] AddPermissionToRoleRequest request)
      {
      if (request.RoleId <= 0 || string.IsNullOrWhiteSpace(request.PermissionName))
   return BadRequest("معرف الـ Role واسم الصلاحية مطلوبة");

   try
  {
             var result = await _permissionService.AddPermissionToRoleAsync(request.RoleId, request.PermissionName);
 if (!result)
 return BadRequest("فشل إضافة الصلاحية - تحقق من معرف الـ Role واسم الصلاحية");

   return Ok("تمت إضافة الصلاحية بنجاح");
         }
       catch (Exception ex)
               {
         _logger.LogError(ex, "خطأ في إضافة الصلاحية للـ Role");
     return StatusCode(500, "حدث خطأ أثناء إضافة الصلاحية");
   }
       }

       [HttpDelete("remove-from-role")]
  [RequireRole("Admin", "SuperAdmin")]
 public async Task<IActionResult> RemovePermissionFromRole([FromBody] RemovePermissionFromRoleRequest request)
        {
        if (request.RoleId <= 0 || string.IsNullOrWhiteSpace(request.PermissionName))
       return BadRequest("معرف الـ Role واسم الصلاحية مطلوبة");

      try
     {
      var result = await _permissionService.RemovePermissionFromRoleAsync(request.RoleId, request.PermissionName);
   if (!result)
    return BadRequest("فشل إزالة الصلاحية");

       return Ok("تمت إزالة الصلاحية بنجاح");
         }
  catch (Exception ex)
   {
   _logger.LogError(ex, "خطأ في إزالة الصلاحية من الـ Role");
  return StatusCode(500, "حدث خطأ أثناء إزالة الصلاحية");
 }
       }

        [HttpGet("accessible-departments")]
      public async Task<IActionResult> GetAccessibleDepartments()
  {
     var userId = User.GetUserId();

    if (userId == 0)
           return Unauthorized("لم يتم تسجيل الدخول");

    try
        {
   var departments = await _departmentPermissionService.GetAccessibleDepartmentsAsync(userId);
        return Ok(new
        {
    userId,
    departments
       });
       }
     catch (Exception ex)
       {
_logger.LogError(ex, "خطأ في الحصول على الأقسام المتاحة");
          return StatusCode(500, "حدث خطأ أثناء الحصول على الأقسام");
           }
        }

[HttpPost("check-entity-access")]
        public async Task<IActionResult> CheckEntityAccess([FromBody] CheckEntityAccessRequest request)
     {
var userId = User.GetUserId();

   if (userId == 0)
   return Unauthorized("لم يتم تسجيل الدخول");

           try
         {
     var canAccess = await _permissionService.CanAccessEntityAsync(
userId,
   request.EntityId,
    request.EntityType,
            request.Action
    );

    return Ok(new
 {
      userId,
       entityId = request.EntityId,
        entityType = request.EntityType,
  action = request.Action,
   canAccess
          });
          }
 catch (Exception ex)
            {
  _logger.LogError(ex, "خطأ في التحقق من الوصول للكيان");
   return StatusCode(500, "حدث خطأ أثناء التحقق من الوصول");
            }
  }
    }

    public class CheckPermissionRequest
    {
    public string PermissionName { get; set; } = string.Empty;
  }

    public class CreatePermissionRequest
    {
  public string Name { get; set; } = string.Empty;
 public string? Description { get; set; }
      }

    public class AddPermissionToRoleRequest
     {
        public int RoleId { get; set; }
      public string PermissionName { get; set; } = string.Empty;
    }

 public class RemovePermissionFromRoleRequest
 {
        public int RoleId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
   }

    public class CheckEntityAccessRequest
 {
  public int EntityId { get; set; }
   public string EntityType { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
    }
}
