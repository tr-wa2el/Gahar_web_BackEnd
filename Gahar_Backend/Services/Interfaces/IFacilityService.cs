using Gahar_Backend.Models.DTOs.Common;
using Gahar_Backend.Models.DTOs.Facility;

namespace Gahar_Backend.Services.Interfaces;

public interface IFacilityService
{
    // Facility Management
    Task<PagedResult<FacilityListDto>> GetAllAsync(FacilityFilterDto filter);
    Task<FacilityDetailDto> GetByIdAsync(int id);
    Task<FacilityDetailDto> GetBySlugAsync(string slug);
    Task<FacilityDetailDto> CreateAsync(CreateFacilityDto dto, int authorId);
    Task<FacilityDetailDto> UpdateAsync(int id, UpdateFacilityDto dto);
    Task DeleteAsync(int id);
    Task PublishAsync(int id);
    Task UnpublishAsync(int id);

// Departments
    Task<FacilityDepartmentDto> AddDepartmentAsync(int facilityId, CreateFacilityDepartmentDto dto);
    Task<FacilityDepartmentDto> UpdateDepartmentAsync(int facilityId, int departmentId, UpdateFacilityDepartmentDto dto);
    Task DeleteDepartmentAsync(int facilityId, int departmentId);

    // Services
    Task<FacilityServiceDto> AddServiceAsync(int facilityId, CreateFacilityServiceDto dto);
    Task<FacilityServiceDto> UpdateServiceAsync(int facilityId, int serviceId, UpdateFacilityServiceDto dto);
    Task DeleteServiceAsync(int facilityId, int serviceId);

    // Images
    Task<FacilityImageDto> AddImageAsync(int facilityId, CreateFacilityImageDto dto);
 Task<FacilityImageDto> UpdateImageAsync(int facilityId, int imageId, UpdateFacilityImageDto dto);
    Task DeleteImageAsync(int facilityId, int imageId);

    // Reviews
  Task<FacilityReviewDto> AddReviewAsync(int facilityId, CreateFacilityReviewDto dto);
    Task ApproveReviewAsync(int facilityId, int reviewId, ApproveFacilityReviewDto dto);
    Task<IEnumerable<FacilityReviewDto>> GetApprovedReviewsAsync(int facilityId);
    Task DeleteReviewAsync(int facilityId, int reviewId);
}
