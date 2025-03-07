using Infrastructure.Interface;
using Npgsql;

namespace Infrastructure;

public class SeartchRepositoryString : BaseRepository, ISearchRespository<string,string>
{
    /*
     *  Creates reposotory instance implenting interface with a datsource from BaseRepository
     */
    public SeartchRepositoryString(NpgsqlDataSource dataSource) : base(dataSource)
    {
    }

    public Task<IEnumerable<string>> QuerySearch(string query)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetFile(int id)
    {
        throw new NotImplementedException();
    }
}