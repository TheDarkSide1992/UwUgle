// See https://aka.ms/new-console-template for more information

using System.Text;
using Logger;
using Service;

public class Program
{
    public static async Task Main()
    {
        var connectionEstablished = false;
        Thread.Sleep(5000);
        using var bus = ConnectionHelper.GetRabbitMQConnection();
        while (!connectionEstablished)
        {
            
        }
    }
}