using Application.Domain.Entities;

namespace Tests.Util;

public static class CriarCategoria
{
    public static CategoriaProduto CriarCategoriaTeste(int id = 1) => new(id, $"Categoria {id}");

    public static List<CategoriaProduto> CriarCategorias(int quantidade = 1)
    {
        List<CategoriaProduto> categorias = [];
        for (int i = 1; i <= quantidade; i++)
        {
            categorias.Add(CriarCategoriaTeste(i));
        }
        return categorias;
    }
}
