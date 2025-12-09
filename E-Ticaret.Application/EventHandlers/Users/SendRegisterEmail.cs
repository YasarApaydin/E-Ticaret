using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Events.Users;
using E_Ticaret.Domain.Interfaces.Repositories;
using E_Ticaret.Infrastructure.Abstractions;
using GenericRepository;
using MediatR;

namespace E_Ticaret.Application.EventHandlers.Users
{
    public sealed class SendRegisterEmail : INotificationHandler<UserDomainEvent>
    {
        private readonly IEmailRepository emailRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;
        private readonly IEncryptionService encryptionService;

        public SendRegisterEmail(IUnitOfWork unitOfWork, IEmailRepository emailRepository, IEmailService emailService, IEncryptionService encryptionService)
        {
            this.unitOfWork = unitOfWork;
            this.emailRepository = emailRepository;
            this.emailService = emailService;
            this.encryptionService = encryptionService;
        }

        public  async Task Handle(UserDomainEvent notification, CancellationToken cancellationToken)
        {

            var verification = new EmailVerification
            {
                UserId = notification.AppUser.Id,
                Code = notification.VerificationCode,
                ExpirationTime = DateTime.UtcNow.AddMinutes(3)
            };
            await emailRepository.AddAsync(verification, cancellationToken);
            await unitOfWork.SaveChangesAsync();



            var subject = "Yöresel Lezzetler - E-posta Doğrulama Kodu";
            string firstName = encryptionService.DecryptString(notification.AppUser.FirstName);
            string lastName = encryptionService.DecryptString(notification.AppUser.LastName);
            //string email = encryptionService.DecryptString(notification.AppUser.Email);


            var body = $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>E-Posta Doğrulama</title>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f9f9f9;
            color: #333;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background-color: #ffffff;
            border-radius: 12px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            overflow: hidden;
        }}
        .header {{
            background-color: #8b0000;
            color: #fff;
            text-align: center;
            padding: 20px;
        }}
        .header h1 {{
            margin: 0;
            font-size: 24px;
        }}
        .content {{
            padding: 30px 20px;
            text-align: center;
        }}
        .content h2 {{
            font-size: 28px;
            color: #8b0000;
            margin: 20px 0;
            letter-spacing: 4px;
        }}
        .content p {{
            font-size: 16px;
            margin: 10px 0;
        }}
        .footer {{
            background-color: #f1f1f1;
            color: #666;
            text-align: center;
            padding: 15px;
            font-size: 13px;
        }}
        .footer a {{
            color: #8b0000;
            text-decoration: none;
            font-weight: 600;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Yöresel Lezzetler</h1>
        </div>
        <div class='content'>
            <p>Merhaba <strong>{firstName} {lastName}</strong>,</p>
            <p>Yöresel Lezzetler ailesine hoş geldin! 🍽️</p>
            <p>Kayıt işlemini tamamlamak için doğrulama kodun aşağıdadır:</p>
            <h2>{notification.VerificationCode}</h2>
            <p>Bu kod yalnızca <strong>10 dakika</strong> boyunca geçerlidir.</p>
            <p>Eğer bu işlemi sen başlatmadıysan, bu e-postayı görmezden gelebilirsin.</p>
        </div>
        <div class='footer'>
            <p>&copy; {DateTime.Now.Year} Yöresel Lezzetler. Tüm hakları saklıdır.</p>
            <p><a href='https://www.yoresellezzetler.com'>www.yoresellezzetler.com</a></p>
        </div>
    </div>
</body>
</html>";


            await emailService.SendEmailAsync(notification.AppUser.Email!, subject, body);

          
        }
    }
}
