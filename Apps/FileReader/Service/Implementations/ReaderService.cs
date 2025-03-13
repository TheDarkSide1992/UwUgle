using System.Collections.Concurrent;
using EasyNetQ;
using EasyNetQ.Topology;
using Infrastructure;
using Infrastructure.Implementations;
using Infrastructure.Interfaces;
using Service.Interfaces;

namespace Service.Implementations;

public class ReaderService : IService
{
    
    private readonly IReader _reader;
    
    private readonly IBus _bus;
    private readonly string _queueName = "FilesV6";
    /*consider testing prefetchcount a bit to see if we can speed file transfer up a bit,
     50 is default. Higher values gives better performance but requires more memory
     */
    //TODO put into environmental secret when done testing
    private readonly string connectionString =
        "host=localhost;username=guest;password=guest;timeout=30;publisherConfirms=true;prefetchcount=50;persistentMessages=true;connectIntervalAttempt=5";

    public ReaderService(Reader reader)
    {
        _reader = reader;
        _bus = RabbitHutch.CreateBus(connectionString);
        _bus.Advanced.QueueDeclareAsync(name: _queueName);
    }
    
    
    public async Task ReadFoldersSequentiallyWithParallelFilesAsBytes(string rootFolderPath)
    {
        string[] allFolders = _reader.GetFoldersPath(rootFolderPath);
        
        for (int i = 0; i < allFolders.Length; i++) 
        {
            //Console.WriteLine($"Processing folder: {allFolders[i]}");
        
            string[] files = _reader.GetFilesPathFromFolder(allFolders[i]);

            Parallel.ForEach(files, new ParallelOptions
            {
                MaxDegreeOfParallelism = 100,
            }, filePath =>
            {
                PubByteArray(_reader.ReadFileAsByteArray(filePath));
            });
        }
    }


    private void PubByteArray(byte[] content)
    {
        MessageProperties properties = new MessageProperties { DeliveryMode = 2 }; // Persistent message
        _bus.Advanced.PublishAsync(Exchange.Default, _queueName, false, properties, content);
    }
    
}
