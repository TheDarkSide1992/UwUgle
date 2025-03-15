namespace Service.Interface;

public interface IIndexer
{
    /**
     * method used to index words from a text
     */
    Dictionary<string, int> Index(string text);
}