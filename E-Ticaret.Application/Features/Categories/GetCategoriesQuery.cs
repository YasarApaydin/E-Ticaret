using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Application.Features.Categories;
    public sealed record GetCategoriesQuery: IRequest<IQueryable<GetAllCategoriesQueryResponse>>;



public sealed class GetAllCategoriesQueryResponse : EntityDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public ICollection<ProductDto>? Products { get; set; }
}

internal sealed class GetCategoriesQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoriesQuery, IQueryable<GetAllCategoriesQueryResponse>>
{
    public Task<IQueryable<GetAllCategoriesQueryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var response = categoryRepository.Where(c=> !c.IsDeleted).Include(c => c.Products).Select(s => new GetAllCategoriesQueryResponse
        {
            Name = s.Name,
            Description = s.Description,
            Products = s.Products!.Select(p => new ProductDto
            {
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl
            }).ToList()
        }).AsQueryable();


        return Task.FromResult(response);
    }
}
