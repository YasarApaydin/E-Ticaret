using E_Ticaret.Domain.Enums;
using E_Ticaret.Domain.Interfaces.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Order;
    public sealed record UpdateOrderStatusCommand(
      Guid OrderId, 
      OrderStatus Status
        ) :IRequest<Result<string>>;

internal sealed class UpdateOrderStatusCommandHandler(IOrderRepository orderRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateOrderStatusCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByExpressionWithTrackingAsync(x => x.Id == request.OrderId, cancellationToken);

        if(order is null)
        {
            return Result<string>.Failure("Order bulunamadı.");
        }

        order.Status = request.Status;
        order.UpdatedAt = DateTime.UtcNow;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed($"Sipariş durumu güncellendi: {order.Status}");
    }
}