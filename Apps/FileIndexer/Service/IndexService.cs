using System.Text;
using Infrastructure;
using Infrastructure.Repositories;
using Logger;
using Serilog;
using Service.Helpers.DatabaseConnectionHelper;
using Service.Interface;

namespace Service;

public class IndexService
{
    private IIndexRepository IndexRepository;
    private IIndexer indexer;
    
    public IndexService()
    {
        IndexRepository = new PgIndexRepository(DbConnectionHelper.PostgressCreateConnection());
        indexer = new WordIndexerHelper();
    }

    /**
     * indexes file and sends a call to the infrastructure layer
     */
    public async Task<bool> Index(byte[] file)
    {
        using var activity = Monitoring.ActivitySource.StartActivity();
        try
        {
            var text = Encoding.UTF8.GetString(file);
            var split = text.Split(" ");
            var name = $@"{split[0]} {split[1]} {split[2]} {split[3]} {split[4]} {split[5]}";
            var index = indexer.Index(text);
            IndexRepository.CreateIndexes(index,name, file);
            return true;
        }
        catch (Exception e)
        {
            Log.Logger.Error("Error while Indexing file");
        }

        return false;
    }
}