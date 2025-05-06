using Application.Domain.Entities;

namespace Tests.Util;
public static class CriarProduto
{
    public static Produto CriarProdutoTeste() => new("Produto Teste", "Descrição Teste", 100.50m, 10, 1);
}
