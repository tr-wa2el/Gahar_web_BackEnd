using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gahar_Backend.Services
{
    /// <summary>
    /// خدمة التحكم في صلاحيات الأقسام
    /// يتحكم بمن يمكنه رؤية البيانات حسب القسم
    /// </summary>
    public interface IDepartmentAccessService
    {
     /// <summary>
      /// التحقق من أن المستخدم ينتمي لقسم معين
        /// </summary>
      Task<bool> IsUserInDepartmentAsync(Guid userId, Guid departmentId);

        /// <summary>
        /// الحصول على قسم المستخدم
        /// </summary>
        Task<Department?> GetUserDepartmentAsync(Guid userId);

        /// <summary>
      /// التحقق من أن المستخدم يمكنه الوصول لنموذج معين
        /// </summary>
        Task<bool> CanUserAccessFormAsync(Guid userId, Guid formId);

        /// <summary>
    /// الحصول على جميع النماذج في قسم المستخدم
        /// </summary>
        Task<List<Form>> GetDepartmentFormsAsync(Guid userId);

     /// <summary>
        /// التحقق من أن المستخدم هو رئيس القسم
        /// </summary>
    Task<bool> IsUserDepartmentHeadAsync(Guid userId);

        /// <summary>
     /// الحصول على جميع الموظفين في قسم المستخدم
      /// </summary>
        Task<List<User>> GetDepartmentEmployeesAsync(Guid userId);

        /// <summary>
        /// التحقق من أن المستخدم يمكنه تعديل نموذج معين
      /// </summary>
        Task<bool> CanUserEditFormAsync(Guid userId, Guid formId);
    }

    public class DepartmentAccessService : IDepartmentAccessService
{
     private readonly ApplicationDbContext _context;

  public DepartmentAccessService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// التحقق من أن المستخدم ينتمي لقسم معين
      /// </summary>
        public async Task<bool> IsUserInDepartmentAsync(Guid userId, Guid departmentId)
        {
   var user = await _context.Users
 .FirstOrDefaultAsync(u => u.Id.ToString() == userId.ToString());

     return user?.DepartmentId.ToString() == departmentId.ToString();
        }

        /// <summary>
        /// الحصول على قسم المستخدم
        /// </summary>
        public async Task<Department?> GetUserDepartmentAsync(Guid userId)
   {
 var user = await _context.Users
          .Include(u => u.Department)
  .FirstOrDefaultAsync(u => u.Id.ToString() == userId.ToString());

   return user?.Department;
   }

      /// <summary>
        /// التحقق من أن المستخدم يمكنه الوصول لنموذج معين
  /// فقط إذا كان في نفس القسم أو كان Admin
        /// </summary>
        public async Task<bool> CanUserAccessFormAsync(Guid userId, Guid formId)
    {
    var form = await _context.Forms
  .FirstOrDefaultAsync(f => f.Id.ToString() == formId.ToString());

    if (form == null)
       return false;

   var user = await _context.Users
  .Include(u => u.UserRoles)
           .ThenInclude(ur => ur.Role)
      .FirstOrDefaultAsync(u => u.Id.ToString() == userId.ToString());

            if (user == null)
      return false;

 // Admin و SuperAdmin يمكنهم الوصول لكل شيء
     var isAdmin = user.UserRoles.Any(ur =>
     ur.Role?.Name == "Admin" || ur.Role?.Name == "SuperAdmin");

         if (isAdmin)
    return true;

            // المستخدمين العاديين يمكنهم فقط الوصول لنماذج قسمهم
       return user.DepartmentId.ToString() == form.DepartmentId.ToString();
    }

        /// <summary>
        /// الحصول على جميع النماذج في قسم المستخدم
     /// </summary>
        public async Task<List<Form>> GetDepartmentFormsAsync(Guid userId)
 {
    var user = await _context.Users
           .FirstOrDefaultAsync(u => u.Id.ToString() == userId.ToString());

       if (user == null)
       return new List<Form>();

       // Admin يشوف كل النماذج
var isAdmin = await _context.UserRoles
       .Include(ur => ur.Role)
            .Where(ur => ur.UserId.ToString() == userId.ToString())
   .AnyAsync(ur => ur.Role!.Name == "Admin" || ur.Role!.Name == "SuperAdmin");

    if (isAdmin)
       {
return await _context.Forms
       .Include(f => f.Department)
       .Include(f => f.Author)
.Where(f => f.IsActive)
      .OrderByDescending(f => f.CreatedAt)
  .ToListAsync();
    }

    // المستخدمين العاديين يشوفوا فقط نماذج قسمهم
      return await _context.Forms
    .Include(f => f.Department)
      .Include(f => f.Author)
     .Where(f => f.DepartmentId.ToString() == user.DepartmentId.ToString() && f.IsActive)
    .OrderByDescending(f => f.CreatedAt)
   .ToListAsync();
        }

  /// <summary>
   /// التحقق من أن المستخدم هو رئيس القسم
        /// </summary>
  public async Task<bool> IsUserDepartmentHeadAsync(Guid userId)
 {
            var department = await _context.Departments
     .FirstOrDefaultAsync(d => d.HeadId.ToString() == userId.ToString());

return department != null;
   }

        /// <summary>
        /// الحصول على جميع الموظفين في قسم المستخدم
     /// </summary>
     public async Task<List<User>> GetDepartmentEmployeesAsync(Guid userId)
 {
       var user = await _context.Users
 .FirstOrDefaultAsync(u => u.Id.ToString() == userId.ToString());

   if (user == null || user.DepartmentId == null)
     return new List<User>();

      return await _context.Users
         .Where(u => u.DepartmentId.ToString() == user.DepartmentId.ToString() && u.IsActive)
         .OrderBy(u => u.FirstName)
        .ToListAsync();
   }

      /// <summary>
/// التحقق من أن المستخدم يمكنه تعديل نموذج معين
        /// Admin و رئيس القسم و مانشئ النموذج يمكنهم التعديل
        /// </summary>
        public async Task<bool> CanUserEditFormAsync(Guid userId, Guid formId)
        {
  var form = await _context.Forms
  .FirstOrDefaultAsync(f => f.Id.ToString() == formId.ToString());

 if (form == null)
  return false;

  var user = await _context.Users
    .Include(u => u.UserRoles)
    .ThenInclude(ur => ur.Role)
     .FirstOrDefaultAsync(u => u.Id.ToString() == userId.ToString());

       if (user == null)
     return false;

  // Admin و SuperAdmin يمكنهم تعديل أي نموذج
       var isAdmin = user.UserRoles.Any(ur =>
    ur.Role?.Name == "Admin" || ur.Role?.Name == "SuperAdmin");

       if (isAdmin)
              return true;

 // من أنشأ النموذج يمكنه تعديله
     if (form.AuthorId == userId.GetHashCode())
      return true;

        // رئيس القسم يمكنه تعديل نماذج قسمه
   var isDepartmentHead = await IsUserDepartmentHeadAsync(userId);
    if (isDepartmentHead && form.DepartmentId.ToString() == user.DepartmentId.ToString())
  return true;

   return false;
     }
    }
}
