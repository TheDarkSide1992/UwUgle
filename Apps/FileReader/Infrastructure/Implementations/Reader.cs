using Infrastructure.Interfaces;

namespace Infrastructure.Implementations;

public class Reader : IReader
{
   
    /**
     * gets list of paths for folders in the given folder path
     */
    public string[] GetFoldersPath(string rootFolderPath)
    {
        return Directory.GetDirectories(rootFolderPath, "*", SearchOption.AllDirectories);
    }

    /**
     * gets a list of file paths from the given folder path
     */
    public string[] GetFilesPathFromFolder(string folderPath)
    {
        return Directory.GetFiles(folderPath);
    }

    /**
     * retrieves a file as a bytearray from the given file path.
     */
    public byte[] ReadFileAsByteArray(string filePath)
    {
        return File.ReadAllBytes(filePath);
    }


}
