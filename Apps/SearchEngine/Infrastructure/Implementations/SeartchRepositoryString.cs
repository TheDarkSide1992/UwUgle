using Infrastructure.Interface;
using Npgsql;

namespace Infrastructure;

public class SeartchRepositoryString : BaseRepository, ISearchRespository<string,string>
{
    public SeartchRepositoryString(NpgsqlDataSource dataSource) : base(dataSource)
    {
    }

    public Task<IEnumerable<string>> QuerySearch(string query)
    {
        Console.WriteLine("Repo Q");

        throw new NotImplementedException();
    }

    public Task<string> GetFile(int id)
    {
        Console.WriteLine("Repo F");
        throw new NotImplementedException();
    }
}