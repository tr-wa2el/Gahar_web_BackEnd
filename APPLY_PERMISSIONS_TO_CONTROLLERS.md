# 🎯 دليل تطبيق الصلاحيات على Controllers الموجودة

## 📋 مقدمة

هذا الدليل يشرح كيفية إضافة الـ Filters للصلاحيات على جميع Controllers الموجودة في التطبيق.

---

## 🔧 خطوات الإضافة

### الخطوة 1: إضافة Using

```csharp
using Gahar_Backend.Filters;
```

### الخطوة 2: إضافة Attributes

اختر الـ Attribute المناسب حسب الحالة:

```csharp
// للصلاحيات العامة
[RequirePermission("Form_Create", "Form_Edit")]

// للأدوار
[RequireRole("Admin", "Editor")]

// لملكية الكيان
[RequireEntityOwnership("Form")]

// للأقسام
[RequireDepartmentAccess("view")]
```

---

## 📝 أمثلة على Controllers

### FormsController

```csharp
using Gahar_Backend.Filters;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FormsController : ControllerBase
{
    [HttpPost]
    [RequirePermission("Form_Create")]
    public async Task<IActionResult> CreateForm([FromBody] FormDto dto)
    {
 // الكود...
  }

    [HttpGet("{id}")]
    [RequirePermission("Form_View")]
    public async Task<IActionResult> GetForm(int id)
    {
      // الكود...
    }

    [HttpPut("{id}")]
    [RequireEntityOwnership("Form")]
    [RequirePermission("Form_Edit")]
    public async Task<IActionResult> UpdateForm(int id, [FromBody] FormDto dto)
    {
  // الكود...
    }

    [HttpDelete("{id}")]
    [RequireEntityOwnership("Form")]
    [RequirePermission("Form_Delete")]
    public async Task<IActionResult> DeleteForm(int id)
    {
        // الكود...
    }

    [HttpGet]
    [RequirePermission("Form_View")]
    public async Task<IActionResult> GetForms()
    {
        // الكود...
    }
}
```

### PagesController

```csharp
using Gahar_Backend.Filters;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PagesController : ControllerBase
{
[HttpPost]
    [RequirePermission("Page_Create")]
    public async Task<IActionResult> CreatePage([FromBody] PageDto dto)
  {
  // الكود...
    }

    [HttpGet("{id}")]
    [RequirePermission("Page_View")]
    public async Task<IActionResult> GetPage(int id)
    {
   // الكود...
    }

    [HttpPut("{id}")]
    [RequireEntityOwnership("Page")]
    [RequirePermission("Page_Edit")]
    public async Task<IActionResult> UpdatePage(int id, [FromBody] PageDto dto)
    {
        // الكود...
    }

    [HttpDelete("{id}")]
    [RequireRole("Admin")]
    [RequirePermission("Page_Delete")]
    public async Task<IActionResult> DeletePage(int id)
    {
        // الكود...
    }

  [HttpGet]
    [RequirePermission("Page_View")]
    public async Task<IActionResult> GetPages()
    {
        // الكود...
    }
}
```

### ContentsController

```csharp
using Gahar_Backend.Filters;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContentsController : ControllerBase
{
    [HttpPost]
    [RequirePermission("Content_Create")]
    public async Task<IActionResult> CreateContent([FromBody] ContentDto dto)
    {
        // الكود...
    }

    [HttpGet("{id}")]
    [RequirePermission("Content_View")]
    public async Task<IActionResult> GetContent(int id)
    {
        // الكود...
    }

    [HttpPut("{id}")]
    [RequireEntityOwnership("Content")]
    [RequirePermission("Content_Edit")]
    public async Task<IActionResult> UpdateContent(int id, [FromBody] ContentDto dto)
    {
     // الكود...
    }

    [HttpDelete("{id}")]
  [RequireRole("Admin")]
    [RequirePermission("Content_Delete")]
    public async Task<IActionResult> DeleteContent(int id)
    {
        // الكود...
    }

    [HttpGet]
    [RequirePermission("Content_View")]
    public async Task<IActionResult> GetContents()
  {
        // الكود...
    }
}
```

### AlbumsController

```csharp
using Gahar_Backend.Filters;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlbumsController : ControllerBase
{
[HttpPost]
  [RequirePermission("Album_Create")]
    public async Task<IActionResult> CreateAlbum([FromBody] AlbumDto dto)
    {
        // الكود...
    }

    [HttpGet("{id}")]
 [RequirePermission("Album_View")]
    public async Task<IActionResult> GetAlbum(int id)
    {
   // الكود...
    }

    [HttpPut("{id}")]
    [RequireEntityOwnership("Album")]
    [RequirePermission("Album_Edit")]
    public async Task<IActionResult> UpdateAlbum(int id, [FromBody] AlbumDto dto)
    {
        // الكود...
    }

    [HttpDelete("{id}")]
    [RequireEntityOwnership("Album")]
    [RequirePermission("Album_Delete")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
      // الكود...
    }

    [HttpGet]
    [RequirePermission("Album_View")]
    public async Task<IActionResult> GetAlbums()
    {
   // الكود...
    }
}
```

### DepartmentsController

```csharp
using Gahar_Backend.Filters;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentsController : ControllerBase
{
    [HttpPost]
    [RequireRole("Admin")]
    public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto dto)
    {
        // الكود...
    }

    [HttpGet("{departmentId}")]
    [RequireDepartmentAccess("view")]
    public async Task<IActionResult> GetDepartment(Guid departmentId)
    {
        // الكود...
    }

    [HttpPut("{departmentId}")]
    [RequireDepartmentAccess("manage")]
    public async Task<IActionResult> UpdateDepartment(Guid departmentId, [FromBody] DepartmentDto dto)
    {
        // الكود...
    }

    [HttpDelete("{departmentId}")]
    [RequireRole("Admin")]
    public async Task<IActionResult> DeleteDepartment(Guid departmentId)
    {
        // الكود...
    }

    [HttpGet]
 [RequireRole("Admin")]
    public async Task<IActionResult> GetDepartments()
    {
        // الكود...
    }
}
```

### MenusController

```csharp
using Gahar_Backend.Filters;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MenusController : ControllerBase
{
    [HttpPost]
    [RequireRole("Admin")]
    public async Task<IActionResult> CreateMenu([FromBody] MenuDto dto)
    {
        // الكود...
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMenu(int id)
    {
        // الكود...
    }

    [HttpPut("{id}")]
    [RequireRole("Admin")]
    public async Task<IActionResult> UpdateMenu(int id, [FromBody] MenuDto dto)
    {
        // الكود...
    }

    [HttpDelete("{id}")]
    [RequireRole("Admin")]
    public async Task<IActionResult> DeleteMenu(int id)
    {
        // الكود...
    }

    [HttpGet]
    public async Task<IActionResult> GetMenus()
    {
     // الكود...
    }
}
```

### FacilitiesController

```csharp
using Gahar_Backend.Filters;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FacilitiesController : ControllerBase
{
  [HttpPost]
    [RequirePermission("Facility_Create")]
    public async Task<IActionResult> CreateFacility([FromBody] FacilityDto dto)
    {
        // الكود...
    }

    [HttpGet("{id}")]
    [RequirePermission("Facility_View")]
    public async Task<IActionResult> GetFacility(int id)
 {
        // الكود...
    }

    [HttpPut("{id}")]
    [RequirePermission("Facility_Edit")]
    public async Task<IActionResult> UpdateFacility(int id, [FromBody] FacilityDto dto)
  {
        // الكود...
    }

    [HttpDelete("{id}")]
    [RequireRole("Admin")]
    public async Task<IActionResult> DeleteFacility(int id)
    {
 // الكود...
    }

    [HttpGet]
[RequirePermission("Facility_View")]
    public async Task<IActionResult> GetFacilities()
    {
        // الكود...
    }
}
```

### CertificatesController

```csharp
using Gahar_Backend.Filters;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CertificatesController : ControllerBase
{
    [HttpPost]
    [RequirePermission("Certificate_Create")]
    public async Task<IActionResult> CreateCertificate([FromBody] CertificateDto dto)
    {
        // الكود...
    }

    [HttpGet("{id}")]
    [RequirePermission("Certificate_View")]
    public async Task<IActionResult> GetCertificate(int id)
    {
        // الكود...
    }

    [HttpPut("{id}")]
    [RequirePermission("Certificate_Edit")]
    public async Task<IActionResult> UpdateCertificate(int id, [FromBody] CertificateDto dto)
    {
        // الكود...
    }

 [HttpDelete("{id}")]
  [RequireRole("Admin")]
    public async Task<IActionResult> DeleteCertificate(int id)
    {
    // الكود...
  }

    [HttpGet]
    [RequirePermission("Certificate_View")]
    public async Task<IActionResult> GetCertificates()
    {
        // الكود...
    }
}
```

---

## 🔑 جدول الصلاحيات للـ Attributes

```
┌──────────────────┬────────────────────────┐
│ Controller       │ الـ Attributes المطلوبة │
├──────────────────┼────────────────────────┤
│ Forms            │ Form_Create/Edit/Delete│
│ Pages   │ Page_Create/Edit/Delete│
│ Contents   │ Content_Create/Edit/Del│
│ Albums           │ Album_Create/Edit/Del  │
│ Departments      │ RequireRole("Admin")   │
│ Menus            │ RequireRole("Admin")   │
│ Facilities       │ Facility_Create/Edit   │
│ Certificates     │ Certificate_Create/Edit│
│ Permissions      │ RequireRole("Admin")   │
└──────────────────┴────────────────────────┘
```

---

## 💡 ملاحظات مهمة

### 1. العملية الإنشاء (Create)

```csharp
[HttpPost]
[RequirePermission("Entity_Create")]
// إذا كانت حساسة جداً:
// [RequireRole("Admin", "Editor")]
public async Task<IActionResult> Create([FromBody] Dto dto)
{
    // الكود...
}
```

### 2. العملية التعديل (Edit)

```csharp
[HttpPut("{id}")]
[RequireEntityOwnership("Entity")]  // تحقق من الملكية أولاً
[RequirePermission("Entity_Edit")]   // ثم الصلاحية
public async Task<IActionResult> Update(int id, [FromBody] Dto dto)
{
    // الكود...
}
```

### 3. العملية الحذف (Delete)

```csharp
[HttpDelete("{id}")]
[RequirePermission("Entity_Delete")]
[RequireRole("Admin", "DepartmentHead")]  // اختياري
public async Task<IActionResult> Delete(int id)
{
    // الكود...
}
```

### 4. العملية العرض (View)

```csharp
[HttpGet]
[RequirePermission("Entity_View")]
public async Task<IActionResult> GetAll()
{
    // الكود...
}
```

---

## ⚙️ الترتيب المثالي للـ Attributes

```csharp
[HttpPost]      // 1. HTTP Method
[Authorize]      // 2. Authentication
[RequireRole("Admin")]            // 3. Role Check
[RequirePermission("Form_Create")]  // 4. Permission Check
[RequireEntityOwnership("Form")]        // 5. Ownership Check
public async Task<IActionResult> Method()
{
    // الكود...
}
```

---

## 📋 Checklist

- [ ] إضافة Using في كل Controller
- [ ] إضافة RequirePermission للـ Create
- [ ] إضافة RequirePermission للـ Edit
- [ ] إضافة RequirePermission للـ Delete
- [ ] إضافة RequireEntityOwnership حيث مناسب
- [ ] إضافة RequireRole للعمليات الحساسة
- [ ] اختبار جميع السيناريوهات
- [ ] توثيق الصلاحيات الجديدة

---

## 🧪 الاختبار

بعد إضافة الـ Attributes، اختبر:

```bash
# 1. بدون صلاحية
GET /api/forms  # → 403 Forbidden

# 2. مع صلاحية
GET /api/forms?Authorization: Bearer <token>  # → 200 OK

# 3. ملكية
PUT /api/forms/123  # (ليس صاحبه) → 403 Forbidden
PUT /api/forms/123  # (هو الصاحب) → 200 OK
```

---

**الآن أنت جاهز لتطبيق الصلاحيات!** ✅
