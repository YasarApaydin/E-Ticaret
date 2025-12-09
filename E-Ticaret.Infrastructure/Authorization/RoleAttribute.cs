using E_Ticaret.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Ticaret.Infrastructure.Authorization
{
    public sealed class RoleAttribute : Attribute, IAuthorizationFilter
    {

        private readonly IUserRoleRepository roleRepository;
        private readonly string role;

        public RoleAttribute(string role, IUserRoleRepository roleRepository)
        {
            this.role = role;
            this.roleRepository = roleRepository;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if(userIdClaim is null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var userRoleHash = roleRepository.Where(p => p.UserId.ToString() == userIdClaim.Value).Include(c => c.AppRole).Any(p => p.AppRole.Name ==role);
            if (!userRoleHash)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

                

        }
    }
}
