using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Facility;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Implementations;

public class FacilityService : IFacilityService
{
    private readonly IFacilityRepository _facilityRepo;
    private readonly IFacilityDepartmentRepository _departmentRepo;
 private readonly IFacilityServiceRepository _serviceRepo;
private readonly IFacilityImageRepository _imageRepo;
    private readonly IFacilityReviewRepository _reviewRepo;

    public FacilityService(
        IFacilityRepository facilityRepo,
        IFacilityDepartmentRepository departmentRepo,
IFacilityServiceRepository serviceRepo,
        IFacilityImageRepository imageRepo,
      IFacilityReviewRepository reviewRepo)
    {
        _facilityRepo = facilityRepo;
        _departmentRepo = departmentRepo;
      _serviceRepo = serviceRepo;
  _imageRepo = imageRepo;
       _reviewRepo = reviewRepo;
    }

    #region Facility Management

    public async Task<PagedResult<FacilityListDto>> GetAllAsync(FacilityFilterDto filter)
    {
        var query = _facilityRepo.GetQueryable()
       .Include(f => f.Author)
            .Include(f => f.Departments)
           .Include(f => f.Services)
      .Include(f => f.Images)
  .Include(f => f.Reviews)
  .AsQueryable();

 if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
    {
       query = query.Where(f =>
   f.Name.Contains(filter.SearchTerm) ||
       f.Description!.Contains(filter.SearchTerm) ||
     f.Address!.Contains(filter.SearchTerm));
        }

       if (filter.IsPublished.HasValue)
       query = query.Where(f => f.IsPublished == filter.IsPublished.Value);

       if (!string.IsNullOrWhiteSpace(filter.AccreditationStatus))
       query = query.Where(f => f.AccreditationStatus == filter.AccreditationStatus);

  var totalCount = await query.CountAsync();

  query = filter.SortBy?.ToLower() switch
        {
    "name" => filter.SortOrder?.ToLower() == "asc"
? query.OrderBy(f => f.Name)
       : query.OrderByDescending(f => f.Name),
        _ => filter.SortOrder?.ToLower() == "asc"
             ? query.OrderBy(f => f.CreatedAt)
      : query.OrderByDescending(f => f.CreatedAt)
      };

      var items = await query
          .Skip((filter.PageNumber - 1) * filter.PageSize)
   .Take(filter.PageSize)
    .Select(f => MapToListDto(f))
            .ToListAsync();

        return new PagedResult<FacilityListDto>
       {
           Items = items,
    TotalCount = totalCount,
 PageNumber = filter.PageNumber,
   PageSize = filter.PageSize
        };
    }

    public async Task<FacilityDetailDto> GetByIdAsync(int id)
    {
  var facility = await _facilityRepo.GetQueryable()
     .Include(f => f.Author)
       .Include(f => f.Departments.OrderBy(d => d.DisplayOrder))
 .Include(f => f.Services.OrderBy(s => s.DisplayOrder))
 .Include(f => f.Images.OrderBy(i => i.DisplayOrder))
     .Include(f => f.Reviews.Where(r => r.IsApproved).OrderByDescending(r => r.CreatedAt))
     .FirstOrDefaultAsync(f => f.Id == id);

      if (facility == null)
   throw new NotFoundException($"Facility with ID {id} not found");

 return MapToDetailDto(facility);
    }

   public async Task<FacilityDetailDto> GetBySlugAsync(string slug)
    {
        var facility = await _facilityRepo.GetBySlugAsync(slug);

    if (facility == null)
      throw new NotFoundException($"Facility with slug '{slug}' not found");

      if (!facility.IsPublished || !facility.IsActive)
   throw new BadRequestException("Facility is not available");

     return MapToDetailDto(facility);
    }

 public async Task<FacilityDetailDto> CreateAsync(CreateFacilityDto dto, int authorId)
    {
        if (await _facilityRepo.SlugExistsAsync(dto.Slug))
     throw new BadRequestException($"Slug '{dto.Slug}' already exists");

 var facility = new Facility
      {
    Name = dto.Name,
       Slug = dto.Slug,
  Description = dto.Description,
      Address = dto.Address,
       PhoneNumber = dto.PhoneNumber,
   Email = dto.Email,
 Website = dto.Website,
    Latitude = dto.Latitude,
       Longitude = dto.Longitude,
        DirectorName = dto.DirectorName,
     TotalBeds = dto.TotalBeds,
   AverageWaitTime = dto.AverageWaitTime,
          AccreditationStatus = dto.AccreditationStatus,
      AuthorId = authorId,
        IsActive = true,
   IsPublished = false
    };

   await _facilityRepo.AddAsync(facility);
        await _facilityRepo.SaveChangesAsync();

   return await GetByIdAsync(facility.Id);
 }

    public async Task<FacilityDetailDto> UpdateAsync(int id, UpdateFacilityDto dto)
   {
        var facility = await _facilityRepo.GetByIdAsync(id);
  if (facility == null)
         throw new NotFoundException($"Facility with ID {id} not found");

    if (dto.Slug != facility.Slug && await _facilityRepo.SlugExistsAsync(dto.Slug, id))
   throw new BadRequestException($"Slug '{dto.Slug}' already exists");

      facility.Name = dto.Name;
 facility.Slug = dto.Slug;
facility.Description = dto.Description;
 facility.Address = dto.Address;
 facility.PhoneNumber = dto.PhoneNumber;
 facility.Email = dto.Email;
 facility.Website = dto.Website;
 facility.Latitude = dto.Latitude;
    facility.Longitude = dto.Longitude;
     facility.DirectorName = dto.DirectorName;
       facility.TotalBeds = dto.TotalBeds;
      facility.AverageWaitTime = dto.AverageWaitTime;
      facility.AccreditationStatus = dto.AccreditationStatus;
    facility.IsActive = dto.IsActive;
 facility.IsPublished = dto.IsPublished;

    _facilityRepo.Update(facility);
      await _facilityRepo.SaveChangesAsync();

    return await GetByIdAsync(id);
   }

   public async Task DeleteAsync(int id)
    {
      var facility = await _facilityRepo.GetByIdAsync(id);
    if (facility == null)
    throw new NotFoundException($"Facility with ID {id} not found");

   _facilityRepo.Delete(facility);
        await _facilityRepo.SaveChangesAsync();
    }

    public async Task PublishAsync(int id)
   {
        var facility = await _facilityRepo.GetByIdAsync(id);
     if (facility == null)
    throw new NotFoundException($"Facility with ID {id} not found");

        facility.IsPublished = true;
       facility.PublishedAt = DateTime.UtcNow;

   _facilityRepo.Update(facility);
      await _facilityRepo.SaveChangesAsync();
   }

    public async Task UnpublishAsync(int id)
    {
        var facility = await _facilityRepo.GetByIdAsync(id);
    if (facility == null)
      throw new NotFoundException($"Facility with ID {id} not found");

        facility.IsPublished = false;

   _facilityRepo.Update(facility);
      await _facilityRepo.SaveChangesAsync();
    }

   #endregion

   #region Departments

 public async Task<FacilityDepartmentDto> AddDepartmentAsync(int facilityId, CreateFacilityDepartmentDto dto)
    {
      var facility = await _facilityRepo.GetByIdAsync(facilityId);
    if (facility == null)
    throw new NotFoundException($"Facility with ID {facilityId} not found");

   var maxOrder = await _departmentRepo.GetQueryable()
    .Where(d => d.FacilityId == facilityId)
   .MaxAsync(d => (int?)d.DisplayOrder) ?? 0;

  var department = new FacilityDepartment
   {
    FacilityId = facilityId,
    Name = dto.Name,
           Description = dto.Description,
   PhoneNumber = dto.PhoneNumber,
     HeadName = dto.HeadName,
            DisplayOrder = maxOrder + 1
    };

      await _departmentRepo.AddAsync(department);
        await _departmentRepo.SaveChangesAsync();

  return MapDepartmentToDto(department);
  }

    public async Task<FacilityDepartmentDto> UpdateDepartmentAsync(int facilityId, int departmentId, UpdateFacilityDepartmentDto dto)
   {
     var department = await _departmentRepo.GetQueryable()
          .FirstOrDefaultAsync(d => d.Id == departmentId && d.FacilityId == facilityId);

    if (department == null)
    throw new NotFoundException($"Department with ID {departmentId} not found");

   department.Name = dto.Name;
       department.Description = dto.Description;
      department.PhoneNumber = dto.PhoneNumber;
 department.HeadName = dto.HeadName;
       department.DisplayOrder = dto.DisplayOrder;

    _departmentRepo.Update(department);
       await _departmentRepo.SaveChangesAsync();

        return MapDepartmentToDto(department);
  }

  public async Task DeleteDepartmentAsync(int facilityId, int departmentId)
    {
      var department = await _departmentRepo.GetQueryable()
      .FirstOrDefaultAsync(d => d.Id == departmentId && d.FacilityId == facilityId);

  if (department == null)
     throw new NotFoundException($"Department with ID {departmentId} not found");

     _departmentRepo.Delete(department);
       await _departmentRepo.SaveChangesAsync();
    }

  #endregion

#region Services

 public async Task<FacilityServiceDto> AddServiceAsync(int facilityId, CreateFacilityServiceDto dto)
    {
 var facility = await _facilityRepo.GetByIdAsync(facilityId);
  if (facility == null)
  throw new NotFoundException($"Facility with ID {facilityId} not found");

  var maxOrder = await _serviceRepo.GetQueryable()
 .Where(s => s.FacilityId == facilityId)
    .MaxAsync(s => (int?)s.DisplayOrder) ?? 0;

  var facilityService = new Models.Entities.FacilityService
    {
    FacilityId = facilityId,
        Name = dto.Name,
       Description = dto.Description,
        Icon = dto.Icon,
  DisplayOrder = maxOrder + 1
    };

     await _serviceRepo.AddAsync(facilityService);
    await _serviceRepo.SaveChangesAsync();

   return MapServiceToDto(facilityService);
   }

    public async Task<FacilityServiceDto> UpdateServiceAsync(int facilityId, int serviceId, UpdateFacilityServiceDto dto)
    {
  var service = await _serviceRepo.GetQueryable()
 .FirstOrDefaultAsync(s => s.Id == serviceId && s.FacilityId == facilityId);

    if (service == null)
       throw new NotFoundException($"Service with ID {serviceId} not found");

      service.Name = dto.Name;
 service.Description = dto.Description;
 service.Icon = dto.Icon;
    service.DisplayOrder = dto.DisplayOrder;

    _serviceRepo.Update(service);
        await _serviceRepo.SaveChangesAsync();

      return MapServiceToDto(service);
   }

  public async Task DeleteServiceAsync(int facilityId, int serviceId)
  {
    var service = await _serviceRepo.GetQueryable()
     .FirstOrDefaultAsync(s => s.Id == serviceId && s.FacilityId == facilityId);

   if (service == null)
    throw new NotFoundException($"Service with ID {serviceId} not found");

      _serviceRepo.Delete(service);
      await _serviceRepo.SaveChangesAsync();
    }

  #endregion

  #region Images

    public async Task<FacilityImageDto> AddImageAsync(int facilityId, CreateFacilityImageDto dto)
    {
  var facility = await _facilityRepo.GetByIdAsync(facilityId);
       if (facility == null)
         throw new NotFoundException($"Facility with ID {facilityId} not found");

      if (dto.IsMainImage)
     {
      var existingMain = await _imageRepo.GetQueryable()
   .FirstOrDefaultAsync(i => i.FacilityId == facilityId && i.IsMainImage);
         if (existingMain != null)
       {
      existingMain.IsMainImage = false;
         _imageRepo.Update(existingMain);
   }
  }

    var maxOrder = await _imageRepo.GetQueryable()
      .Where(i => i.FacilityId == facilityId)
      .MaxAsync(i => (int?)i.DisplayOrder) ?? 0;

        var image = new FacilityImage
   {
    FacilityId = facilityId,
        ImageUrl = dto.ImageUrl,
  Caption = dto.Caption,
DisplayOrder = maxOrder + 1,
       IsMainImage = dto.IsMainImage
    };

        await _imageRepo.AddAsync(image);
  await _imageRepo.SaveChangesAsync();

   return MapImageToDto(image);
    }

    public async Task<FacilityImageDto> UpdateImageAsync(int facilityId, int imageId, UpdateFacilityImageDto dto)
   {
     var image = await _imageRepo.GetQueryable()
       .FirstOrDefaultAsync(i => i.Id == imageId && i.FacilityId == facilityId);

   if (image == null)
      throw new NotFoundException($"Image with ID {imageId} not found");

  if (dto.IsMainImage && !image.IsMainImage)
      {
        var existingMain = await _imageRepo.GetQueryable()
       .FirstOrDefaultAsync(i => i.FacilityId == facilityId && i.IsMainImage);
 if (existingMain != null)
          {
    existingMain.IsMainImage = false;
        _imageRepo.Update(existingMain);
       }
 }

  image.ImageUrl = dto.ImageUrl;
     image.Caption = dto.Caption;
  image.DisplayOrder = dto.DisplayOrder;
      image.IsMainImage = dto.IsMainImage;

   _imageRepo.Update(image);
      await _imageRepo.SaveChangesAsync();

      return MapImageToDto(image);
 }

    public async Task DeleteImageAsync(int facilityId, int imageId)
   {
   var image = await _imageRepo.GetQueryable()
    .FirstOrDefaultAsync(i => i.Id == imageId && i.FacilityId == facilityId);

    if (image == null)
       throw new NotFoundException($"Image with ID {imageId} not found");

  _imageRepo.Delete(image);
     await _imageRepo.SaveChangesAsync();
  }

 #endregion

  #region Reviews

    public async Task<FacilityReviewDto> AddReviewAsync(int facilityId, CreateFacilityReviewDto dto)
   {
  var facility = await _facilityRepo.GetByIdAsync(facilityId);
       if (facility == null)
   throw new NotFoundException($"Facility with ID {facilityId} not found");

 var review = new FacilityReview
      {
        FacilityId = facilityId,
      ReviewerName = dto.ReviewerName,
    Email = dto.Email,
          ReviewText = dto.ReviewText,
    Rating = dto.Rating,
     IsApproved = false
     };

    await _reviewRepo.AddAsync(review);
       await _reviewRepo.SaveChangesAsync();

  return MapReviewToDto(review);
    }

 public async Task ApproveReviewAsync(int facilityId, int reviewId, ApproveFacilityReviewDto dto)
    {
 var review = await _reviewRepo.GetQueryable()
     .FirstOrDefaultAsync(r => r.Id == reviewId && r.FacilityId == facilityId);

 if (review == null)
   throw new NotFoundException($"Review with ID {reviewId} not found");

      review.IsApproved = dto.IsApproved;
        _reviewRepo.Update(review);
      await _reviewRepo.SaveChangesAsync();
    }

    public async Task<IEnumerable<FacilityReviewDto>> GetApprovedReviewsAsync(int facilityId)
    {
     var reviews = await _reviewRepo.GetApprovedAsync(facilityId);
 return reviews.Select(MapReviewToDto);
    }

    public async Task DeleteReviewAsync(int facilityId, int reviewId)
   {
      var review = await _reviewRepo.GetQueryable()
     .FirstOrDefaultAsync(r => r.Id == reviewId && r.FacilityId == facilityId);

     if (review == null)
          throw new NotFoundException($"Review with ID {reviewId} not found");

       _reviewRepo.Delete(review);
    await _reviewRepo.SaveChangesAsync();
   }

    #endregion

    #region Helper Methods

    private FacilityListDto MapToListDto(Facility facility)
  {
     return new FacilityListDto
      {
    Id = facility.Id,
      Name = facility.Name,
       Slug = facility.Slug,
     Description = facility.Description,
     Address = facility.Address,
   PhoneNumber = facility.PhoneNumber,
       Email = facility.Email,
  Latitude = facility.Latitude,
      Longitude = facility.Longitude,
    DirectorName = facility.DirectorName,
 TotalBeds = facility.TotalBeds,
     AverageWaitTime = facility.AverageWaitTime,
      AccreditationStatus = facility.AccreditationStatus,
     IsActive = facility.IsActive,
      IsPublished = facility.IsPublished,
         PublishedAt = facility.PublishedAt,
      AuthorName = facility.Author != null ? $"{facility.Author.FirstName} {facility.Author.LastName}".Trim() : null,
  DepartmentCount = facility.Departments.Count,
  ServiceCount = facility.Services.Count,
      ImageCount = facility.Images.Count,
    CreatedAt = facility.CreatedAt,
    UpdatedAt = facility.UpdatedAt
     };
    }

    private FacilityDetailDto MapToDetailDto(Facility facility)
    {
   return new FacilityDetailDto
   {
   Id = facility.Id,
    Name = facility.Name,
      Slug = facility.Slug,
    Description = facility.Description,
  Address = facility.Address,
        PhoneNumber = facility.PhoneNumber,
      Email = facility.Email,
 Latitude = facility.Latitude,
    Longitude = facility.Longitude,
  DirectorName = facility.DirectorName,
     TotalBeds = facility.TotalBeds,
      AverageWaitTime = facility.AverageWaitTime,
    AccreditationStatus = facility.AccreditationStatus,
       IsActive = facility.IsActive,
  IsPublished = facility.IsPublished,
  PublishedAt = facility.PublishedAt,
 AuthorName = facility.Author != null ? $"{facility.Author.FirstName} {facility.Author.LastName}".Trim() : null,
       DepartmentCount = facility.Departments.Count,
 ServiceCount = facility.Services.Count,
    ImageCount = facility.Images.Count,
      CreatedAt = facility.CreatedAt,
     UpdatedAt = facility.UpdatedAt,
  Departments = facility.Departments.Select(MapDepartmentToDto).ToList(),
        Services = facility.Services.Select(MapServiceToDto).ToList(),
         Images = facility.Images.Select(MapImageToDto).ToList(),
     Reviews = facility.Reviews.Select(MapReviewToDto).ToList(),
      AverageRating = facility.Reviews.Any() ? facility.Reviews.Average(r => r.Rating) : 0,
     ApprovedReviewCount = facility.Reviews.Count(r => r.IsApproved)
       };
    }

  private FacilityDepartmentDto MapDepartmentToDto(FacilityDepartment dept)
    {
      return new FacilityDepartmentDto
    {
     Id = dept.Id,
       Name = dept.Name,
    Description = dept.Description,
      PhoneNumber = dept.PhoneNumber,
HeadName = dept.HeadName,
      DisplayOrder = dept.DisplayOrder,
   IsActive = dept.IsActive
   };
    }

    private FacilityServiceDto MapServiceToDto(Models.Entities.FacilityService service)
    {
  return new FacilityServiceDto
{
     Id = service.Id,
      Name = service.Name,
        Description = service.Description,
 Icon = service.Icon,
   DisplayOrder = service.DisplayOrder,
      IsAvailable = service.IsAvailable
  };
   }

   private FacilityImageDto MapImageToDto(FacilityImage image)
    {
  return new FacilityImageDto
{
     Id = image.Id,
   ImageUrl = image.ImageUrl,
      Caption = image.Caption,
     DisplayOrder = image.DisplayOrder,
      IsMainImage = image.IsMainImage
     };
    }

  private FacilityReviewDto MapReviewToDto(FacilityReview review)
   {
      return new FacilityReviewDto
   {
   Id = review.Id,
    ReviewerName = review.ReviewerName,
     ReviewText = review.ReviewText,
        Rating = review.Rating,
   IsApproved = review.IsApproved,
      CreatedAt = review.CreatedAt
    };
    }

    #endregion
}
