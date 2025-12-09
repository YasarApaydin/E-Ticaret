using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticaret.Presentation.Abstractions
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Bearer")]
    public abstract class ApiController:ControllerBase
    {



        public readonly IMediator mediator;
        protected ApiController(IMediator mediator)
        {
            this.mediator = mediator;
        }


     
    }
}
