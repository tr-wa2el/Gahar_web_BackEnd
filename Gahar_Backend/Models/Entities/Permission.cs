using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    public class Permission : BaseEntity
    {
  [Required]
    [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Module { get; set; } = string.Empty;

        [StringLength(500)]
     public string? Description { get; set; }

        // Navigation Properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
