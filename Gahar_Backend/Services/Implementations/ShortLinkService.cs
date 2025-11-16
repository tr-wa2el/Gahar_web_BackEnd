using AutoMapper;
using Gahar_Backend.Models.DTOs.ShortLinks;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Implementations;

/// <summary>
/// Service implementation for Short Links management
/// </summary>
public class ShortLinkService : IShortLinkService
{
    private readonly IShortLinkRepository _shortLinkRepository;
    private readonly IShortLinkAnalyticsRepository _analyticsRepository;
 private readonly IQrCodeService _qrCodeService;
    private readonly IMapper _mapper;
    private readonly ILogger<ShortLinkService> _logger;
    private const string BaseShortUrl = "https://sh.gahar.sa/";

    public ShortLinkService(
      IShortLinkRepository shortLinkRepository,
    IShortLinkAnalyticsRepository analyticsRepository,
    IQrCodeService qrCodeService,
        IMapper mapper,
        ILogger<ShortLinkService> logger)
    {
      _shortLinkRepository = shortLinkRepository;
        _analyticsRepository = analyticsRepository;
        _qrCodeService = qrCodeService;
 _mapper = mapper;
   _logger = logger;
    }

    public async Task<ShortLinkDto> CreateShortLinkAsync(CreateShortLinkDto dto, int userId)
  {
   try
        {
         if (!IsValidUrl(dto.OriginalUrl))
  {
   throw new BadRequestException("الرابط الأصلي غير صحيح");
  }

 string shortCode = await GenerateUniqueShortCodeAsync();

            var shortLink = new ShortLink
            {
    OriginalUrl = dto.OriginalUrl,
                ShortCode = shortCode,
           ShortUrl = $"{BaseShortUrl}{shortCode}",
          Title = dto.Title,
          Description = dto.Description,
     ExpiryDate = dto.ExpiryDate,
         ShowQrCode = dto.ShowQrCode,
      CreatedByUserId = userId,
        Category = dto.Category,
          Tags = dto.Tags,
                Notes = dto.Notes
            };

    if (dto.ShowQrCode)
          {
         string qrCodeData = await _qrCodeService.GenerateQrCodeBase64Async(shortLink.ShortUrl);
  shortLink.QrCodeData = qrCodeData;
  }

 var savedLink = await _shortLinkRepository.CreateAsync(shortLink);

_logger.LogInformation($"Short link created with code: {shortCode} by user: {userId}");

            return _mapper.Map<ShortLinkDto>(savedLink);
        }
   catch (Exception ex)
        {
   _logger.LogError(ex, "Error creating short link");
         throw;
        }
    }

    public async Task<ShortLinkDto> GetShortLinkAsync(int id)
    {
        var shortLink = await _shortLinkRepository.GetByIdAsync(id);
        if (shortLink == null)
        {
            throw new NotFoundException("الرابط المختصر غير موجود");
    }

        return _mapper.Map<ShortLinkDto>(shortLink);
    }

    public async Task<ShortLinkDto> GetShortLinkByCodeAsync(string shortCode)
    {
    var shortLink = await _shortLinkRepository.GetByShortCodeAsync(shortCode);
 if (shortLink == null || !shortLink.IsActive)
        {
    throw new NotFoundException("الرابط المختصر غير موجود أو غير نشط");
      }

        if (shortLink.ExpiryDate.HasValue && shortLink.ExpiryDate <= DateTime.UtcNow)
        {
            throw new BadRequestException("الرابط المختصر انتهت صلاحيته");
        }

        return _mapper.Map<ShortLinkDto>(shortLink);
    }

    public async Task<ShortLinkDto> UpdateShortLinkAsync(int id, UpdateShortLinkDto dto, int userId)
    {
        var shortLink = await _shortLinkRepository.GetByIdAsync(id);
        if (shortLink == null)
        {
     throw new NotFoundException("الرابط المختصر غير موجود");
        }

        if (shortLink.CreatedByUserId != userId)
        {
            throw new ForbiddenException("ليس لديك صلاحية لتعديل هذا الرابط");
        }

        shortLink.Title = dto.Title ?? shortLink.Title;
        shortLink.Description = dto.Description ?? shortLink.Description;
        shortLink.ExpiryDate = dto.ExpiryDate;
        shortLink.IsActive = dto.IsActive;
        shortLink.ShowQrCode = dto.ShowQrCode;
        shortLink.Category = dto.Category ?? shortLink.Category;
  shortLink.Tags = dto.Tags ?? shortLink.Tags;
   shortLink.Notes = dto.Notes ?? shortLink.Notes;
        shortLink.UpdatedAt = DateTime.UtcNow;

        if (dto.ShowQrCode && string.IsNullOrEmpty(shortLink.QrCodeData))
      {
            string qrCodeData = await _qrCodeService.GenerateQrCodeBase64Async(shortLink.ShortUrl);
         shortLink.QrCodeData = qrCodeData;
  }

        await _shortLinkRepository.UpdateAsync(shortLink);

    _logger.LogInformation($"Short link {id} updated by user: {userId}");

 return _mapper.Map<ShortLinkDto>(shortLink);
    }

    public async Task DeleteShortLinkAsync(int id, int userId)
 {
     var shortLink = await _shortLinkRepository.GetByIdAsync(id);
    if (shortLink == null)
        {
            throw new NotFoundException("الرابط المختصر غير موجود");
  }

    if (shortLink.CreatedByUserId != userId)
 {
            throw new ForbiddenException("ليس لديك صلاحية لحذف هذا الرابط");
        }

        await _shortLinkRepository.DeleteAsync(id);

        _logger.LogInformation($"Short link {id} deleted by user: {userId}");
 }

    public async Task<IEnumerable<ShortLinkDto>> GetUserShortLinksAsync(int userId, int page = 1, int pageSize = 10)
    {
   var links = await _shortLinkRepository.GetByUserAsync(userId, page, pageSize);
        return _mapper.Map<IEnumerable<ShortLinkDto>>(links);
    }

    public async Task<IEnumerable<ShortLinkDto>> SearchShortLinksAsync(string searchTerm, int page = 1, int pageSize = 10)
    {
        var links = await _shortLinkRepository.SearchAsync(searchTerm, page, pageSize);
        return _mapper.Map<IEnumerable<ShortLinkDto>>(links);
    }

    public async Task<IEnumerable<ShortLinkDto>> GetShortLinksByCategoryAsync(string category, int page = 1, int pageSize = 10)
    {
    var links = await _shortLinkRepository.GetByCategoryAsync(category, page, pageSize);
     return _mapper.Map<IEnumerable<ShortLinkDto>>(links);
    }

    public async Task<IEnumerable<ShortLinkDto>> GetActiveShortLinksAsync(int page = 1, int pageSize = 10)
    {
    var links = await _shortLinkRepository.GetActiveAsync(page, pageSize);
        return _mapper.Map<IEnumerable<ShortLinkDto>>(links);
    }

public async Task RecordClickAsync(int shortLinkId, ShortLinkAnalyticsDto analyticsDto)
    {
  try
        {
            await _shortLinkRepository.IncrementClickCountAsync(shortLinkId);

   var analytics = new ShortLinkAnalytics
            {
                ShortLinkId = shortLinkId,
              ClickTime = DateTime.UtcNow,
 IpAddress = analyticsDto.IpAddress,
 Country = analyticsDto.Country,
   City = analyticsDto.City,
      Latitude = analyticsDto.Latitude,
     Longitude = analyticsDto.Longitude,
                DeviceType = analyticsDto.DeviceType,
OperatingSystem = analyticsDto.OperatingSystem,
     BrowserName = analyticsDto.BrowserName,
     BrowserVersion = analyticsDto.BrowserVersion,
           Language = analyticsDto.Language
            };

        await _analyticsRepository.CreateAsync(analytics);

          await _shortLinkRepository.UpdateLastAccessedAsync(
 shortLinkId,
       analyticsDto.Country,
         analyticsDto.City,
       analyticsDto.DeviceType);
        }
 catch (Exception ex)
        {
     _logger.LogError(ex, "Error recording click for short link: {ShortLinkId}", shortLinkId);
      }
    }

    public async Task<ShortLinkStatisticsDto> GetStatisticsAsync(int shortLinkId)
    {
        var shortLink = await _shortLinkRepository.GetByIdAsync(shortLinkId);
        if (shortLink == null)
{
            throw new NotFoundException("الرابط المختصر غير موجود");
   }

        var clicksByCountry = await _analyticsRepository.GetClicksByCountryAsync(shortLinkId);
      var clicksByDevice = await _analyticsRepository.GetClicksByDeviceAsync(shortLinkId);
        var clicksByBrowser = await _analyticsRepository.GetClicksByBrowserAsync(shortLinkId);

      return new ShortLinkStatisticsDto
    {
        ShortLinkId = shortLinkId,
        ShortCode = shortLink.ShortCode,
            Title = shortLink.Title ?? shortLink.ShortCode,
            TotalClicks = shortLink.ClickCount,
            UniqueClicks = await _analyticsRepository.GetUniqueClicksAsync(shortLinkId),
     FirstClickedAt = (await _analyticsRepository.GetByShortLinkAsync(shortLinkId, 1, 1)).FirstOrDefault()?.ClickTime,
     LastClickedAt = shortLink.LastAccessedAt,
    ClicksByCountry = clicksByCountry,
            ClicksByDevice = clicksByDevice,
      ClicksByBrowser = clicksByBrowser
 };
    }

    public async Task<string> GenerateUniqueShortCodeAsync(int length = 6)
 {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();

   string code;
        do
        {
     code = new string(Enumerable.Range(0, length)
    .Select(_ => chars[random.Next(chars.Length)])
         .ToArray());
     } while (await _shortLinkRepository.ShortCodeExistsAsync(code));

        return code;
 }

    public async Task<IEnumerable<ShortLinkDto>> GetTopShortLinksAsync(int count = 10)
    {
        var links = await _shortLinkRepository.GetTopByClicksAsync(count);
     return _mapper.Map<IEnumerable<ShortLinkDto>>(links);
 }

    public async Task<QrCodeResultDto> RegenerateQrCodeAsync(int shortLinkId, string? logoUrl = null)
    {
 var shortLink = await _shortLinkRepository.GetByIdAsync(shortLinkId);
        if (shortLink == null)
   {
   throw new NotFoundException("الرابط المختصر غير موجود");
        }

        string qrCodeData = await _qrCodeService.GenerateQrCodeBase64Async(
      shortLink.ShortUrl,
            300,
  logoUrl);

        shortLink.QrCodeData = qrCodeData;
        await _shortLinkRepository.UpdateAsync(shortLink);

        return new QrCodeResultDto
  {
            QrCodeData = qrCodeData,
    ShortUrl = shortLink.ShortUrl,
    MimeType = "image/png"
        };
    }

private bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
  (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
