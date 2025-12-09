using E_Ticaret.Infrastructure.Abstractions;
using System.Net;
using System.Net.Mail;

namespace E_Ticaret.Persistence.Services
{
    internal sealed class EmailService : IEmailService
    {
        private readonly string _smtpHost = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "apaydinyasar0@gmail.com";
        private readonly string _smtpPass = "pnin cpbf nyiu qtxd";
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var message = new MailMessage();
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                message.From = new MailAddress(_smtpUser, "Yöresel Lezzetler");

                using var client = new SmtpClient(_smtpHost, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUser, _smtpPass),
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

