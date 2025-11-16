using AutoMapper;
using Gahar_Backend.Models.DTOs.ShortLinks;
using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Mappings;

/// <summary>
/// AutoMapper profile for Short Links
/// </summary>
public class ShortLinkMappingProfile : Profile
{
    public ShortLinkMappingProfile()
    {
        // ShortLink entity to DTO
        CreateMap<ShortLink, ShortLinkDto>()
            .ReverseMap();

        // ShortLinkAnalytics entity to DTO
     CreateMap<ShortLinkAnalytics, ShortLinkAnalyticsDto>()
          .ReverseMap();
 }
}
