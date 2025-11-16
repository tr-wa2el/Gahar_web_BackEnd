using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;

namespace Gahar_Backend.Services.Interfaces;

/// <summary>
/// Service interface for QR Code generation
/// </summary>
public interface IQrCodeService
{
    /// <summary>
    /// Generate QR code with optional logo
    /// </summary>
    Task<string> GenerateQrCodeAsync(string content, int size = 300, string? logoUrl = null, int logoSize = 80);

    /// <summary>
    /// Generate QR code as base64 string
    /// </summary>
    Task<string> GenerateQrCodeBase64Async(string content, int size = 300, string? logoUrl = null, int logoSize = 80);

    /// <summary>
    /// Generate QR code as byte array
 /// </summary>
    Task<byte[]> GenerateQrCodeBytesAsync(string content, int size = 300, string? logoUrl = null, int logoSize = 80);
}
