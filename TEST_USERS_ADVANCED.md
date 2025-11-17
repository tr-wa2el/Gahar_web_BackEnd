# 🧪 Test Users - Advanced Guide & Troubleshooting

## 📚 محتويات الملف

1. [المستخدمين بالتفصيل](#المستخدمين-بالتفصيل)
2. [أدوات الاختبار](#أدوات-الاختبار)
3. [حالات الاستخدام](#حالات-الاستخدام)
4. [استكشاف الأخطاء](#استكشاف-الأخطاء)
5. [أمثلة متقدمة](#أمثلة-متقدمة)

---

## المستخدمين بالتفصيل

### 1. Super Admin (`admin@gahar.sa`)

```json
{
  "email": "admin@gahar.sa",
  "password": "Admin@123",
  "username": "superadmin",
  "firstName": "Super",
  "lastName": "Admin",
  "role": "SuperAdmin",
  "displayName": "مدير النظام",
  "isSystemRole": true
}
```

**الصلاحيات**:
```
✅ Users Management
   ├─ Users.View
   ├─ Users.Create
   ├─ Users.Edit
   └─ Users.Delete

✅ Roles Management
   ├─ Roles.View
   ├─ Roles.Create
   ├─ Roles.Edit
   └─ Roles.Delete

✅ Content Management
   ├─ Content.View
   ├─ Content.Create
   ├─ Content.Edit
   ├─ Content.Delete
   └─ Content.Publish

✅ ALL OTHER MODULES

✅ Swagger Access
```

**الاستخدام**:
- إدارة النظام الكاملة
- اختبار جميع الميزات
- إنشاء/حذف المستخدمين والأدوار
- الوصول الكامل للـ Swagger

**مثال curl**:
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@gahar.sa",
    "password": "Admin@123"
  }'
```

---

### 2. Admin (`admin@example.com`)

```json
{
  "email": "admin@example.com",
  "password": "Admin@123",
  "username": "admin",
  "firstName": "محمد",
  "lastName": "علي",
  "role": "Admin",
  "displayName": "مدير",
  "isSystemRole": true
}
```

**الصلاحيات**:
```
✅ Users Management
   ├─ Users.View
   ├─ Users.Create
   ├─ Users.Edit
   └─ Users.Delete

❌ Roles Management (لا يمكن حذف الأدوار)
   ├─ Roles.View
   ├─ Roles.Create
   ├─ Roles.Edit
   └─ ❌ Roles.Delete

✅ Content Management (كامل)

✅ Pages Management (كامل)

✅ Forms Management (كامل)

❌ Audit Logs (View فقط مسموح في البعض)

✅ Swagger Access
```

**الاستخدام**:
- إدارة يومية للنظام
- إدارة المحتوى والصفحات
- إدارة النماذج والمستخدمين
- الوصول لـ Swagger

**مثال curl**:
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
  "email": "admin@example.com",
    "password": "Admin@123"
  }'
```

---

### 3. Editor (`editor@example.com`)

```json
{
  "email": "editor@example.com",
"password": "Editor@123",
  "username": "editor",
  "firstName": "فاطمة",
"lastName": "محمد",
  "role": "Editor",
  "displayName": "محرر",
  "isSystemRole": true
}
```

**الصلاحيات**:
```
✅ View Operations
   ├─ Content.View
   ├─ Pages.View
   ├─ Forms.View
   └─ ...

✅ Create Operations
   ├─ Content.Create
   ├─ Pages.Create
   └─ Forms.Create

✅ Edit Operations
   ├─ Content.Edit
   ├─ Pages.Edit
   └─ Forms.Edit

❌ Delete Operations
   ├─ ❌ Content.Delete
   ├─ ❌ Pages.Delete
   └─ ❌ Forms.Delete

❌ Administrative Operations
   ├─ ❌ Users.* (أي عملية)
   ├─ ❌ Roles.* (أي عملية)
   └─ ❌ Audit Logs.*

❌ Swagger Access
```

**الاستخدام**:
- تحرير المحتوى
- إنشاء صفحات جديدة
- إنشاء نماذج
- لا يمكن الوصول لـ Swagger

**مثال curl**:
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "editor@example.com",
    "password": "Editor@123"
  }'
```

---

### 4. Viewer (`viewer@example.com`)

```json
{
  "email": "viewer@example.com",
  "password": "Viewer@123",
  "username": "viewer",
  "firstName": "أحمد",
  "lastName": "حسن",
  "role": "Viewer",
  "displayName": "مشاهد",
  "isSystemRole": true
}
```

**الصلاحيات**:
```
✅ View Only Operations
   ├─ Content.View
   ├─ Pages.View
   ├─ Forms.View
   ├─ Users.View
   ├─ Roles.View
   └─ ContentTypes.View

❌ All Create Operations
❌ All Edit Operations
❌ All Delete Operations
❌ Administrative Operations
❌ Swagger Access
```

**الاستخدام**:
- مشاهدة المحتوى فقط
- عرض الصفحات والنماذج
- لا يمكن التعديل أو الحذف

**مثال curl**:
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "viewer@example.com",
    "password": "Viewer@123"
  }'
```

---

### 5. Regular User (`user@example.com`)

```json
{
  "email": "user@example.com",
  "password": "User@123",
  "username": "user",
  "firstName": "سارة",
  "lastName": "علي",
  "role": "Viewer",
  "displayName": "مستخدم عادي"
}
```

**الصلاحيات**:
```
✅ View Only (مثل Viewer)
❌ Everything Else
❌ Swagger Access
```

**الاستخدام**:
- مستخدم عادي للنظام
- عرض المحتوى فقط

---

## أدوات الاختبار

### أداة 1: HTML UI (الأسهل)

**الملف**: `test-users-login.html`

**كيفية الاستخدام**:
```html
1. افتح الملف في متصفح
2. اختر مستخدم من القائمة
3. سيظهر بيانات المستخدم
4. اضغط "دخول / Login"
5. سيظهر ال Token
6. انسخ Token وأستخدمه في Swagger
```

**المميزات**:
- واجهة رسومية جميلة
- اختيار سريع
- عرض معلومات المستخدم
- نسخ Token بضغطة زر
- شارة توضح Swagger Access
- رابط مباشر لـ Swagger

**مثال**:
```html
<!-- في المتصفح -->
Choose User: [▼ اختر مستخدم...]
  ↓
           [👨‍💼 Admin (admin@example.com)]
           ↓
[🚀 Login] [📋 Copy Token]
 ↓
✅ Login Successful!
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

✅ Swagger Access: ENABLED
🌐 Open Swagger →
```

---

### أداة 2: PowerShell (Windows)

**الملف**: `login-test.ps1`

**التشغيل**:
```powershell
cd "F:\Web Gahar\bk\Gahar_web_BackEnd"
.\login-test.ps1
```

**المخرجات**:
```
========================================
🔐 Gahar Backend - Test Users Login
========================================

📋 Available Test Users:
========================
1) Super Admin (admin@gahar.sa)
2) Admin (admin@example.com)
3) Editor (editor@example.com)
4) Viewer (viewer@example.com)
5) Regular User (user@example.com)

Select user (1-5): 2

🔐 Logging in as: Admin (admin@example.com)
Endpoint: http://localhost:5000/api/auth/login

✅ Login Successful!

📝 Your Tokens:
===============

Access Token:
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

Refresh Token:
VmFsaWRSZWZyZXNoVG9rZW5IZXJl...

✅ Swagger Access: ENABLED
🌐 Swagger URL: http://localhost:5000/swagger/index.html
```

**الميزات**:
- قائمة تفاعلية
- معلومات كاملة
- نسخ لـ Clipboard
- رابط Swagger

---

### أداة 3: Bash Script (Linux/Mac)

**الملف**: `login-test.sh`

**التشغيل**:
```bash
chmod +x login-test.sh
./login-test.sh
```

**المخرجات**:
```
========================================
🔐 Gahar Backend - Test Users Login
========================================

📋 Available Test Users:
========================
1) Super Admin (admin@gahar.sa)
2) Admin (admin@example.com)
3) Editor (editor@example.com)
4) Viewer (viewer@example.com)
5) Regular User (user@example.com)

Select user (1-5): 2
```

**الميزات**:
- قائمة تفاعلية
- معلومات واضحة
- عمل على Linux/Mac

---

### أداة 4: curl مباشر

**الأساسي**:
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin@123"}'
```

**مع معالجة النتيجة**:
```bash
# Linux/Mac
TOKEN=$(curl -s -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin@123"}' \
  | jq -r '.accessToken')

echo "Token: $TOKEN"
```

**Windows PowerShell**:
```powershell
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/auth/login" `
  -Method Post `
  -Headers @{"Content-Type" = "application/json"} `
  -Body '{"email":"admin@example.com","password":"Admin@123"}'

$token = $response.accessToken
Write-Host "Token: $token"
```

---

### أداة 5: JSON Configuration

**الملف**: `test-users-config.json`

**الاستخدام البرمجي**:
```javascript
// Load JSON
const config = require('./test-users-config.json');

// اختر مستخدم
const admin = config.testUsers[1]; // Admin

// استخدمه
console.log(admin.email);     // admin@example.com
console.log(admin.password);  // Admin@123
console.log(admin.swaggerAccess); // true
```

---

## حالات الاستخدام

### حالة 1: اختبار Swagger Access

```bash
# تسجيل دخول كـ Admin
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin@123"}' \
  > admin-response.json

# استخراج Token
TOKEN=$(jq -r '.accessToken' admin-response.json)

# اختبار Swagger
curl -i -H "Authorization: Bearer $TOKEN" \
  "http://localhost:5000/swagger/index.html"

# النتيجة المتوقعة: 200 OK
```

---

### حالة 2: اختبار الصلاحيات

```bash
# Editor يحاول Delete
TOKEN=$(curl -s -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"editor@example.com","password":"Editor@123"}' \
| jq -r '.accessToken')

# محاولة حذف
curl -X DELETE "http://localhost:5000/api/content/123" \
  -H "Authorization: Bearer $TOKEN"

# النتيجة المتوقعة: 403 Forbidden
```

---

### حالة 3: اختبار Non-Admin Swagger Access

```bash
# Viewer يحاول الدخول لـ Swagger
TOKEN=$(curl -s -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"viewer@example.com","password":"Viewer@123"}' \
  | jq -r '.accessToken')

# محاولة الوصول
curl -i -H "Authorization: Bearer $TOKEN" \
  "http://localhost:5000/swagger/index.html"

# النتيجة المتوقعة: 403 Forbidden
```

---

## استكشاف الأخطاء

### ❌ خطأ: "Invalid credentials"

**السبب**: البريد أو الباسورد خاطئ

**الحل**:
```bash
# تحقق من البيانات
Email: admin@example.com  (correct)
Password: Admin@123     (correct)

# جرب مرة أخرى
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin@123"}'
```

---

### ❌ خطأ: "User not found"

**السبب**: المستخدم لم يتم إنشاؤه

**الحل**:
```bash
# 1. تأكد من تشغيل التطبيق
dotnet run

# 2. انتظر DataSeeder ينتهي
# 3. جرب مرة أخرى
```

---

### ❌ خطأ: "Cannot access Swagger"

**السبب**: User ليس Admin

**الحل**:
```bash
# استخدم Admin أو SuperAdmin
Email: admin@example.com
Password: Admin@123

# ثم حاول مرة أخرى
```

---

### ❌ خطأ: "403 Forbidden on Swagger"

**السبب**: Token صحيح لكن User ليس Admin

**الحل**:
```bash
# تأكد من أن User له role Admin
# استخدم:
- admin@gahar.sa (SuperAdmin)
- admin@example.com (Admin)
```

---

### ❌ خطأ: "Invalid Token"

**السبب**: Token معطوب أو انتهت صلاحيته

**الحل**:
```bash
# احصل على token جديد
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin@123"}'

# استخدم ال token الجديد
```

---

## أمثلة متقدمة

### مثال 1: تسلسل الدخول الكامل

```bash
#!/bin/bash

API_URL="http://localhost:5000"
EMAIL="admin@example.com"
PASSWORD="Admin@123"

echo "🔐 تسجيل الدخول..."
LOGIN_RESPONSE=$(curl -s -X POST "$API_URL/api/auth/login" \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"$EMAIL\",\"password\":\"$PASSWORD\"}")

# استخراج Tokens
ACCESS_TOKEN=$(echo "$LOGIN_RESPONSE" | jq -r '.accessToken')
REFRESH_TOKEN=$(echo "$LOGIN_RESPONSE" | jq -r '.refreshToken')
USER_ID=$(echo "$LOGIN_RESPONSE" | jq -r '.user.id')

echo "✅ تم الدخول بنجاح"
echo "Access Token: $ACCESS_TOKEN"
echo "Refresh Token: $REFRESH_TOKEN"
echo "User ID: $USER_ID"

# اختبار Swagger
echo ""
echo "🧪 اختبار Swagger..."
SWAGGER_RESPONSE=$(curl -s -I -H "Authorization: Bearer $ACCESS_TOKEN" \
  "$API_URL/swagger/index.html")

if echo "$SWAGGER_RESPONSE" | grep -q "200 OK"; then
  echo "✅ Swagger يعمل!"
else
  echo "❌ لا يمكن الوصول لـ Swagger"
fi
```

---

### مثال 2: اختبار آلي للصلاحيات

```python
import requests
import json

API_URL = "http://localhost:5000"

# بيانات المستخدمين
users = [
    {"email": "admin@example.com", "password": "Admin@123", "can_delete": True},
    {"email": "editor@example.com", "password": "Editor@123", "can_delete": False},
    {"email": "viewer@example.com", "password": "Viewer@123", "can_delete": False},
]

for user in users:
    # الدخول
    response = requests.post(
        f"{API_URL}/api/auth/login",
        json={"email": user["email"], "password": user["password"]}
  )
    
 token = response.json()["accessToken"]
    
    # اختبار Delete
    delete_response = requests.delete(
        f"{API_URL}/api/content/123",
     headers={"Authorization": f"Bearer {token}"}
    )
 
can_delete = delete_response.status_code == 200
  expected = user["can_delete"]
    
    status = "✅" if can_delete == expected else "❌"
    print(f"{status} {user['email']}: {can_delete} (expected {expected})")
```

---

### مثال 3: استخدام في JavaScript/Node.js

```javascript
async function loginAndTestSwagger(email, password) {
  // الدخول
  const loginResponse = await fetch('http://localhost:5000/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });

  const data = await loginResponse.json();
  const token = data.accessToken;

  // اختبار Swagger
  const swaggerResponse = await fetch('http://localhost:5000/swagger/index.html', {
    headers: { 'Authorization': `Bearer ${token}` }
  });

  console.log(`${email}: ${swaggerResponse.status === 200 ? '✅' : '❌'}`);
}

// الاستخدام
loginAndTestSwagger('admin@example.com', 'Admin@123');
loginAndTestSwagger('editor@example.com', 'Editor@123');
```

---

## 📊 جدول المقارنة الكامل

| الميزة | Super Admin | Admin | Editor | Viewer | User |
|--------|-------------|-------|--------|--------|------|
| **Users Management** | ✅✅✅✅ | ✅✅✅✅ | ❌ | ✅ | ❌ |
| **Roles Management** | ✅✅✅✅ | ✅✅✅❌ | ❌ | ✅ | ❌ |
| **Content CRUD** | ✅✅✅✅ | ✅✅✅✅ | ✅✅✅❌ | ✅ | ✅ |
| **Pages CRUD** | ✅✅✅✅ | ✅✅✅✅ | ✅✅✅❌ | ✅ | ✅ |
| **Forms CRUD** | ✅✅✅✅ | ✅✅✅✅ | ✅✅✅❌ | ✅ | ✅ |
| **Audit Logs** | ✅✅✅✅ | ❌ | ❌ | ❌ | ❌ |
| **Swagger Access** | ✅ | ✅ | ❌ | ❌ | ❌ |

Legend:
- ✅ = Allowed
- ❌ = Denied
- ✅✅✅✅ = Full Access
- ✅ = View Only

---

## 🔄 حياة Request الكامل

```
1. User selects credentials
2. POST /api/auth/login
3. Verify email & password hash
4. Generate JWT Token
5. Return accessToken & refreshToken
6. Client stores token
7. Client sends token in Authorization header
8. Server validates token
9. Server checks role claims
10. Server returns 200 (success) or 401/403 (failure)
11. Client displays result
```

---

**الملف الأخير في السلسلة** ✅

---

**Status**: ✅ Complete  
**Last Updated**: January 2024  
**Build Status**: ✅ Successful
