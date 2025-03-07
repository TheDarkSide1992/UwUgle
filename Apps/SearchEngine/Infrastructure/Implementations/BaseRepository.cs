using Npgsql;

namespace Infrastructure;

public class BaseRepository
{
    internal readonly NpgsqlDataSource _dataSource;

    public BaseRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
}