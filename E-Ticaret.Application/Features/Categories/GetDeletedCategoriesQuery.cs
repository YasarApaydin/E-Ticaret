using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Interfaces.Repositories;
using MediatR;

namespace E_Ticaret.Application.Features.Categories;
    public sealed record GetDeletedCategoriesQuery:IRequest<IQueryable<GetDeletedCategoryResponse>>;



public sealed class GetDeletedCategoryResponse:EntityDto
{
    public string Name { get; set; } = default!;
}

internal sealed class GetDeletedCategoriesQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetDeletedCategoriesQuery, IQueryable<GetDeletedCategoryResponse>>
{
    public Task<IQueryable<GetDeletedCategoryResponse>> Handle(GetDeletedCategoriesQuery request, CancellationToken cancellationToken)
    {
        var response = categoryRepository.Where(c => c.IsDeleted).Select(c => new GetDeletedCategoryResponse
        {
            Name = c.Name,
            Id = c.Id,
            CreatedAt = c.CreatedAt,
            DeletedAt= c.DeletedAt,
            UpdatedAt=c.UpdatedAt,
            CreatorUser = c.CreatorUser,
            IsDeleted = c.IsDeleted

        }).AsQueryable();


        return Task.FromResult(response);


    }
}