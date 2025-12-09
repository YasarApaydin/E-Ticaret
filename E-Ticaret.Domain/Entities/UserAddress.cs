using E_Ticaret.Domain.Common;

namespace E_Ticaret.Domain.Entities
{
    public class UserAddress:BaseEntity
    {


        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; } = null!;

        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Country { get; set; } = null!;
        public bool IsDefault { get; set; } = false;
    }
}
