using System.Collections.Concurrent;
using EasyNetQ;
using EasyNetQ.Topology;
using Infrastructure.Interfaces;
using Logger;
using Serilog;

namespace Infrastructure.Implementations;

public class Reader : IReader
{
    public Reader()
    {
    }
    
    
    
/**
 * Reads the maildir and returns all files as a list when it's done.
 * Its uses multiprocessing, to read the maildir faster, might be slower than alternative on a very low amount of files
 */


public string[] GetFoldersPath(string rootFolderPath)
{
    return Directory.GetDirectories(rootFolderPath, "*", SearchOption.AllDirectories);
}

public string[] GetFilesPathFromFolder(string folderPath)
{
    return Directory.GetFiles(folderPath);
}

public byte[] ReadFileAsByteArray(string filePath)
{
    return File.ReadAllBytes(filePath);
}

}
