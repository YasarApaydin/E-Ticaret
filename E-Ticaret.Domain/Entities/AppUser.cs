using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace E_Ticaret.Domain.Entities
{
    public  class AppUser:IdentityUser<Guid>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid ImageId { get; set; }
        public Image Image { get; set; } = default!;

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }



        public virtual ICollection<Order>? Orders { get; set; }

    }
}