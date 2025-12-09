namespace E_Ticaret.Domain.Common
{
    public abstract class BaseEntity
    {

        public BaseEntity()
        {
            Id = Guid.CreateVersion7();
        }


        public virtual Guid Id { get; set; } 
        public virtual string CreatorUser { get; set; } = "default";
         public virtual DateTimeOffset CreatedAt { get; set; } 
        public virtual DateTimeOffset? UpdatedAt { get; set; }
        public virtual DateTimeOffset? DeletedAt { get; set; }
        public virtual bool IsDeleted { get; set; } = false;
    }
}
