namespace OA.Infrastructure.MessageSettings
{
    public static class RabbitMqDefaults
    {
        public static string QueueName { get; set; } = "myQueue";
        public static string ExchangeName { get; set; } = "myExchange";
        public static string RoutingKey { get; set; } = "myRoutingKey";
    }
}
