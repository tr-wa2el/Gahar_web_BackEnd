# 🧪 FEATURE 2: FORM BUILDER - TESTING GUIDE

**Date:** 13 يناير 2025  
**Status:** ✅ READY FOR TESTING

---

## 📋 Test Cases

### Group 1: Form Management (5 Tests)

#### Test 1.1: Create Form - Success ✅

**Request:**
```
POST /api/forms
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Contact Us",
  "slug": "contact-us",
  "description": "Contact form for inquiries",
  "successMessage": "Thank you for your message",
  "sendNotificationEmail": true,
  "notificationEmail": "admin@example.com",
  "sendSubmitterEmail": true,
  "submitterEmailField": "email"
}
```

**Expected Response:** 201 Created with Form Details

---

#### Test 1.2: Create Form - Duplicate Slug ❌

**Request:**
```
POST /api/forms
Authorization: Bearer {token}

{
  "title": "Another Contact Form",
  "slug": "contact-us",  // Already exists
  ...
}
```

**Expected:** 400 Bad Request - "Slug already exists"

---

#### Test 1.3: Get All Forms

**Request:**
```
GET /api/forms
```

**Expected:** 200 OK with paginated forms

---

#### Test 1.4: Get Form by ID

**Request:**
```
GET /api/forms/1
```

**Expected:** 200 OK with full form details

---

#### Test 1.5: Get Form by Slug (Public)

**Request:**
```
GET /api/forms/slug/contact-us
```

**Expected:** 200 OK (if published)

---

### Group 2: Form Fields (5 Tests)

#### Test 2.1: Add Text Field

**Request:**
```
POST /api/forms/1/fields
Authorization: Bearer {token}

{
  "label": "Full Name",
  "fieldName": "fullName",
  "fieldType": "Text",
  "isRequired": true,
  "placeholder": "Enter full name",
  "helpText": "Your complete name"
}
```

**Expected:** 200 OK with Field Details

---

#### Test 2.2: Add Email Field

**Request:**
```
POST /api/forms/1/fields
Authorization: Bearer {token}

{
  "label": "Email Address",
  "fieldName": "email",
  "fieldType": "Email",
  "isRequired": true
}
```

**Expected:** 200 OK

---

#### Test 2.3: Add Select Field

**Request:**
```
POST /api/forms/1/fields
Authorization: Bearer {token}

{
  "label": "Subject",
  "fieldName": "subject",
  "fieldType": "Select",
  "fieldConfiguration": "{\"options\": [\"Sales\", \"Support\", \"General\"]}",
  "isRequired": true
}
```

**Expected:** 200 OK

---

#### Test 2.4: Add TextArea Field

**Request:**
```
POST /api/forms/1/fields
Authorization: Bearer {token}

{
  "label": "Message",
  "fieldName": "message",
  "fieldType": "TextArea",
  "isRequired": true,
  "placeholder": "Enter your message"
}
```

**Expected:** 200 OK

---

#### Test 2.5: Reorder Fields

**Request:**
```
POST /api/forms/1/reorder-fields
Authorization: Bearer {token}

{
  "fieldIds": [3, 1, 2]  // New order
}
```

**Expected:** 204 No Content

---

### Group 3: Form Publishing (3 Tests)

#### Test 3.1: Publish Form

**Request:**
```
POST /api/forms/1/publish
Authorization: Bearer {token}
```

**Expected:** 204 No Content

---

#### Test 3.2: Get Published Form

**Request:**
```
GET /api/forms/slug/contact-us
```

**Expected:** 200 OK (now accessible)

---

#### Test 3.3: Unpublish Form

**Request:**
```
POST /api/forms/1/unpublish
Authorization: Bearer {token}
```

**Expected:** 204 No Content

---

### Group 4: Form Submission (6 Tests)

#### Test 4.1: Submit Form - Success ✅

**Request:**
```
POST /api/forms/1/submit
Content-Type: application/json

{
  "data": {
    "fullName": "أحمد محمد",
    "email": "ahmed@example.com",
    "subject": "Sales",
    "message": "I would like to know more about your services"
  },
  "email": "ahmed@example.com"
}
```

**Expected:** 200 OK with Submission Details

---

#### Test 4.2: Get Form Submissions

**Request:**
```
GET /api/forms/1/submissions
Authorization: Bearer {token}
```

**Expected:** 200 OK with paginated submissions

---

#### Test 4.3: Get Single Submission

**Request:**
```
GET /api/forms/submissions/1
Authorization: Bearer {token}
```

**Expected:** 200 OK with submission details

---

#### Test 4.4: Mark As Read

**Request:**
```
POST /api/forms/submissions/1/read
Authorization: Bearer {token}
```

**Expected:** 204 No Content

---

#### Test 4.5: Archive Submission

**Request:**
```
POST /api/forms/submissions/1/archive
Authorization: Bearer {token}
```

**Expected:** 204 No Content

---

#### Test 4.6: Get Unread Submissions

**Request:**
```
GET /api/forms/1/submissions/unread
Authorization: Bearer {token}
```

**Expected:** 200 OK with unread submissions

---

### Group 5: Field Types Testing (10 Tests)

#### Test 5.1: Text Field

```json
{
  "label": "Text",
  "fieldName": "text",
  "fieldType": "Text"
}
```

✅ Expected: Success

---

#### Test 5.2: Email Field

```json
{
  "label": "Email",
  "fieldName": "email",
  "fieldType": "Email"
}
```

✅ Expected: Success

---

#### Test 5.3: Number Field

```json
{
  "label": "Age",
  "fieldName": "age",
  "fieldType": "Number"
}
```

✅ Expected: Success

---

#### Test 5.4: Select Field

```json
{
  "label": "Country",
  "fieldName": "country",
  "fieldType": "Select",
  "fieldConfiguration": "{\"options\": [\"Egypt\", \"Saudi Arabia\", \"UAE\"]}"
}
```

✅ Expected: Success

---

#### Test 5.5: MultiSelect Field

```json
{
  "label": "Interests",
  "fieldName": "interests",
  "fieldType": "MultiSelect",
  "fieldConfiguration": "{\"options\": [\"Tech\", \"Sports\", \"Art\"]}"
}
```

✅ Expected: Success

---

#### Test 5.6: Checkbox Field

```json
{
  "label": "I agree to terms",
  "fieldName": "agreeTerms",
  "fieldType": "Checkbox",
  "isRequired": true
}
```

✅ Expected: Success

---

#### Test 5.7: Radio Field

```json
{
  "label": "Preferred Contact",
  "fieldName": "contact",
  "fieldType": "Radio",
  "fieldConfiguration": "{\"options\": [\"Email\", \"Phone\"]}"
}
```

✅ Expected: Success

---

#### Test 5.8: TextArea Field

```json
{
  "label": "Comments",
  "fieldName": "comments",
  "fieldType": "TextArea"
}
```

✅ Expected: Success

---

#### Test 5.9: Date Field

```json
{
  "label": "Date of Birth",
  "fieldName": "dob",
  "fieldType": "Date"
}
```

✅ Expected: Success

---

#### Test 5.10: File Field

```json
{
  "label": "Upload Document",
  "fieldName": "document",
  "fieldType": "File"
}
```

✅ Expected: Success

---

### Group 6: Filtering & Pagination (5 Tests)

#### Test 6.1: Filter by Published Status

**Request:**
```
GET /api/forms?isPublished=true
```

**Expected:** Only published forms

---

#### Test 6.2: Search by Title

**Request:**
```
GET /api/forms?searchTerm=contact
```

**Expected:** Forms matching "contact"

---

#### Test 6.3: Pagination

**Request:**
```
GET /api/forms?pageNumber=1&pageSize=5
```

**Expected:** 5 items per page

---

#### Test 6.4: Filter Submissions by Status

**Request:**
```
GET /api/forms/1/submissions?isRead=false
```

**Expected:** Only unread submissions

---

#### Test 6.5: Filter Submissions by Date

**Request:**
```
GET /api/forms/1/submissions?fromDate=2025-01-01&toDate=2025-01-31
```

**Expected:** Submissions in date range

---

### Group 7: Update & Delete (4 Tests)

#### Test 7.1: Update Form

**Request:**
```
PUT /api/forms/1
Authorization: Bearer {token}

{
  "title": "Contact Us - Updated",
  "slug": "contact-us",
  "isActive": true,
  "isPublished": true
}
```

**Expected:** 200 OK with updated form

---

#### Test 7.2: Update Field

**Request:**
```
PUT /api/forms/1/fields/1
Authorization: Bearer {token}

{
  "label": "Full Name (Updated)",
  "fieldName": "fullName",
  "fieldType": "Text",
  "displayOrder": 1
}
```

**Expected:** 200 OK

---

#### Test 7.3: Delete Field

**Request:**
```
DELETE /api/forms/1/fields/1
Authorization: Bearer {token}
```

**Expected:** 204 No Content

---

#### Test 7.4: Delete Form

**Request:**
```
DELETE /api/forms/1
Authorization: Bearer {token}
```

**Expected:** 204 No Content

---

### Group 8: Error Handling (5 Tests)

#### Test 8.1: Form Not Found

**Request:**
```
GET /api/forms/9999
```

**Expected:** 404 Not Found

---

#### Test 8.2: Invalid Field Type

**Request:**
```
POST /api/forms/1/fields
Authorization: Bearer {token}

{
  "label": "Field",
  "fieldName": "field",
  "fieldType": "InvalidType"  // ❌
}
```

**Expected:** 400 Bad Request

---

#### Test 8.3: No Authentication

**Request:**
```
POST /api/forms
// No Authorization header

{...}
```

**Expected:** 401 Unauthorized

---

#### Test 8.4: Insufficient Permission

**Request:**
```
POST /api/forms
Authorization: Bearer {token_without_permission}

{...}
```

**Expected:** 403 Forbidden

---

#### Test 8.5: Duplicate Email on Submit

**Request (First):**
```
POST /api/forms/1/submit
{
  "data": {...},
  "email": "duplicate@example.com"
}
```

**Request (Second - Same Email):**
```
POST /api/forms/1/submit
{
  "data": {...},
  "email": "duplicate@example.com"
}
```

**Expected:** Should both succeed (AllowMultipleSubmissions = true)

---

## 📊 Test Summary

**Total Tests:** 40+
- Form Management: 5
- Form Fields: 5  
- Publishing: 3
- Submission: 6
- Field Types: 10
- Filtering: 5
- Update/Delete: 4
- Error Handling: 5

---

## ✅ Success Criteria

- [ ] All 40+ tests pass
- [ ] No compilation errors
- [ ] No database errors
- [ ] Proper error messages
- [ ] Correct HTTP status codes
- [ ] Proper field ordering
- [ ] Correct pagination
- [ ] All field types work

---

**Status:** 🟢 **READY FOR TESTING**

**Last Updated:** 13 يناير 2025
