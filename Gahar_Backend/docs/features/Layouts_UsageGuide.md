# 📐 Layouts System - Usage Guide

## 🎯 Overview

The Layouts System provides a flexible way to manage different display templates for your content. Each layout can have custom configurations stored as JSON, making it highly adaptable to various content types and design requirements.

---

## 🚀 Quick Start

### 1. Create Your First Layout

```http
POST /api/layouts
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "name": "Standard Article",
  "description": "Default layout for articles",
  "configuration": {
    "showAuthor": true,
    "showDate": true,
    "showTags": true,
    "imageSize": "medium"
  },
  "isActive": true
}
```

### 2. Set it as Default

```http
POST /api/layouts/{id}/set-default
Authorization: Bearer {your-token}
```

### 3. Use in Content

When creating content, simply reference the layout:

```http
POST /api/contents
{
  "title": "My Article",
  "contentTypeId": 1,
  "layoutId": {layout-id},
  // ... other fields
}
```

---

## 📋 Common Use Cases

### Use Case 1: Blog Article Layout

Perfect for blog posts with author info and social sharing.

```json
POST /api/layouts
{
  "name": "Blog Post",
  "description": "Layout for blog articles",
  "configuration": {
    "template": "blog-post",
  "showAuthor": true,
    "showPublishDate": true,
    "showReadTime": true,
  "showTags": true,
    "showShare": true,
    "showRelated": true,
    "imageSize": "large",
    "sidebar": "right",
    "enableComments": true,
    "widgets": ["author-bio", "recent-posts", "newsletter"]
  },
  "isActive": true,
  "previewImage": "/images/layouts/blog-post.jpg"
}
```

### Use Case 2: News List Layout

Compact layout for news listings and archives.

```json
POST /api/layouts
{
  "name": "News List",
  "description": "Compact layout for news listings",
  "configuration": {
    "template": "news-list",
    "itemsPerPage": 20,
    "showExcerpt": true,
    "excerptLength": 150,
    "thumbnailSize": "small",
    "showAuthor": false,
    "showDate": true,
    "showCategory": true,
    "gridColumns": 3,
    "enablePagination": true
  },
  "isActive": true,
  "previewImage": "/images/layouts/news-list.jpg"
}
```

### Use Case 3: Landing Page Layout

Full-width layout without sidebars for landing pages.

```json
POST /api/layouts
{
  "name": "Landing Page",
  "description": "Full-width layout for landing pages",
  "configuration": {
    "template": "landing",
    "fullWidth": true,
    "showHeader": false,
    "showFooter": false,
    "sidebar": false,
    "sections": [
      {
        "type": "hero",
    "fullScreen": true
   },
      {
        "type": "features",
"columns": 3
      },
   {
        "type": "cta",
      "centered": true
      }
    ]
  },
  "isActive": true,
  "previewImage": "/images/layouts/landing.jpg"
}
```

### Use Case 4: Photo Gallery Layout

Image-focused layout for photo galleries.

```json
POST /api/layouts
{
  "name": "Photo Gallery",
  "description": "Grid layout for photo galleries",
  "configuration": {
    "template": "gallery",
    "gridType": "masonry",
    "columns": 4,
    "imageSize": "large",
    "showCaptions": true,
    "enableLightbox": true,
    "showExif": false,
    "loadingStyle": "lazy"
  },
  "isActive": true,
  "previewImage": "/images/layouts/gallery.jpg"
}
```

### Use Case 5: Video Content Layout

Optimized for video content.

```json
POST /api/layouts
{
  "name": "Video Player",
  "description": "Layout optimized for video content",
  "configuration": {
    "template": "video",
    "playerSize": "16:9",
 "autoplay": false,
    "showPlaylist": true,
 "showDescription": true,
    "showTranscript": true,
    "sidebar": "right",
    "relatedVideos": true
  },
  "isActive": true,
  "previewImage": "/images/layouts/video.jpg"
}
```

---

## 🔧 Configuration Reference

### Core Properties

| Property | Type | Description | Example |
|----------|------|-------------|---------|
| `template` | string | Template identifier | "blog-post", "news-list" |
| `showAuthor` | boolean | Display author information | true/false |
| `showDate` | boolean | Display publish date | true/false |
| `showTags` | boolean | Display content tags | true/false |
| `imageSize` | string | Image display size | "small", "medium", "large" |
| `sidebar` | string/boolean | Sidebar position | "left", "right", false |
| `fullWidth` | boolean | Full-width layout | true/false |
| `enableComments` | boolean | Enable comments section | true/false |

### Advanced Properties

#### Grid Layouts
```json
{
  "gridType": "masonry", // or "fixed", "flex"
  "columns": 3,        // Number of columns
  "gap": "20px",            // Gap between items
  "responsive": {
    "mobile": 1,
    "tablet": 2,
    "desktop": 3
  }
}
```

#### Content Display
```json
{
  "showExcerpt": true,
  "excerptLength": 150,
  "showReadTime": true,
  "showShare": true,
  "showPrint": false,
  "showRelated": true,
  "relatedCount": 5
}
```

#### SEO & Meta
```json
{
  "showBreadcrumbs": true,
  "structuredData": true,
  "openGraph": true,
  "twitterCard": true
}
```

#### Widgets & Components
```json
{
  "widgets": [
 "author-bio",
    "recent-posts",
    "popular-posts",
    "newsletter",
    "social-links"
  ]
}
```

---

## 📊 Managing Layouts

### Get All Layouts

```http
GET /api/layouts
Authorization: Bearer {your-token}
```

Response:
```json
[
  {
    "id": 1,
    "name": "Standard Article",
    "description": "Default layout for articles",
    "configuration": { /* ... */ },
    "isDefault": true,
    "isActive": true,
    "previewImage": "/images/layouts/standard.jpg",
    "createdAt": "2024-01-15T10:00:00Z",
    "updatedAt": "2024-01-20T15:30:00Z"
  }
]
```

### Get Active Layouts Only

```http
GET /api/layouts?activeOnly=true
Authorization: Bearer {your-token}
```

### Get Layout by ID

```http
GET /api/layouts/1
Authorization: Bearer {your-token}
```

### Get Layout with Statistics

```http
GET /api/layouts/1/stats
Authorization: Bearer {your-token}
```

Response:
```json
{
  "id": 1,
  "name": "Standard Article",
  "description": "Default layout for articles",
  "configuration": { /* ... */ },
  "isDefault": true,
  "isActive": true,
  "contentCount": 45,
  "createdAt": "2024-01-15T10:00:00Z",
  "updatedAt": "2024-01-20T15:30:00Z"
}
```

### Get Default Layout (Public)

```http
GET /api/layouts/default
```

No authentication required. Returns the current default layout.

### Update Layout

```http
PUT /api/layouts/1
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "name": "Updated Standard Article",
  "description": "Updated description",
  "configuration": {
    "showAuthor": true,
    "showDate": false,
    "imageSize": "large"
  },
  "isActive": true,
  "previewImage": "/images/layouts/updated.jpg"
}
```

### Delete Layout

```http
DELETE /api/layouts/1
Authorization: Bearer {your-token}
```

**Important:** You cannot delete:
- The default layout (set another as default first)
- Layouts that are in use by content

---

## 🎨 Frontend Integration

### Example: React Component

```jsx
import { useState, useEffect } from 'react';

function ContentDisplay({ contentId }) {
  const [content, setContent] = useState(null);
  const [layout, setLayout] = useState(null);

  useEffect(() => {
    // Fetch content
    fetch(`/api/contents/${contentId}`)
      .then(res => res.json())
      .then(data => {
        setContent(data);
        
  // Fetch associated layout
        if (data.layoutId) {
          fetch(`/api/layouts/${data.layoutId}`)
            .then(res => res.json())
            .then(setLayout);
        }
      });
  }, [contentId]);

  if (!content || !layout) return <div>Loading...</div>;

  // Apply layout configuration
  const config = layout.configuration;

  return (
    <article className={`layout-${config.template}`}>
      <header>
        <h1>{content.title}</h1>
    {config.showAuthor && <AuthorBio author={content.author} />}
      {config.showDate && <PublishDate date={content.publishedAt} />}
      </header>

      {content.featuredImage && (
        <img 
 src={content.featuredImage} 
        alt={content.title}
    className={`image-${config.imageSize}`}
        />
  )}

      <div className="content-body">
        {content.body}
      </div>

    {config.showTags && <TagList tags={content.tags} />}
      
      {config.enableComments && <CommentSection contentId={content.id} />}
      
      {config.sidebar && (
        <aside className={`sidebar-${config.sidebar}`}>
          <Widgets widgets={config.widgets} />
        </aside>
      )}
    </article>
  );
}
```

### Example: Vue Component

```vue
<template>
  <article :class="`layout-${layoutConfig.template}`">
    <header>
      <h1>{{ content.title }}</h1>
      <AuthorBio v-if="layoutConfig.showAuthor" :author="content.author" />
 <PublishDate v-if="layoutConfig.showDate" :date="content.publishedAt" />
    </header>

    <img 
      v-if="content.featuredImage"
      :src="content.featuredImage" 
      :alt="content.title"
      :class="`image-${layoutConfig.imageSize}`"
    />

    <div class="content-body" v-html="content.body"></div>

    <TagList v-if="layoutConfig.showTags" :tags="content.tags" />
    
    <CommentSection 
 v-if="layoutConfig.enableComments" 
      :content-id="content.id" 
    />
    
    <aside v-if="layoutConfig.sidebar" :class="`sidebar-${layoutConfig.sidebar}`">
      <Widgets :widgets="layoutConfig.widgets" />
    </aside>
  </article>
</template>

<script setup>
import { ref, onMounted } from 'vue';

const props = defineProps(['contentId']);
const content = ref(null);
const layoutConfig = ref({});

onMounted(async () => {
  // Fetch content
  const contentRes = await fetch(`/api/contents/${props.contentId}`);
  content.value = await contentRes.json();
  
  // Fetch layout
  if (content.value.layoutId) {
    const layoutRes = await fetch(`/api/layouts/${content.value.layoutId}`);
    const layout = await layoutRes.json();
    layoutConfig.value = layout.configuration;
  }
});
</script>
```

---

## 🔒 Permissions Required

| Operation | Permission | Description |
|-----------|------------|-------------|
| View layouts | `Layouts.View` | Read layout information |
| Create layout | `Layouts.Create` | Create new layouts |
| Update layout | `Layouts.Edit` | Modify existing layouts |
| Delete layout | `Layouts.Delete` | Remove layouts |
| Set default | `Layouts.Edit` | Change default layout |

---

## ⚠️ Important Notes

### Business Rules

1. **Unique Names**: Each layout must have a unique name
2. **Single Default**: Only one layout can be default at a time
3. **Active Default**: Only active layouts can be set as default
4. **No Delete Default**: Cannot delete the default layout
5. **No Delete In-Use**: Cannot delete layouts assigned to content

### Best Practices

1. **Start Simple**: Begin with basic configurations and expand as needed
2. **Document Configurations**: Keep track of what each configuration property does
3. **Test Thoroughly**: Test layouts with different content types
4. **Use Preview Images**: Add preview images to help content editors choose layouts
5. **Version Control**: Consider keeping backup copies of layout configurations
6. **Responsive Design**: Include responsive breakpoints in configuration

### Performance Tips

1. **Cache Layouts**: Cache layout configurations on the frontend
2. **Lazy Load**: Load layout-specific assets only when needed
3. **Minimize Config**: Keep configuration objects as small as possible
4. **Use Defaults**: Define sensible defaults to reduce configuration size

---

## 🐛 Troubleshooting

### Problem: Cannot set layout as default
**Solution**: Ensure the layout is active (`isActive: true`)

### Problem: Cannot delete layout
**Solution**: Check if it's the default layout or if content is using it

### Problem: Configuration not working
**Solution**: Validate JSON structure, ensure all required properties are present

### Problem: Layout not appearing in dropdown
**Solution**: Check if layout is active, verify user has `Layouts.View` permission

---

## 📞 Support

For issues or questions:
1. Check this documentation
2. Review API responses for error messages
3. Check application logs
4. Contact development team

---

## 🔄 Version History

- **v1.0** - Initial release with basic CRUD operations
- **v1.1** - Added default layout management
- **v1.2** - Added layout statistics
- **Current** - Full featured layout system with JSON configurations

---

**Last Updated:** $(Get-Date -Format "yyyy-MM-dd")  
**Maintained by:** Developer 1
