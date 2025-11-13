using Gahar_Backend.Models.DTOs.Layout;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Implementations;

public class LayoutService : ILayoutService
{
    private readonly ILayoutRepository _layoutRepo;
    private readonly IContentRepository _contentRepo;
    private readonly IAuditLogService _auditLogService;

    public LayoutService(
        ILayoutRepository layoutRepo,
   IContentRepository contentRepo,
        IAuditLogService auditLogService)
    {
    _layoutRepo = layoutRepo;
        _contentRepo = contentRepo;
        _auditLogService = auditLogService;
    }

    public async Task<IEnumerable<LayoutDto>> GetAllAsync()
    {
        var layouts = await _layoutRepo.GetAllAsync();
        return layouts.Select(MapToDto).ToList();
    }

    public async Task<LayoutDetailDto> GetByIdAsync(int id)
    {
var layout = await _layoutRepo.GetByIdAsync(id);
        if (layout == null)
throw new NotFoundException($"التخطيط برقم {id} غير موجود");

     return MapToDetailDto(layout);
    }

    public async Task<LayoutDto> GetDefaultAsync()
    {
        var layout = await _layoutRepo.GetDefaultAsync();
        if (layout == null)
        throw new NotFoundException("لا يوجد تخطيط افتراضي");

   return MapToDto(layout);
    }

    public async Task<LayoutDto> CreateAsync(CreateLayoutDto dto)
    {
        var layout = new Layout
   {
          Name = dto.Name,
     Description = dto.Description,
            Configuration = dto.Configuration,
 PreviewImage = dto.PreviewImage,
            IsActive = true,
  IsDefault = false
        };

        await _layoutRepo.AddAsync(layout);
     await _layoutRepo.SaveChangesAsync();

  await _auditLogService.LogAsync(null, "Create", "Layout", layout.Id, $"إنشاء تخطيط: {layout.Name}");

        return MapToDto(layout);
    }

    public async Task<LayoutDto> UpdateAsync(int id, UpdateLayoutDto dto)
    {
  var layout = await _layoutRepo.GetByIdAsync(id);
        if (layout == null)
 throw new NotFoundException($"التخطيط برقم {id} غير موجود");

        layout.Name = dto.Name;
        layout.Description = dto.Description;
 layout.Configuration = dto.Configuration;
      layout.PreviewImage = dto.PreviewImage;
  layout.IsActive = dto.IsActive;

        _layoutRepo.Update(layout);
        await _layoutRepo.SaveChangesAsync();

        return MapToDto(layout);
    }

    public async Task DeleteAsync(int id)
    {
        var layout = await _layoutRepo.GetByIdAsync(id);
        if (layout == null)
    throw new NotFoundException($"التخطيط برقم {id} غير موجود");

        if (layout.IsDefault)
        throw new BadRequestException("لا يمكن حذف التخطيط الافتراضي");

        if (layout.Contents?.Count > 0)
          throw new BadRequestException("لا يمكن حذف تخطيط يحتوي على محتوى");

   layout.IsDeleted = true;
        _layoutRepo.Update(layout);
 await _layoutRepo.SaveChangesAsync();
    }

    public async Task SetDefaultAsync(int id)
    {
    var layout = await _layoutRepo.GetByIdAsync(id);
        if (layout == null)
            throw new NotFoundException($"التخطيط برقم {id} غير موجود");

        // إزالة الافتراضي من جميع التخطيطات
        var currentDefault = await _layoutRepo.GetDefaultAsync();
        if (currentDefault != null)
        {
    currentDefault.IsDefault = false;
            _layoutRepo.Update(currentDefault);
        }

        // تعيين الجديد كافتراضي
        layout.IsDefault = true;
        _layoutRepo.Update(layout);
   await _layoutRepo.SaveChangesAsync();

        await _auditLogService.LogAsync(null, "Update", "Layout", id, $"تعيين التخطيط كافتراضي: {layout.Name}");
    }

  public async Task BulkUpdateLayoutAsync(int layoutId, List<int> contentIds)
    {
        var layout = await _layoutRepo.GetByIdAsync(layoutId);
        if (layout == null)
            throw new NotFoundException($"التخطيط برقم {layoutId} غير موجود");

     foreach (var contentId in contentIds)
        {
            var content = await _contentRepo.GetByIdAsync(contentId);
      if (content != null)
    {
           content.LayoutId = layoutId;
      _contentRepo.Update(content);
    }
        }

        await _contentRepo.SaveChangesAsync();

        await _auditLogService.LogAsync(null, "Update", "Layout", layoutId, 
      $"تحديث {contentIds.Count} محتوى بالتخطيط: {layout.Name}");
    }

#region Helper Methods

    private LayoutDto MapToDto(Layout layout)
    {
        return new LayoutDto
        {
 Id = layout.Id,
         Name = layout.Name,
      Description = layout.Description,
Configuration = layout.Configuration,
 IsDefault = layout.IsDefault,
            IsActive = layout.IsActive,
            PreviewImage = layout.PreviewImage,
       ContentCount = layout.Contents?.Count ?? 0,
            CreatedAt = layout.CreatedAt
        };
 }

    private LayoutDetailDto MapToDetailDto(Layout layout)
    {
        return new LayoutDetailDto
      {
      Id = layout.Id,
  Name = layout.Name,
    Description = layout.Description,
 Configuration = layout.Configuration,
       IsDefault = layout.IsDefault,
    IsActive = layout.IsActive,
PreviewImage = layout.PreviewImage,
            ContentCount = layout.Contents?.Count ?? 0,
          CreatedAt = layout.CreatedAt
        };
    }

 #endregion
}
