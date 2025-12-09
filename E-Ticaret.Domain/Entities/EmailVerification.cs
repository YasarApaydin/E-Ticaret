using E_Ticaret.Domain.Common;

namespace E_Ticaret.Domain.Entities
{
    public class EmailVerification:BaseEntity
    {


        public Guid UserId { get; set; }
        public AppUser User { get; set; } = default!;
        public string Code { get; set; } = string.Empty;
        public DateTime ExpirationTime { get; set; }
  
    }
}
