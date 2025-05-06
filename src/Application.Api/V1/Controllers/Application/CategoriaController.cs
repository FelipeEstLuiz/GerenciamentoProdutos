using Application.Api.Controllers._Shared;
using Application.Core.Dto;
using Application.Core.Queries.ProdutoCategoria;
using Azure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.V1.Controllers.Application;

[ApiExplorerSettings(GroupName = "Categoria")]
public class CategoriaController(IMediator mediator) : BaseAuthorizationController
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<CategoriaProdutoDto>>))]
    public async Task<IActionResult> GetAllAsync() => HandlerResponse(
        HttpStatusCode.OK,
        await mediator.Send(new GetAllCategoriaQuery())
    );
}
