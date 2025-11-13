using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Gahar_Backend.Constants;
using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Page;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Implementations;

public class PageService : IPageService
{
  private readonly IPageRepository _pageRepository;
    private readonly IPageBlockRepository _pageBlockRepository;

    public PageService(IPageRepository pageRepository, IPageBlockRepository pageBlockRepository)
    {
        _pageRepository = pageRepository;
   _pageBlockRepository = pageBlockRepository;
    }

    public async Task<PagedResult<PageListDto>> GetAllAsync(PageFilterDto filter)
    {
var query = _pageRepository.GetQueryable()
            .Include(p => p.Author)
            .Include(p => p.Blocks)
  .AsQueryable();

    if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
 query = query.Where(p => 
  p.Title.Contains(filter.SearchTerm) || 
        p.Description!.Contains(filter.SearchTerm));
   }

        if (filter.IsPublished.HasValue)
        {
        query = query.Where(p => p.IsPublished == filter.IsPublished.Value);
        }

   if (filter.AuthorId.HasValue)
        {
            query = query.Where(p => p.AuthorId == filter.AuthorId.Value);
        }

   if (!string.IsNullOrWhiteSpace(filter.Template))
        {
          query = query.Where(p => p.Template == filter.Template);
 }

  var totalCount = await query.CountAsync();

        query = filter.SortBy?.ToLower() switch
        {
  "title" => filter.SortOrder?.ToLower() == "asc"
      ? query.OrderBy(p => p.Title)
        : query.OrderByDescending(p => p.Title),
  "publishedat" => filter.SortOrder?.ToLower() == "asc"
      ? query.OrderBy(p => p.PublishedAt)
      : query.OrderByDescending(p => p.PublishedAt),
        _ => filter.SortOrder?.ToLower() == "asc"
          ? query.OrderBy(p => p.CreatedAt)
            : query.OrderByDescending(p => p.CreatedAt)
   };

        var items = await query
   .Skip((filter.PageNumber - 1) * filter.PageSize)
       .Take(filter.PageSize)
        .Select(p => new PageListDto
            {
                Id = p.Id,
           Title = p.Title,
   Slug = p.Slug,
       Description = p.Description,
            IsPublished = p.IsPublished,
                PublishedAt = p.PublishedAt,
           AuthorName = p.Author != null ? $"{p.Author.FirstName} {p.Author.LastName}".Trim() : null,
  BlockCount = p.Blocks.Count,
                CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt
    })
            .ToListAsync();

        return new PagedResult<PageListDto>
     {
            Items = items,
        TotalCount = totalCount,
          PageNumber = filter.PageNumber,
   PageSize = filter.PageSize
        };
    }

    public async Task<PageDetailDto> GetByIdAsync(int id, string? lang = "ar")
    {
        var page = await _pageRepository.GetQueryable()
      .Include(p => p.Author)
            .Include(p => p.Blocks.OrderBy(b => b.DisplayOrder))
         .FirstOrDefaultAsync(p => p.Id == id);

      if (page == null)
        {
   throw new NotFoundException($"Page with ID {id} not found");
     }

        return MapToDetailDto(page);
    }

    public async Task<PageDetailDto> GetBySlugAsync(string slug, string? lang = "ar")
    {
        var page = await _pageRepository.GetBySlugAsync(slug, lang);

        if (page == null)
      {
 throw new NotFoundException($"Page with slug '{slug}' not found");
  }

        if (!page.IsPublished)
        {
            throw new BadRequestException("Page is not published");
        }

        return MapToDetailDto(page);
    }

    public async Task<PageDetailDto> CreateAsync(CreatePageDto dto, int authorId)
    {
        if (await _pageRepository.SlugExistsAsync(dto.Slug))
   {
            throw new BadRequestException($"Slug '{dto.Slug}' already exists");
      }

        var page = new Page
        {
            Title = dto.Title,
      Slug = dto.Slug,
            Description = dto.Description,
   MetaTitle = dto.MetaTitle,
   MetaDescription = dto.MetaDescription,
            MetaKeywords = dto.MetaKeywords,
   OgTitle = dto.OgTitle,
            OgDescription = dto.OgDescription,
   OgImage = dto.OgImage,
    Template = dto.Template,
         ShowTitle = dto.ShowTitle,
            ShowBreadcrumbs = dto.ShowBreadcrumbs,
   AuthorId = authorId,
      IsPublished = false
 };

        await _pageRepository.AddAsync(page);
        await _pageRepository.SaveChangesAsync();

        return await GetByIdAsync(page.Id);
    }

    public async Task<PageDetailDto> UpdateAsync(int id, UpdatePageDto dto)
  {
        var page = await _pageRepository.GetByIdAsync(id);
   if (page == null)
        {
   throw new NotFoundException($"Page with ID {id} not found");
      }

        if (dto.Slug != page.Slug && await _pageRepository.SlugExistsAsync(dto.Slug, id))
        {
     throw new BadRequestException($"Slug '{dto.Slug}' already exists");
        }

     page.Title = dto.Title;
        page.Slug = dto.Slug;
        page.Description = dto.Description;
        page.MetaTitle = dto.MetaTitle;
        page.MetaDescription = dto.MetaDescription;
        page.MetaKeywords = dto.MetaKeywords;
   page.OgTitle = dto.OgTitle;
      page.OgDescription = dto.OgDescription;
        page.OgImage = dto.OgImage;
        page.Template = dto.Template;
   page.ShowTitle = dto.ShowTitle;
        page.ShowBreadcrumbs = dto.ShowBreadcrumbs;
        page.IsPublished = dto.IsPublished;

        _pageRepository.Update(page);
        await _pageRepository.SaveChangesAsync();

    return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var page = await _pageRepository.GetByIdAsync(id);
if (page == null)
        {
          throw new NotFoundException($"Page with ID {id} not found");
  }

        _pageRepository.Delete(page);
        await _pageRepository.SaveChangesAsync();
    }

  public async Task PublishAsync(int id)
    {
        var page = await _pageRepository.GetByIdAsync(id);
        if (page == null)
        {
          throw new NotFoundException($"Page with ID {id} not found");
        }

        page.IsPublished = true;
        page.PublishedAt = DateTime.UtcNow;

     _pageRepository.Update(page);
 await _pageRepository.SaveChangesAsync();
    }

    public async Task UnpublishAsync(int id)
    {
     var page = await _pageRepository.GetByIdAsync(id);
        if (page == null)
        {
    throw new NotFoundException($"Page with ID {id} not found");
        }

        page.IsPublished = false;

        _pageRepository.Update(page);
        await _pageRepository.SaveChangesAsync();
    }

    public async Task<PageBlockDto> AddBlockAsync(int pageId, CreatePageBlockDto dto)
    {
   var page = await _pageRepository.GetByIdAsync(pageId);
        if (page == null)
        {
throw new NotFoundException($"Page with ID {pageId} not found");
        }

        if (!BlockTypes.IsValid(dto.BlockType))
        {
            throw new BadRequestException($"Invalid block type: {dto.BlockType}");
        }

     var maxOrder = await _pageBlockRepository.GetQueryable()
            .Where(b => b.PageId == pageId)
    .MaxAsync(b => (int?)b.DisplayOrder) ?? 0;

        var block = new PageBlock
 {
            PageId = pageId,
     BlockType = dto.BlockType,
       Configuration = JsonSerializer.Serialize(dto.Configuration),
       DisplayOrder = maxOrder + 1,
         IsVisible = dto.IsVisible,
        CssClass = dto.CssClass,
       CustomId = dto.CustomId
   };

        await _pageBlockRepository.AddAsync(block);
        await _pageBlockRepository.SaveChangesAsync();

        return new PageBlockDto
        {
        Id = block.Id,
  BlockType = block.BlockType,
 Configuration = JsonSerializer.Deserialize<object>(block.Configuration)!,
            DisplayOrder = block.DisplayOrder,
            IsVisible = block.IsVisible,
CssClass = block.CssClass,
            CustomId = block.CustomId
        };
    }

    public async Task<PageBlockDto> UpdateBlockAsync(int pageId, int blockId, UpdatePageBlockDto dto)
    {
        var block = await _pageBlockRepository.GetQueryable()
         .FirstOrDefaultAsync(b => b.Id == blockId && b.PageId == pageId);

 if (block == null)
        {
     throw new NotFoundException($"Block with ID {blockId} not found in page {pageId}");
 }

        if (!BlockTypes.IsValid(dto.BlockType))
        {
        throw new BadRequestException($"Invalid block type: {dto.BlockType}");
     }

    block.BlockType = dto.BlockType;
 block.Configuration = JsonSerializer.Serialize(dto.Configuration);
        block.DisplayOrder = dto.DisplayOrder;
 block.IsVisible = dto.IsVisible;
        block.CssClass = dto.CssClass;
     block.CustomId = dto.CustomId;

      _pageBlockRepository.Update(block);
   await _pageBlockRepository.SaveChangesAsync();

        return new PageBlockDto
        {
          Id = block.Id,
            BlockType = block.BlockType,
            Configuration = JsonSerializer.Deserialize<object>(block.Configuration)!,
            DisplayOrder = block.DisplayOrder,
            IsVisible = block.IsVisible,
    CssClass = block.CssClass,
            CustomId = block.CustomId
};
    }

    public async Task DeleteBlockAsync(int pageId, int blockId)
    {
        var block = await _pageBlockRepository.GetQueryable()
      .FirstOrDefaultAsync(b => b.Id == blockId && b.PageId == pageId);

        if (block == null)
        {
   throw new NotFoundException($"Block with ID {blockId} not found in page {pageId}");
        }

        _pageBlockRepository.Delete(block);
      await _pageBlockRepository.SaveChangesAsync();
    }

    public async Task ReorderBlocksAsync(int pageId, List<int> blockIds)
    {
        var page = await _pageRepository.GetByIdAsync(pageId);
        if (page == null)
        {
     throw new NotFoundException($"Page with ID {pageId} not found");
        }

        await _pageBlockRepository.ReorderBlocksAsync(pageId, blockIds);
    }

    public async Task<PageDetailDto> DuplicateAsync(int id, int authorId)
    {
  var originalPage = await _pageRepository.GetQueryable()
            .Include(p => p.Blocks)
  .FirstOrDefaultAsync(p => p.Id == id);

      if (originalPage == null)
        {
        throw new NotFoundException($"Page with ID {id} not found");
        }

        var baseSlug = $"{originalPage.Slug}-copy";
 var slug = baseSlug;
        var counter = 1;
        while (await _pageRepository.SlugExistsAsync(slug))
        {
   slug = $"{baseSlug}-{counter}";
            counter++;
}

     var newPage = new Page
      {
     Title = $"{originalPage.Title} (Copy)",
            Slug = slug,
            Description = originalPage.Description,
   MetaTitle = originalPage.MetaTitle,
            MetaDescription = originalPage.MetaDescription,
   MetaKeywords = originalPage.MetaKeywords,
     OgTitle = originalPage.OgTitle,
     OgDescription = originalPage.OgDescription,
   OgImage = originalPage.OgImage,
    Template = originalPage.Template,
            ShowTitle = originalPage.ShowTitle,
ShowBreadcrumbs = originalPage.ShowBreadcrumbs,
    AuthorId = authorId,
     IsPublished = false
        };

        foreach (var block in originalPage.Blocks.OrderBy(b => b.DisplayOrder))
      {
newPage.Blocks.Add(new PageBlock
     {
        BlockType = block.BlockType,
       Configuration = block.Configuration,
    DisplayOrder = block.DisplayOrder,
  IsVisible = block.IsVisible,
     CssClass = block.CssClass,
      CustomId = block.CustomId
            });
        }

        await _pageRepository.AddAsync(newPage);
        await _pageRepository.SaveChangesAsync();

        return await GetByIdAsync(newPage.Id);
    }

    private static PageDetailDto MapToDetailDto(Page page)
    {
        return new PageDetailDto
        {
      Id = page.Id,
            Title = page.Title,
       Slug = page.Slug,
      Description = page.Description,
   MetaTitle = page.MetaTitle,
 MetaDescription = page.MetaDescription,
            MetaKeywords = page.MetaKeywords,
            OgTitle = page.OgTitle,
            OgDescription = page.OgDescription,
     OgImage = page.OgImage,
    Template = page.Template,
    ShowTitle = page.ShowTitle,
    ShowBreadcrumbs = page.ShowBreadcrumbs,
            IsPublished = page.IsPublished,
            PublishedAt = page.PublishedAt,
      AuthorName = page.Author != null ? $"{page.Author.FirstName} {page.Author.LastName}".Trim() : null,
          BlockCount = page.Blocks.Count,
   CreatedAt = page.CreatedAt,
 UpdatedAt = page.UpdatedAt,
          Blocks = page.Blocks.OrderBy(b => b.DisplayOrder).Select(b => new PageBlockDto
            {
  Id = b.Id,
      BlockType = b.BlockType,
          Configuration = JsonSerializer.Deserialize<object>(b.Configuration)!,
           DisplayOrder = b.DisplayOrder,
       IsVisible = b.IsVisible,
  CssClass = b.CssClass,
       CustomId = b.CustomId
            }).ToList()
 };
    }
}
