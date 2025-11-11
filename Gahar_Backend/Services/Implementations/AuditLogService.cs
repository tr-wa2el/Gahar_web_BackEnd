using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Gahar_Backend.Data;
using Gahar_Backend.Models.DTOs;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Gahar_Backend.Services.Implementations
{
  public class AuditLogService : IAuditLogService
    {
     private readonly ApplicationDbContext _context;
  private readonly IHttpContextAccessor _httpContextAccessor;

     public AuditLogService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
  {
      _context = context;
       _httpContextAccessor = httpContextAccessor;
        }

 public async Task LogAsync(int? userId, string action, string entityType, int? entityId, string? description = null, object? oldValues = null, object? newValues = null)
        {
  var httpContext = _httpContextAccessor.HttpContext;

       var auditLog = new AuditLog
    {
      UserId = userId,
    Action = action,
     EntityType = entityType,
     EntityId = entityId,
  Description = description,
   OldValues = oldValues != null ? JsonSerializer.Serialize(oldValues) : null,
      NewValues = newValues != null ? JsonSerializer.Serialize(newValues) : null,
        IpAddress = httpContext?.Connection.RemoteIpAddress?.ToString(),
   UserAgent = httpContext?.Request.Headers["User-Agent"].ToString()
       };

   await _context.AuditLogs.AddAsync(auditLog);
      await _context.SaveChangesAsync();
        }

  public async Task<IEnumerable<AuditLog>> GetLogsAsync(AuditLogFilterDto filter)
  {
     var query = _context.AuditLogs.AsQueryable();

            if (filter.UserId.HasValue)
    query = query.Where(a => a.UserId == filter.UserId.Value);

   if (!string.IsNullOrEmpty(filter.Action))
     query = query.Where(a => a.Action == filter.Action);

     if (!string.IsNullOrEmpty(filter.EntityType))
      query = query.Where(a => a.EntityType == filter.EntityType);

         if (filter.StartDate.HasValue)
    query = query.Where(a => a.Timestamp >= filter.StartDate.Value);

       if (filter.EndDate.HasValue)
      query = query.Where(a => a.Timestamp <= filter.EndDate.Value);

       return await query
       .OrderByDescending(a => a.Timestamp)
          .Skip((filter.PageNumber - 1) * filter.PageSize)
  .Take(filter.PageSize)
    .ToListAsync();
        }
    }
}
