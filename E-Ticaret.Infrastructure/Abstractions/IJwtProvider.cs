using E_Ticaret.Domain.Entities;

namespace E_Ticaret.Infrastructure.Abstractions
{
    public interface IJwtProvider
    {

        Task<string> CreateTokenAsync(AppUser appUser,CancellationToken cancellationToken = default);
    }
}
    
 