namespace Infrastructure.Interface;

public interface ISearchRespository<T, TF> where T : class where TF : class
{
    Task<IEnumerable<T>> QuerySearch(string query);
    Task<TF> GetFile(int id);
}