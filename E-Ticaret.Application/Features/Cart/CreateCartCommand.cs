using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Cart;

    public sealed record CreateCartCommand(Guid Id): IRequest<Result<string>>;


internal sealed class CreateCartCommandHandler(ICartRespository cartRespository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCartCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        bool existingCart = await cartRespository.AnyAsync(c => c.Id == request.Id, cancellationToken);

        if(existingCart)
        {
            return Result<string>.Failure("Kullanıcının Sepeti Zaten Mevcut.");
        }

       E_Ticaret.Domain.Entities.Cart newCart = request.Adapt<E_Ticaret.Domain.Entities.Cart>();

        cartRespository.Add(newCart);

        await unitOfWork.SaveChangesAsync(cancellationToken);


        return Result<string>.Succeed("Sepet başarıyla oluşturuldu.");
    }
}

public sealed class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
{
    public CreateCartCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id Boş Olamaz!");
    }
}
