# 👥 GAHAR CMS - User Stories & Requirements

## 📋 Table of Contents
1. [User Personas](#user-personas)
2. [Epic 1: Public Website Features](#epic-1-public-website-features)
3. [Epic 2: Content Management System](#epic-2-content-management-system)
4. [Epic 3: Page Builder](#epic-3-page-builder)
5. [Epic 4: Facilities Management](#epic-4-facilities-management)
6. [Epic 5: Certificate Validation](#epic-5-certificate-validation)
7. [Epic 6: Forms & Submissions](#epic-6-forms--submissions)
8. [Epic 7: User Management & Authentication](#epic-7-user-management--authentication)
9. [Epic 8: Multilingual Support](#epic-8-multilingual-support)
10. [Epic 9: Analytics & Reporting](#epic-9-analytics--reporting)

---

## 👤 User Personas

### 1. **Public User (Website Visitor)**
- **Who:** General public, healthcare professionals, facility owners
- **Goals:** Find information about GAHAR, search for certified facilities, validate certificates, read news, contact GAHAR
- **Technical Level:** Low to Medium
- **Devices:** Desktop, Mobile, Tablet
- **Languages:** Arabic (primary), English (secondary)

### 2. **Content Editor**
- **Who:** GAHAR staff responsible for creating and updating content
- **Goals:** Publish news articles, update pages, manage translations, upload images
- **Technical Level:** Medium
- **Permissions:** Create, Edit, View (cannot Publish or Delete)
- **Languages:** Bilingual (Arabic/English)

### 3. **Administrator**
- **Who:** GAHAR IT staff and senior management
- **Goals:** Full control over content, users, settings, and system configuration
- **Technical Level:** High
- **Permissions:** Full access (Create, Edit, Publish, Delete, Manage Users)
- **Languages:** Bilingual (Arabic/English)

### 4. **Super Administrator**
- **Who:** System owner or lead IT manager
- **Goals:** Manage all aspects of the system, including roles, permissions, and critical settings
- **Technical Level:** Very High
- **Permissions:** Unrestricted access
- **Languages:** Bilingual (Arabic/English)

### 5. **Facility Manager**
- **Who:** Healthcare facility administrators
- **Goals:** Check their facility's certification status, download certificates, update facility information
- **Technical Level:** Medium
- **Permissions:** View and manage their own facility data only
- **Languages:** Bilingual (Arabic/English)

---

## 🌐 Epic 1: Public Website Features

### User Story 1.1: Homepage
**As a** public user  
**I want to** visit the GAHAR homepage  
**So that** I can quickly understand what GAHAR is and access key services

**Acceptance Criteria:**
- ✅ Homepage displays a hero banner with GAHAR's mission statement
- ✅ Quick links to main services (Facilities Search, Certificate Validation, News)
- ✅ Latest news section showing 3-6 recent articles
- ✅ Statistics section (e.g., "500+ Certified Facilities", "10,000+ Validated Certificates")
- ✅ Responsive design works on mobile, tablet, and desktop
- ✅ Page loads in under 2 seconds
- ✅ Available in both Arabic and English

**Priority:** P0 (Critical)

---

### User Story 1.2: Browse News Articles
**As a** public user  
**I want to** browse and read news articles  
**So that** I can stay updated on GAHAR's activities and healthcare accreditation news

**Acceptance Criteria:**
- ✅ News listing page shows paginated articles (10 per page)
- ✅ Each article displays: thumbnail image, title, excerpt, publication date
- ✅ Filter by category/tag (e.g., "Announcements", "Events", "Guidelines")
- ✅ Search functionality to find articles by keyword
- ✅ Clicking an article opens the full article page
- ✅ Article page includes: title, featured image, full content, author, date, social sharing buttons
- ✅ Related articles section at the bottom
- ✅ SEO-optimized with unique meta title, description, and OG tags for each article

**Priority:** P0 (Critical)

---

### User Story 1.3: Search for Certified Facilities
**As a** public user  
**I want to** search and browse certified healthcare facilities  
**So that** I can find accredited facilities near me

**Acceptance Criteria:**
- ✅ Facilities page displays an interactive map with facility markers
- ✅ List view option showing facilities in cards/rows
- ✅ Filters: City, Region, Facility Type
- ✅ Search by facility name or code
- ✅ Clicking a marker/facility opens detailed facility page
- ✅ Facility detail page shows: name, address, contact, certification status, certificate expiry date
- ✅ Map auto-zooms to user's location (with permission)
- ✅ Works on mobile devices with touch gestures

**Priority:** P0 (Critical)

---

### User Story 1.4: Validate Certificate
**As a** public user  
**I want to** validate a GAHAR certificate by entering its number  
**So that** I can verify its authenticity

**Acceptance Criteria:**
- ✅ Certificate validation page has a simple input form
- ✅ User enters certificate number (e.g., "CERT-2025-001")
- ✅ System validates in real-time (under 1 second response)
- ✅ If valid: Display success message, certificate details, facility name, issue/expiry dates
- ✅ If invalid/expired: Display error message with clear explanation
- ✅ Option to download certificate PDF (if valid)
- ✅ Multilingual validation messages (Arabic/English)

**Priority:** P0 (Critical)

---

### User Story 1.5: Contact GAHAR
**As a** public user  
**I want to** submit a contact form  
**So that** I can ask questions or provide feedback

**Acceptance Criteria:**
- ✅ Contact page displays a form with fields: Full Name, Email, Phone, Subject, Message
- ✅ All fields are validated (required, email format, phone format)
- ✅ On submit, user sees success confirmation
- ✅ User receives email confirmation of submission
- ✅ Admin receives notification of new submission
- ✅ Form includes CAPTCHA to prevent spam

**Priority:** P1 (High)

---

### User Story 1.6: View Dynamic Pages
**As a** public user  
**I want to** access pages like "About Us", "Services", "FAQs"  
**So that** I can learn more about GAHAR

**Acceptance Criteria:**
- ✅ Pages are created dynamically via Page Builder
- ✅ Each page has unique URL slug (e.g., `/ar/about-us`, `/en/about-us`)
- ✅ Pages support rich content blocks (text, images, videos, accordions)
- ✅ SEO metadata is customizable per page
- ✅ Breadcrumb navigation shows page hierarchy

**Priority:** P1 (High)

---

## 🖊️ Epic 2: Content Management System

### User Story 2.1: Create Content
**As a** content editor  
**I want to** create new content (news, events, services)  
**So that** I can keep the website updated

**Acceptance Criteria:**
- ✅ Admin panel has "Create Content" button
- ✅ Editor selects content type (News, Event, Service, etc.)
- ✅ Form includes: Title, Slug (auto-generated from title), Body (rich text editor), Featured Image, Meta Title, Meta Description
- ✅ Editor can add custom fields defined in content type schema
- ✅ Content is saved as draft by default
- ✅ Editor can save and continue editing or save and exit
- ✅ Validation errors are clearly displayed

**Priority:** P0 (Critical)

---

### User Story 2.2: Edit Existing Content
**As a** content editor  
**I want to** edit existing content  
**So that** I can correct errors or update information

**Acceptance Criteria:**
- ✅ Content listing page shows all content with filters (type, status, date)
- ✅ Click "Edit" to open content in editor
- ✅ All fields are pre-filled with current values
- ✅ Editor can update any field
- ✅ Changes are saved with timestamp and user info
- ✅ Version history is tracked (optional)

**Priority:** P0 (Critical)

---

### User Story 2.3: Manage Translations
**As a** content editor  
**I want to** add and edit content in multiple languages  
**So that** the website is accessible in both Arabic and English

**Acceptance Criteria:**
- ✅ Content editor has language tabs (Arabic / English)
- ✅ Switching tabs shows translation for that language
- ✅ Each translation has independent: Title, Slug, Body, Meta Title, Meta Description
- ✅ Shared fields (e.g., Featured Image, Publish Date) are the same across languages
- ✅ If a translation is missing, user sees placeholder message
- ✅ "Auto-translate" button uses AI to translate content (optional)

**Priority:** P0 (Critical)

---

### User Story 2.4: Publish/Unpublish Content
**As an** administrator  
**I want to** publish or unpublish content  
**So that** I can control what is visible to the public

**Acceptance Criteria:**
- ✅ Content has "Publish" and "Unpublish" buttons
- ✅ Only users with "Publish" permission can publish content
- ✅ Publishing sets `isPublished = true` and `publishedAt = current date`
- ✅ Unpublishing sets `isPublished = false`
- ✅ Published content appears on public website
- ✅ Unpublished content is hidden from public but visible in admin

**Priority:** P0 (Critical)

---

### User Story 2.5: Delete Content
**As an** administrator  
**I want to** delete content  
**So that** I can remove outdated or incorrect information

**Acceptance Criteria:**
- ✅ Delete button is available for content
- ✅ Confirmation dialog appears before deletion
- ✅ Deletion is soft delete (content is archived, not permanently removed)
- ✅ Deleted content can be restored within 30 days
- ✅ Only administrators can delete content

**Priority:** P1 (High)

---

### User Story 2.6: Upload Media Files
**As a** content editor  
**I want to** upload images and files  
**So that** I can use them in content

**Acceptance Criteria:**
- ✅ Media library allows upload via drag-and-drop or file picker
- ✅ Supported formats: JPEG, PNG, GIF, PDF, DOCX
- ✅ Maximum file size: 10MB for images, 50MB for documents
- ✅ Images are automatically optimized and resized
- ✅ Media library shows thumbnails with file name, size, upload date
- ✅ Search and filter media files
- ✅ Copy file URL to clipboard for embedding

**Priority:** P1 (High)

---

## 🎨 Epic 3: Page Builder

### User Story 3.1: Create Page with Page Builder
**As a** content editor  
**I want to** create a new page using a drag-and-drop builder  
**So that** I can design custom layouts without coding

**Acceptance Criteria:**
- ✅ "New Page" button opens Page Builder interface
- ✅ Left panel shows available content blocks (Hero, Text, Image, Video, etc.)
- ✅ Center canvas is empty initially with placeholder message
- ✅ Clicking a block adds it to the canvas
- ✅ Blocks can be dragged to reorder
- ✅ Clicking a block on canvas selects it
- ✅ Right panel shows properties editor for selected block

**Priority:** P0 (Critical)

---

### User Story 3.2: Add and Configure Blocks
**As a** content editor  
**I want to** add blocks and customize their properties  
**So that** I can create rich, dynamic pages

**Acceptance Criteria:**
- ✅ Available blocks: Hero Banner, Latest News, Featured Services, Statistics, Team Members, Contact Form, Map, Custom HTML
- ✅ Each block has configurable properties (e.g., Hero: title, subtitle, background image, CTA button)
- ✅ Properties panel updates in real-time as values change
- ✅ Image fields have "Upload" button to select from media library
- ✅ Changes are reflected in canvas preview immediately

**Priority:** P0 (Critical)

---

### User Story 3.3: Reorder and Delete Blocks
**As a** content editor  
**I want to** reorder or delete blocks  
**So that** I can adjust the page layout

**Acceptance Criteria:**
- ✅ Drag handle on each block allows reordering
- ✅ Smooth animation when blocks are moved
- ✅ Delete icon removes block with confirmation
- ✅ Undo/Redo buttons (optional)

**Priority:** P1 (High)

---

### User Story 3.4: Translate Page Blocks
**As a** content editor  
**I want to** create different versions of page blocks for each language  
**So that** the page is fully localized

**Acceptance Criteria:**
- ✅ Language switcher in Page Builder (Arabic / English)
- ✅ Switching language loads that language's block properties
- ✅ Block structure (order, types) is shared across languages
- ✅ Text properties (titles, descriptions) are per-language
- ✅ Images and links can be different per language (optional)

**Priority:** P0 (Critical)

---

### User Story 3.5: Save and Publish Page
**As a** content editor  
**I want to** save my page as draft or publish it  
**So that** I can control when it goes live

**Acceptance Criteria:**
- ✅ "Save Draft" button saves page without publishing
- ✅ "Publish" button publishes page and makes it live
- ✅ Published pages are accessible via their slug (e.g., `/ar/about-us`)
- ✅ Confirmation message after save/publish
- ✅ Validation ensures required fields are filled

**Priority:** P0 (Critical)

---

### User Story 3.6: Preview Page
**As a** content editor  
**I want to** preview the page before publishing  
**So that** I can see how it will look to public users

**Acceptance Criteria:**
- ✅ "Preview" button opens page in new tab
- ✅ Preview URL is temporary and not indexed by search engines
- ✅ Preview shows exact public layout with real data
- ✅ Preview is available for both languages

**Priority:** P1 (High)

---

## 🏥 Epic 4: Facilities Management

### User Story 4.1: Upload Facilities Excel File
**As an** administrator  
**I want to** upload an Excel file with facility data  
**So that** I can bulk import certified facilities

**Acceptance Criteria:**
- ✅ Upload page accepts .xlsx files
- ✅ Excel template is provided with required columns: Facility Code, Name (AR), Name (EN), City (AR), City (EN), Latitude, Longitude
- ✅ On upload, system validates file format and columns
- ✅ If valid, file is processed in background (Hangfire job)
- ✅ User sees progress indicator and estimated time
- ✅ After processing, user sees summary: X facilities added, Y updated, Z errors
- ✅ Error report is downloadable

**Priority:** P0 (Critical)

---

### User Story 4.2: View Facilities List
**As an** administrator  
**I want to** view all facilities in a data table  
**So that** I can manage and review facility information

**Acceptance Criteria:**
- ✅ Facilities list page shows paginated table
- ✅ Columns: Facility Code, Name, City, Status, Last Updated
- ✅ Search by name or code
- ✅ Filter by city, region, status
- ✅ Sort by any column
- ✅ Export to Excel button

**Priority:** P1 (High)

---

### User Story 4.3: Edit Facility Details
**As an** administrator  
**I want to** edit a facility's information  
**So that** I can correct errors or update details

**Acceptance Criteria:**
- ✅ Click "Edit" on facility row
- ✅ Edit form shows: Facility Code, Names (AR/EN), Address, City, Coordinates
- ✅ Changes are validated and saved
- ✅ Audit log records who made changes and when

**Priority:** P1 (High)

---

### User Story 4.4: View Facility on Map
**As a** public user or administrator  
**I want to** see facilities on an interactive map  
**So that** I can find facilities near a location

**Acceptance Criteria:**
- ✅ Map displays all facilities as markers
- ✅ Clicking marker shows popup with facility name and link
- ✅ Filters apply to map markers
- ✅ User can zoom and pan
- ✅ Mobile-friendly with touch support

**Priority:** P0 (Critical)

---

## 🎓 Epic 5: Certificate Validation

### User Story 5.1: Validate Certificate by Number
**As a** public user  
**I want to** enter a certificate number and check if it's valid  
**So that** I can verify a facility's accreditation

**Acceptance Criteria:**
- ✅ Input field accepts certificate number
- ✅ "Validate" button triggers check
- ✅ System responds in under 1 second
- ✅ If valid: Show green badge, facility name, issue/expiry dates
- ✅ If expired: Show orange badge, expiry date
- ✅ If invalid: Show red badge, "Certificate not found" message
- ✅ Multilingual messages (Arabic/English)

**Priority:** P0 (Critical)

---

### User Story 5.2: Download Certificate PDF
**As a** public user  
**I want to** download the certificate as a PDF  
**So that** I can save it for my records

**Acceptance Criteria:**
- ✅ "Download PDF" button appears for valid certificates
- ✅ PDF includes: Certificate number, facility name, issue/expiry dates, QR code
- ✅ PDF is professionally formatted with GAHAR branding
- ✅ QR code links to validation page

**Priority:** P1 (High)

---

### User Story 5.3: Generate Certificate (Admin)
**As an** administrator  
**I want to** generate a new certificate for a facility  
**So that** I can issue accreditation

**Acceptance Criteria:**
- ✅ Certificate form includes: Facility (dropdown), Issue Date, Expiry Date
- ✅ Certificate number is auto-generated (format: CERT-YYYY-NNN)
- ✅ Certificate is saved in database
- ✅ PDF is generated and stored
- ✅ Facility is notified via email

**Priority:** P1 (High)

---

## 📝 Epic 6: Forms & Submissions (Advanced Form Builder)

### User Story 6.1: Create Form with Drag & Drop Builder
**As an** administrator  
**I want to** create custom forms using a drag-and-drop builder  
**So that** I can collect specific information from users without coding

**Acceptance Criteria:**
- ✅ Form builder has three panels: Field Types (left), Canvas (center), Properties (right)
- ✅ Available field types: Text, Email, Number, Date, Textarea, Dropdown (Select), Checkbox, Radio, File Upload
- ✅ Clicking a field type adds it to the canvas
- ✅ Fields can be dragged to reorder
- ✅ Form has name and description
- ✅ Save as draft or publish immediately
- ✅ Visual preview shows how form looks to end users

**Priority:** P0 (Critical)

---

### User Story 6.2: Configure Field Properties
**As an** administrator  
**I want to** configure each field's properties  
**So that** I can customize the form to my needs

**Acceptance Criteria:**
- ✅ Clicking a field in canvas selects it and shows properties panel
- ✅ Basic properties editable:
  - Field Name (technical identifier, no spaces)
  - Label (displayed to users)
  - Placeholder text
  - Help text (additional instructions)
  - Required toggle
  - Width (25%, 50%, 75%, 100%) for multi-column layouts
- ✅ Field-specific options:
  - **Dropdown/Radio/Checkbox:** Add/edit/delete options (label + value)
  - **File Upload:** Max file size (MB), allowed file types
  - **Text/Textarea:** Min/max character length
  - **Number:** Min/max value, step increment
- ✅ Changes reflect immediately in canvas preview

**Priority:** P0 (Critical)

---

### User Story 6.3: Add Validation Rules
**As an** administrator  
**I want to** add multiple validation rules to each field  
**So that** I can ensure data quality

**Acceptance Criteria:**
- ✅ Validation tab in properties panel
- ✅ "Add Validation" button creates new rule
- ✅ Available validation types:
  - **Required:** Field must be filled
  - **Min Length:** Minimum number of characters
  - **Max Length:** Maximum number of characters
  - **Min Value:** Minimum number (for number fields)
  - **Max Value:** Maximum number (for number fields)
  - **Email:** Valid email format
  - **URL:** Valid URL format
  - **Pattern (Regex):** Custom regular expression
  - **File Size:** Max file size in MB
  - **File Type:** Allowed file extensions
- ✅ Each rule has custom error message in Arabic and English
- ✅ Multiple rules can be applied to one field
- ✅ Rules can be deleted individually
- ✅ Test validation button to verify rules work

**Examples:**
```
Field: "National ID"
Validation 1: Pattern: ^[0-9]{10}$
Error: "رقم الهوية يجب أن يكون 10 أرقام" / "National ID must be 10 digits"

Field: "Full Name"
Validation 1: Required
Error: "هذا الحقل مطلوب" / "This field is required"
Validation 2: Min Length: 3
Error: "الاسم يجب أن يكون 3 أحرف على الأقل" / "Name must be at least 3 characters"

Field: "Attachment"
Validation 1: File Size: 5 MB
Error: "حجم الملف يجب ألا يتجاوز 5 ميجابايت" / "File size must not exceed 5MB"
Validation 2: File Type: pdf, jpg, png
Error: "الملفات المسموحة: PDF, JPG, PNG" / "Allowed files: PDF, JPG, PNG"
```

**Priority:** P0 (Critical)

---

### User Story 6.4: Add Conditional Logic (Show/Hide Fields)
**As an** administrator  
**I want to** show or hide fields based on other field values  
**So that** I can create dynamic, context-aware forms

**Acceptance Criteria:**
- ✅ Conditional Logic tab in properties panel
- ✅ "Add Logic" button creates new conditional rule
- ✅ Conditional rule configuration:
  - **Trigger Field:** Select which field triggers the condition
  - **Condition:** Select comparison (equals, not equals, contains, greater than, less than)
  - **Value:** Enter value to compare against
  - **Action:** Show or Hide current field
- ✅ Multiple logic rules can be added (AND/OR logic)
- ✅ Conditional logic works in preview mode
- ✅ Nested conditions supported (field A → field B → field C)

**Examples:**
```
Scenario 1: Citizenship-based fields
Field: "رقم الهوية الوطنية" (National ID)
Trigger: "الجنسية" (Citizenship)
Condition: equals
Value: "سعودي"
Action: Show
→ National ID field only appears when citizenship = "سعودي"

Scenario 2: Conditional file upload
Field: "رخصة المنشأة" (Facility License)
Trigger: "نوع المنشأة" (Facility Type)
Condition: equals
Value: "مستشفى"
Action: Show
→ License upload only required for hospitals

Scenario 3: Age-based questions
Field: "ولي الأمر" (Guardian Name)
Trigger: "العمر" (Age)
Condition: lessThan
Value: "18"
Action: Show
→ Guardian field appears for minors
```

**Priority:** P1 (High)

---

### User Story 6.5: Preview and Test Form
**As an** administrator  
**I want to** preview the form before publishing  
**So that** I can ensure it works correctly

**Acceptance Criteria:**
- ✅ "Preview" button opens form in modal or new tab
- ✅ Preview shows exact user-facing layout
- ✅ All field types render correctly
- ✅ Multi-column layout displays properly
- ✅ "Test Validations" mode allows filling form and checking validation errors
- ✅ Conditional logic works in preview (fields show/hide correctly)
- ✅ Preview available in both Arabic and English (if form is multilingual)
- ✅ Mobile and desktop preview modes

**Priority:** P1 (High)

---

### User Story 6.6: Reorder Fields
**As an** administrator  
**I want to** reorder form fields by dragging  
**So that** I can organize the form logically

**Acceptance Criteria:**
- ✅ Drag handle visible on each field in canvas
- ✅ Smooth drag-and-drop animation
- ✅ Drop zones highlighted when dragging
- ✅ Field order updates immediately
- ✅ Display order saved when form is saved

**Priority:** P1 (High)

---

### User Story 6.7: Delete Fields
**As an** administrator  
**I want to** delete fields from the form  
**So that** I can remove unnecessary fields

**Acceptance Criteria:**
- ✅ Delete button on each field in canvas
- ✅ Confirmation dialog before deletion
- ✅ Deleted field is removed from canvas and properties
- ✅ If deleted field is referenced in conditional logic, warning is shown
- ✅ Conditional logic referencing deleted field is auto-removed

**Priority:** P1 (High)

---

### User Story 6.8: Duplicate Form
**As an** administrator  
**I want to** duplicate an existing form  
**So that** I can create similar forms quickly

**Acceptance Criteria:**
- ✅ "Duplicate" button on forms list page
- ✅ Duplicate creates copy with all fields, validations, and conditional logic
- ✅ Duplicated form name is "Form Name (Copy)"
- ✅ Duplicated form is saved as draft (not published)
- ✅ Confirmation message after duplication

**Priority:** P2 (Medium)

---

### User Story 6.9: Multi-Column Layout
**As an** administrator  
**I want to** set field widths to create multi-column layouts  
**So that** I can save vertical space and group related fields

**Acceptance Criteria:**
- ✅ Width property for each field: 25%, 50%, 75%, 100%
- ✅ Fields with combined width ≤ 100% appear on same row
- ✅ Fields with combined width > 100% wrap to next row
- ✅ Preview accurately shows multi-column layout
- ✅ Responsive: columns stack on mobile devices

**Example:**
```
Row 1: [First Name 50%] [Last Name 50%]
Row 2: [Email 100%]
Row 3: [City 33%] [ZIP Code 33%] [Country 33%]
```

**Priority:** P2 (Medium)

---

### User Story 6.10: Submit Form (Public User)
**As a** public user  
**I want to** fill and submit a form  
**So that** I can contact GAHAR or provide information

**Acceptance Criteria:**
- ✅ Form is rendered dynamically based on schema from backend
- ✅ All field types display correctly
- ✅ Validation works on client-side (real-time feedback)
- ✅ Required fields are marked with asterisk (*)
- ✅ Validation errors appear below fields in red
- ✅ Conditional logic works (fields show/hide based on user input)
- ✅ File upload field allows drag-and-drop or click to browse
- ✅ Progress indicator shown during file upload
- ✅ On submit:
  - Form data is validated on server-side
  - If valid: Success message + email confirmation to user
  - If invalid: Errors displayed on form
- ✅ Submit button disabled during submission to prevent double-submit
- ✅ CAPTCHA verification (reCAPTCHA v3)

**Priority:** P0 (Critical)

---

### User Story 6.11: View Form Submissions
**As an** administrator  
**I want to** view all submissions for a form  
**So that** I can review and respond to inquiries

**Acceptance Criteria:**
- ✅ Submissions page shows table with: Submitter Name, Email, Submission Date, Status
- ✅ Click submission row to view full details
- ✅ Submission details show all field values
- ✅ File attachments are downloadable
- ✅ Filter by status (Pending, Reviewed, Processed)
- ✅ Filter by date range
- ✅ Search by submitter name or email
- ✅ Pagination (20 submissions per page)
- ✅ Export submissions to Excel (all fields + metadata)

**Priority:** P0 (Critical)

---

### User Story 6.12: Update Submission Status
**As an** administrator  
**I want to** mark submissions as Reviewed or Processed  
**So that** I can track which submissions have been handled

**Acceptance Criteria:**
- ✅ Status dropdown in submission detail view
- ✅ Available statuses: Pending, Reviewed, Processed, Rejected
- ✅ Status change is saved immediately
- ✅ Audit log records who changed status and when
- ✅ Optional: Add internal notes to submission

**Priority:** P1 (High)

---

### User Story 6.13: Email Notifications for Submissions
**As an** administrator  
**I want to** receive email notifications for new submissions  
**So that** I can respond quickly

**Acceptance Criteria:**
- ✅ When form is submitted, admin receives email notification
- ✅ Email includes: Form name, Submitter name, Submission date, Link to view submission
- ✅ Email recipients configurable per form (multiple recipients allowed)
- ✅ User receives auto-reply email confirming submission received

**Priority:** P1 (High)

---

### User Story 6.14: Real-time Notifications (SignalR)
**As an** administrator viewing the dashboard  
**I want to** see real-time notifications when forms are submitted  
**So that** I'm immediately aware of new submissions

**Acceptance Criteria:**
- ✅ Toast notification appears in admin panel when new submission arrives
- ✅ Notification shows: Form name, Submitter name
- ✅ Click notification to open submission details
- ✅ Notification badge on "Forms" menu item shows unread count
- ✅ Notifications are specific to logged-in user (not global)

**Priority:** P2 (Medium)

---

### User Story 6.15: Form Analytics
**As an** administrator  
**I want to** see analytics for each form  
**So that** I can understand form performance

**Acceptance Criteria:**
- ✅ Analytics page for each form shows:
  - Total submissions
  - Submissions over time (chart)
  - Average completion time
  - Abandonment rate (started but not submitted)
  - Most common validation errors
- ✅ Date range filter
- ✅ Export analytics report

**Priority:** P3 (Low - Future Enhancement)

---

### User Story 6.16: Multilingual Forms
**As an** administrator  
**I want to** create forms in both Arabic and English  
**So that** users can fill forms in their preferred language

**Acceptance Criteria:**
- ✅ Form builder has language switcher (AR/EN)
- ✅ Each field's label, placeholder, help text, and error messages are translatable
- ✅ Switching language in builder shows that language's text
- ✅ Form options (dropdown values) are multilingual
- ✅ Public form displays in user's selected language
- ✅ Submissions are stored with language metadata

**Priority:** P1 (High)

---

### User Story 6.17: Form Embedding
**As an** administrator  
**I want to** embed forms on any page  
**So that** forms can be placed contextually

**Acceptance Criteria:**
- ✅ Each form has an embed code (iframe or React component)
- ✅ Forms can be embedded in CMS pages via Page Builder block
- ✅ Embedded forms are responsive
- ✅ Embedded forms submit to same backend endpoint
- ✅ Forms can be embedded on external websites (CORS configured)

**Priority:** P2 (Medium)

---

## 📊 Form Builder Feature Summary

### Supported Field Types:
1. **Text** - Single-line text input
2. **Email** - Email input with validation
3. **Number** - Numeric input with min/max
4. **Date** - Date picker
5. **Textarea** - Multi-line text
6. **Dropdown (Select)** - Single choice from options
7. **Checkbox** - Multiple choices
8. **Radio** - Single choice (radio buttons)
9. **File Upload** - File attachment with size/type validation

### Supported Validations:
1. Required
2. Min Length / Max Length
3. Min Value / Max Value (numbers)
4. Email Format
5. URL Format
6. Regex Pattern
7. File Size
8. File Type

### Conditional Logic Operators:
1. Equals
2. Not Equals
3. Contains
4. Greater Than
5. Less Than

### Form Actions:
1. Show Field
2. Hide Field

---

## 🔐 Epic 7: User Management & Authentication

### User Story 7.1: Register User Account
**As an** administrator  
**I want to** create user accounts for editors and admins  
**So that** they can access the admin panel

**Acceptance Criteria:**
- ✅ User creation form: Email, Full Name, Password, Role (Editor/Admin/Super Admin)
- ✅ Email is unique and validated
- ✅ Password must meet complexity requirements (min 8 chars, uppercase, number, symbol)
- ✅ User receives welcome email with login link

**Priority:** P0 (Critical)

---

### User Story 7.2: Login to Admin Panel
**As a** registered user  
**I want to** log in with my email and password  
**So that** I can access the admin panel

**Acceptance Criteria:**
- ✅ Login page has email and password fields
- ✅ "Forgot Password" link for password recovery
- ✅ On successful login, user is redirected to dashboard
- ✅ JWT token is issued and stored securely
- ✅ Failed login shows error message (max 5 attempts before lockout)

**Priority:** P0 (Critical)

---

### User Story 7.3: Logout
**As a** logged-in user  
**I want to** log out  
**So that** I can secure my session

**Acceptance Criteria:**
- ✅ Logout button in header
- ✅ On logout, JWT token is revoked
- ✅ User is redirected to login page

**Priority:** P0 (Critical)

---

### User Story 7.4: Manage User Roles
**As a** super administrator  
**I want to** assign and update user roles  
**So that** I can control access permissions

**Acceptance Criteria:**
- ✅ Roles: Super Admin, Admin, Editor, Viewer
- ✅ Permissions matrix clearly defines what each role can do
- ✅ Users page allows changing user role
- ✅ Role changes take effect immediately

**Priority:** P1 (High)

**Permissions Matrix:**
| Action | Super Admin | Admin | Editor | Viewer |
|--------|-------------|-------|--------|--------|
| Create Content | ✅ | ✅ | ✅ | ❌ |
| Edit Content | ✅ | ✅ | ✅ | ❌ |
| Publish Content | ✅ | ✅ | ❌ | ❌ |
| Delete Content | ✅ | ✅ | ❌ | ❌ |
| Manage Users | ✅ | ❌ | ❌ | ❌ |
| Manage Settings | ✅ | ❌ | ❌ | ❌ |
| View Reports | ✅ | ✅ | ✅ | ✅ |

---

### User Story 7.5: Password Reset
**As a** user who forgot their password  
**I want to** reset it via email  
**So that** I can regain access

**Acceptance Criteria:**
- ✅ "Forgot Password" link on login page
- ✅ User enters email address
- ✅ If email exists, reset link is sent
- ✅ Reset link is valid for 1 hour
- ✅ User clicks link, enters new password
- ✅ Password is updated and user can log in

**Priority:** P1 (High)

---

## 🌍 Epic 8: Multilingual Support

### User Story 8.1: Switch Language
**As a** public user  
**I want to** switch between Arabic and English  
**So that** I can view the website in my preferred language

**Acceptance Criteria:**
- ✅ Language switcher in header (AR / EN)
- ✅ Clicking switches language and reloads page
- ✅ URL changes to reflect language (e.g., `/ar/news` → `/en/news`)
- ✅ Language preference is remembered (cookie/localStorage)
- ✅ All UI elements, labels, and messages are translated

**Priority:** P0 (Critical)

---

### User Story 8.2: RTL Support
**As a** public user viewing Arabic content  
**I want to** see the layout in right-to-left format  
**So that** the experience is natural for Arabic readers

**Acceptance Criteria:**
- ✅ Arabic pages use `dir="rtl"` attribute
- ✅ Navigation, text, and layout flow from right to left
- ✅ Icons and images are mirrored appropriately
- ✅ Forms and inputs align correctly
- ✅ No overlapping or broken layouts

**Priority:** P0 (Critical)

---

### User Story 8.3: Translate Admin Panel
**As a** bilingual editor  
**I want to** use the admin panel in Arabic or English  
**So that** I can work in my preferred language

**Acceptance Criteria:**
- ✅ Admin panel has language switcher
- ✅ All menus, buttons, labels, and messages are translated
- ✅ RTL layout for Arabic admin panel
- ✅ Language preference is saved per user

**Priority:** P1 (High)

---

## 📊 Epic 9: Analytics & Reporting

### User Story 9.1: View Dashboard Analytics
**As an** administrator  
**I want to** see key metrics on the dashboard  
**So that** I can monitor website performance

**Acceptance Criteria:**
- ✅ Dashboard shows cards with: Total Content, Total Facilities, Total Certificates, Form Submissions (this month)
- ✅ Charts: Content published over time, Form submissions over time, Top visited pages
- ✅ Recent activity feed: Latest content, submissions, user actions

**Priority:** P2 (Medium)

---

### User Story 9.2: Generate Reports
**As an** administrator  
**I want to** generate reports on content, facilities, and submissions  
**So that** I can analyze data and make decisions

**Acceptance Criteria:**
- ✅ Reports page allows selecting: Report Type (Content, Facilities, Submissions), Date Range, Filters
- ✅ Reports can be viewed on screen or exported to Excel/PDF
- ✅ Scheduled reports (daily/weekly) via email (optional)

**Priority:** P2 (Medium)

---

### User Story 9.3: Audit Logs
**As a** super administrator  
**I want to** view audit logs of all system actions  
**So that** I can track changes and ensure accountability

**Acceptance Criteria:**
- ✅ Audit log page shows: User, Action, Entity Type, Entity ID, Timestamp, IP Address
- ✅ Filter by user, action type, date range
- ✅ Export audit logs to CSV
- ✅ Logs are immutable and cannot be deleted

**Priority:** P2 (Medium)

---

## � Epic 10: Albums & Photo Galleries

### User Story 10.1: Create Album
**As an** administrator  
**I want to** create a new photo album  
**So that** I can organize and display event photos

**Acceptance Criteria:**
- ✅ "New Album" button in Albums section
- ✅ Album creation form: Title (AR/EN), Description (AR/EN), Slug (auto-generated)
- ✅ Album is created as draft
- ✅ Redirect to album editor after creation

**Priority:** P1 (High)

---

### User Story 10.2: Bulk Upload Images
**As an** administrator  
**I want to** upload multiple images at once  
**So that** I can quickly populate an album

**Acceptance Criteria:**
- ✅ Drag & drop zone for multiple images
- ✅ Click to browse and select multiple files
- ✅ Supported formats: JPG, PNG, GIF, WebP
- ✅ Max file size: 10MB per image
- ✅ Max upload: 50 images per batch
- ✅ Image preview grid before upload
- ✅ Individual file validation (show errors for invalid files)
- ✅ "Upload All" button to start batch upload
- ✅ **Real-time progress bar** showing:
  - Overall progress percentage
  - Current file being processed
  - Number of files completed/failed
- ✅ Success/error status for each image
- ✅ Background job processes images:
  - Extract dimensions (width, height)
  - Calculate aspect ratio
  - Generate thumbnail (300x300)
  - Optimize and compress
  - Upload to CDN/Storage
  - Save to database

**Priority:** P0 (Critical)

---

### User Story 10.3: View Album Images Grid
**As an** administrator  
**I want to** see all images in an album as a grid  
**So that** I can manage them

**Acceptance Criteria:**
- ✅ Images displayed in responsive grid (6 columns on desktop, 4 on tablet, 2 on mobile)
- ✅ Each image shows thumbnail
- ✅ Image count displayed
- ✅ Load more / pagination for large albums

**Priority:** P1 (High)

---

### User Story 10.4: Reorder Album Images
**As an** administrator  
**I want to** reorder images by dragging  
**So that** I can control the display sequence

**Acceptance Criteria:**
- ✅ Drag handle on each image
- ✅ Smooth drag-and-drop animation
- ✅ Visual feedback while dragging
- ✅ "Save Order" button
- ✅ Order is saved in `displayOrder` field
- ✅ Confirmation message after save

**Priority:** P1 (High)

---

### User Story 10.5: Set Album Cover Image
**As an** administrator  
**I want to** set one image as the album cover  
**So that** it appears in the albums list

**Acceptance Criteria:**
- ✅ "Set as Cover" option on each image
- ✅ Current cover image is highlighted
- ✅ Cover image appears on album card in albums list
- ✅ If no cover is set, first image is used as default

**Priority:** P1 (High)

---

### User Story 10.6: Edit Image Captions
**As an** administrator  
**I want to** add captions to images in multiple languages  
**So that** users can understand the context

**Acceptance Criteria:**
- ✅ Click image to open caption editor
- ✅ Language tabs (AR/EN)
- ✅ Caption field (max 1000 characters)
- ✅ Alt text field for SEO and accessibility
- ✅ Changes saved immediately or on "Save" button
- ✅ Caption appears on hover in public view

**Priority:** P1 (High)

---

### User Story 10.7: Delete Images
**As an** administrator  
**I want to** delete images from an album  
**So that** I can remove unwanted photos

**Acceptance Criteria:**
- ✅ Delete button on each image
- ✅ Confirmation dialog before deletion
- ✅ Image is removed from database and storage
- ✅ If deleted image was cover, cover is reset to first image
- ✅ Success message after deletion

**Priority:** P1 (High)

---

### User Story 10.8: Publish Album
**As an** administrator  
**I want to** publish an album  
**So that** it becomes visible to the public

**Acceptance Criteria:**
- ✅ "Publish" button in album editor
- ✅ Published albums appear on public albums page
- ✅ Unpublished albums are hidden from public
- ✅ Admin can unpublish at any time

**Priority:** P1 (High)

---

### User Story 10.9: Browse Albums (Public)
**As a** public user  
**I want to** browse photo albums  
**So that** I can see GAHAR events and activities

**Acceptance Criteria:**
- ✅ Albums page shows grid of album cards
- ✅ Each card shows: Cover image, Title, Description, Image count
- ✅ Pagination (12 albums per page)
- ✅ Filter by date (newest first / oldest first)
- ✅ Click album to view full gallery

**Priority:** P1 (High)

---

### User Story 10.10: View Album with Dynamic Collage Layout
**As a** public user  
**I want to** view album photos in an attractive collage layout  
**So that** the gallery is visually appealing

**Acceptance Criteria:**
- ✅ **Dynamic Collage Algorithm:**
  - Groups images into balanced rows
  - Mixes landscape, portrait, and square images naturally
  - No gaps between images
  - Rows have similar heights (±30% variance)
  - Algorithm considers aspect ratios to optimize layout
- ✅ Images are lazy-loaded
- ✅ Responsive layout:
  - Desktop: Collage layout with dynamic rows
  - Mobile: Simple 2-column grid
- ✅ Click image to open lightbox
- ✅ Image captions visible on hover (desktop) or below image (mobile)

**Algorithm Details:**
```
For each image in album:
  1. Calculate aspect ratio (width / height)
  2. Add to current row
  3. If row total aspect ratio ≥ target:
     - Check row height would be within acceptable range
     - If yes: commit row and start new one
     - If no: continue adding images
  4. Last row: force commit even if not full
  
Target row aspect ratio = container width / target row height
Min row height = 70% of target
Max row height = 150% of target
```

**Priority:** P0 (Critical)

---

### User Story 10.11: Lightbox Image Viewer
**As a** public user  
**I want to** view images in full screen  
**So that** I can see details clearly

**Acceptance Criteria:**
- ✅ Click image to open lightbox
- ✅ Lightbox shows full-resolution image
- ✅ Image caption displayed at bottom
- ✅ Navigation:
  - Previous/Next buttons
  - Keyboard arrows (← →)
  - Escape key to close
- ✅ Image counter (e.g., "5 / 25")
- ✅ Download button (downloads original image)
- ✅ Share button (Web Share API)
- ✅ Close button (X)
- ✅ Click outside image to close
- ✅ Smooth transitions between images
- ✅ Touch gestures on mobile (swipe left/right)

**Priority:** P0 (Critical)

---

### User Story 10.12: SEO for Albums
**As a** content manager  
**I want** albums to be SEO-optimized  
**So that** they rank well in search engines

**Acceptance Criteria:**
- ✅ Each album has unique meta title and description (AR/EN)
- ✅ Album URL slug is SEO-friendly (e.g., `/ar/albums/ma3rad-sowar-2025`)
- ✅ Image alt text is used for SEO
- ✅ Structured data (schema.org/ImageGallery) for albums
- ✅ Open Graph tags for social sharing
- ✅ Albums appear in sitemap

**Priority:** P1 (High)

---

### User Story 10.13: Move Content Between Sections
**As an** administrator  
**I want to** move content from one content type to another  
**So that** I can correct categorization mistakes

**Acceptance Criteria:**
- ✅ "Move" button in content editor
- ✅ Dropdown to select target content type (e.g., move from "news" to "events")
- ✅ Confirmation dialog showing what will happen
- ✅ Content ID remains the same
- ✅ All translations are preserved
- ✅ Compatible custom fields are migrated
- ✅ Incompatible fields are logged/reported
- ✅ Audit log records the move action
- ✅ Success message after move
- ✅ Content appears in new section immediately

**Example:**
```
Article was created in "news" by mistake
Admin clicks "Move" → Selects "events" → Confirms
Article now appears under "events" with all data intact
```

**Priority:** P1 (High)

---

### User Story 10.14: Album Analytics
**As an** administrator  
**I want to** see analytics for each album  
**So that** I know which albums are most popular

**Acceptance Criteria:**
- ✅ Album analytics page shows:
  - Total views
  - Views per image
  - Most viewed images
  - View count over time (chart)
  - Downloads count
- ✅ Date range filter
- ✅ Export analytics to Excel

**Priority:** P3 (Low - Future Enhancement)

---

### User Story 10.15: Duplicate Album
**As an** administrator  
**I want to** duplicate an existing album  
**So that** I can create similar albums quickly

**Acceptance Criteria:**
- ✅ "Duplicate" button on album
- ✅ Creates copy with same images and settings
- ✅ Duplicated album name is "Album Name (Copy)"
- ✅ Duplicated album is draft (not published)
- ✅ Admin can edit before publishing

**Priority:** P2 (Medium)

---

## 📊 Albums Feature Summary

### Backend Features:
- ✅ Albums table with multilingual support
- ✅ AlbumImages table with aspect ratio calculation
- ✅ Bulk upload endpoint with background job
- ✅ Progress tracking API
- ✅ Image optimization and thumbnail generation
- ✅ CDN integration for fast delivery
- ✅ Reorder images API
- ✅ Set cover image API

### Frontend Features:
- ✅ Drag & drop bulk uploader with progress bar
- ✅ **Dynamic Collage Layout Algorithm:**
  - Balances aspect ratios across rows
  - Handles landscape, portrait, square images
  - No gaps, visually pleasing layout
  - Responsive (desktop: collage, mobile: grid)
- ✅ Full-screen lightbox with navigation
- ✅ Reorder images (drag & drop)
- ✅ Edit captions (multilingual)
- ✅ SEO optimization

### Supported Image Formats:
- JPG / JPEG
- PNG
- GIF
- WebP

### Image Processing:
1. Validate file type and size
2. Extract dimensions (width x height)
3. Calculate aspect ratio (width / height)
4. Generate thumbnail (300x300)
5. Optimize and compress (reduce file size)
6. Upload to CDN / Storage
7. Save metadata to database

---

### Performance
- **NFR-1:** Homepage loads in under 2 seconds on 3G connection
- **NFR-2:** API responses for certificate validation under 500ms
- **NFR-3:** Admin panel supports 50+ concurrent users
- **NFR-4:** Database queries optimized with indexing
- **NFR-5:** Image assets optimized (WebP format, lazy loading)

### Security
- **NFR-6:** All API endpoints use JWT authentication
- **NFR-7:** Passwords hashed with bcrypt (minimum 12 rounds)
- **NFR-8:** HTTPS enforced on production
- **NFR-9:** CSRF protection on all forms
- **NFR-10:** SQL injection prevention via parameterized queries
- **NFR-11:** XSS protection (sanitize user input)
- **NFR-12:** Rate limiting on public APIs (100 requests/minute per IP)

### Accessibility
- **NFR-13:** WCAG 2.1 AA compliance
- **NFR-14:** Keyboard navigation support
- **NFR-15:** Screen reader compatible
- **NFR-16:** Color contrast ratio min 4.5:1
- **NFR-17:** All images have alt text

### SEO
- **NFR-18:** Unique meta title and description for each page
- **NFR-19:** Structured data (JSON-LD) for news articles and organization
- **NFR-20:** XML sitemaps for each language
- **NFR-21:** Canonical URLs to prevent duplicate content
- **NFR-22:** Hreflang tags for multilingual pages
- **NFR-23:** robots.txt properly configured

### Scalability
- **NFR-24:** System supports 100,000+ content items
- **NFR-25:** System supports 50,000+ facilities
- **NFR-26:** Database can scale horizontally
- **NFR-27:** Caching reduces database load (Redis)
- **NFR-28:** CDN for static assets

### Backup & Recovery
- **NFR-29:** Daily automated database backups
- **NFR-30:** Backups retained for 30 days
- **NFR-31:** Disaster recovery plan documented
- **NFR-32:** Recovery time objective (RTO): 4 hours
- **NFR-33:** Recovery point objective (RPO): 24 hours

---

## 📈 Priority Definitions

- **P0 (Critical):** Must-have for MVP launch
- **P1 (High):** Important for full launch
- **P2 (Medium):** Nice-to-have, can be added post-launch
- **P3 (Low):** Future enhancement

---

## ✅ Acceptance Testing Checklist

For each user story:
- [ ] Acceptance criteria are met
- [ ] Unit tests written and passing
- [ ] Integration tests passing
- [ ] Manual testing completed
- [ ] Accessibility testing passed
- [ ] Security review completed (for auth/data flows)
- [ ] Performance benchmarks met
- [ ] Reviewed by Product Owner
- [ ] Documentation updated

---

## 🎯 Definition of Done

A user story is considered DONE when:
1. ✅ All acceptance criteria are met
2. ✅ Code is peer-reviewed and merged
3. ✅ Tests (unit + integration) are written and passing
4. ✅ Feature is deployed to staging environment
5. ✅ QA testing is completed and approved
6. ✅ Product Owner has accepted the feature
7. ✅ Documentation is updated (user guide, API docs)
8. ✅ No critical bugs remaining

---

## 🎨 Epic 11: Multi-Layout System & WebP Optimization

### User Story 11.1: Create Custom Layout
**As an** administrator  
**I want to** create custom layouts for content display  
**So that** I can present content in different visual styles

**Acceptance Criteria:**
- ✅ Layout Builder page accessible from admin panel
- ✅ Visual drag & drop interface for building layouts
- ✅ Left panel shows available elements:
  - Container (group elements)
  - Text Field (title, body, etc.)
  - Image Field (featured image)
  - Date Field (published date)
  - Array Field (tags, categories)
  - 2-Column Grid
  - Divider
- ✅ Center canvas shows live preview of layout
- ✅ Click element to add to canvas
- ✅ Drag elements to reorder
- ✅ Layout name and display name fields
- ✅ Optional: assign to specific content type (news, events, etc.)
- ✅ Save layout button

**Priority:** P1 (High)

---

### User Story 11.2: Configure Element Properties
**As an** administrator  
**I want to** configure properties of each element in the layout  
**So that** I can customize the appearance

**Acceptance Criteria:**
- ✅ Click element in canvas to select it
- ✅ Right panel shows element properties:
  - Field mapping (which data field to display)
  - HTML tag (h1, h2, p, div, span, etc.)
  - CSS classes (Tailwind classes)
  - Prefix/suffix text
- ✅ Properties update live in preview
- ✅ Delete element button
- ✅ Duplicate element button

**Priority:** P1 (High)

---

### User Story 11.3: Select Layout for Content
**As an** administrator editing content  
**I want to** choose which layout to use  
**So that** the content is displayed with the desired style

**Acceptance Criteria:**
- ✅ Layout Selector component in content editor sidebar
- ✅ Shows all available layouts for this content type
- ✅ Each layout card shows:
  - Preview image
  - Display name
  - Description
  - "Default" badge if default layout
- ✅ Click layout card to select it
- ✅ Selected layout is highlighted
- ✅ Link to create new layout
- ✅ Layout selection is saved with content

**Priority:** P0 (Critical)

---

### User Story 11.4: Render Content with Selected Layout
**As a** public user  
**I want to** see content displayed beautifully  
**So that** the reading experience is pleasant

**Acceptance Criteria:**
- ✅ Frontend LayoutRenderer component renders content based on layout structure
- ✅ All element types are supported:
  - Text fields render as specified HTML tag
  - Images render with WebP support
  - Dates are formatted correctly (Arabic/English)
  - Arrays (tags) render as specified
  - Containers group elements correctly
- ✅ CSS classes are applied correctly
- ✅ Layout is responsive (mobile/tablet/desktop)
- ✅ If no layout selected, use default layout

**Priority:** P0 (Critical)

---

### User Story 11.5: Duplicate Existing Layout
**As an** administrator  
**I want to** duplicate an existing layout  
**So that** I can create variations quickly

**Acceptance Criteria:**
- ✅ "Duplicate" button on layout list
- ✅ Creates exact copy with " (Copy)" appended to name
- ✅ Opens copy in layout builder for editing
- ✅ Confirmation message after duplication

**Priority:** P2 (Medium)

---

### User Story 11.6: Set Default Layout
**As an** administrator  
**I want to** set a default layout for each content type  
**So that** new content automatically uses the standard layout

**Acceptance Criteria:**
- ✅ "Set as Default" button on layout
- ✅ Confirmation dialog
- ✅ Only one default layout per content type
- ✅ Default badge appears on layout card
- ✅ New content automatically uses default layout

**Priority:** P1 (High)

---

### User Story 11.7: Automatic WebP Conversion
**As an** administrator uploading images  
**I want** images to be automatically converted to WebP  
**So that** the website loads faster

**Acceptance Criteria:**
- ✅ When uploading JPG/PNG/GIF:
  - Original is saved to storage
  - WebP version is generated (quality: 85%)
  - Thumbnail is generated in both formats
  - All URLs are stored in database
- ✅ Upload response shows:
  - Original file size
  - WebP file size
  - Percentage reduction
- ✅ MediaFiles table stores both URLs
- ✅ Process happens in background (user doesn't wait)

**Priority:** P0 (Critical)

---

### User Story 11.8: Display Images with WebP Fallback
**As a** public user  
**I want** images to load quickly  
**So that** I don't have to wait

**Acceptance Criteria:**
- ✅ Frontend uses `<picture>` tag with WebP source
- ✅ Fallback to original format for unsupported browsers
- ✅ WebPImage component handles this automatically
- ✅ Next.js Image optimization still applies
- ✅ Lazy loading for images below fold

**Example:**
```html
<picture>
  <source srcset="image.webp" type="image/webp">
  <img src="image.jpg" alt="Description">
</picture>
```

**Priority:** P0 (Critical)

---

### User Story 11.9: Edit Social Media Metadata
**As an** administrator  
**I want to** customize how content appears when shared on social media  
**So that** shares are attractive and drive traffic

**Acceptance Criteria:**
- ✅ Content editor has "SEO & Social Media" section
- ✅ Language tabs (AR/EN) for localized metadata
- ✅ Fields per language:
  - Meta Title (for search engines)
  - Meta Description
  - **Open Graph Title** (can differ from meta title)
  - **Open Graph Description**
  - **Open Graph Image** (1200x630 recommended)
  - Twitter Card type (summary / summary_large_image)
- ✅ OG Image can be different from featured image
- ✅ Image uploader suggests optimal dimensions
- ✅ Preview how share will look (mock Facebook/Twitter card)
- ✅ All metadata saved in ContentTranslations table

**Priority:** P0 (Critical)

---

### User Story 11.10: Social Sharing Preview
**As an** administrator  
**I want to** preview how content will look when shared  
**So that** I can optimize for engagement

**Acceptance Criteria:**
- ✅ "Preview Share" button in content editor
- ✅ Shows mockup of Facebook share card
- ✅ Shows mockup of Twitter share card
- ✅ Uses actual OG Title, Description, and Image
- ✅ Toggle between Arabic and English preview
- ✅ Link to test on actual platforms

**Priority:** P2 (Medium)

---

### User Story 11.11: Render Open Graph Tags
**As a** developer  
**I want** proper OG tags in HTML head  
**So that** social platforms display content correctly

**Acceptance Criteria:**
- ✅ generateMetadata function in Next.js page
- ✅ OG tags included:
  - og:title
  - og:description
  - og:image
  - og:type = "article"
  - og:locale (ar_SA / en_US)
  - article:published_time
  - article:author
- ✅ Twitter Card tags:
  - twitter:card
  - twitter:title
  - twitter:description
  - twitter:image
- ✅ Alternate language tags (hreflang)
- ✅ Canonical URL

**Priority:** P0 (Critical)

---

### User Story 11.12: Media Library with WebP Info
**As an** administrator  
**I want to** see WebP file sizes in media library  
**So that** I know how much space I'm saving

**Acceptance Criteria:**
- ✅ Media library shows grid of uploaded media
- ✅ Each media item shows:
  - Thumbnail (WebP version)
  - Original filename
  - Original size
  - WebP size
  - Savings percentage (in green)
  - Upload date
- ✅ Filter by file type
- ✅ Search by filename
- ✅ Click to view full details
- ✅ Delete button (removes all versions)

**Priority:** P1 (High)

---

### User Story 11.13: Layout Preview Before Applying
**As an** administrator  
**I want to** preview what content looks like in a layout  
**So that** I can choose the best one

**Acceptance Criteria:**
- ✅ "Preview" button next to each layout in selector
- ✅ Opens modal with content rendered in that layout
- ✅ Shows actual content data (not placeholder)
- ✅ Responsive preview (desktop/tablet/mobile tabs)
- ✅ "Apply This Layout" button in preview
- ✅ Close preview without applying

**Priority:** P2 (Medium)

---

### User Story 11.14: Validate WebP Browser Support
**As a** developer  
**I want to** ensure WebP works across all browsers  
**So that** all users benefit

**Acceptance Criteria:**
- ✅ `<picture>` tag with WebP source and fallback
- ✅ Tested in:
  - Chrome/Edge (full support)
  - Firefox (full support)
  - Safari (support since iOS 14)
  - Older browsers (fallback to original)
- ✅ Next.js Image component supports WebP
- ✅ No broken images in any browser

**Priority:** P0 (Critical)

---

## 📊 Multi-Layout & WebP Features Summary

### Layout System Features:
- ✅ Visual Layout Builder (drag & drop)
- ✅ 7 element types (Container, Text, Image, Date, Array, Grid, Divider)
- ✅ Element properties editor (field, tag, className)
- ✅ Layout Selector in content editor
- ✅ LayoutRenderer component (dynamic rendering)
- ✅ Default layout per content type
- ✅ Duplicate layout functionality
- ✅ Live preview

### WebP Optimization:
- ✅ Automatic conversion on upload
- ✅ Preserves original file
- ✅ Generates WebP + thumbnail (both formats)
- ✅ 25-35% file size reduction (typical)
- ✅ MediaFiles table stores all URLs
- ✅ Frontend uses `<picture>` with fallback
- ✅ WebPImage component

### Social Media SEO:
- ✅ Open Graph Title (customizable)
- ✅ Open Graph Description
- ✅ Open Graph Image (1200x630)
- ✅ Twitter Card type selector
- ✅ Multilingual metadata (AR/EN)
- ✅ Proper HTML head tags
- ✅ Social sharing preview

### Performance Impact:
- ✅ 25-35% smaller image files (WebP)
- ✅ Faster page loads
- ✅ Better SEO scores (PageSpeed Insights)
- ✅ Improved social sharing CTR (better previews)
- ✅ Reduced bandwidth costs

---

## 🎨 Epic 12: Navigation Menus with Icons

### User Story 12.1: Create Menu with Location
**As an** administrator  
**I want to** create navigation menus for different locations  
**So that** I can organize site navigation (header, footer, sidebar)

**Acceptance Criteria:**
- ✅ "New Menu" button in Menus section
- ✅ Menu form fields:
  - Name (e.g., "Main Menu", "Footer Menu")
  - Location (header, footer, sidebar, mobile)
- ✅ Multiple menus can exist for same location
- ✅ Only one active menu per location at a time
- ✅ List all menus with location and item count

**Priority:** P1 (High)

---

### User Story 12.2: Add Menu Item with Icon
**As an** administrator  
**I want to** add menu items with icons  
**So that** the navigation is visually appealing

**Acceptance Criteria:**
- ✅ "Add Item" button in menu editor
- ✅ Menu item form:
  - Title (AR/EN)
  - Description/Tooltip (optional)
  - **Icon Picker** (choose from library or emoji)
  - Link type (internal page, external, content type)
  - URL
  - Open in new tab checkbox
  - Parent item (for nested menus)
- ✅ Drag & drop to reorder items
- ✅ Visual preview of icon + title
- ✅ Save button

**Priority:** P0 (Critical)

---

### User Story 12.3: Choose Icon from Library
**As an** administrator  
**I want to** choose icons from a large library  
**So that** I can find the perfect icon for each menu item

**Acceptance Criteria:**
- ✅ Icon Picker opens in modal dialog
- ✅ **3 icon types supported:**
  1. **Lucide Icons** (1200+ icons)
     - Search by name
     - Browse by grid
     - Preview before selecting
  2. **Emoji** (30+ common + custom input)
     - Grid of popular emojis
     - Text input for any emoji
  3. **Custom SVG**
     - Paste SVG code
     - Live preview
- ✅ Icon settings:
  - Color picker
  - Size selector (sm, md, lg, xl)
  - Show/Hide toggle
- ✅ Selected icon appears in preview
- ✅ "Save" button applies icon

**Priority:** P0 (Critical)

---

### User Story 12.4: Icon Search and Filter
**As an** administrator  
**I want to** search for icons by keyword  
**So that** I can quickly find the right icon

**Acceptance Criteria:**
- ✅ Search box in Icon Picker (Lucide tab)
- ✅ Search by icon name (e.g., "home", "user", "settings")
- ✅ Real-time filtering (as you type)
- ✅ Display icon count (e.g., "45 icons found")
- ✅ Grid layout (8 columns on desktop)
- ✅ Click icon to select
- ✅ Selected icon is highlighted
- ✅ Clear search button

**Priority:** P1 (High)

---

### User Story 12.5: Customize Icon Appearance
**As an** administrator  
**I want to** customize icon color and size  
**So that** icons match the website design

**Acceptance Criteria:**
- ✅ **Color customization:**
  - Color picker (visual)
  - Hex input field (e.g., #1E40AF)
  - Live preview of selected color
- ✅ **Size options:**
  - Small (16px)
  - Medium (20px) - default
  - Large (24px)
  - Extra Large (32px)
- ✅ Preview shows icon with selected color/size
- ✅ Settings saved with menu item

**Priority:** P1 (High)

---

### User Story 12.6: Create Nested Menu (Dropdown)
**As an** administrator  
**I want to** create dropdown menus  
**So that** I can organize related pages together

**Acceptance Criteria:**
- ✅ "Add Child Item" button on parent menu item
- ✅ Nested items appear indented in list
- ✅ Parent items show dropdown indicator (chevron)
- ✅ Drag & drop works for nested items
- ✅ Max nesting depth: 2 levels (dropdown only)
- ✅ Child items can have different icons
- ✅ Child items can have smaller icons

**Priority:** P1 (High)

---

### User Story 12.7: Reorder Menu Items
**As an** administrator  
**I want to** reorder menu items by dragging  
**So that** I can control the navigation sequence

**Acceptance Criteria:**
- ✅ Drag handle (grip icon) on each item
- ✅ Smooth drag animation
- ✅ Visual placeholder while dragging
- ✅ Drop to new position
- ✅ "Save Order" button
- ✅ Order persisted in database (displayOrder field)
- ✅ Confirmation message

**Priority:** P1 (High)

---

### User Story 12.8: Display Menu with Icons (Public)
**As a** public user  
**I want to** see navigation menus with icons  
**So that** I can easily identify and navigate to sections

**Acceptance Criteria:**
- ✅ Header displays menu from "header" location
- ✅ Each menu item shows:
  - Icon (if enabled)
  - Title text
  - Correct link
- ✅ Icon appears before text (RTL: after text)
- ✅ Icon color matches admin settings
- ✅ Icon size matches admin settings
- ✅ Hover effect on menu items
- ✅ Dropdown menus work correctly
- ✅ Responsive:
  - Desktop: Horizontal menu with dropdowns
  - Mobile: Hamburger menu with icons

**Priority:** P0 (Critical)

---

### User Story 12.9: Mobile Menu with Icons
**As a** mobile user  
**I want to** see icons in the mobile menu  
**So that** navigation is clear and attractive

**Acceptance Criteria:**
- ✅ Hamburger button opens mobile menu
- ✅ Mobile menu shows:
  - Icon + Title for each item
  - Chevron for items with children
- ✅ Tap parent item to expand children
- ✅ Icons are properly sized for mobile
- ✅ Smooth slide animation
- ✅ Close button (X)
- ✅ Tap outside to close
- ✅ Body scroll locked when menu open

**Priority:** P1 (High)

---

### User Story 12.10: Link Menu to Different Targets
**As an** administrator  
**I want to** link menu items to different types of content  
**So that** menus can point to any page or section

**Acceptance Criteria:**
- ✅ **Link type options:**
  1. **Internal Page** - link to custom page
  2. **Content Type** - link to content listing (e.g., /news)
  3. **Specific Content** - link to article/event/etc.
  4. **External URL** - link to external website
  5. **Custom URL** - any custom path
- ✅ Dynamic URL field based on type
- ✅ "Open in new tab" checkbox for external links
- ✅ URL validation
- ✅ Preview link button

**Priority:** P1 (High)

---

### User Story 12.11: Menu Item Description (Tooltip)
**As an** administrator  
**I want to** add descriptions to menu items  
**So that** users understand what each section contains

**Acceptance Criteria:**
- ✅ Description field (optional) in menu item form
- ✅ Multilingual (AR/EN)
- ✅ Max 500 characters
- ✅ Desktop dropdown menus show:
  - Icon + Title (bold)
  - Description (smaller text, gray)
- ✅ Mobile: description hidden (space constraint)
- ✅ Tooltip on hover (desktop)

**Priority:** P2 (Medium)

---

### User Story 12.12: Duplicate Menu
**As an** administrator  
**I want to** duplicate an existing menu  
**So that** I can create variations quickly

**Acceptance Criteria:**
- ✅ "Duplicate" button on menu list
- ✅ Creates exact copy with all items
- ✅ Duplicated menu name: "Menu Name (Copy)"
- ✅ All icons and settings copied
- ✅ Set as inactive by default
- ✅ Confirmation message
- ✅ Redirect to edit duplicated menu

**Priority:** P2 (Medium)

---

### User Story 12.13: Delete Menu Item
**As an** administrator  
**I want to** delete menu items  
**So that** I can remove outdated links

**Acceptance Criteria:**
- ✅ Delete button (trash icon) on each item
- ✅ Confirmation dialog:
  - "هل أنت متأكد؟"
  - Show item title
  - Warning if item has children
- ✅ Deleting parent deletes all children
- ✅ Reorder remaining items automatically
- ✅ Success message
- ✅ Undo option (5 seconds)

**Priority:** P1 (High)

---

### User Story 12.14: Icon Library API
**As a** developer  
**I want** an API to search available icons  
**So that** the Icon Picker can load icons dynamically

**Acceptance Criteria:**
- ✅ `GET /api/icons?library=lucide&search=home`
- ✅ Returns:
  - Icon name
  - Category
  - Tags (for better search)
  - SVG code
  - Preview (base64 image)
- ✅ Pagination (50 icons per page)
- ✅ Fast response (< 200ms)
- ✅ Cached results

**Priority:** P1 (High)

---

### User Story 12.15: Active Menu Indicator
**As a** public user  
**I want to** see which page I'm currently on  
**So that** I can understand my location in the site

**Acceptance Criteria:**
- ✅ Current page's menu item is highlighted
- ✅ Different style (e.g., blue background, bold)
- ✅ Parent item highlighted if on child page
- ✅ Works with all link types
- ✅ URL matching logic (exact + partial)

**Priority:** P2 (Medium)

---

## 📊 Menu Icons Feature Summary

### Icon Libraries Supported:
1. **Lucide Icons** (1200+ icons)
   - Modern, clean design
   - Consistent stroke width
   - React components
   - Free & open source

2. **Emoji** (unlimited)
   - Unicode emojis
   - Platform-native rendering
   - No file loading
   - Universal support

3. **Custom SVG**
   - Any SVG code
   - Brand icons
   - Unique designs
   - Full flexibility

### Icon Customization:
- ✅ Color (hex color picker)
- ✅ Size (sm/md/lg/xl)
- ✅ Show/Hide toggle
- ✅ Live preview

### Menu Features:
- ✅ Multiple menus per location
- ✅ Nested menus (1 level deep)
- ✅ Drag & drop reordering
- ✅ Multilingual titles/descriptions
- ✅ Link to any content type
- ✅ Responsive design

### Admin UX:
- ✅ Visual Icon Picker
- ✅ Search 1200+ icons
- ✅ Drag & drop menu builder
- ✅ Live preview
- ✅ One-click duplicate

### Public UX:
- ✅ Icons in header/footer
- ✅ Dropdown menus with icons
- ✅ Mobile hamburger menu
- ✅ Active page indicator
- ✅ Smooth animations

---

**Document Version:** 1.1  
**Last Updated:** November 10, 2025  
**Total User Stories:** 74 (59 previous + 15 new)  
**Maintained By:** Product Management Team
