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
    private readonly string _queueName = "Files";
    /*consider testing prefetchcount a bit to see if we can speed file transfer up a bit,
     50 is default. Higher values gives better performance but requires more memory
     */
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
     Speed: For Emil's Laptop
     Total execution time: 548667(~610000 with rabbitmq implemented) ms
     Total folders processed: 3499
     */

    // Get all folders, including subfolders
    var allFolders = Directory.GetDirectories(rootFolderPath, "*", SearchOption.AllDirectories);
    
    Log.Logger.Here().Debug($@"Attempting to retrieve read files from {rootFolderPath}");
    
    foreach (var folder in allFolders)
    {
        Console.WriteLine($"Processing folder: {folder}");
        
        var files = Directory.GetFiles(folder);

        Parallel.ForEach(files, filePath =>
        {
            byte[] content = File.ReadAllBytes(filePath);
            
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
     Speed: For Emil's Laptop
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
