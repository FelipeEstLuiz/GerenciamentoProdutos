using Application.Domain.Model;
using Application.Domain.VO;
using MediatR;

namespace Application.Core.Command.Produto;

public record CreateProdutoCommand(
    string? Nome,
    string? Descricao,
    decimal? Valor,
    int? QuantidadeEstoque,
    int? Categoria
) : IRequest<Result<ProdutoVo>>;