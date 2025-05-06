using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.Controllers._Shared;

[ApiController]
[Route("api/app/v{version:apiVersion}/[controller]")]
[ApiExplorerSettings(GroupName = "Application")]
[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(IEnumerable<string>))]
[ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(IEnumerable<string>))]
[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(IEnumerable<string>))]
[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(IEnumerable<string>))]
public class BaseApplicationController() : BaseController()
{
}

