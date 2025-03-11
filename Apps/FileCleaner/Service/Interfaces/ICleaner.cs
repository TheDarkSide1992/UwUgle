namespace Service.Interfaces;

public interface ICleaner<T> where T : class
{
    Task<T> Clean(T message);
}