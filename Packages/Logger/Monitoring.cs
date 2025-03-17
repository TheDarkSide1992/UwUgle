using System.Diagnostics;
using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;

namespace Logger;

public static class Monitoring
{
    public static readonly ActivitySource ActivitySource = new("UwUgle", "1.0.0");
    private static TracerProvider _tracerProvider;

    static Monitoring()
    {
        // Configure tracing
        var serviceName = Assembly.GetExecutingAssembly().GetName().Name;
        var version = "1.0.0";
        var zipkinconn = "http://zipkin:9411/api/v2/spans";

        _tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddZipkinExporter(configure: options =>
            {
                options.Endpoint = new Uri(zipkinconn);
            })
            .AddConsoleExporter()
            .AddSource(ActivitySource.Name)
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: serviceName, serviceVersion: version))
            .Build();
        
        // Configure logging
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.WithSpan()
            .WriteTo.Seq("http://seq:5341")
            .WriteTo.Console()
            .CreateLogger();
    }
}