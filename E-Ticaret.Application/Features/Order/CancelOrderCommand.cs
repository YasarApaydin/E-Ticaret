using E_Ticaret.Domain.Enums;
using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Order;
    public sealed record CancelOrderCommand(Guid OrderId):IRequest<Result<string>>;


internal sealed class CancelOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork) : IRequestHandler<CancelOrderCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByExpressionWithTrackingAsync(x => x.Id == request.OrderId, cancellationToken);
        if(order is null)
        {
            return Result<string>.Failure("Sipariş Bulunamadı.");
        }

        if(order.Status is OrderStatus.Shipped
           or OrderStatus.Delivered)
        {
            return Result<string>.Failure("Bu Sipariş İptal Edilemez. Kargo veya Teslim Aşamasında.");
        }

        if (order.Status == OrderStatus.Cancelled)
        {
            return Result<string>.Failure("Ürün Zaten İptal Edilmiş.");
        }

        order.Status = OrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Ürün Başarıyla İptal Edildi.");





    }
}
    

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().NotNull().WithMessage("SiparişId Boş Olamaz.");
    }
}