using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using E_Ticaret.Persistence.Context;
using GenericRepository;

namespace E_Ticaret.Persistence.Repositories;
internal sealed class RoleRepository : Repository<AppRole, AppDbContext>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }
}