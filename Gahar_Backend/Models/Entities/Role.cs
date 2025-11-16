using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    public class Role : BaseEntity
    {
     [Required]
     [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

     public bool IsSystemRole { get; set; } = false;

 // Navigation Properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
