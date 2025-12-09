using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace E_Ticaret.Application.Features.Cart;
    public sealed record GetCartQueryCommand(Guid UserId): IRequest<Result<GetCartQueryResponse>>;



public sealed class  GetCartQueryResponse  :EntityDto
{

    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<CartItemResponse> Items { get; set; } = new();
}


public sealed class CartItemResponse
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
    public string ImageUrl { get; set; } = string.Empty;
}

internal sealed class GetCartQueryCommandHandler(ICartRespository cartRespository) : IRequestHandler<GetCartQueryCommand, Result<GetCartQueryResponse>>
{
    public async Task<Result<GetCartQueryResponse>> Handle(GetCartQueryCommand request, CancellationToken cancellationToken)
    {
        var cartQuery = cartRespository.Where(c => c.UserId == request.UserId).Include(x => x.Items).ThenInclude(p => p.Product).Select(cart => new GetCartQueryResponse
        {

            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.Items.Select(item => new CartItemResponse
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                ImageUrl = item.Product.ImageUrl,
            }).ToList()

        });

        var cart = await cartQuery.FirstOrDefaultAsync(cancellationToken);
        if(cart is null)
        {
            return Result<GetCartQueryResponse>.Failure("Sepet bulunamadı.");
        }
        cart.TotalPrice = cart.Items.Sum(i => i.LineTotal);

        return new Result<GetCartQueryResponse>(cart);
    }
}

public sealed class GetCartQueryCommandValidator : AbstractValidator<GetCartQueryCommand>
{
    public GetCartQueryCommandValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("UserId Boş Olamaz!");
    }
}