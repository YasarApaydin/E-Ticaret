using E_Ticaret.Domain.Common;

namespace E_Ticaret.Domain.Entities
{
    public class ProductVariant:BaseEntity
    {


        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        public string VariantName { get; set; } = null!; 
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? StockCode { get; set; }
    }
}
