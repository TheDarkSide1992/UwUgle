using System.Collections.Concurrent;
using EasyNetQ;
using EasyNetQ.Topology;
using Infrastructure.Interfaces;
using Logger;
using Serilog;

namespace Infrastructure.Implementations;

public class Reader : IReader
{
    
    private readonly IBus _bus;
    private readonly string _queueName = "FilesV6";
    /*consider testing prefetchcount a bit to see if we can speed file transfer up a bit,
     50 is default. Higher values gives better performance but requires more memory
     */
    //TODO put into environmental secret when done testing
    private readonly string connectionString =
        "host=localhost;username=guest;password=guest;timeout=30;publisherConfirms=true;prefetchcount=50;persistentMessages=true;connectIntervalAttempt=5";
    public Reader()
    {
        _bus = RabbitHutch.CreateBus(connectionString);
        _bus.Advanced.QueueDeclareAsync(name: _queueName);
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
