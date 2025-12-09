using Azure;
using E_Ticaret.Application.Features.Roles;
using E_Ticaret.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Ticaret.Presentation.Controllers
{
    public sealed class RolesController : ApiController
    {
        public RolesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleCommand reguest,CancellationToken cancellationToken)
        {
           var response= await mediator.Send(reguest, cancellationToken);
            if (response.IsSuccessful)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [EnableRateLimiting("DefaultPolicy")]
        public async Task<IActionResult> GetAll([FromQuery] GetRolesQuery reguest,CancellationToken cancellationToken)
        {
         var response = await mediator.Send(reguest,cancellationToken);

            return Ok(response);
        }


        [HttpDelete]
        public async Task<IActionResult> RemoveById( RemoveRoleCommand reguest, CancellationToken cancellationToken)
        {
          var response= await mediator.Send(reguest, cancellationToken);
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
