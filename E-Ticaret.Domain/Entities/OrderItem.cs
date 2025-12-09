using E_Ticaret.Domain.Common;

namespace E_Ticaret.Domain.Entities
{
    public class OrderItem:BaseEntity
    {
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
