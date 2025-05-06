using Application.Api.Controllers._Shared;
using Application.Core.Command.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.V1.Controllers.Application;

[ApiExplorerSettings(GroupName = "Login")]
public class LoginController(IMediator mediator) : BaseApplicationController
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommand request)
       => HandlerResponse(HttpStatusCode.OK, await mediator.Send(request));
}
