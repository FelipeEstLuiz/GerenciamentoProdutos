using Application.Domain.Enums;
using Application.Domain.Exceptions;

namespace Application.Domain.Entities;

public class Produto
{
    protected Produto() { }

    public Produto(
        string nome,
        string? descricao,
        decimal valor,
        int quantidadeEstoque,
        int categoria
    )
    {
        SetNome(nome);
        SetDescricao(descricao);
        SetValor(valor);
        SetQuantidadeEstoque(quantidadeEstoque);
        SetCategoria(categoria);
    }

    public Produto(
        int id,
        string nome,
        string? descricao,
        decimal valor,
        int quantidadeEstoque,
        int categoria,
        StatusProduto status,
        DateTime dataCadastro
    )
    {
        ValidacaoException.When(id <= 0, "Id inválido");
        Id = id;
        DataCadastro = dataCadastro;
        SetNome(nome);
        SetDescricao(descricao);
        SetValor(valor);
        SetQuantidadeEstoque(quantidadeEstoque);
        SetCategoria(categoria);
        SetStatus(status);
    }

    public int Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string? Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public int QuantidadeEstoque { get; private set; }
    public int IdCategoria { get; private set; }
    public StatusProduto CodigoStatusProduto { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public DateTime? DataUltimaVenda { get; private set; }

    public void Update(
        string nome,
        string? descricao,
        decimal valor,
        int quantidadeEstoque,
        int categoria,
        StatusProduto status,
        DateTime? dataUltimaVenda
    )
    {
        SetNome(nome);
        SetDescricao(descricao);
        SetValor(valor);
        SetQuantidadeEstoque(quantidadeEstoque);
        SetCategoria(categoria);
        SetStatus(status);
        SetDataUltimaVenda(dataUltimaVenda);
    }

    public void SetDataUltimaVenda(DateTime? dataUltimaVenda)
    {

        DateTime dataAtual = DateTime.UtcNow;

        ValidacaoException.When(
            dataUltimaVenda.HasValue && dataUltimaVenda > dataAtual,
            "Data da ultima venda nao pode ser maior que a data atual."
        );

        DataUltimaVenda = dataUltimaVenda;

        if (dataUltimaVenda.HasValue && dataUltimaVenda.Value.Date > dataAtual.AddDays(-60).Date)
            SetStatus(StatusProduto.Disponivel);
    }

    private void SetNome(string nome)
    {
        ValidacaoException.When(string.IsNullOrWhiteSpace(nome), "Nome não pode ser vazio ou nulo.");
        ValidacaoException.When(nome.Length > 250, "Nome não pode ter mais que 250 caracteres.");
        ValidacaoException.When(nome.Length < 5, "Nome não pode ter menos que 5 caracteres.");
        Nome = nome;
    }

    private void SetDescricao(string? descricao)
    {
        ValidacaoException.When(descricao is not null && descricao.Length > 800, "Descrição não pode ter mais que 800 caracteres.");
        ValidacaoException.When(descricao is not null && descricao.Length < 5, "Descrição não pode ter menos que 5 caracteres.");

        Descricao = descricao;
    }

    private void SetValor(decimal valor)
    {
        ValidacaoException.When(valor <= 0, "Valor deve ser maior que 0");
        Valor = valor;
    }

    private void SetQuantidadeEstoque(int quantidadeEstoque)
    {
        ValidacaoException.When(quantidadeEstoque < 0, "Quantidade em estoque nao pode ser negativa");
        QuantidadeEstoque = quantidadeEstoque;
    }

    private void SetCategoria(int categoria) => IdCategoria = categoria;
    public void SetId(int id) => Id = id;
    public void SetDataCadastro(DateTime dataCadastro) => DataCadastro = dataCadastro;
    public void SetStatus(StatusProduto status) => CodigoStatusProduto = status;
}
