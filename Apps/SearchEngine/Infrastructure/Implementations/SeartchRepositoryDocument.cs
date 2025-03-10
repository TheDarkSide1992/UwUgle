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

    /**
     * query seartches the database based on word (query)
     */
    public Task<IEnumerable<DocumentSimple>> QuerySearch(string query)
    {
        throw new NotImplementedException();
    }

    /**
     * Retrives docuement from database
     */
    public Task<Document> GetFile(int id)
    {
        throw new NotImplementedException();
    }
}