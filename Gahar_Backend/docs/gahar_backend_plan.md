# 🧩 Backend Developers Work Plan – GAHAR CMS (Advanced .NET 8 Architecture)

## 📅 Duration: 5 Weeks + Sprint 0 (Preparation)
## 🎯 Objective:
Build a **Dynamic & Scalable Headless CMS** based on **.NET 8** with an **Event-Driven Architecture**, allowing seamless integration with frontend frameworks like React or Next.js, and full multilingual (Arabic/English) support using the **Translation Table Pattern** for SEO optimization.

---

## 🏗️ Sprint 0 — Setup and Initialization (Week 0)

### 🎯 Goal:
Prepare the infrastructure and ensure the project is ready for team development.

### ⚙️ Tasks:
1. **Project Setup**
   - Create a solution named `GAHAR.CMS` containing:
     - `GAHAR.API` (Web API .NET 8 Minimal API)
     - `GAHAR.Core` (Business Logic, Domain Models)
     - `GAHAR.Infrastructure` (EF Core, Repositories, Messaging)
     - `GAHAR.Shared` (Contracts & DTOs)

2. **Database Setup (EF Core 9)**
   - Configure PostgreSQL or SQL Server.
   - Create initial migration implementing the Translation Table Pattern schema (detailed in Sprint 2).

3. **Infrastructure Setup**
   - Configure Message Queue (RabbitMQ / Azure Service Bus).
   - Setup Hangfire for background jobs.
   - Implement centralized logging using Serilog.

4. **CI/CD**
   - Configure GitHub Actions or Azure DevOps pipeline.
   - Automate Build, Test, and Deploy processes.

---

## 🔐 Sprint 1 — Authentication & Core Foundation (Week 1)

### 🎯 Goal:
Implement security, authentication, and core backend services.

### ⚙️ Tasks:
1. **Authentication & Authorization**
   - Implement ASP.NET Core Identity.
   - Generate JWT Tokens.
   - Build Roles & Policies system.

2. **Core Middleware Services**
   - Global Error Handling Middleware.
   - Centralized Logging & Auditing.
   - CORS + Rate Limiting.

3. **Event Bus Core**
   - Create `GAHAR.Shared.Events` library for shared event contracts.
   - Implement Publisher/Subscriber with RabbitMQ.

4. **User Management Endpoints (CRUD)**
   - Secure API for user management.

📦 **Deliverable:** Fully functional authentication system and Event Bus foundation.

---

## 🧠 Sprint 2 — Dynamic Content Engine + Translation Table Pattern (Week 2)

### 🎯 Goal:
Develop the dynamic content engine with full multilingual (Arabic/English) support and SEO-ready design.

### ⚙️ Tasks:
1. **Database Schema Design (Translation Table Pattern):**
   ```sql
   ContentTypes
   -------------
   id, name, fields (JSON schema)

   Content
   -------------
   id, type_id (FK), created_at, updated_at, published

   ContentTranslations
   -------------
   id, content_id (FK), language (ar/en), slug, title, body, meta_title, meta_description
   ```

2. **Dynamic API Development:**
   - `POST /api/content/{type}` → Create content with default translation.
   - `GET /api/content/{type}?lang=ar` → Fetch Arabic content.
   - `PUT /api/content/{type}/{id}?lang=en` → Update English translation only.

3. **Block-Based Page Builder API:**
   - Store page layout as JSON Blocks.
   - Endpoint: `/api/pages` for dynamic page management.

4. **AI Translation Module:**
   - Endpoint: `/utils/translate` for automatic content translation.
   - Integrates directly with `ContentTranslations`.

5. **File Management Service:**
   - Secure endpoint for file uploads (images, PDFs).

📦 **Deliverable:** Fully dynamic, multilingual CMS core with SEO support.

---

## ⚙️ Sprint 3 — Specialized GAHAR Services (Week 3)

### 🎯 Goal:
Develop specialized modules for GAHAR (Certified Facilities, Certificate Validation, Forms & Analytics).

### ⚙️ Tasks:
1. **Certified Facilities Service:**
   - Endpoint to upload Excel files → processed by Hangfire in background.
   - Store bilingual facility data (`FacilityTranslations`).
   - Indexed slugs for SEO.

2. **Certificate Validation Service:**
   - Public high-performance endpoint for certificate verification.
   - Native AOT Optimization.
   - Multilingual responses (ar/en).

3. **Forms & Analytics Engine:**
   - Form Builder API + Submission API.
   - Publish `FormSubmitted` event to Event Bus.

📦 **Deliverable:** Advanced GAHAR business logic with multilingual support.

---

## 🧩 Sprint 4 — Advanced Services & Integrations (Week 4)

### 🎯 Goal:
Implement event listeners, intelligent search, notifications, and integrations.

### ⚙️ Tasks:
1. **Event Consumers:**
   - `FormSubmissionConsumer` → Send email + SignalR notifications.
   - `NewSubscriberConsumer` → Manage newsletter subscriptions.

2. **Multilingual Search Service:**
   - Endpoint `/api/search?q=...&lang=ar`.
   - Integrate Semantic Search using .NET AI.

3. **Interactive Map API:**
   - Provide facility data as GeoJSON (bilingual output).

📦 **Deliverable:** Event-driven integrations and intelligent search functionality.

---

## 🚀 Sprint 5 — Optimization, Testing & Documentation (Week 5)

### 🎯 Goal:
Optimize performance, ensure security, and finalize documentation for production.

### ⚙️ Tasks:
1. **Performance Optimization:**
   - Apply Native AOT on high-load services.
   - Implement caching for multilingual content.
   - Conduct full Load & Stress Testing.

2. **Testing:**
   - Unit & Integration Tests (80% coverage).
   - Test content translations (ar/en) and SEO slugs.

3. **Security Review:**
   - Final review of all Roles & Policies.

4. **Documentation (Swagger):**
   - Document all endpoints in both languages.
   - Generate separate Sitemaps for each language.

📦 **Deliverable:** Production-ready multilingual GAHAR CMS API v1.0 — optimized, secure, and scalable.


---

## 🎨 Frontend: Next.js Drag & Drop Page Builder (Admin) — (Added on user's request)

### 🎯 Goal
Build a Next.js frontend admin page that allows editors to visually build pages (drag & drop blocks) and save the page structure as JSON which will be consumed by the dynamic Backend API.

### 🧩 Approach (Overview)
- Use **Next.js** (App Router or Pages Router) for the frontend.
- For drag & drop, use **dnd-kit** (modern, flexible) or **react-beautiful-dnd** (stable). Recommendation: **dnd-kit**.
- The Admin Page Builder communicates with the Backend API endpoints:
  - `GET /api/pages/{slug}` — fetch page JSON
  - `POST /api/pages` — create page (payload: { slug, title, structure })
  - `PUT /api/pages/{id}` — update page structure
- BlockRegistry on the frontend maps `blockType` strings to React components (editor view + preview view).
- Save the page structure as an array of blocks: `[{ blockType, props, id }]`.

### 🔧 Key Features
- Drag & drop to reorder blocks
- Add / Remove / Edit block props via side panel
- Translate block props per language (store per-language values in block props or keep separate translations synced to backend `ContentTranslations`)
- Live preview (desktop / mobile)
- Save / Publish buttons (call backend endpoints)

---

### 📦 Minimal Example: Next.js + dnd-kit Page Builder (Simplified)

```jsx
// app/admin/pages/[slug]/page-builder.jsx (Next.js App Router example)
'use client'
import { useState, useEffect } from 'react'
import axios from 'axios'
import { DndContext, closestCenter, PointerSensor, useSensor, useSensors } from '@dnd-kit/core'
import { arrayMove, SortableContext, verticalListSortingStrategy } from '@dnd-kit/sortable'
import SortableBlock from '../../../components/SortableBlock' // wrapper for draggable block
import BlockEditorPanel from '../../../components/BlockEditorPanel'

export default function PageBuilder({ params }) {
  const { slug } = params
  const [structure, setStructure] = useState([])
  const [selectedBlockId, setSelectedBlockId] = useState(null)

  useEffect(() => {
    axios.get(`/api/pages/${slug}`).then(r => {
      setStructure(r.data.structure || [])
    }).catch(() => setStructure([]))
  }, [slug])

  const sensors = useSensors(useSensor(PointerSensor))

  function handleDragEnd(event) {
    const { active, over } = event
    if (!over || active.id === over.id) return
    const oldIndex = structure.findIndex(b => b.id === active.id)
    const newIndex = structure.findIndex(b => b.id === over.id)
    setStructure(prev => arrayMove(prev, oldIndex, newIndex))
  }

  function addBlock(type) {
    const id = crypto.randomUUID()
    const block = { id, blockType: type, props: {} }
    setStructure(prev => [...prev, block])
    setSelectedBlockId(id)
  }

  function save() {
    axios.put(`/api/pages/${slug}`, { structure }).then(() => alert('Saved'))
  }

  return (
    <div className="page-builder">
      <aside className="left-panel">
        <button onClick={() => addBlock('HeroBanner')}>Add Hero</button>
        <button onClick={() => addBlock('LatestNews')}>Add LatestNews</button>
      </aside>

      <main className="canvas">
        <DndContext sensors={sensors} collisionDetection={closestCenter} onDragEnd={handleDragEnd}>
          <SortableContext items={structure.map(s => s.id)} strategy={verticalListSortingStrategy}>
            {structure.map(block => (
              <SortableBlock key={block.id} id={block.id} block={block} onSelect={() => setSelectedBlockId(block.id)} />
            ))}
          </SortableContext>
        </DndContext>
      </main>

      <aside className="right-panel">
        <BlockEditorPanel
          block={structure.find(b => b.id === selectedBlockId)}
          onChange={(updated) => setStructure(prev => prev.map(b => b.id === updated.id ? updated : b))}
          onSave={save}
        />
        <button onClick={save}>Save Page</button>
      </aside>
    </div>
  )
}
```

**Notes:**
- `SortableBlock` wraps each block to be draggable and also renders the block's preview (use BlockRegistry to render preview).
- `BlockEditorPanel` allows editing the selected block's `props` and per-language fields.
- For translations, you can store `props.translations = { ar: {...}, en: {...} }` or store block content in backend translations table and reference them by `contentId`.

---

### 🌐 Translation & SEO in Page Builder
- While editing a block, allow editors to switch language tabs (AR/EN) and edit translations for title, body, and slug.
- On save, call backend endpoints to persist `Content` and `ContentTranslations` (ensuring SEO slugs per language).
- Provide a sitemap generation action in admin for each language.

---

### ✅ Recommendations
- Use **dnd-kit** for flexible drag & drop and accessibility.
- Use **React Query** or **SWR** for data fetching/caching.
- Keep BlockRegistry normalized: each block must have two components — `EditorComponent` and `PreviewComponent`.
- Sync translations to backend immediately on block save to avoid inconsistency.

---

If you want, I can:
- Add the Next.js Page Builder section to the canvas (done).
- Generate full component files (`SortableBlock`, `BlockEditorPanel`, BlockRegistry) and a minimal Next.js project scaffold.
- Create a demo GitHub repo structure for you to clone.

Which one do you want next?
