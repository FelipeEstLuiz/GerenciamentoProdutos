using Application.Domain.Enums;
using Application.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace Application.Api.Controllers._Shared;

[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class BaseController() : ControllerBase
{
    protected IActionResult HandlerResponse<T>(HttpStatusCode statusCode, Result<T> result)
    {
        if (result.IsSuccess)
            return StatusCode((int)statusCode, result.Data);
        else
        {
            statusCode = result.ResponseCode switch
            {
                ResponseCodes.USER_NOT_HAVE_PERMISSION => HttpStatusCode.Forbidden,
                ResponseCodes.UNAUTHORIZED => HttpStatusCode.Unauthorized,
                ResponseCodes.NOT_FOUND => HttpStatusCode.NotFound,
                _ => HttpStatusCode.BadRequest,
            };
        }

        return StatusCode((int)statusCode, result.Errors);
    }
}
