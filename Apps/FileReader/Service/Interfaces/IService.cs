using System.Collections.Concurrent;

namespace Service.Interfaces;

public interface IService
{
    Task ReadFoldersSequentiallyWithParallelFilesAsBytes(
        string rootFolderPath);
    Dictionary<string, ConcurrentDictionary<string, byte[]>> ReadFolderFilesAsByteArray(string rootFolderPath);
}