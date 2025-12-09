using E_Ticaret.Infrastructure.Abstractions;
using E_Ticaret.Infrastructure.Options;
using System.Net;
using System.Net.Mail;

namespace E_Ticaret.Persistence.Services
{
    internal sealed class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        public EmailService(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var message = new MailMessage();
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                message.From = new MailAddress(emailSettings.Username, emailSettings.SenderName);

                using var client = new SmtpClient(emailSettings.Host, emailSettings.Port)
                {
                    Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password),
                    EnableSsl = true
                };

                await client.SendMailAsync(message);
            }
            catch (SmtpException ex)
            {
               
                throw new Exception($"Email gönderilemedi: {ex.Message}", ex);
            }
        }
    }
}

