using Infrastructure.Interface;
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
    public Task<IEnumerable<DocumentSimple>> QuerySearch(string query)
    {
        return _searchRepository.QuerySearch(query);
    }

    /**
     * Handles request for getting dcument and sends it to repo 
     */
    public Task<Document> GetFile(int id)
    {
        return _searchRepository.GetFile(id);
    }
}