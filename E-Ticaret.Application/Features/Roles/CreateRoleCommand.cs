using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Roles;
    public sealed record CreateRoleCommand(
        string Name
        ):IRequest<Result<string>>;


internal sealed class CreateRoleCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateRoleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {

        var checkRoleIsExist = await roleRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

        if (checkRoleIsExist)
        {
            return Result<string>.Failure("Bu Role Kayıtlıdır.");
        }


        AppRole appRole = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };
        await roleRepository.AddAsync(appRole, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Role Başarılı Bir Şekilde Oluşturuldu.");
    }
}
public sealed class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
{

    public CreateRoleValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Role Adı Boş Olamaz");
        RuleFor(p => p.Name).NotNull().WithMessage("Role Adı Boş Olamaz");
        RuleFor(p => p.Name).MinimumLength(3).WithMessage("Role Adı 3 Karakterden Az Olamaz.");
    }
}