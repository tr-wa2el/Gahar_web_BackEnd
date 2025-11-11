namespace Gahar_Backend.Services.Interfaces
{
    public interface IFileUploadService
{
        Task<string> UploadImageAsync(IFormFile file, string folder = "images");
     Task<string> UploadFileAsync(IFormFile file, string folder = "files");
   Task<bool> DeleteFileAsync(string filePath);
        Task<string> ConvertToWebPAsync(string imagePath);
    }
}
