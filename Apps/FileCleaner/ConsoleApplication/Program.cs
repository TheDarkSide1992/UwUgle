using System.Diagnostics;
using DefaultNamespace;
using EasyNetQ;
using Events;
using Events.EventModels;
using Logger;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Serilog;
using Service;
using Service.Implementations;
using Service.Implementations.converter;

public static class Program
{
   
    
    private static AsyncCircuitBreakerPolicy policy = Policy.Handle<Exception>().CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
        onBreak: (exception, TimeSpan) =>
        {
            using var activity = Monitoring.ActivitySource.StartActivity();
            Log.Logger.Warning("Circuit breaker in Cleaner brake");
        },  onReset: async () =>
        {
            using var activity = Monitoring.ActivitySource.StartActivity();
            Log.Logger.Information("Circuit breaker in Cleaner is reset");
            await Main();
        },
        onHalfOpen: () =>
        {
            using var activity = Monitoring.ActivitySource.StartActivity();
            Log.Logger.Information("Circuit breaker in Cleaner is half open");
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
        CleanerStringSpacing cleaner = new CleanerStringSpacing();
        ByteArrayConverter converter = new ByteArrayConverter();

        CleanerService cs = new CleanerService(cleaner, converter);

        var connectionEstablished = false;
        using var bus = RabbitMqConnectionHelper.GetRabbitMQConnection();
        while (!connectionEstablished)
        {
            policy.ExecuteAsync(async () =>
            {
                {
                    var subscriptionResult = bus.PubSub.SubscribeAsync<RawEvent>("Files", async e =>
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

                        using var cleanActivity = Monitoring.ActivitySource.StartActivity();
                        var convertfrombytes = await converter.From(e.RawMessage);
                        var cleanedBytes = await cs.Clean(convertfrombytes);
                        var cleanEvent = new CleanedEvent
                        {
                            CleanMessage = cleanedBytes,
                        };
                        var activityContext = activity?.Context ?? Activity.Current?.Context ?? default;
                        var propagationContext = new PropagationContext(activityContext, Baggage.Current);
                        propagator.Inject(propagationContext, cleanEvent,
                            (r, key, value) => { r.Headers.Add(key, value); });
                        bus.PubSub.PublishAsync(cleanEvent);

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



    
