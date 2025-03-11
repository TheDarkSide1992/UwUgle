using System.Collections.Concurrent;

namespace Infrastructure.Interfaces;

public interface IReader
{
    Dictionary<string, ConcurrentDictionary<string, byte[]>> ReadFolderFilesAsByteArray(string rootFolderPath);

    Task ReadFoldersSequentiallyWithParallelFilesAsBytes(
        string rootFolderPath);
}