using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Cart;

    public sealed record AddToCartCommand(Guid UserId,
    Guid ProductId,
    decimal Quantity) :IRequest<Result<string>>;


internal sealed class AddToCartCommandHandler(
        ICartRespository cartRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<AddToCartCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByExpressionAsync(x => x.Id == request.ProductId, cancellationToken);
        if (product is null)
        {
            return Result<string>.Failure("Ürün Bulunamadı.");
        }

        if (product.Stoct <= 0 || request.Quantity > product.Stoct)
        {
            return Result<string>.Failure("Ürün stokta yok veya istenen miktar mevcut değil.");
        }
        var cart = await cartRepository.GetCartWithItemsAsync(request.UserId, cancellationToken);

        if(cart is null)
        {
             cart = new E_Ticaret.Domain.Entities.Cart
            {
                 UserId = request.UserId
            };


           await cartRepository.AddAsync(cart,cancellationToken);
        }


        var cartItem = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId);

        if(cartItem is null)
        {
            cartItem = new Domain.Entities.CartItem
            {
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UnitPrice = product.Price
            };

            cart.Items.Add(cartItem);
        } else
        {
            cartItem.Quantity += request.Quantity;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed($"{product.Name} sepete eklendi.");

    }
}


public sealed class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
{
    public AddToCartCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().NotNull().WithMessage("Kullanıcı ID boş olamaz.");

        RuleFor(x => x.ProductId)
            .NotEmpty().NotNull().WithMessage("Ürün ID boş olamaz.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Adet 0'dan büyük olmalıdır.");
    }
}
