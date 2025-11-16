using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Filters
{
    public class AuditLogActionFilter : IAsyncActionFilter
    {
 private readonly IAuditLogService _auditLogService;

     public AuditLogActionFilter(IAuditLogService auditLogService)
        {
  _auditLogService = auditLogService;
 }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
 {
     var executedContext = await next();

       if (executedContext.Exception == null)
       {
      var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var action = context.ActionDescriptor.DisplayName;

       await _auditLogService.LogAsync(
 userId != null ? int.Parse(userId) : null,
  action ?? "Unknown",
   "Action",
    null,
  $"Action executed: {action}"
       );
            }
 }
    }
}
