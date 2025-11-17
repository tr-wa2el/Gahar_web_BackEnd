# 🚀 البدء السريع - نظام الصلاحيات المتقدم

## ⚡ الخطوات الفورية

### الخطوة 1: تشغيل Migration

```bash
cd "F:\Web Gahar\bk\Gahar_web_BackEnd"
dotnet ef migrations add AddAdvancedPermissionsSystem
dotnet ef database update
```

### الخطوة 2: تشغيل التطبيق

```bash
dotnet run
```

### الخطوة 3: اختبر في Swagger

```
http://localhost:5000/swagger
```

---

## 📦 الملفات الجديدة

```
✅ Services/Interfaces/IPermissionService.cs
   └─ 4 واجهات للصلاحيات

✅ Services/Implementations/
   ├─ PermissionService.cs
   ├─ DataAccessService.cs
   ├─ DepartmentPermissionService.cs
   └─ RoleBasedAccessService.cs

✅ Filters/AccessControlFilters.cs
   └─ 4 Filters للتحقق التلقائي

✅ Controllers/PermissionsController.cs
   └─ 8 Endpoints للتحكم بالصلاحيات

✅ Program.cs
   └─ تسجيل الخدمات
```

---

## 🎯 الفكرة الأساسية

```
          ┌─────────────┐
   │  المستخدم    │
         └────────┬────┘
              │
                ┌────────────┼────────────┐
    │      │            │
    ▼     ▼            ▼
          ┌─────────┐ ┌──────────┐ ┌───────┐
       │ Role │ │Department│ │ Owner │
 │ Based   │ │  Based   │ │ Based │
         └─────────┘ └──────────┘ └───────┘
       │       │         │
   └────────────┼────────────┘
   │
         ┌────────▼─────────┐
             │ إذن الوصول؟      │
 └────────┬─────────┘
      Yes   │   No
                 ✅    │    ❌
```

---

## 💡 أمثلة عملية

### مثال 1: إنشاء Form

```csharp
[HttpPost("forms")]
[RequirePermission("Form_Create")]
public async Task<IActionResult> CreateForm([FromBody] FormDto dto)
{
    var userId = User.GetUserId();
    
    var form = new Form
    {
     Title = dto.Title,
        CreatedBy = userId,
        DepartmentId = User.GetDepartmentId()
    };
    
    _dbContext.Forms.Add(form);
    await _dbContext.SaveChangesAsync();
    
    return CreatedAtAction(nameof(GetForm), new { id = form.Id }, form);
}
```

**ما يحدث:**
1. Filter يتحقق: هل المستخدم له صلاحية "Form_Create"؟
2. إذا لا → ترجع 403 Forbidden
3. إذا نعم → تستمر في العملية

### مثال 2: تعديل Form

```csharp
[HttpPut("forms/{id}")]
[RequireEntityOwnership("Form")]
public async Task<IActionResult> UpdateForm(int id, [FromBody] FormDto dto)
{
    var userId = User.GetUserId();
 
    var form = await _dbContext.Forms.FindAsync(id);
    
 if (form?.CreatedBy != userId)
        return Forbid("ليس صاحب هذا النموذج");
    
    form.Title = dto.Title;
    await _dbContext.SaveChangesAsync();
    
    return Ok(form);
}
```

**ما يحدث:**
1. Filter يتحقق: هل أنت مالك هذا النموذج؟
2. إذا لا → ترجع 403 Forbidden
3. إذا نعم → تستمر

### مثال 3: عرض البيانات

```csharp
[HttpGet("forms")]
[Authorize]
public async Task<IActionResult> GetForms()
{
    var userId = User.GetUserId();
    
    // إذا كان Admin → يشوف كل البيانات
    var isAdmin = await _roleBasedAccessService.IsAdminAsync(userId);
    
    var forms = isAdmin 
        ? await _dbContext.Forms.ToListAsync()
     : await _dbContext.Forms
 .Where(f => f.DepartmentId == User.GetDepartmentId())
       .ToListAsync();
    
    return Ok(forms);
}
```

**ما يحدث:**
- Admin → يشوف جميع Forms
- الموظف العادي → يشوف Forms قسمه فقط

---

## 🔐 الصلاحيات الافتراضية

### الـ Admin (كل شيء)
```json
{
  "permissions": [
    "Form_Create", "Form_Edit", "Form_Delete", "Form_View",
    "Content_Create", "Content_Edit", "Content_Delete", "Content_View",
    "Page_Create", "Page_Edit", "Page_Delete", "Page_View",
    "Album_Create", "Album_Edit", "Album_Delete", "Album_View",
    "Dept_Manage", "Dept_View"
  ]
}
```

### الـ Editor (تحرير محدود)
```json
{
  "permissions": [
    "Form_Create", "Form_Edit", "Form_View",
  "Content_Create", "Content_Edit", "Content_View",
    "Page_Create", "Page_Edit", "Page_View"
  ]
}
```

### الـ DepartmentHead (إدارة القسم)
```json
{
  "permissions": [
    "Form_Create", "Form_Edit", "Form_View",
    "Content_Create", "Content_Edit", "Content_View",
    "Dept_Manage", "Dept_View"
  ]
}
```

### الـ Viewer (عرض فقط)
```json
{
  "permissions": [
    "Form_View",
    "Content_View",
    "Page_View"
  ]
}
```

---

## 📡 الـ API التي تحتاجها

### للمستخدم العادي:

```bash
# 1. تسجيل الدخول
POST /api/auth/login
{
  "email": "user@example.com",
  "password": "password"
}

# 2. الحصول على صلاحياتك
GET /api/permissions/my-permissions
Authorization: Bearer <token>

# 3. الحصول على أدوارك
GET /api/permissions/my-roles
Authorization: Bearer <token>

# 4. الحصول على الأقسام المتاحة لك
GET /api/permissions/accessible-departments
Authorization: Bearer <token>
```

### للـ Admin:

```bash
# 1. الحصول على جميع الصلاحيات
GET /api/permissions/all-permissions
Authorization: Bearer <admin-token>

# 2. إنشاء صلاحية جديدة
POST /api/permissions/create
Authorization: Bearer <admin-token>
{
  "name": "Report_Generate",
  "description": "توليد التقارير"
}

# 3. إضافة صلاحية لـ Role
POST /api/permissions/add-to-role
Authorization: Bearer <admin-token>
{
  "roleId": 2,
  "permissionName": "Report_Generate"
}

# 4. إزالة صلاحية من Role
DELETE /api/permissions/remove-from-role
Authorization: Bearer <admin-token>
{
  "roleId": 2,
  "permissionName": "Report_Generate"
}
```

---

## 🧪 اختبار سريع

### في Swagger

```
1. افتح: http://localhost:5000/swagger
2. اضغط: Authorize
3. أدخل: Bearer <your_token>
4. جرّب: GET /api/permissions/my-permissions
```

### في Postman

```
1. استورد: Collection من مرفقات التوثيق
2. سجل دخول: POST /api/auth/login
3. انسخ Token
4. أضفه: Authorization → Bearer Token
5. جرّب الـ Endpoints المختلفة
```

---

## 🛠️ إضافة صلاحية جديدة

### في DataSeeder أو Migration:

```csharp
// 1. إنشاء الصلاحية
var permission = new Permission
{
    Name = "Custom_Action",
    Description = "وصف العملية"
};
dbContext.Permissions.Add(permission);

// 2. إضافتها لـ Role
var editor = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Editor");
if (editor != null)
{
    var rolePermission = new RolePermission
    {
        RoleId = editor.Id,
      PermissionId = permission.Id
    };
    dbContext.RolePermissions.Add(rolePermission);
}

await dbContext.SaveChangesAsync();
```

### أو عبر API:

```bash
POST /api/permissions/create
Authorization: Bearer <admin-token>
{
  "name": "Custom_Action",
  "description": "وصف العملية"
}

POST /api/permissions/add-to-role
Authorization: Bearer <admin-token>
{
  "roleId": 2,
  "permissionName": "Custom_Action"
}
```

---

## 📊 جدول الفهم السريع

```
┌────────────────┬──────┬────────┬─────────┬──────────────┐
│ الصلاحية │Admin │Editor  │Viewer   │DeptHead    │
├────────────────┼──────┼────────┼─────────┼──────────────┤
│ إنشاء Form    │  ✅  │  ✅   │  ❌    │  ✅          │
│ تعديل Form    │  ✅  │  ✅   │  ❌  │  ✅          │
│ حذف Form   │  ✅  │  ❌   │  ❌    │  ❌    │
│ عرض Form     │  ✅  │  ✅   │  ✅    │  ✅          │
│ إدارة القسم   │  ✅  │  ❌   │  ❌  │  ✅  │
│ عرض القسم    │  ✅  │  ❌   │  ❌    │  ✅       │
└────────────────┴──────┴────────┴─────────┴──────────────┘
```

---

## 🎓 السيناريوهات الشائعة

### سيناريو 1: موظف يحاول عرض Form من قسم آخر

```
1. GET /api/forms/123
2. Filter: هل تنتمي للقسم نفسه؟
3. إذا لا → 403 Forbidden
4. إذا نعم → ترجع البيانات
```

### سيناريو 2: موظف يحاول تعديل Form ليس ملكه

```
1. PUT /api/forms/123
2. Filter: هل أنت المالك؟
3. إذا لا → 403 Forbidden
4. إذا نعم → يعدل
```

### سيناريو 3: Admin يشوف كل شيء

```
1. GET /api/forms
2. Check: هل Admin؟
3. إذا نعم → يشوف الكل
4. إذا لا → يطبق الـ Filters
```

---

## 📚 الملفات المرجعية

### للدراسة الدقيقة:
- `ADVANCED_PERMISSIONS_GUIDE.md` ← الدليل الشامل
- `IPermissionService.cs` ← الواجهات
- `PermissionService.cs` ← التنفيذ

### للاستخدام السريع:
- `AccessControlFilters.cs` ← الـ Filters
- `PermissionsController.cs` ← الـ Endpoints

---

## ✅ Checklist التطبيق

- [ ] تشغيل Migration
- [ ] اختبار Endpoints في Swagger
- [ ] إضافة Filters لـ Controllers
- [ ] اختبار السيناريوهات المختلفة
- [ ] توثيق الصلاحيات الجديدة
- [ ] اختبار شامل
- [ ] الإطلاق! 🚀

---

## 🆘 في حالة المشاكل

### خطأ: "Unauthorized"

```
المشكلة: لم يتم إدراج Token
الحل: أضف Authorization header:
Authorization: Bearer <your_token>
```

### خطأ: "Forbidden"

```
المشكلة: ليس لديك صلاحية
الحل: تحقق من صلاحياتك:
GET /api/permissions/my-permissions
```

### خطأ: "Service not found"

```
المشكلة: لم تسجل الخدمات في Program.cs
الحل: تأكد من إضافة:
builder.Services.AddScoped<IPermissionService, PermissionService>();
```

---

## 💪 أنت الآن جاهز!

تم إنشاء نظام صلاحيات متقدم يطبق على جميع الـ Entities في التطبيق.

**الخطوة التالية:**
1. شغّل Migration
2. اختبر في Swagger
3. اضف Filters لـ Controllers
4. استمتع! 🎉

---

**هل تحتاج مساعدة؟**

اقرأ `ADVANCED_PERMISSIONS_GUIDE.md` للمزيد من التفاصيل! 📖
