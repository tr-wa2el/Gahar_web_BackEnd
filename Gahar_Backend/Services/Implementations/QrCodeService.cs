using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Services.Implementations;

/// <summary>
/// Service implementation for QR Code generation with logo support
/// </summary>
public class QrCodeService : IQrCodeService
{
    private readonly ILogger<QrCodeService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public QrCodeService(ILogger<QrCodeService> logger, IHttpClientFactory httpClientFactory)
    {
     _logger = logger;
    _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Generate QR code as base64 string
    /// </summary>
    public async Task<string> GenerateQrCodeBase64Async(string content, int size = 300, string? logoUrl = null, int logoSize = 80)
    {
        try
        {
 var qrCodeBytes = await GenerateQrCodeBytesAsync(content, size, logoUrl, logoSize);
      return Convert.ToBase64String(qrCodeBytes);
     }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating QR code as Base64");
      throw new Exception("فشل توليد رمز QR");
        }
    }

    /// <summary>
    /// Generate QR code as byte array
    /// </summary>
    public async Task<byte[]> GenerateQrCodeBytesAsync(string content, int size = 300, string? logoUrl = null, int logoSize = 80)
    {
        try
    {
  using (var qrGenerator = new QRCodeGenerator())
     {
     var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.H);

      using (var qrCode = new QRCode(qrCodeData))
       {
          var qrImage = qrCode.GetGraphic(size);

      // إذا كان هناك شعار، نضيفه في المنتصف
      if (!string.IsNullOrEmpty(logoUrl))
          {
      qrImage = await AddLogoToQrCodeAsync(qrImage, logoUrl, logoSize);
       }

     // تحويل الصورة إلى byte array
    using (var ms = new MemoryStream())
         {
    qrImage.Save(ms, ImageFormat.Png);
       return ms.ToArray();
         }
 }
   }
        }
        catch (Exception ex)
  {
            _logger.LogError(ex, "Error generating QR code bytes");
            throw new Exception("فشل توليد رمز QR");
     }
    }

    /// <summary>
    /// Generate QR code as Bitmap
    /// </summary>
    public async Task<string> GenerateQrCodeAsync(string content, int size = 300, string? logoUrl = null, int logoSize = 80)
    {
  return await GenerateQrCodeBase64Async(content, size, logoUrl, logoSize);
    }

    /// <summary>
    /// Add logo to the center of QR code
    /// </summary>
    private async Task<Bitmap> AddLogoToQrCodeAsync(Bitmap qrCodeBitmap, string logoUrl, int logoSize)
    {
        try
        {
            // تحميل الشعار من الـ URL
      Bitmap logoBitmap = await DownloadImageAsync(logoUrl);

        // تغيير حجم الشعار
   Bitmap resizedLogo = ResizeImage(logoBitmap, logoSize, logoSize);

            // إنشاء صورة جديدة للنتيجة النهائية
            Bitmap result = (Bitmap)qrCodeBitmap.Clone();

            // حساب موقع الشعار في المنتصف
   int xPosition = (result.Width - resizedLogo.Width) / 2;
         int yPosition = (result.Height - resizedLogo.Height) / 2;

            // رسم خلفية بيضاء حول الشعار لتحسين الرؤية
         using (Graphics g = Graphics.FromImage(result))
   {
      // رسم مربع أبيض حول الشعار
          int padding = 5;
        using (Brush whiteBrush = new SolidBrush(Color.White))
 {
              g.FillRectangle(whiteBrush,
    xPosition - padding,
        yPosition - padding,
           resizedLogo.Width + (padding * 2),
      resizedLogo.Height + (padding * 2));
       }

          // رسم الشعار
 g.DrawImage(resizedLogo, xPosition, yPosition);
}

          logoBitmap.Dispose();
    resizedLogo.Dispose();

    return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding logo to QR code");
   return qrCodeBitmap; // إرجاع الـ QR Code بدون شعار في حالة الفشل
        }
    }

    /// <summary>
    /// Download image from URL
    /// </summary>
    private async Task<Bitmap> DownloadImageAsync(string imageUrl)
    {
        try
        {
     using (var client = _httpClientFactory.CreateClient())
            {
       client.Timeout = TimeSpan.FromSeconds(10);
     var response = await client.GetAsync(imageUrl);
      response.EnsureSuccessStatusCode();

      using (var stream = await response.Content.ReadAsStreamAsync())
       {
              return new Bitmap(stream);
   }
  }
        }
        catch (Exception ex)
        {
   _logger.LogError(ex, "Error downloading logo image from {LogoUrl}", imageUrl);
            throw new Exception("فشل تحميل الشعار من الـ URL");
        }
    }

    /// <summary>
    /// Resize image to specified dimensions
    /// </summary>
    private Bitmap ResizeImage(Image image, int width, int height)
    {
        var resizedBitmap = new Bitmap(width, height);
   using (var g = Graphics.FromImage(resizedBitmap))
      {
      g.DrawImage(image, 0, 0, width, height);
        }
        return resizedBitmap;
    }
}
