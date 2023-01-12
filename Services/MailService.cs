using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using WebService.API.Repository;

namespace WebService.API.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _config;

        public MailService(IConfiguration configuration)
        {
            _config = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _config["SendMailAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@gts.com", "API using .Net Core 7.0");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
