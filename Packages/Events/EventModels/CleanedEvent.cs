namespace Events;

public class CleanedEvent : BaseEventModel
{
    public byte[] CleanMessage { get; set; }
}