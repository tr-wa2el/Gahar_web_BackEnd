# 🎨 GAHAR CMS - Frontend Technical Specification (Next.js 15 + TypeScript)

## 📋 Table of Contents
1. [Architecture Overview](#architecture-overview)
2. [Project Structure](#project-structure)
3. [Pages & Routes](#pages--routes)
4. [Components Library](#components-library)
5. [State Management](#state-management)
6. [API Integration](#api-integration)
7. [Internationalization (i18n)](#internationalization-i18n)
8. [Admin Panel - Page Builder](#admin-panel---page-builder)
9. [Performance Optimization](#performance-optimization)
10. [Deployment](#deployment)

---

## 🏗️ Architecture Overview

### Technology Stack
- **Framework:** Next.js 15 (App Router)
- **Language:** TypeScript 5.3+
- **Styling:** Tailwind CSS 4 + Shadcn/ui
- **State Management:** Zustand + React Query (TanStack Query)
- **Forms:** React Hook Form + Zod validation
- **Drag & Drop:** dnd-kit
- **Rich Text Editor:** Tiptap / Lexical
- **Maps:** Leaflet / Mapbox GL
- **Charts:** Recharts
- **Icons:** Lucide React
- **Animations:** Framer Motion
- **HTTP Client:** Axios / Fetch API
- **Authentication:** NextAuth.js (JWT strategy)

### Architecture Principles
- **Server Components First:** Use RSC by default, CSC when needed
- **Type Safety:** Full TypeScript coverage
- **Component-Driven:** Reusable, composable components
- **Accessibility:** WCAG 2.1 AA compliant
- **SEO-Optimized:** Metadata API, structured data, sitemaps
- **Performance:** Code splitting, lazy loading, image optimization
- **Responsive:** Mobile-first design

---

## 📁 Project Structure

```
gahar-frontend/
│
├── public/
│   ├── images/
│   ├── fonts/
│   └── favicon.ico
│
├── src/
│   ├── app/                                # Next.js App Router
│   │   ├── [lang]/                         # Dynamic language routes
│   │   │   ├── layout.tsx                  # Root layout with providers
│   │   │   ├── page.tsx                    # Homepage
│   │   │   ├── about/
│   │   │   │   └── page.tsx
│   │   │   ├── news/
│   │   │   │   ├── page.tsx                # News listing
│   │   │   │   └── [slug]/
│   │   │   │       └── page.tsx            # Single news article
│   │   │   ├── facilities/
│   │   │   │   ├── page.tsx                # Facilities listing + map
│   │   │   │   └── [slug]/
│   │   │   │       └── page.tsx            # Facility details
│   │   │   ├── certificates/
│   │   │   │   └── validate/
│   │   │   │       └── page.tsx            # Certificate validation
│   │   │   ├── contact/
│   │   │   │   └── page.tsx
│   │   │   └── [...slug]/                  # Dynamic pages from CMS
│   │   │       └── page.tsx
│   │   │
│   │   ├── admin/                          # Admin Panel (protected)
│   │   │   ├── layout.tsx                  # Admin layout with sidebar
│   │   │   ├── dashboard/
│   │   │   │   └── page.tsx
│   │   │   ├── content/
│   │   │   │   ├── page.tsx                # Content listing
│   │   │   │   ├── new/
│   │   │   │   │   └── page.tsx            # Create content
│   │   │   │   └── [id]/
│   │   │   │       └── edit/
│   │   │   │           └── page.tsx        # Edit content
│   │   │   ├── pages/
│   │   │   │   ├── page.tsx                # Pages listing
│   │   │   │   ├── new/
│   │   │   │   │   └── page.tsx            # Create page
│   │   │   │   └── [id]/
│   │   │   │       └── builder/
│   │   │   │           └── page.tsx        # Page Builder (Drag & Drop)
│   │   │   ├── facilities/
│   │   │   │   ├── page.tsx
│   │   │   │   └── upload/
│   │   │   │       └── page.tsx            # Upload Excel
│   │   │   ├── forms/
│   │   │   │   ├── page.tsx                # Forms listing
│   │   │   │   ├── new/
│   │   │   │   │   └── page.tsx            # Form builder
│   │   │   │   └── [id]/
│   │   │   │       ├── edit/
│   │   │   │       │   └── page.tsx
│   │   │   │       └── submissions/
│   │   │   │           └── page.tsx        # View submissions
│   │   │   ├── users/
│   │   │   │   └── page.tsx
│   │   │   └── settings/
│   │   │       └── page.tsx
│   │   │
│   │   ├── api/                            # Next.js API Routes (if needed)
│   │   │   └── auth/
│   │   │       └── [...nextauth]/
│   │   │           └── route.ts
│   │   │
│   │   ├── login/
│   │   │   └── page.tsx
│   │   │
│   │   └── sitemap.ts                      # Dynamic sitemap
│   │
│   ├── components/                         # Reusable Components
│   │   ├── ui/                             # Base UI Components (Shadcn)
│   │   │   ├── button.tsx
│   │   │   ├── input.tsx
│   │   │   ├── card.tsx
│   │   │   ├── dialog.tsx
│   │   │   ├── dropdown-menu.tsx
│   │   │   ├── form.tsx
│   │   │   ├── table.tsx
│   │   │   ├── tabs.tsx
│   │   │   └── ...
│   │   │
│   │   ├── layout/                         # Layout Components
│   │   │   ├── Header.tsx
│   │   │   ├── Footer.tsx
│   │   │   ├── Navbar.tsx
│   │   │   ├── Sidebar.tsx                 # Admin sidebar
│   │   │   └── Container.tsx
│   │   │
│   │   ├── blocks/                         # CMS Content Blocks
│   │   │   ├── HeroBanner.tsx
│   │   │   ├── LatestNews.tsx
│   │   │   ├── FeaturedServices.tsx
│   │   │   ├── StatisticsSection.tsx
│   │   │   ├── TeamMembers.tsx
│   │   │   ├── ContactForm.tsx
│   │   │   ├── FacilitiesMap.tsx
│   │   │   └── BlockRegistry.ts            # Maps blockType to component
│   │   │
│   │   ├── admin/                          # Admin-Specific Components
│   │   │   ├── PageBuilder/
│   │   │   │   ├── Canvas.tsx              # Drag & drop canvas
│   │   │   │   ├── BlocksPanel.tsx         # Available blocks sidebar
│   │   │   │   ├── PropertiesPanel.tsx     # Edit block properties
│   │   │   │   ├── SortableBlock.tsx       # Draggable block wrapper
│   │   │   │   └── BlockEditor.tsx         # Individual block editor
│   │   │   ├── ContentEditor/
│   │   │   │   ├── RichTextEditor.tsx
│   │   │   │   ├── TranslationTabs.tsx
│   │   │   │   └── MediaUploader.tsx
│   │   │   ├── FormBuilder/
│   │   │   │   ├── FieldsEditor.tsx
│   │   │   │   └── FieldTypeSelector.tsx
│   │   │   └── DataTable.tsx               # Generic data table
│   │   │
│   │   ├── forms/                          # Form Components
│   │   │   ├── DynamicForm.tsx             # Renders form from schema
│   │   │   ├── ContactFormComponent.tsx
│   │   │   └── CertificateValidationForm.tsx
│   │   │
│   │   ├── maps/
│   │   │   ├── FacilitiesMap.tsx
│   │   │   └── MarkerPopup.tsx
│   │   │
│   │   └── common/                         # Common Components
│   │       ├── LoadingSpinner.tsx
│   │       ├── ErrorBoundary.tsx
│   │       ├── Pagination.tsx
│   │       ├── SearchBar.tsx
│   │       ├── LanguageSwitcher.tsx
│   │       └── BreadcrumbNav.tsx
│   │
│   ├── lib/                                # Utilities & Configurations
│   │   ├── api/                            # API Client
│   │   │   ├── client.ts                   # Axios instance
│   │   │   ├── endpoints/
│   │   │   │   ├── auth.ts
│   │   │   │   ├── content.ts
│   │   │   │   ├── pages.ts
│   │   │   │   ├── facilities.ts
│   │   │   │   ├── certificates.ts
│   │   │   │   └── forms.ts
│   │   │   └── interceptors.ts
│   │   │
│   │   ├── hooks/                          # Custom React Hooks
│   │   │   ├── useAuth.ts
│   │   │   ├── useContent.ts
│   │   │   ├── useFacilities.ts
│   │   │   ├── usePageBuilder.ts
│   │   │   └── useLanguage.ts
│   │   │
│   │   ├── stores/                         # Zustand Stores
│   │   │   ├── authStore.ts
│   │   │   ├── pageBuilderStore.ts
│   │   │   └── uiStore.ts
│   │   │
│   │   ├── validations/                    # Zod Schemas
│   │   │   ├── contentSchemas.ts
│   │   │   ├── formSchemas.ts
│   │   │   └── authSchemas.ts
│   │   │
│   │   ├── utils/
│   │   │   ├── cn.ts                       # classNames utility
│   │   │   ├── formatDate.ts
│   │   │   ├── slugify.ts
│   │   │   └── seo.ts
│   │   │
│   │   └── constants.ts
│   │
│   ├── types/                              # TypeScript Types
│   │   ├── api.ts                          # API response types
│   │   ├── content.ts
│   │   ├── page.ts
│   │   ├── facility.ts
│   │   ├── form.ts
│   │   └── user.ts
│   │
│   ├── dictionaries/                       # i18n Translations
│   │   ├── ar.json
│   │   └── en.json
│   │
│   └── middleware.ts                       # Next.js Middleware (lang redirect)
│
├── .env.local
├── .env.production
├── next.config.mjs
├── tailwind.config.ts
├── tsconfig.json
└── package.json
```

---

## 🌐 Pages & Routes

### Public Pages (SEO-Optimized)

#### Homepage: `/[lang]`
- **Features:**
  - Hero banner with CTA
  - Latest news carousel
  - Featured services
  - Statistics section
  - Quick certificate validation
- **Metadata:** Dynamic per language
- **Data Fetching:** SSR with `fetch` (cache: 'force-cache')

```tsx
// app/[lang]/page.tsx
import { Metadata } from 'next'
import { getPage } from '@/lib/api/endpoints/pages'

export async function generateMetadata({ params }: { params: { lang: string } }): Promise<Metadata> {
  const page = await getPage('home', params.lang)
  
  return {
    title: page.metaTitle,
    description: page.metaDescription,
    alternates: {
      canonical: `/${params.lang}`,
      languages: {
        'ar': '/ar',
        'en': '/en'
      }
    }
  }
}

export default async function HomePage({ params }: { params: { lang: string } }) {
  const page = await getPage('home', params.lang)
  
  return (
    <main>
      {page.structure.map(block => (
        <BlockRenderer key={block.id} block={block} />
      ))}
    </main>
  )
}
```

#### News Listing: `/[lang]/news`
- **Features:**
  - Paginated news articles
  - Filter by category/tag
  - Search functionality
- **Data Fetching:** SSR with pagination

#### News Article: `/[lang]/news/[slug]`
- **Features:**
  - **Dynamic Layout Rendering** (based on selected layout)
  - Full article content with WebP images
  - Related articles
  - Social sharing with custom OG images
  - Structured data (NewsArticle schema)
- **Data Fetching:** SSG with ISR (revalidate: 3600)

```tsx
// app/[lang]/news/[slug]/page.tsx
import LayoutRenderer from '@/components/layouts/LayoutRenderer'
import { Metadata } from 'next'

export async function generateStaticParams() {
  const articles = await getAllNews()
  
  return articles.map(article => ({
    lang: 'ar',
    slug: article.slug
  }))
}

export const revalidate = 3600 // ISR: revalidate every hour

export async function generateMetadata({ params }: { params: { lang: string, slug: string } }): Promise<Metadata> {
  const article = await getNewsArticle(params.slug, params.lang)
  
  return {
    title: article.metaTitle,
    description: article.metaDescription,
    openGraph: {
      title: article.ogTitle || article.metaTitle,
      description: article.ogDescription || article.metaDescription,
      images: [
        {
          url: article.ogImage || article.customFields.featuredImage.webPUrl,
          width: 1200,
          height: 630,
          alt: article.title
        }
      ],
      type: 'article',
      locale: params.lang === 'ar' ? 'ar_SA' : 'en_US',
      publishedTime: article.publishedAt,
      authors: [article.customFields.author]
    },
    twitter: {
      card: article.twitterCard || 'summary_large_image',
      title: article.ogTitle || article.metaTitle,
      description: article.ogDescription || article.metaDescription,
      images: [article.ogImage || article.customFields.featuredImage.webPUrl]
    },
    alternates: {
      canonical: `/${params.lang}/news/${article.slug}`,
      languages: {
        'ar': `/ar/news/${article.slugAr}`,
        'en': `/en/news/${article.slugEn}`
      }
    }
  }
}

export default async function NewsArticle({ params }: { params: { lang: string, slug: string } }) {
  const article = await getNewsArticle(params.slug, params.lang)
  
  // Structured Data for SEO
  const jsonLd = {
    '@context': 'https://schema.org',
    '@type': 'NewsArticle',
    headline: article.title,
    description: article.metaDescription,
    image: article.customFields.featuredImage.webPUrl,
    datePublished: article.publishedAt,
    dateModified: article.updatedAt,
    author: {
      '@type': 'Organization',
      name: 'GAHAR'
    },
    publisher: {
      '@type': 'Organization',
      name: 'GAHAR',
      logo: {
        '@type': 'ImageObject',
        url: 'https://gahar.sa/logo.png'
      }
    }
  }
  
  return (
    <>
      <script type="application/ld+json" dangerouslySetInnerHTML={{ __html: JSON.stringify(jsonLd) }} />
      
      {/* Render content with selected layout */}
      <LayoutRenderer
        layout={article.layout}
        content={article}
        lang={params.lang}
      />
    </>
  )
}
```

**Layout Renderer Component:**

```tsx
// components/layouts/LayoutRenderer.tsx
'use client'
import { useMemo } from 'react'
import Image from 'next/image'
import { format } from 'date-fns'
import { ar, enUS } from 'date-fns/locale'

interface LayoutRendererProps {
  layout: {
    id: string
    name: string
    structure: any
  }
  content: any
  lang: string
}

export default function LayoutRenderer({ layout, content, lang }: LayoutRendererProps) {
  const locale = lang === 'ar' ? ar : enUS
  
  const renderField = (field: any) => {
    const value = content[field.field] || content.customFields?.[field.field]
    
    if (!value) return null
    
    // Handle different field types
    switch (field.type) {
      case 'field':
        return renderFieldValue(field, value, locale)
      
      case 'container':
        return (
          <div className={field.className}>
            {field.children?.map((child: any, index: number) => (
              <div key={index}>{renderField(child)}</div>
            ))}
          </div>
        )
      
      case 'component':
        return renderComponent(field.component, content)
      
      default:
        return null
    }
  }
  
  const renderFieldValue = (field: any, value: any, locale: any) => {
    const Tag = field.tag || 'div'
    
    // Date formatting
    if (field.format === 'date' && value) {
      const formattedDate = format(new Date(value), 'PPP', { locale })
      return (
        <Tag className={field.className}>
          {field.prefix}{formattedDate}
        </Tag>
      )
    }
    
    // Image field (with WebP support)
    if (field.field === 'featuredImage' || field.fieldType === 'image') {
      return (
        <picture className={field.className}>
          <source srcSet={value.webPUrl} type="image/webp" />
          <Image
            src={value.originalUrl}
            alt={value.altText || content.title}
            width={value.width}
            height={value.height}
            className="w-full h-auto"
            priority={field.priority}
          />
        </picture>
      )
    }
    
    // Array field (e.g., tags)
    if (Array.isArray(value)) {
      return (
        <Tag className={field.className}>
          {value.map((item, index) => (
            <span key={index} className={field.itemClassName}>
              {item}
            </span>
          ))}
        </Tag>
      )
    }
    
    // HTML content
    if (field.field === 'body') {
      return (
        <Tag
          className={field.className}
          dangerouslySetInnerHTML={{ __html: value }}
        />
      )
    }
    
    // Plain text
    return (
      <Tag className={field.className}>
        {field.prefix}{value}
      </Tag>
    )
  }
  
  const renderComponent = (componentName: string, content: any) => {
    // Render special components
    switch (componentName) {
      case 'SocialShareButtons':
        return <SocialShareButtons content={content} />
      
      case 'RelatedArticles':
        return <RelatedArticles articleId={content.id} />
      
      default:
        return null
    }
  }
  
  return (
    <div className={layout.structure.container?.className}>
      {layout.structure.sections?.map((section: any, index: number) => (
        <div key={section.id || index}>
          {renderField(section)}
        </div>
      ))}
    </div>
  )
}
```

#### Albums: `/[lang]/albums`
- **Features:**
  - Grid of album cards with cover images
  - Album title, description, image count
  - Filter by date
- **Data Fetching:** SSR with cache

#### Album Detail: `/[lang]/albums/[slug]`
- **Features:**
  - **Dynamic Collage Layout** (responsive masonry)
  - Lightbox for full-screen image viewing
  - Image captions
  - Social sharing
  - Download original image option
- **Data Fetching:** SSG with ISR

**Dynamic Collage Layout Implementation:**

```tsx
// app/[lang]/albums/[slug]/page.tsx
import AlbumCollage from '@/components/albums/AlbumCollage'

export default async function AlbumPage({ params }: { params: { lang: string, slug: string } }) {
  const album = await getAlbum(params.slug, params.lang)
  
  return (
    <div className="container mx-auto py-12">
      <h1 className="text-4xl font-bold mb-4">{album.title}</h1>
      <p className="text-gray-600 mb-8">{album.description}</p>
      
      <AlbumCollage images={album.images} />
    </div>
  )
}
```

**Collage Algorithm Component:**

```tsx
// components/albums/AlbumCollage.tsx
'use client'
import { useState } from 'react'
import Image from 'next/image'
import Lightbox from '@/components/albums/Lightbox'

interface AlbumImage {
  id: string
  fileUrl: string
  thumbnailUrl: string
  width: number
  height: number
  aspectRatio: number
  caption: string
  altText: string
}

interface CollageRow {
  images: AlbumImage[]
  totalAspectRatio: number
}

export default function AlbumCollage({ images }: { images: AlbumImage[] }) {
  const [selectedImage, setSelectedImage] = useState<AlbumImage | null>(null)
  const [lightboxIndex, setLightboxIndex] = useState(0)
  
  // Collage Layout Algorithm
  const rows = calculateCollageLayout(images, {
    targetRowHeight: 300, // Base row height in pixels
    containerWidth: 1200, // Max container width
    gap: 8 // Gap between images in pixels
  })
  
  const openLightbox = (image: AlbumImage, index: number) => {
    setSelectedImage(image)
    setLightboxIndex(index)
  }
  
  return (
    <>
      <div className="space-y-2">
        {rows.map((row, rowIndex) => (
          <div key={rowIndex} className="flex gap-2">
            {row.images.map((image, imgIndex) => {
              const globalIndex = rows.slice(0, rowIndex).reduce((acc, r) => acc + r.images.length, 0) + imgIndex
              const imageWidth = `${(image.aspectRatio / row.totalAspectRatio) * 100}%`
              
              return (
                <div
                  key={image.id}
                  className="relative overflow-hidden rounded-lg cursor-pointer hover:opacity-90 transition-opacity"
                  style={{ width: imageWidth }}
                  onClick={() => openLightbox(image, globalIndex)}
                >
                  <Image
                    src={image.thumbnailUrl}
                    alt={image.altText}
                    width={image.width}
                    height={image.height}
                    className="w-full h-auto object-cover"
                    loading="lazy"
                  />
                  {image.caption && (
                    <div className="absolute bottom-0 left-0 right-0 bg-black/70 text-white p-2 text-sm opacity-0 hover:opacity-100 transition-opacity">
                      {image.caption}
                    </div>
                  )}
                </div>
              )
            })}
          </div>
        ))}
      </div>
      
      {selectedImage && (
        <Lightbox
          images={images}
          currentIndex={lightboxIndex}
          onClose={() => setSelectedImage(null)}
          onNavigate={(index) => {
            setLightboxIndex(index)
            setSelectedImage(images[index])
          }}
        />
      )}
    </>
  )
}

/**
 * Dynamic Collage Layout Algorithm
 * 
 * This algorithm balances aspect ratios across rows to create a visually pleasing
 * masonry-style layout without gaps.
 * 
 * Strategy:
 * 1. Group images into rows
 * 2. Balance each row's total aspect ratio to fit container width
 * 3. Avoid very tall or very short rows
 * 4. Handle different aspect ratios (landscape, portrait, square)
 */
function calculateCollageLayout(
  images: AlbumImage[],
  options: {
    targetRowHeight: number
    containerWidth: number
    gap: number
  }
): CollageRow[] {
  const { targetRowHeight, containerWidth, gap } = options
  const rows: CollageRow[] = []
  
  let currentRow: AlbumImage[] = []
  let currentRowAspectRatio = 0
  
  // Target aspect ratio for a full row
  const targetRowAspectRatio = containerWidth / targetRowHeight
  
  for (let i = 0; i < images.length; i++) {
    const image = images[i]
    currentRow.push(image)
    currentRowAspectRatio += image.aspectRatio
    
    // Check if we should start a new row
    const isLastImage = i === images.length - 1
    const shouldBreak = currentRowAspectRatio >= targetRowAspectRatio || isLastImage
    
    if (shouldBreak) {
      // Calculate how many gaps we have
      const gapCount = currentRow.length - 1
      const totalGapWidth = gapCount * gap
      const availableWidth = containerWidth - totalGapWidth
      
      // Calculate actual row height based on total aspect ratio
      const rowHeight = availableWidth / currentRowAspectRatio
      
      // Avoid rows that are too tall or too short
      const minRowHeight = targetRowHeight * 0.7
      const maxRowHeight = targetRowHeight * 1.5
      
      if (rowHeight < minRowHeight && !isLastImage) {
        // Row would be too tall, continue adding images
        continue
      }
      
      if (rowHeight > maxRowHeight && currentRow.length > 1) {
        // Row would be too short, remove last image and start new row
        currentRow.pop()
        currentRowAspectRatio -= image.aspectRatio
        i-- // Process this image again in next row
      }
      
      // Commit the row
      rows.push({
        images: [...currentRow],
        totalAspectRatio: currentRowAspectRatio
      })
      
      // Reset for next row
      currentRow = []
      currentRowAspectRatio = 0
    }
  }
  
  return rows
}
```

**Lightbox Component:**

```tsx
// components/albums/Lightbox.tsx
'use client'
import { useEffect } from 'react'
import Image from 'next/image'
import { X, ChevronLeft, ChevronRight, Download, Share2 } from 'lucide-react'
import { Button } from '@/components/ui/button'

interface LightboxProps {
  images: Array<{
    id: string
    fileUrl: string
    caption: string
    altText: string
    width: number
    height: number
  }>
  currentIndex: number
  onClose: () => void
  onNavigate: (index: number) => void
}

export default function Lightbox({ images, currentIndex, onClose, onNavigate }: LightboxProps) {
  const currentImage = images[currentIndex]
  
  // Keyboard navigation
  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose()
      if (e.key === 'ArrowLeft' && currentIndex > 0) onNavigate(currentIndex - 1)
      if (e.key === 'ArrowRight' && currentIndex < images.length - 1) onNavigate(currentIndex + 1)
    }
    
    window.addEventListener('keydown', handleKeyDown)
    return () => window.removeEventListener('keydown', handleKeyDown)
  }, [currentIndex, images.length, onClose, onNavigate])
  
  const handleDownload = async () => {
    const link = document.createElement('a')
    link.href = currentImage.fileUrl
    link.download = `image-${currentImage.id}.jpg`
    link.click()
  }
  
  const handleShare = async () => {
    if (navigator.share) {
      await navigator.share({
        title: currentImage.caption,
        text: currentImage.caption,
        url: currentImage.fileUrl
      })
    }
  }
  
  return (
    <div className="fixed inset-0 bg-black/95 z-50 flex items-center justify-center">
      {/* Close Button */}
      <Button
        variant="ghost"
        size="icon"
        className="absolute top-4 right-4 text-white hover:bg-white/20"
        onClick={onClose}
      >
        <X className="h-6 w-6" />
      </Button>
      
      {/* Previous Button */}
      {currentIndex > 0 && (
        <Button
          variant="ghost"
          size="icon"
          className="absolute left-4 text-white hover:bg-white/20"
          onClick={() => onNavigate(currentIndex - 1)}
        >
          <ChevronLeft className="h-8 w-8" />
        </Button>
      )}
      
      {/* Next Button */}
      {currentIndex < images.length - 1 && (
        <Button
          variant="ghost"
          size="icon"
          className="absolute right-4 text-white hover:bg-white/20"
          onClick={() => onNavigate(currentIndex + 1)}
        >
          <ChevronRight className="h-8 w-8" />
        </Button>
      )}
      
      {/* Image */}
      <div className="relative max-w-7xl max-h-[90vh] mx-auto">
        <Image
          src={currentImage.fileUrl}
          alt={currentImage.altText}
          width={currentImage.width}
          height={currentImage.height}
          className="max-w-full max-h-[90vh] object-contain"
          priority
        />
      </div>
      
      {/* Bottom Bar */}
      <div className="absolute bottom-0 left-0 right-0 bg-black/80 p-4">
        <div className="max-w-7xl mx-auto flex items-center justify-between">
          <div className="flex-1">
            <p className="text-white text-sm">
              {currentIndex + 1} / {images.length}
            </p>
            {currentImage.caption && (
              <p className="text-white mt-1">{currentImage.caption}</p>
            )}
          </div>
          
          <div className="flex gap-2">
            <Button variant="ghost" size="icon" onClick={handleDownload} className="text-white">
              <Download className="h-5 w-5" />
            </Button>
            <Button variant="ghost" size="icon" onClick={handleShare} className="text-white">
              <Share2 className="h-5 w-5" />
            </Button>
          </div>
        </div>
      </div>
    </div>
  )
}
```

**Responsive Mobile Layout:**

```tsx
// On mobile, switch to simple grid layout
const isMobile = useMediaQuery('(max-width: 768px)')

if (isMobile) {
  return (
    <div className="grid grid-cols-2 gap-2">
      {images.map((image, index) => (
        <div key={image.id} onClick={() => openLightbox(image, index)}>
          <Image src={image.thumbnailUrl} alt={image.altText} ... />
        </div>
      ))}
    </div>
  )
}

// Desktop uses collage algorithm
return <AlbumCollage images={images} />
```

---

#### Facilities: `/[lang]/facilities`
- **Features:**
  - Interactive map with markers
  - List/Grid view toggle
  - Filters (city, region)
  - Search by name
- **Data Fetching:** CSR with React Query (real-time filters)

```tsx
'use client'
import { useFacilities } from '@/lib/hooks/useFacilities'
import FacilitiesMap from '@/components/maps/FacilitiesMap'

export default function FacilitiesPage() {
  const [filters, setFilters] = useState({ city: '', search: '' })
  const { data, isLoading } = useFacilities(filters)
  
  return (
    <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <FacilitiesMap facilities={data?.items} />
      <FacilitiesList facilities={data?.items} />
    </div>
  )
}
```

#### Certificate Validation: `/[lang]/certificates/validate`
- **Features:**
  - Simple input form
  - Real-time validation
  - Result display with status badge
- **Data Fetching:** CSR on form submit

```tsx
'use client'
export default function CertificateValidation() {
  const [certificateNumber, setCertificateNumber] = useState('')
  const [result, setResult] = useState(null)
  
  const handleValidate = async () => {
    const res = await validateCertificate(certificateNumber, lang)
    setResult(res)
  }
  
  return (
    <Card>
      <Input 
        value={certificateNumber} 
        onChange={(e) => setCertificateNumber(e.target.value)}
        placeholder={t('enterCertificateNumber')}
      />
      <Button onClick={handleValidate}>{t('validate')}</Button>
      
      {result && (
        <Alert variant={result.isValid ? 'success' : 'destructive'}>
          {result.message}
        </Alert>
      )}
    </Card>
  )
}
```

#### Dynamic CMS Pages: `/[lang]/[...slug]`
- **Features:**
  - Renders any page created via Page Builder
  - Block-based layout
- **Data Fetching:** SSG with ISR

---

### Admin Panel Pages (Protected Routes)

#### Dashboard: `/admin/dashboard`
- **Features:**
  - Quick stats cards
  - Recent activity feed
  - Charts (submissions, visitors)
- **Auth:** Requires 'Admin' or 'Editor' role

#### Album Management: `/admin/albums`
- **Features:**
  - List all albums with cover images
  - Create, Edit, Delete albums
  - Publish/Unpublish toggle
  - Reorder albums (drag & drop)

#### Album Editor: `/admin/albums/[id]/edit`
- **Features:**
  - Edit album title, description (AR/EN)
  - **Bulk Image Upload** with drag & drop
  - Upload progress bar with live updates
  - Image grid with reorder capability
  - Set cover image
  - Edit individual image captions
  - Delete images
  - Publish album

**Bulk Upload Component:**

```tsx
// components/admin/Albums/BulkImageUploader.tsx
'use client'
import { useState, useCallback } from 'react'
import { useDropzone } from 'react-dropzone'
import { Upload, X, Check, AlertCircle } from 'lucide-react'
import { Progress } from '@/components/ui/progress'
import { Button } from '@/components/ui/button'
import { Card } from '@/components/ui/card'

interface UploadFile {
  file: File
  id: string
  status: 'pending' | 'uploading' | 'completed' | 'error'
  progress: number
  error?: string
  preview?: string
}

export default function BulkImageUploader({ albumId }: { albumId: string }) {
  const [files, setFiles] = useState<UploadFile[]>([])
  const [isUploading, setIsUploading] = useState(false)
  
  const onDrop = useCallback((acceptedFiles: File[]) => {
    const newFiles: UploadFile[] = acceptedFiles.map(file => ({
      file,
      id: crypto.randomUUID(),
      status: 'pending',
      progress: 0,
      preview: URL.createObjectURL(file)
    }))
    
    setFiles(prev => [...prev, ...newFiles])
  }, [])
  
  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
    accept: {
      'image/*': ['.jpg', '.jpeg', '.png', '.gif', '.webp']
    },
    maxSize: 10 * 1024 * 1024, // 10MB
    multiple: true
  })
  
  const removeFile = (id: string) => {
    setFiles(prev => prev.filter(f => f.id !== id))
  }
  
  const uploadAll = async () => {
    setIsUploading(true)
    
    const formData = new FormData()
    files.forEach(f => {
      if (f.status === 'pending') {
        formData.append('files', f.file)
      }
    })
    
    try {
      // Start upload job
      const response = await fetch(`/api/albums/${albumId}/images/bulk-upload`, {
        method: 'POST',
        body: formData
      })
      
      const { jobId } = await response.json()
      
      // Poll for progress
      pollUploadProgress(jobId)
    } catch (error) {
      console.error('Upload failed:', error)
      setIsUploading(false)
    }
  }
  
  const pollUploadProgress = async (jobId: string) => {
    const interval = setInterval(async () => {
      const response = await fetch(`/api/albums/${albumId}/images/upload-progress/${jobId}`)
      const progress = await response.json()
      
      // Update progress for each file
      setFiles(prev => prev.map((f, i) => {
        if (i < progress.processedFiles) {
          return { ...f, status: 'completed', progress: 100 }
        } else if (i === progress.processedFiles) {
          return { ...f, status: 'uploading', progress: 50 }
        }
        return f
      }))
      
      if (progress.status === 'completed') {
        clearInterval(interval)
        setIsUploading(false)
        
        // Reload album images
        window.location.reload()
      }
      
      if (progress.status === 'failed') {
        clearInterval(interval)
        setIsUploading(false)
      }
    }, 1000)
  }
  
  const totalProgress = files.length > 0 
    ? files.reduce((acc, f) => acc + f.progress, 0) / files.length 
    : 0
  
  return (
    <div className="space-y-4">
      {/* Dropzone */}
      <div
        {...getRootProps()}
        className={`border-2 border-dashed rounded-lg p-12 text-center cursor-pointer transition-colors ${
          isDragActive ? 'border-blue-500 bg-blue-50' : 'border-gray-300 hover:border-gray-400'
        }`}
      >
        <input {...getInputProps()} />
        <Upload className="h-12 w-12 mx-auto mb-4 text-gray-400" />
        {isDragActive ? (
          <p className="text-blue-600 font-semibold">Drop images here...</p>
        ) : (
          <div>
            <p className="text-gray-700 font-semibold mb-2">
              Drag & drop images here, or click to browse
            </p>
            <p className="text-sm text-gray-500">
              Supports: JPG, PNG, GIF, WebP (max 10MB each, up to 50 images)
            </p>
          </div>
        )}
      </div>
      
      {/* File List */}
      {files.length > 0 && (
        <div>
          <div className="flex justify-between items-center mb-4">
            <h3 className="font-semibold">
              {files.length} {files.length === 1 ? 'image' : 'images'} selected
            </h3>
            <div className="flex gap-2">
              <Button variant="outline" onClick={() => setFiles([])}>
                Clear All
              </Button>
              <Button onClick={uploadAll} disabled={isUploading}>
                {isUploading ? 'Uploading...' : 'Upload All'}
              </Button>
            </div>
          </div>
          
          {isUploading && (
            <div className="mb-4">
              <div className="flex justify-between text-sm mb-2">
                <span>Overall Progress</span>
                <span>{Math.round(totalProgress)}%</span>
              </div>
              <Progress value={totalProgress} />
            </div>
          )}
          
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
            {files.map(file => (
              <Card key={file.id} className="relative overflow-hidden">
                <div className="aspect-square relative">
                  <img 
                    src={file.preview} 
                    alt={file.file.name}
                    className="w-full h-full object-cover"
                  />
                  
                  {/* Status Overlay */}
                  {file.status === 'uploading' && (
                    <div className="absolute inset-0 bg-black/60 flex items-center justify-center">
                      <Progress value={file.progress} className="w-3/4" />
                    </div>
                  )}
                  
                  {file.status === 'completed' && (
                    <div className="absolute inset-0 bg-green-500/80 flex items-center justify-center">
                      <Check className="h-8 w-8 text-white" />
                    </div>
                  )}
                  
                  {file.status === 'error' && (
                    <div className="absolute inset-0 bg-red-500/80 flex items-center justify-center">
                      <AlertCircle className="h-8 w-8 text-white" />
                    </div>
                  )}
                  
                  {/* Remove Button */}
                  {file.status === 'pending' && (
                    <button
                      onClick={() => removeFile(file.id)}
                      className="absolute top-2 right-2 bg-black/60 rounded-full p-1 hover:bg-black/80"
                    >
                      <X className="h-4 w-4 text-white" />
                    </button>
                  )}
                </div>
                
                <div className="p-2">
                  <p className="text-xs truncate">{file.file.name}</p>
                  <p className="text-xs text-gray-500">
                    {(file.file.size / 1024 / 1024).toFixed(2)} MB
                  </p>
                </div>
              </Card>
            ))}
          </div>
        </div>
      )}
    </div>
  )
}
```

**Image Grid with Reorder:**

```tsx
// components/admin/Albums/AlbumImagesGrid.tsx
'use client'
import { useState } from 'react'
import { DndContext, closestCenter, PointerSensor, useSensor, useSensors } from '@dnd-kit/core'
import { SortableContext, rectSortingStrategy } from '@dnd-kit/sortable'
import { SortableImage } from './SortableImage'
import { Button } from '@/components/ui/button'

export default function AlbumImagesGrid({ albumId, images: initialImages }: any) {
  const [images, setImages] = useState(initialImages)
  const sensors = useSensors(useSensor(PointerSensor))
  
  const handleDragEnd = (event: any) => {
    const { active, over } = event
    if (!over || active.id === over.id) return
    
    const oldIndex = images.findIndex((img: any) => img.id === active.id)
    const newIndex = images.findIndex((img: any) => img.id === over.id)
    
    const newImages = [...images]
    const [moved] = newImages.splice(oldIndex, 1)
    newImages.splice(newIndex, 0, moved)
    
    setImages(newImages)
  }
  
  const saveOrder = async () => {
    await fetch(`/api/albums/${albumId}/images/reorder`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        imageOrders: images.map((img: any, i: number) => ({
          imageId: img.id,
          displayOrder: i + 1
        }))
      })
    })
  }
  
  return (
    <div>
      <div className="flex justify-between mb-4">
        <h3 className="text-lg font-semibold">{images.length} Images</h3>
        <Button onClick={saveOrder}>Save Order</Button>
      </div>
      
      <DndContext sensors={sensors} collisionDetection={closestCenter} onDragEnd={handleDragEnd}>
        <SortableContext items={images.map((img: any) => img.id)} strategy={rectSortingStrategy}>
          <div className="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-6 gap-4">
            {images.map((image: any) => (
              <SortableImage key={image.id} image={image} albumId={albumId} />
            ))}
          </div>
        </SortableContext>
      </DndContext>
    </div>
  )
}
```

---

#### Layout Builder: `/admin/layouts`
- **Features:**
  - List all layouts with previews
  - Create new layout from scratch
  - Duplicate existing layout
  - Visual layout editor with drag & drop
  - Preview layout in real-time
  - Set default layout per content type

#### Layout Editor: `/admin/layouts/new` or `/admin/layouts/[id]/edit`
- **Features:**
  - **Visual Layout Builder** with drag & drop
  - Left Panel: Available elements (Container, Text, Image, Date, Tags, Custom Component)
  - Center Canvas: Live preview of layout
  - Right Panel: Element properties (className, tag, field mapping)
  - Grid system selector (1-column, 2-column, 3-column, custom)
  - CSS class input (Tailwind classes)
  - Save layout structure as JSON
  - Generate preview screenshot automatically

**Layout Builder Component:**

```tsx
// app/admin/layouts/new/page.tsx
'use client'
import { useState } from 'react'
import { DndContext, closestCenter, PointerSensor, useSensor, useSensors } from '@dnd-kit/core'
import { SortableContext, verticalListSortingStrategy } from '@dnd-kit/sortable'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import ElementsPanel from '@/components/admin/LayoutBuilder/ElementsPanel'
import LayoutCanvas from '@/components/admin/LayoutBuilder/LayoutCanvas'
import ElementPropertiesPanel from '@/components/admin/LayoutBuilder/ElementPropertiesPanel'
import { useLayoutBuilderStore } from '@/lib/stores/layoutBuilderStore'

export default function LayoutBuilderPage() {
  const { layoutName, displayName, contentTypeId, sections, saveLayout } = useLayoutBuilderStore()
  const [isSaving, setIsSaving] = useState(false)
  const sensors = useSensors(useSensor(PointerSensor))
  
  const handleSave = async () => {
    setIsSaving(true)
    await saveLayout()
    setIsSaving(false)
  }
  
  return (
    <div className="h-screen flex flex-col">
      {/* Top Bar */}
      <header className="border-b p-4 bg-white">
        <div className="max-w-7xl mx-auto grid grid-cols-3 gap-4 items-end">
          <div>
            <Label>Layout Name (Technical)</Label>
            <Input
              value={layoutName}
              onChange={(e) => useLayoutBuilderStore.setState({ layoutName: e.target.value })}
              placeholder="e.g., FeaturedArticleLayout"
            />
          </div>
          
          <div>
            <Label>Display Name (عربي)</Label>
            <Input
              value={displayName}
              onChange={(e) => useLayoutBuilderStore.setState({ displayName: e.target.value })}
              placeholder="e.g., تخطيط المقال المميز"
            />
          </div>
          
          <div>
            <Label>Content Type (Optional)</Label>
            <Select value={contentTypeId} onValueChange={(val) => useLayoutBuilderStore.setState({ contentTypeId: val })}>
              <SelectTrigger>
                <SelectValue placeholder="All Content Types" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="">All Content Types</SelectItem>
                <SelectItem value="news">News</SelectItem>
                <SelectItem value="events">Events</SelectItem>
                <SelectItem value="services">Services</SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>
        
        <div className="max-w-7xl mx-auto flex justify-end gap-2 mt-4">
          <Button variant="outline">Preview</Button>
          <Button onClick={handleSave} disabled={isSaving}>
            {isSaving ? 'Saving...' : 'Save Layout'}
          </Button>
        </div>
      </header>
      
      {/* Main Layout Builder */}
      <div className="flex-1 flex overflow-hidden">
        {/* Left Panel: Elements */}
        <ElementsPanel />
        
        {/* Center: Canvas */}
        <main className="flex-1 overflow-auto bg-gray-100 p-8">
          <div className="max-w-4xl mx-auto bg-white rounded-lg shadow-lg p-8">
            <h2 className="text-lg font-semibold mb-4 text-gray-500">Layout Preview</h2>
            
            <DndContext sensors={sensors} collisionDetection={closestCenter}>
              <SortableContext items={sections.map((s: any) => s.id)} strategy={verticalListSortingStrategy}>
                <LayoutCanvas />
              </SortableContext>
            </DndContext>
          </div>
        </main>
        
        {/* Right Panel: Properties */}
        <ElementPropertiesPanel />
      </div>
    </div>
  )
}
```

**Elements Panel:**

```tsx
// components/admin/LayoutBuilder/ElementsPanel.tsx
'use client'
import { Button } from '@/components/ui/button'
import { useLayoutBuilderStore } from '@/lib/stores/layoutBuilderStore'
import {
  Type, Image as ImageIcon, Calendar, Tags, Box, Grid3x3, Rows
} from 'lucide-react'

const elements = [
  { type: 'container', label: 'Container', icon: Box, description: 'Group elements' },
  { type: 'field-text', label: 'Text Field', icon: Type, description: 'title, body, etc.' },
  { type: 'field-image', label: 'Image Field', icon: ImageIcon, description: 'Featured image' },
  { type: 'field-date', label: 'Date Field', icon: Calendar, description: 'publishedAt, etc.' },
  { type: 'field-array', label: 'Array Field', icon: Tags, description: 'Tags, categories' },
  { type: 'grid', label: '2-Column Grid', icon: Grid3x3, description: 'Side-by-side layout' },
  { type: 'divider', label: 'Divider', icon: Rows, description: 'Horizontal line' }
]

export default function ElementsPanel() {
  const addSection = useLayoutBuilderStore((state: any) => state.addSection)
  
  return (
    <aside className="w-64 border-r bg-white p-4 overflow-auto">
      <h2 className="text-lg font-semibold mb-4">Elements</h2>
      <p className="text-sm text-gray-500 mb-4">Drag or click to add</p>
      
      <div className="space-y-2">
        {elements.map((element) => (
          <Button
            key={element.type}
            variant="outline"
            className="w-full justify-start text-left h-auto py-3"
            onClick={() => addSection(element.type)}
          >
            <element.icon className="h-4 w-4 mr-2 flex-shrink-0" />
            <div className="flex-1">
              <div className="font-medium">{element.label}</div>
              <div className="text-xs text-gray-500">{element.description}</div>
            </div>
          </Button>
        ))}
      </div>
      
      <div className="mt-8 p-4 bg-blue-50 rounded-lg">
        <h3 className="font-semibold mb-2 text-sm">💡 Tips</h3>
        <ul className="text-xs space-y-1 text-gray-600">
          <li>• Use containers to group elements</li>
          <li>• Map fields to content data</li>
          <li>• Use Tailwind classes for styling</li>
          <li>• Preview layout before saving</li>
        </ul>
      </div>
    </aside>
  )
}
```

**Layout Builder Store:**

```tsx
// lib/stores/layoutBuilderStore.ts
import { create } from 'zustand'

interface Section {
  id: string
  type: string
  field?: string
  tag?: string
  className?: string
  children?: Section[]
}

interface LayoutBuilderState {
  layoutName: string
  displayName: string
  description: string
  contentTypeId: string
  sections: Section[]
  selectedSectionId: string | null
  
  addSection: (type: string) => void
  removeSection: (id: string) => void
  updateSection: (id: string, updates: Partial<Section>) => void
  selectSection: (id: string | null) => void
  saveLayout: () => Promise<void>
}

export const useLayoutBuilderStore = create<LayoutBuilderState>((set, get) => ({
  layoutName: '',
  displayName: '',
  description: '',
  contentTypeId: '',
  sections: [],
  selectedSectionId: null,
  
  addSection: (type) => {
    const id = crypto.randomUUID()
    
    let newSection: Section = { id, type, className: '' }
    
    switch (type) {
      case 'container':
        newSection = { id, type: 'container', className: 'mb-8', children: [] }
        break
      
      case 'field-text':
        newSection = {
          id,
          type: 'field',
          field: 'title',
          tag: 'h1',
          className: 'text-4xl font-bold mb-4'
        }
        break
      
      case 'field-image':
        newSection = {
          id,
          type: 'field',
          field: 'featuredImage',
          tag: 'picture',
          className: 'w-full rounded-lg overflow-hidden my-8'
        }
        break
      
      case 'field-date':
        newSection = {
          id,
          type: 'field',
          field: 'publishedAt',
          tag: 'span',
          className: 'text-sm text-gray-600'
        }
        break
      
      case 'field-array':
        newSection = {
          id,
          type: 'field',
          field: 'tags',
          tag: 'div',
          className: 'flex gap-2 flex-wrap'
        }
        break
      
      case 'grid':
        newSection = {
          id,
          type: 'container',
          className: 'grid grid-cols-1 md:grid-cols-2 gap-8',
          children: []
        }
        break
    }
    
    set((state) => ({
      sections: [...state.sections, newSection],
      selectedSectionId: id
    }))
  },
  
  removeSection: (id) => set((state) => ({
    sections: state.sections.filter(s => s.id !== id),
    selectedSectionId: state.selectedSectionId === id ? null : state.selectedSectionId
  })),
  
  updateSection: (id, updates) => set((state) => ({
    sections: state.sections.map(s => s.id === id ? { ...s, ...updates } : s)
  })),
  
  selectSection: (id) => set({ selectedSectionId: id }),
  
  saveLayout: async () => {
    const { layoutName, displayName, description, contentTypeId, sections } = get()
    
    const layoutStructure = {
      version: '1.0',
      container: { className: 'max-w-4xl mx-auto py-12' },
      sections
    }
    
    const response = await fetch('/api/layouts', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        name: layoutName,
        displayName,
        description,
        contentTypeId: contentTypeId || null,
        layoutStructure
      })
    })
    
    if (response.ok) {
      alert('Layout saved successfully!')
    }
  }
}))
```

---

#### Content Management: `/admin/content`
- **Features:**
  - DataTable with filters, search, pagination
  - Actions: Create, Edit, Delete, Publish, Move to different section
  - **Layout preview** for each content item
  - Language tabs for translations

```tsx
'use client'
export default function ContentManagement() {
  const { data: contentTypes } = useContentTypes()
  const [selectedType, setSelectedType] = useState('news')
  const { data: content, isLoading } = useContent(selectedType)
  
  return (
    <>
      <Tabs value={selectedType} onValueChange={setSelectedType}>
        {contentTypes?.map(type => (
          <TabsTrigger key={type.name} value={type.name}>
            {type.displayName}
          </TabsTrigger>
        ))}
      </Tabs>
      
      <DataTable
        columns={contentColumns}
        data={content?.items || []}
        actions={[
          { label: 'Edit', onClick: (row) => router.push(`/admin/content/${row.id}/edit`) },
          { label: 'Move', onClick: (row) => openMoveDialog(row) },
          { label: 'Delete', onClick: (row) => deleteContent(row.id) }
        ]}
      />
    </>
  )
}
```

#### Content Editor: `/admin/content/[id]/edit`
- **Features:**
  - Language tabs (AR/EN) for multilingual editing
  - **Layout Selector** (choose from available layouts)
  - Rich text editor (TinyMCE or Tiptap)
  - Featured image upload (with automatic WebP conversion)
  - **Social Media Metadata Editor:**
    - OG Title (can differ from meta title)
    - OG Description
    - OG Image (1200x630 recommended)
    - Twitter Card type
  - Custom fields based on content type
  - Preview button
  - Publish/Unpublish toggle

```tsx
// app/admin/content/[id]/edit/page.tsx
'use client'
import { useState } from 'react'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import { Button } from '@/components/ui/button'
import { Switch } from '@/components/ui/switch'
import RichTextEditor from '@/components/admin/RichTextEditor'
import MediaUploader from '@/components/admin/MediaUploader'
import LayoutSelector from '@/components/admin/LayoutSelector'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'

export default function ContentEditorPage({ params }: { params: { id: string } }) {
  const [content, setContent] = useState<any>(null)
  const [activeLanguage, setActiveLanguage] = useState('ar')
  const [isSaving, setIsSaving] = useState(false)
  
  // Load content
  useEffect(() => {
    loadContent()
  }, [params.id])
  
  const loadContent = async () => {
    const res = await fetch(`/api/content/news/${params.id}`)
    const data = await res.json()
    setContent(data)
  }
  
  const handleSave = async () => {
    setIsSaving(true)
    await fetch(`/api/content/news/${params.id}`, {
      method: 'PUT',
      body: JSON.stringify(content)
    })
    setIsSaving(false)
  }
  
  if (!content) return <div>Loading...</div>
  
  return (
    <div className="container mx-auto py-8">
      <div className="flex justify-between items-center mb-8">
        <h1 className="text-3xl font-bold">Edit Content</h1>
        <div className="flex gap-2">
          <Button variant="outline">Preview</Button>
          <Button onClick={handleSave} disabled={isSaving}>
            {isSaving ? 'Saving...' : 'Save'}
          </Button>
          <Button variant="outline">Publish</Button>
        </div>
      </div>
      
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* Main Content */}
        <div className="lg:col-span-2 space-y-6">
          {/* Language Tabs */}
          <Card>
            <CardContent className="pt-6">
              <Tabs value={activeLanguage} onValueChange={setActiveLanguage}>
                <TabsList className="grid w-full grid-cols-2 mb-4">
                  <TabsTrigger value="ar">العربية</TabsTrigger>
                  <TabsTrigger value="en">English</TabsTrigger>
                </TabsList>
                
                <TabsContent value="ar" className="space-y-4">
                  <div>
                    <Label>العنوان</Label>
                    <Input
                      value={content.translations.ar.title}
                      onChange={(e) => setContent({
                        ...content,
                        translations: {
                          ...content.translations,
                          ar: { ...content.translations.ar, title: e.target.value }
                        }
                      })}
                    />
                  </div>
                  
                  <div>
                    <Label>Slug</Label>
                    <Input value={content.translations.ar.slug} />
                  </div>
                  
                  <div>
                    <Label>المحتوى</Label>
                    <RichTextEditor
                      value={content.translations.ar.body}
                      onChange={(val) => setContent({
                        ...content,
                        translations: {
                          ...content.translations,
                          ar: { ...content.translations.ar, body: val }
                        }
                      })}
                    />
                  </div>
                </TabsContent>
                
                <TabsContent value="en" className="space-y-4">
                  {/* Same fields for English */}
                </TabsContent>
              </Tabs>
            </CardContent>
          </Card>
          
          {/* SEO & Social Media Metadata */}
          <Card>
            <CardHeader>
              <CardTitle>SEO & Social Media</CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              <Tabs value={activeLanguage} onValueChange={setActiveLanguage}>
                <TabsList className="grid w-full grid-cols-2">
                  <TabsTrigger value="ar">العربية</TabsTrigger>
                  <TabsTrigger value="en">English</TabsTrigger>
                </TabsList>
                
                <TabsContent value="ar" className="space-y-4 mt-4">
                  <div>
                    <Label>Meta Title</Label>
                    <Input placeholder="عنوان SEO" />
                  </div>
                  
                  <div>
                    <Label>Meta Description</Label>
                    <Textarea placeholder="وصف SEO" rows={3} />
                  </div>
                  
                  <hr className="my-4" />
                  
                  <div>
                    <Label>Open Graph Title (for social sharing)</Label>
                    <Input placeholder="يمكن أن يختلف عن meta title" />
                    <p className="text-xs text-gray-500 mt-1">
                      يظهر عند المشاركة على Facebook, LinkedIn, etc.
                    </p>
                  </div>
                  
                  <div>
                    <Label>Open Graph Description</Label>
                    <Textarea placeholder="وصف جذاب للمشاركة" rows={2} />
                  </div>
                  
                  <div>
                    <Label>Open Graph Image (1200x630 recommended)</Label>
                    <MediaUploader
                      value={content.translations.ar.ogImage}
                      onChange={(url) => setContent({
                        ...content,
                        translations: {
                          ...content.translations,
                          ar: { ...content.translations.ar, ogImage: url }
                        }
                      })}
                      aspectRatio="1200:630"
                    />
                    <p className="text-xs text-gray-500 mt-1">
                      صورة مخصصة للمشاركة (مختلفة عن الصورة البارزة)
                    </p>
                  </div>
                  
                  <div>
                    <Label>Twitter Card Type</Label>
                    <Select value={content.translations.ar.twitterCard}>
                      <SelectTrigger>
                        <SelectValue />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="summary">Summary</SelectItem>
                        <SelectItem value="summary_large_image">Summary Large Image</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                </TabsContent>
                
                <TabsContent value="en" className="space-y-4 mt-4">
                  {/* Same fields for English */}
                </TabsContent>
              </Tabs>
            </CardContent>
          </Card>
        </div>
        
        {/* Sidebar */}
        <div className="space-y-6">
          {/* Layout Selector */}
          <Card>
            <CardHeader>
              <CardTitle>Layout</CardTitle>
            </CardHeader>
            <CardContent>
              <LayoutSelector
                contentType="news"
                selectedLayoutId={content.layoutId}
                onChange={(layoutId) => setContent({ ...content, layoutId })}
              />
            </CardContent>
          </Card>
          
          {/* Featured Image */}
          <Card>
            <CardHeader>
              <CardTitle>Featured Image</CardTitle>
            </CardHeader>
            <CardContent>
              <MediaUploader
                value={content.customFields.featuredImage}
                onChange={(media) => setContent({
                  ...content,
                  customFields: { ...content.customFields, featuredImage: media }
                })}
              />
              {content.customFields.featuredImage && (
                <div className="mt-2 text-xs text-gray-600">
                  <p>Original: {(content.customFields.featuredImage.fileSize / 1024 / 1024).toFixed(2)} MB</p>
                  <p className="text-green-600">
                    WebP: {(content.customFields.featuredImage.webPFileSize / 1024 / 1024).toFixed(2)} MB
                    ({Math.round((1 - content.customFields.featuredImage.webPFileSize / content.customFields.featuredImage.fileSize) * 100)}% smaller)
                  </p>
                </div>
              )}
            </CardContent>
          </Card>
          
          {/* Publish */}
          <Card>
            <CardHeader>
              <CardTitle>Publish</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="flex items-center justify-between">
                <Label>Published</Label>
                <Switch checked={content.isPublished} />
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  )
}
```

---

#### Page Builder: `/admin/pages/[id]/builder`
- **Features:**
  - Drag & drop interface (dnd-kit)
  - Left Panel: Available blocks
  - Center Canvas: Page preview with draggable blocks
  - Right Panel: Selected block properties editor
  - Top Bar: Language switcher, Save, Publish, Preview
- **Implementation:** See detailed section below

#### Form Builder: `/admin/forms/new`
- **Features:**
  - **Drag & Drop Field Builder** (dnd-kit)
  - Left Panel: Available field types (Text, Email, Number, Select, Checkbox, Radio, File, Date, Textarea)
  - Center Canvas: Form preview with draggable fields
  - Right Panel: Field properties editor
    - Basic: Label, Placeholder, Help Text, Required, Width
    - Validation Rules: Add multiple validations (min/max length, pattern, custom)
    - Conditional Logic: Show/hide field based on another field's value
  - Visual Validation Editor:
    - Click "Add Validation" → Select rule type → Enter value → Custom error message
    - Validation types: Required, Min Length, Max Length, Email, URL, Pattern (Regex), Min/Max (numbers), File Size, File Type
  - Conditional Logic Builder:
    - Select trigger field → Choose condition (equals, not equals, contains, greater than, less than) → Enter value → Select action (show/hide)
  - Multi-column Layout: Set field width (25%, 50%, 75%, 100%)
  - Preview Mode: See how form looks to end users
  - Test Validation: Fill form and test all validations
  - Save & Publish

**Detailed Implementation:**

```tsx
// app/admin/forms/new/page.tsx
'use client'
import { useState } from 'react'
import { DndContext, closestCenter, PointerSensor, useSensor, useSensors } from '@dnd-kit/core'
import { SortableContext, verticalListSortingStrategy } from '@dnd-kit/sortable'
import FieldTypesPanel from '@/components/admin/FormBuilder/FieldTypesPanel'
import FormCanvas from '@/components/admin/FormBuilder/FormCanvas'
import FieldPropertiesPanel from '@/components/admin/FormBuilder/FieldPropertiesPanel'
import { useFormBuilderStore } from '@/lib/stores/formBuilderStore'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'

export default function FormBuilderPage() {
  const { fields, formName, setFormName, saveForm } = useFormBuilderStore()
  const [isSaving, setIsSaving] = useState(false)
  const sensors = useSensors(useSensor(PointerSensor))

  const handleSave = async () => {
    setIsSaving(true)
    await saveForm()
    setIsSaving(false)
  }

  return (
    <div className="h-screen flex flex-col">
      {/* Top Bar */}
      <header className="border-b p-4 flex items-center justify-between">
        <div className="flex-1">
          <Label>Form Name</Label>
          <Input 
            value={formName} 
            onChange={(e) => setFormName(e.target.value)}
            placeholder="e.g., Contact Form"
            className="max-w-md"
          />
        </div>
        
        <div className="flex gap-2">
          <Button variant="outline">Preview</Button>
          <Button variant="outline">Test Validations</Button>
          <Button onClick={handleSave} disabled={isSaving}>
            {isSaving ? 'Saving...' : 'Save Form'}
          </Button>
          <Button>Publish</Button>
        </div>
      </header>

      {/* Main Layout */}
      <div className="flex-1 flex overflow-hidden">
        {/* Left Panel: Field Types */}
        <FieldTypesPanel />

        {/* Center: Canvas */}
        <main className="flex-1 overflow-auto bg-gray-50 p-8">
          <DndContext sensors={sensors} collisionDetection={closestCenter}>
            <SortableContext items={fields.map(f => f.id)} strategy={verticalListSortingStrategy}>
              <FormCanvas />
            </SortableContext>
          </DndContext>
        </main>

        {/* Right Panel: Properties */}
        <FieldPropertiesPanel />
      </div>
    </div>
  )
}
```

**Field Types Panel:**
```tsx
// components/admin/FormBuilder/FieldTypesPanel.tsx
'use client'
import { useFormBuilderStore } from '@/lib/stores/formBuilderStore'
import { Button } from '@/components/ui/button'
import { 
  Type, Mail, Hash, Calendar, FileText, CheckSquare, 
  Circle, Upload, AlignLeft, List 
} from 'lucide-react'

const fieldTypes = [
  { type: 'text', label: 'Text', icon: Type },
  { type: 'email', label: 'Email', icon: Mail },
  { type: 'number', label: 'Number', icon: Hash },
  { type: 'date', label: 'Date', icon: Calendar },
  { type: 'textarea', label: 'Textarea', icon: AlignLeft },
  { type: 'select', label: 'Dropdown', icon: List },
  { type: 'checkbox', label: 'Checkbox', icon: CheckSquare },
  { type: 'radio', label: 'Radio', icon: Circle },
  { type: 'file', label: 'File Upload', icon: Upload }
]

export default function FieldTypesPanel() {
  const addField = useFormBuilderStore((state) => state.addField)

  return (
    <aside className="w-64 border-r p-4 overflow-auto">
      <h2 className="text-lg font-semibold mb-4">Field Types</h2>
      <p className="text-sm text-gray-500 mb-4">Drag or click to add fields</p>

      <div className="space-y-2">
        {fieldTypes.map((fieldType) => (
          <Button
            key={fieldType.type}
            variant="outline"
            className="w-full justify-start"
            onClick={() => addField(fieldType.type)}
          >
            <fieldType.icon className="h-4 w-4 mr-2" />
            {fieldType.label}
          </Button>
        ))}
      </div>

      <div className="mt-8 p-4 bg-blue-50 rounded-lg">
        <h3 className="font-semibold mb-2">💡 Tips</h3>
        <ul className="text-sm space-y-1 text-gray-600">
          <li>• Drag fields to reorder</li>
          <li>• Click field to edit properties</li>
          <li>• Add validations in right panel</li>
          <li>• Set conditional logic for dynamic forms</li>
        </ul>
      </div>
    </aside>
  )
}
```

**Field Properties Panel with Validation & Conditional Logic:**
```tsx
// components/admin/FormBuilder/FieldPropertiesPanel.tsx
'use client'
import { useFormBuilderStore } from '@/lib/stores/formBuilderStore'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import { Switch } from '@/components/ui/switch'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Button } from '@/components/ui/button'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Plus, Trash2 } from 'lucide-react'

export default function FieldPropertiesPanel() {
  const { fields, selectedFieldId, updateField, addValidation, removeValidation, addConditionalLogic } = useFormBuilderStore()
  const selectedField = fields.find(f => f.id === selectedFieldId)

  if (!selectedField) {
    return (
      <aside className="w-96 border-l p-4">
        <p className="text-gray-400">Select a field to edit its properties</p>
      </aside>
    )
  }

  return (
    <aside className="w-96 border-l p-4 overflow-auto">
      <h2 className="text-lg font-semibold mb-4">Field Properties</h2>

      <Tabs defaultValue="basic">
        <TabsList className="grid w-full grid-cols-3">
          <TabsTrigger value="basic">Basic</TabsTrigger>
          <TabsTrigger value="validation">Validation</TabsTrigger>
          <TabsTrigger value="logic">Logic</TabsTrigger>
        </TabsList>

        {/* Basic Properties */}
        <TabsContent value="basic" className="space-y-4">
          <div>
            <Label>Field Type</Label>
            <Input value={selectedField.fieldType} disabled className="bg-gray-100" />
          </div>

          <div>
            <Label>Field Name (Technical)</Label>
            <Input 
              value={selectedField.fieldName}
              onChange={(e) => updateField(selectedField.id, { fieldName: e.target.value })}
              placeholder="e.g., fullName"
            />
            <p className="text-xs text-gray-500 mt-1">No spaces, used in code</p>
          </div>

          <div>
            <Label>Label</Label>
            <Input 
              value={selectedField.label}
              onChange={(e) => updateField(selectedField.id, { label: e.target.value })}
              placeholder="e.g., Full Name"
            />
          </div>

          <div>
            <Label>Placeholder</Label>
            <Input 
              value={selectedField.placeholder || ''}
              onChange={(e) => updateField(selectedField.id, { placeholder: e.target.value })}
              placeholder="e.g., Enter your full name"
            />
          </div>

          <div>
            <Label>Help Text</Label>
            <Textarea 
              value={selectedField.helpText || ''}
              onChange={(e) => updateField(selectedField.id, { helpText: e.target.value })}
              placeholder="Additional instructions for users"
              rows={2}
            />
          </div>

          <div className="flex items-center justify-between">
            <Label>Required Field</Label>
            <Switch 
              checked={selectedField.isRequired}
              onCheckedChange={(checked) => updateField(selectedField.id, { isRequired: checked })}
            />
          </div>

          <div>
            <Label>Width</Label>
            <Select 
              value={selectedField.width?.toString() || '100'}
              onValueChange={(value) => updateField(selectedField.id, { width: parseInt(value) })}
            >
              <SelectTrigger>
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="25">25% (Quarter)</SelectItem>
                <SelectItem value="50">50% (Half)</SelectItem>
                <SelectItem value="75">75% (Three Quarters)</SelectItem>
                <SelectItem value="100">100% (Full Width)</SelectItem>
              </SelectContent>
            </Select>
          </div>

          {/* Field-Specific Options */}
          {selectedField.fieldType === 'select' && (
            <div>
              <Label>Dropdown Options</Label>
              <OptionsEditor field={selectedField} updateField={updateField} />
            </div>
          )}

          {selectedField.fieldType === 'file' && (
            <>
              <div>
                <Label>Max File Size (MB)</Label>
                <Input 
                  type="number"
                  value={selectedField.fieldOptions?.maxSize ? selectedField.fieldOptions.maxSize / 1048576 : 5}
                  onChange={(e) => updateField(selectedField.id, { 
                    fieldOptions: { 
                      ...selectedField.fieldOptions, 
                      maxSize: parseInt(e.target.value) * 1048576 
                    }
                  })}
                />
              </div>
              <div>
                <Label>Allowed File Types</Label>
                <Input 
                  value={selectedField.fieldOptions?.allowedTypes?.join(', ') || 'pdf, jpg, png'}
                  onChange={(e) => updateField(selectedField.id, { 
                    fieldOptions: { 
                      ...selectedField.fieldOptions, 
                      allowedTypes: e.target.value.split(',').map(t => t.trim()) 
                    }
                  })}
                  placeholder="pdf, jpg, png"
                />
              </div>
            </>
          )}
        </TabsContent>

        {/* Validation Rules */}
        <TabsContent value="validation" className="space-y-4">
          <div className="flex justify-between items-center">
            <Label>Validation Rules</Label>
            <Button size="sm" onClick={() => addValidation(selectedField.id)}>
              <Plus className="h-4 w-4 mr-1" /> Add Rule
            </Button>
          </div>

          {selectedField.validationRules?.map((rule, index) => (
            <Card key={index}>
              <CardContent className="pt-4 space-y-3">
                <div className="flex justify-between items-center">
                  <Label>Rule Type</Label>
                  <Button 
                    size="sm" 
                    variant="ghost" 
                    onClick={() => removeValidation(selectedField.id, index)}
                  >
                    <Trash2 className="h-4 w-4 text-red-500" />
                  </Button>
                </div>

                <Select 
                  value={rule.ruleType}
                  onValueChange={(value) => updateField(selectedField.id, {
                    validationRules: selectedField.validationRules?.map((r, i) => 
                      i === index ? { ...r, ruleType: value } : r
                    )
                  })}
                >
                  <SelectTrigger>
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="required">Required</SelectItem>
                    <SelectItem value="minLength">Min Length</SelectItem>
                    <SelectItem value="maxLength">Max Length</SelectItem>
                    <SelectItem value="min">Min Value (Number)</SelectItem>
                    <SelectItem value="max">Max Value (Number)</SelectItem>
                    <SelectItem value="email">Email Format</SelectItem>
                    <SelectItem value="url">URL Format</SelectItem>
                    <SelectItem value="pattern">Regex Pattern</SelectItem>
                  </SelectContent>
                </Select>

                {!['required', 'email', 'url'].includes(rule.ruleType) && (
                  <div>
                    <Label>Value</Label>
                    <Input 
                      value={rule.ruleValue || ''}
                      onChange={(e) => updateField(selectedField.id, {
                        validationRules: selectedField.validationRules?.map((r, i) => 
                          i === index ? { ...r, ruleValue: e.target.value } : r
                        )
                      })}
                      placeholder={
                        rule.ruleType === 'pattern' ? 'e.g., ^[0-9]{10}$' : 
                        rule.ruleType.includes('Length') ? 'e.g., 10' : 
                        'Enter value'
                      }
                    />
                  </div>
                )}

                <div>
                  <Label>Error Message</Label>
                  <Input 
                    value={rule.errorMessage || ''}
                    onChange={(e) => updateField(selectedField.id, {
                      validationRules: selectedField.validationRules?.map((r, i) => 
                        i === index ? { ...r, errorMessage: e.target.value } : r
                      )
                    })}
                    placeholder="Custom error message"
                  />
                </div>
              </CardContent>
            </Card>
          ))}

          {(!selectedField.validationRules || selectedField.validationRules.length === 0) && (
            <p className="text-sm text-gray-400 text-center py-8">
              No validation rules. Click "Add Rule" to create one.
            </p>
          )}
        </TabsContent>

        {/* Conditional Logic */}
        <TabsContent value="logic" className="space-y-4">
          <div className="flex justify-between items-center">
            <Label>Conditional Logic</Label>
            <Button size="sm" onClick={() => addConditionalLogic(selectedField.id)}>
              <Plus className="h-4 w-4 mr-1" /> Add Logic
            </Button>
          </div>

          <p className="text-sm text-gray-500">
            Show or hide this field based on another field's value
          </p>

          {selectedField.conditionalLogic?.map((logic, index) => (
            <Card key={index}>
              <CardContent className="pt-4 space-y-3">
                <div>
                  <Label>Trigger Field</Label>
                  <Select 
                    value={logic.triggerFieldId}
                    onValueChange={(value) => updateField(selectedField.id, {
                      conditionalLogic: selectedField.conditionalLogic?.map((l, i) => 
                        i === index ? { ...l, triggerFieldId: value } : l
                      )
                    })}
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Select field" />
                    </SelectTrigger>
                    <SelectContent>
                      {fields
                        .filter(f => f.id !== selectedField.id)
                        .map(f => (
                          <SelectItem key={f.id} value={f.id}>
                            {f.label}
                          </SelectItem>
                        ))}
                    </SelectContent>
                  </Select>
                </div>

                <div>
                  <Label>Condition</Label>
                  <Select 
                    value={logic.condition}
                    onValueChange={(value) => updateField(selectedField.id, {
                      conditionalLogic: selectedField.conditionalLogic?.map((l, i) => 
                        i === index ? { ...l, condition: value } : l
                      )
                    })}
                  >
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="equals">Equals</SelectItem>
                      <SelectItem value="notEquals">Not Equals</SelectItem>
                      <SelectItem value="contains">Contains</SelectItem>
                      <SelectItem value="greaterThan">Greater Than</SelectItem>
                      <SelectItem value="lessThan">Less Than</SelectItem>
                    </SelectContent>
                  </Select>
                </div>

                <div>
                  <Label>Value</Label>
                  <Input 
                    value={logic.triggerValue}
                    onChange={(e) => updateField(selectedField.id, {
                      conditionalLogic: selectedField.conditionalLogic?.map((l, i) => 
                        i === index ? { ...l, triggerValue: e.target.value } : l
                      )
                    })}
                    placeholder="Enter value to compare"
                  />
                </div>

                <div>
                  <Label>Action</Label>
                  <Select 
                    value={logic.action}
                    onValueChange={(value) => updateField(selectedField.id, {
                      conditionalLogic: selectedField.conditionalLogic?.map((l, i) => 
                        i === index ? { ...l, action: value } : l
                      )
                    })}
                  >
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="show">Show Field</SelectItem>
                      <SelectItem value="hide">Hide Field</SelectItem>
                    </SelectContent>
                  </Select>
                </div>

                <Button 
                  variant="outline" 
                  size="sm" 
                  className="w-full"
                  onClick={() => updateField(selectedField.id, {
                    conditionalLogic: selectedField.conditionalLogic?.filter((_, i) => i !== index)
                  })}
                >
                  <Trash2 className="h-4 w-4 mr-1" /> Remove Logic
                </Button>
              </CardContent>
            </Card>
          ))}

          {(!selectedField.conditionalLogic || selectedField.conditionalLogic.length === 0) && (
            <p className="text-sm text-gray-400 text-center py-8">
              No conditional logic. Click "Add Logic" to create one.
            </p>
          )}
        </TabsContent>
      </Tabs>
    </aside>
  )
}
```

**Form Builder Store:**
```tsx
// lib/stores/formBuilderStore.ts
import { create } from 'zustand'

interface ValidationRule {
  ruleType: string
  ruleValue?: string
  errorMessage: string
}

interface ConditionalLogic {
  triggerFieldId: string
  condition: string
  triggerValue: string
  action: 'show' | 'hide'
}

interface FormField {
  id: string
  fieldType: string
  fieldName: string
  label: string
  placeholder?: string
  helpText?: string
  isRequired: boolean
  displayOrder: number
  width: number
  fieldOptions?: any
  validationRules?: ValidationRule[]
  conditionalLogic?: ConditionalLogic[]
}

interface FormBuilderState {
  formId?: string
  formName: string
  fields: FormField[]
  selectedFieldId: string | null
  
  setFormName: (name: string) => void
  addField: (fieldType: string) => void
  removeField: (id: string) => void
  updateField: (id: string, updates: Partial<FormField>) => void
  selectField: (id: string | null) => void
  reorderFields: (oldIndex: number, newIndex: number) => void
  
  addValidation: (fieldId: string) => void
  removeValidation: (fieldId: string, index: number) => void
  
  addConditionalLogic: (fieldId: string) => void
  
  saveForm: () => Promise<void>
  loadForm: (formId: string) => Promise<void>
}

export const useFormBuilderStore = create<FormBuilderState>((set, get) => ({
  formName: '',
  fields: [],
  selectedFieldId: null,
  
  setFormName: (name) => set({ formName: name }),
  
  addField: (fieldType) => {
    const id = crypto.randomUUID()
    const displayOrder = get().fields.length + 1
    
    const newField: FormField = {
      id,
      fieldType,
      fieldName: `field_${displayOrder}`,
      label: `New ${fieldType} Field`,
      isRequired: false,
      displayOrder,
      width: 100,
      validationRules: [],
      conditionalLogic: []
    }
    
    set((state) => ({
      fields: [...state.fields, newField],
      selectedFieldId: id
    }))
  },
  
  removeField: (id) => set((state) => ({
    fields: state.fields.filter(f => f.id !== id),
    selectedFieldId: state.selectedFieldId === id ? null : state.selectedFieldId
  })),
  
  updateField: (id, updates) => set((state) => ({
    fields: state.fields.map(f => f.id === id ? { ...f, ...updates } : f)
  })),
  
  selectField: (id) => set({ selectedFieldId: id }),
  
  reorderFields: (oldIndex, newIndex) => set((state) => {
    const newFields = [...state.fields]
    const [removed] = newFields.splice(oldIndex, 1)
    newFields.splice(newIndex, 0, removed)
    
    return {
      fields: newFields.map((f, i) => ({ ...f, displayOrder: i + 1 }))
    }
  }),
  
  addValidation: (fieldId) => set((state) => ({
    fields: state.fields.map(f => 
      f.id === fieldId 
        ? { 
            ...f, 
            validationRules: [
              ...(f.validationRules || []),
              { ruleType: 'required', errorMessage: 'This field is required' }
            ]
          }
        : f
    )
  })),
  
  removeValidation: (fieldId, index) => set((state) => ({
    fields: state.fields.map(f => 
      f.id === fieldId 
        ? { 
            ...f, 
            validationRules: f.validationRules?.filter((_, i) => i !== index)
          }
        : f
    )
  })),
  
  addConditionalLogic: (fieldId) => set((state) => ({
    fields: state.fields.map(f => 
      f.id === fieldId 
        ? { 
            ...f, 
            conditionalLogic: [
              ...(f.conditionalLogic || []),
              { 
                triggerFieldId: '', 
                condition: 'equals', 
                triggerValue: '', 
                action: 'show' 
              }
            ]
          }
        : f
    )
  })),
  
  saveForm: async () => {
    const { formId, formName, fields } = get()
    
    // Call API to save form
    const response = await fetch('/api/forms', {
      method: formId ? 'PUT' : 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ formName, fields })
    })
    
    const data = await response.json()
    set({ formId: data.id })
  },
  
  loadForm: async (formId) => {
    const response = await fetch(`/api/forms/${formId}`)
    const data = await response.json()
    
    set({
      formId: data.id,
      formName: data.name,
      fields: data.fields
    })
  }
}))
```

---

#### Preview Mode:**
```tsx
// components/admin/FormBuilder/FormPreview.tsx
'use client'
import { useFormBuilderStore } from '@/lib/stores/formBuilderStore'
import DynamicForm from '@/components/forms/DynamicForm'

export default function FormPreview() {
  const { fields } = useFormBuilderStore()
  
  return (
    <div className="max-w-3xl mx-auto p-8 bg-white rounded-lg shadow-lg">
      <DynamicForm fields={fields} onSubmit={(data) => console.log('Preview submission:', data)} />
    </div>
  )
}
```

---

## 🧩 Components Library

### Block Components (`components/blocks/`)

Each block has two modes:
1. **Preview Mode:** Rendered on public pages
2. **Editor Mode:** Rendered in Page Builder with edit controls

```tsx
// components/blocks/HeroBanner.tsx
interface HeroBannerProps {
  backgroundImage: string
  title: string
  subtitle: string
  ctaText: string
  ctaLink: string
  editorMode?: boolean
}

export default function HeroBanner({ 
  backgroundImage, 
  title, 
  subtitle, 
  ctaText, 
  ctaLink, 
  editorMode = false 
}: HeroBannerProps) {
  return (
    <section 
      className="relative h-[600px] bg-cover bg-center"
      style={{ backgroundImage: `url(${backgroundImage})` }}
    >
      <div className="absolute inset-0 bg-black/50 flex items-center justify-center">
        <div className="text-center text-white">
          <h1 className="text-5xl font-bold mb-4">{title}</h1>
          <p className="text-xl mb-8">{subtitle}</p>
          {!editorMode && (
            <Link href={ctaLink}>
              <Button size="lg">{ctaText}</Button>
            </Link>
          )}
        </div>
      </div>
    </section>
  )
}
```

#### Block Registry (`components/blocks/BlockRegistry.ts`)
```tsx
import HeroBanner from './HeroBanner'
import LatestNews from './LatestNews'
import FeaturedServices from './FeaturedServices'
// ... import all blocks

export const BlockRegistry: Record<string, React.ComponentType<any>> = {
  HeroBanner,
  LatestNews,
  FeaturedServices,
  StatisticsSection,
  TeamMembers,
  ContactForm,
  FacilitiesMap
}

export const BlockDefaultProps: Record<string, any> = {
  HeroBanner: {
    backgroundImage: '/images/default-hero.jpg',
    title: 'Welcome',
    subtitle: 'Your subtitle here',
    ctaText: 'Learn More',
    ctaLink: '#'
  },
  LatestNews: {
    title: 'Latest News',
    count: 6,
    layout: 'grid'
  }
  // ... defaults for all blocks
}
```

### UI Components (`components/ui/`)

Using **Shadcn/ui** for base components:
- Fully accessible (ARIA)
- Customizable via Tailwind
- TypeScript native

```bash
# Install Shadcn components
npx shadcn-ui@latest init
npx shadcn-ui@latest add button input card dialog table tabs form
```

### Custom Components

#### WebPImage Component (Automatic WebP with Fallback)
```tsx
// components/ui/WebPImage.tsx
'use client'
import Image from 'next/image'

interface WebPImageProps {
  webPUrl: string
  originalUrl: string
  alt: string
  width: number
  height: number
  className?: string
  priority?: boolean
  fill?: boolean
}

export default function WebPImage({
  webPUrl,
  originalUrl,
  alt,
  width,
  height,
  className,
  priority = false,
  fill = false
}: WebPImageProps) {
  return (
    <picture className={className}>
      <source srcSet={webPUrl} type="image/webp" />
      {fill ? (
        <Image
          src={originalUrl}
          alt={alt}
          fill
          className={className}
          priority={priority}
        />
      ) : (
        <Image
          src={originalUrl}
          alt={alt}
          width={width}
          height={height}
          className={className}
          priority={priority}
        />
      )}
    </picture>
  )
}

// Usage:
// <WebPImage
//   webPUrl="image.webp"
//   originalUrl="image.jpg"
//   alt="Description"
//   width={1920}
//   height={1080}
// />
```

#### Layout Selector (Choose Layout for Content)
```tsx
// components/admin/LayoutSelector.tsx
'use client'
import { useState, useEffect } from 'react'
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group'
import { Label } from '@/components/ui/label'
import { Card } from '@/components/ui/card'
import WebPImage from '@/components/ui/WebPImage'

interface LayoutSelectorProps {
  contentType: string
  selectedLayoutId?: string
  onChange: (layoutId: string) => void
}

export default function LayoutSelector({ contentType, selectedLayoutId, onChange }: LayoutSelectorProps) {
  const [layouts, setLayouts] = useState([])
  const [loading, setLoading] = useState(true)
  
  useEffect(() => {
    fetchLayouts()
  }, [contentType])
  
  const fetchLayouts = async () => {
    const response = await fetch(`/api/layouts?contentType=${contentType}`)
    const data = await response.json()
    setLayouts(data.items)
    setLoading(false)
    
    // Set default layout if none selected
    if (!selectedLayoutId) {
      const defaultLayout = data.items.find((l: any) => l.isDefault)
      if (defaultLayout) {
        onChange(defaultLayout.id)
      }
    }
  }
  
  if (loading) return <div>Loading layouts...</div>
  
  return (
    <div>
      <Label className="text-lg font-semibold mb-4 block">
        اختر التخطيط (Layout)
      </Label>
      
      <RadioGroup value={selectedLayoutId} onValueChange={onChange}>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {layouts.map((layout: any) => (
            <Card
              key={layout.id}
              className={`cursor-pointer transition-all ${
                selectedLayoutId === layout.id
                  ? 'ring-2 ring-blue-500 bg-blue-50'
                  : 'hover:bg-gray-50'
              }`}
              onClick={() => onChange(layout.id)}
            >
              <div className="p-4">
                <div className="flex items-start gap-3 mb-3">
                  <RadioGroupItem value={layout.id} id={layout.id} />
                  <div className="flex-1">
                    <Label htmlFor={layout.id} className="font-semibold cursor-pointer">
                      {layout.displayName}
                      {layout.isDefault && (
                        <span className="ml-2 text-xs bg-green-100 text-green-800 px-2 py-1 rounded">
                          افتراضي
                        </span>
                      )}
                    </Label>
                    <p className="text-sm text-gray-600 mt-1">
                      {layout.description}
                    </p>
                  </div>
                </div>
                
                {layout.previewImage && (
                  <div className="rounded-md overflow-hidden border">
                    <img
                      src={layout.previewImage}
                      alt={layout.displayName}
                      className="w-full h-auto"
                    />
                  </div>
                )}
              </div>
            </Card>
          ))}
        </div>
      </RadioGroup>
      
      <p className="text-sm text-gray-500 mt-4">
        💡 يمكنك إنشاء تخطيطات جديدة من قسم <a href="/admin/layouts" className="text-blue-600 hover:underline">إدارة التخطيطات</a>
      </p>
    </div>
  )
}
```

---

## 🗃️ State Management

### Navigation & Menu Components

#### Dynamic Header with Icons
```tsx
// components/Header.tsx
'use client'
import { useState, useEffect } from 'react'
import Link from 'next/link'
import { Menu, X, ChevronDown } from 'lucide-react'
import * as LucideIcons from 'lucide-react'
import { Button } from '@/components/ui/button'
import {
  NavigationMenu,
  NavigationMenuContent,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
  NavigationMenuTrigger,
} from '@/components/ui/navigation-menu'

interface MenuItem {
  id: string
  title: string
  description?: string
  icon?: {
    type: 'lucide' | 'emoji' | 'custom'
    name?: string
    emoji?: string
    svg?: string
    color?: string
    size?: string
    showIcon?: boolean
  }
  link: {
    url: string
    openInNewTab?: boolean
  }
  children?: MenuItem[]
}

export default function Header({ lang }: { lang: string }) {
  const [menu, setMenu] = useState<MenuItem[]>([])
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false)
  
  useEffect(() => {
    fetchMenu()
  }, [lang])
  
  const fetchMenu = async () => {
    const response = await fetch(`/api/menus/main/items?lang=${lang}`)
    const data = await response.json()
    setMenu(data.items)
  }
  
  const renderIcon = (icon?: MenuItem['icon']) => {
    if (!icon || !icon.showIcon) return null
    
    switch (icon.type) {
      case 'lucide':
        const IconComponent = (LucideIcons as any)[icon.name || 'Circle']
        return (
          <IconComponent
            className={`${getSizeClass(icon.size)}`}
            style={{ color: icon.color }}
          />
        )
      
      case 'emoji':
        return <span className="text-xl">{icon.emoji}</span>
      
      case 'custom':
        return <div dangerouslySetInnerHTML={{ __html: icon.svg || '' }} />
      
      default:
        return null
    }
  }
  
  const getSizeClass = (size?: string) => {
    switch (size) {
      case 'sm': return 'h-4 w-4'
      case 'md': return 'h-5 w-5'
      case 'lg': return 'h-6 w-6'
      case 'xl': return 'h-8 w-8'
      default: return 'h-5 w-5'
    }
  }
  
  return (
    <header className="sticky top-0 z-50 bg-white border-b shadow-sm">
      <div className="container mx-auto px-4">
        <div className="flex items-center justify-between h-16">
          {/* Logo */}
          <Link href={`/${lang}`} className="flex items-center gap-2">
            <img src="/logo.png" alt="GAHAR" className="h-10" />
            <span className="text-xl font-bold">غاهر</span>
          </Link>
          
          {/* Desktop Menu */}
          <NavigationMenu className="hidden lg:flex">
            <NavigationMenuList>
              {menu.map((item) => (
                item.children && item.children.length > 0 ? (
                  // Dropdown Menu
                  <NavigationMenuItem key={item.id}>
                    <NavigationMenuTrigger className="flex items-center gap-2">
                      {renderIcon(item.icon)}
                      <span>{item.title}</span>
                    </NavigationMenuTrigger>
                    <NavigationMenuContent>
                      <ul className="grid w-[400px] gap-3 p-4">
                        {item.children.map((child) => (
                          <li key={child.id}>
                            <NavigationMenuLink asChild>
                              <Link
                                href={child.link.url}
                                target={child.link.openInNewTab ? '_blank' : undefined}
                                className="block select-none space-y-1 rounded-md p-3 leading-none no-underline outline-none transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground"
                              >
                                <div className="flex items-center gap-2">
                                  {renderIcon(child.icon)}
                                  <div className="text-sm font-medium leading-none">
                                    {child.title}
                                  </div>
                                </div>
                                {child.description && (
                                  <p className="line-clamp-2 text-sm leading-snug text-muted-foreground">
                                    {child.description}
                                  </p>
                                )}
                              </Link>
                            </NavigationMenuLink>
                          </li>
                        ))}
                      </ul>
                    </NavigationMenuContent>
                  </NavigationMenuItem>
                ) : (
                  // Simple Link
                  <NavigationMenuItem key={item.id}>
                    <Link href={item.link.url} legacyBehavior passHref>
                      <NavigationMenuLink className="group inline-flex h-10 w-max items-center justify-center rounded-md bg-background px-4 py-2 text-sm font-medium transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground focus:outline-none disabled:pointer-events-none disabled:opacity-50">
                        <span className="flex items-center gap-2">
                          {renderIcon(item.icon)}
                          {item.title}
                        </span>
                      </NavigationMenuLink>
                    </Link>
                  </NavigationMenuItem>
                )
              ))}
            </NavigationMenuList>
          </NavigationMenu>
          
          {/* Mobile Menu Button */}
          <Button
            variant="ghost"
            size="icon"
            className="lg:hidden"
            onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
          >
            {mobileMenuOpen ? <X /> : <Menu />}
          </Button>
        </div>
        
        {/* Mobile Menu */}
        {mobileMenuOpen && (
          <div className="lg:hidden border-t py-4">
            {menu.map((item) => (
              <div key={item.id} className="mb-2">
                <Link
                  href={item.link.url}
                  className="flex items-center gap-3 px-4 py-3 rounded-md hover:bg-gray-100"
                  onClick={() => !item.children && setMobileMenuOpen(false)}
                >
                  {renderIcon(item.icon)}
                  <span className="font-medium">{item.title}</span>
                  {item.children && <ChevronDown className="ml-auto h-4 w-4" />}
                </Link>
                
                {item.children && (
                  <div className="ml-8 mt-2 space-y-1">
                    {item.children.map((child) => (
                      <Link
                        key={child.id}
                        href={child.link.url}
                        className="flex items-center gap-2 px-4 py-2 text-sm rounded-md hover:bg-gray-50"
                        onClick={() => setMobileMenuOpen(false)}
                      >
                        {renderIcon(child.icon)}
                        <span>{child.title}</span>
                      </Link>
                    ))}
                  </div>
                )}
              </div>
            ))}
          </div>
        )}
      </div>
    </header>
  )
}
```

#### Icon Picker Component (Admin)
```tsx
// components/admin/IconPicker.tsx
'use client'
import { useState, useEffect } from 'react'
import { Search, X } from 'lucide-react'
import * as LucideIcons from 'lucide-react'
import { Input } from '@/components/ui/input'
import { Button } from '@/components/ui/button'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Label } from '@/components/ui/label'
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group'
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog'
import { ScrollArea } from '@/components/ui/scroll-area'

interface IconPickerProps {
  value?: {
    type: 'lucide' | 'emoji' | 'custom'
    name?: string
    emoji?: string
    svg?: string
    color?: string
    size?: string
  }
  onChange: (icon: any) => void
}

export default function IconPicker({ value, onChange }: IconPickerProps) {
  const [open, setOpen] = useState(false)
  const [iconType, setIconType] = useState<'lucide' | 'emoji' | 'custom'>(value?.type || 'lucide')
  const [searchQuery, setSearchQuery] = useState('')
  const [selectedIcon, setSelectedIcon] = useState(value?.name || 'Circle')
  const [selectedEmoji, setSelectedEmoji] = useState(value?.emoji || '⭐')
  const [customSvg, setCustomSvg] = useState(value?.svg || '')
  const [iconColor, setIconColor] = useState(value?.color || '#1E40AF')
  const [iconSize, setIconSize] = useState(value?.size || 'md')
  
  // Get all Lucide icons
  const allIcons = Object.keys(LucideIcons).filter(
    key => key !== 'createLucideIcon' && key !== 'default'
  )
  
  const filteredIcons = allIcons.filter(icon =>
    icon.toLowerCase().includes(searchQuery.toLowerCase())
  )
  
  const commonEmojis = [
    '⭐', '🏠', '📧', '📞', '📅', '📰', '🎓', '🏥', '💼', '🔔',
    '⚙️', '❤️', '✅', '❌', '📝', '📊', '🔍', '🌍', '🚀', '💡',
    '📚', '🎯', '🏆', '👥', '📱', '💻', '🎨', '📷', '🎵', '⚡'
  ]
  
  const handleSave = () => {
    const icon = {
      type: iconType,
      ...(iconType === 'lucide' && { name: selectedIcon }),
      ...(iconType === 'emoji' && { emoji: selectedEmoji }),
      ...(iconType === 'custom' && { svg: customSvg }),
      color: iconColor,
      size: iconSize,
      showIcon: true
    }
    onChange(icon)
    setOpen(false)
  }
  
  const renderSelectedIcon = () => {
    if (!value) return <div className="h-10 w-10 bg-gray-100 rounded flex items-center justify-center text-gray-400">?</div>
    
    switch (value.type) {
      case 'lucide':
        const IconComponent = (LucideIcons as any)[value.name || 'Circle']
        return <IconComponent className="h-6 w-6" style={{ color: value.color }} />
      
      case 'emoji':
        return <span className="text-2xl">{value.emoji}</span>
      
      case 'custom':
        return <div dangerouslySetInnerHTML={{ __html: value.svg || '' }} className="h-6 w-6" />
      
      default:
        return null
    }
  }
  
  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button variant="outline" className="w-full justify-start">
          <div className="flex items-center gap-3">
            {renderSelectedIcon()}
            <span className="text-sm">
              {value ? `${value.type}: ${value.name || value.emoji || 'Custom'}` : 'اختر أيقونة'}
            </span>
          </div>
        </Button>
      </DialogTrigger>
      
      <DialogContent className="max-w-2xl max-h-[90vh]">
        <DialogHeader>
          <DialogTitle>اختر أيقونة</DialogTitle>
        </DialogHeader>
        
        <Tabs value={iconType} onValueChange={(val: any) => setIconType(val)}>
          <TabsList className="grid w-full grid-cols-3">
            <TabsTrigger value="lucide">Lucide Icons</TabsTrigger>
            <TabsTrigger value="emoji">Emoji</TabsTrigger>
            <TabsTrigger value="custom">Custom SVG</TabsTrigger>
          </TabsList>
          
          {/* Lucide Icons */}
          <TabsContent value="lucide" className="space-y-4">
            <Input
              placeholder="ابحث عن أيقونة... (home, user, settings)"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="mb-4"
            />
            
            <ScrollArea className="h-[400px] border rounded-md p-4">
              <div className="grid grid-cols-8 gap-2">
                {filteredIcons.slice(0, 200).map((iconName) => {
                  const Icon = (LucideIcons as any)[iconName]
                  return (
                    <button
                      key={iconName}
                      onClick={() => setSelectedIcon(iconName)}
                      className={`p-3 rounded-md hover:bg-gray-100 flex items-center justify-center transition-colors ${
                        selectedIcon === iconName ? 'bg-blue-100 ring-2 ring-blue-500' : ''
                      }`}
                      title={iconName}
                    >
                      <Icon className="h-5 w-5" />
                    </button>
                  )
                })}
              </div>
            </ScrollArea>
            
            <div className="text-sm text-gray-500">
              مختار: <strong>{selectedIcon}</strong> ({filteredIcons.length} أيقونة)
            </div>
          </TabsContent>
          
          {/* Emoji */}
          <TabsContent value="emoji" className="space-y-4">
            <div className="grid grid-cols-10 gap-2">
              {commonEmojis.map((emoji) => (
                <button
                  key={emoji}
                  onClick={() => setSelectedEmoji(emoji)}
                  className={`p-3 text-2xl rounded-md hover:bg-gray-100 transition-colors ${
                    selectedEmoji === emoji ? 'bg-blue-100 ring-2 ring-blue-500' : ''
                  }`}
                >
                  {emoji}
                </button>
              ))}
            </div>
            
            <div>
              <Label>أو أدخل Emoji مخصص:</Label>
              <Input
                value={selectedEmoji}
                onChange={(e) => setSelectedEmoji(e.target.value)}
                placeholder="📌"
                className="text-2xl"
              />
            </div>
          </TabsContent>
          
          {/* Custom SVG */}
          <TabsContent value="custom" className="space-y-4">
            <div>
              <Label>كود SVG:</Label>
              <textarea
                value={customSvg}
                onChange={(e) => setCustomSvg(e.target.value)}
                placeholder='<svg>...</svg>'
                className="w-full h-32 p-2 border rounded-md font-mono text-sm"
              />
            </div>
            
            <div>
              <Label>معاينة:</Label>
              <div className="p-4 border rounded-md">
                <div dangerouslySetInnerHTML={{ __html: customSvg }} />
              </div>
            </div>
          </TabsContent>
        </Tabs>
        
        {/* Icon Settings */}
        <div className="space-y-4 border-t pt-4">
          <div>
            <Label>اللون:</Label>
            <div className="flex gap-2">
              <Input
                type="color"
                value={iconColor}
                onChange={(e) => setIconColor(e.target.value)}
                className="w-20"
              />
              <Input
                type="text"
                value={iconColor}
                onChange={(e) => setIconColor(e.target.value)}
                placeholder="#1E40AF"
              />
            </div>
          </div>
          
          <div>
            <Label>الحجم:</Label>
            <RadioGroup value={iconSize} onValueChange={setIconSize} className="flex gap-4">
              <div className="flex items-center space-x-2">
                <RadioGroupItem value="sm" id="sm" />
                <Label htmlFor="sm">صغير</Label>
              </div>
              <div className="flex items-center space-x-2">
                <RadioGroupItem value="md" id="md" />
                <Label htmlFor="md">متوسط</Label>
              </div>
              <div className="flex items-center space-x-2">
                <RadioGroupItem value="lg" id="lg" />
                <Label htmlFor="lg">كبير</Label>
              </div>
              <div className="flex items-center space-x-2">
                <RadioGroupItem value="xl" id="xl" />
                <Label htmlFor="xl">كبير جداً</Label>
              </div>
            </RadioGroup>
          </div>
        </div>
        
        <div className="flex justify-end gap-2">
          <Button variant="outline" onClick={() => setOpen(false)}>إلغاء</Button>
          <Button onClick={handleSave}>حفظ</Button>
        </div>
      </DialogContent>
    </Dialog>
  )
}
```

#### Menu Manager (Admin)
```tsx
// app/admin/menus/[id]/edit/page.tsx
'use client'
import { useState, useEffect } from 'react'
import { DndContext, closestCenter } from '@dnd-kit/core'
import { SortableContext, verticalListSortingStrategy } from '@dnd-kit/sortable'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Card } from '@/components/ui/card'
import IconPicker from '@/components/admin/IconPicker'
import { Plus, GripVertical, Trash2, Edit } from 'lucide-react'

export default function MenuEditor({ params }: { params: { id: string } }) {
  const [menu, setMenu] = useState<any>(null)
  const [items, setItems] = useState<any[]>([])
  
  const addMenuItem = () => {
    const newItem = {
      id: crypto.randomUUID(),
      title: 'عنصر جديد',
      icon: {
        type: 'lucide',
        name: 'Circle',
        color: '#1E40AF',
        size: 'md',
        showIcon: true
      },
      link: {
        type: 'page',
        url: '/',
        openInNewTab: false
      },
      displayOrder: items.length + 1
    }
    setItems([...items, newItem])
  }
  
  return (
    <div className="container mx-auto py-8">
      <div className="flex justify-between items-center mb-8">
        <h1 className="text-3xl font-bold">تحرير القائمة</h1>
        <Button onClick={addMenuItem}>
          <Plus className="h-4 w-4 mr-2" />
          إضافة عنصر
        </Button>
      </div>
      
      <DndContext collisionDetection={closestCenter}>
        <SortableContext items={items.map(i => i.id)} strategy={verticalListSortingStrategy}>
          <div className="space-y-4">
            {items.map((item, index) => (
              <Card key={item.id} className="p-4">
                <div className="grid grid-cols-12 gap-4 items-center">
                  <div className="col-span-1 flex justify-center">
                    <GripVertical className="h-5 w-5 text-gray-400 cursor-grab" />
                  </div>
                  
                  <div className="col-span-3">
                    <Label>الأيقونة</Label>
                    <IconPicker
                      value={item.icon}
                      onChange={(icon) => {
                        const newItems = [...items]
                        newItems[index].icon = icon
                        setItems(newItems)
                      }}
                    />
                  </div>
                  
                  <div className="col-span-3">
                    <Label>العنوان</Label>
                    <Input value={item.title} />
                  </div>
                  
                  <div className="col-span-4">
                    <Label>الرابط</Label>
                    <Input value={item.link.url} />
                  </div>
                  
                  <div className="col-span-1 flex gap-2 justify-end">
                    <Button variant="ghost" size="icon">
                      <Edit className="h-4 w-4" />
                    </Button>
                    <Button variant="ghost" size="icon">
                      <Trash2 className="h-4 w-4 text-red-500" />
                    </Button>
                  </div>
                </div>
              </Card>
            ))}
          </div>
        </SortableContext>
      </DndContext>
    </div>
  )
}
```

---

### Zustand Stores

#### Auth Store (`lib/stores/authStore.ts`)
```tsx
import { create } from 'zustand'
import { persist } from 'zustand/middleware'

interface AuthState {
  user: User | null
  accessToken: string | null
  login: (email: string, password: string) => Promise<void>
  logout: () => void
  isAuthenticated: () => boolean
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      user: null,
      accessToken: null,
      
      login: async (email, password) => {
        const response = await loginAPI(email, password)
        set({ user: response.user, accessToken: response.accessToken })
      },
      
      logout: () => {
        set({ user: null, accessToken: null })
      },
      
      isAuthenticated: () => !!get().accessToken
    }),
    { name: 'auth-storage' }
  )
)
```

#### Page Builder Store (`lib/stores/pageBuilderStore.ts`)
```tsx
interface PageBuilderState {
  blocks: Block[]
  selectedBlockId: string | null
  language: 'ar' | 'en'
  addBlock: (blockType: string) => void
  removeBlock: (id: string) => void
  updateBlock: (id: string, props: any) => void
  reorderBlocks: (oldIndex: number, newIndex: number) => void
  selectBlock: (id: string | null) => void
  setLanguage: (lang: 'ar' | 'en') => void
}

export const usePageBuilderStore = create<PageBuilderState>((set) => ({
  blocks: [],
  selectedBlockId: null,
  language: 'ar',
  
  addBlock: (blockType) => set((state) => ({
    blocks: [...state.blocks, {
      id: crypto.randomUUID(),
      blockType,
      props: BlockDefaultProps[blockType]
    }]
  })),
  
  removeBlock: (id) => set((state) => ({
    blocks: state.blocks.filter(b => b.id !== id),
    selectedBlockId: state.selectedBlockId === id ? null : state.selectedBlockId
  })),
  
  updateBlock: (id, props) => set((state) => ({
    blocks: state.blocks.map(b => b.id === id ? { ...b, props: { ...b.props, ...props } } : b)
  })),
  
  reorderBlocks: (oldIndex, newIndex) => set((state) => {
    const newBlocks = [...state.blocks]
    const [removed] = newBlocks.splice(oldIndex, 1)
    newBlocks.splice(newIndex, 0, removed)
    return { blocks: newBlocks }
  }),
  
  selectBlock: (id) => set({ selectedBlockId: id }),
  
  setLanguage: (lang) => set({ language: lang })
}))
```

### React Query (TanStack Query)

```tsx
// lib/hooks/useContent.ts
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { getContent, createContent, updateContent } from '@/lib/api/endpoints/content'

export function useContent(contentType: string, filters?: any) {
  return useQuery({
    queryKey: ['content', contentType, filters],
    queryFn: () => getContent(contentType, filters)
  })
}

export function useCreateContent() {
  const queryClient = useQueryClient()
  
  return useMutation({
    mutationFn: createContent,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['content'] })
    }
  })
}
```

---

## 🔌 API Integration

### API Client (`lib/api/client.ts`)
```tsx
import axios from 'axios'
import { useAuthStore } from '@/lib/stores/authStore'

const apiClient = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Request Interceptor: Add JWT token
apiClient.interceptors.request.use((config) => {
  const token = useAuthStore.getState().accessToken
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// Response Interceptor: Handle errors
apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401) {
      useAuthStore.getState().logout()
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

export default apiClient
```

### API Endpoints (`lib/api/endpoints/content.ts`)
```tsx
import apiClient from '../client'
import { ContentResponse, CreateContentRequest } from '@/types/api'

export async function getContent(
  contentType: string, 
  filters?: { lang?: string, page?: number }
): Promise<ContentResponse> {
  const { data } = await apiClient.get(`/content/${contentType}`, { params: filters })
  return data
}

export async function getContentBySlug(
  contentType: string, 
  slug: string, 
  lang: string
): Promise<ContentResponse> {
  const { data } = await apiClient.get(`/content/${contentType}/${slug}`, { params: { lang } })
  return data
}

export async function createContent(
  contentType: string, 
  request: CreateContentRequest
): Promise<ContentResponse> {
  const { data } = await apiClient.post(`/content/${contentType}`, request)
  return data
}

export async function updateContent(
  contentType: string, 
  id: string, 
  request: Partial<CreateContentRequest>
): Promise<ContentResponse> {
  const { data } = await apiClient.put(`/content/${contentType}/${id}`, request)
  return data
}

export async function deleteContent(contentType: string, id: string): Promise<void> {
  await apiClient.delete(`/content/${contentType}/${id}`)
}
```

---

## 🌍 Internationalization (i18n)

### Middleware for Language Detection (`middleware.ts`)
```tsx
import { NextRequest, NextResponse } from 'next/server'

const locales = ['ar', 'en']
const defaultLocale = 'ar'

export function middleware(request: NextRequest) {
  const pathname = request.nextUrl.pathname
  
  // Check if pathname already has locale
  const pathnameHasLocale = locales.some(
    (locale) => pathname.startsWith(`/${locale}/`) || pathname === `/${locale}`
  )
  
  if (pathnameHasLocale) return
  
  // Redirect to default locale
  const locale = defaultLocale
  request.nextUrl.pathname = `/${locale}${pathname}`
  return NextResponse.redirect(request.nextUrl)
}

export const config = {
  matcher: ['/((?!api|_next/static|_next/image|favicon.ico|images).*)']
}
```

### Translation Dictionaries
```json
// dictionaries/ar.json
{
  "nav": {
    "home": "الرئيسية",
    "about": "عن غاهر",
    "news": "الأخبار",
    "facilities": "المنشآت المعتمدة",
    "certificates": "التحقق من الشهادات",
    "contact": "اتصل بنا"
  },
  "common": {
    "readMore": "اقرأ المزيد",
    "submit": "إرسال",
    "cancel": "إلغاء",
    "save": "حفظ",
    "delete": "حذف",
    "edit": "تعديل",
    "search": "بحث",
    "loading": "جاري التحميل..."
  },
  "certificates": {
    "validate": "التحقق",
    "enterCertificateNumber": "أدخل رقم الشهادة",
    "validCertificate": "الشهادة صالحة",
    "invalidCertificate": "الشهادة غير صالحة"
  }
}
```

```json
// dictionaries/en.json
{
  "nav": {
    "home": "Home",
    "about": "About GAHAR",
    "news": "News",
    "facilities": "Certified Facilities",
    "certificates": "Validate Certificates",
    "contact": "Contact Us"
  }
  // ... etc
}
```

### Dictionary Hook
```tsx
// lib/hooks/useLanguage.ts
import { useParams } from 'next/navigation'
import ar from '@/dictionaries/ar.json'
import en from '@/dictionaries/en.json'

const dictionaries = { ar, en }

export function useLanguage() {
  const params = useParams()
  const lang = (params.lang as 'ar' | 'en') || 'ar'
  
  const t = (key: string) => {
    const keys = key.split('.')
    let value: any = dictionaries[lang]
    
    for (const k of keys) {
      value = value?.[k]
    }
    
    return value || key
  }
  
  return { lang, t, isRTL: lang === 'ar' }
}
```

### Usage in Components
```tsx
'use client'
import { useLanguage } from '@/lib/hooks/useLanguage'

export default function Header() {
  const { t, isRTL } = useLanguage()
  
  return (
    <header dir={isRTL ? 'rtl' : 'ltr'}>
      <nav>
        <Link href="/news">{t('nav.news')}</Link>
        <Link href="/facilities">{t('nav.facilities')}</Link>
      </nav>
    </header>
  )
}
```

---

## 🎨 Admin Panel - Page Builder (Detailed Implementation)

### Page Builder Main Component
```tsx
// app/admin/pages/[id]/builder/page.tsx
'use client'
import { useState, useEffect } from 'react'
import { DndContext, closestCenter, PointerSensor, useSensor, useSensors } from '@dnd-kit/core'
import { SortableContext, verticalListSortingStrategy } from '@dnd-kit/sortable'
import { usePageBuilderStore } from '@/lib/stores/pageBuilderStore'
import { getPage, updatePage } from '@/lib/api/endpoints/pages'
import BlocksPanel from '@/components/admin/PageBuilder/BlocksPanel'
import Canvas from '@/components/admin/PageBuilder/Canvas'
import PropertiesPanel from '@/components/admin/PageBuilder/PropertiesPanel'
import { Button } from '@/components/ui/button'
import { Tabs, TabsList, TabsTrigger } from '@/components/ui/tabs'

export default function PageBuilderPage({ params }: { params: { id: string } }) {
  const { blocks, language, setLanguage, reorderBlocks } = usePageBuilderStore()
  const [isSaving, setIsSaving] = useState(false)
  
  useEffect(() => {
    // Load page data
    getPage(params.id, language).then((page) => {
      usePageBuilderStore.setState({ blocks: page.structure })
    })
  }, [params.id, language])
  
  const sensors = useSensors(useSensor(PointerSensor))
  
  const handleDragEnd = (event: any) => {
    const { active, over } = event
    if (!over || active.id === over.id) return
    
    const oldIndex = blocks.findIndex(b => b.id === active.id)
    const newIndex = blocks.findIndex(b => b.id === over.id)
    reorderBlocks(oldIndex, newIndex)
  }
  
  const handleSave = async () => {
    setIsSaving(true)
    await updatePage(params.id, { structure: blocks, language })
    setIsSaving(false)
  }
  
  return (
    <div className="h-screen flex flex-col">
      {/* Top Bar */}
      <header className="border-b p-4 flex items-center justify-between">
        <h1 className="text-2xl font-bold">Page Builder</h1>
        
        <div className="flex items-center gap-4">
          <Tabs value={language} onValueChange={(v) => setLanguage(v as 'ar' | 'en')}>
            <TabsList>
              <TabsTrigger value="ar">عربي</TabsTrigger>
              <TabsTrigger value="en">English</TabsTrigger>
            </TabsList>
          </Tabs>
          
          <Button onClick={handleSave} disabled={isSaving}>
            {isSaving ? 'Saving...' : 'Save'}
          </Button>
          <Button variant="outline">Preview</Button>
          <Button>Publish</Button>
        </div>
      </header>
      
      {/* Main Layout */}
      <div className="flex-1 flex overflow-hidden">
        {/* Left Panel: Available Blocks */}
        <BlocksPanel />
        
        {/* Center: Canvas */}
        <main className="flex-1 overflow-auto bg-gray-50 p-8">
          <DndContext 
            sensors={sensors} 
            collisionDetection={closestCenter} 
            onDragEnd={handleDragEnd}
          >
            <SortableContext items={blocks.map(b => b.id)} strategy={verticalListSortingStrategy}>
              <Canvas />
            </SortableContext>
          </DndContext>
        </main>
        
        {/* Right Panel: Properties */}
        <PropertiesPanel />
      </div>
    </div>
  )
}
```

### Blocks Panel Component
```tsx
// components/admin/PageBuilder/BlocksPanel.tsx
'use client'
import { usePageBuilderStore } from '@/lib/stores/pageBuilderStore'
import { Button } from '@/components/ui/button'
import { BlockRegistry } from '@/components/blocks/BlockRegistry'

export default function BlocksPanel() {
  const addBlock = usePageBuilderStore((state) => state.addBlock)
  
  const blockTypes = Object.keys(BlockRegistry)
  
  return (
    <aside className="w-64 border-r p-4 overflow-auto">
      <h2 className="text-lg font-semibold mb-4">Add Blocks</h2>
      
      <div className="space-y-2">
        {blockTypes.map((blockType) => (
          <Button
            key={blockType}
            variant="outline"
            className="w-full justify-start"
            onClick={() => addBlock(blockType)}
          >
            + {blockType}
          </Button>
        ))}
      </div>
    </aside>
  )
}
```

### Canvas Component
```tsx
// components/admin/PageBuilder/Canvas.tsx
'use client'
import { usePageBuilderStore } from '@/lib/stores/pageBuilderStore'
import SortableBlock from './SortableBlock'

export default function Canvas() {
  const blocks = usePageBuilderStore((state) => state.blocks)
  
  if (blocks.length === 0) {
    return (
      <div className="h-full flex items-center justify-center text-gray-400">
        <p>Drag blocks from the left panel to start building your page</p>
      </div>
    )
  }
  
  return (
    <div className="space-y-4">
      {blocks.map((block) => (
        <SortableBlock key={block.id} block={block} />
      ))}
    </div>
  )
}
```

### Sortable Block Wrapper
```tsx
// components/admin/PageBuilder/SortableBlock.tsx
'use client'
import { useSortable } from '@dnd-kit/sortable'
import { CSS } from '@dnd-kit/utilities'
import { usePageBuilderStore } from '@/lib/stores/pageBuilderStore'
import { BlockRegistry } from '@/components/blocks/BlockRegistry'
import { Button } from '@/components/ui/button'
import { GripVertical, Trash2 } from 'lucide-react'

export default function SortableBlock({ block }: { block: any }) {
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({ id: block.id })
  const { selectBlock, removeBlock, selectedBlockId } = usePageBuilderStore()
  
  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1
  }
  
  const BlockComponent = BlockRegistry[block.blockType]
  const isSelected = selectedBlockId === block.id
  
  return (
    <div
      ref={setNodeRef}
      style={style}
      className={`relative border-2 rounded-lg overflow-hidden ${isSelected ? 'border-blue-500' : 'border-gray-200'}`}
      onClick={() => selectBlock(block.id)}
    >
      {/* Block Controls Overlay */}
      <div className="absolute top-2 right-2 z-10 flex gap-2">
        <Button
          size="sm"
          variant="secondary"
          {...attributes}
          {...listeners}
          className="cursor-grab active:cursor-grabbing"
        >
          <GripVertical className="h-4 w-4" />
        </Button>
        <Button
          size="sm"
          variant="destructive"
          onClick={(e) => {
            e.stopPropagation()
            removeBlock(block.id)
          }}
        >
          <Trash2 className="h-4 w-4" />
        </Button>
      </div>
      
      {/* Block Preview */}
      <div className="pointer-events-none">
        <BlockComponent {...block.props} editorMode />
      </div>
    </div>
  )
}
```

### Properties Panel Component
```tsx
// components/admin/PageBuilder/PropertiesPanel.tsx
'use client'
import { usePageBuilderStore } from '@/lib/stores/pageBuilderStore'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'

export default function PropertiesPanel() {
  const { blocks, selectedBlockId, updateBlock } = usePageBuilderStore()
  const selectedBlock = blocks.find(b => b.id === selectedBlockId)
  
  if (!selectedBlock) {
    return (
      <aside className="w-80 border-l p-4">
        <p className="text-gray-400">Select a block to edit its properties</p>
      </aside>
    )
  }
  
  const handlePropChange = (key: string, value: any) => {
    updateBlock(selectedBlock.id, { [key]: value })
  }
  
  return (
    <aside className="w-80 border-l p-4 overflow-auto">
      <h2 className="text-lg font-semibold mb-4">Properties</h2>
      <p className="text-sm text-gray-500 mb-4">{selectedBlock.blockType}</p>
      
      <div className="space-y-4">
        {Object.entries(selectedBlock.props).map(([key, value]) => (
          <div key={key}>
            <Label htmlFor={key} className="capitalize">{key}</Label>
            {typeof value === 'string' && value.length > 50 ? (
              <Textarea
                id={key}
                value={value as string}
                onChange={(e) => handlePropChange(key, e.target.value)}
              />
            ) : (
              <Input
                id={key}
                value={value as string}
                onChange={(e) => handlePropChange(key, e.target.value)}
              />
            )}
          </div>
        ))}
      </div>
    </aside>
  )
}
```

---

## ⚡ Performance Optimization

### Image Optimization
```tsx
import Image from 'next/image'

<Image
  src="/images/hero.jpg"
  alt="Hero"
  width={1920}
  height={1080}
  priority // for above-the-fold images
  placeholder="blur"
  blurDataURL="data:image/jpeg;base64,..."
/>
```

### Code Splitting & Lazy Loading
```tsx
import dynamic from 'next/dynamic'

const FacilitiesMap = dynamic(
  () => import('@/components/maps/FacilitiesMap'),
  { 
    ssr: false,
    loading: () => <LoadingSpinner />
  }
)
```

### React Query Caching
```tsx
// lib/hooks/useContent.ts
export function useContent(contentType: string) {
  return useQuery({
    queryKey: ['content', contentType],
    queryFn: () => getContent(contentType),
    staleTime: 5 * 60 * 1000, // 5 minutes
    cacheTime: 10 * 60 * 1000 // 10 minutes
  })
}
```

### Bundle Analysis
```bash
# Install bundle analyzer
npm install @next/bundle-analyzer

# Add to next.config.mjs
const withBundleAnalyzer = require('@next/bundle-analyzer')({
  enabled: process.env.ANALYZE === 'true'
})

# Run analysis
ANALYZE=true npm run build
```

---

## 🚀 Deployment

### Environment Variables
```env
# .env.production
NEXT_PUBLIC_API_URL=https://api.gahar.sa
NEXT_PUBLIC_SITE_URL=https://gahar.sa
NEXTAUTH_URL=https://gahar.sa
NEXTAUTH_SECRET=***
```

### Build & Deploy (Vercel)
```bash
# Build for production
npm run build

# Start production server
npm run start
```

### Dockerfile (Alternative: Docker deployment)
```dockerfile
FROM node:20-alpine AS base

FROM base AS deps
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm ci

FROM base AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY . .
RUN npm run build

FROM base AS runner
WORKDIR /app
ENV NODE_ENV production
COPY --from=builder /app/public ./public
COPY --from=builder /app/.next/standalone ./
COPY --from=builder /app/.next/static ./.next/static

EXPOSE 3000
ENV PORT 3000

CMD ["node", "server.js"]
```

---

## ✅ Frontend Deliverables Checklist

- [ ] Next.js 15 project initialized with TypeScript
- [ ] Tailwind CSS + Shadcn/ui configured
- [ ] All public pages implemented (Home, News, Facilities, etc.)
- [ ] Admin panel with authentication
- [ ] Page Builder (drag & drop) fully functional
- [ ] Content Editor with multilingual support
- [ ] Form Builder
- [ ] API integration with React Query
- [ ] Zustand stores for state management
- [ ] i18n with Arabic/English dictionaries
- [ ] Responsive design (mobile-first)
- [ ] Accessibility (WCAG 2.1 AA)
- [ ] SEO optimization (metadata, structured data, sitemaps)
- [ ] Performance optimization (images, lazy loading)
- [ ] Error boundaries & loading states
- [ ] Unit tests for critical components
- [ ] E2E tests (Playwright/Cypress)
- [ ] Production build optimized
- [ ] Deployment pipeline configured

---

**Document Version:** 1.0  
**Last Updated:** November 9, 2025  
**Maintained By:** Frontend Development Team
