using Application.Core.Services;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Tests.Util;

namespace Tests.Application.Core.Services;

public class CategoriaCacheServiceTests
{
    private readonly ICategoriaRepository _categoriaRepositoryMock;
    private readonly IMemoryCache _memoryCacheMock;
    private readonly CategoriaCacheService _categoriaCacheService;

    public CategoriaCacheServiceTests()
    {
        _categoriaRepositoryMock = Substitute.For<ICategoriaRepository>();
        _memoryCacheMock = Substitute.For<IMemoryCache>();
        _categoriaCacheService = new CategoriaCacheService(_categoriaRepositoryMock, _memoryCacheMock);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarDoCache_SeCacheExistir()
    {
        // Arrange
        string cacheKey = "categoria_cache";
        List<CategoriaProduto> categoriasCache = CriarCategoria.CriarCategorias(2);

        _memoryCacheMock
            .TryGetValue(cacheKey, out Arg.Any<object>()!)
            .Returns(x =>
            {
                x[1] = categoriasCache;
                return true;
            });

        // Act
        IEnumerable<CategoriaProduto> result = await _categoriaCacheService.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.Equal(categoriasCache, result);
        await _categoriaRepositoryMock.DidNotReceive().GetAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAllAsync_DeveConsultarRepositorio_SeCacheNaoExistir()
    {
        // Arrange
        string cacheKey = "categoria_cache";
        List<CategoriaProduto> categoriasRepositorio = CriarCategoria.CriarCategorias(2);

        _memoryCacheMock
            .TryGetValue(cacheKey, out Arg.Any<object>()!)
            .Returns(false);

        _categoriaRepositoryMock
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(categoriasRepositorio);

        ICacheEntry cacheEntryMock = Substitute.For<ICacheEntry>();
        _memoryCacheMock.CreateEntry(Arg.Any<object>()).Returns(cacheEntryMock);

        // Act
        IEnumerable<CategoriaProduto> result = await _categoriaCacheService.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.Equal(categoriasRepositorio, result);
        _memoryCacheMock.Received(1).CreateEntry(cacheKey);
        cacheEntryMock.Received().Dispose();
    }

    [Fact]
    public async Task GetAllAsync_DeveArmazenarNoCache_SeCacheNaoExistir()
    {
        // Arrange
        string cacheKey = "categoria_cache";
        List<CategoriaProduto> categoriasRepositorio = CriarCategoria.CriarCategorias(2);

        _memoryCacheMock
            .TryGetValue(cacheKey, out Arg.Any<object>()!)
            .Returns(false);

        _categoriaRepositoryMock
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(categoriasRepositorio);

        ICacheEntry cacheEntryMock = Substitute.For<ICacheEntry>();
        _memoryCacheMock.CreateEntry(cacheKey).Returns(cacheEntryMock);

        // Act
        IEnumerable<CategoriaProduto> result = await _categoriaCacheService.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.Equal(categoriasRepositorio, result);
        _memoryCacheMock.Received(1).CreateEntry(cacheKey);
        cacheEntryMock.Received().Dispose();
    }
}
