using System.Collections.Concurrent;

namespace Infrastructure.Interfaces;

public interface IReader
{
    string[] GetFilesPathFromFolder(string folderPath);
    byte[] ReadFileAsByteArray(string filepath);
    string[] GetFoldersPath(string rootFolderPath);
    
}