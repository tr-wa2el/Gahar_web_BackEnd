# 🚀 البدء السريع مع نظام الأقسام

## ⚡ 3 خطوات فقط للبدء

### 1️⃣ Database Migration (5 دقائق)

```bash
# افتح Terminal في مجلد المشروع
cd F:\Web Gahar\bk\Gahar_web_BackEnd

# أنشئ Migration
dotnet ef migrations add AddDepartmentAccessControl

# طبّق التحديثات على قاعدة البيانات
dotnet ef database update
```

### 2️⃣ تشغيل التطبيق

```bash
# شغّل التطبيق
dotnet run

# الآن يعمل على http://localhost:5000
```

### 3️⃣ اختبر في Swagger

```
افتح: http://localhost:5000/swagger/index.html
```

---

## 📝 الخطوات العملية

### الخطوة 1: إنشاء أقسام

**في Swagger:**

```
POST /api/departments
```

**الجسم:**

```json
{
  "name": "HR",
  "nameAr": "الموارد البشرية",
  "description": "قسم الموارد البشرية",
  "code": "HR",
  "displayOrder": 1
}
```

**النتيجة:**

```json
{
  "id": 1,
  "name": "HR",
  "nameAr": "الموارد البشرية",
  "usersCount": 0
}
```

---

### الخطوة 2: تعيين موظف للقسم

**في SQL أو من خلال API (بعد إضافتها):**

```sql
-- تعديل موظف موجود
UPDATE Users
SET DepartmentId = 1
WHERE Email = 'employee@company.com'
```

أو إذا أضفت API للتحديث:

```
PUT /api/users/{userId}

{
  "departmentId": 1
}
```

---

### الخطوة 3: تعيين رئيس القسم

**في Swagger:**

```
POST /api/departments/{departmentId}/set-head/{userId}
```

**مثال:**

```
POST /api/departments/1/set-head/5
```

---

### الخطوة 4: إنشاء نموذج

**في Swagger:**

```
POST /api/forms
```

**الجسم:**

```json
{
  "title": "طلب إجازة",
  "titleAr": "طلب إجازة",
  "description": "نموذج لطلب الإجازة",
  "formConfiguration": "{}"
}
```

**النموذج يُنشأ تلقائياً في قسم الموظف الحالي!**

---

### الخطوة 5: اختبر الصلاحيات

**موظف 1 (من قسم HR):**

```
GET /api/forms

✅ يشوف نماذج HR
❌ لا يشوف نماذج ACCOUNTING
```

**موظف 2 (من قسم ACCOUNTING):**

```
GET /api/forms

❌ لا يشوف نماذج HR
✅ يشوف نماذج ACCOUNTING
```

**Admin:**

```
GET /api/forms

✅ يشوف جميع النماذج
```

---

## 📊 البيانات الاختبارية

### الأقسام المقترحة:

```
1. HR (الموارد البشرية) - Code: HR
2. Accounting (الحسابات) - Code: ACC
3. Operations (العمليات) - Code: OPS
4. IT (تكنولوجيا المعلومات) - Code: IT
5. Marketing (التسويق) - Code: MKT
```

### النماذج المقترحة:

```
قسم HR:
  - طلب إجازة
  - تقرير طبي
  - طلب تدريب
  - طلب تطوير مهارات

قسم Accounting:
  - طلب صرف
  - تقرير فواتير
  - طلب سلفة
  - تقرير الرواتب

قسم Operations:
  - طلب صيانة
  - تقرير حضور
  - تقرير أداء
  - طلب تجهيزات
```

---

## 🔍 أوامر مفيدة

### عرض جميع الأقسام:

```
GET /api/departments
```

### عرض موظفي قسم:

```
GET /api/departments/{id}/employees
```

### عرض نماذج قسم معين (Admin فقط):

```
GET /api/forms/department/{departmentId}
```

### تحديث قسم:

```
PUT /api/departments/{id}

{
  "name": "Human Resources",
  "nameAr": "إدارة الموارد البشرية"
}
```

---

## ✅ Checklist للتحقق

```
☐ تثبيت Migration بنجاح
☐ تشغيل التطبيق بدون أخطاء
☐ إنشاء أقسام
☐ تعيين موظفين للأقسام
☐ إنشاء نماذج
☐ اختبار الوصول (موظف لا يشوف نماذج قسم آخر)
☐ اختبار Admin (يشوف كل شيء)
☐ تعيين رؤساء أقسام
```

---

## 🐛 حل المشاكل الشائعة

### المشكلة: "Table 'Departments' not found"

**الحل:**

```bash
# تأكد من تطبيق Migration
dotnet ef database update

# إذا لم تنجح، جرب إعادة الـ Migration
dotnet ef database drop  # احذر! هذا يحذف قاعدة البيانات
dotnet ef database update
```

---

### المشكلة: "User not in Department"

**الحل:**

```sql
-- تأكد من تعيين DepartmentId للموظفين
SELECT * FROM Users WHERE DepartmentId IS NULL

-- أضف DepartmentId
UPDATE Users SET DepartmentId = 1 WHERE Id = 5
```

---

### المشكلة: "Forbidden - no access to form"

**هذا طبيعي!** ✅

الموظف من قسم آخر لا يمكنه رؤية النموذج. هذا هو المقصود من النظام!

---

## 🎓 أمثلة متقدمة

### إضافة صفحة لاختبار:

```html
<!-- في index.html أو صفحة اختبار -->
<button onclick="getDepartments()">Get All Departments</button>
<button onclick="getUserForms()">Get My Forms</button>
<button onclick="createForm()">Create Form</button>

<script>
async function getDepartments() {
  const res = await fetch('/api/departments', {
    headers: { 'Authorization': `Bearer ${token}` }
  });
  const data = await res.json();
  console.log('Departments:', data);
}

async function getUserForms() {
  const res = await fetch('/api/forms', {
    headers: { 'Authorization': `Bearer ${token}` }
  });
  const data = await res.json();
  console.log('My Forms:', data);
}

async function createForm() {
  const res = await fetch('/api/forms', {
    method: 'POST',
    headers: {
   'Content-Type': 'application/json',
 'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify({
      title: 'نموذج جديد',
      description: 'وصف النموذج'
    })
  });
  const data = await res.json();
  console.log('New Form:', data);
}
</script>
```

---

## 📞 الدعم والمساعدة

**إذا واجهت أي مشكلة:**

```
1. تحقق من الأخطاء في الـ Console
2. راجع DEPARTMENT_ACCESS_CONTROL.md للتفاصيل الكاملة
3. اختبر الـ APIs في Swagger
4. تأكد من تطبيق Migration بنجاح
```

---

## 🎉 ماذا بعد؟

بعد التأكد من أن كل شيء يعمل:

```
✅ أضف صفحة لإدارة الأقسام في الـ Frontend
✅ أضف صفحة لتعيين الموظفين للأقسام
✅ أضف Dashboard يعرض إحصائيات كل قسم
✅ أضف إشعارات داخل القسم
```

---

**الآن أنت جاهز للبدء!** 🚀

استمتع بنظام الأقسام الجديد! 🎊
