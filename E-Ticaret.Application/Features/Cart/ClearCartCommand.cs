using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Cart;
    public sealed record ClearCartCommand(Guid Id):IRequest<Result<string>>;


internal sealed class ClearCartCommandHandler(ICartRespository cartRespository,IUnitOfWork unitOfWork) : IRequestHandler<ClearCartCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await cartRespository.GetCartWithItemsAsync(request.Id, cancellationToken);

        if(cart is null)
        {
            return Result<string>.Failure("Kullanıcı Sepeti Bulunamadı.");
        }

        if (!cart.Items.Any())
        {
            return Result<string>.Failure("Sepet Zaten Boş");
        }

        cart.Items.Clear();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Sepet Başarıyla Temizlendi.");



    }
}


public sealed class ClearCartCommandValidator : AbstractValidator<ClearCartCommand>
{
    public ClearCartCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Kullanıcı Sepeti Bulunamadı!");
    }
}
