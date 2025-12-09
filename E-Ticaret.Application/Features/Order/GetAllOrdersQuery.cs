using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Application.Features.Order;
    public sealed record GetAllOrdersQuery:IRequest<IQueryable<GetAllOrdersQueryResponse>>;



public sealed class GetAllOrdersQueryResponse : EntityDto
{
    public Guid UserId { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;

}

internal sealed class GetAllOrdersQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetAllOrdersQuery, IQueryable<GetAllOrdersQueryResponse>>
{
    public Task<IQueryable<GetAllOrdersQueryResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = orderRepository.GetAll().Include(x => x.User).Select(x => new GetAllOrdersQueryResponse
        {
            UserId = x.UserId,
            CreatedAt = x.CreatedAt,
            LastName = x.User.LastName,
            FirstName = x.User.FirstName,
            TotalPrice = x.TotalPrice,
            Status = x.Status.ToString(),
            IsDeleted = x.IsDeleted
        }).AsQueryable();

        return Task.FromResult(query);


    }
}