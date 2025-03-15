using Service.Interfaces;

namespace Service.Implementations;

public class CleanerString : ICleaner<string>
{
    /**
     * Cleans message by finding X- lines
     */
    public async Task<string> Clean(string message)
    {
        if (message.Trim() == "") return "";

        message = message.Split("X-").Last();
        int endHeader = message.IndexOf("\n", StringComparison.Ordinal);
        
        message = message.Remove(0, endHeader);

        return message.TrimStart();
    }
}