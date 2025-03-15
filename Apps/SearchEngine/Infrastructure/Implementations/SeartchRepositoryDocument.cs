using Dapper;
using Infrastructure.Entities;
using Infrastructure.Implementations.Mappers;
using Infrastructure.Interface;
using Logger;
using Npgsql;
using Serilog;
using SharedModels;

namespace Infrastructure.Implementations;

public class SeartchRepositoryDocument : ISearchRespository<DocumentSimple, Document>
{
    private NpgsqlDataSource _dataSource;
    private FileMapper _mapper;
    public SeartchRepositoryDocument(NpgsqlDataSource dataSource)
    {
        _mapper = new FileMapper();
        _dataSource = dataSource;
    }

    /**
     * query seartches the database based on word (query)
     */
    public async Task<IEnumerable<DocumentSimple>> QuerySearch(string query)
    {
        using var activity = Monitoring.ActivitySource.StartActivity();

        var sql = $@"SELECT Files.file_id, Files.file_name, Files.content FROM Files 
                JOIN Occurrences O on Files.file_id = O.file_id 
                JOIN Words W on W.word_id = O.word_id 
                WHERE word ILIKE @query;
                ";

        using var conn = _dataSource.OpenConnection();
        
        var list = await conn.QueryAsync<DocumentEntity>(sql, new {
            query = @$"%{query}%"
        });

        return list.Select(entity => _mapper.FromEntityToDocumentSimple(entity)); //Maps entity element in list to DocumentSImple
        
    }

    /**
     * Retrives docuement from database based on ID
     */
    public async Task<Document> GetFile(int id)
    {
        using var activity = Monitoring.ActivitySource.StartActivity();

        Log.Logger.Here().Debug($@"Retrieving document {id} ");
        
        var sql = $@"SELECT Files.file_id, Files.file_name, Files.content FROM Files WHERE file_id = @id";

        using var conn = _dataSource.OpenConnection();

        var entiti = await conn.QuerySingleAsync<DocumentEntity>(sql, new {
            id = id
        });
         
        return _mapper.FromEntityToDocument(entiti);
    }
}