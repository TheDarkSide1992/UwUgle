using Events;
namespace DefaultNamespace;

public class RawEvent : BaseEventModel
{
    public byte[] RawMessage { get; set; }
}