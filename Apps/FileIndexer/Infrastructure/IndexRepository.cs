using Npgsql;

namespace Infrastructure;

public class IndexRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public IndexRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
}