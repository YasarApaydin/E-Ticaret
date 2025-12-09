using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using E_Ticaret.Persistence.Context;
using GenericRepository;

namespace E_Ticaret.Persistence.Repositories;
internal sealed class OrderRepository : Repository<Order, AppDbContext>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }
}
