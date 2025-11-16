using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _permission;

   public PermissionAttribute(string permission)
   {
       _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
   {
     var user = context.HttpContext.User;

       if (!user.Identity?.IsAuthenticated ?? true)
   {
context.Result = new UnauthorizedResult();
    return;
}

         var permissions = user.Claims
      .Where(c => c.Type == "Permission")
    .Select(c => c.Value)
                .ToList();

     if (!permissions.Contains(_permission))
      {
     context.Result = new ForbidResult();
       }
        }
    }
}
