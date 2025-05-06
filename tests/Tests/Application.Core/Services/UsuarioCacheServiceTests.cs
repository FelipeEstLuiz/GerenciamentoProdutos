using Application.Core.Services;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace Tests.Application.Core.Services;

public class UsuarioCacheServiceTests
{
    private readonly IUsuarioRepository _usuarioRepositoryMock;
    private readonly IMemoryCache _memoryCacheMock;
    private readonly UsuarioCacheService _usuarioCacheService;

    public UsuarioCacheServiceTests()
    {
        _usuarioRepositoryMock = Substitute.For<IUsuarioRepository>();
        _memoryCacheMock = Substitute.For<IMemoryCache>();
        _usuarioCacheService = new UsuarioCacheService(_usuarioRepositoryMock, _memoryCacheMock);
    }

    [Fact]
    public async Task GetByEmailAsync_DeveRetornarDoCache_SeCacheExistir()
    {
        // Arrange
        string email = "teste@email.com";
        Usuario usuarioCache = new("Teste", "Senha123", email);

        _memoryCacheMock
            .TryGetValue(email, out Arg.Any<object>()!)
            .Returns(x =>
            {
                x[1] = usuarioCache;
                return true;
            });

        // Act
        Usuario? result = await _usuarioCacheService.GetByEmailAsync(email, CancellationToken.None);

        // Assert
        Assert.Equal(usuarioCache, result);
        await _usuarioRepositoryMock.DidNotReceive().GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetByEmailAsync_DeveConsultarRepositorio_SeCacheNaoExistir()
    {
        // Arrange
        string email = "teste@email.com";
        Usuario usuarioRepositorio = new("Teste", "Senha123", email);

        _memoryCacheMock
            .TryGetValue(email, out Arg.Any<object>()!)
            .Returns(false);

        _usuarioRepositoryMock
            .GetByEmailAsync(email, Arg.Any<CancellationToken>())
            .Returns(usuarioRepositorio);

        ICacheEntry cacheEntryMock = Substitute.For<ICacheEntry>();
        _memoryCacheMock.CreateEntry(Arg.Any<object>()).Returns(cacheEntryMock);

        // Act
        Usuario? result = await _usuarioCacheService.GetByEmailAsync(email, CancellationToken.None);

        // Assert
        Assert.Equal(usuarioRepositorio, result);
        await _usuarioRepositoryMock.Received(1).GetByEmailAsync(email, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetByEmailAsync_DeveRetornarNull_SeUsuarioNaoExistirNoRepositorio()
    {
        // Arrange
        string email = "naoexiste@email.com";

        _memoryCacheMock
            .TryGetValue(email, out Arg.Any<object>()!)
            .Returns(false);

        _usuarioRepositoryMock
            .GetByEmailAsync(email, Arg.Any<CancellationToken>())
            .Returns((Usuario?)null);

        // Act
        Usuario? result = await _usuarioCacheService.GetByEmailAsync(email, CancellationToken.None);

        // Assert
        Assert.Null(result);
        await _usuarioRepositoryMock.Received(1).GetByEmailAsync(email, Arg.Any<CancellationToken>());
    }
}
