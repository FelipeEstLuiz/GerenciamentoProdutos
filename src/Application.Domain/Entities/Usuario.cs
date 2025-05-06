using Application.Domain.Exceptions;

namespace Application.Domain.Entities;

public class Usuario
{
    public Usuario() { }

    public Usuario(
        string nome,
        string senha,
        string email
    )
    {
        SetNome(nome);
        SetSenha(senha);
        SetEmail(email);
    }

    public Usuario(
        int id,
        string nome,
        string senha,
        string email,
        DateTime dataCadastro
    )
    {
        ValidacaoException.When(id <= 0, "Id inválido");
        Id = id;
        DataCadastro = dataCadastro;

        SetNome(nome);
        SetSenha(senha);
        SetEmail(email);
    }

    private void SetEmail(string email)
    {
        ValidacaoException.When(string.IsNullOrWhiteSpace(email), "Email não pode ser vazio ou nulo.");
        Email = email;
    }

    private void SetSenha(string senha)
    {
        ValidacaoException.When(string.IsNullOrWhiteSpace(senha), "Senha não pode ser vazio ou nulo.");
        ValidacaoException.When(senha.Length > 250, "Senha não pode ter mais que 250 caracteres.");
        ValidacaoException.When(senha.Length < 5, "Senha não pode ter menos que 5 caracteres.");

        Senha = senha;
    }

    private void SetNome(string nome)
    {
        ValidacaoException.When(string.IsNullOrWhiteSpace(nome), "Nome não pode ser vazio ou nulo.");
        ValidacaoException.When(nome.Length > 250, "Nome não pode ter mais que 250 caracteres.");
        ValidacaoException.When(nome.Length < 3, "Nome não pode ter menos que 3 caracteres.");
        Nome = nome;
    }

    public int Id { get; set; }
    public string Nome { get; private set; } = null!;
    public string Senha { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public DateTime DataCadastro { get; private set; }
}
