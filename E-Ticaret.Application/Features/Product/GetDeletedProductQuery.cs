using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Enums;
using E_Ticaret.Domain.Interfaces.Repositories;
using MediatR;

namespace E_Ticaret.Application.Features.Product;
    public sealed record GetDeletedProductQuery:IRequest<IQueryable<GetDeletedProductResponse>>;



public sealed class GetDeletedProductResponse : EntityDto
{


    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }


    public ProductUnit Unit { get; set; } = ProductUnit.Kg;

    public double UnitSize { get; set; } = 1.0;
}

internal sealed class GetDeletedProductQueryHandler(IProductRepository productRepository) : IRequestHandler<GetDeletedProductQuery, IQueryable<GetDeletedProductResponse>>
{
    public Task<IQueryable<GetDeletedProductResponse>> Handle(GetDeletedProductQuery request, CancellationToken cancellationToken)
    {

        var response = productRepository.Where(p => p.IsDeleted).Select(x => new GetDeletedProductResponse
        {
            Name = x.Name,
            Id = x.Id,
            Description = x.Description,
            Price = x.Price,
            Unit = x.Unit,
            UnitSize= x.UnitSize,
            CreatedAt = x.CreatedAt,
            DeletedAt = x.DeletedAt,
            UpdatedAt = x.UpdatedAt,
            CreatorUser = x.CreatorUser,
            IsDeleted = x.IsDeleted
        }).AsQueryable();


        return Task.FromResult(response);
    }
}

