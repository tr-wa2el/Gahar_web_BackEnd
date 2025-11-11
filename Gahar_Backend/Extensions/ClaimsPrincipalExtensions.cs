using System.Security.Claims;

namespace Gahar_Backend.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
  var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

     public static string GetUserEmail(this ClaimsPrincipal principal)
        {
       return principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
}

  public static string GetUsername(this ClaimsPrincipal principal)
 {
   return principal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        }

     public static List<string> GetUserRoles(this ClaimsPrincipal principal)
      {
return principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
  }

        public static List<string> GetUserPermissions(this ClaimsPrincipal principal)
        {
            return principal.FindAll("Permission").Select(c => c.Value).ToList();
}

        public static bool HasPermission(this ClaimsPrincipal principal, string permission)
     {
   return principal.FindAll("Permission").Any(c => c.Value == permission);
 }

public static bool HasRole(this ClaimsPrincipal principal, string role)
      {
  return principal.IsInRole(role);
   }

        public static bool HasAnyRole(this ClaimsPrincipal principal, params string[] roles)
{
 return roles.Any(role => principal.IsInRole(role));
        }

        public static bool HasAllRoles(this ClaimsPrincipal principal, params string[] roles)
 {
        return roles.All(role => principal.IsInRole(role));
 }
    }
}
