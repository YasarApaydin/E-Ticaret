using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using E_Ticaret.Persistence.Context;
using GenericRepository;

namespace E_Ticaret.Persistence.Repositories;
internal sealed class EmailRepository : Repository<EmailVerification, AppDbContext>, IEmailRepository
{
    public EmailRepository(AppDbContext context) : base(context)
    {
    }

  
}

