using Infrastructure.Interfaces;

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
