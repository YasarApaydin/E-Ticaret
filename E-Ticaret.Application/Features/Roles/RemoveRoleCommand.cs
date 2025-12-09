using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Roles;
    public sealed record RemoveRoleCommand(Guid Id, string Name):IRequest<Result<string>>;


internal sealed class RemoveRoleCommandHandler(IUnitOfWork unitOfWork, IRoleRepository roleRepository) : IRequestHandler<RemoveRoleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        AppRole appRole = await roleRepository.GetByExpressionAsync(p => p.Id == request.Id, cancellationToken);

        if (appRole is null)
        {
            return Result<string>.Failure("Role Bulunamadı.");
        }

        roleRepository.Delete(appRole);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Role Başarıyla Veritabanından Silindi.");

    }
}



public sealed class RemoveRoleValidator : AbstractValidator<RemoveRoleCommand>
{

    public RemoveRoleValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("Role Idsi Boş Olamaz");
        RuleFor(p => p.Id).NotEmpty().WithMessage("Role Idsi Boş Olamaz");
    }
}
