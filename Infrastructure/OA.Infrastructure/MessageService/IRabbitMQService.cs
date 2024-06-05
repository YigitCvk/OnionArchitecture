namespace OA.Infrastructure.MessageService
{
    public interface IRabbitMQService
    {
        void SendMessage(string message);
        void SendMessage(string exchangeName, string routingKey, byte[] body);
        void Dispose();
    }
}
