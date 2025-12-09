using E_Ticaret.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace E_Ticaret.Application.Features.Users;
    public sealed record ActiveUserCommand(Guid UserId):IRequest<Result<string>>;

internal sealed class ActiveUserCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<ActiveUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ActiveUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if(user is null)
        {
            return Result<string>.Failure("Kullanıcı Bulunamadı.");
        }
        user.LockoutEnd = null;
        user.IsDeleted = false;
        user.DeletedAt = null;
        await userManager.UpdateAsync(user);
        return Result<string>.Succeed("Kullanıcı Başarılı Şekilde Aktif Edildi.");
    
    
    }
}


public sealed class ActiveUserCommandValidator : AbstractValidator<ActiveUserCommand>
{
    public ActiveUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotNull().WithMessage("Kullanıcı Id Boş Olamaz.");
    }
}
 