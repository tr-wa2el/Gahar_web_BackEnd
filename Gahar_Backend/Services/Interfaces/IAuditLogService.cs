using Gahar_Backend.Models.DTOs;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Services.Interfaces
{
    public interface IAuditLogService
    {
        Task LogAsync(int? userId, string action, string entityType, int? entityId, string? description = null, object? oldValues = null, object? newValues = null);
      Task<IEnumerable<AuditLog>> GetLogsAsync(AuditLogFilterDto filter);
    }
}
