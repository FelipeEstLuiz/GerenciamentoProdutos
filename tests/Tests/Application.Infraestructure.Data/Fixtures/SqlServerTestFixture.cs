using Application.Infraestructure.Data.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Tests.Application.Infraestructure.Data.Fixtures;
public class SqlServerTestFixture : IAsyncLifetime
{
    public IConfiguration Configuration { get; private set; }

    public SqlServerTestFixture()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();

        Configuration = config;
    }

    public async Task InitializeAsync()
    {
        string fullConnectionString = Configuration.GetConnectionString("DefaultConnection")!;

        await DatabaseInitializer.InitializeAsync(fullConnectionString);
    }

    public async Task ResetDatabaseAsync()
    {
        using SqlConnection connection = new(Configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        await connection.ExecuteAsync(@"
            DELETE FROM Produto;          
        ");

        await connection.ExecuteAsync(@"
            DBCC CHECKIDENT ('Produto', RESEED, 0);          
        ");
    }

    public Task DisposeAsync() => Task.CompletedTask;
}