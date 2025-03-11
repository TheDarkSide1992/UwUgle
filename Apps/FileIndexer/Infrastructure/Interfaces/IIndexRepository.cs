namespace Infrastructure;

public interface IIndexRepository
{
    void CreateIndexes(Dictionary<string, int> indexes, string filename, byte[] data);
}