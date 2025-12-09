using static E_Ticaret.Domain.Common.Pagination;

namespace E_Ticaret.Domain.Common
{
    public sealed record UserFilter(
        string? Search,
        bool? IsDeleted,
        bool? IsLocked,
        DateTime? CreatedFrom,
        DateTime? CreatedTo,
        PagingParams Paging
    );
}
