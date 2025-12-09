using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using E_Ticaret.Persistence.Context;
using GenericRepository;

namespace E_Ticaret.Persistence.Repositories;
internal sealed class PaymentRepository : Repository<Payment, AppDbContext>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context)
    {
    }
}

