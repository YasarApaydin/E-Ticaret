using E_Ticaret.Application.Features.UserRoles;
using E_Ticaret.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticaret.Presentation.Controllers
{
    public sealed class UserRolesController : ApiController
    {
        public UserRolesController(IMediator mediator) : base(mediator)
        {
        }


        [HttpPost]
        public async Task<IActionResult> SetRole(SetUserRoleCommand request, CancellationToken cancellationToken)
        {
           var response = await mediator.Send(request,cancellationToken);
            if (response.IsSuccessful)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
