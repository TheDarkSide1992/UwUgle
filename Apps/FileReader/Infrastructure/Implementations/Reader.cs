using System.Collections.Concurrent;
using System.Text;
using EasyNetQ;
using EasyNetQ.Topology;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Implementations;

public class Reader : IReader
{
    
    private readonly IBus _bus;
    private readonly string _queueName = "Files";

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
public async Task ReadFoldersSequentiallyWithParallelFilesAsBytes(string rootFolderPath)
{
    
    /*
     Speed:
     Total execution time: 548667 ms
     Total folders processed: 3499
     */
    var result = new Dictionary<string, ConcurrentDictionary<string, byte[]>>();

    // Get all folders, including subfolders
    var allFolders = Directory.GetDirectories(rootFolderPath, "*", SearchOption.AllDirectories);

    foreach (var folder in allFolders)
    {
        Console.WriteLine($"Processing folder: {folder}");

        var fileContents = new ConcurrentDictionary<string, byte[]>();
        var files = Directory.GetFiles(folder);

        Parallel.ForEach(files, filePath =>
        {
            byte[] content = File.ReadAllBytes(filePath);
            fileContents[Path.GetFileName(filePath)] = content;
            
            var properties = new MessageProperties { DeliveryMode = 2 }; // Persistent message

            _bus.Advanced.PublishAsync(Exchange.Default, _queueName, false, properties, content);
        });
    }
}



/**
 * Reads the files 1 at a time, and returns the whole maildir as a dictionary when its done.
 */
public Dictionary<string, ConcurrentDictionary<string, byte[]>> ReadFolderFilesAsByteArray(string rootFolderPath)
{
    /*
     Speed:
     Total execution time: 3328948 ms
     Total folders processed: 3499
     */
    var result = new Dictionary<string, ConcurrentDictionary<string, byte[]>>();

    // Get all folders, including subfolders
    var allFolders = Directory.GetDirectories(rootFolderPath, "*", SearchOption.AllDirectories);

    foreach (var folder in allFolders)
    {
        Console.WriteLine($"Processing folder: {folder}");

        var fileContents = new ConcurrentDictionary<string, byte[]>();
        var files = Directory.GetFiles(folder);
        
        foreach (var filePath in files)
        {
            byte[] content = File.ReadAllBytes(filePath);
            fileContents[Path.GetFileName(filePath)] = content;
        }

        result[folder] = fileContents;
    }

    return result;
}
}
