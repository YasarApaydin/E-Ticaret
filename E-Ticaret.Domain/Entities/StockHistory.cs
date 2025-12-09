using E_Ticaret.Domain.Common;

namespace E_Ticaret.Domain.Entities
{
    public class StockHistory:BaseEntity
    {

        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public int QuantityChanged { get; set; }
        public string Reason { get; set; } = null!; 
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
