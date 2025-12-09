using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Enums;
using E_Ticaret.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Application.Features.Product;
    public sealed record GetProductsQuery:IRequest<IQueryable<GetAllProductQueryResponse>>;
    


public sealed class GetAllProductQueryResponse : EntityDto
{

    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stoct { get; set; }
    public ProductUnit Unit { get; set; } = ProductUnit.Kg;

    public double UnitSize { get; set; } = 1.0;
    public string ImageUrl { get; set; } = string.Empty;


    public Guid CategoryId { get; set; }


    public string CategoryName { get; set; } = string.Empty;

}



internal sealed class GetProductQueryHandler(IProductRepository productRepository) : IRequestHandler<GetProductsQuery, IQueryable<GetAllProductQueryResponse>>
{
    public Task<IQueryable<GetAllProductQueryResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {


        var response = productRepository.Where(x => !x.IsDeleted).Include(x => x.Category).Select(y => new GetAllProductQueryResponse
        {

            Name = y.Name,
            Description = y.Description,
            Price = y.Price,
            Stoct = y.Stoct,
            Unit = y.Unit,
            UnitSize = y.UnitSize,
            ImageUrl = y.ImageUrl,
            CategoryId = y.CategoryId,
            CategoryName = y.Category.Name
            


        }).AsQueryable();


        return Task.FromResult(response);

    }
}