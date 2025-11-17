using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gahar_Backend.Controllers
{
    /// <summary>
    /// API للتحكم بالأقسام
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
    /// الحصول على جميع الأقسام
    /// فقط Admin يمكنه رؤية جميع الأقسام
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
      {
 var departments = await _context.Departments
   .Where(d => d.IsActive)
   .OrderBy(d => d.DisplayOrder)
      .Select(d => new DepartmentDto
     {
   Id = d.Id,
    Name = d.Name,
       NameAr = d.NameAr,
   Description = d.Description,
      Code = d.Code,
    UsersCount = d.Users.Count
    })
      .ToListAsync();

   return Ok(departments);
  }

     /// <summary>
        /// الحصول على بيانات قسم معين
  /// </summary>
        [HttpGet("{id}")]
   public async Task<ActionResult<DepartmentDetailDto>> GetDepartment(Guid id)
        {
       var department = await _context.Departments
       .Include(d => d.Head)
          .Include(d => d.Users)
           .FirstOrDefaultAsync(d => d.Id.ToString() == id.ToString() && d.IsActive);

        if (department == null)
     return NotFound(new { message = "القسم غير موجود" });

            return Ok(new DepartmentDetailDto
    {
             Id = department.Id,
    Name = department.Name,
           NameAr = department.NameAr,
                Description = department.Description,
      Code = department.Code,
     HeadName = department.Head?.FirstName + " " + department.Head?.LastName,
           UsersCount = department.Users.Count
    });
      }

    /// <summary>
      /// إنشاء قسم جديد
        /// فقط Admin و SuperAdmin
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] CreateDepartmentDto createDto)
        {
if (!ModelState.IsValid)
     return BadRequest(ModelState);

      var department = new Department
            {
         Name = createDto.Name,
    NameAr = createDto.NameAr,
   Description = createDto.Description,
   Code = createDto.Code,
     IsActive = true,
          DisplayOrder = createDto.DisplayOrder ?? 0
            };

        _context.Departments.Add(department);
  await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, new DepartmentDto
  {
           Id = department.Id,
   Name = department.Name,
       NameAr = department.NameAr,
        Description = department.Description,
      Code = department.Code
  });
    }

        /// <summary>
        /// تحديث قسم
  /// </summary>
     [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> UpdateDepartment(Guid id, [FromBody] UpdateDepartmentDto updateDto)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
      return NotFound(new { message = "القسم غير موجود" });

    department.Name = updateDto.Name ?? department.Name;
            department.NameAr = updateDto.NameAr ?? department.NameAr;
            department.Description = updateDto.Description ?? department.Description;
  department.Code = updateDto.Code ?? department.Code;

     if (updateDto.DisplayOrder.HasValue)
           department.DisplayOrder = updateDto.DisplayOrder.Value;

          _context.Departments.Update(department);
            await _context.SaveChangesAsync();

         return NoContent();
        }

        /// <summary>
  /// الحصول على جميع موظفي القسم
    /// </summary>
    [HttpGet("{id}/employees")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetDepartmentEmployees(Guid id)
        {
    var employees = await _context.Users
      .Where(u => u.DepartmentId == id && u.IsActive)
   .Select(u => new EmployeeDto
   {
         Id = u.Id,
    FirstName = u.FirstName,
                 LastName = u.LastName,
                Email = u.Email,
         PhoneNumber = u.PhoneNumber
        })
           .OrderBy(e => e.FirstName)
        .ToListAsync();

        return Ok(employees);
        }

        /// <summary>
        /// تعيين رئيس للقسم
        /// </summary>
        [HttpPost("{id}/set-head/{userId}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
      public async Task<IActionResult> SetDepartmentHead(Guid id, Guid userId)
   {
       var department = await _context.Departments.FindAsync(id);
 if (department == null)
          return NotFound(new { message = "القسم غير موجود" });

  var user = await _context.Users.FindAsync(userId);
   if (user == null)
      return NotFound(new { message = "المستخدم غير موجود" });

       department.HeadId = userId;
            _context.Departments.Update(department);
    await _context.SaveChangesAsync();

            return Ok(new { message = "تم تعيين رئيس القسم بنجاح" });
        }
  }

// DTOs
    public class DepartmentDto
    {
public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
public string? NameAr { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public int UsersCount { get; set; }
    }

    public class DepartmentDetailDto : DepartmentDto
    {
 public string? HeadName { get; set; }
    }

    public class CreateDepartmentDto
    {
        public string Name { get; set; } = string.Empty;
 public string? NameAr { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public int? DisplayOrder { get; set; }
 }

    public class UpdateDepartmentDto
  {
        public string? Name { get; set; }
  public string? NameAr { get; set; }
   public string? Description { get; set; }
   public string? Code { get; set; }
  public int? DisplayOrder { get; set; }
  }

    public class EmployeeDto
  {
        public int Id { get; set; }
  public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}
