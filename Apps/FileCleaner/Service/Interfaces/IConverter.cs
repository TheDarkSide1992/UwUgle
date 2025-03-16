namespace Service.Interfaces;

public interface IConverter<T, TF> where T : class where TF : class
{
    /**
     * Converts from T
     */
    Task<TF> From(T message);
    /**
     * Converts to T
     */
    Task<T> To(TF message);
}