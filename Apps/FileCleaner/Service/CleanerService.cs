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
    public async Task Start()
    {
        string msg = await GetMessage();
        var msg2 = await _cleaner.Clean(msg);
        Console.WriteLine(msg2);
        
        SendBytes(msg2);
    }

    /**
     * Gets the message and returns as a string
     */
    private async Task<string> GetMessage()
    {
        //TODO INsert rabitmq result here
        //var str = await _converter.From(message);
        
        return "";
    }

    /**
     * sends the message returns void
     */
    private async void SendBytes(string message)
    {
        //TODO Send over rabit MQ
        var bytearr = await _converter.To(message);
    }
}