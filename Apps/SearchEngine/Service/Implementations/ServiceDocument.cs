using Infrastructure.Interface;
using Logger;
using Service.Interfaces;
using SharedModels;

namespace Service.Implementations;

public class ServiceDocument : IService<DocumentSimple,Document>
{
    private ISearchRespository<DocumentSimple,Document> _searchRepository;

    /**
     * Basse Interface for strings
     */
    public ServiceDocument(ISearchRespository<DocumentSimple,Document> searchRepository)
    {
        _searchRepository = searchRepository;
    }
    
    /**
     * Handles query and sends requst to repo
     */
    public async Task<IEnumerable<DocumentSimple>> QuerySearch(string query)
    {
        using var activity = Monitoring.ActivitySource.StartActivity();

        return await _searchRepository.QuerySearch(query);
    }

    /**
     * Handles request for getting dcument and sends it to repo 
     */
    public async Task<Document> GetFile(int id)
    {
        using var activity = Monitoring.ActivitySource.StartActivity();

        return await _searchRepository.GetFile(id);
    }
}