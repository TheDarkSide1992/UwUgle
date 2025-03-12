using EasyNetQ;

namespace Events;

public class RabbitMqConnectionHelper
{
    /**
     * Returns a RabbitMq bus if it can gain connection
     */
    public static IBus GetRabbitMQConnection()
    {
        return RabbitHutch.CreateBus(Environment.GetEnvironmentVariable("rabbitmq_connection"));
    }
}