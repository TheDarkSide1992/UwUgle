using Infrastructure.Interface;
using Service.Interfaces;

namespace Service.Implementations;

public class ServiceString : IService<string,string>
{
    private ISearchRespository<string, string> _searchRepository;

    /**
     * Basse Interface for strings
     */
    public ServiceString(ISearchRespository<string, string> searchRepository)
    {
        _searchRepository = searchRepository;
    }
    
    public Task<IEnumerable<string>> QuerySearch(string query)
    {
        return _searchRepository.QuerySearch(query);
    }

    public Task<string> GetFile(int id)
    {
        return _searchRepository.GetFile(id);
    }
}