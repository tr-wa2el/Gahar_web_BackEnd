namespace Gahar_Backend.Services.Interfaces
{
    public interface IEmailService
{
        Task SendEmailAsync(string to, string subject, string body);
   Task SendTemplateEmailAsync(string to, string templateName, object model);
    }
}
