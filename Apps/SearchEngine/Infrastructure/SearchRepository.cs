using Npgsql;

namespace Infrastructure;

public class SearchRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public SearchRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
}