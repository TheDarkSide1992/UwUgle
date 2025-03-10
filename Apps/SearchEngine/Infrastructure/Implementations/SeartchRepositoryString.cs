using Infrastructure.Interface;
using Npgsql;

namespace Infrastructure;

public class SeartchRepositoryString :ISearchRespository<string,string>
{
    internal readonly NpgsqlDataSource _dataSource;
    public SeartchRepositoryString(NpgsqlDataSource dataSource) 
    {
        _dataSource = dataSource;

    }

    public Task<IEnumerable<string>> QuerySearch(string query)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetFile(int id)
    {
        throw new NotImplementedException();
    }
}