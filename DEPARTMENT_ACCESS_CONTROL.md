# 🔐 نظام الصلاحيات حسب الأقسام (Department-Based Access Control)

## 📋 ملخص سريع

تم إنشاء نظام صلاحيات متقدم حيث:
- ✅ كل قسم يشوف فقط بيانات قسمه
- ✅ عند إنشاء نموذج، كل الناس في نفس القسم يشوفوه
- ✅ الأقسام الأخرى لا تشوفها
- ✅ Admin يشوف كل شيء
- ✅ رئيس القسم له صلاحيات إضافية

---

## 🏗️ المكونات الرئيسية

### 1️⃣ نموذج Department (القسم)

```csharp
public class Department : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } // اسم القسم (English)
    
    [StringLength(100)]
    public string? NameAr { get; set; } // اسم القسم (Arabic)
    
    public string? Description { get; set; } // وصف
    
    public Guid? HeadId { get; set; } // رئيس القسم
    public User? Head { get; set; }
    
    public bool IsActive { get; set; } // إذا كان فعال
    public int DisplayOrder { get; set; } // ترتيب العرض
  
  [StringLength(50)]
    public string? Code { get; set; } // رمز القسم (مثلاً: HR, ACCOUNTING)
    
    // Relationships
    public ICollection<User> Users { get; set; } // موظفي القسم
    public ICollection<Form> Forms { get; set; } // نماذج القسم
}
```

---

### 2️⃣ إضافة DepartmentId للـ User

```csharp
public class User : BaseEntity
{
    // ... الحقول الأخرى ...
    
    /// <summary>
    /// القسم التابع له المستخدم
    /// </summary>
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
}
```

---

### 3️⃣ إضافة DepartmentId للـ Form

```csharp
public class Form : BaseEntity
{
    // ... الحقول الأخرى ...
    
    /// <summary>
    /// القسم التابع له النموذج
    /// يحدد من يمكنه رؤية وتعديل النموذج
    /// </summary>
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
    
    /// <summary>
    /// المستخدم الذي أنشأ النموذج
    /// </summary>
    public int? AuthorId { get; set; }
    public User? Author { get; set; }
}
```

---

### 4️⃣ خدمة التحكم بالصلاحيات (DepartmentAccessService)

```csharp
public interface IDepartmentAccessService
{
    // التحقق من أن المستخدم ينتمي لقسم معين
    Task<bool> IsUserInDepartmentAsync(Guid userId, Guid departmentId);
    
    // الحصول على قسم المستخدم
    Task<Department?> GetUserDepartmentAsync(Guid userId);
    
    // التحقق من أن المستخدم يمكنه الوصول لنموذج
    Task<bool> CanUserAccessFormAsync(Guid userId, Guid formId);
    
    // الحصول على النماذج المتاحة للمستخدم
 Task<List<Form>> GetDepartmentFormsAsync(Guid userId);
    
    // التحقق من أن المستخدم هو رئيس القسم
    Task<bool> IsUserDepartmentHeadAsync(Guid userId);
    
    // الحصول على موظفي قسم المستخدم
    Task<List<User>> GetDepartmentEmployeesAsync(Guid userId);
    
    // التحقق من صلاحية التعديل
  Task<bool> CanUserEditFormAsync(Guid userId, Guid formId);
}
```

---

## 📊 جدول الصلاحيات

```
┌──────────────────┬──────────┬────────┬───────────┬────────┐
│ Operation        │ SuperAdmin │ Admin  │ Editor    │ Viewer │
├──────────────────┼──────────┼────────┼───────────┼────────┤
│ View Own Dept│ ✅ (الكل) │ ✅ (الكل) │ ✅ (الكل) │ ❌    │
│ Create Form      │ ✅       │ ✅     │ ✅        │ ❌    │
│ Edit Own Form │ ✅       │ ✅     │ ✅        │ ❌    │
│ Edit Any Form    │ ✅       │ ✅     │ ❌│ ❌    │
│ Delete Form      │ ✅       │ ✅     │ ✅ (خاص) │ ❌    │
│ View Other Depts │ ❌       │ ✅     │ ❌    │ ❌    │
│ Manage Depts     │ ✅       │ ✅     │ ❌   │ ❌    │
│ Manage Users     │ ✅       │ ✅  │ ❌        │ ❌    │
└──────────────────┴──────────┴────────┴───────────┴────────┘
```

---

## 🚀 كيفية الاستخدام

### الخطوة 1: إنشاء أقسام

```bash
POST /api/departments
{
  "name": "HR",
  "nameAr": "الموارد البشرية",
  "description": "قسم الموارد البشرية",
  "code": "HR",
  "displayOrder": 1
}

Response:
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "HR",
  "nameAr": "الموارد البشرية"
}
```

### الخطوة 2: تعيين الموظفين للأقسام

```sql
-- عند إنشاء مستخدم جديد أو تعديله
UPDATE Users
SET DepartmentId = '550e8400-e29b-41d4-a716-446655440000'
WHERE Email = 'employee@example.com'
```

أو عبر API:

```bash
PUT /api/users/{userId}
{
  "email": "employee@example.com",
  "firstName": "أحمد",
  "lastName": "محمد",
  "departmentId": "550e8400-e29b-41d4-a716-446655440000"
}
```

### الخطوة 3: إنشاء نموذج في القسم

```bash
POST /api/forms
{
  "title": "طلب إجازة",
  "titleAr": "طلب إجازة",
  "description": "نموذج طلب الإجازة",
  "formConfiguration": "{...}"
}

Response:
{
  "id": "660e8400-e29b-41d4-a716-446655440000",
  "title": "طلب إجازة",
  "department": "الموارد البشرية",
  "createdBy": "أحمد محمد",
  "status": "Draft"
}
```

### الخطوة 4: الحصول على النماذج

```bash
# موظف في قسم HR يحصل على نماذج HR فقط
GET /api/forms

Response:
[
  {
    "id": "660e8400-e29b-41d4-a716-446655440000",
    "title": "طلب إجازة",
    "department": "الموارد البشرية",
    "status": "Draft"
  }
]

# موظف في قسم Accounting لا يشوف نماذج HR
# Admin يشوف جميع النماذج
```

---

## 🔐 منطق التحكم بالصلاحيات

### ✅ من يمكنه رؤية النموذج؟

```csharp
public async Task<bool> CanUserAccessFormAsync(Guid userId, Guid formId)
{
    var form = await _context.Forms.FirstOrDefaultAsync(f => f.Id == formId);
    var user = await _context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userId);
    
    // 1️⃣ Admin و SuperAdmin يشوفوا كل النماذج
    var isAdmin = user.UserRoles.Any(ur => 
        ur.Role?.Name == "Admin" || ur.Role?.Name == "SuperAdmin");
    if (isAdmin)
        return true;
    
    // 2️⃣ المستخدمين العاديين يشوفوا فقط نماذج قسمهم
    return user.DepartmentId == form.DepartmentId;
}
```

---

### ✏️ من يمكنه تعديل النموذج؟

```csharp
public async Task<bool> CanUserEditFormAsync(Guid userId, Guid formId)
{
    var form = await _context.Forms.FirstOrDefaultAsync(f => f.Id == formId);
    var user = await _context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userId);
    
    // 1️⃣ Admin و SuperAdmin يعدلوا أي نموذج
    var isAdmin = user.UserRoles.Any(ur => 
        ur.Role?.Name == "Admin" || ur.Role?.Name == "SuperAdmin");
    if (isAdmin)
        return true;
    
    // 2️⃣ من أنشأ النموذج يعدله
    if (form.AuthorId == userId)
        return true;
    
    // 3️⃣ رئيس القسم يعدل نماذج قسمه
    var isDepartmentHead = await IsUserDepartmentHeadAsync(userId);
    if (isDepartmentHead && form.DepartmentId == user.DepartmentId)
        return true;
    
    return false;
}
```

---

## 📝 الملفات المُنشأة/المُعدلة

### ✨ ملفات جديدة:

```
✅ Gahar_Backend/Models/Entities/Department.cs
✅ Gahar_Backend/Services/DepartmentAccessService.cs
✅ Gahar_Backend/Controllers/DepartmentsController.cs
✅ DEPARTMENT_ACCESS_CONTROL.md (هذا الملف)
```

### 🔧 ملفات معدّلة:

```
✅ Gahar_Backend/Models/Entities/User.cs (أضيف DepartmentId)
✅ Gahar_Backend/Models/Entities/Form.cs (أضيف DepartmentId)
✅ Gahar_Backend/Data/ApplicationDbContext.cs (أضيف DbSet<Department>)
✅ Gahar_Backend/Program.cs (تسجيل الخدمة)
```

---

## 🔄 مثال عملي كامل

### السيناريو: شركة بها 3 أقسام

#### 1️⃣ الأقسام:

```
HR (الموارد البشرية)
├── أحمد محمد (رئيس القسم)
├── فاطمة علي
└── محمود سالم

ACCOUNTING (الحسابات)
├── سارة أحمد (رئيسة القسم)
├── علي محمود
└── نور محمد

OPERATIONS (العمليات)
├── خالد إبراهيم (رئيس القسم)
└── ليلى عبدالله
```

#### 2️⃣ النماذج:

```
نموذج 1: "طلب إجازة" - في قسم HR
  ├─ يمكن أحمد رؤيته ✅
  ├─ يمكن فاطمة رؤيته ✅
  ├─ لا يمكن سارة رؤيته ❌
  └─ يمكن Admin رؤيته ✅

نموذج 2: "طلب مالي" - في قسم ACCOUNTING
  ├─ لا يمكن أحمد رؤيته ❌
  ├─ يمكن سارة رؤيته ✅
  ├─ يمكن علي رؤيته ✅
  └─ يمكن Admin رؤيته ✅
```

#### 3️⃣ عملية إنشاء نموذج:

```
1. أحمد (من HR) ينشئ نموذج "طلب إجازة"
↓
2. النموذج يُحفظ مع DepartmentId = HR
   ↓
3. فاطمة (من HR) تشوف النموذج ✅
4. سارة (من ACCOUNTING) لا تشوفه ❌
5. Admin يشوفه ✅
```

---

## 🧪 اختبار النظام

### Test Case 1: موظف يشوف نماذج قسمه فقط

```bash
# تسجيل دخول كـ موظف HR
POST /api/auth/login
{
  "email": "fatima@example.com",
  "password": "password123"
}

# الحصول على النماذج
GET /api/forms

# النتيجة: يشوف فقط نماذج HR
✅ نموذج 1 (طلب إجازة - HR)
❌ نموذج 2 (طلب مالي - ACCOUNTING) - لا يظهر
```

### Test Case 2: Admin يشوف جميع النماذج

```bash
# تسجيل دخول كـ Admin
POST /api/auth/login
{
  "email": "admin@example.com",
  "password": "admin123"
}

# الحصول على النماذج
GET /api/forms

# النتيجة: يشوف جميع النماذج
✅ نموذج 1 (طلب إجازة - HR)
✅ نموذج 2 (طلب مالي - ACCOUNTING)
✅ جميع النماذج الأخرى
```

### Test Case 3: الوصول للنموذج مباشرة

```bash
# موظف ACCOUNTING يحاول الوصول لنموذج HR
GET /api/forms/660e8400-e29b-41d4-a716-446655440000

# النتيجة:
❌ 403 Forbidden
{
  "message": "ليس لديك صلاحية للوصول لهذا النموذج"
}
```

---

## 🔄 Migration قاعدة البيانات

يجب تنفيذ migration لإضافة الحقول الجديدة:

```bash
# إنشاء migration
dotnet ef migrations add AddDepartmentAccessControl

# تطبيق migration
dotnet ef database update
```

---

## 📚 الملفات المرجعية

- `DepartmentAccessService.cs` - الخدمة الرئيسية
- `DepartmentsController.cs` - API للأقسام
- `Department.cs` - نموذج القسم
- `User.cs` - معدل مع DepartmentId
- `Form.cs` - معدل مع DepartmentId

---

## ✅ Checklist

```
✅ نموذج Department مُنشأ
✅ DepartmentId أضيف للـ User
✅ DepartmentId أضيف للـ Form
✅ DepartmentAccessService مُنشأة
✅ DepartmentsController مُنشأ
✅ الخدمة مسجلة في Program.cs
✅ يحتاج migration لقاعدة البيانات
✅ يحتاج اختبار العملية كاملة
```

---

## 🎯 الخطوات التالية

1. تنفيذ migration
```bash
dotnet ef migrations add AddDepartmentAccessControl
dotnet ef database update
```

2. إنشاء أقسام جديدة عبر API

3. تعيين الموظفين للأقسام

4. إنشاء نماذج في الأقسام

5. اختبار الصلاحيات

---

**Status**: ✅ Ready for Testing  
**Build**: Check for errors  
**Database**: Needs Migration

---

هل تريد أي تعديلات أو توضيحات؟ 🤔
