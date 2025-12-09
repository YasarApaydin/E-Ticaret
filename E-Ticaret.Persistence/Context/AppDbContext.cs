using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Entities;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Persistence.Context
{

    internal sealed class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>,IUnitOfWork
    {

        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            
            var entries = ChangeTracker.Entries<BaseEntity>();

            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();


            var user = httpContextAccessor.HttpContext?.User;

            var nameLastName = user?.FindFirst("NameLastname")?.Value;
            if(nameLastName is null)
            {
                nameLastName = "default";
            }
            foreach (var entry in entries)
            {
                if(entry.State== EntityState.Added)
                {
                    entry.Property(p => p.CreatedAt).CurrentValue=DateTimeOffset.Now;
                    entry.Property(p => p.CreatorUser).CurrentValue = nameLastName;
                }

                if(entry.State== EntityState.Modified)
                {
                    if(entry.Property(p => p.IsDeleted).CurrentValue == true)
                    {
                        entry.Property(p => p.DeletedAt).CurrentValue = DateTimeOffset.Now;
                    }
                    else
                    {
                        entry.Property(p => p.UpdatedAt).CurrentValue = DateTimeOffset.Now;

                    }
                       
                }


            }

            return base.SaveChangesAsync(cancellationToken);
        }



    }
}
