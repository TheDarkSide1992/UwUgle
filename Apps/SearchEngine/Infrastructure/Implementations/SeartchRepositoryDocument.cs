using Dapper;
using Infrastructure.Entities;
using Infrastructure.Implementations.Mappers;
using Infrastructure.Interface;
using Npgsql;
using SharedModels;

namespace Infrastructure.Implementations;

public class SeartchRepositoryDocument : ISearchRespository<DocumentSimple, Document>
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly FileMapper _mapper;
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
        var sql = $@"SELECT Files.file_id, Files.file_name, Files.content FROM Files 
                JOIN Occurrences O on Files.file_id = O.file_id 
                JOIN Words W on W.word_id = O.word_id 
                WHERE word LIKE @query';
                ";

        using var conn = _dataSource.OpenConnection();
        
        var list = await conn.QueryAsync<DocumentEntity>(sql, new {
            query = query
        });

        return list.Select(entity => _mapper.FromEntityToDocumentSimple(entity));
        
    }

    /**
     * Retrives docuement from database
     */
    public async Task<Document> GetFile(int id)
    {
        var sql = $@"SELECT * FROM Files WHERE file_id = @id";

        using var conn = _dataSource.OpenConnection();

        var entiti = await conn.QuerySingleAsync<DocumentEntity>(sql, new {
            id = id
        });

        return _mapper.FromEntityToDocument(entiti);
    }
}