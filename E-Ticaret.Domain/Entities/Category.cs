using E_Ticaret.Domain.Common;


namespace E_Ticaret.Domain.Entities
{
    public class Category:BaseEntity
    {


        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
