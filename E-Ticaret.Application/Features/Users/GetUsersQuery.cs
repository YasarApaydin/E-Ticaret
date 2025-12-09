using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Interfaces.Repositories;
using MediatR;
using TS.Result;
using static E_Ticaret.Domain.Common.Pagination;

namespace E_Ticaret.Application.Features.Users;
    public sealed record GetUsersQuery(

         string? Search,
    bool? IsDeleted,
    bool? IsLocked,
    DateTime? CreatedFrom,
    DateTime? CreatedTo,
    PagingParams Paging
        ) :IRequest<Result<PagedResult<UserListDto>>>;





internal sealed class GetUsersQueryHandler(IUserReadRepository userReadRepository) : IRequestHandler<GetUsersQuery, Result<PagedResult<UserListDto>>>
{

   
    public async Task<Result<PagedResult<UserListDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var filter = new UserFilter(
         request.Search,
         request.IsDeleted,
         request.IsLocked,
         request.CreatedFrom,
         request.CreatedTo,
         request.Paging
     );



        var result = await userReadRepository.GetUsersAsync(filter);

        return Result<PagedResult<UserListDto>>.Succeed(result);
    }
}