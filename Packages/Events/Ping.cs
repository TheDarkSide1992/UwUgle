namespace Events;

public class Ping
{
    public Dictionary<string, Object> Header { get; set; } = new();
    public string PingMessage { get; set; }
}
