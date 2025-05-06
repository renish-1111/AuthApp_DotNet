using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text.RegularExpressions;



namespace IdentityApiExample.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _smtSubject;
        private readonly string _smtpBody;

        public EmailSender(IConfiguration Configuration)
        {
            _smtpHost = Configuration["EmailSetting:SmtpHost"];
            _smtpPort = int.Parse(Configuration["EmailSetting:SmtpPort"]);
            _smtpUsername = Configuration["EmailSetting:SmtpUsername"];
            _smtpPassword = Configuration["EmailSetting:SmtpPassword"];
            _smtSubject = Configuration["EmailSetting:SmtpSubject"];
            _smtpBody = Configuration["EmailSetting:SmtpBody"];
        }

        

        public async Task SendEmailAsync(string email,string subject,string htmlBody)
        {
            using var client = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUsername),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            try
            {
                await client.SendMailAsync(mailMessage);
                Console.WriteLine($"Email sent to {email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
