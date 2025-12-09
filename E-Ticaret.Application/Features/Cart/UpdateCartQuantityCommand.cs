using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Cart;
    public sealed record UpdateCartQuantityCommand(
        Guid UserId,
        Guid ProductId,
        decimal NewQuantity
        ) :IRequest<Result<string>>;

internal sealed class UpdateCartQuantityCommandHandler(ICartRespository cartRespository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCartQuantityCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateCartQuantityCommand request, CancellationToken cancellationToken)
    {
        var cart = await cartRespository.GetCartWithItemsAsync(request.UserId, cancellationToken);

        if (cart is null)
        {
            return Result<string>.Failure("Kullanıcıya ait sepet bulunamadı.");
        }
        var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

        if (cartItem is null)
        {
            return Result<string>.Failure("Bu Ürün Sepetde Bulunmamamktadır.");


        }


        if(request.NewQuantity <= 0)
        {
            cart.Items.Remove(cartItem);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Ürün Sepetden Kaldırıldı.");
        }

        cartItem.Quantity = request.NewQuantity;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Sepet Güncellendi.");






    }




}


public sealed class UpdateCartQuantityCommandValidator : AbstractValidator<UpdateCartQuantityCommand>
{
    public UpdateCartQuantityCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty().NotNull().WithMessage("Kullanıcıya ait Sepet Bulunamadı.");

        RuleFor(c => c.ProductId).NotEmpty().NotNull().WithMessage("Sepetde Bulunacak Ürün Belirtilmedi.");
        RuleFor(c => c.NewQuantity).GreaterThanOrEqualTo(0).WithMessage("Ürün Müktari Sıfırdan Kücük Olamaz.");
        
    }
}

