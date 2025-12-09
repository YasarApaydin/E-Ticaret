namespace E_Ticaret.Domain.Common
{
    public  class ProductDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
