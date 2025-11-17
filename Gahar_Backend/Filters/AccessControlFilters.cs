using Gahar_Backend.Extensions;
using Gahar_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gahar_Backend.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequirePermissionAttribute : Attribute, IAsyncActionFilter
    {
       public string[] Permissions { get; set; } = Array.Empty<string>();
  public bool RequireAll { get; set; } = false;

        public RequirePermissionAttribute(params string[] permissions)
     {
            Permissions = permissions;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
      {
            var permissionService = context.HttpContext.RequestServices.GetService(typeof(IPermissionService))
                as IPermissionService;

       if (permissionService == null)
            {
            context.Result = new BadRequestObjectResult("خدمة الصلاحيات غير متاحة");
       return;
            }

     var userId = context.HttpContext.User.GetUserId();

       if (userId == 0)
            {
     context.Result = new UnauthorizedObjectResult("لم يتم تسجيل الدخول");
   return;
            }

            var hasPermission = RequireAll
 ? await permissionService.HasAllPermissionsAsync(userId, Permissions)
         : await permissionService.HasAnyPermissionAsync(userId, Permissions);

  if (!hasPermission)
           {
 context.Result = new ObjectResult("ليس لديك صلاحية لهذه العملية") { StatusCode = 403 };
       return;
            }

        await next();
        }
    }

 [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireDepartmentAccessAttribute : Attribute, IAsyncActionFilter
    {
        public string ActionType { get; set; } = "view";

        public RequireDepartmentAccessAttribute(string actionType = "view")
   {
     ActionType = actionType;
    }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
     var departmentPermissionService = context.HttpContext.RequestServices
    .GetService(typeof(IDepartmentPermissionService))
                as IDepartmentPermissionService;

       if (departmentPermissionService == null)
      {
                context.Result = new BadRequestObjectResult("خدمة صلاحيات الأقسام غير متاحة");
        return;
            }

  var userId = context.HttpContext.User.GetUserId();

   if (userId == 0)
      {
     context.Result = new UnauthorizedObjectResult("لم يتم تسجيل الدخول");
                return;
      }

            if (!context.RouteData.Values.TryGetValue("departmentId", out var departmentIdObj))
            {
 context.Result = new BadRequestObjectResult("معرف القسم غير موجود");
    return;
 }

  if (!Guid.TryParse(departmentIdObj?.ToString(), out var departmentId))
            {
    context.Result = new BadRequestObjectResult("معرف القسم غير صحيح");
         return;
      }

            var canAccess = ActionType.ToLower() switch
         {
      "view" => await departmentPermissionService.CanViewDepartmentDataAsync(userId, departmentId),
   "manage" => await departmentPermissionService.CanManageDepartmentAsync(userId, departmentId),
   _ => false
         };

  if (!canAccess)
            {
                context.Result = new ObjectResult("ليس لديك صلاحية على هذا القسم") { StatusCode = 403 };
        return;
         }

     await next();
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireRoleAttribute : Attribute, IAsyncActionFilter
    {
        public string[] Roles { get; set; } = Array.Empty<string>();
       public bool RequireAll { get; set; } = false;

        public RequireRoleAttribute(params string[] roles)
   {
            Roles = roles;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
      var roleBasedAccessService = context.HttpContext.RequestServices
     .GetService(typeof(IRoleBasedAccessService))
          as IRoleBasedAccessService;

 if (roleBasedAccessService == null)
     {
    context.Result = new BadRequestObjectResult("خدمة الأدوار غير متاحة");
    return;
            }

            var userId = context.HttpContext.User.GetUserId();

            if (userId == 0)
  {
        context.Result = new UnauthorizedObjectResult("لم يتم تسجيل الدخول");
      return;
  }

    var hasRole = RequireAll
                ? await roleBasedAccessService.HasAnyRoleAsync(userId, Roles) // تصحيح المنطق
   : await roleBasedAccessService.HasAnyRoleAsync(userId, Roles);

       if (!hasRole)
            {
     context.Result = new ObjectResult("ليس لديك الدور المطلوب") { StatusCode = 403 };
        return;
     }

        await next();
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireEntityOwnershipAttribute : Attribute, IAsyncActionFilter
    {
     public string EntityType { get; set; }
     public string EntityIdParameterName { get; set; } = "id";

        public RequireEntityOwnershipAttribute(string entityType)
        {
          EntityType = entityType;
        }

  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
     {
        var permissionService = context.HttpContext.RequestServices.GetService(typeof(IPermissionService))
      as IPermissionService;

  if (permissionService == null)
      {
   context.Result = new BadRequestObjectResult("خدمة الصلاحيات غير متاحة");
  return;
      }

var userId = context.HttpContext.User.GetUserId();

     if (userId == 0)
        {
      context.Result = new UnauthorizedObjectResult("لم يتم تسجيل الدخول");
  return;
            }

 if (!context.RouteData.Values.TryGetValue(EntityIdParameterName, out var entityIdObj) ||
  !int.TryParse(entityIdObj?.ToString(), out var entityId))
  {
           context.Result = new BadRequestObjectResult($"معرف {EntityType} غير موجود أو صحيح");
      return;
            }

            var isOwner = await permissionService.IsEntityOwnerAsync(userId, entityId, EntityType);

       if (!isOwner)
   {
     context.Result = new ObjectResult("ليس لديك صلاحية على هذا العنصر") { StatusCode = 403 };
          return;
        }

            await next();
        }
    }
}
