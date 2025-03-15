using System.Diagnostics;
using EasyNetQ;
using Events;
using Events.EventModels;
using Logger;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Polly;
using Polly.CircuitBreaker;
using Serilog;
using Service;

public class Program
{
   private static AsyncCircuitBreakerPolicy policy = Policy.Handle<Exception>().CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
        onBreak: (exception, TimeSpan) =>
        {
            using var activity = Monitoring.ActivitySource.StartActivity();
            Log.Logger.Warning("Circuit breaker in Indexer brake");
        },  onReset: async () =>
        {
            using var activity = Monitoring.ActivitySource.StartActivity();
            Log.Logger.Information("Circuit breaker in Indexer is reset");
            await Main();
        },
        onHalfOpen: () =>
        {
            using var activity = Monitoring.ActivitySource.StartActivity();
            Log.Logger.Information("Circuit breaker in Indexer is half open");
        });
    
    public static async Task Main()
    {
        while (true)
        {
            await handleMessages();
            Thread.Sleep(1000);
        }
    }


    public static async Task handleMessages()
    {
        var service = new IndexService();
        var connectionEstablished = false;
        using var bus = RabbitMqConnectionHelper.GetRabbitMQConnection();
        while (!connectionEstablished)
        {
            policy.ExecuteAsync(async () =>
            {
                {
                    var subscriptionResult = bus.PubSub.SubscribeAsync<CleanedEvent>("Cleaned", async e =>
                    {
                        var propagator = new TraceContextPropagator();
                        var parentContext = propagator.Extract(default, e, (msg, key) =>
                        {
                            return new List<string>(new[]
                                { msg.Headers.ContainsKey(key) ? msg.Headers[key].ToString() : string.Empty });
                        });
                        Baggage.Current = parentContext.Baggage;
                        using var activity = Monitoring.ActivitySource.StartActivity("Message Received",
                            ActivityKind.Consumer, parentContext.ActivityContext);
                        
                        using var indexActivity = Monitoring.ActivitySource.StartActivity();
                        
                        var indexStatus = await service.Index(e.CleanMessage);
                        if (indexStatus)
                        {
                          Log.Logger.Information("Index of file completed");   
                        }
                        else
                        {
                            Log.Logger.Error("Index of file failed");
                        }
                        
                    }).AsTask();

                    await subscriptionResult.WaitAsync(CancellationToken.None);
                    connectionEstablished = subscriptionResult.Status == TaskStatus.RanToCompletion;
                    if (!connectionEstablished) Thread.Sleep(1000);
                }
                var wait = true;
                while (wait)
                {
                    Thread.Sleep(1000);
                    wait = false;
                }
            });
        }
    }
}