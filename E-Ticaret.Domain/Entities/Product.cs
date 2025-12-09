using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Enums;

namespace E_Ticaret.Domain.Entities
{
    public class Product:BaseEntity
    {


        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stoct { get; set; }
        public ProductUnit Unit { get; set; } = ProductUnit.Kg;

        public double UnitSize { get; set; } = 1.0;
        public string ImageUrl { get; set; } = string.Empty;


        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
    }
}
