using Infrastructure.Interface;
using Npgsql;
using SharedModels;

namespace Infrastructure;

public class SeartchRepositoryDocument : ISearchRespository<DocumentSimple, Document>
{
    internal readonly NpgsqlDataSource _dataSource;

    public SeartchRepositoryDocument(NpgsqlDataSource dataSource) 
    {
        _dataSource = dataSource;

    }

    public Task<IEnumerable<DocumentSimple>> QuerySearch(string query)
    {
        throw new NotImplementedException();
    }

    public Task<Document> GetFile(int id)
    {
        throw new NotImplementedException();
    }
}