using Dapper;
using Logger;
using Npgsql;
using Serilog;

namespace Infrastructure.Repositories;

public class PgIndexRepository : IIndexRepository
{
    private readonly NpgsqlDataSource _dataSource;
    public PgIndexRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    
    /**
     * creates the necessary entries in the database 
     */
    public void CreateIndexes(Dictionary<string, int> indexes, string filename, byte[] data)
    {
        using var activity = Monitoring.ActivitySource.StartActivity() ;
        int fileId = 0;
            
        // Sets up the Sql query for the different operations 
        var insertFile = $@"INSERT INTO Files (file_name, content) VALUES (@filename, @data) returning file_id;";
        var wordInsertOrDoNothing = $@"INSERT INTO Words (word) VALUES (@word) ON CONFLICT(word) DO UPDATE SET word = @word RETURNING word_id;";
        var insertOccurence = $@"INSERT INTO Occurrences (word_id,file_id,count) values (@word_id,@file_id,@count)";
            
        // opens a connection to the database
        using (var conn = _dataSource.OpenConnection())
        { 
            // sets up a transaction
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    fileId = conn.QueryFirst<int>(insertFile, new { filename, data }); 
                    foreach (var index in indexes) 
                    { 
                        var word = conn.QueryFirstOrDefaultAsync<int>(wordInsertOrDoNothing, new { word = index.Key }); 
                        conn.Execute(insertOccurence, new { file_id = fileId, word_id = word.Result, count = index.Value });
                    } 
                    transaction.Commit();
                }
                catch (Exception ex) 
                { 
                    Log.Logger.Error("Error in setting up indexes in database"); 
                    transaction.Rollback();
                }
            }
        }
    }
}