using System.Collections.Concurrent;
using System.Diagnostics;
using DefaultNamespace;
using EasyNetQ;
using EasyNetQ.Topology;
using Events;
using Infrastructure.Implementations;
using Infrastructure.Interfaces;
using Logger;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Polly;
using Serilog;
using Service.Interfaces;

namespace Service.Implementations;

public class ReaderService : IService
{
    
    private readonly IReader _reader;
    
    private readonly IBus _bus;
    private readonly string _queueName = "FilesV14";
    
    
    // Create a thread-safe queue
    private ConcurrentQueue<byte[]> _byteArrayQueue = new ConcurrentQueue<byte[]>();
    
    
    private readonly string connectionString =
        "host=localhost;username=guest;password=guest;timeout=5;publisherConfirms=true;prefetchcount=50;persistentMessages=true;connectIntervalAttempt=5";
    public ReaderService(Reader reader)
    {
        
        _reader = reader;
        _bus = RabbitHutch.CreateBus(connectionString);
        _bus.Advanced.QueueDeclareAsync(name: _queueName).ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                Console.WriteLine("Declared Queue");
            }
            else
            {
                Console.WriteLine("Failed to Declare Queue");
            }
            if (task.IsFaulted)
            {
                Console.WriteLine(task.Exception);
            }
        });
    }
    
    
    public async Task ReadFoldersSequentiallyWithParallelFilesAsBytes(string rootFolderPath)
    {
        try
        {
            using var activity = Monitoring.ActivitySource.StartActivity();
            Log.Logger.Here().Debug($@"Attempting to retrieve files from {rootFolderPath}");

            var request = new RawEvent();
        
            var activityContext = activity?.Context ?? Activity.Current?.Context ?? default;
            var propagationContext = new PropagationContext(activityContext, Baggage.Current);
            var propagator = new TraceContextPropagator();
            propagator.Inject(propagationContext, request, (r, key, value) =>
            {
                r.Headers.Add(key, value);
            });
        
        
            string[] allFolders = _reader.GetFoldersPath(rootFolderPath);
        
            foreach (var folder in allFolders) 
            { 
                Console.WriteLine($"Processing folder: {folder}");
                string[] files = _reader.GetFilesPathFromFolder(folder);

                
                
                    Parallel.ForEach(files, new ParallelOptions
                    {
                        MaxDegreeOfParallelism = 100,
                    }, filePath =>
                    {
                        request.RawMessage = _reader.ReadFileAsByteArray(filePath);
                        PubByteArray(request);
                    });
                
                
                //checks if any messages failed to get publish and handles them by infinitely try to send them until they are sent
                //handleFailedMessages();
                
            }
            Thread.Sleep(1500);
            Console.WriteLine($"Ammount of failed files: {_byteArrayQueue.Count}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }


    private void PubByteArray(RawEvent content)
    {
        var policy = Policy.Handle<Exception>().CircuitBreakerAsync(3, TimeSpan.FromSeconds(60),
            onBreak: (exception, TimeSpan) =>
            {
                Log.Logger.Warning("Circuit breaker in FileReader is on break");
            }, onReset: () => {Log.Logger.Information("Circuit breaker in FileReader is reset");},
            onHalfOpen: () => {Log.Logger.Information("Circuit breaker in FileReader is half open");});
        
        
        //policy.ExecuteAsync(async () =>
        //{
            MessageProperties properties = new MessageProperties
            {
                DeliveryMode = 2
            }; // Persistent message
            byte[] body = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(content);
            _bus.Advanced.PublishAsync(Exchange.Default, _queueName, true, properties, body).ContinueWith(task =>
            {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    Console.WriteLine("Publish completed fine.");
                }
                else
                {
                    AddByteArray(body);
                    Console.WriteLine("Publish failed.");
                }
                if (task.IsFaulted)
                {
                    Console.WriteLine(task.Exception);
                }
            });
        //});
        
    }
    
    
    private void BackUpPubByteArray(Byte[] content)
    {
        MessageProperties properties = new MessageProperties
        {
            DeliveryMode = 2
        }; // Persistent message
        _bus.Advanced.PublishAsync(Exchange.Default, _queueName, true, properties, content).ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                TryGetByteArray(out content);
                Console.WriteLine("Publish failed message, completed fine.");
            }
            else
            {
                Console.WriteLine("Publish failed, to publish failed message.");
            }
        });
    }


    /*private void handleFailedMessages()
    {
       
        while (.Count > 0)
        {
            resolvedFailedMessages = new List<byte[]>();
            foreach (var failedFile in failedMessages)
            {
                BackUpPubByteArray(failedFile);
            }

            if (failedMessages.Count > 0)
            {
                foreach (var sentFile in resolvedFailedMessages)
                {
                        failedMessages.Remove(sentFile);
                }
            }
                    
        }
    }*/
    
    
    

    // Add a byte array (Thread-Safe)
    public void AddByteArray(byte[] data)
    {
        _byteArrayQueue.Enqueue(data);
    }

    // Retrieve and remove a byte array (Thread-Safe)
    public bool TryGetByteArray(out byte[] data)
    {
        return _byteArrayQueue.TryDequeue(out data);
    }
    
}
