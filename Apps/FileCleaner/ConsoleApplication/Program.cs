using System.Diagnostics;
using DefaultNamespace;
using EasyNetQ;
using Events;
using Events.EventModels;
using Logger;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Polly;
using Serilog;
using Service;
using Service.Implementations;
using Service.Implementations.converter;

public static class Program
{
    
    public static async Task Main()
    {
        CleanerStringSpacing cleaner = new CleanerStringSpacing();
        ByteArrayConverter converter = new ByteArrayConverter();

        CleanerService cs = new CleanerService(cleaner, converter);
        var policy = Policy.Handle<Exception>().CircuitBreakerAsync(3, TimeSpan.FromSeconds(60),
            onBreak: (exception, TimeSpan) =>
            {
                Log.Logger.Warning("Circuit breaker in indexer is on brake");
            }, onReset: () => {Log.Logger.Information("Circuit breaker in indexer is reset");},
            onHalfOpen: () => {Log.Logger.Information("Circuit breaker in indexer is half open");});
        var connectionEstablished = false;
        policy.ExecuteAsync(async () =>
        {
            using var bus = RabbitMqConnectionHelper.GetRabbitMQConnection();
            while (!connectionEstablished)
            {
               var subscriptionResult = bus.PubSub.SubscribeAsync<RawEvent>("Files", e =>
                {
                    var propagator = new TraceContextPropagator();
                    var parentContext = propagator.Extract(default, e, (msg, key) =>
                    {
                        return new List<string>(new[]
                            { msg.Headers.ContainsKey(key) ? msg.Headers[key].ToString() : string.Empty });
                    });
                    Baggage.Current = parentContext.Baggage;
                    using var activity = Monitoring.ActivitySource.StartActivity("Message Received", ActivityKind.Consumer, parentContext.ActivityContext);
                    
                    using var cleanActivity = Monitoring.ActivitySource.StartActivity();
                    var convertfrombytes = converter.From(e.RawMessage);
                    var cleanedBytes = cs.Clean(convertfrombytes.Result);
                    var cleanEvent = new CleanedEvent
                    {
                        CleanMessage = cleanedBytes.Result,
                    };
                    var activityContext = activity?.Context ?? Activity.Current?.Context ?? default;
                    var propagationContext = new PropagationContext(activityContext, Baggage.Current);
                    propagator.Inject(propagationContext, cleanEvent, (r, key, value) =>
                    {
                        r.Headers.Add(key, value);
                    });
                    bus.PubSub.PublishAsync(cleanEvent);

                }).AsTask();
                
                await subscriptionResult.WaitAsync(CancellationToken.None);
                connectionEstablished = subscriptionResult.Status == TaskStatus.RanToCompletion;
                if (!connectionEstablished) Thread.Sleep(1000);
            }
        });

    }
}



    
