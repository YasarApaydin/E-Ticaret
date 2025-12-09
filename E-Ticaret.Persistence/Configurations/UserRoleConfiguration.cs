using E_Ticaret.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret.Persistence.Configurations
{
    internal sealed class UserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            // Primary key
            builder.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            builder.ToTable("AspNetUserRoles");


            builder.HasOne(r => r.AppUser)
                  .WithMany() 
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.AppRole)
                  .WithMany()
                  .HasForeignKey(r => r.RoleId)
                  .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
