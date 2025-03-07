using Infrastructure.Interface;
using Service.Interfaces;

namespace Service.Implementations;

public class ServiceString : IService<string,string>
{
    private ISearchRespository<string, string> _searchRepository;

    public ServiceString(ISearchRespository<string, string> searchRepository)
    {
        _searchRepository = searchRepository;
    }

    public Task<IEnumerable<string>> QuerySearch(string query)
    {
        Console.WriteLine("Servie Q");
        return _searchRepository.QuerySearch(query);
    }

    public Task<string> GetFile(int id)
    {
        Console.WriteLine("Servie F");
        return _searchRepository.GetFile(id);
    }
}