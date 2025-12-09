using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.UserRoles;
    public sealed record SetUserRoleCommand(Guid UserId,Guid RoleId):IRequest<Result<string>>;

internal sealed class SetUserRoleCommandHandler(IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork) : IRequestHandler<SetUserRoleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SetUserRoleCommand request, CancellationToken cancellationToken)
    {
        var checkIsRoleSetExists = await userRoleRepository.AnyAsync(p => p.UserId == request.UserId && p.RoleId == request.RoleId, cancellationToken);
        if (checkIsRoleSetExists)
        {
            return Result<string>.Failure("Kullanıcı Bu Role Sahip.");
        }
        AppUserRole appUserRole = new()
        {
            UserId = request.UserId,
            RoleId = request.RoleId

        };
        await userRoleRepository.AddAsync(appUserRole, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Kullanıcıya Role Atandı.");
    }
}
