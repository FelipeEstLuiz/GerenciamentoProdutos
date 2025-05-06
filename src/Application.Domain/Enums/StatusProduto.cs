using Application.Domain.Attributes;

namespace Application.Domain.Enums;

public enum StatusProduto
{
    [EnumName("Disponível")]
    Disponivel = 0,

    [EnumName("Fora de estoque")]
    ForaEstoque = 1
}
