using System.Diagnostics;
using Events.EventModels;
using Logger;
using Serilog;
using Service.Interfaces;

namespace Service;

public class CleanerService
{
    private readonly ICleaner<string> _cleaner;
    private readonly IConverter<Byte[], string> _converter;

    public CleanerService(ICleaner<string> cleaner, IConverter<Byte[],string> converter)
    {
        _cleaner = cleaner;
        _converter = converter;
    }

    /**
     * starts the proces to clean messages
     */
    public async Task<Byte[]> Clean(string text)
    {
        using var activity = Monitoring.ActivitySource.StartActivity();
        try
        {
            var cleantext = _cleaner.Clean(text).Result;
            var cleanedTextAsBytes = _converter.To(cleantext);
            return await cleanedTextAsBytes;
        }
        catch (Exception e)
        {
            Log.Logger.Error("Error while cleaning file");
        }

        return null;
    }

}