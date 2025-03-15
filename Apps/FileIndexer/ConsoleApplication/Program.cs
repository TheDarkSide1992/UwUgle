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

   /**
   * Handles the getting messages from mq
   */
    public static async Task handleMessages()
    {
        var service = new IndexService();
        var connectionEstablished = false;
        using var bus = RabbitMqConnectionHelper.GetRabbitMQConnection();
        while (!connectionEstablished)
        {
           // uses the policy variable to isolate the service from faults in the mq
            policy.ExecuteAsync(async () =>
            {
                     // subscribes to a topic on the mq
                    var subscriptionResult = bus.PubSub.SubscribeAsync<CleanedEvent>("Cleaned", async e =>
                    {
                       // setting up the tracing to make destributed tracing possible
                        var propagator = new TraceContextPropagator();
                        var parentContext = propagator.Extract(default, e, (msg, key) =>
                        {
                            return new List<string>(new[]
                                { msg.Headers.ContainsKey(key) ? msg.Headers[key].ToString() : string.Empty });
                        });
                        Baggage.Current = parentContext.Baggage;
                        using var activity = Monitoring.ActivitySource.StartActivity("Message Received",
                            ActivityKind.Consumer, parentContext.ActivityContext);

                       // starts an index activity
                        using var indexActivity = Monitoring.ActivitySource.StartActivity();
                        
                        var indexStatus = await service.Index(e.CleanMessage); // returns true if index was sucessful false if index failed
                        if (indexStatus)
                        {
                          Log.Logger.Information("Index of file completed");   
                        }
                        else
                        {
                            Log.Logger.Error("Index of file failed");
                        }
                        
                    }).AsTask();
                  // awaits the result of the subscriptionResult variable and puts the thread to sleep for a secound
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
