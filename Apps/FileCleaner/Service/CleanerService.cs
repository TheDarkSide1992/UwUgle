using Events.EventModels;
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
    public async Task<string> GetMessage()
    {
        //TODO INsert rabitmq result here
        //var str = await _converter.From(message);
        
        return "";
    }

    /**
     * sends the message returns void
     */
    public async Task<CleanedEvent> SendBytes(string message)
    {
        var bytearr = await _converter.To(message);
        return new CleanedEvent { CleanMessage = bytearr};
    }
}