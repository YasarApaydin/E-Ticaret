using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Interfaces.Repositories;
using E_Ticaret.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static E_Ticaret.Domain.Common.Pagination;

namespace E_Ticaret.Persistence.Repositories
{
    internal sealed class UserReadRepository: IUserReadRepository
    {
        private readonly AppDbContext _db;
        public UserReadRepository(AppDbContext db) => _db = db;

        public async Task<PagedResult<UserListDto>> GetUsersAsync(UserFilter filter)
        {
            var query = _db.Users.Include(u => u.Orders).AsQueryable();


            if (filter.IsDeleted.HasValue)
            {
                query = query.IgnoreQueryFilters().Where(x => x.IsDeleted == filter.IsDeleted.Value);
            }


            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var s = filter.Search.Trim().ToLower();
                query = query.Where(u => u.Email.ToLower().Contains(s) || u.FirstName.ToLower().Contains(s) || u.LastName.ToLower().Contains(s));
            }


            if (filter.IsLocked.HasValue)
            {
                query = filter.IsLocked.Value ? query.Where(u => u.LockoutEnd.HasValue && u.LockoutEnd > DateTimeOffset.UtcNow) : query.Where(u => u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow);

            }

            if (filter.CreatedFrom.HasValue)
                query = query.Where(u => u.CreatedAt >= filter.CreatedFrom.Value);

            if (filter.CreatedTo.HasValue)
                query = query.Where(u => u.CreatedAt <= filter.CreatedTo.Value);


            var total = query.Count();
            var users = await query
          .OrderByDescending(u => u.CreatedAt)
          .Skip(filter.Paging.Skip)
          .Take(filter.Paging.PageSize)
          .ToListAsync();






            var userDtos = users.Select(u => new UserListDto(
              u.Id,
              $"{u.FirstName} {u.LastName}",
              u.Email,
              u.EmailConfirmed,
              u.IsDeleted,
              u.LockoutEnd.HasValue && u.LockoutEnd > DateTimeOffset.UtcNow,
              u.CreatedAt,
              u.Orders?.Count ?? 0
          )).ToList();

            return new PagedResult<UserListDto>(userDtos, total, filter.Paging.Page, filter.Paging.PageSize);




        }

    }
}
