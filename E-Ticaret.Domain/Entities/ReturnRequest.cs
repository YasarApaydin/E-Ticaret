using E_Ticaret.Domain.Common;

namespace E_Ticaret.Domain.Entities
{
    public class ReturnRequest:BaseEntity
    {
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        public string Reason { get; set; } = null!;
        public bool IsApproved { get; set; } = false;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    }
}
