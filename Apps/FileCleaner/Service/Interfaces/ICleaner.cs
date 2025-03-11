namespace Service.Interfaces;

public interface ICleaner<T> where T : class
{
    /**
     * Cleans messages
     */
    Task<T> Clean(T message);
}