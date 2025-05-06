using Application.Domain.Converter;
using Application.Domain.Entities;
using Application.Domain.Enums;

namespace Application.Domain.VO;
public class ProdutoVo
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string? Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public int QuantidadeEstoque { get; private set; }
    public string? Categoria { get; private set; }

    [Newtonsoft.Json.JsonConverter(typeof(EnumToStringConverter<StatusProduto>))]
    public StatusProduto Status { get; private set; }

    [Newtonsoft.Json.JsonConverter(typeof(ConvertDateTime))]
    public DateTime DataCadastro { get; private set; }

    [Newtonsoft.Json.JsonConverter(typeof(ConvertDateTime))]
    public DateTime? DataUltimaAtualizacao { get; private set; }

    public static ProdutoVo FromEntity(Produto produto, IEnumerable<CategoriaProduto> categorias) => new()
    {
        Id = produto.Id,
        Nome = produto.Nome,
        Descricao = produto.Descricao,
        Valor = produto.Valor,
        QuantidadeEstoque = produto.QuantidadeEstoque,
        Categoria = categorias.FirstOrDefault(x => x.Id == produto.IdCategoria)?.Descricao,
        Status = produto.CodigoStatusProduto,
        DataCadastro = produto.DataCadastro,
        DataUltimaAtualizacao = produto.DataUltimaVenda
    };
}
