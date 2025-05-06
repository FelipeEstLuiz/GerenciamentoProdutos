using Application.Core.Services;
using Application.Domain.Entities;
using Application.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Tests.Application.Core.Services;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;

    public TokenServiceTests()
    {
        _tokenService = new TokenService(new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build());
    }

    [Fact]
    public async Task GerarToken_DeveGerarTokenComClaimsEsperadas()
    {
        // Arrange
        Usuario usuario = new("Teste", "Senha123", "teste@email.com");

        // Act
        string token = await _tokenService.GerarToken(usuario);

        // Assert
        Assert.NotNull(token);
        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

        Assert.NotNull(jwtToken);
        Assert.Contains(jwtToken.Claims, c => c.Type == "unique_name" && c.Value == usuario.Email);
        Assert.Contains(jwtToken.Claims, c => c.Type == "nameid" && c.Value == usuario.Nome);
    }

    [Fact]
    public async Task GerarToken_DeveGerarTokenComExpiracaoCorreta()
    {
        // Arrange
        Usuario usuario = new("Teste", "Senha123", "teste@email.com");

        // Act
        string token = await _tokenService.GerarToken(usuario);

        // Assert
        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

        Assert.NotNull(jwtToken);
        Assert.Equal(DateTime.UtcNow.AddHours(1).ToString("yyyy-MM-ddTHH:mm"), jwtToken.ValidTo.ToString("yyyy-MM-ddTHH:mm"));
    }

    [Fact]
    public async Task GerarToken_DeveLancarExcecao_SeChaveDeAssinaturaForInvalida()
    {
        // Arrange
        Usuario usuario = new("Teste", "Senha123", "teste@email.com");

        IConfigurationRoot configuration = new ConfigurationBuilder()
           .AddInMemoryCollection(new Dictionary<string, string?>
           {
                {
                    "Jwt:SecretKey",
                    "chave_invalida"
                }
           })
           .Build();

        TokenService tokenServic = new(configuration);

        // Act & Assert
        await Assert.ThrowsAsync<ValidacaoException>(() => tokenServic.GerarToken(usuario));
    }

    [Fact]
    public async Task GerarToken_DeveLancarExcecao_SeChaveDeAssinaturaNaoExistir()
    {
        // Arrange
        Usuario usuario = new("Teste", "Senha123", "teste@email.com");

        IConfigurationRoot configuration = new ConfigurationBuilder()
           .AddInMemoryCollection(new Dictionary<string, string?>
           {
                {
                    "Jwt:SecretKey",
                    ""
                }
           })
           .Build();

        TokenService tokenServic = new(configuration);

        // Act & Assert
        await Assert.ThrowsAsync<ValidacaoException>(() => tokenServic.GerarToken(usuario));
    }
}
