using Application.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Domain.Util;

public static class ClsGlobal
{
    public static byte[] GetTokenKey(IConfiguration configuration)
    {
        string secretKey = configuration["Jwt:SecretKey"]
            ?? throw new ValidationException("Token nao encontrado no arquivo appsettings");

        ValidacaoException.When(
            secretKey.Length < 64,
            "Token de autenticacao invalido.O tamanho minimo e 64 caracteres."
        );

        return Encoding.ASCII.GetBytes(secretKey);
    }

    public static string CriptografarSenha(string senha) => BCrypt.Net.BCrypt.HashPassword(senha);

    public static bool ValidarSenha(string senhaFornecida, string senhaCriptografada) => BCrypt.Net.BCrypt.Verify(senhaFornecida, senhaCriptografada);
}
