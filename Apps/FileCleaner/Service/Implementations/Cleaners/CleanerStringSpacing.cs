using Logger;
using Serilog;
using Service.Interfaces;

namespace Service.Implementations;

public class CleanerStringSpacing: ICleaner<string>
{
    /**
     * Cleans messages by finding spacing after header
     */
    public async Task<string> Clean(string message)
    {
        using var activity = Monitoring.ActivitySource.StartActivity();
        try
        {
            if (message.Trim() == "") return "";

            message = message.TrimStart();
            int endHeader = message.IndexOf("\n\n", StringComparison.Ordinal);

            message = message.Remove(0, endHeader);

            return message.TrimStart();
        }
        catch (Exception ex)
        {
         Log.Logger.Error("Error while cleaning file");
        }
        return null;
    }
}