using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Enums;
using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Order;
    public sealed record CreateOrderCommand(Guid UserId): IRequest<Result<Guid>>;



internal sealed class CreateOrderCommandHandler(ICartRespository cartRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetCartWithItemsAsync(request.UserId,cancellationToken);
    if(cart is null || !cart.Items.Any())
        {
            return Result<Guid>.Failure("Sepet Boş");
        }


        var order = new E_Ticaret.Domain.Entities.Order
        {
            UserId = request.UserId,
            Status = OrderStatus.Pending,
            TotalPrice = cart.Items.Sum(x=> x.UnitPrice* x.Quantity)
        };



        foreach(var item in cart.Items) {

            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            });
        }


        await orderRepository.AddAsync(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Succeed(order.Id);

    
    }
}



public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().NotNull().WithMessage("UserId Boş Olamaz.");
    }
}
