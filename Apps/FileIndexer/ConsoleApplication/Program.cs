// See https://aka.ms/new-console-template for more information

using System.Text;
using EasyNetQ;
using Events;
using Logger;
using Polly;
using Polly.CircuitBreaker;
using Serilog;
using Service;

public class Program
{
    public static async Task Main()
    {
        AsyncCircuitBreakerPolicy policy = Policy.Handle<Exception>().CircuitBreakerAsync(3, TimeSpan.FromSeconds(60),
            onBreak: (exception, TimeSpan) =>
            {
                Log.Logger.Warning("Circuit breaker in indexer is on brake");
            }, onReset: () => {Log.Logger.Information("Circuit breaker in indexer is reset");},
            onHalfOpen: () => {Log.Logger.Information("Circuit breaker in indexer is half open");});
        policy.ExecuteAsync(async () =>
        {

        });
    }
}