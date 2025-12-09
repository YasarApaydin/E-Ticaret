using Microsoft.AspNetCore.Identity;
namespace E_Ticaret.Domain.Entities
{
    public sealed class AppUserRole:IdentityUserRole<Guid>
    {
        public AppUser AppUser { get; set; } = default!;
        public AppRole AppRole { get; set; } = default!;
    }
}
