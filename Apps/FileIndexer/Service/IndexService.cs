using Infrastructure;
using Infrastructure.Repositories;
using Logger;
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

    public void Index(string text, byte[] file)
    {
        using var activity = Monitoring.ActivitySource.StartActivity();
        var index = indexer.Index(text);
        IndexRepository.CreateIndexes(index,"Lorem ipsum", file);
        
    }
}