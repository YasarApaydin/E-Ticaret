using E_Ticaret.Application.Features.Auth;
using E_Ticaret.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Ticaret.Presentation.Controllers;
    [AllowAnonymous]
    public sealed class AuthController : ApiController
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }




        [HttpPost]
        [EnableRateLimiting("Login")]
        public async Task<IActionResult> Login(LoginCommand registerCommand, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(registerCommand, cancellationToken);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

        }


        [HttpPost]
        [EnableRateLimiting("Register")]
        public async Task<IActionResult> Register(RegisterCommand registerCommand,CancellationToken cancellationToken)
        {
           var response= await mediator.Send(registerCommand, cancellationToken);
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }



        [HttpPost]
      public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command,CancellationToken cancellationToken)
        {
            var response = await mediator.Send(command, cancellationToken);
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }
   
    }

