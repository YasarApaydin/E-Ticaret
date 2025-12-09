using E_Ticaret.Domain.Common;

namespace E_Ticaret.Domain.Entities
{
    public class CartItem:BaseEntity
    {


        public Guid CartId { get; set; }
        public virtual Cart Cart { get; set; } = null!;

        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
