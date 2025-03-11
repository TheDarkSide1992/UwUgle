using EasyNetQ;

namespace Logger;

public static class ConnectionHelper
{
    public static IBus GetRabbitMQConnection()
    {
        return RabbitHutch.CreateBus(Environment.GetEnvironmentVariable("rabbitmq_connection"));
    }
}