using System.Collections.Concurrent;
using System.Text;
using EasyNetQ;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using RabbitMQ.Client;

namespace Infrastructure.Implementations;

public class Reader : IReader
{
    private readonly IBus _bus;
    
    public Reader(string connectionString = "host=localhost;username=guest;password=guest")
    {
        _bus = RabbitHutch.CreateBus(connectionString);
        _bus.PubSub.Subscribe<FilesModel>("Files", async (message) => Console.WriteLine(message));
    }
    
    
/**
 * Reads the maildir and returns all files as a list when it's done.
 * Its uses multiprocessing, to read the maildir faster, might be slower than alternative on a very low amount of files
 */
public async Task ReadFoldersSequentiallyWithParallelFilesAsBytes(string rootFolderPath)
{
    /*
    var factory = new ConnectionFactory { HostName = "localhost" };
    var connection = await factory.CreateConnectionAsync();
    var channel = await connection.CreateChannelAsync();

    await channel.QueueDeclareAsync(queue: "Files", durable: false, exclusive: false, autoDelete: false,
        arguments: null);
    */

    
    
    
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
            FilesModel readFile = new FilesModel
            {
                fileContents = content
            };
            _bus.PubSub.PublishAsync(readFile);
            //channel.BasicPublishAsync(exchange: string.Empty, routingKey: "Files", body: content);
        });

        result[folder] = fileContents;
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
