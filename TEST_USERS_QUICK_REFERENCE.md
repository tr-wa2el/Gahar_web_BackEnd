# 🎉 Test Users - Quick Reference Guide

## ⚡ السريع جداً (30 ثانية)

### المستخدمين الجاهزين:

| # | الدور | البريد | الباسورد | Swagger |
|---|------|--------|---------|---------|
| 1 | Super Admin | `admin@gahar.sa` | `Admin@123` | ✅ |
| 2 | Admin | `admin@example.com` | `Admin@123` | ✅ |
| 3 | Editor | `editor@example.com` | `Editor@123` | ❌ |
| 4 | Viewer | `viewer@example.com` | `Viewer@123` | ❌ |
| 5 | User | `user@example.com` | `User@123` | ❌ |

---

## 🚀 البدء السريع (3 خطوات)

### الخطوة 1️⃣: تشغيل التطبيق
```bash
cd Gahar_Backend
dotnet run
```
✅ المستخدمين سيتم إنشاؤهم تلقائياً

### الخطوة 2️⃣: استخدم أداة الاختبار (اختر واحدة)

#### الخيار A: HTML UI (الأسهل) 🌐
```bash
# افتح في المتصفح:
open test-users-login.html
# أو على Windows:
start test-users-login.html
```

#### الخيار B: PowerShell (Windows) 💻
```powershell
# تشغيل Script:
.\login-test.ps1

# اختر رقم المستخدم
# انسخ ال Token
# استخدمه في Swagger
```

#### الخيار C: Bash (Linux/Mac) 🐧
```bash
# تشغيل Script:
chmod +x login-test.sh
./login-test.sh

# اختر رقم المستخدم
# انسخ ال Token
```

#### الخيار D: curl مباشر 📡
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin@123"}'
```

### الخطوة 3️⃣: استخدم Token في Swagger
```
1. انسخ ال accessToken
2. روح http://localhost:5000/swagger/index.html
3. اضغط "Authorize" 🔒
4. ادخل: Bearer <token>
5. اضغط Authorize
✅ تمام! You're in!
```

---

## 📋 تفاصيل كل مستخدم

### 🔐 Super Admin
```json
{
"email": "admin@gahar.sa",
  "password": "Admin@123",
  "firstName": "Super",
  "lastName": "Admin",
  "role": "SuperAdmin",
  "permissions": "جميع الصلاحيات",
  "swaggerAccess": true
}
```

**الاستخدام**: اختبار كل شيء، لديه أعلى صلاحيات

---

### 👨‍💼 Admin
```json
{
  "email": "admin@example.com",
  "password": "Admin@123",
  "firstName": "محمد",
  "lastName": "علي",
  "role": "Admin",
  "permissions": "جميع الصلاحيات ما عدا التدقيق",
  "swaggerAccess": true
}
```

**الاستخدام**: اختبار Swagger والعمليات الإدارية

---

### ✏️ Editor
```json
{
  "email": "editor@example.com",
  "password": "Editor@123",
  "firstName": "فاطمة",
  "lastName": "محمد",
  "role": "Editor",
  "permissions": "View, Create, Edit (بدون Delete)",
  "swaggerAccess": false
}
```

**الاستخدام**: اختبار محرر المحتوى

---

### 👁️ Viewer
```json
{
  "email": "viewer@example.com",
  "password": "Viewer@123",
  "firstName": "أحمد",
  "lastName": "حسن",
  "role": "Viewer",
  "permissions": "View فقط",
  "swaggerAccess": false
}
```

**الاستخدام**: اختبار صلاحيات محدودة

---

### 👤 Regular User
```json
{
  "email": "user@example.com",
  "password": "User@123",
  "firstName": "سارة",
  "lastName": "علي",
  "role": "Viewer",
  "permissions": "View فقط",
  "swaggerAccess": false
}
```

**الاستخدام**: اختبار مستخدم عادي

---

## 🧪 حالات الاختبار

### ✅ Test Case 1: Admin يدخل لـ Swagger
```bash
# 1. دخول
Email: admin@example.com
Password: Admin@123

# 2. النتيجة المتوقعة
✅ Login Success
✅ Get Token
✅ Swagger Access Allowed (200 OK)
```

### ❌ Test Case 2: Editor لا يدخل لـ Swagger
```bash
# 1. دخول
Email: editor@example.com
Password: Editor@123

# 2. النتيجة المتوقعة
✅ Login Success
✅ Get Token
❌ Swagger Access Denied (403 Forbidden)
```

### 🔍 Test Case 3: Viewer له صلاحيات محدودة
```bash
# 1. دخول
Email: viewer@example.com
Password: Viewer@123

# 2. اختبار الصلاحيات
✅ View Operations: SUCCESS
❌ Create Operations: FAIL
❌ Delete Operations: FAIL
```

### 🔓 Test Case 4: Super Admin له كل شيء
```bash
# 1. دخول
Email: admin@gahar.sa
Password: Admin@123

# 2. اختبار الصلاحيات
✅ All Operations: SUCCESS
✅ Swagger: SUCCESS
✅ Audit Logs: SUCCESS
```

---

## 🛠️ أدوات الاختبار المتاحة

### 1️⃣ HTML UI Tool (الأفضل للمبتدئين) 🌐
```bash
# ملف: test-users-login.html
# كيفية الاستخدام:
1. افتح الملف في المتصفح
2. اختر المستخدم من القائمة
3. اضغط Login
4. انسخ Token من النتيجة
5. استخدمه في Swagger
```

**المميزات**:
- ✅ واجهة رسومية سهلة
- ✅ اختيار سريع للمستخدمين
- ✅ عرض معلومات المستخدم
- ✅ نسخ Token بسهولة
- ✅ رابط مباشر لـ Swagger

---

### 2️⃣ PowerShell Script (Windows) 💻
```bash
# ملف: login-test.ps1
# تشغيل:
.\login-test.ps1

# الميزات:
- قائمة تفاعلية
- نسخ Token تلقائي
- معلومات الدخول الكاملة
- رابط Swagger مباشر
```

---

### 3️⃣ Bash Script (Linux/Mac) 🐧
```bash
# ملف: login-test.sh
# تشغيل:
chmod +x login-test.sh
./login-test.sh

# الميزات:
- قائمة تفاعلية
- عرض Token كامل
- معلومات الوصول
- تعليمات واضحة
```

---

### 4️⃣ curl أوامر مباشرة 📡
```bash
# الأساسية:
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin@123"}'

# مع حفظ Token في متغير:
TOKEN=$(curl -s -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin@123"}' \
  | jq -r '.accessToken')

# استخدام Token:
curl -H "Authorization: Bearer $TOKEN" \
  http://localhost:5000/api/endpoint
```

---

### 5️⃣ Postman Collection
```json
1. Create new request
2. Method: POST
3. URL: http://localhost:5000/api/auth/login
4. Headers:
   - Content-Type: application/json
5. Body (raw):
{
  "email": "admin@example.com",
  "password": "Admin@123"
}
6. Send
7. Copy accessToken
8. Create new request for Swagger
9. URL: http://localhost:5000/swagger/index.html
10. Authorization:
    - Type: Bearer Token
    - Token: <paste token>
```

---

## 📊 جدول الصلاحيات الكامل

| الدور | View | Create | Edit | Delete | Audit | Swagger |
|------|------|--------|------|--------|-------|---------|
| Super Admin | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Admin | ✅ | ✅ | ✅ | ✅ | ❌ | ✅ |
| Editor | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ |
| Viewer | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |

---

## 🔑 نمط الباسورد

جميع الباسورات تتبع النمط:
```
[RoleName]@123
```

مثال:
- Super Admin: `Admin@123`
- Admin: `Admin@123`
- Editor: `Editor@123`
- Viewer: `Viewer@123`
- User: `User@123`

---

## 🚀 استخدام سريع في curl

```bash
# Login + Save Token
TOKEN=$(curl -s -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin@123"}' \
  | grep -o '"accessToken":"[^"]*' | cut -d'"' -f4)

echo "Token: $TOKEN"

# Test Swagger Access
curl -I -H "Authorization: Bearer $TOKEN" \
  "http://localhost:5000/swagger/index.html"

# Expected: 200 OK for Admin
# Expected: 403 Forbidden for Non-Admin
```

---

## ⚙️ البيئات المختلفة

### Development Mode
```bash
# جميع المستخدمين لديهم وصول كامل لـ Swagger
# بدون التحقق من Token
```

### Production Mode
```bash
# فقط Admin و SuperAdmin لديهم وصول لـ Swagger
# يتطلب Token صحيح و Admin role
```

---

## 📁 الملفات المتعلقة

```
📄 Files Created/Modified:
├── Gahar_Backend/Utilities/DataSeeder.cs (MODIFIED)
│   └── أضيفت دالة SeedTestUsersAsync
│
├── TEST_USERS.md (THIS FILE)
│   └── توثيق المستخدمين التجريبيين
│
├── login-test.ps1
│   └── سكريبت PowerShell للاختبار
│
├── login-test.sh
│   └── سكريبت Bash للاختبار
│
├── test-users-login.html
│   └── أداة ويب رسومية للاختبار
│
└── test-users-config.json
    └── معلومات المستخدمين بصيغة JSON
```

---

## 🔄 دورة الاختبار الكاملة

```
1️⃣ تشغيل التطبيق
   ↓
2️⃣ اختيار أداة اختبار
   ├─ HTML UI
   ├─ PowerShell
   ├─ Bash
   └─ curl
   ↓
3️⃣ اختيار مستخدم
   ├─ Super Admin
   ├─ Admin
   ├─ Editor
   ├─ Viewer
   └─ User
   ↓
4️⃣ الدخول والحصول على Token
   ↓
5️⃣ اختبار Swagger
   ├─ Admin: ✅ يعمل
   └─ Others: ❌ فشل متوقع
   ↓
6️⃣ اختبار الصلاحيات
   └─ كل دور له صلاحياته
```

---

## 🎯 أسرع طريقة (Copy & Paste)

### Windows:
```powershell
# 1. شغّل PowerShell
# 2. انسخ ولصق هذا:
cd "F:\Web Gahar\bk\Gahar_web_BackEnd"
.\login-test.ps1
```

### Linux/Mac:
```bash
# 1. افتح Terminal
# 2. انسخ ولصق هذا:
cd "F/Web Gahar/bk/Gahar_web_BackEnd"
chmod +x login-test.sh
./login-test.sh
```

### أي نظام:
```bash
# 1. افتح test-users-login.html في المتصفح
# 2. اختر مستخدم
# 3. اضغط Login
# 4. نسخ Token
```

---

## ✅ Verification Checklist

- [ ] تشغيل التطبيق
- [ ] المستخدمين تم إنشاؤهم
- [ ] Admin يدخل بنجاح
- [ ] Editor يدخل لكن لا يدخل Swagger
- [ ] Viewer لديه صلاحيات محدودة
- [ ] Super Admin لديه كل الصلاحيات
- [ ] Token يعمل في Swagger
- [ ] الصلاحيات تُطبق بشكل صحيح

---

## 🎓 المقابل الإنجليزي

### Quick Users Table:

| # | Role | Email | Password | Swagger |
|---|------|-------|----------|---------|
| 1 | Super Admin | `admin@gahar.sa` | `Admin@123` | ✅ |
| 2 | Admin | `admin@example.com` | `Admin@123` | ✅ |
| 3 | Editor | `editor@example.com` | `Editor@123` | ❌ |
| 4 | Viewer | `viewer@example.com` | `Viewer@123` | ❌ |
| 5 | User | `user@example.com` | `User@123` | ❌ |

### Quick Start:
1. Run: `dotnet run`
2. Users auto-created
3. Choose tool: HTML/PowerShell/Bash/curl
4. Login
5. Copy Token
6. Use in Swagger

---

## 🔗 Related Documentation

- [`SWAGGER_QUICK_START.md`](SWAGGER_QUICK_START.md) - Quick Swagger access
- [`SWAGGER_ADMIN_ACCESS.md`](SWAGGER_ADMIN_ACCESS.md) - Complete guide
- [`SWAGGER_API_EXAMPLES.md`](SWAGGER_API_EXAMPLES.md) - API examples
- [`TEST_USERS.md`](TEST_USERS.md) - Full user documentation
- [`test-users-config.json`](test-users-config.json) - User configuration

---

## 📞 Support

**Problem**: Can't login
**Solution**: Check email and password spelling

**Problem**: No Swagger access
**Solution**: Use Admin or SuperAdmin account

**Problem**: Token not working
**Solution**: Copy full token with "Bearer " prefix

**Problem**: Can't run PowerShell script
**Solution**: Use HTML UI tool instead

---

**Status**: ✅ Ready to Test  
**Created**: January 2024  
**Last Updated**: January 2024  

---

**Happy Testing!** 🧪✨

بالتوفيق في الاختبار! 🎉
