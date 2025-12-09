using Microsoft.AspNetCore.Mvc;

namespace E_Ticaret.Infrastructure.Authorization
{
    public sealed class RoleFilterAttribute : TypeFilterAttribute
    {
        public RoleFilterAttribute(string role) : base(typeof(RoleAttribute))
        {
            Arguments = new object[] { role };

        }
    }
}
