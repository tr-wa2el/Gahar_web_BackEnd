namespace Gahar_Backend.Models.DTOs
{
    public class AuditLogFilterDto
    {
        public int? UserId { get; set; }
    public string? Action { get; set; }
        public string? EntityType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
