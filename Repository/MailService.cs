using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System.Text;
using WebService.API.Services;

namespace WebService.API.Repository
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
            var keyMail = System.Convert.FromBase64String(_config["SendMailAPIKey"]);
            string apiKey = System.Text.Encoding.UTF8.GetString(keyMail);
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@gts.com", "API using .Net Core 7.0");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
            
        }
    }
}
