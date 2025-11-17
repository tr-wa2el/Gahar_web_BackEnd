# 🔐 نظام الصلاحيات المتقدم - دليل شامل

## 📋 مقدمة

تم إنشاء نظام صلاحيات متقدم وشامل يطبق على **جميع الـ Entities** في التطبيق، ليس فقط على Forms و Departments.

النظام يوفر:
- ✅ التحكم بالصلاحيات حسب الأدوار (RBAC)
- ✅ التحكم بالوصول حسب البيانات (DBAC)
- ✅ التحكم بالصلاحيات حسب الأقسام
- ✅ التحكم بملكية الكيانات (Entity Ownership)
- ✅ Filters وAttributes للتحقق التلقائي من الصلاحيات

---

## 🏗️ بنية النظام

### 1. Interfaces (الواجهات)

```
IPermissionService
├─ التحقق من صلاحيات
├─ إدارة الصلاحيات
└─ الحصول على صلاحيات المستخدم

IDataAccessService
├─ التحقق من الوصول للبيانات
├─ بناء Filters
└─ التحقق من ملكية الكيانات

IDepartmentPermissionService
├─ صلاحيات الأقسام
└─ الأقسام المتاحة

IRoleBasedAccessService
├─ التحقق من الأدوار
└─ الأدوار المتاحة
```

### 2. Implementations (التنفيذات)

```
PermissionService
DataAccessService
DepartmentPermissionService
RoleBasedAccessService
```

### 3. Filters

```
RequirePermissionAttribute   → التحقق من صلاحيات
RequireDepartmentAccessAttribute  → صلاحيات الأقسام
RequireRoleAttribute              → التحقق من الأدوار
RequireEntityOwnershipAttribute   → ملكية الكيانات
```

### 4. Controllers

```
PermissionsController
├─ POST /api/permissions/check
├─ GET /api/permissions/my-permissions
├─ GET /api/permissions/my-roles
├─ POST /api/permissions/create
├─ GET /api/permissions/all-permissions
└─ ... وغيرها
```

---

## 📊 أنواع الصلاحيات

### 1. صلاحيات الأدوار (Role-Based Permissions)

**الأدوار المتاحة:**

```
┌─────────────────────────────────────────┐
│  نموذج الأدوار   │
├─────────────────┬───────────────────────┤
│ Admin           │ دخول كامل            │
│ SuperAdmin      │ دخول كامل جداً       │
│ Editor          │ تحرير محدود          │
│ Viewer        │ عرض فقط       │
│ DepartmentHead  │ إدارة القسم     │
└─────────────────┴───────────────────────┘
```

### 2. صلاحيات الأقسام (Department-Based Permissions)

```
🔐 القسم
├─ Admin/SuperAdmin → يرى جميع البيانات
├─ رئيس القسم → يدير قسمه فقط
└─ الموظف العادي → يرى بيانات قسمه فقط
```

### 3. صلاحيات الكيانات (Entity-Based Permissions)

```
📦 الكيان (Form, Content, Page, Album, إلخ)
├─ المالك → يستطيع تعديل وحذف
├─ Admin → يستطيع تعديل وحذف أي شيء
└─ الآخرون → حسب مستوى الوصول
```

### 4. مستويات الوصول (Access Levels)

```
┌──────────────────────────────────────┐
│ مستويات الوصول      │
├──────────┬──────────┬───────────────┤
│ Public   │ Internal │ Private       │
│ للجميع   │ القسم    │ المالك فقط    │
└──────────┴──────────┴───────────────┘
```

---

## 🚀 كيفية الاستخدام

### 1. استخدام Filters

#### التحقق من صلاحية معينة

```csharp
[RequirePermission("Form_Create", "Form_Edit")]
public async Task<IActionResult> CreateForm([FromBody] FormDto dto)
{
    // كود العملية
}
```

#### التحقق من دور معين

```csharp
[RequireRole("Admin", "DepartmentHead")]
public async Task<IActionResult> ManageForms()
{
    // كود الإدارة
}
```

#### التحقق من ملكية الكيان

```csharp
[RequireEntityOwnership("Form")]
public async Task<IActionResult> UpdateForm(int id, [FromBody] FormDto dto)
{
    // كود التحديث
}
```

#### التحقق من صلاحيات الأقسام

```csharp
[RequireDepartmentAccess("manage")]
public async Task<IActionResult> ManageDepartment(Guid departmentId)
{
    // كود الإدارة
}
```

### 2. استخدام الخدمات مباشرة

```csharp
[ApiController]
[Route("api/[controller]")]
public class MyController : ControllerBase
{
    private readonly IPermissionService _permissionService;
    private readonly IRoleBasedAccessService _roleBasedAccessService;

    public MyController(
  IPermissionService permissionService,
        IRoleBasedAccessService roleBasedAccessService)
    {
        _permissionService = permissionService;
      _roleBasedAccessService = roleBasedAccessService;
    }

    public async Task<IActionResult> MyAction()
    {
        var userId = User.GetUserId();

        // التحقق من الصلاحيات
        var hasPermission = await _permissionService
 .HasPermissionAsync(userId, "Form_Create");

        if (!hasPermission)
            return Forbid("ليس لديك صلاحية");

  // كود العملية
        return Ok();
    }
}
```

### 3. الحصول على البيانات مع تطبيق الصلاحيات

```csharp
var userId = User.GetUserId();
var accessFilter = await _dataAccessService.GetAccessFilterAsync<Form>(userId);

// تطبيق الـ Filter على Query
var forms = await _dbContext.Forms
    .Where(accessFilter)
 .ToListAsync();
```

---

## 📡 API Endpoints

### 1. التحقق من الصلاحيات

```http
POST /api/permissions/check
Content-Type: application/json

{
    "permissionName": "Form_Create"
}

Response:
{
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "permissionName": "Form_Create",
    "hasPermission": true
}
```

### 2. الحصول على صلاحيات المستخدم

```http
GET /api/permissions/my-permissions
Authorization: Bearer <token>

Response:
{
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "permissions": [
        "Form_Create",
        "Form_Edit",
        "Form_Delete",
        "Content_View"
    ]
}
```

### 3. الحصول على أدوار المستخدم

```http
GET /api/permissions/my-roles
Authorization: Bearer <token>

Response:
{
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "roles": ["Editor", "DepartmentHead"]
}
```

### 4. الحصول على جميع الصلاحيات (Admin فقط)

```http
GET /api/permissions/all-permissions
Authorization: Bearer <token>

Response:
[
    {
        "id": 1,
        "name": "Form_Create",
    "description": "إنشاء نموذج جديد",
        "roles": ["Admin", "Editor", "DepartmentHead"]
    },
    ...
]
```

### 5. إنشاء صلاحية جديدة (Admin فقط)

```http
POST /api/permissions/create
Authorization: Bearer <token>
Content-Type: application/json

{
    "name": "Custom_Permission",
    "description": "وصف الصلاحية"
}

Response:
{
    "message": "تم إنشاء الصلاحية بنجاح"
}
```

### 6. إضافة صلاحية لـ Role (Admin فقط)

```http
POST /api/permissions/add-to-role
Authorization: Bearer <token>
Content-Type: application/json

{
    "roleId": 1,
    "permissionName": "Form_Create"
}

Response:
{
    "message": "تمت إضافة الصلاحية بنجاح"
}
```

### 7. التحقق من الوصول للكيان

```http
POST /api/permissions/check-entity-access
Authorization: Bearer <token>
Content-Type: application/json

{
    "entityId": 1,
    "entityType": "Form",
    "action": "Edit"
}

Response:
{
    "userId": "550e8400-e29b-41d4-a716-446655440000",
 "entityId": 1,
    "entityType": "Form",
    "action": "Edit",
    "canAccess": true
}
```

### 8. الحصول على الأقسام المتاحة

```http
GET /api/permissions/accessible-departments
Authorization: Bearer <token>

Response:
{
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "departments": [
        "550e8400-e29b-41d4-a716-446655440001",
        "550e8400-e29b-41d4-a716-446655440002"
    ]
}
```

---

## 📚 الصلاحيات الافتراضية

### صلاحيات الأدوار الجديدة

```csharp
// عند إنشاء Role جديد، يجب إضافة هذه الصلاحيات:

// للـ Admin
AddPermissionToRole("Admin", "Form_Create");
AddPermissionToRole("Admin", "Form_Edit");
AddPermissionToRole("Admin", "Form_Delete");
AddPermissionToRole("Admin", "Form_View");
AddPermissionToRole("Admin", "Content_Create");
AddPermissionToRole("Admin", "Content_Edit");
AddPermissionToRole("Admin", "Content_Delete");
// إلخ...

// للـ Editor
AddPermissionToRole("Editor", "Form_Create");
AddPermissionToRole("Editor", "Form_Edit");
AddPermissionToRole("Editor", "Form_View");
AddPermissionToRole("Editor", "Content_Create");
AddPermissionToRole("Editor", "Content_Edit");
// لا يملك الحذف

// للـ Viewer
AddPermissionToRole("Viewer", "Form_View");
AddPermissionToRole("Viewer", "Content_View");
// عرض فقط
```

---

## 🔧 التخصيص والتوسع

### إضافة صلاحيات جديدة

```csharp
// في DataSeeder أو أثناء الـ Migration:

var newPermission = new Permission
{
    Name = "Report_Generate",
    Description = "توليد التقارير"
};

dbContext.Permissions.Add(newPermission);
await dbContext.SaveChangesAsync();
```

### إنشاء أدوار جديدة

```csharp
var newRole = new Role
{
    Name = "Analyst",
    Description = "محلل البيانات"
};

dbContext.Roles.Add(newRole);
await dbContext.SaveChangesAsync();

// إضافة الصلاحيات
var permission = await dbContext.Permissions
    .FirstOrDefaultAsync(p => p.Name == "Report_Generate");

var rolePermission = new RolePermission
{
    RoleId = newRole.Id,
 PermissionId = permission.Id
};

dbContext.RolePermissions.Add(rolePermission);
await dbContext.SaveChangesAsync();
```

### دعم أنواع كيانات جديدة

```csharp
// في DataAccessService.BuildAccessFilter:

if (entityType == typeof(MyNewEntity))
{
    return x => ((MyNewEntity)(object)x).CreatedBy == userId
    as Expression<Func<T, bool>>;
}

// في PermissionService.IsEntityOwnerAsync:

case "mynewentity":
    var entity = await _dbContext.Set<MyNewEntity>()
        .FirstOrDefaultAsync(e => e.Id == entityId);
    return entity?.CreatedBy == userId;
```

---

## 🔒 أمثلة الاستخدام في Controllers

### مثال 1: إنشاء Form مع التحقق من الصلاحيات

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FormsController : ControllerBase
{
    private readonly IFormService _formService;
    private readonly IPermissionService _permissionService;

    [HttpPost]
    [RequirePermission("Form_Create")]
    public async Task<IActionResult> CreateForm([FromBody] FormDto dto)
    {
        var userId = User.GetUserId();

        // إنشاء النموذج
        var form = new Form
        {
    // ...
          CreatedBy = userId,
            DepartmentId = User.GetDepartmentId()
      };

        // حفظ الكود...
      return CreatedAtAction(nameof(GetForm), form);
    }

    [HttpGet("{id}")]
    [RequireEntityOwnership("Form", "id")]
    public async Task<IActionResult> GetForm(int id)
  {
        // الكود...
    }

    [HttpPut("{id}")]
    [RequireEntityOwnership("Form", "id")]
    public async Task<IActionResult> UpdateForm(int id, [FromBody] FormDto dto)
    {
      // الكود...
    }

    [HttpDelete("{id}")]
    [RequireEntityOwnership("Form", "id")]
    public async Task<IActionResult> DeleteForm(int id)
    {
        // الكود...
    }
}
```

### مثال 2: إدارة الأقسام

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentPermissionService _departmentPermissionService;

[HttpGet("{departmentId}/data")]
    [RequireDepartmentAccess("view")]
    public async Task<IActionResult> GetDepartmentData(Guid departmentId)
    {
        // الكود...
    }

    [HttpPut("{departmentId}")]
    [RequireDepartmentAccess("manage")]
    public async Task<IActionResult> UpdateDepartment(Guid departmentId)
    {
        // الكود...
    }
}
```

### مثال 3: عرض البيانات مع تطبيق الصلاحيات

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContentsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
 private readonly IDataAccessService _dataAccessService;

    [HttpGet]
    public async Task<IActionResult> GetContents()
    {
        var userId = User.GetUserId();

        // الحصول على الـ Filter
        var accessFilter = await _dataAccessService
      .GetAccessFilterAsync<Content>(userId);

        // تطبيق الـ Filter
   var contents = await _dbContext.Contents
     .Where(accessFilter)
            .ToListAsync();

        return Ok(contents);
    }
}
```

---

## 📈 جدول الصلاحيات الكامل

```
┌────────────────────┬──────┬────────┬────────┬─────────────┐
│ الصلاحية           │Admin │Editor  │Viewer  │DepartmentHead
├────────────────────┼──────┼────────┼────────┼─────────────┤
│ Form_Create      │  ✅  │  ✅    │  ❌    │  ✅        │
│ Form_Edit          │  ✅  │  ✅    │  ❌    │  ✅ │
│ Form_Delete        │  ✅  │  ❌    │  ❌    │  ✅   │
│ Form_View        │  ✅  │  ✅    │  ✅    │  ✅        │
│ Content_Create     │  ✅  │  ✅    │  ❌    │  ❌        │
│ Content_Edit       │  ✅  │  ✅    │  ❌    │  ❌     │
│ Content_Delete     │  ✅  │  ❌  │  ❌    │  ❌     │
│ Content_View       │  ✅  │  ✅    │  ✅    │  ✅        │
│ Page_Create     │  ✅  │  ✅    │  ❌    │  ❌        │
│ Page_Edit        │  ✅  │  ✅    │  ❌    │  ❌        │
│ Page_Delete│  ✅  │  ❌    │  ❌    │  ❌        │
│ Page_View          │  ✅  │  ✅    │  ✅    │  ✅        │
│ Dept_Manage        │  ✅  │  ❌    │  ❌    │  ✅│
│ Dept_View       │  ✅  │  ❌    │  ❌    │  ✅ │
└────────────────────┴──────┴────────┴────────┴─────────────┘
```

---

## 🧪 اختبار الصلاحيات

### 1. اختبار في Postman

```
1. سجل الدخول وأحصل على Token
2. أضف Token إلى Authorization header
3. اختبر Endpoints المختلفة

GET /api/permissions/my-permissions
POST /api/permissions/check
POST /api/permissions/check-entity-access
```

### 2. اختبار في Swagger

```
1. افتح http://localhost:5000/swagger
2. اضغط "Authorize" وأدخل Token
3. اختبر جميع Endpoints
```

---

## 📝 الخطوات التالية

### 1. تحديث Controllers الموجودة

أضف الـ Filters المناسبة على جميع Controllers:

```csharp
[RequirePermission("Entity_Create")]
public async Task<IActionResult> CreateEntity()

[RequirePermission("Entity_Edit")]
public async Task<IActionResult> UpdateEntity(int id)

[RequirePermission("Entity_Delete")]
public async Task<IActionResult> DeleteEntity(int id)

[RequirePermission("Entity_View")]
public async Task<IActionResult> GetEntity(int id)
```

### 2. إضافة بيانات اختبارية

```csharp
// في DataSeeder:
await SeedPermissionsAsync(dbContext);
await SeedRolePermissionsAsync(dbContext);
```

### 3. توثيق الصلاحيات

توثق جميع الصلاحيات المتاحة في ملف منفصل.

### 4. اختبار شامل

اختبر جميع السيناريوهات والحالات الحدية.

---

## ✅ Checklist

- [ ] تشغيل Migration
- [ ] اختبار Endpoints
- [ ] إضافة Filters للـ Controllers
- [ ] تحديث بيانات الـ Seed
- [ ] توثيق الصلاحيات الجديدة
- [ ] اختبار شامل
- [ ] تدريب الفريق
- [ ] الإطلاق! 🚀

---

## 💬 الدعم والأسئلة

هل تحتاج أي توضيح أو تعديل على النظام؟

أنا هنا للمساعدة! 💪
