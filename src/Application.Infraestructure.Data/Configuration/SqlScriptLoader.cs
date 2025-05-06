using System.Reflection;

namespace Application.Infraestructure.Data.Configuration;

public static class SqlScriptLoader
{
    public static string LoadInitSql()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string resourceName = assembly
            .GetManifestResourceNames()
            .FirstOrDefault(name => name.EndsWith("CreateTables.sql", StringComparison.OrdinalIgnoreCase))
            ?? throw new FileNotFoundException("Recurso SQL não encontrado.");

        using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}

