using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Cart;
    public sealed record RemoveCartItemCommand(Guid UserId, Guid ProductId) :IRequest<Result<string>>;

internal sealed class RemoveCartItemCommandHandler(ICartRespository cartRespository, IUnitOfWork unitOfWork) : IRequestHandler<RemoveCartItemCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await cartRespository.GetCartWithItemsAsync(request.UserId, cancellationToken);

        if (cart is null)
        {
            return Result<string>.Failure("Kullanıcının sepeti bulunamadı.");
        }


        var cartItem = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
        if(cartItem is null)
        {
            return Result<string>.Failure("Bu Ürün Sepetde Bulunmuyor.");
        }

        cart.Items.Remove(cartItem);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Ürün Sepetden Başarıyla Kaldırıldı.");

    }
}
public sealed class RemoveCartItemCommandValidator : AbstractValidator<RemoveCartItemCommand>
{
    public RemoveCartItemCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().NotNull().WithMessage("UserId Boş Olamaz!");
        RuleFor(x => x.ProductId).NotEmpty().NotNull().WithMessage("ProductId Boş Olamaz!");
    }
}