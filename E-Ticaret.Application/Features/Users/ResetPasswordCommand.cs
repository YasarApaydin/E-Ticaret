using E_Ticaret.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace E_Ticaret.Application.Features.Users;
    public sealed record ResetPasswordCommand(string Email,string Token,string NewPassword,
    string ConfirmNewPassword) : IRequest<Result<string>>;



internal sealed class ResetPasswordCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<ResetPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if(user is null)
        {
            return Result<string>.Failure("Kullanıcı Bulunamadı.");
        }





        var result = await userManager.ResetPasswordAsync(user,request.Token,request.NewPassword);

        if (!result.Succeeded)
        {
           return Result<string>.Failure("Şifre sıfırlanamadı. Token geçersiz olabilir.");
        }

        return Result<string>.Succeed("Şifreniz başarıyla güncellendi.");
    }
}


public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {

        RuleFor(x => x.Token)
         .NotEmpty().NotNull().WithMessage("Token eksik veya geçersiz.");


        RuleFor(p => p.NewPassword).NotNull().WithMessage("Şifre Boş Olamaz");
        RuleFor(p => p.NewPassword).NotEmpty().WithMessage("Şifre Boş Olamaz");
        RuleFor(p => p.NewPassword).MinimumLength(8).WithMessage("Şifre En Az 8 Karakter Olmalıdır.");
        RuleFor(p => p.NewPassword).Matches("[A-Z]").WithMessage("Şifre En Az 1 Büyük Harf İçermelidir.");
        RuleFor(p => p.NewPassword).Matches("[a-z]").WithMessage("Şifre En Az 1 Küçük Harf İçermelidir.");
        RuleFor(p => p.NewPassword).Matches("[^a-zA-z0-9]").WithMessage("Şifre En Az 1 Adet Özel Karakter İçermelidir.");
      
        RuleFor(p => p.Email).NotNull().WithMessage("Email Boş Olamaz");
        RuleFor(p => p.Email).NotEmpty().WithMessage("Email Boş Olamaz");


        RuleFor(x => x)
          .Must(x => x.NewPassword == x.ConfirmNewPassword)
          .WithMessage("Yeni şifre ile şifre tekrarı uyuşmuyor!");

        RuleFor(p => p.ConfirmNewPassword).NotNull().WithMessage("Şifre Tekrarı Boş Olamaz");
        RuleFor(p => p.ConfirmNewPassword).NotEmpty().WithMessage("Şifre Tekrarı Boş Olamaz");
        RuleFor(p => p.ConfirmNewPassword).MinimumLength(8).WithMessage("Şifre En Az 8 Karakter Olmalıdır.");
        RuleFor(p => p.ConfirmNewPassword).Matches("[A-Z]").WithMessage("Şifre En Az 1 Büyük Harf İçermelidir.");
        RuleFor(p => p.ConfirmNewPassword).Matches("[a-z]").WithMessage("Şifre En Az 1 Küçük Harf İçermelidir.");
        RuleFor(p => p.ConfirmNewPassword).Matches("[^a-zA-z0-9]").WithMessage("Şifre En Az 1 Adet Özel Karakter İçermelidir.");
    }
}