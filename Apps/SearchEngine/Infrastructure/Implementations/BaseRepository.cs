using Npgsql;

namespace Infrastructure;

public abstract class BaseRepository
{
    internal readonly NpgsqlDataSource _dataSource;
    /**
     * Abstract Base repository for repositories to enharit
     */
    public BaseRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
}