using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace E_Ticaret.Application.Features.Auth;
    public sealed record VerifyEmailCommand(
        string Email, 
        string Code

        ) :IRequest<Result<string>>;

internal sealed class VerifyEmailCommandHandler(IUnitOfWork unitOfWork, IEmailRepository emailRepository, UserManager<AppUser> userManager) : IRequestHandler<VerifyEmailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var verificationCode = await emailRepository
               .Where(x => x.User.Email == request.Email && x.Code == request.Code)
               .OrderByDescending(c => c.CreatedAt)
               .FirstOrDefaultAsync(cancellationToken);
        if (verificationCode is null)
        {
            return Result<string>.Failure("Doğrulama Kodu Bulunamadı.");
        }

        if (verificationCode.ExpirationTime < DateTime.UtcNow)
        {
            emailRepository.Delete(verificationCode);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Failure("Doğrulama kodunun süresi dolmuştur.");


        }
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı.");
        }
        user.EmailConfirmed = true;
        await userManager.UpdateAsync(user);
        emailRepository.Delete(verificationCode);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Email Onaylandı.");

    }
}
