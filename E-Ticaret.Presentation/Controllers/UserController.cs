using E_Ticaret.Application.Features.Users;
using E_Ticaret.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticaret.Presentation.Controllers;
public sealed class UserController : ApiController
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }


    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]  GetUsersQuery reguest, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(reguest, cancellationToken);

        return Ok(response);
    }
}



