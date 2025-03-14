using System.Collections.Concurrent;
using EasyNetQ;
using EasyNetQ.Topology;
using Infrastructure.Interfaces;
using Logger;
using Serilog;

namespace Infrastructure.Implementations;

public class Reader : IReader
{
    
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
