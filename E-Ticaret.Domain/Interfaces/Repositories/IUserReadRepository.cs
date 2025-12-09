using E_Ticaret.Domain.Common;
using static E_Ticaret.Domain.Common.Pagination;

namespace E_Ticaret.Domain.Interfaces.Repositories
{
    public interface IUserReadRepository
    {
        Task<PagedResult<UserListDto>> GetUsersAsync(UserFilter filter);
    }
}
