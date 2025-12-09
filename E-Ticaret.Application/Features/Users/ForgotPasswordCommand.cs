using E_Ticaret.Domain.Entities;
using E_Ticaret.Infrastructure.Abstractions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;
using static System.Net.WebRequestMethods;

namespace E_Ticaret.Application.Features.Users;
    public sealed record ForgotPasswordCommand(string Email):IRequest<Result<string>>;


internal sealed class ForgotPasswordCommandHandler(UserManager<AppUser> userManager, IEmailService emailService) : IRequestHandler<ForgotPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if(user is null)
        {
            return Result<string>.Failure("Kullanıcı Bulunamadı.");
        }
        var token = await userManager.GeneratePasswordResetTokenAsync(user);



        string resetUrl = $"https://localhost:7100/reset-password?email={user.Email}&token={Uri.EscapeDataString(token)}";

        string body = $$"""
<div style="font-family:Arial;padding:20px;">
  <h2 style="color:#2c3e50;">🔐 Şifre Yenileme Talebi</h2>
  <p style="font-size:16px;color:#444;">
    Yeni şifre oluşturmak için aşağıdaki butona tıklayın:
  </p>
  <a href='{resetUrl}' style='
    display:inline-block;
    margin-top:20px;
    background:#28a745;
    color:white;
    padding:12px 20px;
    text-decoration:none;
    border-radius:6px;
    font-weight:bold;
  '>Şifreyi Yenile</a>

  <p style="margin-top:30px;font-size:14px;color:#888;">
    Bu işlemi siz başlatmadıysanız lütfen bu mesajı dikkate almayın.
  </p>
</div>

""";
        await emailService.SendEmailAsync(
            user.Email!, "Şifre Yenileme Bağlantısı",body);



        return Result<string>.Succeed("Şifre Sıfırlama Bağlantısı Mailinize Başarıyla Gönderildi.");

    }
}


public sealed class ForgotPasswordCommandValidator: AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(p => p.Email).NotEmpty().WithMessage("Email Boş Olamaz");
        RuleFor(p => p.Email).NotNull().WithMessage("Email Boş Olamaz");
        RuleFor(p => p.Email).MinimumLength(3).WithMessage("Mail Adresi En Az 3 Karakter Olamlıdır.");
        RuleFor(p => p.Email).EmailAddress().WithMessage("Gecerli Bir Mail Adresi Girin.");
    }
}