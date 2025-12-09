using E_Ticaret.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace E_Ticaret.Application.Features.Users;
    public  sealed record SoftDeleteUserCommand(Guid UserId):IRequest<Result<string>>;


internal sealed class SoftDeleteUserCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<SoftDeleteUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if(user is null)
        {
            return Result<string>.Failure("Kullanıcı Bulunamadı.");
        }

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        user.LockoutEnd = DateTimeOffset.MaxValue;

        await userManager.UpdateAsync(user);
        return Result<string>.Succeed("Hesabınız Başarıyla Silindi. Cıkışa Yönlendiriliyorsunuz.");


    }
}


public sealed class SoftDeleteUserCommandValidator : AbstractValidator<SoftDeleteUserCommand>
{
    public SoftDeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().NotNull().WithMessage("UserId Boş Olamaz!");
    }
}