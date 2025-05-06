using Application.Domain.Entities;
using Application.Infraestructure.Data.Configuration;
using Application.Infraestructure.Data.Repository;
using Tests.Application.Infraestructure.Data.Fixtures;

namespace Tests.Application.Infraestructure.Data.Repository;

public class CategoriaRepositoryTests : IClassFixture<SqlServerTestFixture>, IAsyncLifetime
{
    private readonly SqlServerTestFixture _fixture;
    private readonly CategoriaRepository _categoriaRepository;

    public CategoriaRepositoryTests(SqlServerTestFixture fixture)
    {
        _fixture = fixture;

        DbConnectionFactory dbConnection = new(_fixture.Configuration);

        _categoriaRepository = new CategoriaRepository(dbConnection);
    }

    public async Task InitializeAsync() => await _fixture.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetAllAsync_DeveRetornarSucesso_QuandoEncontrar()
    {
        IEnumerable<CategoriaProduto> result = await _categoriaRepository.GetAllAsync(CancellationToken.None);

        Assert.NotNull(result);
    }
}