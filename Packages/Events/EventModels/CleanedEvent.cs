namespace Events.EventModels;

public class CleanedEvent : BaseEventModel
{
    public byte[] CleanMessage { get; set; }
}