using Application.Domain.Enums;
using Application.Domain.Model;
using Application.Domain.VO;
using MediatR;

namespace Application.Core.Command.Produto;

public record UpdateProdutoCommand(
    string? Nome,
    string? Descricao,
    decimal? Valor,
    int? QuantidadeEstoque,
    int? Categoria,
    DateTime? DataUltimaVenda
) : IRequest<Result<ProdutoVo>>
{
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public int Id { get; set; }
}
