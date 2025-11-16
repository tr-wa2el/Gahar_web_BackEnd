using Gahar_Backend.Models.DTOs.Certificate;
using Gahar_Backend.Models.DTOs.Common;

namespace Gahar_Backend.Services.Interfaces;

public interface ICertificateService
{
    // Certificate Management
    Task<PagedResult<CertificateListDto>> GetAllAsync(CertificateFilterDto filter);
    Task<CertificateDetailDto> GetByIdAsync(int id);
    Task<CertificateDetailDto> GetBySlugAsync(string slug);
    Task<CertificateDetailDto> CreateAsync(CreateCertificateDto dto, int authorId);
    Task<CertificateDetailDto> UpdateAsync(int id, UpdateCertificateDto dto);
    Task DeleteAsync(int id);
    Task PublishAsync(int id);
    Task UnpublishAsync(int id);

    // Categories
    Task<CertificateCategoryDto> AddCategoryAsync(int certificateId, CreateCertificateCategoryDto dto);
    Task<CertificateCategoryDto> UpdateCategoryAsync(int certificateId, int categoryId, UpdateCertificateCategoryDto dto);
    Task DeleteCategoryAsync(int certificateId, int categoryId);

    // Requirements
    Task<CertificateRequirementDto> AddRequirementAsync(int certificateId, CreateCertificateRequirementDto dto);
    Task<CertificateRequirementDto> UpdateRequirementAsync(int certificateId, int requirementId, UpdateCertificateRequirementDto dto);
    Task DeleteRequirementAsync(int certificateId, int requirementId);

    // Holders
    Task<CertificateHolderDto> AddHolderAsync(int certificateId, CreateCertificateHolderDto dto);
    Task<CertificateHolderDto> UpdateHolderAsync(int certificateId, int holderId, UpdateCertificateHolderDto dto);
    Task DeleteHolderAsync(int certificateId, int holderId);
    Task<CertificateHolderDto> VerifyHolderAsync(int certificateId, int holderId, bool isVerified, string? notes);
    Task<IEnumerable<CertificateHolderDto>> SearchHoldersAsync(string searchTerm);
}
