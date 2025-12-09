using E_Ticaret.Domain.Common;

namespace E_Ticaret.Domain.Entities
{
    public class Payment:BaseEntity
    {
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;
        public string PaymentProvider { get; set; } = "iyzico";
        public string PaymentReference { get; set; } = null!;
        public bool IsSuccess { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
