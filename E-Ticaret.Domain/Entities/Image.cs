using E_Ticaret.Domain.Common;

namespace E_Ticaret.Domain.Entities
{
    public sealed class Image:BaseEntity
    {

        public string ImageUrl { get; set; } = default!;
        public ICollection<AppUser>? Users { get; set; }
    }
}
