using System.Collections.Concurrent;

namespace Service.Interfaces;

public interface IService
{
    Task ReadFoldersSequentiallyWithParallelFilesAsBytes(
        string rootFolderPath);
}