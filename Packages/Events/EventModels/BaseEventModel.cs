namespace Events;

public abstract class BaseEventModel
{
    public Dictionary<string, Object> Headers { get; set; } = new();
}