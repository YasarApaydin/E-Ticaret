using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Application.Features.Order;
    public sealed record GetOrderDetailQuery(Guid OrderId):IRequest<GetOrderDetailResponse>;



public sealed class GetOrderDetailResponse : EntityDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;

    public List<OrderDetailItemResponse> Items { get; set; } = new();
}


public sealed class OrderDetailItemResponse
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
}


internal sealed class GetOrderDetailQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrderDetailQuery, GetOrderDetailResponse>
{
    public async Task<GetOrderDetailResponse> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.Where(x => x.Id == request.OrderId).Include(x => x.User).Include(x => x.Items).ThenInclude(x => x.Product).FirstOrDefaultAsync(cancellationToken);

        if(order is null)
        {
            return null!;
        }


        return new GetOrderDetailResponse
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            FirstName = order.User.FirstName,
            LastName = order.User.LastName,
            TotalPrice = order.TotalPrice,
            Status = order.Status.ToString(),
            Items = order.Items.Select(i => new OrderDetailItemResponse
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                UnitPrice = i.UnitPrice,
                Quantity =i.Quantity
            }).ToList()
        };


    }
}
