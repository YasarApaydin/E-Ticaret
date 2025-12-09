using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Enums;

namespace E_Ticaret.Domain.Entities
{
    public class Order:BaseEntity
    {
        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; } = null!;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal TotalPrice { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
