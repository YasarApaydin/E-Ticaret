using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace E_Ticaret.Application.Features.Cart;
    public sealed record CartSummaryQuery(Guid UserId):IRequest<Result<CartSummaryResponse>>;



public sealed class CartSummaryResponse
{
    public Guid UserId { get; set; }
    public int TotalItemCount { get; set; }
    public int ProductCount { get; set; }
    public decimal TotalPrice { get; set; }
}

internal sealed class CartSummaryQueryHandler(ICartRespository cartRespository) : IRequestHandler<CartSummaryQuery, Result<CartSummaryResponse>>
{
    public async Task<Result<CartSummaryResponse>> Handle(CartSummaryQuery request, CancellationToken cancellationToken)
    {
        var cartSummary = await cartRespository.Where(c => c.UserId == request.UserId).Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefaultAsync(cancellationToken);
    
    if(cartSummary is null)
        {
            return Result<CartSummaryResponse>.Failure("Sepet bulunamadı.");
        }



        var response = new CartSummaryResponse
        {
            UserId = request.UserId,
            ProductCount = cartSummary.Items.Count,
            TotalItemCount = cartSummary.Items.Sum(i => (int)i.Quantity),
            TotalPrice = cartSummary.Items.Sum(i => i.Quantity * i.UnitPrice)
        };
        return Result<CartSummaryResponse>.Succeed(response);

    }
}


public sealed class CartSummaryQueryValidator: AbstractValidator<CartSummaryQuery>
{
    public CartSummaryQueryValidator()
    {
        RuleFor(c => c.UserId).NotEmpty().NotNull().WithMessage("Kullanıcı Id Boş Olamaz.");
    }
}