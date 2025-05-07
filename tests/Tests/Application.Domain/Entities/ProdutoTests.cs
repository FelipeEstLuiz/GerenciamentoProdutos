using Application.Domain.Entities;
using Application.Domain.Enums;
using Application.Domain.Exceptions;
using Tests.Util;

namespace Tests.Application.Domain.Entities;

public class ProdutoTests
{
    [Fact]
    public void Construtor_DeveCriarProdutoComValoresValidos()
    {
        // Arrange
        string nome = "Produto Teste";
        string descricao = "Descrição Teste";
        decimal valor = 100.50m;
        int quantidadeEstoque = 10;
        int categoria = 1;

        // Act
        Produto produto = new(nome, descricao, valor, quantidadeEstoque, categoria);

        // Assert
        Assert.Equal(nome, produto.Nome);
        Assert.Equal(descricao, produto.Descricao);
        Assert.Equal(valor, produto.Valor);
        Assert.Equal(quantidadeEstoque, produto.QuantidadeEstoque);
        Assert.Equal(categoria, produto.IdCategoria);
    }

    [Fact]
    public void Construtor_DeveLancarExcecao_SeIdInvalido()
    {
        // Arrange
        int id = 0;

        // Act & Assert
        Assert.Throws<ValidacaoException>(() => new Produto(id, "Produto Teste", "Descrição Teste", 100.50m, 10, 1, StatusProduto.Disponivel, DateTime.UtcNow));
    }

    [Fact]
    public void Update_DeveAtualizarValoresDoProduto()
    {
        // Arrange
        Produto produto = CriarProduto.CriarProdutoTeste();
        string novoNome = "Produto Atualizado";
        string novaDescricao = "Descrição Atualizada";
        decimal novoValor = 200.00m;
        int novaQuantidadeEstoque = 20;
        int novaCategoria = 2;
        DateTime? novaDataUltimaVenda = DateTime.UtcNow.AddDays(-61);
        StatusProduto statusProduto = StatusProduto.Disponivel;

        // Act
        produto.Update(novoNome, novaDescricao, novoValor, novaQuantidadeEstoque, novaCategoria, statusProduto, novaDataUltimaVenda);

        // Assert
        Assert.Equal(novoNome, produto.Nome);
        Assert.Equal(novaDescricao, produto.Descricao);
        Assert.Equal(novoValor, produto.Valor);
        Assert.Equal(novaQuantidadeEstoque, produto.QuantidadeEstoque);
        Assert.Equal(novaCategoria, produto.IdCategoria);
        Assert.Equal(novaDataUltimaVenda, produto.DataUltimaVenda);
        Assert.Equal(statusProduto, produto.CodigoStatusProduto);
    }

    [Fact]
    public void SetDataUltimaVenda_DeveAtualizarStatusParaDisponivel_SeDataDentroDosUltimos60Dias()
    {
        // Arrange
        Produto produto = CriarProduto.CriarProdutoTeste();
        DateTime dataUltimaVenda = DateTime.UtcNow.AddDays(-30);

        // Act
        produto.SetDataUltimaVenda(dataUltimaVenda);

        // Assert
        Assert.Equal(StatusProduto.Disponivel, produto.CodigoStatusProduto);
    }

    [Fact]
    public void SetDataUltimaVenda_DeveLancarExcecao_SeDataMaiorQueDataAtual()
    {
        // Arrange
        Produto produto = CriarProduto.CriarProdutoTeste();
        DateTime dataFutura = DateTime.UtcNow.AddDays(1);

        // Act & Assert
        Assert.Throws<ValidacaoException>(() => produto.SetDataUltimaVenda(dataFutura));
    }

    [Fact]
    public void SetNome_DeveLancarExcecao_SeNomeInvalido()
    {
        // Arrange
        Produto produto = new();

        // Act & Assert
        Assert.Throws<ValidacaoException>(() => produto.Update("", "Descrição Teste", 100.50m, 10, 1, StatusProduto.Disponivel, null));
        Assert.Throws<ValidacaoException>(() => produto.Update("A", "Descrição Teste", 100.50m, 10, 1, StatusProduto.Disponivel, null));
        Assert.Throws<ValidacaoException>(() => produto.Update(new string('A', 251), "Descrição Teste", 100.50m, 10, 1, StatusProduto.Disponivel, null));
    }

    [Fact]
    public void SetDescricao_DeveLancarExcecao_SeDescricaoInvalida()
    {
        // Arrange
        Produto produto = new();

        // Act & Assert
        Assert.Throws<ValidacaoException>(() => produto.Update("Produto Teste", "A", 100.50m, 10, 1, StatusProduto.Disponivel, null));
        Assert.Throws<ValidacaoException>(() => produto.Update("Produto Teste", new string('A', 801), 100.50m, 10, 1, StatusProduto.Disponivel, null));
    }

    [Fact]
    public void SetValor_DeveLancarExcecao_SeValorInvalido()
    {
        // Arrange
        Produto produto = new();

        // Act & Assert
        Assert.Throws<ValidacaoException>(() => produto.Update("Produto Teste", "Descrição Teste", 0, 10, 1, StatusProduto.Disponivel, null));
        Assert.Throws<ValidacaoException>(() => produto.Update("Produto Teste", "Descrição Teste", -1, 10, 1, StatusProduto.Disponivel, null));
    }

    [Fact]
    public void SetQuantidadeEstoque_DeveLancarExcecao_SeQuantidadeNegativa()
    {
        // Arrange
        Produto produto = new();

        // Act & Assert
        Assert.Throws<ValidacaoException>(() => produto.Update("Produto Teste", "Descrição Teste", 100.50m, -1, 1, StatusProduto.Disponivel, null));
    }
}
