using Application.Domain.Entities;
using Application.Infraestructure.Data.Configuration;
using Application.Infraestructure.Data.Repository;
using Tests.Application.Infraestructure.Data.Fixtures;

namespace Tests.Application.Infraestructure.Data.Repository;

public class UsuarioRepositoryTests : IClassFixture<SqlServerTestFixture>, IAsyncLifetime
{
    private readonly SqlServerTestFixture _fixture;
    private readonly UsuarioRepository _usuarioRepository;

    public UsuarioRepositoryTests(SqlServerTestFixture fixture)
    {
        _fixture = fixture;

        DbConnectionFactory dbConnection = new(_fixture.Configuration);

        _usuarioRepository = new UsuarioRepository(dbConnection);
    }

    public async Task InitializeAsync() => await _fixture.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetByEmailAsync_DeveRetornarSucesso_QuandoEncontrar()
    {
        Usuario? result = await _usuarioRepository.GetByEmailAsync("teste@teste.com", CancellationToken.None);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByEmailAsync_DeveRetornarSucesso_QuandoNaoEncontrar()
    {
        Usuario? result = await _usuarioRepository.GetByEmailAsync("teste@naoexiste.com", CancellationToken.None);

        Assert.Null(result);
    }
}
