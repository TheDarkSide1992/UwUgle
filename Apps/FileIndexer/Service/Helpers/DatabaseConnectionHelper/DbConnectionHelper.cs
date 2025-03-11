using Npgsql;

namespace Service.Helpers.DatabaseConnectionHelper;

public static class DbConnectionHelper
{
    /*
     * Creates a NpgsqlDataSource
     */
    public static NpgsqlDataSource PostgressCreateConnection()
    {
        var builder = new NpgsqlDataSourceBuilder(Environment.GetEnvironmentVariable("pgconn"));
        var dbSource = builder.Build();
        return dbSource;
    }
}