using System.Collections.Concurrent;
using Infrastructure;
using Infrastructure.Implementations;
using Infrastructure.Interfaces;
using Service.Interfaces;

namespace Service.Implementations;

public class ReaderService : IService
{
    
    private readonly IReader _reader;

    public ReaderService(Reader reader)
    {
        _reader = reader;
    }
    
    
    public async Task ReadFoldersSequentiallyWithParallelFilesAsBytes(string rootFolderPath)
    {
        _reader.ReadFoldersSequentiallyWithParallelFilesAsBytes(rootFolderPath);
    }

    public Dictionary<string, ConcurrentDictionary<string, byte[]>> ReadFolderFilesAsByteArray(string rootFolderPath)
    {
        return _reader.ReadFolderFilesAsByteArray(rootFolderPath);
    }
}
