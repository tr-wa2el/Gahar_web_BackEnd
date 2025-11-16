using Gahar_Backend.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Gahar_Backend.Services.Implementations
{
    public class EmailService : IEmailService
    {
    private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
   {
  _configuration = configuration;
   }

     public async Task SendEmailAsync(string to, string subject, string body)
  {
      // TODO: Implement email sending logic (SMTP, SendGrid, etc.)
     await Task.CompletedTask;
   }

 public async Task SendTemplateEmailAsync(string to, string templateName, object model)
   {
   // TODO: Implement template-based email sending
    await Task.CompletedTask;
 }
    }
}
