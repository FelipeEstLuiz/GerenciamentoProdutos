using Application.Api.Controllers._Shared;
using Application.Core.Command.Produto;
using Application.Core.Dto;
using Application.Core.Queries.Produto;
using Application.Domain.VO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.V1.Controllers.Application;

[ApiExplorerSettings(GroupName = "Produto")]
public class ProdutoController(IMediator mediator) : BaseAuthorizationController
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ProdutoVo))]
    public async Task<IActionResult> PostAsync([FromBody] CreateProdutoCommand command) => HandlerResponse(
        HttpStatusCode.Created,
        await mediator.Send(command)
    );

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ProdutoVo>))]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetAllProdutosQuery query) => HandlerResponse(
        HttpStatusCode.OK,
        await mediator.Send(query)
    );

    [HttpGet("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ProdutoDto))]
    public async Task<IActionResult> GetByIdAsync(int id) => HandlerResponse(
        HttpStatusCode.OK,
        await mediator.Send(new GetProdutoByIdQuery(id))
    );

    [HttpPut("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ProdutoVo))]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateProdutoCommand request)
    {
        request.Id = id;
        return HandlerResponse(
            HttpStatusCode.OK,
            await mediator.Send(request)
        );
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
    public async Task<IActionResult> Delete(int id)
        => HandlerResponse(HttpStatusCode.OK, await mediator.Send(new DeleteProdutoCommand(id)));
}
