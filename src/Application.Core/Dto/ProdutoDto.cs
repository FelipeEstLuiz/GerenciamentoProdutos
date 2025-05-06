using Application.Domain.Enums;

namespace Application.Core.Dto;

public record ProdutoDto(
    int Id,
    string Nome,
    string? Descricao,
    decimal Valor,
    int QuantidadeEstoque,
    int Categoria,
    StatusProduto Status,
    DateTime DataCadastro,
    DateTime? DataUltimaVenda
);
