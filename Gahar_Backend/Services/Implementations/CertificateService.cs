using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Models.DTOs.Certificate;
using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Implementations;

public class CertificateService : ICertificateService
{
    private readonly ICertificateRepository _certRepo;
    private readonly ICertificateCategoryRepository _categoryRepo;
    private readonly ICertificateRequirementRepository _requirementRepo;
    private readonly ICertificateHolderRepository _holderRepo;

    public CertificateService(
ICertificateRepository certRepo,
        ICertificateCategoryRepository categoryRepo,
        ICertificateRequirementRepository requirementRepo,
        ICertificateHolderRepository holderRepo)
    {
      _certRepo = certRepo;
        _categoryRepo = categoryRepo;
        _requirementRepo = requirementRepo;
        _holderRepo = holderRepo;
    }

    #region Certificate Management

    public async Task<PagedResult<CertificateListDto>> GetAllAsync(CertificateFilterDto filter)
    {
        var query = _certRepo.GetQueryable()
      .Include(c => c.Author)
        .Include(c => c.Categories)
            .Include(c => c.Requirements)
      .Include(c => c.Holders)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        query = query.Where(c => c.Name.Contains(filter.SearchTerm) || c.Description!.Contains(filter.SearchTerm));

if (filter.IsPublished.HasValue)
   query = query.Where(c => c.IsPublished == filter.IsPublished.Value);

        if (!string.IsNullOrWhiteSpace(filter.IssuingBody))
            query = query.Where(c => c.IssuingBody == filter.IssuingBody);

     if (filter.IsRenewable.HasValue)
            query = query.Where(c => c.IsRenewable == filter.IsRenewable.Value);

        var totalCount = await query.CountAsync();

 query = filter.SortBy?.ToLower() switch
        {
    "name" => filter.SortOrder?.ToLower() == "asc" ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
            _ => filter.SortOrder?.ToLower() == "asc" ? query.OrderBy(c => c.DisplayOrder) : query.OrderByDescending(c => c.DisplayOrder)
    };

  var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
    .Select(c => MapToListDto(c))
  .ToListAsync();

        return new PagedResult<CertificateListDto>
        {
    Items = items,
            TotalCount = totalCount,
  PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

    public async Task<CertificateDetailDto> GetByIdAsync(int id)
    {
 var cert = await _certRepo.GetQueryable()
            .Include(c => c.Author)
      .Include(c => c.Categories.OrderBy(c => c.DisplayOrder))
            .Include(c => c.Requirements.OrderBy(r => r.DisplayOrder))
 .Include(c => c.Holders)
  .FirstOrDefaultAsync(c => c.Id == id);

        if (cert == null)
  throw new NotFoundException($"Certificate with ID {id} not found");

    return MapToDetailDto(cert);
    }

    public async Task<CertificateDetailDto> GetBySlugAsync(string slug)
    {
     var cert = await _certRepo.GetBySlugAsync(slug);

        if (cert == null)
            throw new NotFoundException($"Certificate with slug '{slug}' not found");

        if (!cert.IsPublished || !cert.IsActive)
     throw new BadRequestException("Certificate is not available");

        return MapToDetailDto(cert);
    }

    public async Task<CertificateDetailDto> CreateAsync(CreateCertificateDto dto, int authorId)
    {
     if (await _certRepo.SlugExistsAsync(dto.Slug))
            throw new BadRequestException($"Slug '{dto.Slug}' already exists");

        var cert = new Certificate
        {
        Name = dto.Name,
            Slug = dto.Slug,
            Description = dto.Description,
       ImageUrl = dto.ImageUrl,
          DocumentUrl = dto.DocumentUrl,
IssuingBody = dto.IssuingBody,
     IssueDate = dto.IssueDate,
     ExpiryDate = dto.ExpiryDate,
          IsRenewable = dto.IsRenewable,
     AuthorId = authorId,
        IsActive = true,
         IsPublished = false
    };

        await _certRepo.AddAsync(cert);
     await _certRepo.SaveChangesAsync();

        return await GetByIdAsync(cert.Id);
    }

    public async Task<CertificateDetailDto> UpdateAsync(int id, UpdateCertificateDto dto)
    {
     var cert = await _certRepo.GetByIdAsync(id);
        if (cert == null)
            throw new NotFoundException($"Certificate with ID {id} not found");

     if (dto.Slug != cert.Slug && await _certRepo.SlugExistsAsync(dto.Slug, id))
          throw new BadRequestException($"Slug '{dto.Slug}' already exists");

        cert.Name = dto.Name;
   cert.Slug = dto.Slug;
        cert.Description = dto.Description;
    cert.ImageUrl = dto.ImageUrl;
   cert.DocumentUrl = dto.DocumentUrl;
      cert.DisplayOrder = dto.DisplayOrder;
        cert.IssuingBody = dto.IssuingBody;
        cert.IssueDate = dto.IssueDate;
        cert.ExpiryDate = dto.ExpiryDate;
 cert.IsRenewable = dto.IsRenewable;
        cert.IsActive = dto.IsActive;
        cert.IsPublished = dto.IsPublished;

        _certRepo.Update(cert);
        await _certRepo.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

  public async Task DeleteAsync(int id)
    {
        var cert = await _certRepo.GetByIdAsync(id);
        if (cert == null)
            throw new NotFoundException($"Certificate with ID {id} not found");

     _certRepo.Delete(cert);
        await _certRepo.SaveChangesAsync();
    }

    public async Task PublishAsync(int id)
    {
   var cert = await _certRepo.GetByIdAsync(id);
        if (cert == null)
         throw new NotFoundException($"Certificate with ID {id} not found");

        cert.IsPublished = true;
   cert.PublishedAt = DateTime.UtcNow;

        _certRepo.Update(cert);
  await _certRepo.SaveChangesAsync();
    }

    public async Task UnpublishAsync(int id)
    {
        var cert = await _certRepo.GetByIdAsync(id);
        if (cert == null)
            throw new NotFoundException($"Certificate with ID {id} not found");

     cert.IsPublished = false;

        _certRepo.Update(cert);
 await _certRepo.SaveChangesAsync();
  }

    #endregion

  #region Categories

    public async Task<CertificateCategoryDto> AddCategoryAsync(int certificateId, CreateCertificateCategoryDto dto)
    {
        var cert = await _certRepo.GetByIdAsync(certificateId);
        if (cert == null)
            throw new NotFoundException($"Certificate with ID {certificateId} not found");

 var maxOrder = await _categoryRepo.GetQueryable()
        .Where(c => c.CertificateId == certificateId)
    .MaxAsync(c => (int?)c.DisplayOrder) ?? 0;

        var category = new CertificateCategory
      {
            CertificateId = certificateId,
    Name = dto.Name,
      Description = dto.Description,
     DisplayOrder = maxOrder + 1
      };

        await _categoryRepo.AddAsync(category);
     await _categoryRepo.SaveChangesAsync();

 return MapCategoryToDto(category);
    }

    public async Task<CertificateCategoryDto> UpdateCategoryAsync(int certificateId, int categoryId, UpdateCertificateCategoryDto dto)
    {
        var category = await _categoryRepo.GetQueryable()
  .FirstOrDefaultAsync(c => c.Id == categoryId && c.CertificateId == certificateId);

        if (category == null)
            throw new NotFoundException($"Category with ID {categoryId} not found");

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.DisplayOrder = dto.DisplayOrder;

        _categoryRepo.Update(category);
        await _categoryRepo.SaveChangesAsync();

     return MapCategoryToDto(category);
    }

    public async Task DeleteCategoryAsync(int certificateId, int categoryId)
    {
        var category = await _categoryRepo.GetQueryable()
          .FirstOrDefaultAsync(c => c.Id == categoryId && c.CertificateId == certificateId);

        if (category == null)
       throw new NotFoundException($"Category with ID {categoryId} not found");

      _categoryRepo.Delete(category);
      await _categoryRepo.SaveChangesAsync();
 }

    #endregion

    #region Requirements

    public async Task<CertificateRequirementDto> AddRequirementAsync(int certificateId, CreateCertificateRequirementDto dto)
    {
        var cert = await _certRepo.GetByIdAsync(certificateId);
        if (cert == null)
      throw new NotFoundException($"Certificate with ID {certificateId} not found");

   var maxOrder = await _requirementRepo.GetQueryable()
 .Where(r => r.CertificateId == certificateId)
      .MaxAsync(r => (int?)r.DisplayOrder) ?? 0;

        var requirement = new CertificateRequirement
      {
        CertificateId = certificateId,
        Requirement = dto.Requirement,
            Description = dto.Description,
     DisplayOrder = maxOrder + 1,
   IsOptional = dto.IsOptional
   };

        await _requirementRepo.AddAsync(requirement);
   await _requirementRepo.SaveChangesAsync();

        return MapRequirementToDto(requirement);
}

    public async Task<CertificateRequirementDto> UpdateRequirementAsync(int certificateId, int requirementId, UpdateCertificateRequirementDto dto)
    {
        var requirement = await _requirementRepo.GetQueryable()
            .FirstOrDefaultAsync(r => r.Id == requirementId && r.CertificateId == certificateId);

      if (requirement == null)
            throw new NotFoundException($"Requirement with ID {requirementId} not found");

        requirement.Requirement = dto.Requirement;
        requirement.Description = dto.Description;
        requirement.DisplayOrder = dto.DisplayOrder;
        requirement.IsOptional = dto.IsOptional;

        _requirementRepo.Update(requirement);
        await _requirementRepo.SaveChangesAsync();

        return MapRequirementToDto(requirement);
    }

    public async Task DeleteRequirementAsync(int certificateId, int requirementId)
    {
        var requirement = await _requirementRepo.GetQueryable()
        .FirstOrDefaultAsync(r => r.Id == requirementId && r.CertificateId == certificateId);

        if (requirement == null)
            throw new NotFoundException($"Requirement with ID {requirementId} not found");

   _requirementRepo.Delete(requirement);
        await _requirementRepo.SaveChangesAsync();
  }

    #endregion

    #region Holders

    public async Task<CertificateHolderDto> AddHolderAsync(int certificateId, CreateCertificateHolderDto dto)
    {
    var cert = await _certRepo.GetByIdAsync(certificateId);
        if (cert == null)
   throw new NotFoundException($"Certificate with ID {certificateId} not found");

        var holder = new CertificateHolder
        {
          CertificateId = certificateId,
      HolderName = dto.HolderName,
 HolderEmail = dto.HolderEmail,
     RegistrationNumber = dto.RegistrationNumber,
            IssueDate = dto.IssueDate,
         ExpiryDate = dto.ExpiryDate,
   CertificateUrl = dto.CertificateUrl
        };

        await _holderRepo.AddAsync(holder);
        await _holderRepo.SaveChangesAsync();

        return MapHolderToDto(holder);
    }

    public async Task<CertificateHolderDto> UpdateHolderAsync(int certificateId, int holderId, UpdateCertificateHolderDto dto)
    {
        var holder = await _holderRepo.GetQueryable()
     .FirstOrDefaultAsync(h => h.Id == holderId && h.CertificateId == certificateId);

  if (holder == null)
    throw new NotFoundException($"Holder with ID {holderId} not found");

        holder.HolderName = dto.HolderName;
        holder.HolderEmail = dto.HolderEmail;
        holder.RegistrationNumber = dto.RegistrationNumber;
   holder.IssueDate = dto.IssueDate;
        holder.ExpiryDate = dto.ExpiryDate;
        holder.CertificateUrl = dto.CertificateUrl;
        holder.IsVerified = dto.IsVerified;
        holder.VerificationNotes = dto.VerificationNotes;

      _holderRepo.Update(holder);
        await _holderRepo.SaveChangesAsync();

        return MapHolderToDto(holder);
    }

    public async Task DeleteHolderAsync(int certificateId, int holderId)
    {
        var holder = await _holderRepo.GetQueryable()
            .FirstOrDefaultAsync(h => h.Id == holderId && h.CertificateId == certificateId);

        if (holder == null)
      throw new NotFoundException($"Holder with ID {holderId} not found");

        _holderRepo.Delete(holder);
        await _holderRepo.SaveChangesAsync();
    }

    public async Task<CertificateHolderDto> VerifyHolderAsync(int certificateId, int holderId, bool isVerified, string? notes)
    {
        var holder = await _holderRepo.GetQueryable()
.FirstOrDefaultAsync(h => h.Id == holderId && h.CertificateId == certificateId);

        if (holder == null)
    throw new NotFoundException($"Holder with ID {holderId} not found");

        holder.IsVerified = isVerified;
        holder.VerificationNotes = notes;

        _holderRepo.Update(holder);
        await _holderRepo.SaveChangesAsync();

        return MapHolderToDto(holder);
}

    public async Task<IEnumerable<CertificateHolderDto>> SearchHoldersAsync(string searchTerm)
    {
        var holders = await _holderRepo.SearchAsync(searchTerm);
        return holders.Select(MapHolderToDto);
    }

    #endregion

    #region Helper Methods

    private CertificateListDto MapToListDto(Certificate cert)
    {
        return new CertificateListDto
        {
            Id = cert.Id,
            Name = cert.Name,
            Slug = cert.Slug,
  Description = cert.Description,
            ImageUrl = cert.ImageUrl,
            DisplayOrder = cert.DisplayOrder,
            IssuingBody = cert.IssuingBody,
            IssueDate = cert.IssueDate,
            ExpiryDate = cert.ExpiryDate,
            IsExpired = cert.IsExpired,
    IsRenewable = cert.IsRenewable,
            IsActive = cert.IsActive,
      IsPublished = cert.IsPublished,
         PublishedAt = cert.PublishedAt,
          AuthorName = cert.Author != null ? $"{cert.Author.FirstName} {cert.Author.LastName}".Trim() : null,
            CategoryCount = cert.Categories.Count,
 RequirementCount = cert.Requirements.Count,
            HolderCount = cert.Holders.Count,
       CreatedAt = cert.CreatedAt,
            UpdatedAt = cert.UpdatedAt
     };
    }

    private CertificateDetailDto MapToDetailDto(Certificate cert)
    {
  return new CertificateDetailDto
        {
       Id = cert.Id,
            Name = cert.Name,
    Slug = cert.Slug,
  Description = cert.Description,
ImageUrl = cert.ImageUrl,
            DocumentUrl = cert.DocumentUrl,
      DisplayOrder = cert.DisplayOrder,
      IssuingBody = cert.IssuingBody,
  IssueDate = cert.IssueDate,
ExpiryDate = cert.ExpiryDate,
            IsExpired = cert.IsExpired,
          IsRenewable = cert.IsRenewable,
            IsActive = cert.IsActive,
  IsPublished = cert.IsPublished,
    PublishedAt = cert.PublishedAt,
            AuthorName = cert.Author != null ? $"{cert.Author.FirstName} {cert.Author.LastName}".Trim() : null,
       CategoryCount = cert.Categories.Count,
        RequirementCount = cert.Requirements.Count,
            HolderCount = cert.Holders.Count,
          CreatedAt = cert.CreatedAt,
            UpdatedAt = cert.UpdatedAt,
            Categories = cert.Categories.Select(MapCategoryToDto).ToList(),
      Requirements = cert.Requirements.Select(MapRequirementToDto).ToList(),
            Holders = cert.Holders.Select(MapHolderToDto).ToList()
        };
    }

    private CertificateCategoryDto MapCategoryToDto(CertificateCategory cat)
    {
    return new CertificateCategoryDto
        {
         Id = cat.Id,
      Name = cat.Name,
   Description = cat.Description,
            DisplayOrder = cat.DisplayOrder,
  IsActive = cat.IsActive
        };
    }

    private CertificateRequirementDto MapRequirementToDto(CertificateRequirement req)
    {
        return new CertificateRequirementDto
        {
       Id = req.Id,
      Requirement = req.Requirement,
    Description = req.Description,
         DisplayOrder = req.DisplayOrder,
            IsOptional = req.IsOptional
        };
    }

    private CertificateHolderDto MapHolderToDto(CertificateHolder holder)
    {
        return new CertificateHolderDto
        {
  Id = holder.Id,
    HolderName = holder.HolderName,
   HolderEmail = holder.HolderEmail,
    RegistrationNumber = holder.RegistrationNumber,
          IssueDate = holder.IssueDate,
            ExpiryDate = holder.ExpiryDate,
    IsExpired = holder.IsExpired,
    IsVerified = holder.IsVerified,
     CertificateUrl = holder.CertificateUrl,
         VerificationNotes = holder.VerificationNotes
        };
 }

    #endregion
}
