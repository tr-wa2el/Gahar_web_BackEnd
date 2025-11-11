using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    public class AuditLog : BaseEntity
    {
 public int? UserId { get; set; }
        public User? User { get; set; }

    [Required]
        [StringLength(50)]
        public string Action { get; set; } = string.Empty;

        [Required]
      [StringLength(100)]
      public string EntityType { get; set; } = string.Empty;

        public int? EntityId { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        [StringLength(45)]
        public string? IpAddress { get; set; }

        [StringLength(500)]
        public string? UserAgent { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
