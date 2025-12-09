using E_Ticaret.Domain.Common;

namespace E_Ticaret.Domain.Entities
{
    public class Cart: BaseEntity
    {

        public Guid UserId { get; set; }

        public virtual AppUser AppUser { get; set; } = null!;
        public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
