using System.ComponentModel.DataAnnotations;

namespace Gahar_Backend.Models.Entities
{
    /// <summary>
    /// القسم / Department
    /// يمثل الأقسام المختلفة في المؤسسة
    /// </summary>
    public class Department : BaseEntity
  {
     [Required]
        [StringLength(100)]
    public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

  [StringLength(100)]
        public string? NameAr { get; set; }

    /// <summary>
 /// رئيس القسم
        /// </summary>
  public Guid? HeadId { get; set; }
        public User? Head { get; set; }

        /// <summary>
        /// إذا كان القسم نشط
        /// </summary>
  public bool IsActive { get; set; } = true;

        /// <summary>
        /// ترتيب القسم
     /// </summary>
    public int DisplayOrder { get; set; } = 0;

      /// <summary>
        /// رمز القسم (مثلاً: HR, ACCOUNTING)
      /// </summary>
      [StringLength(50)]
        public string? Code { get; set; }

        // Navigation Properties
        public ICollection<User> Users { get; set; } = new List<User>();
     public ICollection<Form> Forms { get; set; } = new List<Form>();
    }
}
