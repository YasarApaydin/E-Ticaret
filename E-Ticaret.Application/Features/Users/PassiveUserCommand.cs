using E_Ticaret.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace E_Ticaret.Application.Features.Users;
    public sealed record PassiveUserCommand(Guid UserId):IRequest<Result<string>>;



internal sealed class PassiveUserCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<PassiveUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PassiveUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
    
    
    if(user is null)
        {
            return Result<string>.Failure("Kullanıcı Bulunamadı.");
        }

        user.LockoutEnd = DateTimeOffset.MaxValue;
        await userManager.UpdateAsync(user);
        return Result<string>.Succeed("Hesap Pasif Edilmiştir. Cıkışa Yönlendiriliyorsunuz.");


    
    }
}


public sealed class PassiveUserCommandValidator : AbstractValidator<PassiveUserCommand>
{
    public PassiveUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().NotNull().WithMessage("UserId Boş Olamaz!");
    }
}