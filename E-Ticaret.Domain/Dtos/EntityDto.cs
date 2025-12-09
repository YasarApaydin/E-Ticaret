namespace E_Ticaret.Domain.Common
{
    public abstract class EntityDto
    {



        public virtual Guid Id { get; set; }
        public virtual string CreatorUser { get; set; } = "default";
        public virtual DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual DateTimeOffset? UpdatedAt { get; set; }
        public virtual DateTimeOffset? DeletedAt { get; set; }
        public virtual bool IsDeleted { get; set; } = false;
    }
}






