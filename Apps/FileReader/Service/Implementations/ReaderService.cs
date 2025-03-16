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
using Serilog;
using Service.Interfaces;

namespace Service.Implementations;

public class ReaderService : IService
{
    
    private readonly IReader _reader;
    
    private readonly IBus _bus;
    private readonly string _queueName = "Files";
    
    public ReaderService(Reader reader)
    {
        
        _reader = reader;
        _bus = RabbitMqConnectionHelper.GetRabbitMQConnection();
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
                //Console.WriteLine($"Processing folder: {folder}");
                string[] files = _reader.GetFilesPathFromFolder(folder);

                await Parallel.ForEachAsync(files, new ParallelOptions { MaxDegreeOfParallelism = 100 },
                    async (filePath, _) =>
                    {
                        try
                        {
                            var request = new RawEvent { RawMessage = _reader.ReadFileAsByteArray(filePath) };
                            await PubByteArrayAsync(request);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to process file {filePath}: {ex.Message}");
                        }
                    });
            }
            Thread.Sleep(1500);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }
    
    private async Task PubByteArrayAsync(RawEvent content)
    {
        MessageProperties properties = new MessageProperties { DeliveryMode = 2 };
        byte[] body = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(content);

        try
        {
            await _bus.Advanced.PublishAsync(Exchange.Default, _queueName, true, properties, body);
            Console.WriteLine("Publish successful.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Publish failed: {ex.Message}");
        }
    }
    
    
}
