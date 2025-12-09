using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace E_Ticaret.Application.Features.Order;
    public sealed record GetOrdersByUserQuery(Guid UserId):IRequest<Result<List<GetUserOrderResponse>>>;



public sealed class GetUserOrderResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
}


internal sealed class GetOrdersByUserQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrdersByUserQuery, Result<List<GetUserOrderResponse>>>
{
    public async Task<Result<List<GetUserOrderResponse>>> Handle(GetOrdersByUserQuery request, CancellationToken cancellationToken)
    {
        var query = await orderRepository.Where(x => x.UserId == request.UserId && !x.IsDeleted).OrderByDescending(x => x.CreatedAt).Select(x => new GetUserOrderResponse
        {
            Id = x.Id,
          
            Status = x.Status.ToString(),
            CreatedAt = x.CreatedAt.UtcDateTime,
           TotalPrice = x.TotalPrice
        }).ToListAsync(cancellationToken);

        return Result<List<GetUserOrderResponse>>.Succeed(query);

    }
}
