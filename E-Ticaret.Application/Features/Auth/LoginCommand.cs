using E_Ticaret.Domain.Entities;
using E_Ticaret.Infrastructure.Abstractions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace E_Ticaret.Application.Features.Auth;
    public sealed record LoginCommand(string UserNameOrEmail,string Password) :IRequest<Result<LoginCommandResponse>>;


public sealed record LoginCommandResponse(string AccesToken, Guid UserId);

internal sealed class LoginCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {

        var user = await userManager.Users
            .Where(u => u.UserName == request.UserNameOrEmail || u.Email == request.UserNameOrEmail)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        { return Result<LoginCommandResponse>.Failure("Email veya Şifre Yanlış.");
        }
           
        if (!user.EmailConfirmed)
        {

            return Result<LoginCommandResponse>.Failure("Email Daha Doğrulanmamış.");
        }

     
        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                var lockoutEnd = await userManager.GetLockoutEndDateAsync(user);
                var remaining = lockoutEnd.HasValue ? lockoutEnd.Value.UtcDateTime - DateTime.UtcNow : TimeSpan.Zero;
                return Result<LoginCommandResponse>.Failure($"Hesabınız geçici olarak kilitlendi. Lütfen {remaining.Minutes} dakika {remaining.Seconds} saniye sonra tekrar deneyiniz.");
            }

            return Result<LoginCommandResponse>.Failure("Email veya Şifre Yanlış.");
        }

        await userManager.ResetAccessFailedCountAsync(user);

     
        string token = await jwtProvider.CreateTokenAsync(user,user.PasswordHash!,cancellationToken);

        return Result<LoginCommandResponse>.Succeed(new LoginCommandResponse(AccesToken: token, UserId: user.Id));


    }
}


public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {

        RuleFor(p => p.UserNameOrEmail).NotEmpty().WithMessage("Kullanıcı Adı Boş Olamaz.");
        RuleFor(p => p.UserNameOrEmail).NotNull().WithMessage("Kullanıcı Adı Boş Olamaz");
        RuleFor(p => p.UserNameOrEmail).MinimumLength(3).WithMessage("Kullanıcı En Az 3 Karakter Olmalıdır.");

        RuleFor(p => p.Password).NotNull().WithMessage("Şifre Boş Olamaz");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Şifre Boş Olamaz");
        RuleFor(p => p.Password).MinimumLength(8).WithMessage("Şifre En Az 8 Karakter Olmalıdır.");
        RuleFor(p => p.Password).Matches("[A-Z]").WithMessage("Şifre En Az 1 Büyük Harf İçermelidir.");
        RuleFor(p => p.Password).Matches("[a-z]").WithMessage("Şifre En Az 1 Küçük Harf İçermelidir.");
        RuleFor(p => p.Password).Matches("[^a-zA-z0-9]").WithMessage("Şifre En Az 1 Adet Özel Karakter İçermelidir.");
    }
}