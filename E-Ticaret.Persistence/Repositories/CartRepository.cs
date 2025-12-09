using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using E_Ticaret.Persistence.Context;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Persistence.Repositories
{
    internal sealed class CartRepository : Repository<Cart, AppDbContext>,ICartRespository
    {

        private readonly AppDbContext appDbContext;
        public CartRepository(AppDbContext context) : base(context)
        {
            appDbContext = context;
        }


        public async Task<Cart> GetCartWithItemsAsync(Guid userId, CancellationToken cancellationToken)
        {
           
            return await appDbContext.Set<Cart>()
         .Where(x => x.UserId == userId)
         .Include(x => x.Items)
         .FirstOrDefaultAsync(cancellationToken);
        }


    }
}
