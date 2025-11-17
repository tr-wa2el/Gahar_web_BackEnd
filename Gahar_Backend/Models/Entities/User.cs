using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(100)]
    public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [EmailAddress]
      public string Email { get; set; } = string.Empty;

        [Required]
      public string PasswordHash { get; set; } = string.Empty;

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public bool EmailConfirmed { get; set; } = false;

        public DateTime? LastLoginAt { get; set; }

        public int FailedLoginAttempts { get; set; } = 0;

  public DateTime? LockedUntil { get; set; }

        /// <summary>
     /// القسم التابع له المستخدم
        /// </summary>
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Navigation Properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
     public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}
