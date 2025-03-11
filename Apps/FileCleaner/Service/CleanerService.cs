using Service.Interfaces;

namespace Service;

public class CleanerService
{
    private ICleaner<string> _cleaner;

    public CleanerService(ICleaner<string> cleaner)
    {
        _cleaner = cleaner;
    }

    public async Task start()
    {
        string msg = getMessage();
        var msg2 = await _cleaner.Clean(msg);
        Console.WriteLine(msg2);

    }

    private string getMessage()
    {
        return "";
    }
}