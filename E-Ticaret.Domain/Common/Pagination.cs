namespace E_Ticaret.Domain.Common
{
    public class Pagination
    {

        public sealed record PagingParams(int Page = 1, int PageSize = 20)
        {
            public int Skip => (Page - 1) * PageSize;
        }

        public sealed record PagedResult<T>(IEnumerable<T> Items, int TotalCount, int Page, int PageSize);

    }
}
