using Application.Domain.Entities;
using Application.Domain.Util;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Application.Infraestructure.Data.Configuration;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(string connectionString)
    {
        try
        {
            string targetDb = GetDatabaseNameFromConnectionString(connectionString);
            string masterConnectionString = ReplaceDatabaseInConnectionString(connectionString, "master");

            using (SqlConnection masterConnection = new(masterConnectionString))
            {
                await masterConnection.OpenAsync();

                string createDbSql = $@"
                    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{targetDb}')
                        CREATE DATABASE [{targetDb}]
                ";

                await masterConnection.ExecuteAsync(createDbSql);
            }

            using SqlConnection targetConnection = new(connectionString);
            await targetConnection.OpenAsync();

            string script = SqlScriptLoader.LoadInitSql();
            await targetConnection.ExecuteAsync(script);

            await SeedUsers(targetConnection);
        }
        catch { }
    }

    private static async Task SeedUsers(SqlConnection targetConnection)
    {
        string query = @"
            IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Email = @Email)
                INSERT INTO Usuario (Nome, Senha, Email)
                VALUES (@Nome, @Senha, @Email)
        ";

        string password = "P#ssw0rd";

        await targetConnection.ExecuteAsync(query, new Usuario("teste", ClsGlobal.CriptografarSenha(password), "teste@teste.com"));
    }

    private static string GetDatabaseNameFromConnectionString(string conn)
        => new SqlConnectionStringBuilder(conn).InitialCatalog;

    private static string ReplaceDatabaseInConnectionString(string conn, string newDb)
    {
        SqlConnectionStringBuilder builder = new(conn)
        {
            InitialCatalog = newDb
        };
        return builder.ToString();
    }
}
